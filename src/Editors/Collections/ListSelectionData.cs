// <copyright file="ListSelectionData.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Editors.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class ListSelectionData<T> :
        IListSelectionData,
        IListSelectionData<T>,
        IReadOnlyDictionary<int, T>
    {
        public ListSelectionData(
            IListSelection selection)
        {
            Selection = selection.Copy();
            BaseDictionary = new Dictionary<int, T>(Selection.Count);
            foreach (var index in Selection)
            {
                BaseDictionary.Add(index, default);
            }
        }

        public ListSelectionData(
            IListSelection selection,
            IReadOnlyList<T> data)
        {
            Selection = selection.Copy();
            BaseDictionary = new Dictionary<int, T>(Selection.Count);
            foreach (var index in Selection)
            {
                BaseDictionary.Add(index, data[index]);
            }
        }

        private ListSelectionData(
            IListSelection selection,
            IDictionary<int, T> dictionary)
        {
            Selection = selection.Copy();
            BaseDictionary = new Dictionary<int, T>(dictionary);
        }

        public int Count
        {
            get
            {
                return Selection.Count;
            }
        }

        public IListSelection Selection
        {
            get;
        }

        public ICollection<T> Values
        {
            get
            {
                return BaseDictionary.Values;
            }
        }

        ICollection<int> IDictionary<int, T>.Keys
        {
            get
            {
                return new KeyCollection(Selection);
            }
        }

        IEnumerable<int> IReadOnlyDictionary<int, T>.Keys
        {
            get
            {
                return Selection;
            }
        }

        ICollection IDictionary.Keys
        {
            get
            {
                return new KeyCollection(Selection);
            }
        }

        ICollection<T> IDictionary<int, T>.Values
        {
            get
            {
                return Values;
            }
        }

        IEnumerable<T> IReadOnlyDictionary<int, T>.Values
        {
            get
            {
                return Values;
            }
        }

        ICollection IDictionary.Values
        {
            get
            {
                return BaseDictionary.Values;
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return (BaseDictionary as ICollection).IsSynchronized;
            }
        }

        bool ICollection<KeyValuePair<int, T>>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                return (BaseDictionary as ICollection).SyncRoot;
            }
        }

        bool IDictionary.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        bool IDictionary.IsFixedSize
        {
            get
            {
                return true;
            }
        }

        private Dictionary<int, T> BaseDictionary
        {
            get;
        }

        public T this[int key]
        {
            get
            {
                return BaseDictionary[key];
            }

            set
            {
                BaseDictionary[key] = value;
            }
        }

        object IDictionary.this[object key]
        {
            get
            {
                return this[(int)key];
            }

            set
            {
                this[(int)key] = (T)value;
            }
        }

        public ListSelectionData<T> Copy()
        {
            return new ListSelectionData<T>(Selection, BaseDictionary);
        }

        public bool ContainsKey(int key)
        {
            return Selection.Contains(key);
        }

        public IEnumerator<KeyValuePair<int, T>> GetEnumerator()
        {
            foreach (var key in Selection)
            {
                yield return new KeyValuePair<int, T>(
                    key,
                    BaseDictionary[key]);
            }
        }

        public bool TryGetValue(int key, out T value)
        {
            return BaseDictionary.TryGetValue(key, out value);
        }

        IListSelectionData<T> IListSelectionData<T>.Copy()
        {
            return Copy();
        }

        IListSelectionData IListSelectionData.Copy()
        {
            return Copy();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return new DictionaryEnumerator(GetEnumerator());
        }

        void IDictionary<int, T>.Add(int key, T value)
        {
            throw new NotSupportedException();
        }

        bool IDictionary<int, T>.Remove(int key)
        {
            throw new NotSupportedException();
        }

        bool ICollection<KeyValuePair<int, T>>.Remove(
            KeyValuePair<int, T> item)
        {
            throw new NotSupportedException();
        }

        void ICollection<KeyValuePair<int, T>>.Add(KeyValuePair<int, T> item)
        {
            throw new NotSupportedException();
        }

        bool ICollection<KeyValuePair<int, T>>.Contains(
            KeyValuePair<int, T> item)
        {
            return (BaseDictionary as ICollection<KeyValuePair<int, T>>).
                Contains(item);
        }

        void ICollection<KeyValuePair<int, T>>.Clear()
        {
            throw new NotSupportedException();
        }

        void ICollection<KeyValuePair<int, T>>.CopyTo(
            KeyValuePair<int, T>[] array, int arrayIndex)
        {
            (BaseDictionary as ICollection<KeyValuePair<int, T>>).CopyTo(
                array,
                arrayIndex);
        }

        void IDictionary.Add(object key, object value)
        {
            throw new NotSupportedException();
        }

        void IDictionary.Clear()
        {
            throw new NotSupportedException();
        }

        bool IDictionary.Contains(object key)
        {
            return (BaseDictionary as IDictionary).Contains(key);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            (BaseDictionary as ICollection).CopyTo(array, index);
        }

        void IDictionary.Remove(object key)
        {
            throw new NotSupportedException();
        }

        private class KeyCollection : ICollection<int>, ICollection
        {
            public KeyCollection(IListSelection selection)
            {
                Selection = selection;
            }

            public int Count
            {
                get
                {
                    return Selection.Count;
                }
            }

            bool ICollection<int>.IsReadOnly
            {
                get
                {
                    return true;
                }
            }

            bool ICollection.IsSynchronized
            {
                get
                {
                    return false;
                }
            }

            object ICollection.SyncRoot
            {
                get;
            }

            private IListSelection Selection
            {
                get;
            }

            public bool Contains(int key)
            {
                return Selection.Contains(key);
            }

            public IEnumerator<int> GetEnumerator()
            {
                return Selection.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            void ICollection<int>.CopyTo(int[] array, int arrayIndex)
            {
                for (var i = 0; i < Selection.Count; i++)
                {
                    array[arrayIndex + i] = Selection[i];
                }
            }

            void ICollection.CopyTo(Array array, int index)
            {
                for (var i = 0; i < Selection.Count; i++)
                {
                    array.SetValue(i, index + i);
                }
            }

            void ICollection<int>.Add(int item)
            {
                throw new NotSupportedException();
            }

            void ICollection<int>.Clear()
            {
                throw new NotSupportedException();
            }

            bool ICollection<int>.Remove(int item)
            {
                throw new NotSupportedException();
            }
        }

        private class DictionaryEnumerator : IDictionaryEnumerator
        {
            public DictionaryEnumerator(
                IEnumerator<KeyValuePair<int, T>> baseEnumerator)
            {
                BaseEnumerator = baseEnumerator
                    ?? throw new ArgumentNullException(nameof(baseEnumerator));
            }

            public KeyValuePair<int, T> Current
            {
                get;
                private set;
            }

            public int Key
            {
                get
                {
                    return Current.Key;
                }
            }

            public T Value
            {
                get
                {
                    return Current.Value;
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            DictionaryEntry IDictionaryEnumerator.Entry
            {
                get
                {
                    return new DictionaryEntry(Key, Value);
                }
            }

            object IDictionaryEnumerator.Key
            {
                get
                {
                    return Key;
                }
            }

            object IDictionaryEnumerator.Value
            {
                get
                {
                    return Current.Value;
                }
            }

            private IEnumerator<KeyValuePair<int, T>> BaseEnumerator
            {
                get;
            }

            public void Reset()
            {
                BaseEnumerator.Reset();
            }

            public bool MoveNext()
            {
                return BaseEnumerator.MoveNext();
            }

            public void Dispose()
            {
                BaseEnumerator.Dispose();
            }
        }
    }
}
