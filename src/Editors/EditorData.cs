namespace Maseya.Editors
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using static Helper.ThrowHelper;

    /// <summary>
    /// A base class that represents a strongly typed list of objects that can
    /// be accessed by index and uses an internal <see cref=" List{T}"/> of
    /// <see cref="Byte"/> as its data storage.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the elements in the list.
    /// </typeparam>
    public abstract class EditorData<T> :
        IList,
        IList<T>,
        IReadOnlyList<T>
    {
        /// <summary>
        /// The default capacity of <see cref="List{T}"/>.
        /// </summary>
        private const int DefaultCapacity = 4;

        /// <summary>
        /// The byte offset of to begin reading from in the internal data <see
        /// cref="List{T}"/>.
        /// </summary>
        private int _startOffset;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorData{T}"/> class
        /// that is empty and has the default initial capacity.
        /// </summary>
        protected EditorData()
            : this(DefaultCapacity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorData{T}"/> class
        /// that is empty and has the specified initial capacity.
        /// </summary>
        /// <param name="capacity">
        /// The number of elements that the list can initially store.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="capacity"/> is less than 0.
        /// </exception>
        protected EditorData(int capacity)
        {
            if (capacity < 0)
            {
                throw ValueNotGreaterThanEqualTo(nameof(capacity), capacity);
            }

            InternalData = new List<byte>(GetByteCount(capacity, 0));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorData{T}"/> class
        /// that contains <see cref="Byte"/> data copied from a specified
        /// collection and has sufficient capacity to accommodate the number of
        /// elements copied.
        /// </summary>
        /// <param name="data">
        /// The collection whose elements are copied to the <see cref="
        /// EditorData{T}"/> internal storage <see cref="List{T}"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="data"/> is <see langword="null"/>.
        /// </exception>
        protected EditorData(IEnumerable<byte> data)
        {
            InternalData = new List<byte>(data);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorData{T}"/> class
        /// that directly uses a <see cref="List{T}"/> as its internal data
        /// storage/
        /// </summary>
        /// <param name="data">
        /// The internal <see cref="Byte"/> data that will make up the <see
        /// cref="EditorData{T}"/>.
        /// </param>
        protected EditorData(List<byte> data)
        {
            InternalData = data ??
                throw new ArgumentNullException(nameof(data));
        }

        /// <summary>
        /// Gets or sets the byte offset to begin reading from in the internal
        /// data <see cref="List{T}"/>.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <see cref="StartOffset"/> is set to a value less than 0.
        /// <para/>
        /// -or-
        /// <para/>
        /// <see cref="StartOffset"/> is set to a value greater than or equal
        /// to <see cref="InternalDataSize"/>.
        /// </exception>
        public int StartOffset
        {
            get
            {
                return _startOffset;
            }

            set
            {
                if ((uint)value > (uint)InternalDataSize)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                _startOffset = value;
                Version++;
            }
        }

        /// <summary>
        /// Gets or sets the total number of elements the internal data
        /// structure can hold without resizing.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <see cref="Capacity"/> is set to a value that is less than <see
        /// cref="Count"/>.
        /// </exception>
        /// <exception cref="OutOfMemoryException">
        /// There is not enough memory available to the system.
        /// </exception>
        public int Capacity
        {
            get
            {
                return GetIndex(InternalData.Capacity);
            }

            set
            {
                InternalData.Capacity = GetOffset(value);
            }
        }

        /// <summary>
        /// Gets the number of elements contained in this <see cref="
        /// EditorData{T}"/>.
        /// </summary>
        public int Count
        {
            get
            {
                return GetIndex(InternalData.Count);
            }
        }

        /// <summary>
        /// Gets the number of bytes contained in the internal data <see
        /// cref="List{T}"/>.
        /// </summary>
        public int InternalDataSize
        {
            get
            {
                return InternalData.Count;
            }
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
                return (InternalData as IList).SyncRoot;
            }
        }

        /// <summary>
        /// Gets the internal <see cref="List{T}"/> that makes up the <see
        /// cref="EditorData{T}"/>.
        /// </summary>
        private List<byte> InternalData
        {
            get;
        }

        /// <summary>
        /// Gets or sets the current list write version. This value is strictly
        /// increased whenever a write to <see cref="InternalData"/> is
        /// performed.
        /// </summary>
        private int Version
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the <see cref="EditorData{T}"/> to get or
        /// set.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than 0.
        /// <para/>
        /// -or-
        /// <para/>
        /// <paramref name="index"/> is greater than or equal to <see
        /// cref="Count"/>.
        /// </exception>
        public T this[int index]
        {
            get
            {
                if ((uint)index >= (uint)Count)
                {
                    throw ValueNotInArrayBounds(
                        nameof(index),
                        index,
                        Count);
                }

                return GetValueAtOffset(GetOffset(index));
            }

            set
            {
                if ((uint)index >= (uint)Count)
                {
                    throw ValueNotInArrayBounds(
                        nameof(index),
                        index,
                        Count);
                }

                SetValueAtOffset(GetOffset(index), value);
            }
        }

        /// <inheritdoc/>
        ///
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

        /// <summary>
        /// Adds an element to the end of the <see cref=" EditorData{T}"/>.
        /// </summary>
        /// <param name="item">
        /// The element to be added to the end of the <see cref="
        /// EditorData{T}"/>.
        /// </param>
        public void Add(T item)
        {
            InternalData.InsertRange(GetOffset(Count), GetByteData(item));
        }

        /// <inheritdoc/>
        ///
        int IList.Add(object value)
        {
            if (value is T item)
            {
                Add(item);
                return Count - 1;
            }

            throw new ArgumentException();
        }

        /// <summary>
        /// Adds the elements of the specified collection to the end of the
        /// <see cref="EditorData{T}"/>.
        /// </summary>
        /// <param name="collection">
        /// The collection whose elements should be added to the end of the
        /// <see cref="EditorData{T}"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection"/> is <see langword="null"/>.
        /// </exception>
        public void AddRange(IEnumerable<T> collection)
        {
            InsertRange(Count, collection);
        }

        /// <summary>
        /// Returns a read-only <see cref="ReadOnlyCollection{T}"/> wrapper for
        /// the current collection.
        /// </summary>
        /// <returns>
        /// An object that acts as a read-only wrapper around the <see
        /// cref="EditorData{T}"/>.
        /// </returns>
        public ReadOnlyCollection<T> AsReadOnly()
        {
            return new ReadOnlyCollection<T>(this);
        }

        /// <summary>
        /// Returns a read-only <see cref="ReadOnlyCollection{T}"/> wrapper of
        /// the internal <see cref="Byte"/><see cref=" List{T}"/> for this
        /// collection.
        /// </summary>
        /// <returns>
        /// An object that acts as a read-only wrapper around the current
        /// internal <see cref="Byte"/><see cref="List{T}"/> for the <see
        /// cref="EditorData{T}"/>.
        /// </returns>
        public ReadOnlyCollection<byte> InternalDataAsReadOnly()
        {
            return new ReadOnlyCollection<byte>(InternalData);
        }

        /// <summary>
        /// Removes all elements from the <see cref="EditorData{T}"/>.
        /// </summary>
        public void Clear()
        {
            InternalData.RemoveRange(StartOffset, GetByteCount(Count));
            Version++;
        }

        /// <summary>
        /// Removes all internal byte data from the <see cref="
        /// EditorData{T}"/>.
        /// </summary>
        public void ClearAllData()
        {
            InternalData.Clear();
            Version++;
        }

        /// <summary>
        /// Determines whether an element is in the <see cref="
        /// EditorData{T}"/>.
        /// </summary>
        /// <param name="item">
        /// The object to locate in the <see cref="EditorData{T}"/>.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="item"/> is found in the
        /// <see cref="EditorData{T}"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Contains(T item)
        {
            return IndexOf(item) != -1;
        }

        /// <inheritdoc/>
        ///
        bool IList.Contains(object value)
        {
            return value is T item ? Contains(item) : false;
        }

        /// <summary>
        /// Determines whether an element is in the <see cref="
        /// EditorData{T}"/> starting at any offset in the internal data
        /// structure.
        /// </summary>
        /// <param name="item">
        /// The object to locate in the <see cref="EditorData{T}"/>.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="item"/> is found anywhere
        /// in the internal data of the <see cref=" EditorData{T}"/>; otherwise
        /// <see langword="false"/>.
        /// </returns>
        public bool ContainsAtAnyOffset(T item)
        {
            return OffsetOf(item) != -1;
        }

        /// <summary>
        /// Converts the elements in the current <see cref=" EditorData{T}"/>
        /// to another type, and returns a list containing the converted
        /// elements.
        /// </summary>
        /// <typeparam name="TOutput">
        /// The type of the elements of the target array.
        /// </typeparam>
        /// <param name="converter">
        /// A <see cref="Converter{TInput, TOutput}"/> delegate that converts
        /// each element from one type to another type. <typeparamref
        /// name="TOutput"/>.
        /// </param>
        /// <returns>
        /// A <see cref="List{T}"/> of the target type containing the converted
        /// elements from the current <see cref=" EditorData{T}"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="converter"/> is <see langword="null"/>.
        /// </exception>
        public List<TOutput> ConvertAll<TOutput>(
            Converter<T, TOutput> converter)
        {
            if (converter is null)
            {
                throw new ArgumentNullException(nameof(converter));
            }

            return new List<TOutput>(this.Select(item => converter(item)));
        }

        /// <summary>
        /// Converts the elements at all offsets in the current <see cref="
        /// EditorData{T}"/> to another type, and returns a list containing the
        /// converted elements.
        /// </summary>
        /// <typeparam name="TOutput">
        /// The type of the elements of the target array.
        /// </typeparam>
        /// <param name="converter">
        /// A <see cref="Converter{TInput, TOutput}"/> delegate that converts
        /// each element from one type to another type. <typeparamref
        /// name="TOutput"/>.
        /// </param>
        /// <returns>
        /// A <see cref="List{T}"/> of the target type containing the converted
        /// elements from the current <see cref=" EditorData{T}"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="converter"/> is <see langword="null"/>.
        /// </exception>
        public List<TOutput> ConvertAtAllOffsets<TOutput>(
            Converter<T, TOutput> converter)
        {
            if (converter is null)
            {
                throw new ArgumentNullException(nameof(converter));
            }

            return new List<TOutput>(
                EnumerateAtAllOffsets().Select(item => converter(item)));
        }

        /// <summary>
        /// Copies the entire <see cref="EditorData{T}"/> to a compatible
        /// one-dimensional array, starting at the beginning of the target
        /// array.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="Array"/> that is the destination of
        /// the elements copied from <see cref=" EditorData{T}"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The number of elements in the source <see cref=" EditorData{T}"/>
        /// is greater than the number of elements that the destination <see
        /// cref="Array"/> can contain.
        /// </exception>
        public void CopyTo(T[] array)
        {
            CopyTo(array, 0);
        }

        /// <summary>
        /// Copies the entire <see cref="EditorData{T}"/> to a compatible
        /// one-dimensional array, starting at the specified index of the
        /// target array.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="Array"/> that is the destination of
        /// the elements copied from <see cref=" EditorData{T}"/>.
        /// </param>
        /// <param name="arrayIndex">
        /// The zero-based index in <paramref name="array"/> at which the
        /// copying begins.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="arrayIndex"/> is less than 0.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The number of elements in this <see cref="EditorData{T}"/> is
        /// greater than the available space from <paramref name="
        /// arrayIndex"/> to the end of the destination <paramref name="
        /// array"/>.
        /// </exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            CopyTo(0, array, arrayIndex, Count);
        }

        /// <summary>
        /// Copies a range of elements from the <see cref=" EditorData{T}"/> to
        /// a compatible one-dimensional array, starting at the specified index
        /// of the target array.
        /// </summary>
        /// <param name="index">
        /// The zero-based index in <see cref="EditorData{T}"/> at which
        /// copying begins.
        /// </param>
        /// <param name="array">
        /// The one-dimensional <see cref="Array"/> that is the destination of
        /// the elements copied from <see cref=" EditorData{T}"/>.
        /// </param>
        /// <param name="arrayIndex">
        /// The zero-based index in <paramref name="array"/> at which copying
        /// begins.
        /// </param>
        /// <param name="count">
        /// The number of elements to copy.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than 0.
        /// <para/>
        /// -or-
        /// <para/>
        /// <paramref name="arrayIndex"/> is less than 0.
        /// <para/>
        /// -or-
        /// <para/>
        /// <paramref name="count"/> is less than 0.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="index"/> is greater than the <see cref=" Count"/>
        /// of this <see cref="EditorData{T}"/>.
        /// <para/>
        /// -or-
        /// <para/>
        /// The number of elements from <paramref name="index"/> to the end of
        /// this <see cref="EditorData{T}"/> is greater than the available
        /// space from <paramref name="arrayIndex"/> to the end of the
        /// destination <paramref name="array"/>.
        /// </exception>
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

        /// <inheritdoc/>
        ///
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

        /// <summary>
        /// Gets the elements created by the byte data from every offset usable
        /// in the internal data structure.
        /// </summary>
        /// <returns>
        /// The elements constructed from the byte data starting at each offset
        /// in the internal data structure.
        /// </returns>
        public IEnumerable<T> EnumerateAtAllOffsets()
        {
            return EnumerateAtAllOffsets(0);
        }

        /// <summary>
        /// Gets the elements created by the byte data from every offset usable
        /// in the internal data structure, starting at a specified offset.
        /// </summary>
        /// <param name="start">
        /// The byte offset to begin reading at.
        /// </param>
        /// <returns>
        /// The elements constructed from the byte data starting at each offset
        /// in the internal data structure, starting from <paramref
        /// name="start"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="start"/> is outside the range of the internal data
        /// collection.
        /// </exception>
        public IEnumerable<T> EnumerateAtAllOffsets(int start)
        {
            return EnumerateAtAllOffsets(start, InternalDataSize - start);
        }

        /// <summary>
        /// Gets the elements created by the byte data from every offset usable
        /// in the internal data structure, within a specified range.
        /// </summary>
        /// <param name="startOffset">
        /// The byte offset to begin reading at.
        /// </param>
        /// <param name="count">
        /// The total number of bytes to read.
        /// </param>
        /// <returns>
        /// The elements constructed from the byte data starting at each offset
        /// in the internal data structure, starting from <paramref
        /// name="startOffset"/> and going up to <paramref name=" count"/>
        /// bytes.
        /// </returns>
        public IEnumerable<T> EnumerateAtAllOffsets(int startOffset, int count)
        {
            if ((uint)startOffset > (uint)InternalDataSize)
            {
                throw new ArgumentOutOfRangeException();
            }

            if ((uint)count > (uint)InternalDataSize)
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

                yield return GetItem(InternalData.Skip(i));
            }
        }

        /// <summary>
        /// Determines whether the <see cref="EditorData{T}"/> contains
        /// elements that match the conditions defined by the specified
        /// predicate.
        /// </summary>
        /// <param name="match">
        /// The <see cref="Predicate{T}"/> delegate that defines the conditions
        /// of the elements to search for.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="EditorData{T}"/> contains
        /// one more elements that match the conditions defined by <paramref
        /// name="match"/>; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="match"/> is <see langword="null"/>.
        /// </exception>
        public bool Exists(Predicate<T> match)
        {
            return FindIndex(match) != -1;
        }

        /// <summary>
        /// Determines whether the <see cref="EditorData{T}"/> contains
        /// elements at any internal data offset that match the conditions
        /// defined by the specified predicate.
        /// </summary>
        /// <param name="match">
        /// The <see cref="Predicate{T}"/> delegate that defines the conditions
        /// of the elements to search for.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="EditorData{T}"/> contains
        /// one more elements at any internal data offset that match the
        /// conditions defined by <paramref name="match"/>; otherwise, <see
        /// langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="match"/> is <see langword="null"/>.
        /// </exception>
        public bool ExistsAtAnyOffset(Predicate<T> match)
        {
            return FindOffset(match) != -1;
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the
        /// specified predicate, and returns the first occurrence within the
        /// entire <see cref="EditorData{T}"/>.
        /// </summary>
        /// <param name="match">
        /// The <see cref="Predicate{T}"/> delegate that defines the conditions
        /// of the elements to search for.
        /// </param>
        /// <returns>
        /// The first element that matches the conditions defined by <paramref
        /// name="match"/>, if found; otherwise, the default value for type
        /// <typeparamref name="T"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="match"/> is <see langword="null"/>.
        /// </exception>
        public T Find(Predicate<T> match)
        {
            return FindInternal(match, this);
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the
        /// specified predicate, and returns the first occurrence at any offset
        /// within the entire <see cref="EditorData{T}"/>.
        /// </summary>
        /// <param name="match">
        /// The <see cref="Predicate{T}"/> delegate that defines the conditions
        /// of the elements to search for.
        /// </param>
        /// <returns>
        /// The first element at any offset that matches the conditions defined
        /// by <paramref name="match"/>, if found; otherwise the default value
        /// for type <typeparamref name="T"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="match"/> is <see langword="null"/>.
        /// </exception>
        public T FindAtAnyOffset(Predicate<T> match)
        {
            return FindInternal(match, EnumerateAtAllOffsets());
        }

        private static T FindInternal(
            Predicate<T> match,
            IEnumerable<T> collection)
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

        /// <summary>
        /// Retrieves all the elements that match the conditions defined by the
        /// specified predicate.
        /// </summary>
        /// <param name="match">
        /// The <see cref="Predicate{T}"/> delegate that defines the conditions
        /// of the elements to search for.
        /// </param>
        /// <returns>
        /// A <see cref="List{T}"/> containing all the elements that match the
        /// conditions defined by <paramref name="match"/>, if found;
        /// otherwise, an empty <see cref="List{T}"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="match"/> is <see langword="null"/>.
        /// </exception>
        public List<T> FindAll(Predicate<T> match)
        {
            return FindAllInternal(match, this);
        }

        /// <summary>
        /// Retrieves all the elements at every byte offset that match the
        /// conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">
        /// The <see cref="Predicate{T}"/> delegate that defines the conditions
        /// of the elements to search for.
        /// </param>
        /// <returns>
        /// A <see cref="List{T}"/> containing all the elements at every byte
        /// offset that match the conditions defined by <paramref
        /// name="match"/>, if found; otherwise, an empty <see
        /// cref="List{T}"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="match"/> is <see langword="null"/>.
        /// </exception>
        public List<T> FindAtAllOffsets(Predicate<T> match)
        {
            return FindAllInternal(match, EnumerateAtAllOffsets());
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

        /// <summary>
        /// Searches for an element that matches the conditions defined by the
        /// specified predicate, and returns the zero-based index of the first
        /// occurrence within the entire <see cref=" EditorData{T}"/>.
        /// </summary>
        /// <param name="match">
        /// The <see cref="Predicate{T}"/> delegate that defines the conditions
        /// of the elements to search for.
        /// </param>
        /// <returns>
        /// The zero-based index of the first occurrence of an element that
        /// matches the conditions defined by <paramref name=" match"/>, if
        /// found; otherwise, -1.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="match"/> is <see langword="null"/>.
        /// </exception>
        public int FindIndex(Predicate<T> match)
        {
            return FindIndex(0, Count, match);
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the
        /// specified predicate, and returns the zero-based index of the first
        /// occurrence within the range of elements in the <see
        /// cref="EditorData{T}"/> that extends from the specified index to the
        /// last element.
        /// </summary>
        /// <param name="startIndex">
        /// The zero-based starting index of the search.
        /// </param>
        /// <param name="match">
        /// The <see cref="Predicate{T}"/> delegate that defines the conditions
        /// of the elements to search for.
        /// </param>
        /// <returns>
        /// The zero-based index of the first occurrence of an element that
        /// matches the conditions defined by <paramref name=" match"/>, if
        /// found; otherwise, -1.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="match"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="startIndex"/> is outside the range of valid indexes
        /// for the <see cref="List{T}"/>.
        /// </exception>
        public int FindIndex(int startIndex, Predicate<T> match)
        {
            return FindIndex(startIndex, Count - startIndex, match);
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the
        /// specified predicate, and returns the zero-based index of the first
        /// occurrence within the range of elements in the <see
        /// cref="EditorData{T}"/> that starts at the specified index and
        /// contains the specified number of elements.
        /// </summary>
        /// <param name="startIndex">
        /// The zero-based starting index of the search.
        /// </param>
        /// <param name="count">
        /// The number of elements in the section to search.
        /// </param>
        /// <param name="match">
        /// The <see cref="Predicate{T}"/> delegate that defines the conditions
        /// of the elements to search for.
        /// </param>
        /// <returns>
        /// The zero-based index of the first occurrence of an element that
        /// matches the conditions defined by <paramref name=" match"/>, if
        /// found; otherwise, -1.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="match"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="startIndex"/> is outside the range of valid indexes
        /// for the <see cref="List{T}"/>.
        /// <para/>
        /// -or-
        /// <para/>
        /// <paramref name="count"/> is less than 0.
        /// <para/>
        /// -or-
        /// <para/>
        /// <paramref name="startIndex"/> and <paramref name="count"/> do not
        /// specify a valid section in the <see cref=" EditorData{T}"/>.
        /// </exception>
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

        /// <summary>
        /// Searches every offset for an element that matches the conditions
        /// defined by the specified predicate, and returns the offset of the
        /// first occurrence within the entire <see cref=" EditorData{T}"/>.
        /// </summary>
        /// <param name="match">
        /// The <see cref="Predicate{T}"/> delegate that defines the conditions
        /// of the elements to search for.
        /// </param>
        /// <returns>
        /// The zero-based index of the first occurrence of an element that
        /// matches the conditions defined by <paramref name=" match"/>, if
        /// found; otherwise, -1.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="match"/> is <see langword="null"/>.
        /// </exception>
        public int FindOffset(Predicate<T> match)
        {
            return FindOffset(0, match);
        }

        /// <summary>
        /// Searches every offset for an element that matches the conditions
        /// defined by the specified predicate, and returns the offset of the
        /// first occurrence within the range of elements in the <see
        /// cref="EditorData{T}"/> that extends from the specified index to the
        /// last element.
        /// </summary>
        /// <param name="startOffset">
        /// The starting offset of the search.
        /// </param>
        /// <param name="match">
        /// The <see cref="Predicate{T}"/> delegate that defines the conditions
        /// of the elements to search for.
        /// </param>
        /// <returns>
        /// The offset of the first occurrence of an element that matches the
        /// conditions defined by <paramref name=" match"/>, if found;
        /// otherwise, -1.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="match"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="startOffset"/> is outside the range of valid
        /// offsets.
        /// </exception>
        public int FindOffset(int startOffset, Predicate<T> match)
        {
            return FindOffset(
                startOffset,
                InternalDataSize - startOffset,
                match);
        }

        /// <summary>
        /// Searches every offset for an element that matches the conditions
        /// defined by the specified predicate, and returns the offset of the
        /// first occurrence within the range of elements in the <see
        /// cref="EditorData{T}"/> that starts at the specified offset and
        /// contains the specified number of bytes.
        /// </summary>
        /// <param name="startOffset">
        /// The starting offset of the search.
        /// </param>
        /// <param name="count">
        /// The number of bytes in the section to search.
        /// </param>
        /// <param name="match">
        /// The <see cref="Predicate{T}"/> delegate that defines the conditions
        /// of the elements to search for.
        /// </param>
        /// <returns>
        /// The data offset of the first occurrence of an element that matches
        /// the conditions defined by <paramref name=" match"/>, if found;
        /// otherwise, -1.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="match"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="startOffset"/> is outside the range of valid
        /// offsets.
        /// <para/>
        /// -or-
        /// <para/>
        /// <paramref name="count"/> is less than 0.
        /// <para/>
        /// -or-
        /// <para/>
        /// <paramref name="startOffset"/> and <paramref name="count"/> do not
        /// specify a valid section in the <see cref=" EditorData{T}"/>.
        /// </exception>
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

        /// <summary>
        /// Searches for an element that matches the conditions defined by the
        /// specified predicate, and returns the last occurrence within the
        /// entire <see cref="List{T}"/>.
        /// </summary>
        /// <param name="match">
        /// The <see cref="Predicate{T}"/> delegate that defines the conditions
        /// of the elements to search for.
        /// </param>
        /// <returns>
        /// The last element that matches the conditions defined by the
        /// specified predicate, if found; otherwise, the default value for
        /// type <typeparamref name="T"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="match"/> is <see langword="null"/>.
        /// </exception>
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

        /// <summary>
        /// Searches every offset for an element that matches the conditions
        /// defined by the specified predicate, and returns the last occurrence
        /// within the entire <see cref="List{T}"/>.
        /// </summary>
        /// <param name="match">
        /// The <see cref="Predicate{T}"/> delegate that defines the conditions
        /// of the elements to search for.
        /// </param>
        /// <returns>
        /// The last element at any offset that matches the conditions defined
        /// by the specified predicate, if found; otherwise, the default value
        /// for type <typeparamref name="T"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="match"/> is <see langword="null"/>.
        /// </exception>
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

        /// <summary>
        /// Searches for an element that matches the conditions defined by the
        /// specified predicate, and returns the zero-based index of the last
        /// occurrence within the entire <see cref=" EditorData{T}"/>.
        /// </summary>
        /// <param name="match">
        /// The <see cref="Predicate{T}"/> delegate that defines the conditions
        /// of the elements to search for.
        /// </param>
        /// <returns>
        /// The zero-based index of the last occurrence of an element that
        /// matches the conditions defined by <paramref name=" match"/>, if
        /// found; otherwise, -1.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="match"/> is <see langword="null"/>.
        /// </exception>
        public int FindLastIndex(Predicate<T> match)
        {
            return FindLastIndex(Count - 1, Count, match);
        }

        /// <summary>
        ///Searches for an element that matches the conditions defined
        /// by the specified predicate, and returns the zero-based index
        /// of the last occurrence within the range of elements in the
        /// <see cref="EditorData{T}"/> that extends from the first
        /// element to the specified index.
        /// </summary>
        /// <param name="startIndex">
        /// The zero-based starting index of the backward search.
        /// </param>
        /// <param name="match">
        /// The <see cref="Predicate{T}"/> delegate that defines the
        /// conditions of the elements to search for.
        /// </param>
        /// <returns>
        /// The zero-based index of the last occurrence of an element
        /// that matches the conditions defined by <paramref name="
        /// match"/>, if found; otherwise, -1.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="match"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="startIndex"/> is outside the range of valid
        /// indexes for the <see cref="List{T}"/>.
        /// </exception>
        public int FindLastIndex(int startIndex, Predicate<T> match)
        {
            return FindLastIndex(startIndex, startIndex + 1, match);
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the
        /// specified predicate, and returns the zero-based index of the last
        /// occurrence within the range of elements in the <see
        /// cref="EditorData{T}"/> that contains the specified number of
        /// elements and ends at the specified index.
        /// </summary>
        /// <param name="startIndex">
        /// The zero-based starting index of the backward search.
        /// </param>
        /// <param name="count">
        /// The number of elements in the section to search.
        /// </param>
        /// <param name="match">
        /// The <see cref="Predicate{T}"/> delegate that defines the conditions
        /// of the elements to search for.
        /// </param>
        /// <returns>
        /// The zero-based index of the last occurrence of an element that
        /// matches the conditions defined by <paramref name=" match"/>, if
        /// found; otherwise, -1.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="match"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="startIndex"/> is outside the range of valid indexes
        /// for the <see cref="List{T}"/>.
        /// <para/>
        /// -or-
        /// <para/>
        /// <paramref name="count"/> is less than 0.
        /// <para/>
        /// -or-
        /// <para/>
        /// <paramref name="startIndex"/> and <paramref name="count"/> do not
        /// specify a valid section in the <see cref=" EditorData{T}"/>.
        /// </exception>
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
                throw ValueNotInArrayBounds(
                    nameof(startIndex),
                    startIndex,
                    Count);
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
            return FindLastOffset(InternalDataSize - 1, match);
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

            if (InternalDataSize == 0)
            {
                if (startOffset != -1)
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
            else if ((uint)startOffset >= (uint)InternalDataSize)
            {
                throw ValueNotInArrayBounds(
                    nameof(startOffset),
                    startOffset,
                    Count);
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

        /// <summary>
        /// Performs the specified action on each element of the <see
        /// cref="EditorData{T}"/>.
        /// </summary>
        /// <param name="action">
        /// The <see cref="Action{T}"/> delegate to perform on each element of
        /// the <see cref="EditorData{T}"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action"/> is <see langword="null"/>.
        /// </exception>
        public void ForEach(Action<T> action)
        {
            ForEachInternal(action, this);
        }

        public void ForEachAtAllOffsets(Action<T> action)
        {
            ForEachInternal(action, EnumerateAtAllOffsets());
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

            var version = Version;
            foreach (var item in collection)
            {
                if (Version != version)
                {
                    break;
                }

                action(item);
            }

            if (Version != version)
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="
        /// EditorData{T}"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> for the <see cref="
        /// EditorData{T}"/>.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }

        /// <inheritdoc/>
        ///
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Creates an <see cref="IEnumerable{T}"/> of a range of elements in
        /// the <see cref="EditorData{T}"/>.
        /// </summary>
        /// <param name="index">
        /// The zero-based <see cref="EditorData{T}"/> index at which the range
        /// starts.
        /// </param>
        /// <param name="count">
        /// The number of elements in the range.
        /// </param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> of a range of elements in the
        /// source <see cref="EditorData{T}"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than 0.
        /// <para/>
        /// -or-
        /// <para/>
        /// <paramref name="count"/> is less than 0.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="index"/> and <paramref name="count"/> do not denote
        /// a valid range of elements in the <see cref=" EditorData{T}"/>.
        /// </exception>
        public IEnumerable<T> GetRange(int index, int count)
        {
            return Enumerable.Range(index, count).Select(i => this[i]);
        }

        /// <summary>
        /// Creates a shallow copy of a range of the <see cref="Byte"/> data
        /// that internally represents the <see cref=" EditorData{T}"/>.
        /// </summary>
        /// <param name="index">
        /// The zero-based <see cref="EditorData{T}"/> index at which the range
        /// starts.
        /// </param>
        /// <param name="count">
        /// The number of elements in the range.
        /// </param>
        /// <returns>
        /// A shallow copy of the a range of the <see cref="Byte"/> data that
        /// internally represents the <see cref="EditorData{T}"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than 0.
        /// <para/>
        /// -or-
        /// <para/>
        /// <paramref name="count"/> is less than 0.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="index"/> and <paramref name="count"/> do not denote
        /// a valid range of elements in the <see cref=" EditorData{T}"/>.
        /// </exception>
        public List<byte> GetDataRange(int index, int count)
        {
            var offset = GetOffset(index);
            var end = GetOffset(index + count);
            return InternalData.GetRange(offset, end - offset);
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index
        /// of the first occurrence within the range of elements in the <see
        /// cref="EditorData{T}"/> that extends from the specified index to the
        /// last element.
        /// </summary>
        /// <param name="item">
        /// The object to locate in the <see cref="EditorData{T}"/>.
        /// </param>
        /// <returns>
        /// The zero-based index of the first occurrence of <paramref
        /// name="item"/> within the entire <see cref="EditorData{T}"/>, if
        /// found; otherwise, -1.
        /// </returns>
        public int IndexOf(T item)
        {
            return IndexOf(item, 0);
        }

        /// <inheritdoc/>
        ///
        int IList.IndexOf(object value)
        {
            return value is T item
                ? IndexOf(item)
                : throw new ArgumentException();
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index
        /// of the first occurrence within the range of elements in the <see
        /// cref="EditorData{T}"/> that extends from the specified index to the
        /// last element.
        /// </summary>
        /// <param name="item">
        /// The object to locate in the <see cref="EditorData{T}"/>.
        /// </param>
        /// <param name="index">
        /// The zero-based starting index of the search. 0 (zero) is valid in
        /// an empty list.
        /// </param>
        /// <returns>
        /// The zero-based index of the first occurrence of <paramref
        /// name="item"/> within the range of elements in the <see
        /// cref="EditorData{T}"/> that extends from <paramref name=" index"/>
        /// to the last element, if found; otherwise, -1.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is outside the range of valid indexes for
        /// the <see cref="EditorData{T}"/>.
        /// </exception>
        public int IndexOf(T item, int index)
        {
            return IndexOf(item, index, Count);
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index
        /// of the first occurrence within the range of elements in the <see
        /// cref="EditorData{T}"/> that starts at the specified index and
        /// contains the specified number of elements.
        /// </summary>
        /// <param name="item">
        /// </param>
        /// <param name="index">
        /// The zero-based <see cref="EditorData{T}"/> index at which the range
        /// starts.
        /// </param>
        /// <param name="count">
        /// The number of elements in the range.
        /// </param>
        /// <returns>
        /// The zero-based index of the first occurrence of <paramref
        /// name="item"/> within the range of elements in the <see
        /// cref="EditorData{T}"/> that starts at index and contains <paramref
        /// name="count"/> number of elements, if found; otherwise, -1.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than 0.
        /// <para/>
        /// -or-
        /// <para/>
        /// <paramref name="count"/> is less than 0.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="index"/> and <paramref name="count"/> do not denote
        /// a valid range of elements in the <see cref=" EditorData{T}"/>.
        /// </exception>
        public int IndexOf(T item, int index, int count)
        {
            if (index > Count)
            {
                throw ValueNotInArrayBounds(
                    nameof(index),
                    index,
                    Count);
            }

            if (count < 0 || index > Count - count)
            {
                throw ValueNotInArrayBounds(
                    nameof(count),
                    count,
                    Count);
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

        public int OffsetOf(T item)
        {
            return OffsetOf(item, 0);
        }

        public int OffsetOf(T item, int offset)
        {
            return OffsetOf(item, offset, InternalDataSize - offset);
        }

        public int OffsetOf(T item, int offset, int count)
        {
            if (offset > InternalDataSize)
            {
                throw ValueNotInArrayBounds(
                    nameof(offset),
                    offset,
                    Count);
            }

            if (count < 0 || offset > InternalDataSize - count)
            {
                throw ValueNotInArrayBounds(
                    nameof(count),
                    count,
                    Count);
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

        /// <summary>
        /// Inserts an element into the <see cref="EditorData{T}"/> at the
        /// specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index at which <paramref name="item"/> should be
        /// inserted.
        /// </param>
        /// <param name="item">
        /// The object to insert.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than 0.
        /// <para/>
        /// -or-
        /// <para/>
        /// <paramref name="index"/> is greater than <see cref=" Count"/>.
        /// </exception>
        public void Insert(int index, T item)
        {
            InsertAtOffset(GetOffset(index), item);
        }

        /// <inheritdoc/>
        ///
        void IList.Insert(int index, object value)
        {
            if (value is T item)
            {
                Insert(index, item);
            }

            throw new ArgumentException();
        }

        public void InsertAtOffset(int offset, T item)
        {
            InternalData.InsertRange(
                offset,
                GetByteData(item));
        }

        /// <summary>
        /// Inserts the elements of a collection into the <see cref="
        /// EditorData{T}"/> at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index at which the new elements should be inserted.
        /// </param>
        /// <param name="collection">
        /// The collection whose elements should be inserted into the <see
        /// cref="EditorData{T}"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than 0.
        /// <para/>
        /// -or-
        /// <para/>
        /// <paramref name="index"/> is greater than <see cref=" Count"/>.
        /// </exception>
        public void InsertRange(int index, IEnumerable<T> collection)
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if ((uint)index > (uint)Count)
            {
                throw ValueNotInArrayBounds(nameof(index), index, Count);
            }

            if (collection is ICollection<T> c)
            {
                var dataCollection = new ReadOnlyDataCollection(this, c);
                var offset = GetOffset(index);
                InternalData.InsertRange(offset, dataCollection);
            }
            else
            {
                foreach (var item in collection)
                {
                    Insert(index++, item);
                }
            }

            Version++;
        }

        public void InsertRangeAtOffset(int offset, IEnumerable<T> collection)
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if ((uint)offset > (uint)InternalDataSize)
            {
                throw ValueNotInArrayBounds(
                    nameof(offset),
                    offset,
                    InternalDataSize);
            }

            if (collection is ICollection<T> c)
            {
                var dataCollection = new ReadOnlyDataCollection(this, c);
                InternalData.InsertRange(offset, dataCollection);
            }
            else
            {
                foreach (var item in collection)
                {
                    InsertAtOffset(offset, item);
                    offset += GetSizeOfItem(item);
                }
            }

            Version++;
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index
        /// of the last occurrence within the entire <see cref="
        /// EditorData{T}"/>.
        /// </summary>
        /// <param name="item">
        /// The object to locate in the <see cref="EditorData{T}"/>.
        /// </param>
        /// <returns>
        /// The zero-based index of the last occurrence of <paramref
        /// name="item"/> within the entire <see cref="EditorData{T}"/>, if
        /// found; otherwise, -1.
        /// </returns>
        public int LastIndexOf(T item)
        {
            return Count > 0 ? LastIndexOf(item, Count - 1, Count) : -1;
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index
        /// of the last occurrence within the range of elements in the <see
        /// cref="EditorData{T}"/> that extends from the first element to the
        /// specified index.
        /// </summary>
        /// <param name="item">
        /// The object to locate in the <see cref="EditorData{T}"/>.
        /// </param>
        /// <param name="index">
        /// The zero-based starting index of the backward search.
        /// </param>
        /// <returns>
        /// The zero-based index of the last occurrence of <paramref
        /// name="item"/> within the range of elements in the <see
        /// cref="EditorData{T}"/> that extends from the first element to
        /// <paramref name="index"/>, if found; otherwise, -1.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is outside the range of valid indexes for
        /// the <see cref="EditorData{T}"/>.
        /// </exception>
        public int LastIndexOf(T item, int index)
        {
            if (index >= Count)
            {
                throw ValueNotLessThan(nameof(index), index, Count);
            }

            return LastIndexOf(item, index, index + 1);
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index
        /// of the last occurrence within the range of elements in the <see
        /// cref="EditorData{T}"/> that contains the specified number of
        /// elements and ends at the specified index.
        /// </summary>
        /// <param name="item">
        /// The object to locate in the <see cref="EditorData{T}"/>.
        /// </param>
        /// <param name="index">
        /// The zero-based starting index of the backward search.
        /// </param>
        /// <param name="count">
        /// The number of elements in the range.
        /// </param>
        /// <returns>
        /// The zero-based index of the last occurrence of <paramref
        /// name="item"/> within the range of elements in the <see
        /// cref="EditorData{T}"/> that contains <paramref name=" count"/>
        /// number of elements and ends at <paramref name=" index"/>, if found;
        /// otherwise, -1.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than 0.
        /// <para/>
        /// -or-
        /// <para/>
        /// <paramref name="count"/> is less than 0.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="index"/> and <paramref name="count"/> do not denote
        /// a valid range of elements in the <see cref=" EditorData{T}"/>.
        /// </exception>
        public int LastIndexOf(T item, int index, int count)
        {
            if (Count == 0)
            {
                return -1;
            }

            if ((uint)index > (uint)Count)
            {
                throw ValueNotInArrayBounds(nameof(index), index, Count);
            }

            if (count < 0)
            {
                throw ValueNotGreaterThanEqualTo(nameof(count), count);
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
            return LastOffsetOf(item, InternalDataSize - 1);
        }

        public int LastOffsetOf(T item, int offset)
        {
            return LastOffsetOf(item, offset, offset + 1);
        }

        public int LastOffsetOf(T item, int offset, int count)
        {
            if (InternalDataSize == 0)
            {
                return -1;
            }

            if ((uint)offset > (uint)InternalDataSize)
            {
                throw ValueNotInArrayBounds(
                    nameof(offset),
                    offset,
                    InternalDataSize);
            }

            if (count < 0)
            {
                throw ValueNotGreaterThanEqualTo(nameof(count), count);
            }

            if (InternalDataSize > offset + 1)
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

        /// <summary>
        /// Removes the first occurrence of the specified object from the <see
        /// cref="EditorData{T}"/>.
        /// </summary>
        /// <param name="item">
        /// The object to remove from the <see cref="EditorData{T}"/>.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="item"/> is successfully
        /// removed; otherwise <see langword="false"/>. This method also
        /// returns <see langword="false"/> if <paramref name="item"/> was not
        /// found in the <see cref=" EditorData{T}"/>.
        /// </returns>
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

        /// <inheritdoc/>
        ///
        void IList.Remove(object value)
        {
            if (value is T item)
            {
                Remove(item);
            }
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

        /// <summary>
        /// Removes all the elements that match the conditions defined by the
        /// specified predicate.
        /// </summary>
        /// <param name="match">
        /// The <see cref="Predicate{T}"/> delegate that defines the conditions
        /// of elements to remove.
        /// </param>
        /// <returns>
        /// The number of elements removed from the <see cref="
        /// EditorData{T}"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="match"/> is <see langword="null"/>.
        /// </exception>
        public int RemoveAll(Predicate<T> match)
        {
            var freeIndex = FindIndex(match);
            if (freeIndex == -1)
            {
                return 0;
            }

            for (var current = freeIndex + 1; current < Count; current++)
            {
                if (match(this[current]))
                {
                    continue;
                }

                this[freeIndex++] = this[current];
            }

            var result = Count - freeIndex;
            InternalData.RemoveAt(GetOffset(freeIndex));
            Version++;
            return result;
        }

        public int RemoveAtAllOffsets(Predicate<T> match)
        {
            var freeOffset = FindOffset(match);
            if (freeOffset == -1)
            {
                return 0;
            }

            var size = InternalDataSize;
            for (var current = freeOffset + 1; current < size;)
            {
                if (match(this[current]))
                {
                    current++;
                    continue;
                }

                var item = GetValueAtOffset(current);
                var sizeOfItem = GetSizeOfItemAtOffset(current);
                SetValueAtOffset(freeOffset, item);
                freeOffset += sizeOfItem;
                current += sizeOfItem;
            }

            var result = size - freeOffset;
            InternalData.RemoveAt(freeOffset);
            Version++;
            return result;
        }

        /// <summary>
        /// Removes the element at the specified index of the <see
        /// cref="EditorData{T}"/>.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the element to remove.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than 0.
        /// <para/>
        /// -or-
        /// <para/>
        /// <paramref name="index"/> is equal to or greater than <see
        /// cref="Count"/>.
        /// </exception>
        public void RemoveAt(int index)
        {
            RemoveAtOffset(GetOffset(index));
        }

        public void RemoveAtOffset(int offset)
        {
            var size = GetSizeOfItemAtOffset(offset);
            InternalData.RemoveRange(offset, size);
            Version++;
        }

        /// <summary>
        /// Removes a range of elements from the <see cref=" EditorData{T}"/>.
        /// </summary>
        /// <param name="index">
        /// The zero-based <see cref="EditorData{T}"/> index at which the range
        /// starts.
        /// </param>
        /// <param name="count">
        /// The number of elements in the range.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than 0.
        /// <para/>
        /// -or-
        /// <para/>
        /// <paramref name="count"/> is less than 0.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="index"/> and <paramref name="count"/> do not denote
        /// a valid range of elements in the <see cref=" EditorData{T}"/>.
        /// </exception>
        public void RemoveRange(int index, int count)
        {
            var offset = GetOffset(index);
            var end = GetOffset(index + count);
            InternalData.RemoveRange(offset, end - offset);
            Version++;
        }

        public void RemoveRangeAtOffset(int offset, int count)
        {
            InternalData.RemoveRange(offset, GetOffset(count, offset));
            Version++;
        }

        /// <summary>
        /// Reverses the order of the elements in the entire <see cref="
        /// EditorData{T}"/>.
        /// </summary>
        public void Reverse()
        {
            Reverse(0, Count);
        }

        /// <summary>
        /// Reverses the order of the elements in the specified range.
        /// </summary>
        /// <param name="index">
        /// The zero-based starting index of the range to reverse.
        /// </param>
        /// <param name="count">
        /// The number of elements in range to reverse.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than 0.
        /// <para/>
        /// -or-
        /// <para/>
        /// <paramref name="count"/> is less than 0.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="index"/> and <paramref name="count"/> do not denote
        /// a valid range of elements in the <see cref=" EditorData{T}"/>.
        /// </exception>
        public void Reverse(int index, int count)
        {
            if (index < 0)
            {
                throw ValueNotGreaterThanEqualTo(nameof(index), index);
            }

            if (count < 0)
            {
                throw ValueNotGreaterThanEqualTo(nameof(count), count);
            }

            var end = index + count;
            if (end > Count)
            {
                throw new ArgumentException();
            }

            for (var i = 0; i < count; i++)
            {
                var temp = this[index];
                this[index++] = this[--end];
                this[end] = temp;
            }
            Version++;
        }

        /// <summary>
        /// Copies the elements of the <see cref="EditorData{T}"/> to a new
        /// array.
        /// </summary>
        /// <returns>
        /// An array containing copies of the elements of the <see
        /// cref="EditorData{T}"/>.
        /// </returns>
        public T[] ToArray()
        {
            var array = new T[Count];
            CopyTo(array);
            return array;
        }

        /// <summary>
        /// Sets the capacity to the actual number of elements in the <see
        /// cref="EditorData{T}"/>, if that number is less than a threshold
        /// value.
        /// </summary>
        public void TrimExcess()
        {
            InternalData.TrimExcess();
        }

        /// <summary>
        /// Determines whether every element in the <see cref="
        /// EditorData{T}"/> matches the conditions defined by the specified
        /// predicate.
        /// </summary>
        /// <param name="match">
        /// The <see cref="Predicate{T}"/> delegate the defines the conditions
        /// to check against the elements.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if every element in the <see cref="
        /// EditorData{T}"/> matches the conditions defined by the specified
        /// predicate; otherwise, <see langword="false"/>. If the list has no
        /// elements, the return value is <see langword="true"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="match"/> is <see langword="null"/>.
        /// </exception>
        public bool TrueForAll(Predicate<T> match)
        {
            return TrueForAllInternal(match, this);
        }

        public bool TrueForAllOffsets(Predicate<T> match)
        {
            return TrueForAllInternal(match, EnumerateAtAllOffsets());
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

        /// <summary>
        /// Gets the element at the specified <see cref="Byte"/> offset.
        /// </summary>
        /// <param name="offset">
        /// The zero-based index of the internal data storage.
        /// </param>
        /// <returns>
        /// The element constructed by the byte data at <paramref name="
        /// offset"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// The number of bytes required to construct an instance of
        /// <typeparamref name="T"/> exceeds <see cref="InternalDataSize"/>
        /// starting at <paramref name="offset"/>.
        /// </exception>
        public virtual T GetValueAtOffset(int offset)
        {
            return GetItem(Skip());

            IEnumerable<byte> Skip()
            {
                for (var i = offset; i < InternalDataSize; i++)
                {
                    yield return InternalData[i];
                }
            }
        }

        /// <summary>
        /// Sets the element at the specified <see cref="Byte"/> offset.
        /// </summary>
        /// <param name="offset">
        /// The zero-based index of the internal data storage.
        /// </param>
        /// <param name="value">
        /// The value to convert to byte data and set at <paramref
        /// name="offset"/>.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The number of bytes required to construct an instance of
        /// <typeparamref name="T"/> exceeds <see cref="InternalDataSize"/>
        /// starting at <paramref name="offset"/>.
        /// </exception>
        public virtual void SetValueAtOffset(int offset, T value)
        {
            foreach (var item in GetByteData(value))
            {
                InternalData[offset++] = item;
            }

            Version++;
        }

        /// <summary>
        /// Gets the number of bytes required to get a list index from <see
        /// cref="StartOffset"/>.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the object in <see cref=" EditorData{T}"/>
        /// to get the byte offset of.
        /// </param>
        /// <returns>
        /// The number of bytes <paramref name="index"/> is from <see
        /// cref="StartOffset"/> in the internal data array.
        /// </returns>
        public int GetByteCount(int index)
        {
            return GetByteCount(index, StartOffset);
        }

        /// <summary>
        /// Gets the number of bytes between a start offset and the offset of a
        /// zero-based index in the <see cref=" EditorData{T}"/>.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the object in <see cref=" EditorData{T}"/>
        /// to get the byte offset of.
        /// </param>
        /// <param name="startOffset">
        /// The starting offset to begin reading from in the internal data
        /// array.
        /// </param>
        /// <returns>
        /// The number of bytes <paramref name="index"/> is from <paramref
        /// name="startOffset"/> in the internal data array.
        /// </returns>
        public int GetByteCount(int index, int startOffset)
        {
            return GetOffset(index, startOffset) - startOffset;
        }

        /// <summary>
        /// Gets the zero-based index of the object that contains the byte at a
        /// specified offset.
        /// </summary>
        /// <param name="offset">
        /// The offset of the byte to get the object index of.
        /// </param>
        /// <returns>
        /// The index of the object in <see cref="EditorData{T}"/> that
        /// contains the byte at <paramref name="offset"/>.
        /// </returns>
        public int GetIndex(int offset)
        {
            return GetIndex(offset, StartOffset);
        }

        /// <summary>
        /// Gets the zero-based index of the object that contains the byte at a
        /// specified offset.
        /// </summary>
        /// <param name="offset">
        /// The offset of the byte to get the object index of.
        /// </param>
        /// <param name="startOffset">
        /// The starting offset to begin reading from in the internal data
        /// array.
        /// </param>
        /// <returns>
        /// The index of the object in <see cref="EditorData{T}"/> that
        /// contains the byte at <paramref name="offset"/>.
        /// </returns>
        public abstract int GetIndex(int offset, int startOffset);

        /// <summary>
        /// Gets the size, in bytes, of the item described by the internal data
        /// starting at a given offset.
        /// </summary>
        /// <param name="offset">
        /// The offset of the byte data to get the size of.
        /// </param>
        /// <returns>
        /// The size, in bytes, of the object described at <paramref
        /// name="offset"/>.
        /// </returns>
        public abstract int GetSizeOfItemAtOffset(int offset);

        /// <summary>
        /// Gets the size, in bytes, of an element after its converted to byte
        /// data.
        /// </summary>
        /// <param name="item">
        /// The element to get the size of
        /// </param>
        /// <returns>
        /// The size, in bytes, of the byte representation of <paramref
        /// name="item"/>.
        /// </returns>
        public abstract int GetSizeOfItem(T item);

        /// <summary>
        /// Get the byte offset given a list index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the object in <see cref=" EditorData{T}"/>
        /// to get the byte offset of.
        /// </param>
        /// <returns>
        /// The byte offset of the internal data array where the object at
        /// <paramref name="index"/> is (or would be if it does not exist).
        /// </returns>
        public int GetOffset(int index)
        {
            return GetOffset(index, StartOffset);
        }

        /// <summary>
        /// Gets the offset in the internal data array of a list index given
        /// the start offset.
        /// </summary>
        /// <param name="index">
        /// The zero-based list index to get the offset of.
        /// </param>
        /// <param name="startOffset">
        /// The starting offset to begin reading from in the internal data
        /// array.
        /// </param>
        /// <returns>
        /// The offset of <paramref name="index"/> starting at <paramref
        /// name="startOffset"/>.
        /// </returns>
        public abstract int GetOffset(int index, int startOffset);

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> of bytes that make up an
        /// object in the <see cref="EditorData{T}"/>.
        /// </summary>
        /// <param name="item">
        /// The object to convert to bytes
        /// </param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> of the bytes that define <paramref
        /// name="item"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="item"/> is <see langword="null"/>.
        /// </exception>
        protected abstract IEnumerable<byte> GetByteData(T item);

        /// <summary>
        /// Gets an instance of <typeparamref name="T"/> constructed from an
        /// <see cref="IEnumerable{T}"/> of bytes.
        /// </summary>
        /// <param name="data">
        /// The byte data to read from.
        /// </param>
        /// <returns>
        /// An instance of <typeparamref name="T"/> constructed from <paramref
        /// name="data"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="data"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="data"/> cannot properly construct the desired
        /// object.
        /// </exception>
        protected abstract T GetItem(IEnumerable<byte> data);

        /// <summary>
        /// Specifies a collection of bytes represented as a collection of the
        /// data type of an instance of <see cref="EditorData{T}"/>.
        /// </summary>
        private class ReadOnlyDataCollection :
            ICollection<byte>,
            IReadOnlyCollection<byte>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="
            /// ReadOnlyCollection{T}"/> class.
            /// </summary>
            /// <param name="editorData">
            /// The parent <see cref="EditorData"/> that created this instance.
            /// </param>
            /// <param name="baseCollection">
            /// The higher-level collection to read from.
            /// </param>
            public ReadOnlyDataCollection(
                EditorData<T> editorData,
                ICollection<T> baseCollection)
            {
                EditorData = editorData
                    ?? throw new ArgumentNullException(nameof(editorData));

                BaseCollection = baseCollection
                    ?? throw new ArgumentNullException(nameof(baseCollection));
            }

            /// <inheritdoc cref="ICollection{T}.Count"/>
            ///
            public int Count
            {
                get
                {
                    return EditorData.GetByteCount(BaseCollection.Count);
                }
            }

            /// <inheritdoc/>
            ///
            bool ICollection<byte>.IsReadOnly
            {
                get
                {
                    return true;
                }
            }

            /// <summary>
            /// Gets the parent <see cref="EditorData"/> that created this
            /// instance.
            /// </summary>
            private EditorData<T> EditorData
            {
                get;
            }

            /// <summary>
            /// Gets the higher-level collection to read from.
            /// </summary>
            private ICollection<T> BaseCollection
            {
                get;
            }

            /// <inheritdoc/>
            ///
            void ICollection<byte>.Add(byte item)
            {
                throw new NotSupportedException();
            }

            /// <inheritdoc/>
            ///
            void ICollection<byte>.Clear()
            {
                throw new NotSupportedException();
            }

            /// <inheritdoc/>
            ///
            bool ICollection<byte>.Remove(byte item)
            {
                throw new NotSupportedException();
            }

            /// <inheritdoc cref="ICollection{T}.Contains(T)"/>
            ///
            public bool Contains(byte item)
            {
                foreach (var x in this)
                {
                    if (x == item)
                    {
                        return true;
                    }
                }

                return false;
            }

            /// <inheritdoc cref="ICollection{T}.CopyTo(T[], Int32)"/>
            ///
            public void CopyTo(byte[] array, int index)
            {
                if (array is null)
                {
                    throw new ArgumentNullException(nameof(array));
                }

                var count = Count;
                if (index + count < array.Length)
                {
                    throw new ArgumentException();
                }

                foreach (var x in this)
                {
                    array[index++] = x;
                }
            }

            /// <inheritdoc/>
            ///
            public IEnumerator<byte> GetEnumerator()
            {
                foreach (var item in BaseCollection)
                {
                    foreach (var x in EditorData.GetByteData(item))
                    {
                        yield return x;
                    }
                }
            }

            /// <inheritdoc/>
            ///
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
