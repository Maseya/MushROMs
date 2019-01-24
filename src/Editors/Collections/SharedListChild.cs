// <copyright file="SharedListChild.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Editors.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public abstract class SharedListChild<T> :
        ISharedListChild<T>,
        IList,
        IReadOnlyList<T>
    {
        protected SharedListChild(ISharedListParent parent)
        {
            Parent = parent
                ?? throw new ArgumentNullException(nameof(parent));

            Parent.Children.Add(this);
        }

        public int Count
        {
            get
            {
                return GetIndex(Parent.Count);
            }
        }

        public int StartOffset
        {
            get;
            set;
        }

        /// <inheritdoc/>
        ///
        bool IList.IsFixedSize
        {
            get
            {
                return false;
            }
        }

        /// <inheritdoc/>
        ///
        bool ICollection<T>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <inheritdoc/>
        ///
        bool IList.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <inheritdoc/>
        ///
        bool ICollection.IsSynchronized
        {
            get
            {
                return false;
            }
        }

        /// <inheritdoc/>
        ///
        object ICollection.SyncRoot
        {
            get
            {
                return (Parent as IList).SyncRoot;
            }
        }

        private ISharedListParent Parent
        {
            get;
        }

        public T this[int index]
        {
            get
            {
                if ((uint)index >= (uint)Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                return GetValueAtOffset(GetOffset(index));
            }

            set
            {
                if ((uint)index >= (uint)Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                Parent.SetRange(GetOffset(index), GetByteData(value));
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
                if (value is T item)
                {
                    this[index] = item;
                }

                throw new ArgumentException();
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
            Parent.RemoveRange(StartOffset, GetByteCount(Count));
        }

        public bool Contains(T item)
        {
            return IndexOf(item) != -1;
        }

        public bool ContainsAtAnyOffset(T item)
        {
            return OffsetOf(item) != -1;
        }

        public IEnumerable<TOutput> ConvertAll<TOutput>(
            Converter<T, TOutput> converter)
        {
            if (converter is null)
            {
                throw new ArgumentNullException(nameof(converter));
            }

            return this.Select(item => converter(item));
        }

        public IEnumerable<TOutput> ConvertAtAllOffsets<TOutput>(
            Converter<T, TOutput> converter)
        {
            if (converter is null)
            {
                throw new ArgumentNullException(nameof(converter));
            }

            return EnumerateAtAllOffsets().Select(item => converter(item));
        }

        public void CopyTo(T[] array)
        {
            CopyTo(array, 0);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            CopyTo(0, array, arrayIndex, Count);
        }

        public void CopyTo(
            int index,
            T[] array,
            int arrayIndex,
            int count)
        {
            for (var i = 0; i < count; i++)
            {
                array[arrayIndex + i] = this[index + i];
            }
        }

        public IEnumerable<T> EnumerateAtAllOffsets()
        {
            return EnumerateAtAllOffsets(0);
        }

        public IEnumerable<T> EnumerateAtAllOffsets(int start)
        {
            return EnumerateAtAllOffsets(start, Parent.Count - start);
        }

        public IEnumerable<T> EnumerateAtAllOffsets(int startOffset, int count)
        {
            if ((uint)startOffset > (uint)Parent.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            if ((uint)count > (uint)Parent.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (startOffset + count > Count)
            {
                throw new ArgumentException();
            }

            for (var i = startOffset; i < count; i++)
            {
                var size = GetSizeOfItemAtOffset(i);
                if (i + size >= count)
                {
                    yield break;
                }

                yield return GetItem(Parent.Skip(i));
            }
        }

        public bool Exists(Predicate<T> match)
        {
            return FindIndex(match) != -1;
        }

        public bool ExistsAtAnyOffset(Predicate<T> match)
        {
            return FindOffset(match) != -1;
        }

        public T Find(Predicate<T> match)
        {
            return FindInternal(this, match);
        }

        public List<T> FindAll(Predicate<T> match)
        {
            return FindAllInternal(match, this);
        }

        public List<T> FindAtAllOffsets(Predicate<T> match)
        {
            return FindAllInternal(match, EnumerateAtAllOffsets());
        }

        public T FindAtAnyOffset(Predicate<T> match)
        {
            return FindInternal(EnumerateAtAllOffsets(), match);
        }

        public int FindIndex(Predicate<T> match)
        {
            return FindIndex(0, Count, match);
        }

        public int FindIndex(int startIndex, Predicate<T> match)
        {
            return FindIndex(startIndex, Count - startIndex, match);
        }

        public int FindIndex(int startIndex, int count, Predicate<T> match)
        {
            if ((uint)startIndex > (uint)Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (count < 0 || startIndex > Count - count)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (match is null)
            {
                throw new ArgumentNullException(nameof(match));
            }

            var endIndex = startIndex + count;
            for (var i = startIndex; i < endIndex; i++)
            {
                var item = this[i];
                if (match(item))
                {
                    return i;
                }
            }

            return -1;
        }

        public T FindLast(Predicate<T> match)
        {
            if (match is null)
            {
                throw new ArgumentNullException(nameof(match));
            }

            for (var i = Count; --i >= 0;)
            {
                var item = this[i];
                if (match(item))
                {
                    return item;
                }
            }

            return default;
        }

        public T FindLastAtAnyOffset(Predicate<T> match)
        {
            if (match is null)
            {
                throw new ArgumentNullException();
            }

            foreach (var item in EnumerateAtAllOffsets().Reverse())
            {
                if (match(item))
                {
                    return item;
                }
            }

            return default;
        }

        public int FindLastIndex(Predicate<T> match)
        {
            return FindLastIndex(Count - 1, Count, match);
        }

        public int FindLastIndex(int startIndex, Predicate<T> match)
        {
            return FindLastIndex(startIndex, startIndex + 1, match);
        }

        public int FindLastIndex(int startIndex, int count, Predicate<T> match)
        {
            if (match is null)
            {
                throw new ArgumentNullException(nameof(match));
            }

            if (Count == 0)
            {
                if (startIndex != -1)
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
            else if ((uint)startIndex >= (uint)Count)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            if (count < 0 || startIndex - count + 1 < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            var endIndex = startIndex - count;
            for (var i = startIndex; i > endIndex; i--)
            {
                var item = this[i];
                if (match(item))
                {
                    return i;
                }
            }

            return -1;
        }

        public int FindLastOffset(Predicate<T> match)
        {
            return FindLastOffset(Parent.Count - 1, match);
        }

        public int FindLastOffset(int startOffset, Predicate<T> match)
        {
            return FindLastOffset(startOffset, startOffset + 1, match);
        }

        public int FindLastOffset(
                            int startOffset,
                            int count,
                            Predicate<T> match)
        {
            if (match is null)
            {
                throw new ArgumentNullException(nameof(match));
            }

            if (Parent.Count == 0)
            {
                if (startOffset != -1)
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
            else if ((uint)startOffset >= (uint)Parent.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(startOffset));
            }

            var end = startOffset - (count - 1);
            if (count < 0 || end < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            var i = startOffset;
            foreach (var item in EnumerateAtAllOffsets(end, count).Reverse())
            {
                if (match(item))
                {
                    return i;
                }

                i--;
            }

            return -1;
        }

        public int FindOffset(Predicate<T> match)
        {
            return FindOffset(0, match);
        }

        public int FindOffset(int startOffset, Predicate<T> match)
        {
            return FindOffset(
                startOffset,
                Parent.Count - startOffset,
                match);
        }

        public int FindOffset(int startOffset, int count, Predicate<T> match)
        {
            if (match is null)
            {
                throw new ArgumentNullException(nameof(match));
            }

            var offset = startOffset;
            foreach (var item in EnumerateAtAllOffsets(startOffset, count))
            {
                if (match(item))
                {
                    return offset;
                }

                offset++;
            }

            return -1;
        }

        public void ForEach(Action<T> action)
        {
            ForEachInternal(action, this);
        }

        public void ForEachAtAllOffsets(Action<T> action)
        {
            ForEachInternal(action, EnumerateAtAllOffsets());
        }

        public int IndexOf(T item)
        {
            return IndexOf(item, 0);
        }

        public int IndexOf(T item, int index)
        {
            return IndexOf(item, index, Count);
        }

        public int IndexOf(T item, int index, int count)
        {
            if (index > Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (count < 0 || index > Count - count)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            var last = index + count;
            for (var i = index; i < last; i++)
            {
                if (Equals(this[i], item))
                {
                    return i;
                }
            }

            return -1;
        }

        public void Insert(int index, T item)
        {
            InsertAtOffset(GetOffset(index), item);
        }

        public void InsertAtOffset(int offset, T item)
        {
            Parent.InsertRange(
                offset,
                GetByteData(item));
        }

        public void InsertRange(int index, IEnumerable<T> collection)
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if ((uint)index > (uint)Count)
            {
                throw new ArgumentException();
            }

            Parent.InsertRange(
                GetOffset(index),
                GetByteData(collection));
        }

        public void SetRange(int index, IEnumerable<T> collection)
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if ((uint)index > (uint)Count)
            {
                throw new ArgumentException();
            }

            Parent.SetRange(
                GetOffset(index),
                GetByteData(collection));
        }

        public int LastIndexOf(T item)
        {
            return Count > 0 ? LastIndexOf(item, Count - 1, Count) : -1;
        }

        public int LastIndexOf(T item, int index)
        {
            if (index >= Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return LastIndexOf(item, index, index + 1);
        }

        public int LastIndexOf(T item, int index, int count)
        {
            if (Count == 0)
            {
                return -1;
            }

            if ((uint)index > (uint)Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (Count > index + 1)
            {
                throw new ArgumentOutOfRangeException();
            }

            var end = index - count + 1;
            for (var i = index; i > end; i--)
            {
                if (Equals(this[i], item))
                {
                    return i;
                }
            }

            return -1;
        }

        public int LastOffsetOf(T item)
        {
            return LastOffsetOf(item, Parent.Count - 1);
        }

        public int LastOffsetOf(T item, int offset)
        {
            return LastOffsetOf(item, offset, offset + 1);
        }

        public int LastOffsetOf(T item, int offset, int count)
        {
            if (Parent.Count == 0)
            {
                return -1;
            }

            if ((uint)offset > (uint)Parent.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (Parent.Count > offset + 1)
            {
                throw new ArgumentOutOfRangeException();
            }

            var i = offset;
            var end = offset - count + 1;
            foreach (var element in EnumerateAtAllOffsets(end, count))
            {
                if (Equals(element, item))
                {
                    return i;
                }

                i--;
            }

            return -1;
        }

        public int OffsetOf(T item)
        {
            return OffsetOf(item, 0);
        }

        public int OffsetOf(T item, int offset)
        {
            return OffsetOf(item, offset, Parent.Count - offset);
        }

        public int OffsetOf(T item, int offset, int count)
        {
            if (offset > Parent.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            if (count < 0 || offset > Parent.Count - count)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            var last = offset + count;
            var i = offset;
            foreach (var element in EnumerateAtAllOffsets())
            {
                if (Equals(element, item))
                {
                    return i;
                }

                i++;
            }

            return -1;
        }

        public bool Remove(T item)
        {
            var index = IndexOf(item);
            if (index == -1)
            {
                return false;
            }

            RemoveAt(index);
            return true;
        }

        public int RemoveAll(Predicate<T> match)
        {
            var selection = new EnumerableIndexListSelection(
                this.FindAllIndexes(match));

            var originalSize = Parent.Count;
            RemoveSelection(selection);
            return originalSize - Parent.Count;
        }

        public void RemoveAt(int index)
        {
            RemoveAtOffset(GetOffset(index));
        }

        public void RemoveAtOffset(int offset)
        {
            var size = GetSizeOfItemAtOffset(offset);
            Parent.RemoveRange(offset, size);
        }

        public bool RemoveFromAnyOffset(T item)
        {
            var offset = OffsetOf(item);
            if (offset == -1)
            {
                return false;
            }

            RemoveAtOffset(offset);
            return true;
        }

        public void RemoveRange(int index, int count)
        {
            var offset = GetOffset(index);
            var end = GetOffset(index + count);
            Parent.RemoveRange(offset, end - offset);
        }

        public void RemoveRangeAtOffset(int offset, int count)
        {
            Parent.RemoveRange(offset, GetOffset(count, offset));
        }

        public void Reverse()
        {
            Reverse(0, Count);
        }

        public void Reverse(int index, int count)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            var end = index + count;
            if (end > Count)
            {
                throw new ArgumentException();
            }

            SetRange(index, Reverse());

            IEnumerable<T> Reverse()
            {
                for (var i = count; --i >= 0;)
                {
                    yield return this[index + i];
                }
            }
        }

        public T[] ToArray()
        {
            var array = new T[Count];
            CopyTo(array);
            return array;
        }

        public bool TrueForAll(Predicate<T> match)
        {
            return TrueForAllInternal(match, this);
        }

        public bool TrueForAllOffsets(Predicate<T> match)
        {
            return TrueForAllInternal(match, EnumerateAtAllOffsets());
        }

        public void WriteSelection(IListSelectionData<T> values)
        {
            WriteSelection(StartOffset, values);
        }

        public void WriteSelection(
            int startOffset,
            IListSelectionData<T> values)
        {
            Parent.WriteSelection(GetSelectedByteData(startOffset, values));
        }

        public void TransformSelection(
            IListSelection selection,
            Func<T, T> func)
        {
            TransformSelection(StartOffset, selection, func);
        }

        public void TransformSelection(
            int startOffset,
            IListSelection selection,
            Func<T, T> func)
        {
            var values = new ListSelectionData<T>(selection, this);
            foreach (var index in selection)
            {
                values[index] = func(values[index]);
            }

            WriteSelection(values);
        }

        public void InsertSelection(IListSelectionData<T> values)
        {
            InsertSelection(StartOffset, values);
        }

        public void InsertSelection(
            int startOffset,
            IListSelectionData<T> values)
        {
            Parent.InsertSelection(GetSelectedByteData(startOffset, values));
        }

        public void RemoveSelection(IListSelection selection)
        {
            RemoveSelection(StartOffset, selection);
        }

        public void RemoveSelection(int startOffset, IListSelection selection)
        {
            Parent.RemoveSelection(
                GetByteSelection(startOffset, selection));
        }

        public IEnumerable<int> GetSelectedByteIndexes(
            IListSelection itemSelection)
        {
            return GetSelectedByteIndexes(StartOffset, itemSelection);
        }

        public IEnumerable<int> GetSelectedByteIndexes(
            int startOffset,
            IListSelection itemSelection)
        {
            if (itemSelection is null)
            {
                throw new ArgumentNullException(nameof(itemSelection));
            }

            foreach (var index in itemSelection)
            {
                var offset = GetOffset(index, startOffset);
                var size = GetSizeOfItemAtOffset(offset);
                for (var i = 0; i < size; i++)
                {
                    yield return offset + i;
                }
            }
        }

        public EnumerableIndexListSelection GetByteSelection(
            IListSelection itemSelection)
        {
            return GetByteSelection(StartOffset, itemSelection);
        }

        public EnumerableIndexListSelection GetByteSelection(
            int startOffset,
            IListSelection itemSelection)
        {
            var byteIndexes = GetSelectedByteIndexes(
                startOffset,
                itemSelection);

            return new EnumerableIndexListSelection(byteIndexes);
        }

        public ListSelectionData<byte> GetSelectedByteData(
            IListSelectionData<T> values)
        {
            return GetSelectedByteData(StartOffset, values);
        }

        public ListSelectionData<byte> GetSelectedByteData(
            int startOffset,
            IListSelectionData<T> values)
        {
            var byteSelection = GetByteSelection(
                startOffset,
                values.Selection);

            return new ListSelectionData<byte>(
                byteSelection,
                new ReadOnlyCollection<byte>(Parent));
        }

        public int GetByteCount(int index)
        {
            return GetByteCount(index, StartOffset);
        }

        public int GetByteCount(int index, int startOffset)
        {
            return GetOffset(index, startOffset) - startOffset;
        }

        public int GetOffset(int index)
        {
            return GetOffset(index, StartOffset);
        }

        public abstract int GetOffset(int index, int startOffset);

        public int GetIndex(int offset)
        {
            return GetIndex(offset, StartOffset);
        }

        public abstract int GetIndex(int offset, int startOffset);

        public abstract int GetSizeOfItem(T item);

        public abstract int GetSizeOfItemAtOffset(int offset);

        public virtual T GetValueAtOffset(int offset)
        {
            return GetItem(Skip());

            IEnumerable<byte> Skip()
            {
                for (var i = offset; i < Parent.Count; i++)
                {
                    yield return Parent[i];
                }
            }
        }

        public virtual IEnumerable<byte> GetByteData(IEnumerable<T> items)
        {
            if (items is null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            foreach (var item in items)
            {
                foreach (var x in GetByteData(item))
                {
                    yield return x;
                }
            }
        }

        public virtual IEnumerable<T> GetItems(IEnumerable<byte> data)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            using (var en = data.GetEnumerator())
            {
                while (en.MoveNext())
                {
                    yield return GetItem(Current());
                }

                IEnumerable<byte> Current()
                {
                    do
                    {
                        yield return en.Current;
                    }
                    while (en.MoveNext());
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }

        public abstract T GetItem(IEnumerable<byte> data);

        public abstract IEnumerable<byte> GetByteData(T item);

        /// <inheritdoc/>
        ///
        int IList.Add(object value)
        {
            if (value is T item)
            {
                Add(item);
                return Count - 1;
            }

            return -1;
        }

        bool IList.Contains(object value)
        {
            return value is T item ? Contains(item) : false;
        }

        void ICollection.CopyTo(Array array, int index)
        {
            if (array is null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (array.Rank != 1)
            {
                throw new ArgumentException();
            }

            for (var i = 0; i < array.Length; i++)
            {
                array.SetValue(this[i], i);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        int IList.IndexOf(object value)
        {
            return value is T item
                ? IndexOf(item)
                : -1;
        }

        void IList.Insert(int index, object value)
        {
            if (value is T item)
            {
                Insert(index, item);
            }

            throw new ArgumentException();
        }

        void IList.Remove(object value)
        {
            if (value is T item)
            {
                Remove(item);
            }
        }

        private static bool TrueForAllInternal(
            Predicate<T> match,
            IEnumerable<T> collection)
        {
            if (match is null)
            {
                throw new ArgumentNullException(nameof(match));
            }

            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            foreach (var item in collection)
            {
                if (!match(item))
                {
                    return false;
                }
            }

            return true;
        }

        private static List<T> FindAllInternal(
            Predicate<T> match,
            IEnumerable<T> collection)
        {
            if (match is null)
            {
                throw new ArgumentNullException(nameof(match));
            }

            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            var list = new List<T>();
            foreach (var item in collection)
            {
                if (match(item))
                {
                    list.Add(item);
                }
            }

            return list;
        }

        private static T FindInternal(
            IEnumerable<T> collection,
            Predicate<T> match)
        {
            if (match is null)
            {
                throw new ArgumentNullException(nameof(match));
            }

            if (collection is null)
            {
                throw new ArgumentNullException(nameof(match));
            }

            foreach (var item in collection)
            {
                if (match(item))
                {
                    return item;
                }
            }

            return default;
        }

        private void ForEachInternal(
            Action<T> action,
            IEnumerable<T> collection)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            foreach (var item in collection)
            {
                action(item);
            }
        }
    }
}
