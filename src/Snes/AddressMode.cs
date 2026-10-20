// <copyright file="AddressMode.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes;

/// <summary>
/// Specifies the SNES ROM memory map used to translate CPU addresses into
/// ROM offsets.
/// </summary>
/// <remarks>
/// See the NESdev articles on <see href="https://snes.nesdev.org/wiki/LoROM">
/// LoROM</see>, <see href="https://snes.nesdev.org/wiki/HiROM">HiROM</see>,
/// <see href="https://snes.nesdev.org/wiki/ExHiROM">ExHiROM</see>, and the
/// <see href="https://snes.nesdev.org/wiki/ROM_header">ROM header</see>.
/// </remarks>
public enum AddressMode
{
    /// <summary>
    /// Standard LoROM mapping, which exposes ROM in 32 KiB banks.
    /// </summary>
    LoRom,

    /// <summary>
    /// Standard HiROM mapping, which exposes ROM in 64 KiB banks.
    /// </summary>
    HiRom,

    /// <summary>
    /// Extended HiROM mapping for ROMs larger than the standard HiROM range.
    /// </summary>
    ExHiRom,

    /// <summary>
    /// Extended LoROM mapping for ROMs larger than the standard LoROM range.
    /// </summary>
    ExLoRom,

    /// <summary>
    /// LoROM mapping with FastROM access enabled.
    /// </summary>
    LoRom2,

    /// <summary>
    /// HiROM mapping with FastROM access enabled.
    /// </summary>
    HiRom2,
}
