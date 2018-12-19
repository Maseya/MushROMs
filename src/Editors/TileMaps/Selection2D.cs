// <copyright file="Selection2D.cs" company="Public Domain">
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
    /// Provides a base implementation for making a selection of data in
    /// a <see cref="ITileMap2D"/> instance.
    /// </summary>
    public abstract class Selection2D : ISelection2D
    {
        /// <summary>
        /// An instance of <see cref="ISelection2D"/> that contains no
        /// data.
        /// </summary>
        public static readonly ISelection2D Empty =
            new EmptySelection2D();

        /// <summary>
        /// Initializes a new instance the <see cref="Selection2D"/>
        /// class.
        /// </summary>
        /// <param name="startPosition">
        /// The location of the first selected tile of this <see cref="
        /// ISelection1D"/>.
        /// </param>
        protected Selection2D(Point startPosition = default)
        {
            StartPosition = startPosition;
        }

        /// <summary>
        /// Gets the location of the first selected tile of this
        /// <see cref="Selection2D"/>.
        /// </summary>
        public Point StartPosition
        {
            get;
            protected set;
        }

        /// <summary>
        /// When overridden in a derived class, gets the number of
        /// selected tiles in this <see cref="Selection2D"/>.
        /// </summary>
        public abstract int Count
        {
            get;
        }

        /// <summary>
        /// When overridden in a derived class, creates a new instance
        /// of <see cref="Selection2D"/> that has the same selection
        /// properties as this instance.
        /// </summary>
        /// <returns>
        /// A copy of this <see cref="Selection2D"/>.
        /// </returns>
        public abstract Selection2D Copy();

        /// <inheritdoc/>
        ISelection2D ISelection2D.Copy()
        {
            return Copy();
        }

        /// <summary>
        /// When overridden in a derived class, determines whether a
        /// data grid point is part of this <see cref="Selection2D"/>.
        /// </summary>
        /// <param name="position">
        /// The <see cref="ITileMap2D"/> grid index to inspect.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="position"/> is
        /// in this <see cref="Selection2D"/>; otherwise
        /// <see langword="false"/>.
        /// </returns>
        public abstract bool Contains(Point position);

        /// <summary>
        /// When overridden in a derived class, returns an enumerator
        /// that enumerates through the <see cref="Selection2D"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator{T}"/> for the <see cref="
        /// Selection2D"/>.
        /// </returns>
        public abstract IEnumerator<Point> GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private sealed class EmptySelection2D : ISelection2D
        {
            /// <inheritdoc/>
            Point ISelection2D.StartPosition
            {
                get
                {
                    return Point.Empty;
                }
            }

            /// <inheritdoc/>
            int IReadOnlyCollection<Point>.Count
            {
                get
                {
                    return 0;
                }
            }

            /// <inheritdoc/>
            ISelection2D ISelection2D.Copy()
            {
                return Empty;
            }

            /// <inheritdoc/>
            bool ISelection2D.Contains(Point position)
            {
                return false;
            }

            /// <inheritdoc/>
            IEnumerator<Point> IEnumerable<Point>.GetEnumerator()
            {
                return default(Enumerator);
            }

            /// <inheritdoc/>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return default(Enumerator);
            }

            private struct Enumerator : IEnumerator<Point>
            {
                /// <inheritdoc/>
                Point IEnumerator<Point>.Current
                {
                    get;
                }

                /// <inheritdoc/>
                object IEnumerator.Current
                {
                    get;
                }

                /// <inheritdoc/>
                void IEnumerator.Reset()
                {
                }

                /// <inheritdoc/>
                bool IEnumerator.MoveNext()
                {
                    return false;
                }

                /// <inheritdoc/>
                void IDisposable.Dispose()
                {
                }
            }
        }
    }
}
