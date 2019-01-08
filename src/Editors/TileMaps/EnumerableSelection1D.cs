// <copyright file="EnumerableSelection1D.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Editors.TileMaps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Specifies a selection that is a binary combination of two <see
    /// cref="ISelection1D"/> instances.
    /// </summary>
    public sealed class EnumerableSelection1D : Selection1D
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="
        /// EnumerableSelection1D"/>
        /// </summary>
        /// <param name="left">
        /// The <see cref="ISelection1D"/> to combine with <paramref
        /// name="right"/>.
        /// </param>
        /// <param name="right">
        /// The <see cref="ISelection1D"/> to combine with <paramref
        /// name="left"/>.
        /// </param>
        /// <param name="rule">
        /// The binary operation to use when selecting each index of <paramref
        /// name="left"/> and <paramref name="right"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="left"/>, <paramref name="right"/>, or <paramref
        /// name="rule"/> is <see langword="null"/>.
        /// </exception>
        public EnumerableSelection1D(
            ISelection1D left,
            ISelection1D right,
            Func<bool, bool, bool> rule)
            : this(GetSelectedIndexes(left, right, rule))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="
        /// EnumerableSelection1D"/> from a collection of selected indexes.
        /// </summary>
        /// <param name="selection">
        /// The collection of selected indexes.
        /// </param>
        public EnumerableSelection1D(IEnumerable<int> selection)
            : base()
        {
            SelectedIndexes = new HashSet<int>(selection);
            StartIndex = SelectedIndexes.Min();
        }

        /// <summary>
        /// Gets the number of selected tiles in this <see cref="
        /// EnumerableSelection1D"/>.
        /// </summary>
        public override int Count
        {
            get
            {
                return SelectedIndexes.Count;
            }
        }

        /// <summary>
        /// Gets a collection of the all of the selected indexes.
        /// </summary>
        private HashSet<int> SelectedIndexes
        {
            get;
        }

        /// <summary>
        /// Creates a new instance of <see cref="EnumerableSelection1D"/> that
        /// has the same selection properties as this instance.
        /// </summary>
        /// <returns>
        /// A copy of this <see cref="EnumerableSelection1D"/>.
        /// </returns>
        public override Selection1D Copy()
        {
            return new EnumerableSelection1D(this);
        }

        /// <summary>
        /// Determines whether a data grid index is part of this <see
        /// cref="EnumerableSelection1D"/>.
        /// </summary>
        /// <param name="index">
        /// The <see cref="ITileMap1D"/> grid location to inspect.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="index"/> is in this <see
        /// cref="EnumerableSelection1D"/>; otherwise <see langword="false"/>.
        /// </returns>
        public override bool Contains(int index)
        {
            return SelectedIndexes.Contains(index);
        }

        /// <summary>
        /// Returns an enumerator that enumerates through the <see
        /// cref="EnumerableSelection1D"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator{T}"/> for the <see cref="
        /// EnumerableSelection1D"/>.
        /// </returns>
        public override IEnumerator<int> GetEnumerator()
        {
            return SelectedIndexes.GetEnumerator();
        }

        /// <summary>
        /// Get the selected indexes as a binary operation between the indexes
        /// of two <see cref="ISelection1D"/> instances.
        /// </summary>
        /// <param name="left">
        /// The <see cref="ISelection1D"/> to combine with <paramref
        /// name="right"/>.
        /// </param>
        /// <param name="right">
        /// The <see cref="ISelection1D"/> to combine with <paramref
        /// name="left"/>.
        /// </param>
        /// <param name="rule">
        /// The binary operation to use when selecting each index of <paramref
        /// name="left"/> and <paramref name="right"/>.
        /// </param>
        /// <returns>
        /// A collection of the selected indexes that were in <paramref
        /// name="left"/>, <paramref name="right"/>, both, or neither,
        /// depending on the return result of <paramref name="rule"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="left"/>, <paramref name="right"/>, or <paramref
        /// name="rule"/> is <see langword="null"/>.
        /// </exception>
        private static IEnumerable<int> GetSelectedIndexes(
            ISelection1D left,
            ISelection1D right,
            Func<bool, bool, bool> rule)
        {
            if (left is null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            if (right is null)
            {
                throw new ArgumentNullException(nameof(right));
            }

            if (rule is null)
            {
                throw new ArgumentNullException(nameof(rule));
            }

            // Get indexes in left expression.
            var leftQuery =
                from index in left
                where rule(true, right.Contains(index))
                select index;

            // Hash the current results.
            var leftHash = new HashSet<int>(leftQuery);

            // Get indexes in right selection.
            var rightQuery =
                from index in right
                where !leftHash.Contains(index)
                    && rule(left.Contains(index), true)
                select index;

            // Return the union of both queries.
            return leftQuery.Concat(rightQuery);
        }
    }
}
