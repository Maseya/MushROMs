// <copyright file="SharedListParentEditor.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Editors.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Maseya.Helper;

    public class SharedListParentEditor :
        Editor,
        ISharedListParent,
        IListEditor<byte>,
        IList,
        IReadOnlyList<byte>
    {
        public SharedListParentEditor(string path)
            : base(path)
        {
            BaseList = new SharedListParent();
        }

        public SharedListParentEditor(
            string path,
            IEnumerable<byte> collection)
            : base(path)
        {
            BaseList = new SharedListParent();
        }

        public override bool CanCopy
        {
            get
            {
                return CurrentSelection != null && CurrentSelection.Count > 0;
            }
        }

        public override bool CanDelete
        {
            get
            {
                return CurrentSelection != null;
            }
        }

        public override bool CanInsert
        {
            get
            {
                return true;
            }
        }

        public override bool CanPaste
        {
            get
            {
                return CopyData != null;
            }
        }

        public override bool CanSelectAll
        {
            get
            {
                return true;
            }
        }

        public IListSelectionData<byte> CopyData
        {
            get;
            set;
        }

        public IListSelection CurrentSelection
        {
            get;
            set;
        }

        public bool IsInsert
        {
            get;
            set;
        }

        public int Count
        {
            get
            {
                return BaseList.Count;
            }
        }

        public SharedListParent.ChildCollection Children
        {
            get
            {
                return BaseList.Children;
            }
        }

        ICollection<ISharedListChild> ISharedListParent.Children
        {
            get
            {
                return Children;
            }
        }

        bool IList.IsFixedSize
        {
            get
            {
                return false;
            }
        }

        bool IList.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        bool ICollection<byte>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return (BaseList as ICollection).IsSynchronized;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                return (BaseList as ICollection).SyncRoot;
            }
        }

        private SharedListParent BaseList
        {
            get;
            set;
        }

        public byte this[int index]
        {
            get
            {
                return BaseList[index];
            }

            set
            {
                var copy = this[index];
                WriteData(
                    () => BaseList[index] = value,
                    () => BaseList[index] = copy);
            }
        }

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }

            set
            {
                this[index] = (byte)value;
            }
        }

        public override void Cut()
        {
            Cut(CurrentSelection);
        }

        public void Cut(IListSelection selection)
        {
            if (!CanCut)
            {
                return;
            }

            var e = new CancelEventArgs();
            OnBeginCut(e);
            if (e.Cancel)
            {
                return;
            }

            CopyData = BaseList.GetSelectionData(selection);
            RemoveSelection(selection);
            OnCutComplete(EventArgs.Empty);
        }

        public override void Copy()
        {
            Copy(CurrentSelection);
        }

        public void Copy(IListSelection selection)
        {
            if (!CanCopy)
            {
                return;
            }

            if (selection is null)
            {
                throw new ArgumentNullException(nameof(selection));
            }

            if (selection.Count == 0)
            {
                return;
            }

            var e = new CancelEventArgs();
            OnBeginCopy(e);
            if (e.Cancel)
            {
                return;
            }

            CopyData = BaseList.GetSelectionData(selection);
            OnCopyComplete(EventArgs.Empty);
        }

        public override void Paste()
        {
            Paste(CopyData, IsInsert);
        }

        public void Paste(IListSelectionData<byte> values, bool insert)
        {
            if (!CanPaste)
            {
                return;
            }

            var e = new CancelEventArgs();
            OnBeginPaste(e);
            if (e.Cancel)
            {
                return;
            }

            if (insert)
            {
                InsertSelection(values);
            }
            else
            {
                WriteSelection(values);
            }

            OnPasteComplete(EventArgs.Empty);
        }

        public override void Delete()
        {
            Delete(CurrentSelection);
        }

        public void Delete(IListSelection selection)
        {
            if (!CanDelete)
            {
                return;
            }

            var e = new CancelEventArgs();
            OnBeginDelete(e);
            if (e.Cancel)
            {
                return;
            }

            RemoveSelection(CurrentSelection);
            OnDeleteComplete(EventArgs.Empty);
        }

        public override void SelectAll()
        {
            CurrentSelection = new LinearListSelection(0, BaseList.Count);
        }

        public void ModifyList(
            Action<IList<byte>> action,
            Action<IList<byte>> undo = null)
        {
            if (undo is null)
            {
                WriteData(() => action(BaseList));
            }
            else
            {
                WriteData(() => action(BaseList), () => undo(BaseList));
            }
        }

        public void ModifyItemList<T>(
            Converter<ISharedListParent, ISharedListChild<T>> converter,
            Action<IList<T>> action,
            Action<IList<T>> undo = null)
        {
            var child = converter(BaseList);
            if (undo is null)
            {
                WriteData(() => action(child));
            }
            else
            {
                WriteData(() => action(child), () => undo(child));
            }
        }

        public void InsertSelection(IListSelectionData<byte> values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var copy = values.Copy();
            WriteData(
                () => BaseList.InsertSelection(copy),
                () => BaseList.RemoveSelection(copy.Selection));
        }

        public void RemoveSelection(IListSelection selection)
        {
            var values = BaseList.GetSelectionData(selection);
            WriteData(
                () => BaseList.RemoveSelection(values.Selection),
                () => BaseList.InsertSelection(values));
        }

        public void TransformSelection(
            IListSelection selection,
            Func<byte, byte> func)
        {
            var values = BaseList.GetSelectionData(selection);
            WriteData(
                () => BaseList.TransformSelection(selection, func),
                () => BaseList.WriteSelection(values));
        }

        public void WriteSelection(IListSelectionData<byte> values)
        {
            var copy = values.Copy();
            var oldValues = BaseList.GetSelectionData(copy.Selection);
            WriteData(
                () => BaseList.WriteSelection(copy),
                () => BaseList.WriteSelection(oldValues));
        }

        public void SetRange(int index, IEnumerable<byte> collection)
        {
            var data = new List<byte>(collection);
            var original = new byte[data.Count];
            BaseList.CopyTo(index, original, 0, original.Length);
            WriteData(
                () => BaseList.SetRange(index, data),
                () => BaseList.SetRange(index, original));
        }

        public void Add(byte item)
        {
            Insert(Count, item);
        }

        public void AddRange(IEnumerable<byte> collection)
        {
            InsertRange(Count, collection);
        }

        public void Clear()
        {
            var copy = BaseList;
            WriteData(
                () => BaseList.Clear(),
                () => BaseList.AddRange(copy));
        }

        public bool Contains(byte item)
        {
            return BaseList.Contains(item);
        }

        public int IndexOf(byte item)
        {
            return BaseList.IndexOf(item);
        }

        public void Insert(int index, byte item)
        {
            WriteData(
                () => BaseList.Insert(index, item),
                () => BaseList.RemoveAt(index));
        }

        public void InsertRange(int index, IEnumerable<byte> collection)
        {
            var data = new List<byte>(collection);
            WriteData(
                () => BaseList.InsertRange(index, data),
                () => BaseList.RemoveRange(index, data.Count));
        }

        public bool Remove(byte item)
        {
            var index = IndexOf(item);
            if (index == -1)
            {
                return false;
            }

            WriteData(
                () => BaseList.RemoveAt(index),
                () => BaseList.Insert(index, item));

            return true;
        }

        public void RemoveAt(int index)
        {
            var item = this[index];
            WriteData(
                () => BaseList.RemoveAt(index),
                () => BaseList.Insert(index, item));
        }

        public void RemoveRange(int index, int count)
        {
            var copy = new byte[count];
            BaseList.CopyTo(index, copy, 0, count);
            WriteData(
                () => BaseList.RemoveRange(index, count),
                () => BaseList.InsertRange(index, copy));
        }

        public void CopyTo(byte[] array, int arrayIndex)
        {
            BaseList.CopyTo(array, arrayIndex);
        }

        public IEnumerator<byte> GetEnumerator()
        {
            return BaseList.GetEnumerator();
        }

        void ICollection.CopyTo(Array array, int index)
        {
            (BaseList as ICollection).CopyTo(array, index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        int IList.Add(object value)
        {
            if (value is byte item)
            {
                Add(item);
                return Count;
            }

            return -1;
        }

        bool IList.Contains(object value)
        {
            return value is byte item ? Contains(item) : false;
        }

        int IList.IndexOf(object value)
        {
            return value is byte item ? IndexOf(item) : -1;
        }

        void IList.Insert(int index, object value)
        {
            Insert(index, (byte)value);
        }

        void IList.Remove(object value)
        {
            if (value is byte item)
            {
                Remove(item);
            }
        }

        internal void AddChild(ISharedListChild child)
        {
            Children.Add(child);
        }

        private void UpdateInsert(int index, int count)
        {
            foreach (var child in Children)
            {
                if (child.StartOffset > index)
                {
                    child.StartOffset += count;
                }
            }
        }

        private void UpdateRemove(int index, int count)
        {
            foreach (var child in Children)
            {
                if (child.StartOffset > index + count)
                {
                    child.StartOffset -= count;
                }
                else if (child.StartOffset > index)
                {
                    child.StartOffset = index;
                }
            }
        }
    }
}
