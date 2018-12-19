// <copyright file="BoxSelection2D.cs" company="Public Domain">
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
    /// Specifies a rectangular selection of <see cref="ITileMap2D"/>
    /// tiles.
    /// </summary>
    public sealed class BoxSelection2D : Selection2D
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="
        /// BoxSelection2D"/> class.
        /// </summary>
        /// <param name="startPosition">
        /// The location of the first selected tile.
        /// </param>
        /// <param name="size">
        /// The size of the selection.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// One of the properties of <paramref name="size"/> is less
        /// than zero.
        /// </exception>
        public BoxSelection2D(
            Point startPosition,
            Size size)
            : this(startPosition, size.Width, size.Height)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="
        /// BoxSelection2D"/> class.
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
        /// <para/>-or-<para/>
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
        /// Creates a new instance of <see cref="BoxSelection1D"/>
        /// that has the same selection properties as this instance.
        /// </summary>
        /// <returns>
        /// A copy of this <see cref="BoxSelection1D"/>.
        /// </returns>
        public override Selection2D Copy()
        {
            return new BoxSelection2D(StartPosition, Size);
        }

        /// <summary>
        /// Determines whether a data grid index is part of this
        /// <see cref="BoxSelection2D"/>.
        /// </summary>
        /// <param name="position">
        /// The <see cref="ITileMap2D"/> grid location to inspect.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="position"/> is
        /// in this <see cref="BoxSelection2D"/>; otherwise
        /// <see langword="false"/>.
        /// </returns>
        public override bool Contains(Point position)
        {
            var x = position.X - StartPosition.X;
            var y = position.Y - StartPosition.Y;
            var bounds = new Rectangle(Point.Empty, Size);
            return bounds.Contains(x, y);
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
            /// The <see cref="BoxSelection1D"/> that created this
            /// <see cref="Enumerator"/>.
            /// </param>
            /// <exception cref="ArgumentNullException">
            /// <paramref name="selection"/> is <see langword="null"/>.
            /// </exception>
            public Enumerator(BoxSelection2D selection)
            {
                Selection = selection
                    ?? throw new ArgumentNullException(nameof(selection));

                Index = StartPosition;
                Current = default;
            }

            /// <summary>
            /// Gets the location in the <see cref="BoxSelection2D"/>
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
            /// Gets the <see cref="BoxSelection2D"/> that created
            /// this <see cref="Enumerator"/>.
            /// </summary>
            private BoxSelection2D Selection
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
            /// Gets or sets the location of the next tile to be
            /// selected.
            /// </summary>
            private Point Index
            {
                get;
                set;
            }

            /// <summary>
            /// Sets the enumerator to the initial position.
            /// </summary>
            public void Reset()
            {
                Index = StartPosition;
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
                var x = Index.X;
                var y = Index.Y;

                x++;

                if (x >= StartPosition.X + Size.Width)
                {
                    x = StartPosition.X;
                    y = 0;
                }

                if (y < StartPosition.Y + Size.Height)
                {
                    Current = Index;
                    Index = new Point(x, y);
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
