// <copyright file="IListSelection.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Editors.Collections
{
    using System.Collections.Generic;

    public interface IListSelection : IReadOnlyList<int>
    {
        int MinIndex
        {
            get;
        }

        int MaxIndex
        {
            get;
        }

        bool ContainsIndex(int index);

        IListSelection CopySelection();

        IListSelection MoveSelection(int offset);

        IEnumerable<T> EnumerateValues<T>(IReadOnlyList<T> list);

        IEnumerable<(int index, T value)> EnumerateIndexValues<T>(
            IReadOnlyList<T> list);

        IDictionary<int, T> GetValueDictionary<T>(IReadOnlyList<T> list);
    }
}
