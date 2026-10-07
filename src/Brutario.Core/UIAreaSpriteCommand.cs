// <copyright file="UIAreaSpriteCommand.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Brutario.Core;

using System;
using System.Drawing;

using Maseya.Smas.Smb1.AreaData.ObjectData;
using Maseya.Smas.Smb1.AreaData.SpriteData;
using Maseya.Snes;

public struct UIAreaSpriteCommand : IEquatable<UIAreaSpriteCommand>
{
    private AreaSpriteCommand _command;

    public UIAreaSpriteCommand(AreaSpriteCommand command, int page, int z = 0)
    {
        _command = command;
        Page = page;
        Z = z;
    }

    public AreaSpriteCommand Command
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
            return (Command.X | (Page << 4)) + Offset(Command.Code).Width;
        }

        set
        {
            value -= Offset(Command.Code).Width;
            Page = value >> 4;
            _command.X = value & 0x0F;
        }
    }

    public int Z
    {
        get; set;
    }

    public int Y
    {
        readonly get
        {
            return _command.Y + Offset(Command.Code).Height;
        }

        set
        {
            _command.Y = value - Offset(Command.Code).Height;
        }
    }

    public readonly Rectangle SelectionRectangle
    {
        get
        {
            return new Rectangle(X << 4, Y << 4, 0x10, 0x10);
        }
    }

    public readonly string HexString
    {
        get
        {
            return $"{Page:X2}:{Command.HexString}";
        }
    }

    public static bool operator ==(
        UIAreaSpriteCommand left,
        UIAreaSpriteCommand right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(
        UIAreaSpriteCommand left,
        UIAreaSpriteCommand right)
    {
        return !(left == right);
    }

    public static bool TryGetCommand(string text, out UIAreaSpriteCommand command)
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
        result &= AreaSpriteCommand.TryGetCommand(tokens1[1], out var command2);
        result &= command2.Code != AreaSpriteCode.ScreenJump;
        command.Command = command2;
        return result;
    }

    public readonly bool Equals(UIAreaSpriteCommand other)
    {
        return Command.Equals(other.Command)
            && Page.Equals(other.Page)
            && Z.Equals(other.Z);
    }

    public override readonly bool Equals(object? obj)
    {
        return obj is UIAreaSpriteCommand other && Equals(other);
    }

    public override readonly int GetHashCode()
    {
        return HashCode.Combine(Command, Page, Z);
    }

    public override readonly string ToString()
    {
        return $"{X}, {Y}, {Z}, {Command.FullName}";
    }

    public static Size Offset(AreaSpriteCode code)
    {
        return code switch
        {
            AreaSpriteCode.AreaPointer => new Size(0, 1),
            AreaSpriteCode.GreenKoopaTroopa => new Size(0, 1),
            AreaSpriteCode.RedKoopaTroopa => new Size(0, 1),
            AreaSpriteCode.BuzzyBeetle => new Size(0, 1),
            AreaSpriteCode.RedKoopaTroopaPatrol => new Size(0, 1),
            AreaSpriteCode.GreenKoopaTroopaStopped => new Size(0, 1),
            AreaSpriteCode.HammerBros => new Size(0, 1),
            AreaSpriteCode.Goomba => new Size(0, 1),
            AreaSpriteCode.Blooper => new Size(0, 1),
            AreaSpriteCode.BulletBill => new Size(0, 1),
            AreaSpriteCode.YellowKoopaParatroopaStopped => new Size(0, 1),
            AreaSpriteCode.GreenCheepCheep => new Size(0, 1),
            AreaSpriteCode.RedCheepCheep => new Size(0, 1),
            AreaSpriteCode.Podoboo => new Size(0, 1),
            AreaSpriteCode.PiranhaPlant => new Size(0, 0),
            AreaSpriteCode.GreenKoopaParatroopaLeaping => new Size(0, 1),
            AreaSpriteCode.RedKoopaParatroopa => new Size(0, 1),
            AreaSpriteCode.GreenKoopaParatroopaFlying => new Size(0, 1),
            AreaSpriteCode.Lakitu => new Size(0, 1),
            AreaSpriteCode.Spiny => new Size(0, 1),
            AreaSpriteCode.RedFlyingCheepCheep => new Size(0, 1),
            AreaSpriteCode.BowsersFire => new Size(0, 1),
            AreaSpriteCode.Fireworks => new Size(0, 1),
            AreaSpriteCode.BulletBillOrCheepCheeps => new Size(0, 1),
            AreaSpriteCode.FireBarClockwise => new Size(0, 0),
            AreaSpriteCode.FastFireBarClockwise => new Size(0, 0),
            AreaSpriteCode.FireBarCounterClockwise => new Size(0, 0),
            AreaSpriteCode.FastFireBarCounterClockwise => new Size(0, 0),
            AreaSpriteCode.LongFireBarClockwise => new Size(0, 0),
            AreaSpriteCode.BalanceRopeLift => new Size(0, 0),
            AreaSpriteCode.LiftDownThenUp => new Size(0, 0),
            AreaSpriteCode.LiftUp => new Size(1, 0),
            AreaSpriteCode.LiftDown => new Size(1, 0),
            AreaSpriteCode.LiftLeftThenRight => new Size(0, 0),
            AreaSpriteCode.LiftFalling => new Size(0, 0),
            AreaSpriteCode.LiftRight => new Size(0, 0),
            AreaSpriteCode.ShortLiftUp => new Size(1, 0),
            AreaSpriteCode.ShortLiftDown => new Size(1, 0),
            AreaSpriteCode.Bowser => new Size(0, 1),
            AreaSpriteCode.WarpZoneCommand => new Size(0, 1),
            AreaSpriteCode.ToadOrPrincess => new Size(0, 0),
            AreaSpriteCode.TwoGoombasY10 => new Size(-3, 1),
            AreaSpriteCode.ThreeGoombasY10 => new Size(-3, 1),
            AreaSpriteCode.TwoGoombasY6 => new Size(-3, 1),
            AreaSpriteCode.ThreeGoombasY6 => new Size(-3, 1),
            AreaSpriteCode.TwoGreenKoopasY10 => new Size(-3, 1),
            AreaSpriteCode.ThreeGreenKoopasY10 => new Size(-3, 1),
            AreaSpriteCode.TwoGreenKoopasY6 => new Size(-3, 1),
            AreaSpriteCode.ThreeGreenKoopasY6 => new Size(-3, 1),
            AreaSpriteCode.ScreenJump => new Size(0, 0),
            _ => new Size(0, 0),
        };
    }
}
