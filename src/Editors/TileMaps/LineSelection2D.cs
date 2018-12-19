// <copyright file="LineSelection2D.cs" company="Public Domain">
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
    using static Helper.ThrowHelper;

    /// <summary>
    /// Specifies a linear selection of <see cref="ITileMap2D"/> tiles.
    /// </summary>
    public sealed class LineSelection2D : Selection2D
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="
        /// LineSelection2D"/> class.
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
        /// <para/>-or-<para/>
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
        /// Creates a new instance of <see cref="LineSelection1D"/>
        /// that has the same selection properties as this instance.
        /// </summary>
        /// <returns>
        /// A copy of this <see cref="LineSelection1D"/>.
        /// </returns>
        public override Selection2D Copy()
        {
            return new LineSelection2D(StartPosition, RegionWidth, Length);
        }

        /// <summary>
        /// Determines whether a data grid index is part of this
        /// <see cref="LineSelection2D"/>.
        /// </summary>
        /// <param name="position">
        /// The <see cref="ITileMap2D"/> grid location to inspect.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="position"/> is
        /// in this <see cref="LineSelection2D"/>; otherwise
        /// <see langword="false"/>.
        /// </returns>
        public override bool Contains(Point position)
        {
            var x = position.X - StartPosition.X;
            var y = position.Y - StartPosition.Y;
            var index = (y * RegionWidth) + x;
            return index >= 0 && index < Length;
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
            return new Enumerator(this);
        }

        /// <summary>
        /// Supports an iteration over a <see cref="SingleSelection1D"/>
        /// instance.
        /// </summary>
        private struct Enumerator : IEnumerator<Point>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="
            /// Enumerator"/> struct.
            /// </summary>
            /// <param name="selection">
            /// The <see cref="LineSelection1D"/> that created this
            /// <see cref="Enumerator"/>.
            /// </param>
            /// <exception cref="ArgumentNullException">
            /// <paramref name="selection"/> is <see langword="null"/>.
            /// </exception>
            public Enumerator(LineSelection2D selection)
            {
                Selection = selection
                    ?? throw new ArgumentNullException(nameof(selection));
                Index = default;
                Current = default;
            }

            /// <summary>
            /// Gets the location in the <see cref="LineSelection2D"/>
            /// at the current position of the enumerator.
            /// </summary>
            public Point Current
            {
                get;
                private set;
            }

            /// <inheritdoc/>
            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            /// <summary>
            /// Gets the <see cref="LineSelection2D"/> that created
            /// this <see cref="Enumerator"/>.
            /// </summary>
            private LineSelection2D Selection
            {
                get;
            }

            /// <summary>
            /// Gets the location of the first selected tile.
            /// </summary>
            private Point StartPosition
            {
                get
                {
                    return Selection.StartPosition;
                }
            }

            /// <summary>
            /// Gets the width of the data grid.
            /// </summary>
            private int RegionWidth
            {
                get
                {
                    return Selection.RegionWidth;
                }
            }

            /// <summary>
            /// Gets the length of the linear selection.
            /// </summary>
            private int Length
            {
                get
                {
                    return Selection.Length;
                }
            }

            /// <summary>
            /// Gets the index of the first selected tile.
            /// </summary>
            private int StartIndex
            {
                get
                {
                    return TileMap1D.GetGridTile(
                        Selection.StartPosition,
                        RegionWidth);
                }
            }

            /// <summary>
            /// Gets or sets the index of the next tile to be selected.
            /// </summary>
            private int Index
            {
                get;
                set;
            }

            /// <summary>
            /// Sets the enumerator to the initial position.
            /// </summary>
            public void Reset()
            {
                Index = StartIndex;
                Current = default;
            }

            /// <summary>
            /// Advances the enumerator to the next index in the
            /// <see cref="LineSelection1D"/>.
            /// </summary>
            /// <returns>
            /// <see langword="true"/> if the enumerator was
            /// successfully advanced to the next index; <see langword="
            /// false"/> if the enumerator has passed the end of the
            /// <see cref="LineSelection1D"/>.
            /// </returns>
            public bool MoveNext()
            {
                if (Index >= StartIndex + Length)
                {
                    return false;
                }

                Current = new Point(Index % RegionWidth, Index / RegionWidth);
                Index++;
                return true;
            }

            /// <inheritdoc/>
            void IDisposable.Dispose()
            {
            }
        }
    }
}
