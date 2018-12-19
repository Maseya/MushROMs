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
            /// The <see cref="SingleSelection2D"/> that created this
            /// <see cref="Enumerator"/>.
            /// </param>
            /// <exception cref="ArgumentNullException">
            /// <paramref name="selection"/> is <see langword="null"/>.
            /// </exception>
            public Enumerator(SingleSelection2D selection)
            {
                Selection = selection
                    ?? throw new ArgumentNullException(nameof(selection));

                CanMove = true;
                Current = default;
            }

            /// <summary>
            /// Gets the location in the <see cref="SingleSelection2D"/>
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
            /// Gets the <see cref="SingleSelection2D"/> that created
            /// this <see cref="Enumerator"/>.
            /// </summary>
            private SingleSelection2D Selection
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
            /// Gets or sets a value that determines whether this
            /// instance of <see cref="Enumerator"/> can advance in
            /// position by a call to <see cref="MoveNext"/>.
            /// </summary>
            private bool CanMove
            {
                get;
                set;
            }

            /// <summary>
            /// Sets the enumerator to the initial position.
            /// </summary>
            public void Reset()
            {
                CanMove = true;
                Current = default;
            }

            /// <summary>
            /// Advances the enumerator to the next index in the
            /// <see cref="SingleSelection2D"/>.
            /// </summary>
            /// <returns>
            /// <see langword="true"/> if the enumerator was
            /// successfully advanced to the next index; <see langword="
            /// false"/> if the enumerator has passed the end of the
            /// <see cref="SingleSelection2D"/>.
            /// </returns>
            public bool MoveNext()
            {
                if (CanMove)
                {
                    Current = StartPosition;
                    CanMove = false;
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
