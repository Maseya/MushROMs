// <copyright file="AreaObjectCommand.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Smas.Smb1.AreaData.ObjectData;

using System;
using System.Collections.ObjectModel;

using HeaderData;

using Maseya.Smas.Smb1;
using Maseya.Snes;

public struct AreaObjectCommand : IEquatable<AreaObjectCommand>
{
    /// <summary>
    /// The object command to read that defined the end of the area object data.
    /// </summary>
    public const byte TerminationCode = 0xFD;

    public static readonly ReadOnlyCollection<ObjectType> ValidCodes = new([
            ObjectType.QuestionBlockPowerup,
            ObjectType.QuestionBlockCoin,
            ObjectType.HiddenBlockCoin,
            ObjectType.HiddenBlock1UP,
            ObjectType.BrickPowerup,
            ObjectType.BrickBeanstalk,
            ObjectType.BrickStar,
            ObjectType.Brick10Coins,
            ObjectType.Brick1UP,
            ObjectType.SidewaysPipe,
            ObjectType.UsedBlock,
            ObjectType.Spring,
            ObjectType.JPipe,
            ObjectType.FlagPole,
            ObjectType.Nothing1,
            ObjectType.Nothing2,
            ObjectType.AreaSpecificPlatform,
            ObjectType.HorizontalBricks,
            ObjectType.HorizontalBlocks,
            ObjectType.HorizontalCoins,
            ObjectType.VerticalBricks,
            ObjectType.VerticalBlocks,
            ObjectType.UnenterablePipe,
            ObjectType.EnterablePipe,
            ObjectType.Hole,
            ObjectType.BalanceHorizontalRope,
            ObjectType.BridgeV7,
            ObjectType.BridgeV8,
            ObjectType.BridgeV10,
            ObjectType.HoleWithWaterOrLava,
            ObjectType.HorizontalQuestionBlocksV3,
            ObjectType.HorizontalQuestionBlocksV7,
            ObjectType.AltJPipe,
            ObjectType.AltFlagPole,
            ObjectType.BowserAxe,
            ObjectType.RopeForAxe,
            ObjectType.BowserBridge,
            ObjectType.ScrollStopWarpZone,
            ObjectType.ScrollStop,
            ObjectType.AltScrollStop,
            ObjectType.JumpingCheepCheepGenerator,
            ObjectType.BulletBillGenerator,
            ObjectType.StopGenerator,
            ObjectType.LoopCommand,
            ObjectType.TerrainAndBackgroundSceneryChange,
            ObjectType.ForegroundSceneryChange,
            ObjectType.RopeForLift,
            ObjectType.PulleyRope,
            ObjectType.EmptyTile,
            ObjectType.Castle,
            ObjectType.CastleCeilingCap,
            ObjectType.Staircase,
            ObjectType.CastleStairs,
            ObjectType.CastleRectangularCeilingTiles,
            ObjectType.CastleFloorRightEdge,
            ObjectType.CastleFloorLeftEdge,
            ObjectType.CastleFloorLeftWall,
            ObjectType.CastleFloorRightWall,
            ObjectType.VerticalSeaBlocks,
            ObjectType.ExtendableJPipe,
            ObjectType.VerticalBalls,
        ]);

    public AreaObjectCommand(byte value1, byte value2, byte value3 = 0)
    {
        Value1 = value1;
        Value2 = value2;
        Value3 = (byte)(((value1 & 0x0F) == 0x0F) ? value3 : 0);
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
    public readonly int Size
    {
        get
        {
            return IsThreeByteCommand ? 3 : 2;
        }
    }

    public int X
    {
        readonly get
        {
            return Value1 >> 4;
        }

        set
        {
            Value1 &= 0x0F;
            Value1 |= (byte)(value << 4);
        }
    }

    public readonly bool HasYCoord
    {
        get
        {
            var y = Value1 & 0x0F;
            return y is < 0x0C or 0x0F;
        }
    }

    public int Y
    {
        readonly get
        {
            return IsThreeByteCommand ? Value2 >> 4 : Value1 & 0x0F;
        }

        set
        {
            if (IsThreeByteCommand)
            {
                Value2 &= 0x0F;
                Value2 |= (byte)(value << 4);
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
        readonly get
        {
            return ((IsThreeByteCommand ? Value3 : Value2) & 0x80) != 0;
        }

        set
        {
            var mask = (byte)(value ? 0x80 : 0);
            if (IsThreeByteCommand)
            {
                Value3 &= 0x7F;
                Value3 |= mask;
            }
            else
            {
                Value2 &= 0x7F;
                Value2 |= mask;
            }
        }
    }

    public int PrimaryCommand
    {
        readonly get
        {
            return (IsThreeByteCommand ? Value3 : Value2) & 0x7F;
        }

        set
        {
            if (IsThreeByteCommand)
            {
                Value3 &= 0x80;
                Value3 |= (byte)(value & 0x7F);
            }
            else
            {
                Value2 &= 0x80;
                Value2 |= (byte)(value & 0x7F);
            }
        }
    }

    public int SecondaryCommand
    {
        readonly get
        {
            return (PrimaryCommand >> 4) & 7;
        }

        set
        {
            PrimaryCommand &= 0x8F;
            PrimaryCommand |= (byte)((value & 7) << 4);
        }
    }

    public int Parameter
    {
        readonly get
        {
            return Value2 & 0x0F;
        }

        set
        {
            Value2 &= 0xF0;
            Value2 |= (byte)(value & 0x0F);
        }
    }

    public readonly ObjectType ObjectType
    {
        get
        {
            return (Value1 & 0x0F) switch
            {
                0x0F => (ObjectType)(0xF00 | PrimaryCommand),
                0x0C => (ObjectType)(0xC00 | (SecondaryCommand << 4)),
                0x0D => (SecondaryCommand & ~3) == 0
                    ? ObjectType.PageSkip
                    : (ObjectType)(0xD00 | (SecondaryCommand << 4) | Parameter),
                0x0E => (ObjectType)(0xE00 | ((SecondaryCommand & 4) << 4)),
                _ => SecondaryCommand switch
                {
                    0 => (ObjectType)Parameter,
                    7 => Parameter < 8
                        ? ObjectType.UnenterablePipe
                        : ObjectType.EnterablePipe,
                    _ => (ObjectType)(SecondaryCommand << 4),
                },
            };
        }
    }

    public readonly bool IsExtendableObject
    {
        get
        {
            return ObjectType.IsExtendableObject();
        }
    }

    public readonly bool IsTerrainAndBackgroundChange
    {
        get
        {
            return ObjectType == ObjectType.TerrainAndBackgroundSceneryChange;
        }
    }

    public readonly bool IsForegroundChange
    {
        get
        {
            return ObjectType == ObjectType.ForegroundSceneryChange;
        }
    }

    public int Length
    {
        readonly get
        {
            return ObjectType switch
            {
                ObjectType.PageSkip => Value2 & 0x1F,
                ObjectType.EnterablePipe or
                ObjectType.UnenterablePipe => Parameter & 7,
                _ => IsExtendableObject ? Parameter : 0,
            };
        }

        set
        {
            switch (ObjectType)
            {
            case ObjectType.PageSkip:
                Value2 &= 0xE0;
                Value2 |= (byte)(value & 0x1F);
                break;

            case ObjectType.EnterablePipe:
            case ObjectType.UnenterablePipe:
                Parameter &= 0xF8;
                Parameter |= (byte)(value & 7);
                break;

            default:
                if (IsExtendableObject)
                {
                    Parameter &= 0xF0;
                    Parameter |= (byte)(value & 0x0F);
                }

                break;
            }
        }
    }

    public ForegroundScenery ForegroundScenery
    {
        readonly get
        {
            return (ForegroundScenery)(IsForegroundChange
                ? Parameter & 7
                : 0);
        }

        set
        {
            if (IsForegroundChange)
            {
                Parameter &= 0xF8;
                Parameter |= (byte)((int)value & 7);
            }
        }
    }

    public TerrainMode TerrainMode
    {
        readonly get
        {
            return (TerrainMode)(IsTerrainAndBackgroundChange
                ? PrimaryCommand & 0x0F
                : 0);
        }

        set
        {
            if (IsTerrainAndBackgroundChange)
            {
                PrimaryCommand &= 0xF0;
                PrimaryCommand |= (int)value & 0x0F;
            }
        }
    }

    public BackgroundScenery BackgroundScenery
    {
        readonly get
        {
            return (BackgroundScenery)(ObjectType == ObjectType.TerrainAndBackgroundSceneryChange
                ? (PrimaryCommand >> 4) & 3
                : 0);
        }

        set
        {
            if (ObjectType == ObjectType.TerrainAndBackgroundSceneryChange)
            {
                PrimaryCommand &= 0xCF;
                PrimaryCommand |= ((int)value & 3) << 4;
            }
        }
    }

    public readonly string BaseName
    {
        get
        {
            return ObjectType.BaseName();
        }
    }

    public readonly bool IsThreeByteCommand
    {
        get
        {
            return IsThreeByteSpecifier(Value1);
        }
    }

    public readonly bool IsValid
    {
        get
        {
            return Value1 != TerminationCode
                && (IsThreeByteCommand || Value3 == 0);
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

    public static bool operator ==(AreaObjectCommand left, AreaObjectCommand right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(AreaObjectCommand left, AreaObjectCommand right)
    {
        return !(left == right);
    }

    public static bool IsThreeByteSpecifier(int coordinates)
    {
        return (coordinates & 0x0F) == 0x0F;
    }

    public static bool TryGetCommand(string text, out AreaObjectCommand command)
    {
        var result = true;
        var tokens = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        result &= tokens.Length is 3 or 2;

        var bytes = new byte[3];
        for (var i = 0; i < Math.Min(tokens.Length, bytes.Length); i++)
        {
            result &= MathHelper.TryGetHexByte(tokens[i], out bytes[i]);
        }

        command = new AreaObjectCommand(bytes[0], bytes[1], bytes[2]);
        return result
            && command.IsValid
            && command.IsThreeByteCommand ^ tokens.Length != 3;
    }

    public readonly string FullName(AreaPlatformType areaPlatformType)
    {
        var length = Parameter + 1;

        switch (ObjectType)
        {
        case ObjectType.QuestionBlockPowerup:
            return "Question Block (Powerup)";

        case ObjectType.QuestionBlockCoin:
            return "Question Block (Coin)";

        case ObjectType.HiddenBlockCoin:
            return "Hidden Block (Coin)";

        case ObjectType.HiddenBlock1UP:
            return "Hidden Block (1UP)";

        case ObjectType.BrickPowerup:
            return "Brick (Powerup)";

        case ObjectType.BrickBeanstalk:
            return "Brick (Beanstalk)";

        case ObjectType.BrickStar:
            return "Brick (Star)";

        case ObjectType.Brick10Coins:
            return "Brick (10 Coins)";

        case ObjectType.Brick1UP:
            return "Brick (1UP)";

        case ObjectType.SidewaysPipe:
            return "Sideways Pipe Cap";

        case ObjectType.UsedBlock:
            return "Used Block";

        case ObjectType.Spring:
            return "Spring Board";

        case ObjectType.JPipe:
        case ObjectType.AltJPipe:
            return "J-Pipe";

        case ObjectType.FlagPole:
        case ObjectType.AltFlagPole:
            return "Flag Pole";

        case ObjectType.Nothing1:
        case ObjectType.Nothing2:
            return "Nothing";

        case ObjectType.AreaSpecificPlatform:
            switch (areaPlatformType)
            {
            case AreaPlatformType.Trees:
                return $"Tree Top Platform (Width={length})";

            case AreaPlatformType.Mushrooms:
                return $"Mushroom Platform (Width={length})";

            case AreaPlatformType.BulletBillTurrets:
                return $"Bullet Bill Shooter (Height={length})";

            case AreaPlatformType.CloudGround:
                return $"Cloud Ground (Width={length})";

            default:
                break;
            }

            break;

        case ObjectType.HorizontalBricks:
            return $"Horizontal Bricks (Width={length})";

        case ObjectType.HorizontalBlocks:
            return $"Horizontal Blocks (Width={length})";

        case ObjectType.HorizontalCoins:
            return $"Horizontal Coins (Width={length})";

        case ObjectType.VerticalBricks:
            return $"Vertical Bricks (Height={length})";

        case ObjectType.VerticalBlocks:
            return $"Vertical Blocks (Height={length})";

        case ObjectType.UnenterablePipe:
            return $"Unenterable Pipe (Height={length})";

        case ObjectType.EnterablePipe:
            return $"Enterable Pipe (Height={length})";

        case ObjectType.Hole:
            return $"Hole (Width={length})";

        case ObjectType.BalanceHorizontalRope:
            return $"Pulley Platforms (Width={length})";

        case ObjectType.BridgeV7:
            return $"Rope Bridge (Y=7, Width={length})";

        case ObjectType.BridgeV8:
            return $"Rope Bridge (Y=8, Width={length})";

        case ObjectType.BridgeV10:
            return $"Rope Bridge (Y=10, Width={length})";

        case ObjectType.HoleWithWaterOrLava:
            return $"Hole with water or lava (Width={length})";

        case ObjectType.HorizontalQuestionBlocksV3:
            return $"Row of Coin Blocks (Y=3, Width={length})";

        case ObjectType.HorizontalQuestionBlocksV7:
            return $"Row of Coin Blocks (Y=7, Width={length})";

        case ObjectType.PageSkip:
            return $"Skip to screen 0x{PrimaryCommand:X2}";

        case ObjectType.BowserAxe:
            return $"Bowser Axe";

        case ObjectType.BowserBridge:
            return $"Bowser Bridge";

        case ObjectType.ScrollStopWarpZone:
            return $"Scroll Stop (Warp Zone)";

        case ObjectType.ScrollStop:
        case ObjectType.AltScrollStop:
            return $"Scroll Stop";

        case ObjectType.JumpingCheepCheepGenerator:
            return $"Generator: Red flying cheep-cheeps";

        case ObjectType.BulletBillGenerator:
            return $"Generator: Bullet Bills";

        case ObjectType.StopGenerator:
            return $"Stop Generator (also stops Lakitus)";

        case ObjectType.LoopCommand:
            return $"Screen Loop Command";

        case ObjectType.TerrainAndBackgroundSceneryChange:
            return "Brick and scenery change";

        case ObjectType.ForegroundSceneryChange:
            return "Foreground Change";

        case ObjectType.RopeForLift:
            return "Rope for platform lifts";

        case ObjectType.PulleyRope:
            return $"Rope for pulley platforms (Height={length})";

        case ObjectType.EmptyTile:
            return "Empty tile";

        case ObjectType.Castle:
            return "Castle";

        case ObjectType.CastleCeilingCap:
            return "Castle Object: Ceiling Cap Tile";

        case ObjectType.Staircase:
            return $"Staircase (Width={length})";

        case ObjectType.CastleStairs:
            return "Castle Object: Descending Stairs";

        case ObjectType.CastleRectangularCeilingTiles:
            return "Castle Object: Rectangular Ceiling Tiles";

        case ObjectType.CastleFloorRightEdge:
            return "Castle Object: Right-Facing Wall To Floor";

        case ObjectType.CastleFloorLeftEdge:
            return "Castle Object: Left-Facing Wall To Floor";

        case ObjectType.CastleFloorLeftWall:
            return "Castle Object: Left-Facing Wall";

        case ObjectType.CastleFloorRightWall:
            return "Castle Object: Right-Facing Wall";

        case ObjectType.VerticalSeaBlocks:
            return $"Vertical Sea Blocks (Height={length})";

        case ObjectType.ExtendableJPipe:
            return $"Extendable J-Pipe (Height={length})";

        case ObjectType.VerticalBalls:
            return $"Vertical Climbing Balls (Height={length})";
        }

        return $"Unknown command: {this}";
    }

    public override readonly bool Equals(object? obj)
    {
        return obj is AreaObjectCommand other && Equals(other);
    }

    public readonly bool Equals(AreaObjectCommand other)
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
        return $"({X}, {Y}): {BaseName}";
    }
}
