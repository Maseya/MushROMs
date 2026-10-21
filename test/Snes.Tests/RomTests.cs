// <copyright file="RomTests.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes.Tests;

using System;
using System.ComponentModel;
using System.Text;
using System.Text.Json;

using Xunit;

public class RomTests
{
    private static Dictionary<string, TestRomGenerator>? _testRoms = null;

    private static Dictionary<string, TestRomGenerator> TestRoms
    {
        get
        {
            _testRoms ??= JsonSerializer.Deserialize<Dictionary<string, TestRomGenerator>>(
                    Properties.Resources.RomFormatTestData)!;

            return _testRoms;
        }
    }

    [Theory]
    // TODO(swr): I need Test ROMs for different address modes.
    [InlineData("Test ROM", "", "??")]
    [InlineData("Test ROM", "?", "??")]
    [InlineData("Test ROM", "??", "??")]
    [InlineData("Test ROM", "???", "??")]
    [InlineData("Test ROM", " ", " ?")]
    [InlineData("Test ROM", "A", "A?")]
    [InlineData("Test ROM", "AB", "AB")]
    [InlineData("Test ROM", "ABCD", "AB")]
    [InlineData("Test ROM", " AB", " A")]
    public void MakerCode_ValidMakerCode_ReturnsExpectedMakerCode(
        string romName,
        string inputCode,
        string expectedMakerCode)
    {
        var rom = TestRoms[romName].CreateRom();
        rom.MakerCode = inputCode;

        Assert.Equal(expectedMakerCode, rom.MakerCode);
    }

    [Theory]
    [InlineData("Test ROM", "", "????")]
    [InlineData("Test ROM", "?", "????")]
    [InlineData("Test ROM", "??", "????")]
    [InlineData("Test ROM", "???", "????")]
    [InlineData("Test ROM", "????", "????")]
    [InlineData("Test ROM", "??????", "????")]
    [InlineData("Test ROM", " ", " ???")]
    [InlineData("Test ROM", "A", "A???")]
    [InlineData("Test ROM", "AB", "AB??")]
    [InlineData("Test ROM", "ABCD", "ABCD")]
    [InlineData("Test ROM", " AB", " AB?")]
    [InlineData("Test ROM", "ABCDEF", "ABCD")]
    public void GameCode_ValidGameCode_ReturnsExpectedGameCode(
        string romName,
        string inputCode,
        string expectedGameCode)
    {
        var rom = TestRoms[romName].CreateRom();
        rom.GameCode = inputCode;

        Assert.Equal(expectedGameCode, rom.GameCode);
    }

    [Fact]
    public void SnesToPc_InvalidAddressMode_ThrowsInvalidEnumException()
    {
        _ = Assert.Throws<InvalidEnumArgumentException>(
            () => _ = Rom.SnesToPc(0x8000, false, (AddressMode)(-1)));
    }

    [Theory]
    [InlineData(0x00_8000, 0x00_0000)]
    [InlineData(0x00_FFFF, 0x00_7FFF)]
    [InlineData(0x01_8000, 0x00_8000)]
    [InlineData(0x3F_FFFF, 0x1F_FFFF)]
    [InlineData(0x40_0000, 0x20_0000)]
    [InlineData(0x40_8000, 0x20_0000)]
    [InlineData(0x6F_FFFF, 0x37_FFFF)]
    [InlineData(0x80_8000, 0x00_0000)]
    [InlineData(0x80_FFFF, 0x00_7FFF)]
    [InlineData(0x81_8000, 0x00_8000)]
    [InlineData(0xBF_FFFF, 0x1F_FFFF)]
    [InlineData(0xC0_0000, 0x20_0000)]
    [InlineData(0xC0_8000, 0x20_0000)]
    [InlineData(0xEF_FFFF, 0x37_FFFF)]
    [InlineData(0xFF_FFFF, 0x3F_FFFF)]
    public void SnesToPc_ValidLoRomAddress_ReturnsValidPcAddress(
        int snesAddress,
        int expectedPcAddress)
    {
        SnesToPc_ValidInput_ReturnsValidAddress(
            snesAddress,
            false,
            AddressMode.LoRom,
            expectedPcAddress);
    }

    [Theory]
    [InlineData(0x00_8000, 0x00_0000)]
    [InlineData(0x00_FFFF, 0x00_7FFF)]
    [InlineData(0x01_8000, 0x00_8000)]
    [InlineData(0x3F_FFFF, 0x1F_FFFF)]
    [InlineData(0x40_0000, 0x20_0000)]
    [InlineData(0x40_8000, 0x20_0000)]
    [InlineData(0x6F_FFFF, 0x37_FFFF)]
    [InlineData(0x80_8000, 0x00_0000)]
    [InlineData(0x80_FFFF, 0x00_7FFF)]
    [InlineData(0x81_8000, 0x00_8000)]
    [InlineData(0xBF_FFFF, 0x1F_FFFF)]
    [InlineData(0xC0_0000, 0x20_0000)]
    [InlineData(0xC0_8000, 0x20_0000)]
    [InlineData(0xEF_FFFF, 0x37_FFFF)]
    [InlineData(0xFF_FFFF, 0x3F_FFFF)]
    public void SnesToPc_ValidLoRom2Address_ReturnsValidPcAddress(
        int snesAddress,
        int expectedPcAddress)
    {
        SnesToPc_ValidInput_ReturnsValidAddress(
            snesAddress,
            false,
            AddressMode.LoRom2,
            expectedPcAddress);
    }

    [Theory]
    [InlineData(0x00_8000, 0x00_8000)]
    [InlineData(0x00_FFFF, 0x00_FFFF)]
    [InlineData(0x01_8000, 0x01_8000)]
    [InlineData(0x3F_FFFF, 0x3F_FFFF)]
    [InlineData(0x40_0000, 0x00_0000)]
    [InlineData(0x40_7FFF, 0x00_7FFF)]
    [InlineData(0x40_8000, 0x00_8000)]
    [InlineData(0x6F_FFFF, 0x2F_FFFF)]
    [InlineData(0x80_8000, 0x00_8000)]
    [InlineData(0xBF_FFFF, 0x3F_FFFF)]
    [InlineData(0xC0_0000, 0x00_0000)]
    [InlineData(0xC0_7FFF, 0x00_7FFF)]
    [InlineData(0xC0_8000, 0x00_8000)]
    [InlineData(0xFF_FFFF, 0x3F_FFFF)]
    public void SnesToPc_ValidHiRomAddress_ReturnsValidPcAddress(
        int snesAddress,
        int expectedPcAddress)
    {
        SnesToPc_ValidInput_ReturnsValidAddress(
            snesAddress,
            false,
            AddressMode.HiRom,
            expectedPcAddress);
    }

    [Theory]
    [InlineData(0x00_8000, 0x00_8000)]
    [InlineData(0x00_FFFF, 0x00_FFFF)]
    [InlineData(0x01_8000, 0x01_8000)]
    [InlineData(0x3F_FFFF, 0x3F_FFFF)]
    [InlineData(0x40_0000, 0x00_0000)]
    [InlineData(0x40_7FFF, 0x00_7FFF)]
    [InlineData(0x40_8000, 0x00_8000)]
    [InlineData(0x6F_FFFF, 0x2F_FFFF)]
    [InlineData(0x80_8000, 0x00_8000)]
    [InlineData(0xBF_FFFF, 0x3F_FFFF)]
    [InlineData(0xC0_0000, 0x00_0000)]
    [InlineData(0xC0_7FFF, 0x00_7FFF)]
    [InlineData(0xC0_8000, 0x00_8000)]
    [InlineData(0xFF_FFFF, 0x3F_FFFF)]
    public void SnesToPc_ValidHiRom2Address_ReturnsValidPcAddress(
        int snesAddress,
        int expectedPcAddress)
    {
        SnesToPc_ValidInput_ReturnsValidAddress(
            snesAddress,
            false,
            AddressMode.HiRom2,
            expectedPcAddress);
    }

    [Theory]
    [InlineData(0x00_8000, 0x40_0000)]
    [InlineData(0x00_FFFF, 0x40_7FFF)]
    [InlineData(0x01_8000, 0x40_8000)]
    [InlineData(0x3F_FFFF, 0x5F_FFFF)]
    [InlineData(0x40_0000, 0x60_0000)]
    [InlineData(0x40_8000, 0x60_0000)]
    [InlineData(0x41_0000, 0x60_8000)]
    [InlineData(0x41_8000, 0x60_8000)]
    [InlineData(0x6F_FFFF, 0x77_FFFF)]
    [InlineData(0x70_8000, 0x78_0000)]
    [InlineData(0x70_FFFF, 0x78_7FFF)]
    [InlineData(0x7D_FFFF, 0x7E_FFFF)]
    [InlineData(0x80_8000, 0x00_0000)]
    [InlineData(0x81_8000, 0x00_8000)]
    [InlineData(0xBF_FFFF, 0x1F_FFFF)]
    [InlineData(0xFF_FFFF, 0x3F_FFFF)]
    public void SnesToPc_ValidExLoRomAddress_ReturnsValidPcAddress(
        int snesAddress,
        int expectedPcAddress)
    {
        SnesToPc_ValidInput_ReturnsValidAddress(
            snesAddress,
            false,
            AddressMode.ExLoRom,
            expectedPcAddress);
    }

    [Theory]
    [InlineData(0x00_8000, 0x40_8000)]
    [InlineData(0x00_FFFF, 0x40_FFFF)]
    [InlineData(0x01_8000, 0x41_8000)]
    [InlineData(0x3F_FFFF, 0x7F_FFFF)]
    [InlineData(0x40_0000, 0x40_0000)]
    [InlineData(0x40_8000, 0x40_8000)]
    [InlineData(0x6F_FFFF, 0x6F_FFFF)]
    [InlineData(0x70_8000, 0x70_8000)]
    [InlineData(0x70_FFFF, 0x70_FFFF)]
    [InlineData(0x78_0000, 0x78_0000)]
    [InlineData(0x7D_FFFF, 0x7D_FFFF)]
    [InlineData(0x80_8000, 0x40_8000)]
    [InlineData(0x81_8000, 0x41_8000)]
    [InlineData(0xBF_FFFF, 0x7F_FFFF)]
    [InlineData(0xC0_0000, 0x00_0000)]
    [InlineData(0xFF_FFFF, 0x3F_FFFF)]
    public void SnesToPc_ValidExHiRomAddress_ReturnsValidPcAddress(
        int snesAddress,
        int expectedPcAddress)
    {
        SnesToPc_ValidInput_ReturnsValidAddress(
            snesAddress,
            false,
            AddressMode.ExHiRom,
            expectedPcAddress);
    }

    [Theory]
    [InlineData(0x008000, 0x000200)]
    [InlineData(0x6FFFFF, 0x3801FF)]
    public void SnesToPc_ValidHeaderLoRomAddress_ReturnsValidPcAddress(
        int snesAddress,
        int expectedPcAddress)
    {
        SnesToPc_ValidInput_ReturnsValidAddress(
            snesAddress,
            true,
            AddressMode.LoRom,
            expectedPcAddress);
    }

    [Theory]
    [InlineData(0x008000, 0x000200)]
    [InlineData(0x6FFFFF, 0x3801FF)]
    public void SnesToPc_ValidHeaderLoRom2Address_ReturnsValidPcAddress(
        int snesAddress,
        int expectedPcAddress)
    {
        SnesToPc_ValidInput_ReturnsValidAddress(
            snesAddress,
            true,
            AddressMode.LoRom2,
            expectedPcAddress);
    }

    [Theory]
    [InlineData(0x008000, 0x008200)]
    [InlineData(0x3FFFFF, 0x4001FF)]
    [InlineData(0x400000, 0x000200)]
    [InlineData(0x6FFFFF, 0x3001FF)]
    [InlineData(0x808000, 0x008200)]
    [InlineData(0xBFFFFF, 0x4001FF)]
    [InlineData(0xC00000, 0x000200)]
    [InlineData(0xFFFFFF, 0x4001FF)]
    public void SnesToPc_ValidHeaderHiRomAddress_ReturnsValidPcAddress(
        int snesAddress,
        int expectedPcAddress)
    {
        SnesToPc_ValidInput_ReturnsValidAddress(
            snesAddress,
            true,
            AddressMode.HiRom,
            expectedPcAddress);
    }

    [Theory]
    [InlineData(0x3FFFFF, 0x4001FF)]
    [InlineData(0x400000, 0x000200)]
    [InlineData(0x6FFFFF, 0x3001FF)]
    [InlineData(0x808000, 0x008200)]
    [InlineData(0xBFFFFF, 0x4001FF)]
    [InlineData(0xC00000, 0x000200)]
    [InlineData(0xFFFFFF, 0x4001FF)]
    public void SnesToPc_ValidHeaderHiRom2Address_ReturnsValidPcAddress(
        int snesAddress,
        int expectedPcAddress)
    {
        SnesToPc_ValidInput_ReturnsValidAddress(
            snesAddress,
            true,
            AddressMode.HiRom2,
            expectedPcAddress);
    }

    [Theory]
    [InlineData(0x808000, 0x000200)]
    [InlineData(0xFFFFFF, 0x4001FF)]
    [InlineData(0x6FFFFF, 0x7801FF)]
    public void SnesToPc_ValidHeaderExLoRomAddress_ReturnsValidPcAddress(
        int snesAddress,
        int expectedPcAddress)
    {
        SnesToPc_ValidInput_ReturnsValidAddress(
            snesAddress,
            true,
            AddressMode.ExLoRom,
            expectedPcAddress);
    }

    [Theory]
    [InlineData(0x008000, 0x408200)]
    [InlineData(0x3FFFFF, 0x8001FF)]
    [InlineData(0xBFFFFF, 0x8001FF)]
    [InlineData(0xC00000, 0x000200)]
    [InlineData(0xFFFFFF, 0x4001FF)]
    public void SnesToPc_ValidHeaderExHiRomAddress_ReturnsValidPcAddress(
        int snesAddress,
        int expectedPcAddress)
    {
        SnesToPc_ValidInput_ReturnsValidAddress(
            snesAddress,
            true,
            AddressMode.ExHiRom,
            expectedPcAddress);
    }

    [Fact]
    public void PcToSnes_InvalidAddressMode_ThrowsInvalidEnumException()
    {
        static void action()
        {
            _ = Rom.PcToSnes(0, false, (AddressMode)(-1));
        }

        _ = Assert.Throws<InvalidEnumArgumentException>(action);
    }

    [Theory]
    [InlineData(Int32.MinValue, true)]
    [InlineData(0, true)]
    [InlineData(0x1FF, true)]
    [InlineData(-1, false)]
    public void PcToSnes_AddressBeforeRomData_ThrowsArgumentOutOfRangeException(
        int pointer,
        bool hasHeader)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => Rom.PcToSnes(pointer, hasHeader, AddressMode.LoRom));

        Assert.Equal("pointer", exception.ParamName);
    }

    [Theory]
    [InlineData(0x000000, 0x008000)]
    [InlineData(0x007FFF, 0x00FFFF)]
    [InlineData(0x008000, 0x018000)]
    [InlineData(0x37FFFF, 0x6FFFFF)]
    [InlineData(0x3FFFFF, 0xFFFFFF)]
    public void PcToSnes_ValidLoRomAddress_ReturnsValidPcAddress(
        int snesAddress,
        int expectedPcAddress)
    {
        PcToSnes_ValidInput_ReturnsValidAddress(
            snesAddress,
            false,
            AddressMode.LoRom,
            expectedPcAddress);
    }

    [Theory]
    [InlineData(0x000000, 0x808000)]
    [InlineData(0x007FFF, 0x80FFFF)]
    [InlineData(0x008000, 0x818000)]
    [InlineData(0x37FFFF, 0xEFFFFF)]
    [InlineData(0x3FFFFF, 0xFFFFFF)]
    public void PcToSnes_ValidLoRom2Address_ReturnsValidPcAddress(
        int snesAddress,
        int expectedPcAddress)
    {
        PcToSnes_ValidInput_ReturnsValidAddress(
            snesAddress,
            false,
            AddressMode.LoRom2,
            expectedPcAddress);
    }

    [Theory]
    [InlineData(0x000000, 0xC00000)]
    [InlineData(0x3FFFFF, 0xFFFFFF)]
    public void PcToSnes_ValidHiRomAddress_ReturnsValidPcAddress(
        int snesAddress,
        int expectedPcAddress)
    {
        PcToSnes_ValidInput_ReturnsValidAddress(
            snesAddress,
            false,
            AddressMode.HiRom,
            expectedPcAddress);
    }

    [Theory]
    [InlineData(0x000000, 0x400000)]
    [InlineData(0x2FFFFF, 0x6FFFFF)]
    [InlineData(0x300000, 0xF00000)]
    [InlineData(0x3FFFFF, 0xFFFFFF)]
    public void PcToSnes_ValidHiRom2Address_ReturnsValidPcAddress(
        int snesAddress,
        int expectedPcAddress)
    {
        PcToSnes_ValidInput_ReturnsValidAddress(
            snesAddress,
            false,
            AddressMode.HiRom2,
            expectedPcAddress);
    }

    [Theory]
    [InlineData(0x000000, 0x808000)]
    [InlineData(0x008000, 0x818000)]
    [InlineData(0x3FFFFF, 0xFFFFFF)]
    [InlineData(0x400000, 0x008000)]
    [InlineData(0x700000, 0x608000)]
    [InlineData(0x7EFFFF, 0x7DFFFF)]
    public void PcToSnes_ValidExLoRomAddress_ReturnsValidPcAddress(
        int snesAddress,
        int expectedPcAddress)
    {
        PcToSnes_ValidInput_ReturnsValidAddress(
            snesAddress,
            false,
            AddressMode.ExLoRom,
            expectedPcAddress);
    }

    [Theory]
    [InlineData(0x000000, 0xC00000)]
    [InlineData(0x3FFFFF, 0xFFFFFF)]
    [InlineData(0x400000, 0x400000)]
    [InlineData(0x6FFFFF, 0x6FFFFF)]
    [InlineData(0x708000, 0x708000)]
    [InlineData(0x778000, 0x778000)]
    [InlineData(0x780000, 0x780000)]
    [InlineData(0x7DFFFF, 0x7DFFFF)]
    [InlineData(0x7E8000, 0x3E8000)]
    [InlineData(0x7FFFFF, 0x3FFFFF)]
    public void PcToSnes_ValidExHiRomAddress_ReturnsValidPcAddress(
        int snesAddress,
        int expectedPcAddress)
    {
        PcToSnes_ValidInput_ReturnsValidAddress(
            snesAddress,
            false,
            AddressMode.ExHiRom,
            expectedPcAddress);
    }

    [Theory]
    [InlineData(0x000200, 0x008000)]
    [InlineData(0x4001FF, 0xFFFFFF)]
    public void PcToSnes_ValidHeaderLoRomAddress_ReturnsValidPcAddress(
        int snesAddress,
        int expectedPcAddress)
    {
        PcToSnes_ValidInput_ReturnsValidAddress(
            snesAddress,
            true,
            AddressMode.LoRom,
            expectedPcAddress);
    }

    [Theory]
    [InlineData(0x000200, 0x808000)]
    [InlineData(0x4001FF, 0xFFFFFF)]
    public void PcToSnes_ValidHeaderLoRom2Address_ReturnsValidPcAddress(
        int snesAddress,
        int expectedPcAddress)
    {
        PcToSnes_ValidInput_ReturnsValidAddress(
            snesAddress,
            true,
            AddressMode.LoRom2,
            expectedPcAddress);
    }

    [Theory]
    [InlineData(0x000200, 0xC00000)]
    [InlineData(0x4001FF, 0xFFFFFF)]
    public void PcToSnes_ValidHeaderHiRomAddress_ReturnsValidPcAddress(
        int snesAddress,
        int expectedPcAddress)
    {
        PcToSnes_ValidInput_ReturnsValidAddress(
            snesAddress,
            true,
            AddressMode.HiRom,
            expectedPcAddress);
    }

    [Theory]
    [InlineData(0x000200, 0x400000)]
    [InlineData(0x4001FF, 0xFFFFFF)]
    public void PcToSnes_ValidHeaderHiRom2Address_ReturnsValidPcAddress(
        int snesAddress,
        int expectedPcAddress)
    {
        PcToSnes_ValidInput_ReturnsValidAddress(
            snesAddress,
            true,
            AddressMode.HiRom2,
            expectedPcAddress);
    }

    [Theory]
    [InlineData(0x000200, 0x808000)]
    public void PcToSnes_ValidHeaderExLoRomAddress_ReturnsValidPcAddress(
        int snesAddress,
        int expectedPcAddress)
    {
        PcToSnes_ValidInput_ReturnsValidAddress(
            snesAddress,
            true,
            AddressMode.ExLoRom,
            expectedPcAddress);
    }

    [Theory]
    [InlineData(0x8001FF, 0x3FFFFF)]
    public void PcToSnes_ValidHeaderExHiRomAddress_ReturnsValidPcAddress(
        int snesAddress,
        int expectedPcAddress)
    {
        PcToSnes_ValidInput_ReturnsValidAddress(
            snesAddress,
            true,
            AddressMode.ExHiRom,
            expectedPcAddress);
    }

    [Fact]
    public void IsValidAddress_InvalidAddressMode_ThrowsInvalidEnumException()
    {
        static void action()
        {
            _ = Rom.IsValidAddress(0, (AddressMode)(-1));
        }

        _ = Assert.Throws<InvalidEnumArgumentException>(action);
    }

    [Theory]
    [InlineData(Int32.MinValue)]
    [InlineData(-1)]
    [InlineData(0x000000)]
    [InlineData(0x007FFF)]
    [InlineData(0x010000)]
    [InlineData(0x3F7FFF)]
    [InlineData(0x700000)]
    [InlineData(0x708000)]
    [InlineData(0x770000)]
    [InlineData(0x778000)]
    [InlineData(0x780000)]
    [InlineData(0x788000)]
    [InlineData(0x7E0000)]
    [InlineData(0x7FFFFF)]
    [InlineData(0x800000)]
    [InlineData(0xBF7FFF)]
    [InlineData(0x1000000)]
    [InlineData(0x1008000)]
    [InlineData(Int32.MaxValue)]
    public void IsValidAddress_InvalidLoRomAddress_ReturnsFalse(int snesAddress)
    {
        IsValidAddress_InvalidAddress_ReturnsFalse(
            snesAddress: snesAddress,
            addressMode: AddressMode.LoRom);
    }

    [Theory]
    [InlineData(0x008000)]
    [InlineData(0x3FFFFF)]
    [InlineData(0x400000)]
    [InlineData(0x6FFFFF)]
    [InlineData(0x808000)]
    [InlineData(0xC00000)]
    [InlineData(0xFF0000)]
    [InlineData(0xFFFFFF)]
    public void IsValidAddress_ValidLoRomAddress_ReturnsTrue(int snesAddress)
    {
        /*
         * There are two ways to consider a LOROM address. If I'm trying to read from a LOROM address, then
         * it doesn't matter if I get $00:8000 or $80:8000 (also note that even $40:8000 or several other values
         * may be valid based on mirroring). They point to the same PC location. However, if I'm trying to write
         * to a PC location, then writing in LOROM or LOROM2 actually matters. Not so much because they point to
         * different location (because they don't), but because it just affects the written data.
         */
        IsValidAddress_ValidAddress_ReturnsTrue(
            snesAddress: snesAddress,
            addressMode: AddressMode.LoRom);
    }

    [Theory]
    [InlineData(Int32.MinValue)]
    [InlineData(-1)]
    [InlineData(0x000000)]
    [InlineData(0x007FFF)]
    [InlineData(0x010000)]
    [InlineData(0x3F7FFF)]
    [InlineData(0x700000)]
    [InlineData(0x708000)]
    [InlineData(0x770000)]
    [InlineData(0x778000)]
    [InlineData(0x780000)]
    [InlineData(0x788000)]
    [InlineData(0x7E0000)]
    [InlineData(0x7FFFFF)]
    [InlineData(0x800000)]
    [InlineData(0xBF7FFF)]
    [InlineData(0x1000000)]
    [InlineData(0x1008000)]
    [InlineData(Int32.MaxValue)]
    public void IsValidAddress_InvalidLoRom2Address_ReturnsFalse(int snesAddress)
    {
        IsValidAddress_InvalidAddress_ReturnsFalse(
            snesAddress: snesAddress,
            addressMode: AddressMode.LoRom2);
    }

    [Theory]
    [InlineData(0x008000)]
    [InlineData(0x3FFFFF)]
    [InlineData(0x400000)]
    [InlineData(0x6FFFFF)]
    [InlineData(0x808000)]
    [InlineData(0xC00000)]
    [InlineData(0xFF0000)]
    [InlineData(0xFFFFFF)]
    public void IsValidAddress_ValidLoRom2Address_ReturnsTrue(int snesAddress)
    {
        IsValidAddress_ValidAddress_ReturnsTrue(
            snesAddress: snesAddress,
            addressMode: AddressMode.LoRom2);
    }

    [Theory]
    [InlineData(Int32.MinValue)]
    [InlineData(-1)]
    [InlineData(0x000000)]
    [InlineData(0x007FFF)]
    [InlineData(0x010000)]
    [InlineData(0x3F7FFF)]
    [InlineData(0x700000)]
    [InlineData(0x770000)]
    [InlineData(0x780000)]
    [InlineData(0x7E0000)]
    [InlineData(0x7FFFFF)]
    [InlineData(0x800000)]
    [InlineData(0xBF7FFF)]
    [InlineData(0x1000000)]
    [InlineData(0x1008000)]
    [InlineData(Int32.MaxValue)]
    public void IsValidAddress_InvalidExLoRomAddress_ReturnsFalse(int snesAddress)
    {
        IsValidAddress_InvalidAddress_ReturnsFalse(
            snesAddress: snesAddress,
            addressMode: AddressMode.ExLoRom);
    }

    [Theory]
    [InlineData(0x008000)]
    [InlineData(0x3FFFFF)]
    [InlineData(0x400000)]
    [InlineData(0x6FFFFF)]
    [InlineData(0x708000)]
    [InlineData(0x778000)]
    [InlineData(0x788000)]
    [InlineData(0x808000)]
    [InlineData(0xC00000)]
    [InlineData(0xFF0000)]
    [InlineData(0xFFFFFF)]
    public void IsValidAddress_ValidExLoRomAddress_ReturnsTrue(int snesAddress)
    {
        IsValidAddress_ValidAddress_ReturnsTrue(
            snesAddress: snesAddress,
            addressMode: AddressMode.ExLoRom);
    }

    [Theory]
    [InlineData(Int32.MinValue)]
    [InlineData(-1)]
    [InlineData(0x000000)]
    [InlineData(0x007FFF)]
    [InlineData(0x010000)]
    [InlineData(0x3F7FFF)]
    [InlineData(0x700000)]
    [InlineData(0x708000)]
    [InlineData(0x770000)]
    [InlineData(0x778000)]
    [InlineData(0x780000)]
    [InlineData(0x788000)]
    [InlineData(0x7E0000)]
    [InlineData(0x7FFFFF)]
    [InlineData(0x800000)]
    [InlineData(0xBF7FFF)]
    [InlineData(0x1000000)]
    [InlineData(0x1008000)]
    [InlineData(Int32.MaxValue)]
    public void IsValidAddress_InvalidHiRomAddress_ReturnsFalse(int snesAddress)
    {
        IsValidAddress_InvalidAddress_ReturnsFalse(
            snesAddress: snesAddress,
            addressMode: AddressMode.HiRom);
    }

    [Theory]
    [InlineData(0x008000)]
    [InlineData(0x3FFFFF)]
    [InlineData(0x400000)]
    [InlineData(0x6FFFFF)]
    [InlineData(0x808000)]
    [InlineData(0xC00000)]
    [InlineData(0xFF0000)]
    [InlineData(0xFFFFFF)]
    public void IsValidAddress_ValidHiRomAddress_ReturnsTrue(int snesAddress)
    {
        IsValidAddress_ValidAddress_ReturnsTrue(
            snesAddress: snesAddress,
            addressMode: AddressMode.HiRom);
    }

    [Theory]
    [InlineData(Int32.MinValue)]
    [InlineData(-1)]
    [InlineData(0x000000)]
    [InlineData(0x007FFF)]
    [InlineData(0x010000)]
    [InlineData(0x3F7FFF)]
    [InlineData(0x700000)]
    [InlineData(0x708000)]
    [InlineData(0x770000)]
    [InlineData(0x778000)]
    [InlineData(0x780000)]
    [InlineData(0x788000)]
    [InlineData(0x7E0000)]
    [InlineData(0x7FFFFF)]
    [InlineData(0x800000)]
    [InlineData(0xBF7FFF)]
    [InlineData(0x1000000)]
    [InlineData(0x1008000)]
    [InlineData(Int32.MaxValue)]
    public void IsValidAddress_InvalidHiRom2Address_ReturnsFalse(int snesAddress)
    {
        IsValidAddress_InvalidAddress_ReturnsFalse(
            snesAddress: snesAddress,
            addressMode: AddressMode.HiRom2);
    }

    [Theory]
    [InlineData(0x008000)]
    [InlineData(0x3FFFFF)]
    [InlineData(0x400000)]
    [InlineData(0x6FFFFF)]
    [InlineData(0x808000)]
    [InlineData(0xC00000)]
    [InlineData(0xFF0000)]
    [InlineData(0xFFFFFF)]
    public void IsValidAddress_ValidHiRom2Address_ReturnsTrue(int snesAddress)
    {
        IsValidAddress_ValidAddress_ReturnsTrue(
            snesAddress: snesAddress,
            addressMode: AddressMode.HiRom2);
    }

    [Theory]
    [InlineData(Int32.MinValue)]
    [InlineData(-1)]
    [InlineData(0x00_0000)]
    [InlineData(0x00_7FFF)]
    [InlineData(0x01_0000)]
    [InlineData(0x3F_7FFF)]
    [InlineData(0x70_0000)]
    [InlineData(0x77_0000)]
    [InlineData(0x7E_0000)]
    [InlineData(0x7F_FFFF)]
    [InlineData(0x80_0000)]
    [InlineData(0xBF_7FFF)]
    [InlineData(0x100_0000)]
    [InlineData(0x100_8000)]
    [InlineData(Int32.MaxValue)]
    public void IsValidAddress_InvalidExHiRomAddress_ReturnsFalse(int snesAddress)
    {
        IsValidAddress_InvalidAddress_ReturnsFalse(
            snesAddress: snesAddress,
            addressMode: AddressMode.ExHiRom);
    }

    [Theory]
    [InlineData(0x008000)]
    [InlineData(0x3FFFFF)]
    [InlineData(0x400000)]
    [InlineData(0x6FFFFF)]
    [InlineData(0x708000)]
    [InlineData(0x778000)]
    [InlineData(0x780000)]
    [InlineData(0x788000)]
    [InlineData(0x808000)]
    [InlineData(0xC00000)]
    [InlineData(0xFF0000)]
    [InlineData(0xFFFFFF)]
    public void IsValidAddress_ValidExHiRomAddress_ReturnsTrue(int snesAddress)
    {
        IsValidAddress_ValidAddress_ReturnsTrue(
            snesAddress: snesAddress,
            addressMode: AddressMode.ExHiRom);
    }

    [Theory]
    [InlineData(0x008000, 0x0000, 0x008000)]
    [InlineData(0x008000, 0x0001, 0x008001)]
    [InlineData(0x00FFFF, 0x0001, 0x008000)]
    [InlineData(0x00FFFF, 0xFFFF, 0x00FFFE)]
    [InlineData(0xFFFFFF, 0x0001, 0xFF8000)]
    [InlineData(0x008000, 0x10000, 0x008000)]
    [InlineData(0x008000, -1, 0x00FFFF)]
    [InlineData(0x808000, Int32.MaxValue, 0x80FFFF)]
    [InlineData(0x7F8000, Int32.MinValue, 0x7F8000)]
    public void IncrementSnesAddress_ValidInput_ReturnsValidOutput(
        int snesAddress,
        int increment,
        int expected)
    {
        var actual = Rom.IncrementSnesAddress(
            snesAddress,
            increment,
            crossBanks: false);
        var message = new StringBuilder("SNES address: 0x")
                .Append(snesAddress.ToString("X6")).AppendLine()
            .Append("Increment: 0x").Append(increment.ToString("X4")).AppendLine()
            .Append("Expected: 0x").Append(expected.ToString("X6")).AppendLine()
            .Append("Actual: 0x").Append(actual.ToString("X6")).AppendLine()
            .ToString();

        Assert.True(expected == actual, message);
    }

    [Theory]
    [InlineData("Aladdin (E)")]
    [InlineData("Aladdin (F)")]
    [InlineData("Aladdin (G) [!]")]
    [InlineData("Aladdin (J)")]
    [InlineData("Aladdin (U) [!]")]
    [InlineData("Arkanoid - Doh It Again (U) [!]")]
    [InlineData("Axelay (U)")]
    [InlineData("Battletoads & Double Dragon - The Ultimate Team (U) [!]")]
    [InlineData("BS Mario Collection 3 (J)")]
    [InlineData("Chrono Trigger (U) [!]")]
    [InlineData("Contra III - The Alien Wars (U) [!]")]
    [InlineData("Contra Spirits (J)")]
    [InlineData("Super Probotector - The Alien Rebels (E)")]
    [InlineData("Daikaijuu Monogatari II (Japan)")]
    [InlineData("Demon's Blazon - Makai-Mura Monshou Hen (J)")]
    [InlineData("Demon's Crest (E)")]
    [InlineData("Demon's Crest (U) [!]")]
    [InlineData("Donkey Kong Country (E) (V1.0) [!]")]
    [InlineData("Donkey Kong Country (E) (V1.1) [!]")]
    [InlineData("Donkey Kong Country (U) (V1.0) [!]")]
    [InlineData("Donkey Kong Country (U) (V1.1)")]
    [InlineData("Donkey Kong Country (U) (V1.2) [!]")]
    [InlineData("Donkey Kong Country - Competition Cartridge (U)")]
    [InlineData("Super Donkey Kong (J) (V1.0)")]
    [InlineData("Super Donkey Kong (J) (V1.1)")]
    [InlineData("Donkey Kong Country 2 - Diddy's Kong Quest (E) (V1.1) [!]")]
    [InlineData("Donkey Kong Country 2 - Diddy's Kong Quest (G) (V1.0)")]
    [InlineData("Donkey Kong Country 2 - Diddy's Kong Quest (G) (V1.1) [!]")]
    [InlineData("Donkey Kong Country 2 - Diddy's Kong Quest (U) (V1.0)")]
    [InlineData("Donkey Kong Country 2 - Diddy's Kong Quest (U) (V1.1) [!]")]
    [InlineData("Super Donkey Kong 2 - Dixie & Diddy (J) (V1.0)")]
    [InlineData("Super Donkey Kong 2 - Dixie & Diddy (J) (V1.1)")]
    [InlineData("Donkey Kong Country 3 - Dixie Kong's Double Trouble (E) [!]")]
    [InlineData("Donkey Kong Country 3 - Dixie Kong's Double Trouble (U) [!]")]
    [InlineData("Super Donkey Kong 3 - Nazo no Krems Shima (J) (V1.0)")]
    [InlineData("Super Donkey Kong 3 - Nazo no Krems Shima (J) (V1.1)")]
    [InlineData("Earthbound (U)")]
    [InlineData("Mother 2 (J)")]
    [InlineData("F-ZERO (E)")]
    [InlineData("F-ZERO (J)")]
    [InlineData("F-ZERO (U) [!]")]
    [InlineData("Final Fantasy - Mystic Quest (U) (V1.0) [!]")]
    [InlineData("Final Fantasy - Mystic Quest (U) (V1.1)")]
    [InlineData("Final Fantasy USA - Mystic Quest (J)")]
    [InlineData("Mystic Quest Legend (E)")]
    [InlineData("Mystic Quest Legend (F)")]
    [InlineData("Mystic Quest Legend (G)")]
    [InlineData("FF4FE.bAAMI_KkGAMAF4kA6JQ.6DYM4S5PAX")]
    [InlineData("Final Fantasy II (U) (V1.0) [!]")]
    [InlineData("Final Fantasy II (U) (V1.1)")]
    [InlineData("Final Fantasy IV (J)")]
    [InlineData("Final Fantasy IV - 10th Anniversary Edition v3.21")]
    [InlineData("Final Fantasy IV - Ultima v5.12f no header")]
    [InlineData("Final Fantasy V (J)")]
    [InlineData("Final Fantasy III (U) (V1.0) [!]")]
    [InlineData("Final Fantasy III (U) (V1.1) [!]")]
    [InlineData("Final Fantasy VI (J)")]
    [InlineData("Firepower 2000 (U)")]
    [InlineData("Super SWIV (E)")]
    [InlineData("Super SWIV (J) (32469)")]
    [InlineData("Super SWIV (J) (62746) [b1]")]
    [InlineData("Super SWIV (J) (62746) [f1]")]
    [InlineData("Super SWIV (J) (62746) [hI]")]
    [InlineData("Super SWIV (J) (62746)")]
    [InlineData("Gradius III (U) [!]")]
    [InlineData("Hagane (Beta)")]
    [InlineData("Hagane (E) [o1]")]
    [InlineData("Hagane (E)")]
    [InlineData("Hagane (J) [b1]")]
    [InlineData("Hagane (J) [t1]")]
    [InlineData("Hagane (J)")]
    [InlineData("Hagane (U) [t1]")]
    [InlineData("Hagane (U)")]
    [InlineData("Killer Instinct (E) [!]")]
    [InlineData("Killer Instinct (U) (V1.0)")]
    [InlineData("Killer Instinct (U) (V1.1) [!]")]
    [InlineData("King of Dragons, The (J) [f1]")]
    [InlineData("King of Dragons, The (J) [t1]")]
    [InlineData("King of Dragons, The (J)")]
    [InlineData("King of Dragons, The (U) [h1]")]
    [InlineData("King of Dragons, The (U) [T+Ita]")]
    [InlineData("King of Dragons, The (U)")]
    [InlineData("Kirby Super Star (U) [!]")]
    [InlineData("Kirby's Avalanche (U) [!]")]
    [InlineData("Kirby's Ghost Trap (E)")]
    [InlineData("Hoshi no Kirby 3 (J)")]
    [InlineData("Kirby's Dream Land 3 (U)")]
    [InlineData("Kaizoalttp")]
    [InlineData("Legend of Zelda, The - A Link to the Past (E) [!]")]
    [InlineData("Legend of Zelda, The - A Link to the Past (F)")]
    [InlineData("Legend of Zelda, The - A Link to the Past (FC)")]
    [InlineData("Legend of Zelda, The - A Link to the Past (G) [!]")]
    [InlineData("Legend of Zelda, The - A Link to the Past (U) [!]-rand-pal")]
    [InlineData("Legend of Zelda, The - A Link to the Past (U) [!]")]
    [InlineData("Zelda no Densetsu - Kamigami no Triforce (J) (V1.0) [b1]")]
    [InlineData("Zelda no Densetsu - Kamigami no Triforce (J) (V1.0)")]
    [InlineData("Zelda no Densetsu - Kamigami no Triforce (J) (V1.1)")]
    [InlineData("Zelda no Densetsu - Kamigami no Triforce (J) (V1.2)")]
    [InlineData("Zelda Parallel Worlds")]
    [InlineData("Magical Quest Starring Mickey Mouse, The (U) [!]")]
    [InlineData("Marko's Magic Football (E) [f1]")]
    [InlineData("Marko's Magic Football (E)")]
    [InlineData("Mega Man X (E)")]
    [InlineData("Mega Man X (U) (V1.0) [!]")]
    [InlineData("Mega Man X (U) (V1.1)")]
    [InlineData("Rockman X (J) (V1.0) [!]")]
    [InlineData("Rockman X (J) (V1.1)")]
    [InlineData("Ms. Pac-Man (U)")]
    [InlineData("Pilotwings (E) [!]")]
    [InlineData("Pilotwings (J)")]
    [InlineData("Pilotwings (U) [!]")]
    [InlineData("Pilotwings (U) [f1]")]
    [InlineData("Power Lode Runner (J) (NP)")]
    [InlineData("Run Saber (Beta) [h1]")]
    [InlineData("Run Saber (Beta)")]
    [InlineData("Run Saber (E) [!]")]
    [InlineData("Run Saber (U) [!]")]
    [InlineData("Run Saber (U) [t1]")]
    [InlineData("Run Saber (U) [t2]")]
    [InlineData("Run Saber (U) [t3]")]
    [InlineData("Secret of Mana (E) (V1.0)")]
    [InlineData("Secret of Mana (E) (V1.1) [!]")]
    [InlineData("Secret of Mana (F)")]
    [InlineData("Secret of Mana (G)")]
    [InlineData("Secret of Mana (U) [!]")]
    [InlineData("Seiken Densetsu 2 (J)")]
    [InlineData("Shin Megami Tensei (J) (V1.0)")]
    [InlineData("Shin Megami Tensei (J) (V1.1)")]
    [InlineData("Space Megaforce (U) [!]")]
    [InlineData("Space Megaforce (U) [T+Spa100%_Sayans]")]
    [InlineData("Super Aleste (E) [h1C]")]
    [InlineData("Super Aleste (E)")]
    [InlineData("Super Aleste (J) [t1]")]
    [InlineData("Super Aleste (J) [t2]")]
    [InlineData("Super Aleste (J)")]
    [InlineData("Star Fox (J) [!]")]
    [InlineData("Star Fox (U) (V1.0) [!]")]
    [InlineData("Star Fox (U) (V1.2) [!]")]
    [InlineData("StarWing (E) (V1.0) [!]")]
    [InlineData("StarWing (E) (V1.1) [!]")]
    [InlineData("StarWing (G) [!]")]
    [InlineData("StarWing Offizieller Wettbewerb (G) [!]")]
    [InlineData("StarWing Super Weekend Competition (E) [!]")]
    [InlineData("Stunt Race FX (E) [!]")]
    [InlineData("Stunt Race FX (U) [!]")]
    [InlineData("Wild Trax (J)")]
    [InlineData("Akumajou Dracula (J) [!]")]
    [InlineData("Super Castlevania IV (E) [!]")]
    [InlineData("Super Castlevania IV (U) [!]")]
    [InlineData("Super Mario 3 Expert (Hack)")]
    [InlineData("Super Mario All-Stars (E) [!]")]
    [InlineData("Super Mario All-Stars (U) [!] - 8MB")]
    [InlineData("Super Mario All-Stars (U) [!]")]
    [InlineData("Super Mario All-Stars + Super Mario World (E) [!]")]
    [InlineData("Super Mario All-Stars + Super Mario World (U) [!]")]
    [InlineData("Super Mario Bros. (U)")]
    [InlineData("Super Mario Collection (J) (V1.0)")]
    [InlineData("Super Mario Collection (J) (V1.1)")]
    [InlineData("Test ROM")]
    [InlineData("Mario Kart R")]
    [InlineData("Super Mario Kart (E) [!]")]
    [InlineData("Super Mario Kart (J)")]
    [InlineData("Super Mario Kart (U) [!]")]
    [InlineData("SMRPG_GBARP_US_7.1.8_full_4121810938")]
    [InlineData("Super Mario RPG - Legend of the Seven Stars (U) [!]")]
    [InlineData("Super Mario RPG Armageddon v10 (Hard)")]
    [InlineData("Super Mario RPG Armageddon v10 (Normal)")]
    [InlineData("Bowser's Strike Back")]
    [InlineData("Brutal Mario")]
    [InlineData("Demo World - The Legend Continues")]
    [InlineData("Kaizo Kindergarten v1.09")]
    [InlineData("Luigi's Adventure")]
    [InlineData("Mario Is Missing 2")]
    [InlineData("Mario's Return")]
    [InlineData("MarioX World Chronicles")]
    [InlineData("Panic In the Mushroom Kingdom")]
    [InlineData("Return to Dinosaur Land")]
    [InlineData("SMW - HELL Edition")]
    [InlineData("Super Mario Islands")]
    [InlineData("Super Mario World (U) [!] - 8MB Hack")]
    [InlineData("Super Mario World (U) [!]")]
    [InlineData("Super Mario World R 2")]
    [InlineData("Super Mario World R EX")]
    [InlineData("Super Mario World R")]
    [InlineData("The Adventures of Phantasm World")]
    [InlineData("The Second Reality Project (Hard Type)")]
    [InlineData("The Second Reality Project (SNES Type)")]
    [InlineData("VLDC11_BaseROM")]
    [InlineData("Super Mario - Yoshi Island (J) (V1.0)")]
    [InlineData("Super Mario - Yoshi Island (J) (V1.1) [!]")]
    [InlineData("Super Mario - Yoshi Island (J) (V1.2)")]
    [InlineData("Super Mario World 2 - Yoshi's Island (E) (V1.0) [!]")]
    [InlineData("Super Mario World 2 - Yoshi's Island (E) (V1.1)")]
    [InlineData("Super Mario World 2 - Yoshi's Island (U) (V1.0) [!]")]
    [InlineData("Super Mario World 2 - Yoshi's Island (U) (V1.1)")]
    [InlineData("Super MarioWorld 2+")]
    [InlineData("Super Metroid (E) [!]")]
    [InlineData("Super Metroid (JU) [!]")]
    [InlineData("Tales of Phantasia (J) [!]")]
    [InlineData("Tetris & Dr. Mario (E) [!]")]
    [InlineData("Tetris & Dr. Mario (U) [!]")]
    [InlineData("BS Panel de Pon Event '98 (J)")]
    [InlineData("BS Yoshi no Panepon (J)")]
    [InlineData("Panel de Pon (J)")]
    [InlineData("Tetris Attack (E)")]
    [InlineData("Tetris Attack (U) [!]")]
    [InlineData("Tetris Attack (U) [f1]")]
    [InlineData("Tetris Attack (U) [T+Spa050_A2j]")]
    [InlineData("Uniracers (U) [!]")]
    [InlineData("Unirally (E) [!]")]

    public void Constructor_ValidData_CanDetermineFormat(string name)
    {
        var testRom = TestRoms[name];
        var data = testRom.CreateRomData();
        Rom rom;
        try
        {
            rom = new Rom(data);
        }
        catch (Exception ex)
        {
            var exceptionMessage = new StringBuilder(name)
                    .AppendLine(" could not get address mode.")
                .Append(ex.Message).AppendLine()
                .ToString();
            Assert.True(false, exceptionMessage);
            return;
        }

        var message = new StringBuilder("File Name: ").Append(name)
                .AppendLine()
            .Append("Expected Format: ").Append(testRom.AddressMode).AppendLine()
            .Append("Actual Format: ").Append(rom.AddressMode).AppendLine()
            .ToString();
        var expectedAddressMode = Enum.Parse<AddressMode>(testRom.AddressMode!);
        var actualAddressMode = rom.AddressMode;
        if (actualAddressMode == AddressMode.LoRom2)
        {
            actualAddressMode = AddressMode.LoRom;
        }

        if (actualAddressMode == AddressMode.HiRom2)
        {
            actualAddressMode = AddressMode.HiRom;
        }

        Assert.True(
            condition: expectedAddressMode == actualAddressMode,
            userMessage: message);
    }

    private static void SnesToPc_ValidInput_ReturnsValidAddress(
        int snesAddress,
        bool hasHeader,
        AddressMode addressMode,
        int expectedPcAddress)
    {
        var actualAddress = Rom.SnesToPc(snesAddress, hasHeader, addressMode);
        var message = new StringBuilder()
            .Append("Address Mode: ").Append(addressMode).AppendLine()
            .Append("Has Header: ").Append(hasHeader).AppendLine()
            .Append("SNES Address: 0x").Append(snesAddress.ToString("X6")).AppendLine()
            .Append("Expected PC Address: 0x")
                .Append(expectedPcAddress.ToString("X6")).AppendLine()
            .Append("Actual PC Address: 0x").Append(actualAddress.ToString("X6"))
                .AppendLine()
            .ToString();

        Assert.True(
            condition: expectedPcAddress == actualAddress,
            userMessage: message);
    }

    private static void PcToSnes_ValidInput_ReturnsValidAddress(
        int pointer,
        bool hasHeader,
        AddressMode addressMode,
        int expectedAddress)
    {
        var actualAddress = Rom.PcToSnes(pointer, hasHeader, addressMode);
        var message = new StringBuilder()
            .Append("Address Mode: ").Append(addressMode).AppendLine()
            .Append("Has Header: ").Append(hasHeader).AppendLine()
            .Append("PC Address: 0x").Append(pointer.ToString("X6")).AppendLine()
            .Append("Expected SNES Address: 0x")
                .Append(expectedAddress.ToString("X6")).AppendLine()
            .Append("Actual SNES Address: 0x").Append(actualAddress.ToString("X6"))
                .AppendLine()
            .ToString();

        Assert.True(
            condition: expectedAddress == actualAddress,
            userMessage: message);
    }

    private static void IsValidAddress_ValidAddress_ReturnsTrue(
        int snesAddress,
        AddressMode addressMode)
    {
        var result = Rom.IsValidAddress(snesAddress, addressMode);
        var message = new StringBuilder("Address Mode: ").Append(addressMode)
                .AppendLine()
            .Append("SNES Address: 0x").Append(snesAddress.ToString("X6"))
                .AppendLine()
            .ToString();

        Assert.True(
            condition: result,
            userMessage: message);
    }

    private static void IsValidAddress_InvalidAddress_ReturnsFalse(
        int snesAddress,
        AddressMode addressMode)
    {
        var result = Rom.IsValidAddress(snesAddress, addressMode);
        var message = new StringBuilder("Address Mode: ").Append(addressMode)
                .AppendLine()
            .Append("SNES Address: 0x").Append(snesAddress.ToString("X6"))
                .AppendLine()
            .ToString();

        Assert.False(
            condition: result,
            userMessage: message);
    }
}
