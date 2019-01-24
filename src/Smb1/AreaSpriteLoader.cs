// <copyright file="AreaSpriteLoader.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Smb1
{
    using System;
    using System.Collections.Generic;
    using static Nes.AddressConverter;

    public class AreaSpriteLoader
    {
        public const int DefaultLowBytePointer = 0x9CE4;
        public const int DefaultHighBytePointer = 0x9D06;
        public const int DefaultAreaTypeOffsetPointer = 0x9CE0;

        public AreaSpriteLoader(
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

        public int GetAreaPointer(int areaNumber)
        {
            var lowByteIndex = NesToPc(LowBytePointer);
            var highByteIndex = NesToPc(HighBytePointer);
            var areaTypeOffsetIndex = NesToPc(AreaTypeOffsetPointer);

            var areaType = AreaLoader.GetAreaType(areaNumber);
            var reducedAreaNumber = areaNumber & 0x1F;
            var areaTypeIndex = Rom[areaTypeOffsetIndex + (int)areaType];
            var areaIndex = reducedAreaNumber + areaTypeIndex;
            return Rom[lowByteIndex + areaIndex]
                | (Rom[highByteIndex + areaIndex] << 8);
        }

        public IEnumerable<AreaSpriteCommand> GetAreaObjectData(int address)
        {
            var index = NesToPc(address);
            return AreaSpriteCommand.GetAreaData(
                Rom,
                index,
                Rom.Length - index);
        }

        public void WriteAreaData(
            int address,
            IEnumerable<AreaSpriteCommand> data)
        {
            var bytes = new List<byte>(
                AreaSpriteCommand.GetAreaByteData(data));

            bytes.CopyTo(Rom, NesToPc(address));
        }
    }
}
