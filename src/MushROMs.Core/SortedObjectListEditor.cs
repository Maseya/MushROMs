// <copyright file="SortedObjectListEditor.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.Core;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Maseya.Smas.Smb1.AreaData.ObjectData;

/// <remarks>
/// TLDR: There are fancy structures I could use, but the objects are displayed
/// in text as an array, so I'm stuck with using a simple array.
///
/// I spent a lot of time wondering which data structure should house the object
/// data. The data must remain sorted, so I actually wrote a whole red-black
/// tree structure for this purpose. However, "sorting" this object data is
/// actually quite a vague task. Data can come in any order, and the X
/// coordinates _can_ go backwards, but then it's not rendered right. For
/// practical purposes, let's say that X is strictly increasing (this will stop
/// being true when I rewrite the level engine). Then yes, X is a good sorting
/// key, but multiple objects can exists on the same X.
///
/// In this scenario, we need a subsort. At first, I thought sorting by Y would
/// be okay, but multiple objects can even exist on the same Y, and there are
/// scenarios in vanilla smb1 that would break if we tried to order Y, so this
/// is out. This leaves us with creating a Z buffer. Objects on the same X are
/// subsorted by a Z buffer which is defined as the order they appear in the
/// object data. Simple enough.
///
/// But now the question becomes whether the Z buffer should be an explicit
/// variable, or implicitly defined from its index. If I were to do a red-black
/// tree, then Z would need to be a variable. The red-black tree starts to fall
/// apart now, because if I want to move objects around, then I'll need to edit
/// all of the Z variables on the X. It quickly gets messy, and is O(n logn) in
/// worst case (not that runtime is crucial on such a small scale as a SNES
/// game).
///
/// I then considered bucket sorting. An smb1 level is 0x200 tiles wide at max,
/// so I could 0x200 array of sorted object lists. Each object list would be
/// very small (5 or less elements), so sorting them is easy, and moving objects
/// around is very easy as I just index to the X coordinate. Iteration would be
/// a chore though, as I need to go through all 0x200 buckets (which isn't bad
/// really). But getting the index of an object would be O(n). Would I ever need
/// the index here though? There's going to be a UI array, which is my concern.
///
/// The objects will be visible in text window as an array of strings. Whatever
/// data structure I use will need to work nicely with this. For example, if a
/// user clicks on an object in the graphic window, then I need to find that
/// object in the bucket, which should be O(1) since I know the X coordinate.
/// And I also need the object in the UI window, is O(log n) since the UI array
/// will always be sorted. Adding and removing objects would be O(1) in the
/// bucket, but still O(n) in the UI array, so it's O(n) overall.
///
/// End of the day, what's the point of having an optimized data structure, if
/// it's still dominated by a less optimized structure that I need to use
/// </remarks>
public class SortedObjectListEditor :
    IList<UIAreaObjectCommand>,
    IReadOnlyList<UIAreaObjectCommand>
{
    public SortedObjectListEditor()
    {
        Items = [];
    }

    public SortedObjectListEditor(IEnumerable<AreaObjectCommand> commands)
    {
        Items = [.. GetSortedUICommands(commands)];
    }

    public event EventHandler? DataReset;

    public event EventHandler<ObjectEditedEventArgs>? ItemEdited;

    public event EventHandler<ItemAddedEventArgs>? ItemAdded;

    public event EventHandler<ItemAddedEventArgs>? ItemRemoved;

    public event EventHandler? DataCleared;

    public int Count
    {
        get
        {
            return Items.Count;
        }
    }

    public UIAreaObjectCommand this[int index]
    {
        get
        {
            return Items[index];
        }
        set
        {
            _ = Edit(index, value);
        }
    }

    bool ICollection<UIAreaObjectCommand>.IsReadOnly
    {
        get
        {
            return false;
        }
    }

    private List<UIAreaObjectCommand> Items
    {
        get;
    }

    public static IEnumerable<AreaObjectCommand> GetAreaData(
        IEnumerable<byte> bytes)
    {
        using var en = bytes.GetEnumerator();
        if (!en.MoveNext())
        {
            throw new ArgumentException("Area object data ended early.");
        }

        while (en.Current != AreaObjectCommand.TerminationCode)
        {
            if (AreaObjectCommand.IsThreeByteSpecifier(en.Current))
            {
                var list = new List<byte>(GetBytes(3));
                yield return new AreaObjectCommand(
                    list[0],
                    list[1],
                    list[2]);
            }
            else
            {
                var list = new List<byte>(GetBytes(2));
                yield return new AreaObjectCommand(
                    list[0],
                    list[1]);
            }
        }

        IEnumerable<byte> GetBytes(int size)
        {
            for (var i = 0; i < size; i++)
            {
                yield return en.Current;

                if (!en.MoveNext())
                {
                    throw new ArgumentException("Area object data ended early.");
                }
            }
        }
    }

    public void Reset(IEnumerable<AreaObjectCommand> commands)
    {
        Reset(GetUICommands(commands));
    }

    public void Reset(IEnumerable<UIAreaObjectCommand> commands)
    {
        Items.Clear();
        foreach (var item in commands.OrderBy(x => x.X))
        {
            if (item.Command.ObjectType == ObjectType.PageSkip)
            {
                continue;
            }

            Items.Add(item);
        }

        DataReset?.Invoke(this, EventArgs.Empty);
    }

    public bool Contains(UIAreaObjectCommand command)
    {
        return IndexOf(command) >= 0;
    }

    public int IndexOf(UIAreaObjectCommand item)
    {
        var result = SearchFirstIndexOf(item);
        for (; result < Count; result++)
        {
            if (item == Items[result])
            {
                return result;
            }
        }

        return -1;
    }

    public int AtCoords(int x, int y)
    {
        if (Items.Count == 0)
        {
            return -1;
        }

        var command = Items[0];
        command.X = x;
        var index = SearchLastIndexOf(command);
        for (; --index >= 0 && Items[index].X == x;)
        {
            if (Items[index].Y == y)
            {
                return index;
            }
        }

        return -1;
    }

    public int Add(AreaObjectCommand command, int page)
    {
        return Add(new UIAreaObjectCommand(command, page));
    }

    public int Add(UIAreaObjectCommand command)
    {
        var index = AddInternal(command);
        ItemAdded?.Invoke(this, new ItemAddedEventArgs(index));
        return index;
    }

    void ICollection<UIAreaObjectCommand>.Add(UIAreaObjectCommand command)
    {
        _ = Add(command);
    }

    public bool Remove(UIAreaObjectCommand item)
    {
        var index = IndexOf(item);
        if (index != -1)
        {
            RemoveAt(index);
            return true;
        }

        return false;
    }

    public void RemoveAt(int index)
    {
        Items.RemoveAt(index);
        ItemRemoved?.Invoke(this, new ItemAddedEventArgs(index));
    }

    public int Edit(int index, UIAreaObjectCommand newItem)
    {
        if (Items[index] == newItem)
        {
            return index;
        }

        var oldItem = Items[index];
        Items.RemoveAt(index);
        var newIndex = AddInternal(newItem);

        ItemEdited?.Invoke(
            this,
            new ObjectEditedEventArgs(index, newIndex, oldItem, newItem));
        return newIndex;
    }

    public void Clear()
    {
        Items.Clear();
        DataCleared?.Invoke(this, EventArgs.Empty);
    }

    void IList<UIAreaObjectCommand>.Insert(
        int index,
        MushROMs.Core.UIAreaObjectCommand item)
    {
        _ = Add(item);
    }

    public IEnumerator<UIAreaObjectCommand> GetEnumerator()
    {
        return Items.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerable<AreaObjectCommand> GetObjectData()
    {
        var lastPage = 0;
        foreach (var command in Items)
        {
            var page = command.Page;
            var item = command.Command;
            if (page <= lastPage + 1)
            {
                Debug.Assert(page >= lastPage);

                item.PageFlag = page != lastPage;
            }
            else
            {
                yield return new AreaObjectCommand(0x0D, (byte)(page & 0x1F));
                item.PageFlag = false;
            }

            lastPage = page;
            yield return item;
        }
    }

    public byte[] ToByteArray()
    {
        var result = new List<byte>();
        foreach (var command in GetObjectData())
        {
            result.Add(command.Value1);
            result.Add(command.Value2);
            if (command.IsThreeByteCommand)
            {
                result.Add(command.Value3);
            }
        }

        result.Add(AreaObjectCommand.TerminationCode);

        return [.. result];
    }

    void ICollection<UIAreaObjectCommand>.CopyTo(
        UIAreaObjectCommand[] array,
        int index)
    {
        if ((uint)index >= (uint)array.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        if (index + Count < array.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        foreach ((var i, var value) in Enumerable.Range(0, Count).Zip(this))
        {
            array[index + i] = value;
        }
    }

    private static IEnumerable<UIAreaObjectCommand> GetUICommands(
        IEnumerable<AreaObjectCommand> data)
    {
        var page = 0;
        foreach (var item in data)
        {
            if (item.PageFlag)
            {
                page++;
            }
            else if (item.ObjectType == ObjectType.PageSkip)
            {
                page = item.PrimaryCommand & 0x1F;
                continue;
            }

            yield return new UIAreaObjectCommand(item, page);
        }
    }

    private static IEnumerable<UIAreaObjectCommand> GetSortedUICommands(
        IEnumerable<AreaObjectCommand> data)
    {
        return GetUICommands(data).OrderBy(item => item.X);
    }

    private int AddInternal(UIAreaObjectCommand item)
    {
        if (item.Command.ObjectType == ObjectType.PageSkip)
        {
            return -1;
        }

        var index = SearchLastIndexOf(item);
        Items.Insert(index, item);
        return index;
    }

    private int SearchFirstIndexOf(UIAreaObjectCommand item)
    {
        var comparer = Comparer<UIAreaObjectCommand>.Create(
            (x, y) => x.X < y.X ? -1 : +1);
        return ~Items.BinarySearch(item, comparer);
    }

    private int SearchLastIndexOf(UIAreaObjectCommand item)
    {
        var comparer = Comparer<UIAreaObjectCommand>.Create(
            (x, y) => x.X <= y.X ? -1 : +1);
        return ~Items.BinarySearch(item, comparer);
    }
}
