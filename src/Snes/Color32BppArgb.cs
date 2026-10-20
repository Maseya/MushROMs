// <copyright file="Color32BppArgb.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes;

using System;
using System.Drawing;
using System.Runtime.InteropServices;

/// <summary>
/// Represents a 32-bit color with 8-bit alpha, red, green, and blue channels.
/// </summary>
/// <remarks>
/// Its sequential layout is designed for working with <see cref="Bitmap"/>
/// pixels using <see cref="PixelFormat.Format32bppArgb"/>.
/// </remarks>
[StructLayout(LayoutKind.Sequential)]
public unsafe struct Color32BppArgb : IEquatable<Color32BppArgb>
{
    /// <summary>
    /// The size of a <see cref="Color32BppArgb"/> value, in bytes.
    /// </summary>
    public const int SizeOf = 4;

    /// <summary>
    /// Gets or sets the blue channel.
    /// </summary>
    public byte B
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the green channel.
    /// </summary>
    public byte G
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the red channel.
    /// </summary>
    public byte R
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the alpha channel.
    /// </summary>
    public byte A
    {
        get;
        set;
    }

    /// <summary>
    /// Gets the packed 32-bit ARGB value.
    /// </summary>
    public readonly int Value
    {
        get
        {
            return this;
        }
    }

    /// <summary>
    /// Initializes a color from its 8-bit ARGB channel values.
    /// </summary>
    /// <param name="alpha">
    /// The alpha channel value.
    /// </param>
    /// <param name="red">
    /// The red channel value.
    /// </param>
    /// <param name="green">
    /// The green channel value.
    /// </param>
    /// <param name="blue">
    /// The blue channel value.
    /// </param>
    /// <remarks>
    /// Each argument is cast to a byte, discarding any bits above the lowest
    /// eight.
    /// </remarks>
    public Color32BppArgb(int alpha, int red, int green, int blue)
    {
        A = (byte)alpha;
        R = (byte)red;
        G = (byte)green;
        B = (byte)blue;
    }

    /// <summary>
    /// Converts the low and high bytes of an SNES color value to a color.
    /// </summary>
    /// <param name="low">
    /// The low byte of the SNES color value.
    /// </param>
    /// <param name="high">
    /// The high byte of the SNES color value.
    /// </param>
    /// <returns>
    /// The expanded 32-bit color.
    /// </returns>
    /// <remarks>
    /// The alpha channel is set to 255. Each 5-bit RGB channel is shifted left
    /// by three bits without bit replication.
    /// </remarks>
    public static Color32BppArgb FromSnesColor(int low, int high)
    {
        return FromSnesColor(low | (high << 8));
    }

    /// <summary>
    /// Converts a 15-bit SNES color value to a 32-bit color.
    /// </summary>
    /// <param name="value">
    /// The SNES color value, with 5-bit red, green, and blue channels.
    /// </param>
    /// <returns>
    /// The expanded 32-bit color.
    /// </returns>
    /// <remarks>
    /// The alpha channel is set to 255. Each 5-bit RGB channel is shifted left
    /// by three bits without bit replication.
    /// </remarks>
    public static Color32BppArgb FromSnesColor(int value)
    {
        return new Color32BppArgb(
            Byte.MaxValue,
            ((value >> (5 * 0)) & 0x1F) << 3,
            ((value >> (5 * 1)) & 0x1F) << 3,
            ((value >> (5 * 2)) & 0x1F) << 3);
    }

    /// <summary>
    /// Converts a 32-bit color to a 15-bit SNES color value.
    /// </summary>
    /// <param name="color">
    /// The color to convert.
    /// </param>
    /// <returns>
    /// The SNES color value, with 5-bit red, green, and blue channels.
    /// </returns>
    /// <remarks>
    /// The alpha channel is ignored, and the lowest three bits of each RGB
    /// channel are discarded.
    /// </remarks>
    public static int ToSnesColor(Color32BppArgb color)
    {
        return (color.R >> 3) | ((color.G >> 3) << 5) | ((color.B >> 3) << 10);
    }

    /// <summary>
    /// Determines whether two colors have equal channel values.
    /// </summary>
    /// <param name="left">
    /// The first color to compare.
    /// </param>
    /// <param name="right">
    /// The second color to compare.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the colors are equal; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public static bool operator ==(Color32BppArgb left, Color32BppArgb right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two colors have different channel values.
    /// </summary>
    /// <param name="left">
    /// The first color to compare.
    /// </param>
    /// <param name="right">
    /// The second color to compare.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the colors are different; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public static bool operator !=(Color32BppArgb left, Color32BppArgb right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Converts a color to its packed 32-bit ARGB value.
    /// </summary>
    /// <param name="pixel">
    /// The color to convert.
    /// </param>
    /// <returns>
    /// The packed ARGB value.
    /// </returns>
    public static implicit operator int(Color32BppArgb pixel)
    {
        return *(int*)&pixel;
    }

    /// <summary>
    /// Converts a packed 32-bit ARGB value to a color.
    /// </summary>
    /// <param name="value">
    /// The packed ARGB value.
    /// </param>
    /// <returns>
    /// The color represented by <paramref name="value"/>.
    /// </returns>
    public static implicit operator Color32BppArgb(int value)
    {
        return *(Color32BppArgb*)&value;
    }

    /// <summary>
    /// Converts a color to a <see cref="Color"/>.
    /// </summary>
    /// <param name="pixel">
    /// The color to convert.
    /// </param>
    /// <returns>
    /// The equivalent <see cref="Color"/>.
    /// </returns>
    public static implicit operator Color(Color32BppArgb pixel)
    {
        return Color.FromArgb(pixel);
    }

    /// <summary>
    /// Converts a <see cref="Color"/> to a color value.
    /// </summary>
    /// <param name="color">
    /// The color to convert.
    /// </param>
    /// <returns>
    /// The equivalent <see cref="Color32BppArgb"/>.
    /// </returns>
    public static explicit operator Color32BppArgb(Color color)
    {
        return color.ToArgb();
    }

    /// <summary>
    /// Returns the packed ARGB value as the hash code.
    /// </summary>
    /// <returns>
    /// The packed ARGB value.
    /// </returns>
    public override readonly int GetHashCode()
    {
        return Value;
    }

    /// <summary>
    /// Determines whether this color has the same channel values as another.
    /// </summary>
    /// <param name="other">
    /// The color to compare with this instance.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the colors are equal; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public readonly bool Equals(Color32BppArgb other)
    {
        return Value == other.Value;
    }

    /// <summary>
    /// Determines whether this color has the same channel values as an object.
    /// </summary>
    /// <param name="obj">
    /// The object to compare with this instance.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="obj"/> is an equal
    /// <see cref="Color32BppArgb"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public override readonly bool Equals(object? obj)
    {
        return obj is Color32BppArgb other && Equals(other);
    }

    /// <summary>
    /// Returns the packed ARGB value as an uppercase hexadecimal string.
    /// </summary>
    /// <returns>
    /// The packed ARGB value in uppercase hexadecimal, padded to at least six
    /// digits.
    /// </returns>
    public override readonly string ToString()
    {
        return Value.ToString("X6");
    }
}
