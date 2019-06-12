namespace Maseya.Smas.Smb1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Maseya.Smb1;

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

        public int GetAreaIndex(int areaNumber)
        {
            var areaTypeOffsetIndex = AreaLoader.SnesLoRomToPc(
                AreaTypeOffsetPointer);

            var areaType = AreaLoader.GetAreaType(areaNumber);
            var reducedAreaNumber = areaNumber & 0x1F;
            var areaTypeIndex = Rom[areaTypeOffsetIndex + (int)areaType];
            return reducedAreaNumber + areaTypeIndex;
        }

        public int GetAreaPointer(int areaNumber)
        {
            var areaIndex = GetAreaIndex(areaNumber);
            var lowByteIndex = AreaLoader.SnesLoRomToPc(LowBytePointer);
            var highByteIndex = AreaLoader.SnesLoRomToPc(HighBytePointer);
            return Rom[lowByteIndex + areaIndex]
                | (Rom[highByteIndex + areaIndex] << 8);
        }

        public IEnumerable<AreaSpriteCommand> GetAreaData(int address)
        {
            var index = AreaLoader.SnesLoRomToPc(address);
            return AreaSpriteCommand.GetAreaData(Rom.Skip(index));
        }

        public void WriteAreaData(
            int address,
            IEnumerable<AreaSpriteCommand> data)
        {
            var bytes = new List<byte>(
                AreaSpriteCommand.GetAreaByteData(data));

            bytes.CopyTo(Rom, AreaLoader.SnesLoRomToPc(address));
        }
    }
}
