// <copyright file="RecentFileCollection.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.MushROMs
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Maseya.Helper;
    using static System.IO.Path;

    public class RecentFileCollection :
        IList,
        IList<string>,
        IReadOnlyList<string>
    {
        public RecentFileCollection(int capacity)
        {
            RecentFiles = new List<string>(capacity);
        }

        public event EventHandler CollectionChanged;

        public int Count
        {
            get
            {
                return RecentFiles.Count;
            }
        }

        public int Capacity
        {
            get
            {
                return RecentFiles.Capacity;
            }

            set
            {
                RecentFiles.Capacity = value;
            }
        }

        bool ICollection<string>.IsReadOnly
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

        bool IList.IsFixedSize
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
                return (RecentFiles as ICollection).IsSynchronized;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                return (RecentFiles as ICollection).SyncRoot;
            }
        }

        private List<string> RecentFiles
        {
            get;
        }

        public string this[int index]
        {
            get
            {
                return RecentFiles[index];
            }

            set
            {
                var fullPath = GetFullPath(value);
                var comparer = StringFuncComparer.WindowsPathComparer;
                if (comparer.Equals(this[index], fullPath))
                {
                    return;
                }

                RecentFiles[index] = fullPath;
                OnCollectionChanged(EventArgs.Empty);
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
                this[index] = (string)value;
            }
        }

        public int IndexOf(string item)
        {
            var comparer = StringFuncComparer.WindowsPathComparer;
            for (var i = 0; i < Count; i++)
            {
                if (comparer.Equals(this[i], item))
                {
                    return i;
                }
            }

            return -1;
        }

        int IList.IndexOf(object value)
        {
            return value is string item ? IndexOf(item) : -1;
        }

        public bool Contains(string item)
        {
            return IndexOf(item) != -1;
        }

        bool IList.Contains(object value)
        {
            return value is string item ? Contains(item) : false;
        }

        public void RemoveAt(int index)
        {
            RecentFiles.RemoveAt(index);
            OnCollectionChanged(EventArgs.Empty);
        }

        public void Add(string item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            RemoveInternal(item);
            RestrictSize();
            RecentFiles.Insert(0, item);

            OnCollectionChanged(EventArgs.Empty);
        }

        int IList.Add(object value)
        {
            if (value is string item)
            {
                Add(item);
                return Count - 1;
            }
            else
            {
                return -1;
            }
        }

        public void Insert(int index, string item)
        {
            if (index >= Capacity)
            {
                index = Capacity - 1;
            }

            RemoveInternal(item);
            RestrictSize();
            RecentFiles.Insert(index, item);

            OnCollectionChanged(EventArgs.Empty);
        }

        void IList.Insert(int index, object value)
        {
            Insert(index, (string)value);
        }

        public void Clear()
        {
            RecentFiles.Clear();
            OnCollectionChanged(EventArgs.Empty);
        }

        public bool Remove(string item)
        {
            if (!RemoveInternal(item))
            {
                return false;
            }

            OnCollectionChanged(EventArgs.Empty);
            return true;
        }

        void IList.Remove(object value)
        {
            if (value is string item)
            {
                Remove(item);
            }
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            RecentFiles.CopyTo(array, arrayIndex);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            (RecentFiles as ICollection).CopyTo(array, index);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return RecentFiles.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected virtual void OnCollectionChanged(EventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        private bool RemoveInternal(string item)
        {
            // We need to use the path comparer.
            var index = IndexOf(item);
            if (index == -1)
            {
                return false;
            }

            RecentFiles.RemoveAt(index);
            return true;
        }

        private void RestrictSize()
        {
            if (Count >= Capacity)
            {
                RecentFiles.RemoveRange(Capacity - 1, Count - Capacity + 1);
            }
        }
    }
}
