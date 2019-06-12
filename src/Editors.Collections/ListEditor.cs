// <copyright file="ListEditor.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Editors.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using Maseya.Editors;
    using Maseya.Editors.IO;
    using Maseya.Helper;

    public class ListEditor<T> :
        Editor,
        IListEditor<T>,
        IList,
        IReadOnlyList<T>
    {
        public ListEditor(string path)
            : base(path)
        {
            BaseList = new List<T>();
        }

        public ListEditor(string path, IEnumerable<T> collection)
            : base(path)
        {
            BaseList = new List<T>(collection);
        }

        public event EventHandler SizeChanged;

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
                return CurrentSelection != null && CurrentSelection.Count > 0;
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
                return CopyData != null && CopyData.Count > 0;
            }
        }

        public override bool CanSelectAll
        {
            get
            {
                return Count > 0;
            }
        }

        public IListSelectionData<T> CopyData
        {
            get;
            set;
        }

        public IListSelection CurrentSelection
        {
            get;
            set;
        }

        public bool IsInsertMode
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

        bool ICollection<T>.IsReadOnly
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

        private List<T> BaseList
        {
            get;
            set;
        }

        private bool IsListLocked
        {
            get;
            set;
        }

        public T this[int index]
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
                this[index] = (T)value;
            }
        }

        public static OpenEditorCallback OpenByteFile(
            Converter<IEnumerable<byte>, IEnumerable<T>> converter)
        {
            return path =>
            {
                return new ListEditor<T>(
                    path,
                    converter(File.ReadAllBytes(path)));
            };
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

            if (selection is null)
            {
                throw new ArgumentNullException(nameof(selection));
            }

            if (selection.Count == 0)
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
            Paste(CopyData, IsInsertMode);
        }

        public void Paste(IListSelectionData<T> values, bool insert)
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

            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (values.Count == 0)
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

            if (selection is null)
            {
                throw new ArgumentNullException(nameof(selection));
            }

            if (selection.Count == 0)
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
            Action<IList<T>> action,
            Action<IList<T>> undo = null)
        {
            var oldCount = Count;
            if (undo is null)
            {
                WriteData(() => action(BaseList));
            }
            else
            {
                WriteData(() => action(BaseList), () => undo(BaseList));
            }

            if (Count != oldCount)
            {
                OnSizeChanged(EventArgs.Empty);
            }
        }

        public void InsertSelection(IListSelectionData<T> values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (values.Count == 0)
            {
                return;
            }

            var copy = values.Copy();
            WriteData(
                () => BaseList.InsertSelection(copy),
                () => BaseList.RemoveSelection(copy.Selection));

            OnSizeChanged(EventArgs.Empty);
        }

        public void RemoveSelection(IListSelection selection)
        {
            if (selection is null)
            {
                throw new ArgumentNullException(nameof(selection));
            }

            if (selection.Count == 0)
            {
                throw new ArgumentNullException(nameof(selection));
            }

            var values = BaseList.GetSelectionData(selection);
            WriteData(
                () => BaseList.RemoveSelection(values.Selection),
                () => BaseList.InsertSelection(values));
        }

        public void TransformSelection(
            IListSelection selection,
            Func<T, T> func)
        {
            if (selection is null)
            {
                throw new ArgumentNullException(nameof(selection));
            }

            if (selection.Count == 0)
            {
                return;
            }

            var values = BaseList.GetSelectionData(selection);
            WriteData(
                () => BaseList.TransformSelection(selection, func),
                () => BaseList.WriteSelection(values));
        }

        public void WriteSelection(IListSelectionData<T> values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (values.Count == 0)
            {
                return;
            }

            var copy = values.Copy();
            var oldValues = BaseList.GetSelectionData(copy.Selection);
            WriteData(
                () => BaseList.WriteSelection(copy),
                () => BaseList.WriteSelection(oldValues));
        }

        public void SetRange(int index, IEnumerable<T> collection)
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            foreach (var item in collection)
            {
                this[index++] = item;
            }
        }

        public void Add(T item)
        {
            Insert(Count, item);
        }

        public void AddRange(IEnumerable<T> collection)
        {
            InsertRange(Count, collection);
        }

        public void Clear()
        {
            var copy = BaseList;
            WriteData(
                () => BaseList = new List<T>(),
                () => BaseList = copy);

            OnSizeChanged(EventArgs.Empty);
        }

        public bool Contains(T item)
        {
            return BaseList.Contains(item);
        }

        public int IndexOf(T item)
        {
            return BaseList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            WriteData(
                () => BaseList.Insert(index, item),
                () => BaseList.RemoveAt(index));

            OnSizeChanged(EventArgs.Empty);
        }

        public void InsertRange(int index, IEnumerable<T> collection)
        {
            var data = new List<T>(collection);
            if (data.Count == 0)
            {
                return;
            }

            WriteData(
                () => BaseList.InsertRange(index, data),
                () => BaseList.RemoveRange(index, data.Count));

            OnSizeChanged(EventArgs.Empty);
        }

        public bool Remove(T item)
        {
            var index = IndexOf(item);
            if (index == -1)
            {
                return false;
            }

            WriteData(
                () => BaseList.RemoveAt(index),
                () => BaseList.Insert(index, item));

            OnSizeChanged(EventArgs.Empty);

            return true;
        }

        public void RemoveAt(int index)
        {
            var item = this[index];
            WriteData(
                () => BaseList.RemoveAt(index),
                () => BaseList.Insert(index, item));

            OnSizeChanged(EventArgs.Empty);
        }

        public void RemoveRange(int index, int count)
        {
            if (count == 0)
            {
                return;
            }

            var copy = new T[count];
            BaseList.CopyTo(index, copy, 0, count);
            WriteData(
                () => BaseList.RemoveRange(index, count),
                () => BaseList.InsertRange(index, copy));

            OnSizeChanged(EventArgs.Empty);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            BaseList.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
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
            if (value is T item)
            {
                Add(item);
                return Count;
            }

            return -1;
        }

        bool IList.Contains(object value)
        {
            return value is T item ? Contains(item) : false;
        }

        int IList.IndexOf(object value)
        {
            return value is T item ? IndexOf(item) : -1;
        }

        void IList.Insert(int index, object value)
        {
            Insert(index, (T)value);
        }

        void IList.Remove(object value)
        {
            if (value is T item)
            {
                Remove(item);
            }
        }

        protected virtual void OnSizeChanged(EventArgs e)
        {
            SizeChanged?.Invoke(this, e);
        }
    }
}
