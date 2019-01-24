// <copyright file="ListSelection.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Editors.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public abstract class ListSelection :
        IListSelection
    {
        public abstract int MinIndex
        {
            get;
        }

        public abstract int MaxIndex
        {
            get;
        }

        public abstract int Count
        {
            get;
        }

        public abstract int this[int index]
        {
            get;
        }

        public abstract ListSelection Move(int offset);

        public ListSelection Copy()
        {
            return Move(0);
        }

        public abstract bool Contains(int index);

        public IEnumerable<T> EnumerateValues<T>(IReadOnlyList<T> list)
        {
            if (MaxIndex >= list.Count)
            {
                throw new InvalidOperationException();
            }

            foreach (var index in this)
            {
                yield return list[index];
            }
        }

        public IEnumerable<KeyValuePair<int, T>> EnumerateIndexValues<T>(
            IReadOnlyList<T> list)
        {
            if (MaxIndex >= list.Count)
            {
                throw new InvalidOperationException();
            }

            foreach (var index in this)
            {
                yield return new KeyValuePair<int, T>(index, list[index]);
            }
        }

        public Dictionary<int, T> GetValueDictionary<T>(IReadOnlyList<T> list)
        {
            var result = new Dictionary<int, T>(list.Count);
            foreach (var kvp in EnumerateIndexValues(list))
            {
                result.Add(kvp.Key, kvp.Value);
            }

            return result;
        }

        public abstract IEnumerator<int> GetEnumerator();

        IListSelection IListSelection.Move(int offset)
        {
            return Move(offset);
        }

        IListSelection IListSelection.Copy()
        {
            return Copy();
        }

        IDictionary<int, T> IListSelection.GetValueDictionary<T>(
            IReadOnlyList<T> list)
        {
            return GetValueDictionary(list);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
