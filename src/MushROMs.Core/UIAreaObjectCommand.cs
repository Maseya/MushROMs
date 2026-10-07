// <copyright file="UIAreaObjectCommand.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.Core;

using System;
using System.Drawing;

using Maseya.Smas.Smb1.AreaData.ObjectData;
using Maseya.Snes;

public struct UIAreaObjectCommand : IEquatable<UIAreaObjectCommand>
{
    private AreaObjectCommand _command;

    public UIAreaObjectCommand(AreaObjectCommand command, int page, int z = 0)
    {
        _command = command;
        Page = page;
        Z = z;
    }

    public AreaObjectCommand Command
    {
        readonly get
        {
            return _command;
        }

        set
        {
            _command = value;
        }
    }

    public int Page
    {
        get; set;
    }

    public int X
    {
        readonly get
        {
            return Command.X | (Page << 4);
        }

        set
        {
            Page = value >> 4;
            _command.X = value & 0x0F;
        }
    }

    public int Y
    {
        readonly get
        {
            return _command.Y;
        }

        set
        {
            _command.Y = value;
        }
    }

    public readonly Rectangle SelectionRectangle
    {
        get
        {
            return new Rectangle(X << 4, (Y + 2) << 4, 0x10, 0x10);
        }
    }

    public int Z
    {
        get; set;
    }

    public readonly string HexString
    {
        get
        {
            return $"{Page:X2}:{Command.HexString}";
        }
    }

    public static bool operator ==(UIAreaObjectCommand left, UIAreaObjectCommand right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(UIAreaObjectCommand left, UIAreaObjectCommand right)
    {
        return !(left == right);
    }

    public static bool TryGetCommand(string text, out UIAreaObjectCommand command)
    {
        var result = true;

        command = default;
        var tokens1 = text.Split(':');
        if (tokens1.Length == 0)
        {
            return false;
        }

        result &= MathHelper.TryGetHexByte(tokens1[0], out var screen);
        if (screen >= 0x20)
        {
            result = false;
            screen = 0x1F;
        }

        command.Page = screen;

        if (tokens1.Length == 1)
        {
            return false;
        }

        result &= tokens1.Length == 2;
        result &= AreaObjectCommand.TryGetCommand(tokens1[1], out var command2);
        result &= command2.ObjectType != ObjectType.PageSkip;
        command.Command = command2;
        return result;
    }

    public readonly bool Equals(UIAreaObjectCommand other)
    {
        return Command.Equals(other.Command)
            && Page.Equals(other.Page)
            && Z.Equals(other.Z);
    }

    public override readonly bool Equals(object? obj)
    {
        return obj is UIAreaObjectCommand other && Equals(other);
    }

    public override readonly int GetHashCode()
    {
        return HashCode.Combine(Command, Page, Z);
    }

    public override readonly string ToString()
    {
        return $"{X}, {Y}, {Z}, {Command.BaseName}";
    }
}
