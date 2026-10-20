// <copyright file="MathHelper.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes;

using System.Globalization;

/// <summary>
/// Provides utility methods for common mathematical operations.
/// </summary>
public static class MathHelper
{
    /// <summary>
    /// Calculates the largest power of 2 that is a lower-bound for a given
    /// integer.
    /// </summary>
    /// <param name="x">
    /// The integer to lower-bound.
    /// </param>
    /// <returns>
    /// The largest power of 2 that does not exceed <paramref name="x"/>, or 0
    /// if <paramref name="x"/> is nonpositive.
    /// </returns>
    public static int BitFloor(int x)
    {
        x |= x >> 1;
        x |= x >> 2;
        x |= x >> 4;
        x |= x >> 8;
        x |= x >> 16;

        return x ^ (x >> 1);
    }

    /// <summary>
    /// Calculates the smallest power of 2 that is an upper-bound for a given integer.
    /// </summary>
    /// <param name="x">
    /// The integer to upper-bound.
    /// </param>
    /// <returns>
    /// An integer that is the smallest power of 2 that upper-bounds <paramref
    /// name="x"/>.
    /// </returns>
    /// <exception cref="OverflowException">
    /// The smallest power of 2 that upper-bounds <paramref name="x"/> cannot be
    /// represented as an <see cref="Int32"/>.
    /// </exception>
    public static int BitCeil(int x)
    {
        if (x <= 0)
        {
            return 0;
        }

        x--;
        x |= x >> 1;
        x |= x >> 2;
        x |= x >> 4;
        x |= x >> 8;
        x |= x >> 16;

        return checked(x + 1);
    }

    /// <summary>
    /// Divides <paramref name="x"/> by <paramref name="m"/> and rounds the result
    /// toward negative infinity (floor division).
    /// </summary>
    /// <param name="x">The dividend.</param>
    /// <param name="m">The divisor.</param>
    /// <returns>The quotient rounded toward negative infinity.</returns>
    /// <remarks>
    /// Unlike standard integer division, which truncates toward zero, this method
    /// rounds the quotient toward negative infinity. Intermediate calculations
    /// use <see cref="Int64"/> to avoid overflow during division and rounding.
    /// The result must fit within the range of <see cref="Int32"/>.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="m"/> is zero.
    /// </exception>
    /// <exception cref="OverflowException">
    /// The quotient rounded toward negative infinity cannot be represented as an
    /// <see cref="Int32"/>.
    /// </exception>
    public static int FloorDiv(int x, int m)
    {
        ArgumentOutOfRangeException.ThrowIfZero(m);

        var quotient = (long)x / m;
        var remainder = (long)x % m;

        if (remainder != 0 && (x < 0) != (m < 0))
        {
            quotient--;
        }

        return checked((int)quotient);
    }

    /// <summary>
    /// Divides <paramref name="x"/> by <paramref name="m"/> and rounds the result
    /// toward positive infinity (ceiling division).
    /// </summary>
    /// <param name="x">The dividend.</param>
    /// <param name="m">The divisor.</param>
    /// <returns>The quotient rounded toward positive infinity.</returns>
    /// <remarks>
    /// Unlike standard integer division, which truncates toward zero, this method
    /// rounds the quotient toward positive infinity. Intermediate calculations
    /// use <see cref="Int64"/> to avoid overflow during division and rounding.
    /// The result must fit within the range of <see cref="Int32"/>.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="m"/> is zero.
    /// </exception>
    /// <exception cref="OverflowException">
    /// The quotient rounded toward positive infinity cannot be represented as an
    /// <see cref="Int32"/>.
    /// </exception>
    public static int CeilDiv(int x, int m)
    {
        ArgumentOutOfRangeException.ThrowIfZero(m);

        var quotient = (long)x / m;
        var remainder = (long)x % m;

        if (remainder != 0 && (x < 0) == (m < 0))
        {
            quotient++;
        }

        return checked((int)quotient);
    }

    /// <summary>
    /// Attempts to parse a two-character hexadecimal string as a byte.
    /// </summary>
    /// <param name="token">The text to parse.</param>
    /// <param name="value">
    /// The parsed byte when parsing succeeds, even if <paramref name="token"/>
    /// is not exactly two characters; otherwise, zero.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="token"/> is exactly two
    /// characters and can be parsed as a hexadecimal byte; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public static bool TryGetHexByte(string token, out byte value)
    {
        return Byte.TryParse(
            token,
            NumberStyles.HexNumber,
            CultureInfo.CurrentUICulture,
            out value)
            && token.Length == 2;
    }
}
