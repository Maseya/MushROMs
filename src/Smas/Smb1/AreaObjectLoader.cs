namespace Maseya.Smas.Smb1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Maseya.Smb1;

    public class AreaObjectLoader
    {
        public const int DefaultBankByte = 0x04;
        public const int DefaultLowBytePointer = 0x04C194;
        public const int DefaultHighBytePointer = 0x04C1B6;
        public const int DefaultAreaTypeOffsetPointer = 0x04C190;

        public AreaObjectLoader(
            byte[] rom,
            int lowBytePointer = DefaultLowBytePointer,
            int highBytePointer = DefaultHighBytePointer,
            int bankByte = DefaultBankByte,
            int areaTypeOffsetPointer = DefaultAreaTypeOffsetPointer)
        {
            Rom = rom
                ?? throw new ArgumentNullException(nameof(rom));

            LowBytePointer = lowBytePointer;
            HighBytePointer = highBytePointer;
            BankByte = bankByte;
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

        public int BankByte
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
                | (Rom[highByteIndex + areaIndex] << 8)
                | (BankByte << 0x10);
        }

        public AreaHeader GetAreaHeader(int address)
        {
            var index = AreaLoader.SnesLoRomToPc(address);
            return new AreaHeader(Rom[index], Rom[index + 1]);
        }

        public IEnumerable<AreaObjectCommand> GetAreaData(int address)
        {
            var index = AreaLoader.SnesLoRomToPc(address);
            return AreaObjectCommand.GetAreaData(Rom.Skip(index));
        }

        public void WriteAreaData(
            int address,
            IEnumerable<AreaObjectCommand> data)
        {
            var bytes = new List<byte>(
                AreaObjectCommand.GetAreaByteData(data));

            bytes.CopyTo(Rom, AreaLoader.SnesLoRomToPc(address));
        }
    }
}
