// <copyright file="LayerPriority.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes;

/// <summary>
/// Specifies a tile's drawing order within its layer.
/// </summary>
/// <remarks>
/// The SNES supports multiple BG and CHR layers, each of which can be assigned
/// a priority level. When multiple layers overlap, the layer with the higher
/// priority is drawn on top of the lower-priority layers. The exact order
/// between each layer depends on the SNES's display mode and the layer's
/// priority level, but lower priority levels are always drawn first, behind
/// higher priority levels.
/// </remarks>
public enum LayerPriority
{
    /// <summary>
    /// The lowest priority level.
    /// </summary>
    Priority0 = 0,

    /// <summary>
    /// The second-lowest priority level.
    /// </summary>
    Priority1 = 1,

    /// <summary>
    /// The second-highest priority level.
    /// </summary>
    Priority2 = 2,

    /// <summary>
    /// The highest priority level.
    /// </summary>
    Priority3 = 3,
}
