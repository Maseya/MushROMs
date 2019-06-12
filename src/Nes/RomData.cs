// <copyright file="RomData.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Nes
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using static AddressConverter;

    public class RomData : IRomData
    {
        public RomData(byte[] data)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            Data = new byte[data.Length];
            data.CopyTo(Data, 0);
        }

        public int Count
        {
            get;
            private set;
        }

        private int Capacity
        {
            get
            {
                return Data.Length;
            }
        }

        private byte[] Data
        {
            get;
            set;
        }

        public byte this[int address]
        {
            get
            {
                var index = NesToPc(address);
                if ((uint)index >= (uint)index)
                {
                    throw new ArgumentOutOfRangeException(nameof(address));
                }

                return Data[index];
            }

            set
            {
                if ((uint)address >= (uint)Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(address));
                }

                Data[address] = value;
            }
        }

        public IEnumerator<byte> GetEnumerator()
        {
            for (var i = 0x10; i < Count + 0x10; i++)
            {
                yield return Data[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int MemoryAddressToRomIndex(int address)
        {
            return NesToPc(address);
        }

        public int RomIndexToMemoryAddress(int index)
        {
            return PcToNes(index);
        }

        public void Resize(int size)
        {
            var result = new byte[size];
        }

        public byte GetByte(int address)
        {
            throw new NotImplementedException();
        }

        public void SetByte(int value, int address)
        {
            throw new NotImplementedException();
        }

        public short GetInt16(int address, bool crossBoundaries)
        {
            throw new NotImplementedException();
        }

        public void SetInt16(int value, int address, bool crossBoundaries)
        {
            throw new NotImplementedException();
        }

        public int GetUInt16(int address, bool crossBoundaries)
        {
            throw new NotImplementedException();
        }

        public void SetUInt16(int value, int address, bool crossBoundaries)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<byte> GetBytes(
            int startAddress,
            int count,
            bool crossBoundariess)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<short> GetInt16s(
            int startAddress,
            int count,
            bool crossBoundaries)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<int> GetUInt16s(
            int startAddress,
            int count,
            bool crossBoundaries)
        {
            throw new NotImplementedException();
        }

        public void WriteBytes(
            IEnumerable<byte> values,
            int startAddress,
            bool crossBoundaries)
        {
            throw new NotImplementedException();
        }

        public void WriteInt16s(
            IEnumerable<short> values,
            int startAddress,
            bool crossBoundaries)
        {
            throw new NotImplementedException();
        }

        public void WriteUInt16s(
            IEnumerable<int> values,
            int startAddress,
            bool crossBoundaries)
        {
            throw new NotImplementedException();
        }
    }
}
