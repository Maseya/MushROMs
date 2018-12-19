// <copyright file="ISelection2D.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Editors.TileMaps
{
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
    }
}
