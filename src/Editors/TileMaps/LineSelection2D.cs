// <copyright file="LineSelection2D.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Editors.TileMaps
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using static Helper.ThrowHelper;

    /// <summary>
    /// Specifies a linear selection of <see cref="ITileMap2D"/> tiles.
    /// </summary>
    public sealed class LineSelection2D : Selection2D
    {
        /// <summary>
        /// Initializes a new instance of the <see cref=" LineSelection2D"/>
        /// class.
        /// </summary>
        /// <param name="startPosition">
        /// The location of the first selected tile of this <see cref="
        /// LineSelection2D"/>.
        /// </param>
        /// <param name="regionWidth">
        /// The width of the data grid.
        /// </param>
        /// <param name="length">
        /// The length of the linear selection.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="length"/> is less than zero.
        /// <para/>
        /// -or-
        /// <para/>
        /// <paramref name="regionWidth"/> is less than zero.
        /// </exception>
        public LineSelection2D(
            Point startPosition,
            int regionWidth,
            int length)
            : base(startPosition)
        {
            if (length < 0)
            {
                throw ValueNotGreaterThanEqualTo(
                    nameof(length),
                    length);
            }

            if (regionWidth <= 0)
            {
                throw ValueNotGreaterThan(
                    nameof(regionWidth),
                    regionWidth);
            }

            Length = length;
        }

        /// <summary>
        /// Gets the number of selected tiles in this <see cref="
        /// LineSelection2D"/>.
        /// </summary>
        public override int Count
        {
            get
            {
                return Length;
            }
        }

        /// <summary>
        /// Gets the length of the linear selection.
        /// </summary>
        private int Length
        {
            get;
        }

        /// <summary>
        /// Gets the width of the data grid.
        /// </summary>
        private int RegionWidth
        {
            get;
        }

        /// <summary>
        /// Creates a new instance of <see cref="LineSelection1D"/> that has
        /// the same selection properties as this instance.
        /// </summary>
        /// <returns>
        /// A copy of this <see cref="LineSelection1D"/>.
        /// </returns>
        public override Selection2D Copy()
        {
            return new LineSelection2D(StartPosition, RegionWidth, Length);
        }

        /// <summary>
        /// Determines whether a data grid index is part of this <see
        /// cref="LineSelection2D"/>.
        /// </summary>
        /// <param name="position">
        /// The <see cref="ITileMap2D"/> grid location to inspect.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="position"/> is in this
        /// <see cref="LineSelection2D"/>; otherwise <see langword="false"/>.
        /// </returns>
        public override bool Contains(Point position)
        {
            var x = position.X - StartPosition.X;
            var y = position.Y - StartPosition.Y;
            var index = (y * RegionWidth) + x;
            return index >= 0 && index < Length;
        }

        /// <summary>
        /// Returns an enumerator that enumerates through the <see
        /// cref="SingleSelection2D"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator{T}"/> for the <see cref="
        /// SingleSelection2D"/>.
        /// </returns>
        public override IEnumerator<Point> GetEnumerator()
        {
            var start = TileMap1D.GetGridTile(StartPosition, RegionWidth);
            var end = start + Length;
            for (var i = start; i < end; i++)
            {
                yield return TileMap1D.GetViewTile(i, RegionWidth);
            }
        }
    }
}
