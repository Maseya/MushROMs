// <copyright file="MathHelper.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes;

using System.Globalization;

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
