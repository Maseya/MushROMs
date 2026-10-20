// <copyright file="ObjTile.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes;

using System;

public struct ObjTile : IEquatable<ObjTile>
{
    public const int SizeOf = sizeof(ushort);

    private const int TileIndexMask = 0x3FF;
    private const int PaletteOffset = 10;
    private const int PaletteMask = 7;
    private const int PriorityOffset = 13;
    private const int PriorityMask = 1;
    private const int FlipOffset = 14;
    private const int FlipMask = 3;

    private ushort _value;

    private ObjTile(int value)
    {
        _value = (ushort)value;
    }

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

    public LayerPriority Priority
    {
        readonly get
        {
            return (LayerPriority)(
                (Value >> PriorityOffset) & PriorityMask);
        }

        set
        {
            if (value != LayerPriority.Priority0)
            {
                Value |= PriorityMask << PriorityOffset;
            }
            else
            {
                Value &= ~PriorityMask << PriorityOffset;
            }
        }
    }

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

    public static implicit operator int(ObjTile tile)
    {
        return tile.Value;
    }

    public static implicit operator ObjTile(int value)
    {
        return new ObjTile(value);
    }

    public static bool operator ==(ObjTile left, ObjTile right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ObjTile left, ObjTile right)
    {
        return left.Value != right.Value;
    }

    public readonly ObjTile FlipX()
    {
        var tile = this;
        tile.XFlipped ^= true;
        return tile;
    }

    public readonly ObjTile FlipY()
    {
        var tile = this;
        tile.YFlipped ^= true;
        return tile;
    }

    public readonly bool Equals(ObjTile other)
    {
        return Value.Equals(other.Value);
    }

    public override readonly bool Equals(object? obj)
    {
        return obj is ObjTile tile && Equals(tile);
    }

    public override readonly int GetHashCode()
    {
        return Value;
    }

    public override readonly string ToString()
    {
        return Value.ToString("X4");
    }
}
