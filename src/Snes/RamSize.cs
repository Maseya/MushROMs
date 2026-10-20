// <copyright file="RamSize.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes;

/// <summary>
/// Represents supported RAM capacities encoded in a SNES ROM header.
/// </summary>
/// <remarks>
/// The header stores the base-2 exponent of the RAM size in kilobytes at
/// $00FFD8. See the
/// <see href="https://snes.nesdev.org/wiki/ROM_header">NESdev SNES ROM header
/// documentation</see> for the encoding.
/// </remarks>
public enum RamSize
{
    /// <summary>
    /// No RAM.
    /// </summary>
    NoRam = 0,

    /// <summary>
    /// 16 Kbit (2 KiB).
    /// </summary>
    Size16Kbit = 0x01,

    /// <summary>
    /// 64 Kbit (8 KiB).
    /// </summary>
    Size64Kbit = 0x03,

    /// <summary>
    /// 256 Kbit (32 KiB).
    /// </summary>
    Size256Kbit = 0x05,

    /// <summary>
    /// 512 Kbit (64 KiB).
    /// </summary>
    Size512Kbit = 0x06,

    /// <summary>
    /// 1 Mbit (128 KiB).
    /// </summary>
    Size1Mbit = 0x07,
}
