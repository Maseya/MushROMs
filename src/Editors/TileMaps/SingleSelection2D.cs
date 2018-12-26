// <copyright file="SingleSelection2D.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Editors.TileMaps
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;

    /// <summary>
    /// Specifies a selection of a single <see cref="ITileMap2D"/> tile.
    /// </summary>
    public sealed class SingleSelection2D : Selection2D
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="
        /// SingleSelection2D"/> class.
        /// </summary>
        /// <param name="position">
        /// The index of the selected location of this <see cref="
        /// SingleSelection2D"/>.
        /// </param>
        public SingleSelection2D(Point position)
            : base(position)
        {
        }

        /// <summary>
        /// Gets the number of selected tiles in this <see cref="
        /// SingleSelection2D"/>.
        /// </summary>
        public override int Count
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// Creates a new instance of <see cref="SingleSelection2D"/>
        /// that has the same selection properties as this instance.
        /// </summary>
        /// <returns>
        /// A copy of this <see cref="SingleSelection2D"/>.
        /// </returns>
        public override Selection2D Copy()
        {
            return new SingleSelection2D(StartPosition);
        }

        /// <summary>
        /// Determines whether a data grid index is part of this
        /// <see cref="SingleSelection2D"/>.
        /// </summary>
        /// <param name="position">
        /// The <see cref="ITileMap2D"/> grid location to inspect.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="position"/> is
        /// in this <see cref="SingleSelection2D"/>; otherwise
        /// <see langword="false"/>.
        /// </returns>
        public override bool Contains(Point position)
        {
            return position == StartPosition;
        }

        /// <summary>
        /// Returns an enumerator that enumerates through the
        /// <see cref="SingleSelection2D"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator{T}"/> for the <see cref="
        /// SingleSelection2D"/>.
        /// </returns>
        public override IEnumerator<Point> GetEnumerator()
        {
            yield return StartPosition;
        }
    }
}
