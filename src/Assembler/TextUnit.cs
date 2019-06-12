// <copyright file="TextUnit.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Assembler
{
    using System;
    using System.Diagnostics;

    [DebuggerDisplay("{Value}: {Index}")]
    public struct TextUnit : IEquatable<TextUnit>, IComparable<TextUnit>
    {
        public TextUnit(
            char value,
            int index,
            int column,
            int line,
            int file)
        {
            Value = value;
            Index = index;
            Line = line;
            Column = column;
            File = file;
        }

        public int File
        {
            get;
        }

        public int Line
        {
            get;
        }

        public int Column
        {
            get;
        }

        public int Index
        {
            get;
        }

        public char Value
        {
            get;
        }

        public static implicit operator char(TextUnit unit)
        {
            return unit.Value;
        }

        public static bool operator ==(TextUnit left, TextUnit right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TextUnit left, TextUnit right)
        {
            return !(left == right);
        }

        public bool Equals(TextUnit other)
        {
            return Value.Equals(other.Value)
                && Index.Equals(other.Index)
                && Line.Equals(other.Line)
                && File.Equals(other.File);
        }

        public override bool Equals(object obj)
        {
            return obj is TextUnit other
                ? Equals(other)
                : obj is char x
                ? Value.Equals(x)
                : false;
        }

        public int CompareTo(TextUnit other)
        {
            return File != other.File
                ? File - other.File
                : Line != other.Line
                ? Line - other.Line
                : Column - other.Column;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
