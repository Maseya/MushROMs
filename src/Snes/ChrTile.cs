// <copyright file="ChrTile.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes;

using System;

/// <summary>
/// Represents a SNES character tile-map entry stored as a 16-bit value.
/// </summary>
/// <remarks>
/// The value contains a 9-bit tile index, a 3-bit palette index,
/// a 2-bit layer priority, and horizontal and vertical flip flags.
/// </remarks>
public struct ChrTile : IEquatable<ChrTile>
{
    /// <summary>
    /// Gets the size of a tile entry in bytes.
    /// </summary>
    public const int SizeOf = sizeof(ushort);

    /// <summary>
    /// Mask for the 9-bit tile index.
    /// </summary>
    private const int TileIndexMask = 0x1FF;

    /// <summary>
    /// Starting bit of the palette index.
    /// </summary>
    private const int PaletteOffset = 9;

    /// <summary>
    /// Mask for the 3-bit palette index.
    /// </summary>
    private const int PaletteMask = 7;

    /// <summary>
    /// Starting bit of the layer priority.
    /// </summary>
    private const int PriorityOffset = 12;

    /// <summary>
    /// Mask for the 2-bit layer priority.
    /// </summary>
    private const int PriorityMask = 3;

    /// <summary>
    /// Starting bit of the flip flags.
    /// </summary>
    private const int FlipOffset = 14;

    /// <summary>
    /// Bit offset of the horizontal flip flag.
    /// </summary>
    private const int FlipXOffset = FlipOffset;

    /// <summary>
    /// Bit offset of the vertical flip flag.
    /// </summary>
    private const int FlipYOffset = FlipOffset + 1;

    /// <summary>
    /// Mask for both flip flags.
    /// </summary>
    private const int FlipMask = 3;

    private ushort _value;

    /// <summary>
    /// Initializes a tile entry from its tile index and display attributes.
    /// </summary>
    /// <param name="tileIndex">
    /// The index of the character tile.
    /// </param>
    /// <param name="paletteIndex">
    /// The palette index.
    /// </param>
    /// <param name="layerPriority">
    /// The layer priority.
    /// </param>
    /// <param name="tileFlip">
    /// The horizontal and vertical flip flags.
    /// </param>
    /// <remarks>
    /// Each argument is masked to fit its field in the value.
    /// </remarks>
    public ChrTile(
        int tileIndex,
        int paletteIndex,
        LayerPriority layerPriority,
        TileFlip tileFlip)
    {
        _value = (ushort)(tileIndex & TileIndexMask);
        Value |= (paletteIndex & PaletteMask) << PaletteOffset;
        Value |= ((int)layerPriority & PriorityMask) << PriorityOffset;
        Value |= ((int)tileFlip & FlipMask) << FlipOffset;
    }

    /// <summary>
    /// Initializes a tile entry from its value.
    /// </summary>
    /// <param name="value">
    /// The tile entry value; only its lowest 16 bits are used.
    /// </param>
    private ChrTile(int value)
    {
        _value = (ushort)value;
    }

    /// <summary>
    /// Gets or sets the 16-bit tile entry value.
    /// </summary>
    /// <remarks>
    /// Values assigned to this property are truncated to 16 bits.
    /// </remarks>
    public int Value
    {
        readonly get
        {
            return _value;
        }

        set
        {
            _value = (ushort)value;
        }
    }

    /// <summary>
    /// Gets or sets the character tile index.
    /// </summary>
    /// <remarks>
    /// The index occupies bits 0 through 8 and is masked to 9 bits.
    /// </remarks>
    public int TileIndex
    {
        readonly get
        {
            return Value & TileIndexMask;
        }

        set
        {
            Value &= ~TileIndexMask;
            Value |= value & TileIndexMask;
        }
    }

    /// <summary>
    /// Gets or sets the palette index.
    /// </summary>
    /// <remarks>
    /// The index occupies bits 9 through 11 and is masked to 3 bits.
    /// </remarks>
    public int PaletteIndex
    {
        readonly get
        {
            return (Value >> PaletteOffset) & PaletteMask;
        }

        set
        {
            Value &= ~(PaletteMask << PaletteOffset);
            Value |= (value & PaletteMask) << PaletteOffset;
        }
    }

    /// <summary>
    /// Gets or sets the layer priority.
    /// </summary>
    /// <remarks>
    /// The priority occupies bits 12 and 13.
    /// </remarks>
    public LayerPriority Priority
    {
        readonly get
        {
            return (LayerPriority)(
                (Value >> PriorityOffset) & PriorityMask);
        }

        set
        {
            Value &= ~(PriorityMask << PriorityOffset);
            Value |= ((int)value & PriorityMask) << PriorityOffset;
        }
    }

    /// <summary>
    /// Gets or sets the horizontal and vertical flip flags.
    /// </summary>
    /// <remarks>
    /// The flags occupy bits 14 and 15.
    /// </remarks>
    public TileFlip TileFlip
    {
        readonly get
        {
            return (TileFlip)((Value >> FlipOffset) & FlipMask);
        }

        set
        {
            Value &= ~(FlipMask << FlipOffset);
            Value |= ((int)value & FlipMask) << FlipOffset;
        }
    }

    /// <summary>
    /// Gets or sets whether the tile is flipped horizontally.
    /// </summary>
    public bool XFlipped
    {
        readonly get
        {
            return ((Value >> FlipXOffset) & 1) != 0;
        }

        set
        {
            if (value)
            {
                Value |= 1 << FlipXOffset;
            }
            else
            {
                Value &= ~(1 << FlipXOffset);
            }
        }
    }

    /// <summary>
    /// Gets or sets whether the tile is flipped vertically.
    /// </summary>
    public bool YFlipped
    {
        readonly get
        {
            return ((Value >> FlipYOffset) & 1) != 0;
        }

        set
        {
            if (value)
            {
                Value |= 1 << FlipYOffset;
            }
            else
            {
                Value &= ~(1 << FlipYOffset);
            }
        }
    }

    /// <summary>
    /// Converts a tile entry to its integer value.
    /// </summary>
    /// <param name="tile">
    /// The tile entry to convert.
    /// </param>
    /// <returns>
    /// The 16-bit value as an integer.
    /// </returns>
    public static implicit operator int(ChrTile tile)
    {
        return tile.Value;
    }

    /// <summary>
    /// Creates a tile entry from an integer value.
    /// </summary>
    /// <param name="value">
    /// The value; only its lowest 16 bits are used.
    /// </param>
    /// <returns>
    /// The tile entry represented by <paramref name="value"/>.
    /// </returns>
    public static implicit operator ChrTile(int value)
    {
        return new ChrTile(value);
    }

    /// <summary>
    /// Determines whether two tile entries have the same value.
    /// </summary>
    /// <param name="left">
    /// The first tile entry to compare.
    /// </param>
    /// <param name="right">
    /// The second tile entry to compare.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the values are equal; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public static bool operator ==(ChrTile left, ChrTile right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two tile entries have different values.
    /// </summary>
    /// <param name="left">
    /// The first tile entry to compare.
    /// </param>
    /// <param name="right">
    /// The second tile entry to compare.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the values differ; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public static bool operator !=(ChrTile left, ChrTile right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Determines whether this tile entry has the same value as another.
    /// </summary>
    /// <param name="obj">
    /// The tile entry to compare with this instance.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the values are equal; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public readonly bool Equals(ChrTile obj)
    {
        return Value.Equals(obj.Value);
    }

    /// <summary>
    /// Determines whether this tile entry is equal to the specified object.
    /// </summary>
    /// <param name="obj">
    /// The object to compare with this instance.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="obj"/> is an equal
    /// <see cref="ChrTile"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public override readonly bool Equals(object? obj)
    {
        return obj is ChrTile tile && Equals(tile);
    }

    /// <summary>
    /// Returns a hash code for this tile entry.
    /// </summary>
    /// <returns>
    /// The tile entry value.
    /// </returns>
    public override readonly int GetHashCode()
    {
        return Value;
    }

    /// <summary>
    /// Returns the value formatted as four hexadecimal digits.
    /// </summary>
    /// <returns>
    /// The value in uppercase hexadecimal format.
    /// </returns>
    public override readonly string ToString()
    {
        return $"{Value:X4}";
    }
}
