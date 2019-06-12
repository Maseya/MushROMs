// <copyright file="AreaObjectLoader.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Smb1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using static Nes.AddressConverter;

    public class AreaObjectLoader
    {
        public const int DefaultLowBytePointer = 0x9D2C;
        public const int DefaultHighBytePointer = 0x9D4E;
        public const int DefaultAreaTypeOffsetPointer = 0x9D28;

        public AreaObjectLoader(
            byte[] rom,
            int lowBytePointer = DefaultLowBytePointer,
            int highBytePointer = DefaultHighBytePointer,
            int areaTypeOffsetPointer = DefaultAreaTypeOffsetPointer)
        {
            Rom = rom
                ?? throw new ArgumentNullException(nameof(rom));

            LowBytePointer = lowBytePointer;
            HighBytePointer = highBytePointer;
            AreaTypeOffsetPointer = areaTypeOffsetPointer;
        }

        public int LowBytePointer
        {
            get;
            set;
        }

        public int HighBytePointer
        {
            get;
            set;
        }

        public int AreaTypeOffsetPointer
        {
            get;
            set;
        }

        private byte[] Rom
        {
            get;
        }

        public int GetAreaIndex(int areaNumber)
        {
            var areaTypeOffsetIndex = NesToPc(AreaTypeOffsetPointer);
            var areaType = AreaLoader.GetAreaType(areaNumber);
            var reducedAreaNumber = areaNumber & 0x1F;
            var areaTypeIndex = Rom[areaTypeOffsetIndex + (int)areaType];
            return reducedAreaNumber + areaTypeIndex;
        }

        public int GetAreaPointer(int areaNumber)
        {
            var areaIndex = GetAreaIndex(areaNumber);
            var lowByteIndex = NesToPc(LowBytePointer);
            var highByteIndex = NesToPc(HighBytePointer);
            return Rom[lowByteIndex + areaIndex]
                | (Rom[highByteIndex + areaIndex] << 8);
        }

        public AreaHeader GetAreaHeader(int address)
        {
            var index = NesToPc(address);
            return new AreaHeader(Rom[index], Rom[index + 1]);
        }

        public IEnumerable<AreaObjectCommand> GetAreaData(int address)
        {
            var index = NesToPc(address);
            return AreaObjectCommand.GetAreaData(Rom.Skip(index));
        }

        public void WriteAreaData(
            int address,
            IEnumerable<AreaObjectCommand> data)
        {
            var bytes = new List<byte>(
                AreaObjectCommand.GetAreaByteData(data));

            bytes.CopyTo(Rom, NesToPc(address));
        }
    }
}
