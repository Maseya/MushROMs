// <copyright file="LineSelection1D.cs" company="Public Domain">
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
    using static Helper.ThrowHelper;

    /// <summary>
    /// Specifies a linear selection of <see cref="ITileMap1D"/> tiles.
    /// </summary>
    public sealed class LineSelection1D : Selection1D
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="
        /// LineSelection1D"/> class.
        /// </summary>
        /// <param name="startIndex">
        /// The index of the first selected tile of this <see cref="
        /// LineSelection1D"/>.
        /// </param>
        /// <param name="length">
        /// The length of the linear selection.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="length"/> is less than zero.
        /// </exception>
        public LineSelection1D(int startIndex, int length)
            : base(startIndex)
        {
            if (length < 0)
            {
                throw ValueNotGreaterThanEqualTo(
                    nameof(length),
                    length);
            }

            Length = length;
        }

        /// <summary>
        /// Gets the number of selected tiles in this <see cref="
        /// LineSelection1D"/>.
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
        /// Creates a new instance of <see cref="LineSelection1D"/>
        /// that has the same selection properties as this instance.
        /// </summary>
        /// <returns>
        /// A copy of this <see cref="LineSelection1D"/>.
        /// </returns>
        public override Selection1D Copy()
        {
            return new LineSelection1D(StartIndex, Length);
        }

        /// <summary>
        /// Determines whether a data grid index is part of this
        /// <see cref="LineSelection1D"/>.
        /// </summary>
        /// <param name="index">
        /// The <see cref="ITileMap1D"/> grid location to inspect.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="index"/> is
        /// in this <see cref="LineSelection1D"/>; otherwise
        /// <see langword="false"/>.
        /// </returns>
        public override bool Contains(int index)
        {
            index -= StartIndex;
            return index >= 0 && index < Length;
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
        /// Supports an iteration over a <see cref="LineSelection1D"/>
        /// instance.
        /// </summary>
        private struct Enumerator : IEnumerator<int>
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
            public Enumerator(LineSelection1D selection)
            {
                Selection = selection
                    ?? throw new ArgumentNullException(nameof(selection));

                Index = default;
                Current = default;
            }

            /// <summary>
            /// Gets the index in the <see cref="LineSelection1D"/> at
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
            /// Gets the <see cref="LineSelection1D"/> that created
            /// this <see cref="Enumerator"/>.
            /// </summary>
            private LineSelection1D Selection
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

                Current = Index++;
                return true;
            }

            /// <inheritdoc/>
            void IDisposable.Dispose()
            {
            }
        }
    }
}
