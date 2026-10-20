// <copyright file="ObjTile.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes;

using System;

/// <summary>
/// Represents a single SNES tilemap entry that can be used by a background.
/// </summary>
/// <remarks>
/// The value contains a 10-bit tile index, a 3-bit palette index, a 1-bit
/// priority, and horizontal and vertical flip flags.
/// In 16x16 tile mode, the tilemap entry represents a 2x2 group of tiles;
/// use <see cref="Obj16Tile"/> to represent that group.
/// For more information, see the NESdev articles on
/// <see href="https://snes.nesdev.org/wiki/Tilemaps">SNES tilemaps</see> and
/// <see href="https://snes.nesdev.org/wiki/Backgrounds">SNES backgrounds</see>.
/// </remarks>
public struct ObjTile : IEquatable<ObjTile>
{
    /// <summary>
    /// Gets the size of an object tile entry in bytes.
    /// </summary>
    public const int SizeOf = sizeof(ushort);

    /// <summary>
    /// Gets the number of colors in each object palette.
    /// </summary>
    public const int MaxColorsPerPalette = 0x10;

    /// <summary>
    /// Gets the maximum number of object palettes.
    /// </summary>
    public const int MaxPalettes = 8;

    /// <summary>
    /// Gets the maximum number of colors supported across object palettes.
    /// </summary>
    public const int MaxSupportColors = MaxColorsPerPalette * MaxPalettes;

    /// <summary>
    /// Mask for the 10-bit tile index.
    /// </summary>
    private const int TileIndexMask = 0x3FF;

    /// <summary>
    /// Starting bit of the palette index.
    /// </summary>
    private const int PaletteOffset = 10;

    /// <summary>
    /// Mask for the 3-bit palette index.
    /// </summary>
    private const int PaletteMask = 7;

    /// <summary>
    /// Bit offset of the priority flag.
    /// </summary>
    private const int PriorityOffset = 13;

    /// <summary>
    /// Mask for the 1-bit priority flag.
    /// </summary>
    private const int PriorityMask = 1;

    /// <summary>
    /// Starting bit of the flip flags.
    /// </summary>
    private const int FlipOffset = 14;

    /// <summary>
    /// Mask for both flip flags.
    /// </summary>
    private const int FlipMask = 3;

    private ushort _value;

    /// <summary>
    /// Initializes an object tile entry from its 16-bit value.
    /// </summary>
    /// <param name="value">
    /// The entry value. Only its lowest 16 bits are used.
    /// </param>
    private ObjTile(int value)
    {
        _value = (ushort)value;
    }

    /// <summary>
    /// Gets or sets the 16-bit object tile entry value.
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
    /// Gets or sets the object tile index.
    /// </summary>
    /// <remarks>
    /// The index occupies bits 0 through 9 and is masked to 10 bits.
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
    /// Gets or sets the object palette index.
    /// </summary>
    /// <remarks>
    /// The index occupies bits 10 through 12 and is masked to 3 bits.
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
    /// Gets or sets the object tile priority.
    /// </summary>
    /// <remarks>
    /// The priority flag occupies bit 13. BG tilemaps only support support two
    /// layer priorities. Therefore, only the least significant bit of the
    /// <see cref="LayerPriority"/> value is stored.
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
    /// Gets or sets whether the object tile is flipped horizontally.
    /// </summary>
    public bool XFlipped
    {
        readonly get
        {
            return
                (TileFlip & TileFlip.Horizontal) != 0;
        }

        set
        {
            if (value)
            {
                TileFlip |= TileFlip.Horizontal;
            }
            else
            {
                TileFlip &= ~TileFlip.Horizontal;
            }
        }
    }

    /// <summary>
    /// Gets or sets whether the object tile is flipped vertically.
    /// </summary>
    public bool YFlipped
    {
        readonly get
        {
            return (TileFlip & TileFlip.Veritcal) != 0;
        }

        set
        {
            if (value)
            {
                TileFlip |= TileFlip.Veritcal;
            }
            else
            {
                TileFlip &= ~TileFlip.Veritcal;
            }
        }
    }

    /// <summary>
    /// Converts an object tile to its 16-bit entry value.
    /// </summary>
    /// <param name="tile">
    /// The object tile to convert.
    /// </param>
    /// <returns>
    /// The tile's entry value.
    /// </returns>
    public static implicit operator int(ObjTile tile)
    {
        return tile.Value;
    }

    /// <summary>
    /// Converts a 16-bit entry value to an object tile.
    /// </summary>
    /// <param name="value">
    /// The entry value. Only its lowest 16 bits are used.
    /// </param>
    /// <returns>
    /// The object tile represented by <paramref name="value"/>.
    /// </returns>
    public static implicit operator ObjTile(int value)
    {
        return new ObjTile(value);
    }

    /// <summary>
    /// Determines whether two object tile entries have the same value.
    /// </summary>
    /// <param name="left">
    /// The first entry to compare.
    /// </param>
    /// <param name="right">
    /// The second entry to compare.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the entries are equal; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public static bool operator ==(ObjTile left, ObjTile right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two object tile entries have different values.
    /// </summary>
    /// <param name="left">
    /// The first entry to compare.
    /// </param>
    /// <param name="right">
    /// The second entry to compare.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the entries differ; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public static bool operator !=(ObjTile left, ObjTile right)
    {
        return left.Value != right.Value;
    }

    /// <summary>
    /// Returns a copy of this entry with its horizontal flip flag toggled.
    /// </summary>
    /// <returns>
    /// The horizontally flipped copy.
    /// </returns>
    public readonly ObjTile FlipX()
    {
        var tile = this;
        tile.XFlipped ^= true;
        return tile;
    }

    /// <summary>
    /// Returns a copy of this entry with its vertical flip flag toggled.
    /// </summary>
    /// <returns>
    /// The vertically flipped copy.
    /// </returns>
    public readonly ObjTile FlipY()
    {
        var tile = this;
        tile.YFlipped ^= true;
        return tile;
    }

    /// <summary>
    /// Determines whether this entry has the same value as another entry.
    /// </summary>
    /// <param name="other">
    /// The entry to compare with this instance.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the values are equal; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public readonly bool Equals(ObjTile other)
    {
        return Value.Equals(other.Value);
    }

    /// <summary>
    /// Determines whether this entry has the same value as another object.
    /// </summary>
    /// <param name="obj">
    /// The object to compare with this instance.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="obj"/> is an equal
    /// <see cref="ObjTile"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public override readonly bool Equals(object? obj)
    {
        return obj is ObjTile tile && Equals(tile);
    }

    /// <summary>
    /// Returns the 16-bit entry value as the hash code.
    /// </summary>
    /// <returns>
    /// The entry value.
    /// </returns>
    public override readonly int GetHashCode()
    {
        return Value;
    }

    /// <summary>
    /// Returns the entry value as a four-digit hexadecimal string.
    /// </summary>
    /// <returns>
    /// The entry value formatted in uppercase hexadecimal.
    /// </returns>
    public override readonly string ToString()
    {
        return Value.ToString("X4");
    }
}
