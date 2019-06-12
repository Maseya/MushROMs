// <copyright file="Define.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Assembler
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class Define
    {
        public Define(
            TextUnit start,
            string name,
            IEnumerable<PreprocessorToken> tokens)
        {
            Start = start;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Tokens = new ReadOnlyCollection<PreprocessorToken>(
                new List<PreprocessorToken>(tokens));
        }

        public TextUnit Start
        {
            get;
        }

        public string Name
        {
            get;
        }

        public ReadOnlyCollection<PreprocessorToken> Tokens
        {
            get;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
