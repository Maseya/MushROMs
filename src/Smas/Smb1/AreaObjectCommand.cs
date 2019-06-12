namespace Maseya.Smas.Smb1
{
    using System;
    using System.Collections.Generic;
    using SR = Helper.StringHelper;

    public struct AreaObjectCommand : IEquatable<AreaObjectCommand>
    {
        /// <summary>
        /// The object command to read that defined the end of the area object
        /// data.
        /// </summary>
        public const byte TerminationCode = 0xFD;

        public AreaObjectCommand(byte value1, byte value2)
            : this(value1, value2, 0)
        { }

        public AreaObjectCommand(byte value1, byte value2, byte value3)
        {
            Value1 = value1;
            Value2 = value2;
            Value3 = (byte)(((value1 & 0x0F) == 0x0F) ? value3 : 0);
        }

        public AreaObjectCommand(
            int x,
            int y,
            bool pageFlag,
            int command,
            int parameter,
            int extendedCommand)
            : this()
        {
            ExtendedCommand = extendedCommand;
            X = x;
            Y = y;
            PageFlag = pageFlag;
            Command = command;
            Parameter = parameter;
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

        public byte Value3
        {
            get;
            set;
        }

        /// <summary>
        /// The size, in bytes, of this <see cref="AreaObjectCommand"/>.
        /// </summary>
        public int Size
        {
            get
            {
                return IsThreeByteObject ? 3 : 2;
            }
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
                Value1 |= (byte)((value & 0x0F) << 4);
            }
        }

        public int Y
        {
            get
            {
                return IsThreeByteObject ? Value2 >> 4 : Value1 & 0x0F;
            }

            set
            {
                if (IsThreeByteObject)
                {
                    Value2 &= 0x0F;
                    Value2 |= (byte)((value & 0x0F) << 4);
                }
                else
                {
                    Value1 &= 0xF0;
                    Value1 |= (byte)(value & 0x0F);
                }
            }
        }

        public bool PageFlag
        {
            get
            {
                return ((IsThreeByteObject ? Value3 : Value2) & 0x80) != 0;
            }

            set
            {
                if (IsThreeByteObject)
                {
                    if (value)
                    {
                        Value3 |= 0x80;
                    }
                    else
                    {
                        Value3 &= 0x7F;
                    }
                }
                else
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
        }

        public int Command
        {
            get
            {
                return IsThreeByteObject ? (Value3 >> 4) & 7 : (Value2 >> 4) & 7;
            }

            set
            {
                if (IsThreeByteObject)
                {
                    Value3 &= 0x8F;
                    Value3 |= (byte)((value & 7) << 4);
                }
                else
                {
                    Value2 &= 0x8F;
                    Value2 |= (byte)((value & 7) << 4);
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

        public int ExtendedCommand
        {
            get
            {
                return IsThreeByteObject ? Value3 & 0x0F : -1;
            }

            set
            {
                // We have to save these because their storage locations can
                // change if the extended command makes us have a three byte
                // object.
                var pageFlag = PageFlag;
                var command = Command;
                var paraeter = Parameter;
                var y = Y;

                if (value == -1)
                {
                    IsThreeByteObject = false;
                    Y = y;
                    PageFlag = pageFlag;
                    Command = command;
                    Parameter = paraeter;
                    Value3 = 0;
                }
                else
                {
                    IsThreeByteObject = true;
                    Y = y;
                    PageFlag = pageFlag;
                    Command = command;
                    Parameter = paraeter;
                    Value3 &= 0xF0;
                    Value3 |= (byte)(value & 0x0F);
                }
            }
        }

        public AreaObjectCode AreaObjectCode
        {
            get
            {
                return (AreaObjectCode)(IsThreeByteObject
                    ? (ExtendedCommand << 0x0C) | 0xF00 | (Command << 4)
                    : Y >= 0x0C
                        ? Y == 0x0D
                            ? Command == 0
                                ? (int)AreaObjectCode.PageSkip
                                : (Y << 8) | (Command << 4) | (Parameter)
                            : Y == 0x0E
                                ? 0x0E00 | (Command >= 4 ? 0x40 : 0)
                                : (Y << 8) | (Command << 4)
                        : Command == 0
                            ? Parameter
                            : Command << 4);
            }
        }

        public bool IsEmpty
        {
            get
            {
                switch (AreaObjectCode)
                {
                case AreaObjectCode.Empty:
                case AreaObjectCode.Empty2:
                case AreaObjectCode.Empty3:
                case AreaObjectCode.Empty4:
                    return true;

                default:
                    return false;
                }
            }
        }

        private bool IsThreeByteObject
        {
            get
            {
                return IsThreeByteSpecifier(Value1);
            }

            set
            {
                if (value)
                {
                    Value1 |= 0x0F;
                }
                else
                {
                    Value1 &= 0xF0;
                }
            }
        }

        public static implicit operator AreaObjectCommand(
            Maseya.Smb1.AreaObjectCommand src)
        {
            return src.Y == 0x0F
                ? new AreaObjectCommand(
                    src.X,
                    0x0F,
                    src.PageFlag,
                    src.Command,
                    src.Parameter,
                    0)
                : new AreaObjectCommand(
                    src.X,
                    src.Y,
                    src.PageFlag,
                    src.Command,
                    src.Parameter,
                    -1);
        }

        public static bool operator ==(AreaObjectCommand left, AreaObjectCommand right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(AreaObjectCommand left, AreaObjectCommand right)
        {
            return !(left == right);
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

                    if (IsThreeByteSpecifier(en.Current))
                    {
                        var list = new List<byte>(GetBytes(3));
                        yield return new AreaObjectCommand(
                            list[0],
                            list[1],
                            list[2]);
                    }
                    else
                    {
                        var list = new List<byte>(GetBytes(2));
                        yield return new AreaObjectCommand(
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

        public static IEnumerable<byte> GetAreaByteData(
            IEnumerable<AreaObjectCommand> collection)
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

        public override bool Equals(object obj)
        {
            return obj is AreaObjectCommand other ? Equals(other) : false;
        }

        public bool Equals(AreaObjectCommand other)
        {
            return !Size.Equals(other.Size)
                ? false
                : !Value1.Equals(other.Value1) || !Value2.Equals(other.Value2)
                ? false
                : Size == 3
                ? Value3.Equals(other.Value3)
                : true;
        }

        public override int GetHashCode()
        {
            return (Value1) | (Value2 << 8) | (Value3 << 16);
        }

        public override string ToString()
        {
            return SR.GetString($"({X}, {Y}): {AreaObjectCode}");
        }

        private static bool IsThreeByteSpecifier(int coordinates)
        {
            return (coordinates & 0x0F) == 0x0F;
        }
    }
}
