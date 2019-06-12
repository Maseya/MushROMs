// <copyright file="AreaLoader.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Smb1
{
    using System;
    using System.Collections.Generic;
    using static Nes.AddressConverter;

    public class AreaLoader
    {
        public const int DefaultAreaListPointer = 0x9CBC;
        public const int DefaultWorldLevelOffsetPointer = 0x9CB4;
        public const int DefaultNumberOfAreas = 0x22;
        public const int DefaultNumberOfWorlds = 8;

        public AreaLoader(
            byte[] rom,
            int worldLevelOffsetPointer = DefaultWorldLevelOffsetPointer,
            int numberOfWorlds = DefaultNumberOfWorlds,
            int areaListPointer = DefaultAreaListPointer,
            int numberOfAreas = DefaultNumberOfAreas)
        {
            Rom = rom
                ?? throw new ArgumentNullException(nameof(rom));

            AreaListPointer = areaListPointer;
            WorldLevelOffsetPointer = worldLevelOffsetPointer;
            NumberOfWorlds = numberOfWorlds;
            NumberOfAreas = numberOfAreas;
        }

        public int AreaListPointer
        {
            get;
            set;
        }

        public int WorldLevelOffsetPointer
        {
            get;
            set;
        }

        public int NumberOfAreas
        {
            get;
            set;
        }

        public int NumberOfWorlds
        {
            get;
            set;
        }

        private byte[] Rom
        {
            get;
        }

        public static AreaType GetAreaType(int areaNumber)
        {
            return (AreaType)((areaNumber >> 5) & 3);
        }

        public byte GetAreaNumber(int world, int level)
        {
            var worldStartLevel = GetWorldStartLevel(world);
            var actualLevel = worldStartLevel + level;
            var areaListIndex = NesToPc(AreaListPointer);
            return (byte)(Rom[areaListIndex + actualLevel] & 0x7F);
        }

        public byte GetWorldStartLevel(int world)
        {
            var areaListIndex = NesToPc(AreaListPointer);
            var levelsPerWorldIndex = NesToPc(WorldLevelOffsetPointer);
            return Rom[levelsPerWorldIndex + (byte)world];
        }

        public IEnumerable<byte> EnumerateAreaNumbers()
        {
            var index = NesToPc(AreaListPointer);
            for (var i = 0; i < NumberOfAreas; i++)
            {
                yield return Rom[index + i];
            }
        }
    }
}
