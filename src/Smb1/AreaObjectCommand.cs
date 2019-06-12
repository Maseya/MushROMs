// <copyright file="AreaObjectCommand.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Smb1
{
    using System;
    using System.Collections.Generic;
    using SR = Maseya.Helper.StringHelper;

    public struct AreaObjectCommand : IEquatable<AreaObjectCommand>
    {
        /// <summary>
        /// The size, in bytes, of this <see cref="AreaObjectCommand"/>.
        /// </summary>
        public const int Size = 2;

        /// <summary>
        /// The object command to read that defined the end of the area object
        /// data.
        /// </summary>
        public const byte TerminationCode = 0xFD;

        public AreaObjectCommand(byte value1, byte value2)
        {
            Value1 = value1;
            Value2 = value2;
        }

        public AreaObjectCommand(
            int x,
            int y,
            bool pageFlag,
            int command,
            int parameter)
            : this()
        {
            X = x;
            Y = y;
            PageFlag = pageFlag;
            Command = command;
            Parameter = parameter;
        }

        public int Command
        {
            get
            {
                return (Value2 >> 4) & 7;
            }

            set
            {
                Value2 &= 0x8F;
                Value2 |= (byte)((value & 7) << 4);
            }
        }

        public AreaObjectCode ObjectType
        {
            get
            {
                return (AreaObjectCode)(Y >= 0x0C
                    ? Y == 0x0D
                        ? Command == 0
                            ? (int)AreaObjectCode.PageSkip
                            : ((Y << 8) | (Command << 4) | Parameter)
                        : ((Y << 8) | (Command << 4))
                    : Command == 0
                        ? Parameter
                        : (Command << 4));
            }
        }

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

        public int Parameter
        {
            get
            {
                return Value2 & 0x0F;
            }

            set
            {
                Value2 &= 0xF0;
                Value2 |= (byte)(value & 0x0F);
            }
        }

        public byte Value1
        {
            get;
            set;
        }

        public byte Value2
        {
            get;
            set;
        }

        public int X
        {
            get
            {
                return Value1 >> 4;
            }

            set
            {
                Value1 &= 0x0F;
                Value1 |= (byte)((value << 4) & 0xF0);
            }
        }

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
            AreaObjectCommand left,
            AreaObjectCommand right)
        {
            return !(left == right);
        }

        public static bool operator ==(
            AreaObjectCommand left,
            AreaObjectCommand right)
        {
            return left.Equals(right);
        }

        public static IEnumerable<byte> GetAreaByteData(
            IEnumerable<AreaObjectCommand> collection)
        {
            foreach (var command in collection)
            {
                yield return command.Value1;
                yield return command.Value2;
            }

            yield return TerminationCode;
        }

        public static IEnumerable<AreaObjectCommand> GetAreaData(
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

                    var list = new List<byte>(GetBytes());
                    yield return new AreaObjectCommand(
                        list[0],
                        list[1]);
                }

                IEnumerable<byte> GetBytes()
                {
                    for (var i = 0; i < Size; i++)
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
            return obj is AreaObjectCommand other ? Equals(other) : false;
        }

        public bool Equals(AreaObjectCommand other)
        {
            return Value1.Equals(other.Value1) && Value2.Equals(other.Value2);
        }

        public override int GetHashCode()
        {
            return Value1 | (Value2 << 8);
        }

        public override string ToString()
        {
            return SR.GetString($"({X:X}, {Y:X}): {ObjectType}");
        }
    }
}
