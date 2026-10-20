// <copyright file="CartridgeType.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes;

/// <summary>
/// Specifies the hardware configuration recorded in a SNES cartridge header.
/// </summary>
/// <remarks>
/// Values correspond to the chipset byte at $00FFD6. See the
/// <see href="https://snes.nesdev.org/wiki/ROM_header">NESdev SNES ROM header
/// documentation</see> for the field encoding and coprocessor codes.
/// </remarks>
public enum CartridgeType
{
    /// <summary>
    /// ROM only.
    /// </summary>
    RomOnly = 0x00,

    /// <summary>
    /// ROM with RAM.
    /// </summary>
    RomRam = 0x01,

    /// <summary>
    /// ROM with battery-backed RAM.
    /// </summary>
    RamBattery = 0x02,

    /// <summary>
    /// ROM with a DSP coprocessor.
    /// </summary>
    RomDsp = 0x03,

    /// <summary>
    /// ROM with a DSP coprocessor and RAM.
    /// </summary>
    RomDspRam = 0x04,

    /// <summary>
    /// ROM with a DSP coprocessor and battery-backed RAM.
    /// </summary>
    RomDspRamBattery = 0x05,

    /// <summary>
    /// ROM with a DSP coprocessor and battery-backed memory.
    /// </summary>
    RomDspBattery = 0x06,

    /// <summary>
    /// ROM with a Super FX (GSU) coprocessor.
    /// </summary>
    RomSuperFx = 0x13,

    /// <summary>
    /// ROM with a Super FX (GSU) coprocessor and RAM.
    /// </summary>
    RomSuperFxRam = 0x14,

    /// <summary>
    /// ROM with a Super FX (GSU) coprocessor and battery-backed RAM.
    /// </summary>
    RomSuperFxRamBattery = 0x15,

    /// <summary>
    /// ROM with a Super FX (GSU) coprocessor and battery-backed memory.
    /// </summary>
    RomSuperFxBattery = 0x16,

    /// <summary>
    /// ROM with an OBC1 coprocessor.
    /// </summary>
    RoObc1 = 0x23,

    /// <summary>
    /// ROM with an OBC1 coprocessor and RAM.
    /// </summary>
    RoObc1Ram = 0x24,

    /// <summary>
    /// ROM with an OBC1 coprocessor and battery-backed RAM.
    /// </summary>
    RoObc1RamBattery = 0x25,

    /// <summary>
    /// ROM with an OBC1 coprocessor and battery-backed memory.
    /// </summary>
    RoObc1Battery = 0x26,

    /// <summary>
    /// ROM with an SA-1 coprocessor.
    /// </summary>
    RomSa1 = 0x33,

    /// <summary>
    /// ROM with an SA-1 coprocessor and RAM.
    /// </summary>
    RomSa1Ram = 0x34,

    /// <summary>
    /// ROM with an SA-1 coprocessor and battery-backed RAM.
    /// </summary>
    RomSa1RamBattery = 0x35,

    /// <summary>
    /// ROM with an SA-1 coprocessor and battery-backed memory.
    /// </summary>
    RomSa1Battery = 0x36,

    /// <summary>
    /// ROM with another coprocessor, such as Super Game Boy or Satellaview.
    /// </summary>
    RomOther = 0xE3,

    /// <summary>
    /// ROM with another coprocessor and RAM.
    /// </summary>
    RomOtherRam = 0xE4,

    /// <summary>
    /// ROM with another coprocessor and battery-backed RAM.
    /// </summary>
    RomOtherRamBattery = 0xE5,

    /// <summary>
    /// ROM with another coprocessor and battery-backed memory.
    /// </summary>
    RomOtherBattery = 0xE6,

    /// <summary>
    /// ROM with a custom coprocessor identified by the extended header.
    /// </summary>
    RomCustomChip = 0xF3,

    /// <summary>
    /// ROM with a custom coprocessor and RAM.
    /// </summary>
    RomCustomChipRam = 0xF4,

    /// <summary>
    /// ROM with a custom coprocessor and battery-backed RAM.
    /// </summary>
    RomCustomChipRamBattery = 0xF5,

    /// <summary>
    /// ROM with a custom coprocessor and battery-backed memory.
    /// </summary>
    RomCustomChipBattery = 0xF6,
}
