// <copyright file="SingleSelection1D.cs" company="Public Domain">
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
    /// Specifies a selection of a single <see cref="ITileMap1D"/> tile.
    /// </summary>
    public sealed class SingleSelection1D : Selection1D
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="
        /// SingleSelection1D"/> class.
        /// </summary>
        /// <param name="index">
        /// The index of the selected tile of this <see cref="
        /// SingleSelection1D"/>.
        /// </param>
        public SingleSelection1D(int index)
            : base(index)
        {
        }

        /// <summary>
        /// Gets the number of selected tiles in this <see cref="
        /// SingleSelection1D"/>.
        /// </summary>
        public override int Count
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// Creates a new instance of <see cref="SingleSelection1D"/>
        /// that has the same selection properties as this instance.
        /// </summary>
        /// <returns>
        /// A copy of this <see cref="SingleSelection1D"/>.
        /// </returns>
        public override Selection1D Copy()
        {
            return new SingleSelection1D(StartIndex);
        }

        /// <summary>
        /// Determines whether a data grid index is part of this
        /// <see cref="SingleSelection1D"/>.
        /// </summary>
        /// <param name="index">
        /// The <see cref="ITileMap1D"/> grid index to inspect.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="index"/> is
        /// in this <see cref="SingleSelection1D"/>; otherwise
        /// <see langword="false"/>.
        /// </returns>
        public override bool Contains(int index)
        {
            return index == StartIndex;
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
        /// Supports an iteration over a <see cref="SingleSelection1D"/>
        /// instance.
        /// </summary>
        private struct Enumerator : IEnumerator<int>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="
            /// Enumerator"/> struct.
            /// </summary>
            /// <param name="selection">
            /// The <see cref="SingleSelection1D"/> that created this
            /// <see cref="Enumerator"/>.
            /// </param>
            /// <exception cref="ArgumentNullException">
            /// <paramref name="selection"/> is <see langword="null"/>.
            /// </exception>
            public Enumerator(SingleSelection1D selection)
            {
                Selection = selection
                    ?? throw new ArgumentNullException(nameof(selection));

                CanMove = true;
                Current = default;
            }

            /// <summary>
            /// Gets the index in the <see cref="SingleSelection1D"/> at
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
            /// Gets the <see cref="SingleSelection1D"/> that created
            /// this <see cref="Enumerator"/>.
            /// </summary>
            private SingleSelection1D Selection
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
                CanMove = Selection != null;
                Current = default;
            }

            /// <summary>
            /// Advances the enumerator to the next index in the
            /// <see cref="SingleSelection1D"/>.
            /// </summary>
            /// <returns>
            /// <see langword="true"/> if the enumerator was
            /// successfully advanced to the next index; <see langword="
            /// false"/> if the enumerator has passed the end of the
            /// <see cref="SingleSelection1D"/>.
            /// </returns>
            public bool MoveNext()
            {
                if (CanMove)
                {
                    CanMove = false;
                    Current = StartIndex;
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
