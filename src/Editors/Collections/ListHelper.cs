// <copyright file="ListHelper.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Editors.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public static class ListHelper
    {
        public static ReadOnlyCollection<T> AsReadOnly<T>(this IList<T> list)
        {
            return new ReadOnlyCollection<T>(list);
        }

        public static int FindIndex<T>(
            this IReadOnlyList<T> list,
            IListSelection selection,
            Predicate<T> match)
        {
            if (list is null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (selection is null)
            {
                throw new ArgumentNullException(nameof(selection));
            }

            if (match is null)
            {
                throw new ArgumentNullException(nameof(match));
            }

            foreach (var index in selection)
            {
                if ((uint)index >= list.Count)
                {
                    continue;
                }

                if (match(list[index]))
                {
                    return index;
                }
            }

            return -1;
        }

        public static int FindLastIndex<T>(
            this IReadOnlyList<T> list,
            IListSelection selection,
            Predicate<T> match)
        {
            if (list is null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (selection is null)
            {
                throw new ArgumentNullException(nameof(selection));
            }

            if (match is null)
            {
                throw new ArgumentNullException(nameof(match));
            }

            for (var i = selection.Count; --i >= 0;)
            {
                var index = selection[i];
                if ((uint)index >= list.Count)
                {
                    continue;
                }

                if (match(list[index]))
                {
                    return index;
                }
            }

            return -1;
        }

        public static IEnumerable<int> FindAllIndexes<T>(
            this IReadOnlyList<T> list,
            Predicate<T> match)
        {
            if (list is null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (match is null)
            {
                throw new ArgumentNullException(nameof(match));
            }

            for (var i = 0; i < list.Count; i++)
            {
                if (match(list[i]))
                {
                    yield return i;
                }
            }
        }

        public static int IndexOf<T>(
            this IReadOnlyList<T> list,
            T item,
            IListSelection selection)
        {
            return FindIndex(list, selection, x => Equals(x, item));
        }

        public static int LastIndexOf<T>(
            this IReadOnlyList<T> list,
            T item,
            IListSelection selection)
        {
            return FindLastIndex(list, selection, x => Equals(x, item));
        }

        public static ListSelectionData<T> GetSelectionData<T>(
            this IReadOnlyList<T> list,
            IListSelection selection)
        {
            return new ListSelectionData<T>(selection, list);
        }

        public static void ClearSelection<T>(
            this List<T> list,
            IListSelection selection)
        {
            TransformSelection(list, selection, _ => default);
        }

        public static void TransformSelection<T>(
            this List<T> list,
            IListSelection selection,
            Func<T, T> func)
        {
            if (list is null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (selection is null)
            {
                throw new ArgumentNullException(nameof(selection));
            }

            if (func is null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            var values = new ListSelectionData<T>(selection);
            foreach (var index in selection)
            {
                values[index] = func(list[index]);
            }

            list.WriteSelection(values);
        }

        public static void WriteSelection<T>(
            this List<T> list,
            IListSelectionData<T> values)
        {
            if (list is null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (values.Selection.MaxIndex >= list.Count)
            {
                throw new ArgumentException();
            }

            foreach (var kvp in values)
            {
                list[kvp.Key] = kvp.Value;
            }
        }

        public static void InsertSelection<T>(
            this IList<T> list,
            IListSelectionData<T> values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (values.Selection.MaxIndex >= list.Count)
            {
                throw new ArgumentException();
            }

            IncreaseSize(list, values.Count);

            var current = list.Count;
            for (var i = values.Count; --i >= 0;)
            {
                var freeIndex = values.Selection[i];
                while (--current != freeIndex)
                {
                    list[current] = list[current - (i + 1)];
                }

                list[current] = values[freeIndex];
            }
        }

        public static void RemoveSelection<T>(
            this IList<T> list,
            IListSelection selection)
        {
            if (selection is null)
            {
                throw new ArgumentNullException(nameof(selection));
            }

            if (selection.MaxIndex >= list.Count)
            {
                throw new ArgumentException();
            }

            var freeIndex = selection.MinIndex;
            var current = freeIndex + 1;
            while (current < list.Count)
            {
                while (current < list.Count && selection.Contains(current))
                {
                    current++;
                }

                if (current < list.Count)
                {
                    list[freeIndex++] = list[current++];
                }
            }

            DecreaseSize(list, list.Count - freeIndex);
        }

        private static void IncreaseSize<T>(IList<T> list, int amount)
        {
            if (list is List<T> l)
            {
                l.AddRange(new T[amount]);
            }
            else
            {
                for (var i = 0; i < amount; i++)
                {
                    list.Add(default);
                }
            }
        }

        private static void DecreaseSize<T>(IList<T> list, int amount)
        {
            if (list is List<T> l)
            {
                l.RemoveRange(l.Count - amount, amount);
            }
            else
            {
                for (var i = 1; i <= amount; i++)
                {
                    list.RemoveAt(list.Count - i);
                }
            }
        }
    }
}
