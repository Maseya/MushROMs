// <copyright file="IRomData.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Nes
{
    using System.Collections.Generic;

    public interface IRomData : IReadOnlyList<byte>
    {
        new byte this[int address]
        {
            get;
            set;
        }

        int MemoryAddressToRomIndex(int address);

        int RomIndexToMemoryAddress(int index);

        void Resize(int size);

        byte GetByte(int address);

        void SetByte(int value, int address);

        short GetInt16(int address, bool crossBoundaries);

        void SetInt16(int value, int address, bool crossBoundaries);

        int GetUInt16(int address, bool crossBoundaries);

        void SetUInt16(int value, int address, bool crossBoundaries);

        IEnumerable<byte> GetBytes(
            int startAddress,
            int count,
            bool crossBoundariess);

        IEnumerable<short> GetInt16s(
            int startAddress,
            int count,
            bool crossBoundaries);

        IEnumerable<int> GetUInt16s(
            int startAddress,
            int count,
            bool crossBoundaries);

        void WriteBytes(
            IEnumerable<byte> values,
            int startAddress,
            bool crossBoundaries);

        void WriteInt16s(
            IEnumerable<short> values,
            int startAddress,
            bool crossBoundaries);

        void WriteUInt16s(
            IEnumerable<int> values,
            int startAddress,
            bool crossBoundaries);
    }
}
