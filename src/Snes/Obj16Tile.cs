// <copyright file="Obj16Tile.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes;

using System;

/// <summary>
/// Represents a 16x16 SNES background tile composed of four 8x8 tile entries.
/// </summary>
/// <remarks>
/// The component tiles use row-major order: top-left, top-right,
/// bottom-left, and bottom-right.
/// For background and tilemap context, see the NESdev articles on
/// <see href="https://snes.nesdev.org/wiki/Tilemaps">SNES tilemaps</see> and
/// <see href="https://snes.nesdev.org/wiki/Backgrounds">SNES backgrounds</see>.
/// </remarks>
public struct Obj16Tile : IEquatable<Obj16Tile>
{
    /// <summary>
    /// Gets the number of 8x8 tiles in an <see cref="Obj16Tile"/>.
    /// </summary>
    public const int NumberOfTiles = 4;

    /// <summary>
    /// Gets the row-major index of the top-left tile.
    /// </summary>
    public const int TopLeftIndex = 0;

    /// <summary>
    /// Gets the row-major index of the top-right tile.
    /// </summary>
    public const int TopRightIndex = 1;

    /// <summary>
    /// Gets the row-major index of the bottom-left tile.
    /// </summary>
    public const int BottomLeftIndex = 2;

    /// <summary>
    /// Gets the row-major index of the bottom-right tile.
    /// </summary>
    public const int BottomRightIndex = 3;

    /// <summary>
    /// Gets the size of the four-tile representation in bytes.
    /// </summary>
    public const int SizeOf = NumberOfTiles * ObjTile.SizeOf;

    /// <summary>
    /// Initializes an <see cref="Obj16Tile"/> from four
    /// <see cref="ObjTile"/> instances.
    /// </summary>
    /// <param name="topLeft">
    /// The top-left tile.
    /// </param>
    /// <param name="topRight">
    /// The top-right tile.
    /// </param>
    /// <param name="bottomLeft">
    /// The bottom-left tile.
    /// </param>
    /// <param name="bottomRight">
    /// The bottom-right tile.
    /// </param>
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

    /// <summary>
    /// Gets or sets the top-left 8x8 tile.
    /// </summary>
    public ObjTile TopLeft
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the bottom-left 8x8 tile.
    /// </summary>
    public ObjTile BottomLeft
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the top-right 8x8 tile.
    /// </summary>
    public ObjTile TopRight
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the bottom-right 8x8 tile.
    /// </summary>
    public ObjTile BottomRight
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets an 8x8 tile by its row-major index.
    /// </summary>
    /// <param name="index">
    /// The tile index: 0 for top-left, 1 for top-right, 2 for bottom-left,
    /// or 3 for bottom-right.
    /// </param>
    /// <returns>
    /// The tile at <paramref name="index"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="index"/> is outside the range 0 through 3.
    /// </exception>
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

    /// <summary>
    /// Determines whether two <see cref="Obj16Tile"/>s are equal.
    /// </summary>
    /// <param name="left">
    /// The first tile to compare.
    /// </param>
    /// <param name="right">
    /// The second tile to compare.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the tiles are equal; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public static bool operator ==(Obj16Tile left, Obj16Tile right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two <see cref="Obj16Tile"/>s are different.
    /// </summary>
    /// <param name="left">
    /// The first tile to compare.
    /// </param>
    /// <param name="right">
    /// The second tile to compare.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the tiles differ; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public static bool operator !=(Obj16Tile left, Obj16Tile right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Gets the horizontal coordinate of a tile in row-major order.
    /// </summary>
    /// <param name="index">
    /// The row-major tile index, from 0 through 3.
    /// </param>
    /// <returns>
    /// The horizontal coordinate, 0 for the left column or 1 for the right.
    /// </returns>
    public static int GetXCoordinate(int index)
    {
        return index % 2;
    }

    /// <summary>
    /// Gets the vertical coordinate of a tile in row-major order.
    /// </summary>
    /// <param name="index">
    /// The row-major tile index, from 0 through 3.
    /// </param>
    /// <returns>
    /// The vertical coordinate, 0 for the top row or 1 for the bottom.
    /// </returns>
    public static int GetYCoordinate(int index)
    {
        return index / 2;
    }

    /// <summary>
    /// Returns a copy of this tile flipped horizontally.
    /// </summary>
    /// <returns>
    /// The horizontally flipped copy.
    /// </returns>
    public readonly Obj16Tile FlipX()
    {
        return new Obj16Tile(
            TopRight.FlipX(),
            TopLeft.FlipX(),
            BottomRight.FlipX(),
            BottomLeft.FlipX());
    }

    /// <summary>
    /// Returns a copy of this tile flipped vertically.
    /// </summary>
    /// <returns>
    /// The vertically flipped copy.
    /// </returns>
    public readonly Obj16Tile FlipY()
    {
        return new Obj16Tile(
            BottomLeft.FlipY(),
            BottomRight.FlipY(),
            TopLeft.FlipY(),
            TopRight.FlipY());
    }

    /// <summary>
    /// Determines whether this <see cref="Obj16Tile"/> is equal to another.
    /// </summary>
    /// <param name="other">
    /// The tile to compare with this instance.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if all four component tiles are equal;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public readonly bool Equals(Obj16Tile other)
    {
        return
            TopLeft.Equals(other.TopLeft) &&
            TopRight.Equals(other.TopRight) &&
            BottomLeft.Equals(other.BottomLeft) &&
            BottomRight.Equals(other.BottomRight);
    }

    /// <summary>
    /// Determines whether this tile is equal to another object.
    /// </summary>
    /// <param name="obj">
    /// The object to compare with this instance.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="obj"/> is an equal
    /// <see cref="Obj16Tile"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public override readonly bool Equals(object? obj)
    {
        return obj is Obj16Tile tile && Equals(tile);
    }

    /// <summary>
    /// Returns a hash code for this tile.
    /// </summary>
    /// <returns>
    /// A hash code based on the four component tiles.
    /// </returns>
    public override readonly int GetHashCode()
    {
        return HashCode.Combine(TopLeft, TopRight, BottomLeft, BottomRight);
    }

    /// <summary>
    /// Returns the component tile values in top-left, bottom-left,
    /// top-right, bottom-right order.
    /// </summary>
    /// <returns>
    /// A hyphen-delimited string containing the four tile values.
    /// </returns>
    public override readonly string ToString()
    {
        return $"{TopLeft}-{TopRight}-{BottomLeft}-{BottomRight}";
    }
}
