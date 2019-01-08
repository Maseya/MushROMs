// <copyright file="BoxSelection2D.cs" company="Public Domain">
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
    /// Specifies a rectangular selection of <see cref="ITileMap2D"/> tiles.
    /// </summary>
    public sealed class BoxSelection2D : Selection2D
    {
        /// <summary>
        /// Initializes a new instance of the <see cref=" BoxSelection2D"/>
        /// class.
        /// </summary>
        /// <param name="startPosition">
        /// The location of the first selected tile.
        /// </param>
        /// <param name="size">
        /// The size of the selection.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// One of the properties of <paramref name="size"/> is less than zero.
        /// </exception>
        public BoxSelection2D(
            Point startPosition,
            Size size)
            : this(startPosition, size.Width, size.Height)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref=" BoxSelection2D"/>
        /// class.
        /// </summary>
        /// <param name="startPosition">
        /// The location of the first selected tile.
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
        public BoxSelection2D(
            Point startPosition,
            int width,
            int height)
            : base(startPosition)
        {
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

            Size = new Size(width, height);
        }

        /// <summary>
        /// Gets the number of selected tiles in this <see cref="
        /// BoxSelection2D"/>.
        /// </summary>
        public override int Count
        {
            get
            {
                return Size.Width * Size.Height;
            }
        }

        /// <summary>
        /// Gets the size of the selection.
        /// </summary>
        private Size Size
        {
            get;
        }

        /// <summary>
        /// Creates a new instance of <see cref="BoxSelection1D"/> that has the
        /// same selection properties as this instance.
        /// </summary>
        /// <returns>
        /// A copy of this <see cref="BoxSelection1D"/>.
        /// </returns>
        public override Selection2D Copy()
        {
            return new BoxSelection2D(StartPosition, Size);
        }

        /// <summary>
        /// Determines whether a data grid index is part of this <see
        /// cref="BoxSelection2D"/>.
        /// </summary>
        /// <param name="position">
        /// The <see cref="ITileMap2D"/> grid location to inspect.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="position"/> is in this
        /// <see cref="BoxSelection2D"/>; otherwise <see langword="false"/>.
        /// </returns>
        public override bool Contains(Point position)
        {
            var x = position.X - StartPosition.X;
            var y = position.Y - StartPosition.Y;
            var bounds = new Rectangle(Point.Empty, Size);
            return bounds.Contains(x, y);
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
            for (var y = 0; y < Size.Height; y++)
            {
                for (var x = 0; x < Size.Width; x++)
                {
                    yield return new Point(
                        StartPosition.X + x,
                        StartPosition.Y + y);
                }
            }
        }
    }
}
