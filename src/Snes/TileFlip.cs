// <copyright file="TileFlip.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes;

using System;

/// <summary>
/// Specifies the horizontal and vertical flip flags for a SNES tile entry.
/// </summary>
[Flags]
public enum TileFlip
{
    /// <summary>
    /// Does not flip the tile.
    /// </summary>
    None = 0,

    /// <summary>
    /// Flips the tile horizontally.
    /// </summary>
    Horizontal = 1,

    /// <summary>
    /// Flips the tile vertically.
    /// </summary>
    Veritcal = 2,

    /// <summary>
    /// Flips the tile horizontally and vertically.
    /// </summary>
    Both = Horizontal | Veritcal,
}
