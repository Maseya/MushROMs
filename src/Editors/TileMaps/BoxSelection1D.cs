// <copyright file="BoxSelection1D.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved. Licensed under
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
    /// Specifies a rectangular selection of <see cref="ITileMap1D"/> tiles.
    /// </summary>
    public sealed class BoxSelection1D : Selection1D
    {
        /// <summary>
        /// Initializes a new instance of the <see cref=" BoxSelection1D"/>
        /// class.
        /// </summary>
        /// <param name="startIndex">
        /// The index of the first selected tile.
        /// </param>
        /// <param name="regionWidth">
        /// The width of the data grid.
        /// </param>
        /// <param name="size">
        /// The size of the selection.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// One of the properties of <paramref name="size"/> is less than zero.
        /// </exception>
        public BoxSelection1D(
            int startIndex,
            int regionWidth,
            Size size)
            : this(startIndex, regionWidth, size.Width, size.Height)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref=" BoxSelection1D"/>
        /// class.
        /// </summary>
        /// <param name="startIndex">
        /// The index of the first selected tile.
        /// </param>
        /// <param name="regionWidth">
        /// The width of the data grid.
        /// </param>
        /// <param name="width">
        /// The width of the selection.
        /// </param>
        /// <param name="height">
        /// The height of the selection.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="width"/> is less than zero.
        /// <para/>
        /// -or-
        /// <para/>
        /// <paramref name="height"/> is less than zero.
        /// </exception>
        public BoxSelection1D(
            int startIndex,
            int regionWidth,
            int width,
            int height)
            : base(startIndex)
        {
            if (regionWidth <= 0)
            {
                throw ValueNotGreaterThan(
                    nameof(regionWidth),
                    regionWidth);
            }

            if (width <= 0)
            {
                throw ValueNotGreaterThan(
                    nameof(width),
                    width);
            }

            if (height <= 0)
            {
                throw ValueNotGreaterThan(
                    nameof(height),
                    height);
            }

            RegionWidth = regionWidth;
            Size = new Size(width, height);
        }

        /// <summary>
        /// Gets the width of the data grid.
        /// </summary>
        public int RegionWidth
        {
            get;
        }

        /// <summary>
        /// Gets the size of the selection.
        /// </summary>
        public Size Size
        {
            get;
        }

        /// <summary>
        /// Gets the number of selected tiles in this <see cref="
        /// BoxSelection1D"/>.
        /// </summary>
        public override int Count
        {
            get
            {
                return Size.Width * Size.Height;
            }
        }

        /// <summary>
        /// Creates a new instance of <see cref="BoxSelection1D"/> that has the
        /// same selection properties as this instance.
        /// </summary>
        /// <returns>
        /// A copy of this <see cref="BoxSelection1D"/>.
        /// </returns>
        public override Selection1D Copy()
        {
            return new BoxSelection1D(StartIndex, RegionWidth, Size);
        }

        /// <summary>
        /// Determines whether a data grid index is part of this <see
        /// cref="BoxSelection1D"/>.
        /// </summary>
        /// <param name="index">
        /// The <see cref="ITileMap1D"/> grid location to inspect.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="index"/> is in this <see
        /// cref="BoxSelection1D"/>; otherwise <see langword="false"/>.
        /// </returns>
        public override bool Contains(int index)
        {
            var viewIndex = index -= StartIndex;
            var x = viewIndex % RegionWidth;
            var y = viewIndex / RegionWidth;
            var bounds = new Rectangle(Point.Empty, Size);
            return bounds.Contains(x, y);
        }

        /// <summary>
        /// Returns an enumerator that enumerates through the <see
        /// cref="SingleSelection1D"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator{T}"/> for the <see cref="
        /// SingleSelection1D"/>.
        /// </returns>
        public override IEnumerator<int> GetEnumerator()
        {
            for (var y = 0; y < Size.Height; y++)
            {
                var rowIndex = StartIndex + (y * RegionWidth);
                for (var x = 0; x < Size.Width; x++)
                {
                    yield return rowIndex + x;
                }
            }
        }
    }
}
