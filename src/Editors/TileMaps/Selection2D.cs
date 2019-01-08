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
    using static Helper.ThrowHelper;

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
        /// Create a data dictionary of the values at the indexes of a
        /// specified list.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the values in the dictionary.
        /// </typeparam>
        /// <param name="collection">
        /// An implementation of <see cref="IReadOnlyList{T}"/> to
        /// retrieve values from indexes specified by the <see cref="
        /// Selection2D"/>. If the index is outside the bounds of
        /// <paramref name="collection"/>, the default value of
        /// <typeparamref name="T"/> will be used.
        /// </param>
        /// <param name="width">
        /// The width of the 2D area representing <paramref name="
        /// collection"/>.
        /// </param>
        /// <returns>
        /// A <see cref="Dictionary{TKey, TValue}"/> whose keys are the
        /// integer indexes of the <see cref="Selection2D"/> and whose
        /// values are the values of <paramref name="collection"/> at
        /// the corresponding index.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection"/> is <see langword="null"/>.
        /// </exception>
        public Dictionary<Point, T> GetValues<T>(
            IReadOnlyList<T> collection,
            int width)
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (width <= 0)
            {
                throw ValueNotGreaterThan(nameof(width), width);
            }

            var result = new Dictionary<Point, T>(Count);
            foreach (var point in this)
            {
                if ((uint)point.X >= (uint)width)
                {
                    result.Add(point, default);
                }

                var index = TileMap1D.GetGridTile(point, width);
                if ((uint)index >= (uint)collection.Count)
                {
                    result.Add(point, default);
                }

                result.Add(point, collection[index]);
            }

            return result;
        }

        /// <inheritdoc/>
        IDictionary<Point, T> ISelection2D.GetValues<T>(
            IReadOnlyList<T> collection,
            int width)
        {
            return GetValues(collection, width);
        }

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

        /// <summary>
        /// Represents an implementation of <see cref="ISelection1D"/>
        /// that contains no data.
        /// </summary>
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
            IDictionary<Point, T> ISelection2D.GetValues<T>(
                IReadOnlyList<T> collection,
                int width)
            {
                if (collection is null)
                {
                    throw new ArgumentNullException(nameof(collection));
                }

                return new Dictionary<Point, T>();
            }

            /// <inheritdoc/>
            public IEnumerator<Point> GetEnumerator()
            {
                yield break;
            }

            /// <inheritdoc/>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
