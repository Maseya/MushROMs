// <copyright file="Rom.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using static MathHelper;

public class Rom
{
    public const int DefaultHeaderSize = 0x200;

    public const int PageSize = 0x2000;
    public const int PageMask = PageSize - 1;

    // TODO(swr)
    public const int LoRomBankSize = 0x8000;
    public const int LoRomWordMask = LoRomBankSize - 1;
    public const int BankMask = 0xFF_0000;

    public const int LoRomBankLimit = 0x80;
    public const int LoRomSizeLimit = LoRomBankLimit * LoRomBankSize;

    public const int ExLoRomBankLimit = 0xFE;
    public const int ExLoRomSizeLimit = ExLoRomBankLimit * LoRomBankSize;

    public const int MakerCodeAddress = 0xFFB0;
    public const int MakerCodeSize = 2;
    public const int GameCodeAddress = 0xFFB2;
    public const int GameCodeSize = 4;
    public const int ExpansionRAMSizeAddress = 0xFFBD;
    public const int SpecialVersionAddress = 0xFFBE;
    public const int CartTypeSubNumberAddress = 0xFFBF;
    public const int GameTitleAddress = 0xFFC0;
    public const int GameTitleSize = 0x16;
    public const int MapModeAddress = 0xFFD5;
    public const int CartridgeTypeAddress = 0xFFD6;
    public const int RomSizeAddress = 0xFFD7;
    public const int MinRomSize = 7;
    public const int MaxRomSize = 0x0D;
    public const int RomSizeBaseValue = 0x400;
    public const int RamSizeAddress = 0xFFD8;
    public const int DestinationCodeAddress = 0xFFD9;
    public const int FixedValueAddress = 0xFFDA;
    public const int IntendedFixedValue = 0x33;
    public const int MaskRomVersionNumberAddress = 0xFFDB;
    public const int ComplementCheckAddress = 0xFFDC;
    public const int CheckSumAddress = 0xFFDE;

    public const int NativeCOPVectorAddress = 0xFFE4;
    public const int NativeBRKVectorAddress = 0xFFE6;
    public const int NativeAbortVectorAddress = 0xFFE8;
    public const int NativeNMIVectorAddress = 0xFFEA;
    public const int NativeResetVectorAddress = 0xFFEC;
    public const int NativeIRQVectorAddress = 0xFFEE;

    public const int EmuCOPVectorAddress = 0xFFF4;
    public const int EmuBRKVectorAddress = 0xFFF6;
    public const int EmuAbortVectorAddress = 0xFFF8;
    public const int EmuNMIVectorAddress = 0xFFFA;
    public const int EmuResetVectorAddress = 0xFFFC;
    public const int EmuIRQVectorAddress = 0xFFFE;

    private static readonly int JisX201CodePage = 50221;

    private static Encoding? _gameTitleEncoding = null;

    public Rom(byte[] data)
    {
        var headerSize = data.Length & PageMask;
        if (headerSize is not 0 and not DefaultHeaderSize)
        {
            throw new ArgumentException(
                "Rom does not have valid header size (0 or 512 bytes).");
        }

        AddressMode = GetAddressMode(
            new ReadOnlySpan<byte>(data, headerSize, data.Length - headerSize));
        Data = new byte[data.Length];
        Array.Copy(data, Data, data.Length);
    }

    public int Size
    {
        get
        {
            return Data.Length;
        }
    }

    public bool HasHeader
    {
        get
        {
            return HeaderSize == DefaultHeaderSize;
        }
    }

    public int HeaderSize
    {
        get
        {
            return Data.Length & LoRomWordMask;
        }
    }

    public int HeaderlessSize
    {
        get
        {
            return Size - HeaderSize;
        }
    }

    public AddressMode AddressMode
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets or sets two alphanumeric ASCII bytes identifying your company. Ignored by
    /// emulators; for ROM hackers and home-brewers, just insert whatever.
    /// </summary>
    public string MakerCode
    {
        get
        {
            var result = ReadBytes(MakerCodeAddress, MakerCodeSize);
            return Encoding.ASCII.GetString(result);
        }

        set
        {
            var bytes = Encoding.ASCII.GetBytes(value);
            var realLength = Math.Min(bytes.Length - 1, MakerCodeSize);
            var result = new byte[MakerCodeSize];
            Array.Copy(bytes, result, realLength);
            WriteBytes(MakerCodeAddress, result);
        }
    }

    /// <summary>
    /// Get or sets four alphanumeric ASCII bytes identifying your game. Ignored by
    /// emulators; for ROM hackers and home-brewers, just insert whatever.
    /// Exception: If the game code starts with Z and ends with J, it's a BS-X flash cartridge.
    /// </summary>
    public string GameCode
    {
        get
        {
            var result = ReadBytes(GameCodeAddress, GameCodeSize);
            return Encoding.ASCII.GetString(result);
        }

        set
        {
            var bytes = Encoding.ASCII.GetBytes(value);
            var realLength = Math.Min(bytes.Length - 1, GameCodeSize);
            var result = new byte[GameCodeSize];
            Array.Copy(bytes, result, realLength);
            WriteBytes(GameCodeAddress, result);
        }
    }

    /// <summary>
    /// Gets or sets the size of the expansion RAM installed in the game pak.
    /// </summary>
    public RamSize ExpansionRAMSize
    {
        get
        {
            return (RamSize)ReadByte(ExpansionRAMSizeAddress);
        }

        set
        {
            WriteByte(ExpansionRAMSizeAddress, (int)value);
        }
    }

    public bool IsValidExpansionRamSize
    {
        get
        {
            return Enum.IsDefined(typeof(RamSize), ExpansionRAMSize);
        }
    }

    /// <summary>
    /// Gets or sets a value that should be 0x00 under normal circumstances. This value
    /// is usually used during promotional events.
    /// </summary>
    public int SpecialVersion
    {
        get
        {
            return ReadByte(SpecialVersionAddress);
        }

        set
        {
            WriteByte(SpecialVersionAddress, value);
        }
    }

    /// <summary>
    /// Gets or sets a value only assigned when it is necessary to distinguish between
    /// games which use the same cartridge type. Should normally be 00H.
    /// </summary>
    public int CartTypeSubNumber
    {
        get
        {
            return ReadByte(CartTypeSubNumberAddress);
        }

        set
        {
            WriteByte(CartTypeSubNumberAddress, value);
        }
    }

    /// <summary>
    /// Gets or sets the game title, which is 21 bytes long, encoded with the JIS X
    /// 0201 character set (which consists of standard ASCII plus katakana). If the
    /// title is shorter than 21 bytes, then the remainder should be padded with spaces
    /// (0x20).
    /// </summary>
    public string GameTitle
    {
        get
        {
            var result = ReadBytes(GameTitleAddress, GameTitleSize);
            return GameTitleEncoding.GetString(result).TrimEnd().Replace('\\', '¥');
        }

        set
        {
            var bytes = GameTitleEncoding.GetBytes(value.Replace('¥', '\\'));
            var realLength = Math.Min(bytes.Length - 1, GameTitleSize);
            var result = new byte[GameTitleSize];
            Array.Fill(result, (byte)' ');
            Array.Copy(bytes, result, realLength);
            WriteBytes(GameTitleAddress, result);
        }
    }

    /// <summary>
    /// Gets or sets a value determining whether this is a HiROM or LoROM game.
    /// </summary>
    public MapMode MapMode
    {
        get
        {
            return (MapMode)ReadByte(MapModeAddress);
        }

        set
        {
            WriteByte(MapModeAddress, (int)value);
        }
    }

    public bool IsFastROM
    {
        get
        {
            return ((int)MapMode & 0x30) == 0x30;
        }
    }

    public bool IsHiROM
    {
        get
        {
            return ((int)MapMode & 1) != 0;
        }
    }

    public CartridgeType CartridgeType
    {
        get
        {
            return (CartridgeType)ReadByte(CartridgeTypeAddress);
        }

        set
        {
            WriteByte(CartridgeTypeAddress, (int)value);
        }
    }

    public bool IsCartridgeTypeValid
    {
        get
        {
            return Enum.IsDefined(typeof(CartridgeType), CartridgeType);
        }
    }

    public RomSize RomSize
    {
        get
        {
            return (RomSize)ReadByte(RomSizeAddress);
        }

        set
        {
            WriteByte(RomSizeAddress, (int)value);
        }
    }

    public int RomCapacity
    {
        get
        {
            return RomSizeBaseValue << (int)RomSize;
        }
    }

    public bool IsRomSizeValid
    {
        get
        {
            return Enum.IsDefined(typeof(RomSize), RomSize)
                && HeaderlessSize <= RomCapacity
                && HeaderlessSize > RomCapacity >> 1;
        }
    }

    public RamSize RamSize
    {
        get
        {
            return (RamSize)ReadByte(RamSizeAddress);
        }

        set
        {
            WriteByte(RamSizeAddress, (int)value);
        }
    }

    public bool IsRamSizeValid
    {
        get
        {
            return Enum.IsDefined(typeof(RamSize), RamSize);
        }
    }

    public DestinationCode DestinationCode
    {
        get
        {
            return (DestinationCode)ReadByte(DestinationCodeAddress);
        }

        set
        {
            WriteByte(DestinationCodeAddress, (int)value);
        }
    }

    public bool IsDestinationCodeValid
    {
        get
        {
            return Enum.IsDefined(typeof(DestinationCode), DestinationCode);
        }
    }

    /// <summary>
    /// Gets or sets a value that should always be 0x33.
    /// </summary>
    public int FixedValue
    {
        get
        {
            return ReadByte(FixedValueAddress);
        }

        set
        {
            WriteByte(FixedValueAddress, value);
        }
    }

    public bool IsFixedValueValid
    {
        get
        {
            return FixedValue == IntendedFixedValue;
        }
    }

    /// <summary>
    /// Gets or sets a value that stores the version number of the ROM released to the
    /// market as a product. The number begins with 0 at production and increases with
    /// each revised version.
    /// </summary>
    public int MaskRomVersion
    {
        get
        {
            return ReadByte(MaskRomVersionNumberAddress);
        }

        set
        {
            WriteByte(MaskRomVersionNumberAddress, value);
        }
    }

    /// <summary>
    /// Gets or sets the 16-bit complements (bit-inverse) of <see cref="Checksum"/>.
    /// </summary>
    /// <remarks>
    /// Summing <see cref="ComplementCheck"/> and <see cref="Checksum"/> should always
    /// equal 0xFFFF.
    /// </remarks>
    public int ComplementCheck
    {
        get
        {
            return ReadInt16(ComplementCheckAddress);
        }

        set
        {
            WriteInt16(ComplementCheckAddress, value);
        }
    }

    /// <summary>
    /// Gets or sets the checksum of the ROM.
    /// </summary>
    public int Checksum
    {
        get
        {
            return ReadInt16(CheckSumAddress);
        }

        set
        {
            WriteInt16(CheckSumAddress, value);
        }
    }

    public bool IsComplementCheckValid
    {
        get
        {
            return (Checksum ^ ComplementCheck) == Int16.MaxValue;
        }
    }

    public int RealChecksum
    {
        get
        {
            var sum = 0;
            for (var i = HeaderSize; i < Data.Length; i++)
            {
                sum += Data[i];
            }

            var repeatBase = BitCeil(HeaderlessSize) / 2;
            var remainder = HeaderlessSize - repeatBase;
            for (var i = HeaderlessSize; i < RomCapacity; i++)
            {
                sum += Data[HeaderSize + repeatBase + (i % remainder)];
            }

            return sum & UInt16.MaxValue;
        }
    }

    public bool IsChecksumValid
    {
        get
        {
            return Checksum == RealChecksum;
        }
    }

    public int NativeCOPVector
    {
        get
        {
            return ReadInt16(NativeCOPVectorAddress);
        }

        set
        {
            WriteInt16(NativeCOPVectorAddress, value);
        }
    }

    public int NativeBRKVector
    {
        get
        {
            return ReadInt16(NativeBRKVectorAddress);
        }

        set
        {
            WriteInt16(NativeBRKVectorAddress, value);
        }
    }

    public int NativeAbortVector
    {
        get
        {
            return ReadInt16(NativeAbortVectorAddress);
        }

        set
        {
            WriteInt16(NativeAbortVectorAddress, value);
        }
    }

    public int NativeNMIVector
    {
        get
        {
            return ReadInt16(NativeNMIVectorAddress);
        }

        set
        {
            WriteInt16(NativeNMIVectorAddress, value);
        }
    }

    public int NativeResetVector
    {
        get
        {
            return ReadInt16(NativeResetVectorAddress);
        }

        set
        {
            WriteInt16(NativeResetVectorAddress, value);
        }
    }

    public int NativeIRQVector
    {
        get
        {
            return ReadInt16(NativeIRQVectorAddress);
        }

        set
        {
            WriteInt16(NativeIRQVectorAddress, value);
        }
    }

    public int EmuCOPVector
    {
        get
        {
            return ReadInt16(EmuCOPVectorAddress);
        }

        set
        {
            WriteInt16(EmuCOPVectorAddress, value);
        }
    }

    public int EmuBRKVector
    {
        get
        {
            return ReadInt16(EmuBRKVectorAddress);
        }

        set
        {
            WriteInt16(EmuBRKVectorAddress, value);
        }
    }

    public int EmuNMIVector
    {
        get
        {
            return ReadInt16(EmuNMIVectorAddress);
        }

        set
        {
            WriteInt16(EmuNMIVectorAddress, value);
        }
    }

    public int EmuResetVector
    {
        get
        {
            return ReadInt16(EmuResetVectorAddress);
        }

        set
        {
            WriteInt16(EmuResetVectorAddress, value);
        }
    }

    public int EmuIRQVector
    {
        get
        {
            return ReadInt16(EmuIRQVectorAddress);
        }

        set
        {
            WriteInt16(EmuIRQVectorAddress, value);
        }
    }

    public static Encoding GameTitleEncoding
    {
        get
        {
            if (_gameTitleEncoding is null)
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                _gameTitleEncoding = Encoding.GetEncoding(
                    codepage: JisX201CodePage);
            }

            return _gameTitleEncoding;
        }
    }

    private byte[] Data
    {
        get;
    }

    private bool IsValidAddressMode
    {
        get
        {
            return HeaderlessSize >= AddressMode.MinSize()
                && AddressMode.IsHiRom() == IsHiROM
                && (AddressMode.IsExRom() || AddressMode.IsFastRom() == IsFastROM)
                && !IsComplementCheckValid
                && IsRomSizeValid;
        }
    }

    public static bool IsValidAddress(int pointer, AddressMode mode)
    {
        return mode switch
            {
                AddressMode.LoRom => IsValidPointer(pointer),
                AddressMode.LoRom2 => IsValidPointer(pointer),
                AddressMode.ExLoRom => IsValidExLoRomPointer(pointer),
                AddressMode.HiRom => IsValidPointer(pointer),
                AddressMode.HiRom2 => IsValidPointer(pointer),
                AddressMode.ExHiRom => IsValidExHiRomPointer(pointer),
                _ => throw new InvalidEnumArgumentException(
                    nameof(mode),
                    (int)mode,
                    typeof(AddressMode)),
            };
    }

    public static int IncrementSnesAddress(
        int snesAddress,
        int amount,
        bool crossBanks = true)
    {
        if (crossBanks)
        {
            return snesAddress + amount;
        }

        // TODO(swr): Account for HiROM.
        var bank = snesAddress & BankMask;
        var word = snesAddress & LoRomWordMask;
        return bank | 0x8000 | ((word + amount) & LoRomWordMask);
    }

    public static int SnesToPc(
        int pointer,
        bool hasHeader,
        AddressMode mode)
    {
        if (pointer < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(pointer),
                "SNES address cannot be less than zero.");
        }

        var header = hasHeader ? DefaultHeaderSize : 0;
        int result;
        switch (mode)
        {
        case AddressMode.LoRom:
        case AddressMode.LoRom2:
            result = ((pointer & 0x7F_0000) >> 1) | (pointer & 0x7FFF);
            break;

        case AddressMode.HiRom:
        case AddressMode.HiRom2:
            result = pointer & 0x3F_FFFF;
            break;

        case AddressMode.ExHiRom:
            result = pointer & 0x3F_FFFF;
            if (pointer < 0xC0_0000)
            {
                result |= 0x40_0000;
            }

            break;

        case AddressMode.ExLoRom:
            result = ((pointer & 0x7F_0000) >> 1) | (pointer & 0x7FFF);
            if (pointer < 0x80_0000)
            {
                result |= 0x40_0000;
            }

            break;

        default:
            throw new InvalidEnumArgumentException(
                nameof(mode),
                (int)mode,
                typeof(AddressMode));
        }

        return result + header;
    }

    public static int PcToSnes(int pointer, bool hasHeader, AddressMode mode)
    {
        pointer -= hasHeader ? DefaultHeaderSize : 0;
        if (pointer < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(pointer),
                "PC Address cannot be less than zero.");
        }

        int result;
        switch (mode)
        {
        case AddressMode.LoRom:
            result = ((pointer << 1) & 0x7F_0000) | 0x8000 | (pointer & 0x7FFF);
            if (pointer >= 0x38_0000)
            {
                result |= 0x80_0000;
            }

            break;

        case AddressMode.HiRom:
            result = 0xC0_0000 | pointer;
            break;

        case AddressMode.ExHiRom:
            result = pointer >= 0x7E_0000
                ? ~0x40_0000 & pointer
                : pointer < 0x40_0000
                ? 0xC0_0000 | pointer
                : pointer;
            break;

        case AddressMode.ExLoRom:
            result = ((pointer << 1) & 0x7F_0000) | 0x8000 | (pointer & 0x7FFF);
            if (pointer < 0x40_0000)
            {
                result |= 0x80_0000;
            }

            break;

        case AddressMode.LoRom2:
            result = 0x80_0000
                | ((pointer << 1) & 0x7F_0000) | 0x8000 | (pointer & 0x7FFF);
            break;

        case AddressMode.HiRom2:
            result = 0x40_0000 | pointer;
            if (pointer >= 0x30_0000)
            {
                result |= 0x80_0000;
            }

            break;

        default:
            throw new InvalidEnumArgumentException(
                nameof(mode),
                (int)mode,
                typeof(AddressMode));
        }

        return result;
    }

    public bool IsValidAddress(int pointer)
    {
        return IsValidAddress(pointer, AddressMode)
            && SnesToPc(pointer) < RomCapacity;
    }

    public byte ReadByte(int snesAddress)
    {
        var pcAddress = SnesToPc(snesAddress);
        return Data[pcAddress];
    }

    public byte ReadByteIndirect(int snesAddress)
    {
        return ReadByteIndirectIndexed(snesAddress, 0, false);
    }

    public byte ReadByteIndexed(
        int snesAddress,
        int index,
        bool crossBanks = true)
    {
        var address = IncrementSnesAddress(snesAddress, index, crossBanks);
        return ReadByte(address);
    }

    public byte ReadByteIndirectIndexed(
        int snesAddress,
        int index,
        bool crossBanks = true)
    {
        var bank = snesAddress & BankMask;
        var word = ReadInt16(snesAddress);
        return ReadByteIndexed(bank | word, index, crossBanks);
    }

    public int ReadInt16(int snesAddress, bool crossBanks = true)
    {
        return ReadInt16(
            snesAddress,
            IncrementSnesAddress(snesAddress, 1, crossBanks));
    }

    public int ReadInt16(int snesAddressLow, int snesAddressHigh)
    {
        var low = ReadByte(snesAddressLow);
        var high = ReadByte(snesAddressHigh);
        return low | (high << 8);
    }

    public int ReadInt16Indexed(
        int snesAddress,
        int index,
        bool crossBanks = true)
    {
        var address = IncrementSnesAddress(snesAddress, index, crossBanks);
        return ReadInt16(address, crossBanks);
    }

    public int ReadInt16Indexed(
        int snesAddressLow,
        int snesAddressHigh,
        int index,
        bool crossBanks = true)
    {
        var low = ReadByteIndexed(snesAddressLow, index, crossBanks);
        var high = ReadByteIndexed(snesAddressHigh, index, crossBanks);
        return low | (high << 8);
    }

    public int ReadInt16Indirect(int snesAddress)
    {
        return ReadInt16IndirectIndexed(snesAddress, 0);
    }

    public int ReadInt16Indirect(int snesAddressLow, int snesAddressHigh)
    {
        return ReadByteIndirect(snesAddressLow)
            | (ReadByteIndirect(snesAddressHigh) << 8);
    }

    public int ReadInt16IndirectIndexed(
        int snesAddress,
        int index,
        bool crossBanks = true)
    {
        var bank = snesAddress & BankMask;
        var word = ReadInt16(snesAddress);
        return ReadInt16Indexed(bank | word, index, crossBanks);
    }

    public int ReadInt16IndirectIndexed(
        int snesAddressLow,
        int snesAddressHigh,
        int index,
        bool crossBanks = true)
    {
        return ReadByteIndirectIndexed(snesAddressLow, index, crossBanks)
            | (ReadByteIndirectIndexed(snesAddressHigh, index, crossBanks) << 8);
    }

    public void ReadBytes(
        int snesAddress,
        int count,
        Span<byte> dest,
        bool crossBanks = true)
    {
        ReadBytes(snesAddress, dest[..count], crossBanks);
    }

    public void ReadBytes(
        int snesAddress,
        Span<byte> dest,
        bool crossBanks = true)
    {
        var count = dest.Length;

        // TODO(swr): Handle HiROM.
        var subCount = crossBanks
            ? count
            : LoRomBankSize - (snesAddress & LoRomWordMask);
        var index = 0;
        while (count > 0)
        {
            var pcAddress = SnesToPc(snesAddress);
            new Span<byte>(Data, pcAddress, subCount).CopyTo(dest[index..]);

            index += subCount;
            count -= subCount;
            subCount = Math.Min(count, LoRomBankSize);
            snesAddress &= BankMask;
        }
    }

    public byte[] ReadBytes(
        int snesAddress,
        int count,
        bool crossBanks = true)
    {
        var result = new byte[count];
        ReadBytes(snesAddress, result, crossBanks);
        return result;
    }

    public IEnumerable<byte> EnumerateBytes(
        int snesAddress,
        int count,
        bool crossBanks = true)
    {
        var pcAddress = SnesToPc(snesAddress);
        if (crossBanks)
        {
            while (count > 0)
            {
                yield return Data[pcAddress];
                count--;
                if (++pcAddress == Data.Length)
                {
                    pcAddress = HeaderSize;
                }
            }
        }
        else
        {
            var subCount = LoRomBankSize - (snesAddress & LoRomWordMask);
            while (count > 0)
            {
                for (var i = 0; i < subCount && count > 0; i++)
                {
                    yield return Data[pcAddress + i];
                    count--;
                }

                subCount = BankMask;
                pcAddress &= ~LoRomWordMask;
            }
        }
    }

    public void ReadBytesIndirect(
        int snesAddress,
        Span<byte> dest,
        bool crossBanks = true)
    {
        ReadBytesIndirectIndexed(snesAddress, 0, dest, crossBanks);
    }

    public byte[] ReadBytesIndirect(
        int snesAddress,
        int count,
        bool crossBanks = true)
    {
        var result = new byte[count];
        ReadBytesIndirect(snesAddress, result, crossBanks);
        return result;
    }

    public void ReadBytesIndexed(
        int snesAddress,
        int index,
        Span<byte> dest,
        bool crossBanks = true)
    {
        var address = IncrementSnesAddress(snesAddress, index, crossBanks);
        ReadBytes(address, dest, crossBanks);
    }

    public byte[] ReadBytesIndexed(
        int snesAddress,
        int index,
        int count,
        bool crossBanks = true)
    {
        var result = new byte[count];
        ReadBytesIndexed(snesAddress, index, result, crossBanks);
        return result;
    }

    public void ReadBytesIndirectIndexed(
        int snesAddress,
        int index,
        Span<byte> dest,
        bool crossBanks = true)
    {
        var bank = snesAddress & BankMask;
        var word = ReadInt16(snesAddress);
        ReadBytesIndexed(
            bank | word,
            index,
            dest,
            crossBanks);
    }

    public byte[] ReadBytesIndirectIndexed(
        int snesAddress,
        int index,
        int count,
        bool crossBanks = true)
    {
        var result = new byte[count];
        ReadBytesIndirectIndexed(snesAddress, index, result, crossBanks);
        return result;
    }

    public void ReadInt16Array(
        int snesAddress,
        Span<short> dest,
        bool crossBanks = true)
    {
        unsafe
        {
            fixed (short* ptr = dest)
            {
                ReadBytes(
                    snesAddress,
                    dest.Length * sizeof(short),
                    new Span<byte>((byte*)ptr, dest.Length * sizeof(short)),
                    crossBanks);
            }
        }
    }

    public short[] ReadInt16Array(
        int snesAddress,
        int count,
        bool crossBanks = true)
    {
        var result = new short[count];
        ReadInt16Array(snesAddress, result, crossBanks);
        return result;
    }

    public void ReadInt16ArrayIndirect(
        int snesAddress,
        Span<short> dest,
        bool crossBanks = true)
    {
        var bank = snesAddress & BankMask;
        var word = ReadInt16(snesAddress);
        ReadInt16Array(bank | word, dest, crossBanks);
    }

    public short[] ReadInt16ArrayIndirect(
        int snesAddress,
        int count,
        bool crossBanks = true)
    {
        var result = new short[count];
        ReadInt16ArrayIndirect(snesAddress, result, crossBanks);
        return result;
    }

    public void ReadInt16ArrayIndexed(
        int snesAddress,
        int index,
        Span<short> dest,
        bool crossBanks = true)
    {
        var address = IncrementSnesAddress(snesAddress, index, crossBanks);
        ReadInt16Array(address, dest, crossBanks);
    }

    public short[] ReadInt16ArrayIndexed(
        int snesAddress,
        int index,
        int count,
        bool crossBanks = true)
    {
        var result = new short[count];
        ReadInt16ArrayIndexed(snesAddress, index, result, crossBanks);
        return result;
    }

    public void ReadInt16ArrayIndirectIndexed(
        int snesAddress,
        int index,
        Span<short> dest,
        bool crossBanks = true)
    {
        var bank = snesAddress & BankMask;
        var word = ReadInt16(snesAddress);
        ReadInt16ArrayIndexed(
            bank | word,
            index,
            dest,
            crossBanks);
    }

    public short[] ReadInt16ArrayIndirectIndexed(
        int snesAddress,
        int index,
        int count,
        bool crossBanks = true)
    {
        var result = new short[count];
        ReadInt16ArrayIndirectIndexed(snesAddress, index, result, crossBanks);
        return result;
    }

    public void ReadInt16ArrayAs<T>(
        int snesAddress,
        Span<T> dest,
        Func<short, T> func,
        bool crossBanks = true)
    {
        var source = ReadInt16Array(snesAddress, dest.Length, crossBanks);
        for (var i = 0; i < dest.Length; i++)
        {
            dest[i] = func(source[i]);
        }
    }

    public T[] ReadInt16ArrayAs<T>(
        int snesAddress,
        int count,
        Func<short, T> func,
        bool crossBanks = true)
    {
        var result = new T[count];
        ReadInt16ArrayAs(snesAddress, result, func, crossBanks);
        return result;
    }

    public void ReadInt16ArrayIndirectAs<T>(
        int snesAddress,
        Span<T> dest,
        Func<short, T> func,
        bool crossBanks = true)
    {
        var bank = snesAddress & BankMask;
        var word = ReadInt16(snesAddress);
        ReadInt16ArrayAs(bank | word, dest, func, crossBanks);
    }

    public T[] ReadInt16ArrayIndirectAs<T>(
        int snesAddress,
        int count,
        Func<short, T> func,
        bool crossBanks = true)
    {
        var result = new T[count];
        ReadInt16ArrayIndirectAs(snesAddress, result, func, crossBanks);
        return result;
    }

    public void WriteByte(int snesAddress, int value)
    {
        var pcAddress = SnesToPc(snesAddress);
        Data[pcAddress] = (byte)value;
    }

    public void WriteByteIndirect(int snesAddress, int value)
    {
        var bank = snesAddress & BankMask;
        var word = ReadInt16(snesAddress);
        WriteByte(bank | word, value);
    }

    public void WriteInt16(
        int snesAddress,
        int value,
        bool crossBanks = true)
    {
        WriteInt16(
            snesAddress,
            IncrementSnesAddress(snesAddress, 1, crossBanks),
            value);
    }

    public void WriteInt16Indirect(
        int snesAddress,
        int value,
        bool crossBanks = true)
    {
        var bank = snesAddress & BankMask;
        var word = ReadInt16(snesAddress);
        WriteInt16(bank | word, value, crossBanks);
    }

    public void WriteInt16(
        int snesAddressLow,
        int snesAddressHigh,
        int value)
    {
        WriteByte(snesAddressLow, value);
        WriteByte(snesAddressHigh, value >> 8);
    }

    public void WriteInt16Indirect(
        int snesAddressLow,
        int snesAddressHigh,
        int value)
    {
        WriteByteIndirect(snesAddressLow, value);
        WriteByteIndirect(snesAddressHigh, value >> 8);
    }

    public void WriteBytes(
        int snesAddress,
        ReadOnlySpan<byte> bytes,
        bool crossBanks = true)
    {
        var pcAddress = SnesToPc(snesAddress);
        if (crossBanks || IsArrayInBank(bytes.Length, snesAddress))
        {
            bytes.CopyTo(new Span<byte>(Data, pcAddress, bytes.Length));
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    public void WriteBytesIndirect(
        int snesAddress,
        ReadOnlySpan<byte> bytes,
        bool crossBanks = true)
    {
        WriteBytesIndirectIndexed(snesAddress, 0, bytes, crossBanks);
    }

    public void WriteBytesIndirectIndexed(
        int snesAddress,
        int index,
        ReadOnlySpan<byte> bytes,
        bool crossBanks = true)
    {
        var bank = snesAddress & BankMask;
        var word = ReadInt16(snesAddress);
        var address = IncrementSnesAddress(bank | word, index, crossBanks);
        WriteBytes(address, bytes, crossBanks);
    }

    public void WriteInt16Array(
        int snesAddress,
        ReadOnlySpan<short> words,
        bool crossBanks = true)
    {
        unsafe
        {
            fixed (short* ptr = words)
            {
                WriteBytes(
                    snesAddress,
                    new ReadOnlySpan<byte>(
                        (byte*)ptr,
                        words.Length * sizeof(short)),
                    crossBanks);
            }
        }
    }

    public void WriteInt16ArrayIndirect(
        int snesAddress,
        ReadOnlySpan<short> words,
        bool crossBanks = true)
    {
        WriteInt16ArrayIndirectIndexed(snesAddress, 0, words, crossBanks);
    }

    public void WriteInt16ArrayIndirectIndexed(
        int snesAddress,
        int index,
        ReadOnlySpan<short> words,
        bool crossBanks = true)
    {
        var bank = snesAddress & BankMask;
        var word = ReadInt16(snesAddress);
        var address = IncrementSnesAddress(bank | word, index, crossBanks);
        WriteInt16Array(address, words, crossBanks);
    }

    public void WriteArrayAsInt16<T>(
        int snesAddress,
        ReadOnlySpan<T> data,
        Func<T, short> func,
        bool crossBanks = true)
    {
        var source = new short[data.Length];
        for (var i = 0; i < data.Length; i++)
        {
            source[i] = func(data[i]);
        }

        WriteInt16Array(snesAddress, source, crossBanks);
    }

    public void WriteArrayAsInt16Indirect<T>(
        int snesAddress,
        ReadOnlySpan<T> data,
        Func<T, short> func,
        bool crossBanks = true)
    {
        var bank = snesAddress & BankMask;
        var word = ReadInt16(snesAddress);
        WriteArrayAsInt16(bank | word, data, func, crossBanks);
    }

    public byte[] GetData()
    {
        var result = new byte[Data.Length];
        Array.Copy(Data, result, Data.Length);
        return result;
    }

    public static int XBA(int value)
    {
        return (ushort)(((ushort)value << 8) | ((ushort)value >> 8));
    }

    private static bool IsArrayInBank(int count, int snesAddress)
    {
        return count + (snesAddress & LoRomWordMask) <= LoRomBankSize;
    }

    private static bool IsValidPointer(int pointer)
    {
        return
            (uint)(pointer & ~0x80_0000) < 0x40_0000
            ? (pointer & 0x8000) != 0
            : (uint)(pointer - 0x40_0000) < 0x70_0000 - 0x40_0000 ||
                (uint)(pointer - 0xC0_0000) < 0x100_0000 - 0xC0_0000;
    }

    private static bool IsValidExLoRomPointer(int pointer)
    {
        return
            (uint)(pointer & ~0x80_0000) < 0x40_0000 ||
                (uint)(pointer - 0x70_0000) < 0x7E_0000 - 0x70_0000
            ? (pointer & 0x8000) != 0
            : (uint)(pointer - 0x40_0000) < 0x7E_0000 - 0x40_0000 ||
                (uint)(pointer - 0xC0_0000) < 0x100_0000 - 0xC0_0000;
    }

    private static bool IsValidExHiRomPointer(int pointer)
    {
        return 
            (uint)(pointer & ~0x80_0000) < 0x40_0000 ||
                (uint)(pointer - 0x70_0000) < 0x78_0000 - 0x70_0000
            ? (pointer & 0x8000) != 0
            : (uint)(pointer - 0x40_0000) < 0x7E_0000 - 0x40_0000 ||
                (uint)(pointer - 0xC0_0000) < 0x100_0000 - 0xC0_0000;
    }

    private static AddressMode GetAddressMode(ReadOnlySpan<byte> headerlessData)
    {
        var loRomHeader = new RomHeader(
            headerlessData.Length,
            headerlessData.Slice(0x7FB0, 0x50),
            AddressMode.LoRom);
        var bestHeader = loRomHeader;

        if (headerlessData.Length >= 0x1_0000)
        {
            var hiRomHeader = new RomHeader(
                headerlessData.Length,
                headerlessData.Slice(0xFFB0, 0x50),
                AddressMode.HiRom);
            if (bestHeader.Score < hiRomHeader.Score)
            {
                bestHeader = hiRomHeader;
            }

            if (headerlessData.Length >= 0x40_8000)
            {
                var exLoRomHeader = new RomHeader(
                    headerlessData.Length,
                    headerlessData.Slice(0x407FB0, 0x50),
                    AddressMode.ExLoRom);
                if (bestHeader.Score < exLoRomHeader.Score)
                {
                    bestHeader = exLoRomHeader;
                }

                if (headerlessData.Length >= 0x41_0000)
                {
                    var exHiRomHeader = new RomHeader(
                        headerlessData.Length,
                        headerlessData.Slice(0x40FFB0, 0x50),
                        AddressMode.ExHiRom);
                    if (bestHeader.Score < exHiRomHeader.Score)
                    {
                        bestHeader = exHiRomHeader;
                    }
                }
            }
        }

        return bestHeader.Score > 0
            ? bestHeader.AddressMode
            : throw new ArgumentException("Could not find suitable address mode.");
    }

    private int SnesToPc(int pointer)
    {
        return SnesToPc(pointer, HasHeader, AddressMode);
    }
}
