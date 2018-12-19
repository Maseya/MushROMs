// <copyright file="BoxSelection1D.cs" company="Public Domain">
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
    /// Specifies a rectangular selection of <see cref="ITileMap1D"/>
    /// tiles.
    /// </summary>
    public sealed class BoxSelection1D : Selection1D
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="
        /// BoxSelection1D"/> class.
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
        /// One of the properties of <paramref name="size"/> is less
        /// than zero.
        /// </exception>
        public BoxSelection1D(
            int startIndex,
            int regionWidth,
            Size size)
            : this(startIndex, regionWidth, size.Width, size.Height)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="
        /// BoxSelection1D"/> class.
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
        /// <para/>-or-<para/>
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
        /// Creates a new instance of <see cref="BoxSelection1D"/>
        /// that has the same selection properties as this instance.
        /// </summary>
        /// <returns>
        /// A copy of this <see cref="BoxSelection1D"/>.
        /// </returns>
        public override Selection1D Copy()
        {
            return new BoxSelection1D(StartIndex, RegionWidth, Size);
        }

        /// <summary>
        /// Determines whether a data grid index is part of this
        /// <see cref="BoxSelection1D"/>.
        /// </summary>
        /// <param name="index">
        /// The <see cref="ITileMap1D"/> grid location to inspect.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="index"/> is
        /// in this <see cref="BoxSelection1D"/>; otherwise
        /// <see langword="false"/>.
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
        /// Returns an enumerator that enumerates through the
        /// <see cref="SingleSelection1D"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator{T}"/> for the <see cref="
        /// SingleSelection1D"/>.
        /// </returns>
        public override IEnumerator<int> GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <summary>
        /// Supports an iteration over a <see cref="BoxSelection1D"/>
        /// instance.
        /// </summary>
        private struct Enumerator : IEnumerator<int>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="
            /// Enumerator"/> struct.
            /// </summary>
            /// <param name="selection">
            /// The <see cref="BoxSelection1D"/> that created this
            /// <see cref="Enumerator"/>.
            /// </param>
            /// <exception cref="ArgumentNullException">
            /// <paramref name="selection"/> is <see langword="null"/>.
            /// </exception>
            public Enumerator(BoxSelection1D selection)
            {
                Selection = selection
                    ?? throw new ArgumentNullException(nameof(selection));

                Index = 0;
                Current = default;
            }

            /// <summary>
            /// Gets the index in the <see cref="BoxSelection1D"/> at
            /// the current position of the enumerator.
            /// </summary>
            public int Current
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
            /// Gets the <see cref="BoxSelection1D"/> that created
            /// this <see cref="Enumerator"/>.
            /// </summary>
            private BoxSelection1D Selection
            {
                get;
            }

            /// <summary>
            /// Gets the index of the first selected tile.
            /// </summary>
            private int StartIndex
            {
                get
                {
                    return Selection.StartIndex;
                }
            }

            /// <summary>
            /// Gets the size of the selection.
            /// </summary>
            private Size Size
            {
                get
                {
                    return Selection.Size;
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
                Index = 0;
                Current = default;
            }

            /// <summary>
            /// Advances the enumerator to the next index in the
            /// <see cref="BoxSelection1D"/>.
            /// </summary>
            /// <returns>
            /// <see langword="true"/> if the enumerator was
            /// successfully advanced to the next index; <see langword="
            /// false"/> if the enumerator has passed the end of the
            /// <see cref="BoxSelection1D"/>.
            /// </returns>
            public bool MoveNext()
            {
                // Get the X and Y coordinates of the Index.
                var x = Index % RegionWidth;
                var y = Index / RegionWidth;

                // Go to the next X-coordinate.
                x++;

                // If we've exceeded the width...
                if (x >= Size.Width)
                {
                    // ... go to the start of the next row.
                    x = 0;
                    y++;
                }

                // If we're still within our height...
                if (y < Size.Height)
                {
                    // Update the current value and our index.
                    Current = StartIndex + Index;
                    Index = TileMap1D.GetGridTile(
                        new Point(x, y),
                        RegionWidth);

                    return true;
                }

                return false;
            }

            /// <inheritdoc/>
            void IDisposable.Dispose()
            {
            }
        }
    }
}
