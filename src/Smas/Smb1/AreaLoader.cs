namespace Maseya.Smas.Smb1
{
    using System;
    using System.Collections.Generic;
    using Maseya.Smb1;

    public class AreaLoader
    {
        public const int DefaultAreaListPointer = 0x4C124;
        public const int DefaultWorldLevelOffsetPointer = 0x4C11C;
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
            return Maseya.Smb1.AreaLoader.GetAreaType(areaNumber);
        }

        public byte GetAreaNumber(int world, int level)
        {
            var worldStartLevel = GetWorldStartLevel(world);
            var actualLevel = worldStartLevel + level;
            var areaListIndex = SnesLoRomToPc(AreaListPointer);
            return (byte)(Rom[areaListIndex + actualLevel] & 0x7F);
        }

        public byte GetWorldStartLevel(int world)
        {
            var areaListIndex = SnesLoRomToPc(AreaListPointer);
            var levelsPerWorldIndex = SnesLoRomToPc(WorldLevelOffsetPointer);
            return Rom[levelsPerWorldIndex + (byte)world];
        }

        public IEnumerable<byte> EnumerateAreaNumbers()
        {
            var index = SnesLoRomToPc(AreaListPointer);
            for (var i = 0; i < NumberOfAreas; i++)
            {
                yield return Rom[index + i];
            }
        }

        internal static int SnesLoRomToPc(int snes, int header = 0)
        {
            return (((snes & 0xFF0000) >> 1) | (snes & 0x7FFF)) + header;
        }
    }
}
