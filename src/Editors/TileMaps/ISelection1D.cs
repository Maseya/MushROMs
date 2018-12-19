// <copyright file="ISelection1D.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Editors.TileMaps
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides properties and methods for making a selection of data
    /// in a <see cref="ITileMap1D"/> instance.
    /// </summary>
    public interface ISelection1D : IReadOnlyCollection<int>
    {
        /// <summary>
        /// Gets the index of the first selected tile of this
        /// <see cref="ISelection1D"/>.
        /// </summary>
        int StartIndex { get; }

        /// <summary>
        /// Determines whether a data grid index is part of this
        /// <see cref="ISelection1D"/>.
        /// </summary>
        /// <param name="index">
        /// The <see cref="ITileMap1D"/> grid index to inspect.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="index"/> is
        /// in this <see cref="ISelection1D"/>; otherwise
        /// <see langword="false"/>.
        /// </returns>
        bool Contains(int index);

        /// <summary>
        /// Creates a new instance of <see cref="ISelection1D"/> that
        /// has the same selection properties as this instance.
        /// </summary>
        /// <returns>
        /// A copy of this <see cref="ISelection1D"/>.
        /// </returns>
        ISelection1D Copy();
    }
}
