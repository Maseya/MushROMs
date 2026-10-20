// <copyright file="Obj16Tile.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes;

using System;

public struct Obj16Tile : IEquatable<Obj16Tile>
{
    public const int NumberOfTiles = 4;

    public const int TopLeftIndex = 0;
    public const int TopRightIndex = 1;
    public const int BottomLeftIndex = 2;
    public const int BottomRightIndex = 3;

    public const int SizeOf = NumberOfTiles * ObjTile.SizeOf;

    public Obj16Tile(
        ObjTile topLeft,
        ObjTile topRight,
        ObjTile bottomLeft,
        ObjTile bottomRight)
    {
        TopLeft = topLeft;
        TopRight = topRight;
        BottomLeft = bottomLeft;
        BottomRight = bottomRight;
    }

    public ObjTile TopLeft
    {
        get;
        set;
    }

    public ObjTile BottomLeft
    {
        get;
        set;
    }

    public ObjTile TopRight
    {
        get;
        set;
    }

    public ObjTile BottomRight
    {
        get;
        set;
    }

    public ObjTile this[int index]
    {
        readonly get
        {
            return index switch
            {
                TopLeftIndex => TopLeft,
                TopRightIndex => TopRight,
                BottomLeftIndex => BottomLeft,
                BottomRightIndex => BottomRight,
                _ => throw new ArgumentOutOfRangeException(
                    nameof(index)),
            };
        }

        set
        {
            switch (index)
            {
            case TopLeftIndex:
                TopLeft = value;
                return;

            case TopRightIndex:
                TopRight = value;
                return;

            case BottomLeftIndex:
                BottomLeft = value;
                return;

            case BottomRightIndex:
                BottomRight = value;
                return;

            default:
                throw new ArgumentOutOfRangeException(
                    nameof(index));
            }
        }
    }

    public static bool operator ==(Obj16Tile left, Obj16Tile right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Obj16Tile left, Obj16Tile right)
    {
        return !(left == right);
    }

    public static int GetXCoordinate(int index)
    {
        return index % 2;
    }

    public static int GetYCoordinate(int index)
    {
        return index / 2;
    }

    public readonly Obj16Tile FlipX()
    {
        return new Obj16Tile(
            TopRight.FlipX(),
            TopLeft.FlipX(),
            BottomRight.FlipX(),
            BottomLeft.FlipX());
    }

    public readonly Obj16Tile FlipY()
    {
        return new Obj16Tile(
            BottomLeft.FlipY(),
            BottomRight.FlipY(),
            TopLeft.FlipY(),
            TopRight.FlipY());
    }

    public readonly bool Equals(Obj16Tile other)
    {
        return
            TopLeft.Equals(other.TopLeft) &&
            TopRight.Equals(other.TopRight) &&
            BottomLeft.Equals(other.BottomLeft) &&
            BottomRight.Equals(other.BottomRight);
    }

    public override readonly bool Equals(object? obj)
    {
        return obj is Obj16Tile tile && Equals(tile);
    }

    public override readonly int GetHashCode()
    {
        return HashCode.Combine(TopLeft, TopRight, BottomLeft, BottomRight);
    }

    public override readonly string ToString()
    {
        return $"{TopLeft}-{TopRight}-{BottomLeft}-{BottomRight}";
    }
}
