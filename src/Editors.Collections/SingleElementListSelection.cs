// <copyright file="SingleElementListSelection.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Editors.Collections
{
    using System;
    using System.Collections.Generic;

    public class SingleElementListSelection : ListSelection
    {
        public SingleElementListSelection(int value)
        {
            Value = value;
        }

        public int Value
        {
            get;
        }

        public override int MinIndex
        {
            get
            {
                return Value;
            }
        }

        public override int MaxIndex
        {
            get
            {
                return Value;
            }
        }

        public override int Count
        {
            get
            {
                return 1;
            }
        }

        public override int this[int index]
        {
            get
            {
                if (index != 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                return Value;
            }
        }

        public override bool ContainsIndex(int index)
        {
            return index == Value;
        }

        public override ListSelection Move(int amount)
        {
            return new SingleElementListSelection(Value + amount);
        }

        public override IEnumerator<int> GetEnumerator()
        {
            yield return Value;
        }
    }
}
