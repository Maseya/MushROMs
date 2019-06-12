// <copyright file="SharedListParent.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Editors.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class SharedListParent :
        ISharedListParent,
        IList,
        IReadOnlyList<byte>
    {
        public SharedListParent()
        {
            BaseList = new List<byte>();
            Children = new ChildCollection(this);
        }

        public SharedListParent(int capacity)
        {
            BaseList = new List<byte>(capacity);
            Children = new ChildCollection(this);
        }

        public SharedListParent(IEnumerable<byte> collection)
        {
            BaseList = new List<byte>(collection);
            Children = new ChildCollection(this);
        }

        public int Count
        {
            get
            {
                return BaseList.Count;
            }
        }

        public int Capacity
        {
            get
            {
                return BaseList.Capacity;
            }

            set
            {
                BaseList.Capacity = value;
            }
        }

        public ChildCollection Children
        {
            get;
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

        private List<byte> BaseList
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
                BaseList[index] = value;
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

        public void InsertSelection(IListSelectionData<byte> values)
        {
            BaseList.InsertSelection(values);
        }

        public void RemoveSelection(IListSelection selection)
        {
            BaseList.RemoveSelection(selection);
        }

        public void TransformSelection(IListSelection selection, Func<byte, byte> func)
        {
            BaseList.TransformSelection(selection, func);
        }

        public void WriteSelection(IListSelectionData<byte> values)
        {
            BaseList.WriteSelection(values);
        }

        public void SetRange(int index, IEnumerable<byte> collection)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            foreach (var item in collection)
            {
                if (index >= Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                this[index++] = item;
            }
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
            BaseList.Clear();
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
            BaseList.Insert(index, item);
            UpdateInsert(index, 1);
        }

        public void InsertRange(int index, IEnumerable<byte> collection)
        {
            var orginalCount = Count;
            BaseList.InsertRange(index, collection);
            UpdateInsert(index, Count - orginalCount);
        }

        public bool Remove(byte item)
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
            BaseList.RemoveAt(index);
            UpdateRemove(index, 1);
        }

        public void RemoveRange(int index, int count)
        {
            BaseList.RemoveRange(index, count);
            UpdateRemove(index, count);
        }

        public void CopyTo(byte[] array, int arrayIndex = 0)
        {
            BaseList.CopyTo(array, arrayIndex);
        }

        public void CopyTo(int index, byte[] array, int arrayIndex, int count)
        {
            BaseList.CopyTo(index, array, arrayIndex, count);
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

        public class ChildCollection : ICollection<ISharedListChild>
        {
            internal ChildCollection(SharedListParent parent)
            {
                Parent = parent
                    ?? throw new ArgumentNullException(nameof(parent));

                BaseCollection = new List<ISharedListChild>();
            }

            public int Count
            {
                get
                {
                    return BaseCollection.Count;
                }
            }

            bool ICollection<ISharedListChild>.IsReadOnly
            {
                get
                {
                    return false;
                }
            }

            private SharedListParent Parent
            {
                get;
            }

            private List<ISharedListChild> BaseCollection
            {
                get;
            }

            public void Add(ISharedListChild item)
            {
                BaseCollection.Add(item);
            }

            public void Clear()
            {
                BaseCollection.Clear();
            }

            public bool Remove(ISharedListChild item)
            {
                return BaseCollection.Remove(item);
            }

            public bool Contains(ISharedListChild item)
            {
                return BaseCollection.Contains(item);
            }

            public void CopyTo(ISharedListChild[] item, int arrayIndex)
            {
                BaseCollection.CopyTo(item, arrayIndex);
            }

            public IEnumerator<ISharedListChild> GetEnumerator()
            {
                return BaseCollection.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
