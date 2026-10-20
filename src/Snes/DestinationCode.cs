// <copyright file="DestinationCode.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes;

/// <summary>
/// Specifies the destination region or language recorded in a SNES ROM header.
/// </summary>
/// <remarks>
/// Values correspond to the destination code at $00FFD9. See the
/// <see href="https://snes.nesdev.org/wiki/ROM_header">NESdev SNES ROM header
/// documentation</see> for the region codes.
/// </remarks>
public enum DestinationCode
{
    /// <summary>
    /// Japan.
    /// </summary>
    Japan = 0x00,

    /// <summary>
    /// North America, originally covered USA and Canada
    /// </summary>
    NorthAmerica = 0x01,

    /// <summary>
    /// Europe, originally also covering Oceania and Asia.
    /// </summary>
    Europe = 0x02,

    /// <summary>
    /// Scandinavia, originally specific to Sweden.
    /// </summary>
    Scandinavia = 0x03,

    /// <summary>
    /// French-language Europe.
    /// </summary>
    France = 0x06,

    /// <summary>
    /// The Netherlands.
    /// </summary>
    Dutch = 0x07,

    /// <summary>
    /// Spain.
    /// </summary>
    Spanish = 0x08,

    /// <summary>
    /// Germany, Austria, and Switzerland.
    /// </summary>
    German = 0x09,

    /// <summary>
    /// Italy.
    /// </summary>
    Italian = 0x0A,

    /// <summary>
    /// Hong Kong and mainland China.
    /// </summary>
    Chinese = 0x0B,

    /// <summary>
    /// South Korea.
    /// </summary>
    Korean = 0x0D,

    /// <summary>
    /// Common destination code.
    /// </summary>
    Common = 0x0E,

    /// <summary>
    /// Canada.
    /// </summary>
    Canada = 0x0F,

    /// <summary>
    /// Brazil.
    /// </summary>
    Brazil = 0x10,

    /// <summary>
    /// Australia.
    /// </summary>
    Australia = 0x11,

    /// <summary>
    /// Other variation X.
    /// </summary>
    OtherVariationX = 0x12,

    /// <summary>
    /// Other variation Y.
    /// </summary>
    OtherVariationY = 0x13,

    /// <summary>
    /// Other variation Z.
    /// </summary>
    OtherVariationZ = 0x14,
}
