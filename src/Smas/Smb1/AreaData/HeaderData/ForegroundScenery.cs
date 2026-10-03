// <copyright file="ForegroundScenery.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Smas.Smb1.AreaData.HeaderData;

/// <summary>
/// The layer 1 foreground to use for the current area.
/// </summary>
public enum ForegroundScenery
{
    /// <summary>
    /// No background.
    /// </summary>
    None,

    /// <summary>
    /// User for underwater area (e.g. main area of W2-2).
    /// </summary>
    Underwater,

    /// <summary>
    /// A castle wall that is behind the player (e.g. main area of W8-3).
    /// </summary>
    CastleWall,

    /// <summary>
    /// Water or lava is at ground level (e.g. main area of W2-3).
    /// </summary>
    OverWater,

    /// <summary>
    /// Unused.
    /// </summary>
    Night,

    /// <summary>
    /// Unused.
    /// </summary>
    Snow,

    /// <summary>
    /// Unused.
    /// </summary>
    NightAndSnow,

    /// <summary>
    /// Unused.
    /// </summary>
    Castle,
}
