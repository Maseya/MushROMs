// <copyright file="ObjectListForm.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.Win.Dialogs.BaseForms;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

using MushROMs.Core;

using Maseya.Smas.Smb1;
using Maseya.Smas.Smb1.AreaData.ObjectData;

public sealed partial class ObjectListForm : Form
{
    private AreaPlatformType _areaPlatformType;

    public ObjectListForm()
    {
        InitializeComponent();

        Items = new ItemCollection(this);
    }

    public event EventHandler? AreaPlatformTypeChanged;

    public event EventHandler? SelectedIndexChanged;

    public event EventHandler? EditItem;

    public event EventHandler? AddItem_Click;

    public event EventHandler? DeleteItem_Click;

    public event EventHandler? ClearItems_Click;

    public event EventHandler? MoveItemDown_Click;

    public event EventHandler? MoveItemUp_Click;

    public AreaPlatformType AreaPlatformType
    {
        get
        {
            return _areaPlatformType;
        }

        set
        {
            if (_areaPlatformType == value)
            {
                return;
            }

            _areaPlatformType = value;
            OnAreaPlatformTypeChanged(EventArgs.Empty);
        }
    }

    public int SelectedIndex
    {
        get
        {
            return lvwObjects.SelectedIndices.Count > 0
                ? lvwObjects.SelectedIndices[0]
                : -1;
        }

        set
        {
            if ((uint)value < (uint)lvwObjects.Items.Count)
            {
                lvwObjects.Items[value].Selected = true;
            }
            else
            {
                lvwObjects.SelectedIndices.Clear();
            }
        }
    }

    public ItemCollection Items
    {
        get;
    }

    private void UpdateAreaPlatformDescriptions()
    {
        for (var i = 0; i < Items.Count; i++)
        {
            if (Items[i].Command.ObjectType != ObjectType.AreaSpecificPlatform)
            {
                continue;
            }

            // TODO(swr): Rewrites the description. It's totally hacky
            Items[i] = Items[i];
        }
    }

    private void OnAreaPlatformTypeChanged(EventArgs e)
    {
        UpdateAreaPlatformDescriptions();
        AreaPlatformTypeChanged?.Invoke(this, e);
    }

    private void OnSelectedIndexChanged(EventArgs e)
    {
        SelectedIndexChanged?.Invoke(this, e);
    }

    private void OnEditItem(EventArgs e)
    {
        EditItem?.Invoke(this, e);
    }

    private void Add_Click(object? sender, EventArgs e)
    {
        AddItem_Click?.Invoke(this, EventArgs.Empty);
    }

    private void Delete_Click(object? sender, EventArgs e)
    {
        DeleteItem_Click?.Invoke(this, EventArgs.Empty);
    }

    private void Clear_Click(object? sender, EventArgs e)
    {
        ClearItems_Click?.Invoke(this, EventArgs.Empty);
    }

    private void MoveDown_Click(object? sender, EventArgs e)
    {
        MoveItemDown_Click?.Invoke(this, EventArgs.Empty);
    }

    private void MoveUp_Click(object? sender, EventArgs e)
    {
        MoveItemUp_Click?.Invoke(this, EventArgs.Empty);
    }

    private void Objects_MouseDoubleClick(object? sender, MouseEventArgs e)
    {
        if (lvwObjects.SelectedItems.Count != 1)
        {
            return;
        }

        OnEditItem(EventArgs.Empty);
    }

    private void Objects_SelectedIndexChanged(object? sender, EventArgs e)
    {
        OnSelectedIndexChanged(EventArgs.Empty);
    }

    private void Objects_ItemSelectionChanged(
        object? sender,
        ListViewItemSelectionChangedEventArgs e)
    {
        OnSelectedIndexChanged(EventArgs.Empty);
    }

    public class ItemCollection : IList<UIAreaObjectCommand>
    {
        public ItemCollection(ObjectListForm owner)
        {
            Owner = owner;
        }

        public int Count
        {
            get
            {
                return BaseItems.Count;
            }
        }

        bool ICollection<UIAreaObjectCommand>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        private ObjectListForm Owner
        {
            get;
        }

        private ListView.ListViewItemCollection BaseItems
        {
            get
            {
                return Owner.lvwObjects.Items;
            }
        }

        public UIAreaObjectCommand this[int index]
        {
            get
            {
                return (UIAreaObjectCommand)BaseItems[index].Tag;
            }

            set
            {
                BaseItems[index].Tag = value;
                BaseItems[index].SubItems[0].Text = value.Command.HexString;
                BaseItems[index].SubItems[1].Text = $"{value.Page}";
                BaseItems[index].SubItems[2].Text = $"{value.X:X1},{value.Y:X1}";
                BaseItems[index].SubItems[3].Text = value.Command.GetDescription(
                    Owner.AreaPlatformType);
            }
        }

        public void Add(UIAreaObjectCommand item)
        {
            Insert(Count, item);
        }

        public void MoveItem(int oldIndex, int newIndex)
        {
            var change = Math.Sign(newIndex - oldIndex);
            var temp = this[oldIndex];
            for (var i = oldIndex; i != newIndex; i += change)
            {
                this[i] = this[i + change];
            }

            this[newIndex] = temp;
        }

        public void Insert(int index, UIAreaObjectCommand item)
        {
            _ = BaseItems.Insert(index, Create(item));
        }

        public void Clear()
        {
            BaseItems.Clear();
        }

        public bool Contains(UIAreaObjectCommand item)
        {
            return IndexOf(item) != -1;
        }

        public int IndexOf(UIAreaObjectCommand item)
        {
            for (var i = 0; i < Count; i++)
            {
                if (item.Equals(this[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        public bool Remove(UIAreaObjectCommand item)
        {
            var index = IndexOf(item);
            if (index == -1)
            {
                return false;
            }

            RemoveAt(index);
            return true;
        }

        public void RemoveAt(int index)
        {
            BaseItems.RemoveAt(index);
        }

        public IEnumerator<UIAreaObjectCommand> GetEnumerator()
        {
            foreach (var item in BaseItems)
            {
                yield return (UIAreaObjectCommand)(item as ListViewItem)!.Tag;
            }
        }

        void ICollection<UIAreaObjectCommand>.CopyTo(
            UIAreaObjectCommand[] dest,
            int index)
        {
            if ((uint)(index + Count) < (uint)dest.Length || index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            for (var i = 0; i < Count; i++)
            {
                dest[index + i] = this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private ListViewItem Create(UIAreaObjectCommand item)
        {
            return new ListViewItem([
                item.Command.HexString,
                $"{item.Page}",
                $"{item.X:X1},{item.Y:X1}",
                item.Command.GetDescription(Owner.AreaPlatformType)])
            {
                Tag = item
            };
        }
    }
}
