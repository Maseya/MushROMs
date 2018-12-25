// <copyright file="ISelection2D.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Editors.TileMaps
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    /// <summary>
    /// Provides properties and methods for making a selection of data
    /// in an <see cref="ITileMap2D"/> instance.
    /// </summary>
    public interface ISelection2D : IReadOnlyCollection<Point>
    {
        /// <summary>
        /// Gets the location of the first selected tile of this
        /// <see cref="ISelection2D"/>.
        /// </summary>
        Point StartPosition { get; }

        /// <summary>
        /// Determines whether a data grid index is part of this
        /// <see cref="ISelection2D"/>.
        /// </summary>
        /// <param name="point">
        /// The <see cref="ITileMap2D"/> grid location to inspect.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="point"/> is
        /// in this <see cref="ISelection2D"/>; otherwise
        /// <see langword="false"/>.
        /// </returns>
        bool Contains(Point point);

        /// <summary>
        /// Creates a new instance of <see cref="ISelection2D"/> that
        /// has the same selection properties as this instance.
        /// </summary>
        /// <returns>
        /// A copy of this <see cref="ISelection2D"/>.
        /// </returns>
        ISelection2D Copy();

        /// <summary>
        /// Create a data dictionary of the values at the indexes of a
        /// specified list.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the values in the dictionary.
        /// </typeparam>
        /// <param name="collection">
        /// An implementation of <see cref="IReadOnlyList{T}"/> to
        /// retrieve values from indexes specified by the <see cref="
        /// ISelection2D"/>. If the index is outside the bounds of
        /// <paramref name="collection"/>, the default value of
        /// <typeparamref name="T"/> will be used.
        /// </param>
        /// <param name="width">
        /// The width of the 2D area representing <paramref name="
        /// collection"/>.
        /// </param>
        /// <returns>
        /// An implementation of <see cref="IDictionary{TKey, TValue}"/>
        /// whose keys are the integer indexes of the <see cref="
        /// ISelection2D"/> and whose values are the values of
        /// <paramref name="collection"/> at the corresponding index.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection"/> is <see langword="null"/>.
        /// </exception>
        IDictionary<Point, T> GetValues<T>(
            IReadOnlyList<T> collection,
            int width);
    }
}
