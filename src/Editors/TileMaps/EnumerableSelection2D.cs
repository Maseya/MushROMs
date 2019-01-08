// <copyright file="EnumerableSelection2D.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Editors.TileMaps
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    /// <summary>
    /// Specifies a selection that is a binary combination of two <see
    /// cref="ISelection2D"/> instances.
    /// </summary>
    public sealed class EnumerableSelection2D : Selection2D
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="
        /// EnumerableSelection2D"/>
        /// </summary>
        /// <param name="left">
        /// The <see cref="ISelection2D"/> to combine with <paramref
        /// name="right"/>.
        /// </param>
        /// <param name="right">
        /// The <see cref="ISelection2D"/> to combine with <paramref
        /// name="left"/>.
        /// </param>
        /// <param name="rule">
        /// The binary operation to use when selecting each point of <paramref
        /// name="left"/> and <paramref name="right"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="left"/>, <paramref name="right"/>, or <paramref
        /// name="rule"/> is <see langword="null"/>.
        /// </exception>
        public EnumerableSelection2D(
            ISelection2D left,
            ISelection2D right,
            Func<bool, bool, bool> rule)
            : this(GetSelectedPoints(left, right, rule))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="
        /// EnumerableSelection2D"/> from a collection of selected points.
        /// </summary>
        /// <param name="selection">
        /// The collection of selected points.
        /// </param>
        public EnumerableSelection2D(IEnumerable<Point> selection)
            : base()
        {
            SelectedPoints = new HashSet<Point>(selection);

            var minPoint = new Point(Int32.MaxValue, Int32.MaxValue);
            foreach (var point in this)
            {
                if (minPoint.Y > point.Y)
                {
                    minPoint = point;
                }
                else if (minPoint.Y == point.Y && minPoint.X > point.X)
                {
                    minPoint = point;
                }
            }

            StartPosition = minPoint;
        }

        /// <summary>
        /// Gets the number of selected tiles in this <see cref="
        /// EnumerableSelection2D"/>.
        /// </summary>
        public override int Count
        {
            get
            {
                return SelectedPoints.Count;
            }
        }

        /// <summary>
        /// Gets a collection of the all of the selected points.
        /// </summary>
        private HashSet<Point> SelectedPoints
        {
            get;
        }

        /// <summary>
        /// Creates a new instance of <see cref="EnumerableSelection2D"/> that
        /// has the same selection properties as this instance.
        /// </summary>
        /// <returns>
        /// A copy of this <see cref="EnumerableSelection2D"/>.
        /// </returns>
        public override Selection2D Copy()
        {
            return new EnumerableSelection2D(this);
        }

        /// <summary>
        /// Determines whether a data grid point is part of this <see
        /// cref="EnumerableSelection2D"/>.
        /// </summary>
        /// <param name="location">
        /// The <see cref="ITileMap2D"/> grid location to inspect.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="location"/> is in this
        /// <see cref="EnumerableSelection2D"/>; otherwise <see
        /// langword="false"/>.
        /// </returns>
        public override bool Contains(Point location)
        {
            return SelectedPoints.Contains(location);
        }

        /// <summary>
        /// Returns an enumerator that enumerates through the <see
        /// cref="EnumerableSelection2D"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator{T}"/> for the <see cref="
        /// EnumerableSelection2D"/>.
        /// </returns>
        public override IEnumerator<Point> GetEnumerator()
        {
            return SelectedPoints.GetEnumerator();
        }

        /// <summary>
        /// Get the selected points as a binary operation between the points of
        /// two <see cref="ISelection2D"/> instances.
        /// </summary>
        /// <param name="left">
        /// The <see cref="ISelection2D"/> to combine with <paramref
        /// name="right"/>.
        /// </param>
        /// <param name="right">
        /// The <see cref="ISelection2D"/> to combine with <paramref
        /// name="left"/>.
        /// </param>
        /// <param name="rule">
        /// The binary operation to use when selecting each point of <paramref
        /// name="left"/> and <paramref name="right"/>.
        /// </param>
        /// <returns>
        /// A collection of the selected points that were in <paramref
        /// name="left"/>, <paramref name="right"/>, both, or neither,
        /// depending on the return result of <paramref name="rule"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="left"/>, <paramref name="right"/>, or <paramref
        /// name="rule"/> is <see langword="null"/>.
        /// </exception>
        private static IEnumerable<Point> GetSelectedPoints(
            ISelection2D left,
            ISelection2D right,
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

            // Get points in left expression.
            var leftQuery =
                from point in left
                where rule(true, right.Contains(point))
                select point;

            // Hash the current results.
            var leftHash = new HashSet<Point>(leftQuery);

            // Get points in right selection.
            var rightQuery =
                from point in right
                where !leftHash.Contains(point)
                    && rule(left.Contains(point), true)
                select point;

            // Return the union of both queries.
            return leftQuery.Concat(rightQuery);
        }
    }
}
