// <copyright file="RomSize.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes;

/// <summary>
/// Represents supported ROM capacities encoded in a SNES ROM header.
/// </summary>
/// <remarks>
/// The header stores the base-2 exponent of the ROM size in kilobytes at
/// $00FFD7. See the
/// <see href="https://snes.nesdev.org/wiki/ROM_header">NESdev SNES ROM header
/// documentation</see> for the encoding.
/// </remarks>
public enum RomSize
{
    /// <summary>
    /// 4 Mbit (512 KiB).
    /// </summary>
    Size4Mbit = 0x09,

    /// <summary>
    /// 8 Mbit (1 MiB).
    /// </summary>
    Size8Mbit = 0x0A,

    /// <summary>
    /// 16 Mbit (2 MiB).
    /// </summary>
    Size16Mbit = 0x0B,

    /// <summary>
    /// 32 Mbit (4 MiB).
    /// </summary>
    Size32Mbit = 0x0C,

    /// <summary>
    /// 64 Mbit (8 MiB).
    /// </summary>
    Size64Mbit = 0x0D,
}
