// <copyright file="AreaSpriteCommand.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Smas.Smb1.AreaData.SpriteData;

using System;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;

public struct AreaSpriteCommand : IEquatable<AreaSpriteCommand>
{
    public const byte TerminationCode = 0xFF;

    public static readonly ReadOnlyCollection<AreaSpriteCode> ValidCodes = new([
            AreaSpriteCode.GreenKoopaTroopa,
            AreaSpriteCode.RedKoopaTroopa,
            AreaSpriteCode.BuzzyBeetle,
            AreaSpriteCode.RedKoopaTroopaPatrol,
            AreaSpriteCode.GreenKoopaTroopaStopped,
            AreaSpriteCode.HammerBros,
            AreaSpriteCode.Goomba,
            AreaSpriteCode.Blooper,
            AreaSpriteCode.BulletBill,
            AreaSpriteCode.YellowKoopaParatroopaStopped,
            AreaSpriteCode.GreenCheepCheep,
            AreaSpriteCode.RedCheepCheep,
            AreaSpriteCode.Podoboo,
            AreaSpriteCode.PiranhaPlant,
            AreaSpriteCode.GreenKoopaParatroopaLeaping,
            AreaSpriteCode.RedKoopaParatroopa,
            AreaSpriteCode.GreenKoopaParatroopaFlying,
            AreaSpriteCode.Lakitu,
            AreaSpriteCode.Spiny,
            AreaSpriteCode.RedFlyingCheepCheep,
            AreaSpriteCode.BowsersFire,
            AreaSpriteCode.Fireworks,
            AreaSpriteCode.BulletBillOrCheepCheeps,
            AreaSpriteCode.FireBarClockwise,
            AreaSpriteCode.FastFireBarClockwise,
            AreaSpriteCode.FireBarCounterClockwise,
            AreaSpriteCode.FastFireBarCounterClockwise,
            AreaSpriteCode.LongFireBarClockwise,
            AreaSpriteCode.BalanceRopeLift,
            AreaSpriteCode.LiftDownThenUp,
            AreaSpriteCode.LiftUp,
            AreaSpriteCode.LiftDown,
            AreaSpriteCode.LiftLeftThenRight,
            AreaSpriteCode.LiftFalling,
            AreaSpriteCode.LiftRight,
            AreaSpriteCode.ShortLiftUp,
            AreaSpriteCode.ShortLiftDown,
            AreaSpriteCode.Bowser,
            AreaSpriteCode.WarpZoneCommand,
            AreaSpriteCode.ToadOrPrincess,
            AreaSpriteCode.TwoGoombasY10,
            AreaSpriteCode.ThreeGoombasY10,
            AreaSpriteCode.TwoGoombasY6,
            AreaSpriteCode.ThreeGoombasY6,
            AreaSpriteCode.TwoGreenKoopasY10,
            AreaSpriteCode.ThreeGreenKoopasY10,
            AreaSpriteCode.TwoGreenKoopasY6,
            AreaSpriteCode.ThreeGreenKoopasY6,
            AreaSpriteCode.AreaPointer,
        ]);

    public AreaSpriteCommand(byte value1, byte value2, byte value3 = 0)
    {
        Value1 = value1;
        Value2 = value2;
        Value3 = value3;
    }

    /// <summary>
    /// Gets or sets the command value of this <see cref="AreaSpriteCommand"/>.
    /// </summary>
    public AreaSpriteCode Code
    {
        readonly get
        {
            return IsThreeByteCommand
                ? AreaSpriteCode.AreaPointer
                : Y == 0x0F
                ? AreaSpriteCode.ScreenJump
                : (AreaSpriteCode)(Value2 & 0x3F);
        }

        set
        {
            Value2 &= 0xC0;
            Value2 |= (byte)((int)value & 0x3F);
        }
    }

    public readonly bool IsValid
    {
        get
        {
            return Value1 != 0xFF && (IsThreeByteCommand || Value3 == 0);
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="AreaSpriteCommand"/>
    /// only spawns after the hard world flag has been set.
    /// </summary>
    public bool HardWorldFlag
    {
        readonly get
        {
            return !IsThreeByteCommand && (Value2 & 0x40) != 0;
        }

        set
        {
            if (IsThreeByteCommand)
            {
                return;
            }

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
    /// Gets or sets a value indicating whether this <see cref="AreaSpriteCommand"/>
    /// starts on the next page.
    /// </summary>
    public bool ScreenFlag
    {
        readonly get
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
    public readonly int Size
    {
        get
        {
            return IsThreeByteCommand ? 3 : 2;
        }
    }

    /// <summary>
    /// Gets or sets the first value of this <see cref="AreaSpriteCommand"/>.
    /// </summary>
    public byte Value1
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the second value of this <see cref="AreaSpriteCommand"/>.
    /// </summary>
    public byte Value2
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the third value of this <see cref="AreaSpriteCommand"/>.
    /// </summary>
    public byte Value3
    {
        get;
        set;
    }

    public readonly int BaseCommand
    {
        get
        {
            return Value2 & 0x7F;
        }
    }

    /// <summary>
    /// Gets or sets the X-coordinate of this <see cref="AreaSpriteCommand"/>. The
    /// coordinate is relative to the page the object is in.
    /// </summary>
    public int X
    {
        readonly get
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
    /// Gets or sets the Y-coordinate of this <see cref="AreaSpriteCommand"/>.
    /// </summary>
    public int Y
    {
        readonly get
        {
            return Value1 & 0x0F;
        }

        set
        {
            Value1 &= 0xF0;
            Value1 |= (byte)(value & 0x0F);
        }
    }

    public int AreaNumber
    {
        readonly get
        {
            return IsThreeByteCommand
                ? Value2 & 0x7F
                : 0;
        }

        set
        {
            if (IsThreeByteCommand)
            {
                Value2 &= 0x80;
                Value2 |= (byte)(value & 0x7F);
            }
        }
    }

    public int WorldLimit
    {
        readonly get
        {
            return IsThreeByteCommand
                ? Value3 >> 5
                : 0;
        }

        set
        {
            if (IsThreeByteCommand)
            {
                Value3 &= 0x0F;
                Value3 |= (byte)((value & 7) << 4);
            }
        }
    }

    public int AreaPointerScreen
    {
        readonly get
        {
            return IsThreeByteCommand
                ? Value3 & 0x1F
                : 0;
        }

        set
        {
            if (IsThreeByteCommand)
            {
                Value3 &= 0xE0;
                Value3 |= (byte)(value & 0x1F);
            }
        }
    }

    public readonly bool IsThreeByteCommand
    {
        get
        {
            return IsThreeByteSpecifier(Value1);
        }
    }

    public readonly string HexString
    {
        get
        {
            return $"{Value1:X2} {Value2:X2}"
                + (IsThreeByteCommand ? $" {Value3:X2}" : String.Empty);
        }
    }

    public readonly string FullName
    {
        get
        {
            return Code switch
            {
                AreaSpriteCode.AreaPointer => $"Transition: Area Number={AreaNumber:X2}, Page={AreaPointerScreen + 1} (For world {1 + WorldLimit})",
                AreaSpriteCode.GreenKoopaTroopa => "Koopa Troopa (Green)",
                AreaSpriteCode.RedKoopaTroopa => "Koopa Troopa (Red; Walks off floors)",
                AreaSpriteCode.BuzzyBeetle => "Buzzy Beetle",
                AreaSpriteCode.RedKoopaTroopaPatrol => "Koopa Troopa (Red; Stays on floors)",
                AreaSpriteCode.GreenKoopaTroopaStopped => "Koopa Troopa (Green; Walks in place)",
                AreaSpriteCode.HammerBros => "Hammer Bros.",
                AreaSpriteCode.Goomba => "Goomba",
                AreaSpriteCode.Blooper => "Squid",
                AreaSpriteCode.BulletBill => "Bullet Bill",
                AreaSpriteCode.YellowKoopaParatroopaStopped => "Yellow Koopa Paratroopa (Flies in place)",
                AreaSpriteCode.GreenCheepCheep => "Green Cheep-Cheep",
                AreaSpriteCode.RedCheepCheep => "Red Cheep-Cheep",
                AreaSpriteCode.Podoboo => "Podoboo",
                AreaSpriteCode.PiranhaPlant => "Piranha Plant",
                AreaSpriteCode.GreenKoopaParatroopaLeaping => "Green Koopa Paratroopa (Leaping)",
                AreaSpriteCode.RedKoopaParatroopa => "Red Koopa Paratroopa (Flies vertically)",
                AreaSpriteCode.GreenKoopaParatroopaFlying => "Green Koopa Paratroopa (Flies horizontally)",
                AreaSpriteCode.Lakitu => "Lakitu",
                AreaSpriteCode.Spiny => "Spiny (undefined walk speed)",
                AreaSpriteCode.RedFlyingCheepCheep => "Red Flying Cheep-Cheep",
                AreaSpriteCode.BowsersFire => "Bowser's Fire (generator)",
                AreaSpriteCode.Fireworks => "Single Firework",
                AreaSpriteCode.BulletBillOrCheepCheeps => "Generator (Bullet Bill or Cheep-Cheeps)",
                AreaSpriteCode.FireBarClockwise => "Fire Bar (Clockwise)",
                AreaSpriteCode.FastFireBarClockwise => "Fire Bar (Fast; Clockwise)",
                AreaSpriteCode.FireBarCounterClockwise => "Fire Bar (Counter-Clockwise)",
                AreaSpriteCode.FastFireBarCounterClockwise => "Fire Bar (Fast; Counter-Clockwise)",
                AreaSpriteCode.LongFireBarClockwise => "Long Fire Bar (Fast; Clockwise)",
                AreaSpriteCode.BalanceRopeLift => "Rope for Lift Balance",
                AreaSpriteCode.LiftDownThenUp => "Lift (Down, then up)",
                AreaSpriteCode.LiftUp => "Lift (Up)",
                AreaSpriteCode.LiftDown => "Lift (Down)",
                AreaSpriteCode.LiftLeftThenRight => "Lift (Left, then right)",
                AreaSpriteCode.LiftFalling => "Lift (Falling)",
                AreaSpriteCode.LiftRight => "Lift (Right)",
                AreaSpriteCode.ShortLiftUp => "Short Lift (Up)",
                AreaSpriteCode.ShortLiftDown => "Short Lift (Down)",
                AreaSpriteCode.Bowser => "Bowser: King of the Koopa",
                AreaSpriteCode.WarpZoneCommand => "Command: Load Warp Zone",
                AreaSpriteCode.ToadOrPrincess => "Toad or Princess",
                AreaSpriteCode.TwoGoombasY10 => "Two Goombas (Y=10)",
                AreaSpriteCode.ThreeGoombasY10 => "Three Goombas (Y=10)",
                AreaSpriteCode.TwoGoombasY6 => "Two Goombas (Y=6)",
                AreaSpriteCode.ThreeGoombasY6 => "Three Goombas (Y=6)",
                AreaSpriteCode.TwoGreenKoopasY10 => "Two Green Koopa Troopas (Y=10)",
                AreaSpriteCode.ThreeGreenKoopasY10 => "Three Green Koopa Troopas (Y=10)",
                AreaSpriteCode.TwoGreenKoopasY6 => "Two Green Koopa Troopas (Y=6)",
                AreaSpriteCode.ThreeGreenKoopasY6 => "Three Green Koopa Troopas (Y=6)",
                AreaSpriteCode.ScreenJump => $"Page Skip: {BaseCommand & 0x1F}",
                _ => $"Unknown command: {this}",
            };
        }
    }

    public static bool operator ==(AreaSpriteCommand left, AreaSpriteCommand right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(AreaSpriteCommand left, AreaSpriteCommand right)
    {
        return !(left == right);
    }

    public static bool IsThreeByteSpecifier(int coordinates)
    {
        return (coordinates & 0x0F) == 0x0E;
    }

    public override readonly bool Equals(object? obj)
    {
        return obj is AreaSpriteCommand other && Equals(other);
    }

    public readonly bool Equals(AreaSpriteCommand other)
    {
        return Value1.Equals(other.Value1) && Value2.Equals(other.Value2)
            && (!IsThreeByteCommand || Value3.Equals(other.Value3));
    }

    public override readonly int GetHashCode()
    {
        return IsThreeByteCommand
            ? HashCode.Combine(Value1, Value2, Value3)
            : HashCode.Combine(Value1, Value2);
    }

    public override readonly string ToString()
    {
        return IsThreeByteCommand
            ? $"({X}, {Y}): Area pointer to 0x{AreaNumber:X2}"
            : $"({X}, {Y}): {Code}";
    }
}
