// <copyright file="MapMode.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes;

/// <summary>
/// Specifies the SNES ROM mapping and access-speed mode.
/// </summary>
/// <remarks>
/// This value is encoded in the ROM header at $00FFD5. See the
/// <see href="https://snes.nesdev.org/wiki/ROM_header">NESdev ROM header
/// documentation</see> for the speed bit and the articles on
/// <see href="https://snes.nesdev.org/wiki/LoROM">LoROM</see>,
/// <see href="https://snes.nesdev.org/wiki/HiROM">HiROM</see>, and
/// <see href="https://snes.nesdev.org/wiki/ExHiROM">ExHiROM</see> for mapping
/// details.
/// </remarks>
public enum MapMode
{
    /// <summary>
    /// Slow-access LoROM mapping mode.
    /// </summary>
    Mode20 = 0x20,

    /// <summary>
    /// Slow-access HiROM mapping mode.
    /// </summary>
    Mode21 = 0x21,

    /// <summary>
    /// Reserved mapping mode.
    /// </summary>
    Reserved = 0x22,

    /// <summary>
    /// SA-1 cartridge mapping mode.
    /// </summary>
    Mode23Sa1 = 0x23,

    /// <summary>
    /// ExHiROM mapping mode.
    /// </summary>
    Mode25 = 0x25,

    /// <summary>
    /// FastROM LoROM mapping mode.
    /// </summary>
    Mode20Fast = 0x30,

    /// <summary>
    /// FastROM HiROM mapping mode.
    /// </summary>
    Mode21Fast = 0x31,

    /// <summary>
    /// FastROM ExHiROM mapping mode.
    /// </summary>
    Mode25Fast = 0x35,
}
