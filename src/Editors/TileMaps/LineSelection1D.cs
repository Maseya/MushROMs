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
            for (var i = 0; i < Length; i++)
            {
                yield return StartIndex + i;
            }
        }
    }
}
