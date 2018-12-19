// <copyright file="Selection1D.cs" company="Public Domain">
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

    /// <summary>
    /// Provides a base implementation for making a selection of data in
    /// an <see cref="ITileMap1D"/> instance.
    /// </summary>
    public abstract class Selection1D : ISelection1D
    {
        /// <summary>
        /// An instance of <see cref="ISelection1D"/> that contains no
        /// data.
        /// </summary>
        public static readonly ISelection1D Empty =
            new EmptySelection1D();

        /// <summary>
        /// Initializes a new instance the <see cref="Selection1D"/>
        /// class.
        /// </summary>
        /// <param name="startIndex">
        /// The index of the first selected tile of this <see cref="
        /// Selection1D"/>.
        /// </param>
        protected Selection1D(int startIndex = default)
        {
            StartIndex = startIndex;
        }

        /// <summary>
        /// Gets the index of the first selected tile of this
        /// <see cref="Selection1D"/>.
        /// </summary>
        public int StartIndex
        {
            get;
            protected set;
        }

        /// <summary>
        /// When overridden in a derived class, gets the number of
        /// selected tiles in this <see cref="Selection1D"/>.
        /// </summary>
        public abstract int Count
        {
            get;
        }

        /// <summary>
        /// When overridden in a derived class, creates a new instance
        /// of <see cref="Selection1D"/> that has the same selection
        /// properties as this instance.
        /// </summary>
        /// <returns>
        /// A copy of this <see cref="Selection1D"/>.
        /// </returns>
        public abstract Selection1D Copy();

        /// <inheritdoc/>
        ISelection1D ISelection1D.Copy()
        {
            return Copy();
        }

        /// <summary>
        /// When overridden in a derived class, determines whether a
        /// data grid index is part of this <see cref="Selection1D"/>.
        /// </summary>
        /// <param name="index">
        /// The <see cref="ITileMap1D"/> grid index to inspect.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="index"/> is
        /// in this <see cref="Selection1D"/>; otherwise
        /// <see langword="false"/>.
        /// </returns>
        public abstract bool Contains(int index);

        /// <summary>
        /// When overridden in a derived class, returns an enumerator
        /// that enumerates through the <see cref="Selection1D"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator{T}"/> for the <see cref="
        /// Selection1D"/>.
        /// </returns>
        public abstract IEnumerator<int> GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Represents an implementation of <see cref="ISelection1D"/>
        /// that contains no data.
        /// </summary>
        private sealed class EmptySelection1D : ISelection1D
        {
            /// <inheritdoc/>
            int ISelection1D.StartIndex
            {
                get
                {
                    return 0;
                }
            }

            /// <inheritdoc/>
            int IReadOnlyCollection<int>.Count
            {
                get
                {
                    return 0;
                }
            }

            /// <inheritdoc/>
            ISelection1D ISelection1D.Copy()
            {
                // Do not make an actual copy. Empty selection has no
                // data so there is no risk of unwanted sharing.
                return Empty;
            }

            /// <inheritdoc/>
            bool ISelection1D.Contains(int index)
            {
                return false;
            }

            /// <inheritdoc/>
            IEnumerator<int> IEnumerable<int>.GetEnumerator()
            {
                return default(Enumerator);
            }

            /// <inheritdoc/>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return default(Enumerator);
            }

            /// <summary>
            /// Provides an empty enumerator that has no elements.
            /// </summary>
            private struct Enumerator : IEnumerator<int>
            {
                /// <inheritdoc/>
                int IEnumerator<int>.Current
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
