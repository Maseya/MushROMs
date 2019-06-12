// <copyright file="AreaSpriteCommand.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Smb1
{
    using System;
    using System.Collections.Generic;
    using SR = Maseya.Helper.StringHelper;

    public struct AreaSpriteCommand : IEquatable<AreaSpriteCommand>
    {
        public const byte TerminationCode = 0xFF;

        public AreaSpriteCommand(byte value1, byte value2)
            : this(value1, value2, 0)
        {
        }

        public AreaSpriteCommand(byte value1, byte value2, byte value3)
        {
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
        }

        /// <summary>
        /// Gets or sets the command value of this <see
        /// cref="AreaSpriteCommand"/>.
        /// </summary>
        public AreaSpriteCode Code
        {
            get
            {
                return (AreaSpriteCode)(Value2 & 0x3F);
            }

            set
            {
                Value2 &= 0xC0;
                Value2 |= (byte)((int)value & 0x3F);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see
        /// cref="AreaSpriteCommand"/> only spawns after the hard world flag
        /// has been set.
        /// </summary>
        public bool HardWorldFlag
        {
            get
            {
                return (Value2 & 0x40) != 0;
            }

            set
            {
                if (value)
                {
                    Value2 |= 0x40;
                }
                else
                {
                    Value2 &= 0xBF;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see
        /// cref="AreaSpriteCommand"/> starts on the next page.
        /// </summary>
        public bool PageFlag
        {
            get
            {
                return (Value2 & 0x80) != 0;
            }

            set
            {
                if (value)
                {
                    Value2 |= 0x80;
                }
                else
                {
                    Value2 &= 0x7F;
                }
            }
        }

        /// <summary>
        /// Gets the size, in bytes, of this <see cref="AreaSpriteCommand"/>.
        /// </summary>
        public int Size
        {
            get
            {
                return Y == 0x0E ? 3 : 2;
            }
        }

        /// <summary>
        /// Gets or sets the first value of this <see
        /// cref="AreaSpriteCommand"/>.
        /// </summary>
        public byte Value1
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the second value of this <see
        /// cref="AreaSpriteCommand"/>.
        /// </summary>
        public byte Value2
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the third value of this <see
        /// cref="AreaSpriteCommand"/>.
        /// </summary>
        public byte Value3
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the X-coordinate of this <see
        /// cref="AreaSpriteCommand"/>. The coordinate is relative to the page
        /// the object is in.
        /// </summary>
        public int X
        {
            get
            {
                return Value1 >> 4;
            }

            set
            {
                Value1 &= 0x0F;
                Value1 |= (byte)((value & 0x0F) << 4);
            }
        }

        /// <summary>
        /// Gets or sets the Y-coordinate of this <see
        /// cref="AreaSpriteCommand"/>.
        /// </summary>
        public int Y
        {
            get
            {
                return Value1 & 0x0F;
            }

            set
            {
                Value1 &= 0xF0;
                Value1 |= (byte)(value & 0x0F);
            }
        }

        public static bool operator !=(
            AreaSpriteCommand left,
            AreaSpriteCommand right)
        {
            return !(left == right);
        }

        public static bool operator ==(
            AreaSpriteCommand left,
            AreaSpriteCommand right)
        {
            return left.Equals(right);
        }

        public static IEnumerable<byte> GetAreaByteData(
            IEnumerable<AreaSpriteCommand> collection)
        {
            foreach (var command in collection)
            {
                yield return command.Value1;
                yield return command.Value2;
                if (command.Size == 3)
                {
                    yield return command.Value3;
                }
            }

            yield return TerminationCode;
        }

        public static IEnumerable<AreaSpriteCommand> GetAreaData(
            IEnumerable<byte> bytes)
        {
            if (bytes is null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            using (var en = bytes.GetEnumerator())
            {
                while (en.MoveNext())
                {
                    if (en.Current == TerminationCode)
                    {
                        yield break;
                    }

                    if (IsThreeByteSpecifier(en.Current))
                    {
                        var list = new List<byte>(GetBytes(3));
                        yield return new AreaSpriteCommand(
                            list[0],
                            list[1],
                            list[2]);
                    }
                    else
                    {
                        var list = new List<byte>(GetBytes(2));
                        yield return new AreaSpriteCommand(
                            list[0],
                            list[1]);
                    }
                }

                IEnumerable<byte> GetBytes(int size)
                {
                    for (var i = 0; i < size; i++)
                    {
                        yield return en.Current;

                        if (!en.MoveNext())
                        {
                            throw NoMoreBytesException();
                        }
                    }
                }
            }

            throw NoMoreBytesException();

            ArgumentException NoMoreBytesException()
            {
                return new ArgumentException();
            }
        }

        public override bool Equals(object obj)
        {
            return obj is AreaSpriteCommand other ? Equals(other) : false;
        }

        public bool Equals(AreaSpriteCommand other)
        {
            return !Size.Equals(other.Size)
                ? false
                : !Value1.Equals(other.Value1) || Value2.Equals(other.Value2)
                    ? false
                    : Size == 2 || Value3.Equals(other.Value3);
        }

        public override int GetHashCode()
        {
            return Value1 | (Value2 << 8) | (Value3 << 0x10);
        }

        public override string ToString()
        {
            return SR.GetString($"({X}, {Y}): {Code}");
        }

        private static bool IsThreeByteSpecifier(int coordinates)
        {
            return (coordinates & 0x0F) == 0x0E;
        }
    }
}
