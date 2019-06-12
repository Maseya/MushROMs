// <copyright file="IRomData.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes
{
    using System.Collections.Generic;
    using INesRomData = Maseya.Nes.IRomData;

    public interface IRomData : INesRomData
    {
        IEnumerable<int> GetUInt24s(
            int startAddress,
            int count,
            bool crossBoundaries);

        void WriteUInt24s(
            IEnumerable<int> values,
            int startAddress,
            bool crossBoundaries);
    }
}
