// <copyright file="SingleSelection1D.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Editors.TileMaps
{
    using System.Collections.Generic;

    /// <summary>
    /// Specifies a selection of a single <see cref="ITileMap1D"/> tile.
    /// </summary>
    public sealed class SingleSelection1D : Selection1D
    {
        /// <summary>
        /// Initializes a new instance of the <see cref=" SingleSelection1D"/>
        /// class.
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
        /// Creates a new instance of <see cref="SingleSelection1D"/> that has
        /// the same selection properties as this instance.
        /// </summary>
        /// <returns>
        /// A copy of this <see cref="SingleSelection1D"/>.
        /// </returns>
        public override Selection1D Copy()
        {
            return new SingleSelection1D(StartIndex);
        }

        /// <summary>
        /// Determines whether a data grid index is part of this <see
        /// cref="SingleSelection1D"/>.
        /// </summary>
        /// <param name="index">
        /// The <see cref="ITileMap1D"/> grid index to inspect.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="index"/> is in this <see
        /// cref="SingleSelection1D"/>; otherwise <see langword="false"/>.
        /// </returns>
        public override bool Contains(int index)
        {
            return index == StartIndex;
        }

        /// <summary>
        /// Returns an enumerator that enumerates through the <see
        /// cref="SingleSelection1D"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator{T}"/> for the <see cref="
        /// SingleSelection1D"/>.
        /// </returns>
        public override IEnumerator<int> GetEnumerator()
        {
            yield return StartIndex;
        }
    }
}
