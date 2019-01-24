// <copyright file="LinearListSelection.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Editors.Collections
{
    using System;
    using System.Collections.Generic;

    public class LinearListSelection : ListSelection
    {
        public LinearListSelection(int index, int count)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            MinIndex = index;
            Count = count;
        }

        public override int Count
        {
            get;
        }

        public override int MinIndex
        {
            get;
        }

        public override int MaxIndex
        {
            get
            {
                return MinIndex + Count - 1;
            }
        }

        private int Index
        {
            get;
        }

        public override int this[int index]
        {
            get
            {
                if (!Contains(index))
                {
                    throw new ArgumentOutOfRangeException();
                }

                return MinIndex + index;
            }
        }

        public override ListSelection Move(int amount)
        {
            return new LinearListSelection(MinIndex + amount, Count);
        }

        public override bool Contains(int index)
        {
            return (index >= MinIndex) && (index <= MaxIndex);
        }

        public override IEnumerator<int> GetEnumerator()
        {
            for (var i = MinIndex; i <= MaxIndex; i++)
            {
                yield return i;
            }
        }
    }
}
