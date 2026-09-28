// <copyright file="ExtensionMethods.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Smas.Smb1;

using System.Collections.Generic;
using System.Collections.Immutable;

using AreaData.ObjectData;
using AreaData.SpriteData;

public static class ExtensionMethods
{
    private static readonly ImmutableHashSet<ObjectType>
        HorizontallyExtendableObjects = [
            ObjectType.AreaSpecificPlatform,
            ObjectType.GreenIsland,
            ObjectType.MushroomIsland,
            ObjectType.CloudGround,
            ObjectType.HorizontalBricks,
            ObjectType.HorizontalBlocks,
            ObjectType.HorizontalCoins,
            ObjectType.Hole,
            ObjectType.BalanceHorizontalRope,
            ObjectType.BridgeV7,
            ObjectType.BridgeV8,
            ObjectType.BridgeV10,
            ObjectType.HoleWithWaterOrLava,
            ObjectType.HorizontalQuestionBlocksV3,
            ObjectType.HorizontalQuestionBlocksV7,
            ObjectType.Staircase,
        ];

    private static readonly ImmutableHashSet<ObjectType> VerticallyExtendableObjects
        = [
            ObjectType.VerticalBricks,
            ObjectType.VerticalBlocks,
            ObjectType.UnenterablePipe,
            ObjectType.EnterablePipe,
            ObjectType.RopeForLift,
            ObjectType.PulleyRope,
            ObjectType.Castle,
            ObjectType.CastleCeilingCap,
            ObjectType.Staircase,
            ObjectType.VerticalSeaBlocks,
            ObjectType.ExtendableJPipe,
            ObjectType.VerticalBalls,
        ];

    private static readonly ImmutableHashSet<ObjectType> ExtendableObjects =
        HorizontallyExtendableObjects
        .Union(VerticallyExtendableObjects)
        .Add(ObjectType.PageSkip);

    public static IEnumerable<byte> ToBytes(
        this IEnumerable<AreaObjectCommand> items)
    {
        foreach (var command in items)
        {
            yield return command.Value1;
            yield return command.Value2;
            if (command.IsThreeByteCommand)
            {
                yield return command.Value3;
            }
        }

        yield return AreaObjectCommand.TerminationCode;
    }

    public static IEnumerable<byte> ToBytes(
        this IEnumerable<AreaSpriteCommand> items)
    {
        foreach (var command in items)
        {
            yield return command.Value1;
            yield return command.Value2;
            if (command.IsThreeByteCommand)
            {
                yield return command.Value3;
            }
        }

        yield return AreaSpriteCommand.TerminationCode;
    }

    public static ObjectType ToObjectCode(this AreaPlatformType type)
    {
        return ObjectType.AreaSpecificPlatform | (ObjectType)(8 | (int)type);
    }

    public static bool IsHorizontallyExtendableObject(this ObjectType code)
    {
        return HorizontallyExtendableObjects.Contains(code);
    }

    public static bool IsVerticallyExtendableObject(this ObjectType code)
    {
        return VerticallyExtendableObjects.Contains(code);
    }

    public static bool IsExtendableObject(this ObjectType code)
    {
        return ExtendableObjects.Contains(code);
    }

    public static string BaseName(this ObjectType code)
    {
        return code switch
        {
            ObjectType.QuestionBlockPowerup => AddDescriptor(
                            Resources.QuestionBlock,
                            Resources.Powerup),
            ObjectType.QuestionBlockCoin => AddDescriptor(
                            Resources.QuestionBlock,
                            Resources.Coin),
            ObjectType.HiddenBlockCoin => AddDescriptor(
                            Resources.HiddenBlock,
                            Resources.Coin),
            ObjectType.HiddenBlock1UP => AddDescriptor(
                            Resources.HiddenBlock,
                            Resources.LifeMushroom),
            ObjectType.BrickPowerup => AddDescriptor(
                            Resources.Brick,
                            Resources.Powerup),
            ObjectType.BrickBeanstalk => AddDescriptor(
                            Resources.Brick,
                            Resources.Beanstalk),
            ObjectType.BrickStar => AddDescriptor(
                            Resources.Brick,
                            Resources.Star),
            ObjectType.Brick10Coins => AddDescriptor(
                            Resources.Brick,
                            Resources.TenCoins),
            ObjectType.Brick1UP => AddDescriptor(
                            Resources.Brick,
                            Resources.LifeMushroom),
            ObjectType.SidewaysPipe => Resources.SidewaysPipe,
            ObjectType.UsedBlock => Resources.UsedBlock,
            ObjectType.Spring => Resources.SpringBoard,
            ObjectType.JPipe or ObjectType.AltJPipe => Resources.JPipe,
            ObjectType.FlagPole or ObjectType.AltFlagPole => Resources.FlagPole,
            ObjectType.Nothing1 or ObjectType.Nothing2 => Resources.Empty,
            ObjectType.AreaSpecificPlatform => Resources.AreaSpecificPlatform,
            ObjectType.GreenIsland => Resources.AreaSpecificPlatform_Trees,
            ObjectType.MushroomIsland => Resources.AreaSpecificPlatform_Mushrooms,
            ObjectType.Cannon => Resources.AreaSpecificPlatform_BulletBillTurrets,
            ObjectType.CloudGround => Resources.AreaSpecificPlatform_Trees,// This is not a mistake. The regular ground changes to clouds, and
                                                                           // area specific platform is still trees.
            ObjectType.HorizontalBricks => Resources.HorizontalBricks,
            ObjectType.HorizontalBlocks => Resources.HorizontalStones,
            ObjectType.HorizontalCoins => Resources.HorizontalCoins,
            ObjectType.VerticalBricks => Resources.VerticalBricks,
            ObjectType.VerticalBlocks => Resources.VerticalStones,
            ObjectType.UnenterablePipe => Resources.UnenterablePipe,
            ObjectType.EnterablePipe => Resources.EnterablePipe,
            ObjectType.Hole => Resources.Hole,
            ObjectType.BalanceHorizontalRope => Resources.BalanceHorizontalRope,
            ObjectType.BridgeV7 => AddYDescriptor(Resources.Bridge, 7),
            ObjectType.BridgeV8 => AddYDescriptor(Resources.Bridge, 8),
            ObjectType.BridgeV10 => AddYDescriptor(Resources.Bridge, 10),
            ObjectType.HoleWithWaterOrLava => Resources.HoleWithWaterOrLava,
            ObjectType.HorizontalQuestionBlocksV3 => AddYDescriptor(Resources.HorizontalQuestionBlocks, 3),
            ObjectType.HorizontalQuestionBlocksV7 => AddYDescriptor(Resources.HorizontalQuestionBlocks, 7),
            ObjectType.PageSkip => Resources.ScreenJump,
            ObjectType.BowserAxe => Resources.BowserAxe,
            ObjectType.RopeForAxe => Resources.RopeForAxe,
            ObjectType.BowserBridge => Resources.BowserBridge,
            ObjectType.ScrollStopWarpZone => Resources.ScrollStopWarpZone,
            ObjectType.ScrollStop or ObjectType.AltScrollStop => Resources.ScrollStop,
            ObjectType.JumpingCheepCheepGenerator => Resources.RedCheepCheepFlying,
            ObjectType.BulletBillGenerator => Resources.BulletBillGenerator,
            ObjectType.StopGenerator => Resources.StopGenerator,
            ObjectType.LoopCommand => Resources.LoopCommand,
            ObjectType.TerrainAndBackgroundSceneryChange => Resources.BrickAndSceneryChange,
            ObjectType.ForegroundSceneryChange => Resources.ForegroundChange,
            ObjectType.RopeForLift => Resources.RopeForLift,
            ObjectType.PulleyRope => Resources.PulleyRope,
            ObjectType.EmptyTile => Resources.EmptyTile,
            ObjectType.Castle => Resources.Castle,
            ObjectType.CastleCeilingCap => Resources.CastleCeilingCap,
            ObjectType.Staircase => Resources.Staircase,
            ObjectType.CastleStairs => Resources.CastleStairs,
            ObjectType.CastleRectangularCeilingTiles => Resources.CastleRectangularCeilingTiles,
            ObjectType.CastleFloorRightEdge => Resources.CastleFloorRightEdge,
            ObjectType.CastleFloorLeftEdge => Resources.CastleFloorLeftEdge,
            ObjectType.CastleFloorLeftWall => Resources.CastleFloorLeftWall,
            ObjectType.CastleFloorRightWall => Resources.CastleFloorRightWall,
            ObjectType.VerticalSeaBlocks => Resources.VerticalSeaBlocks,
            ObjectType.ExtendableJPipe => Resources.ExtendableJPipe,
            ObjectType.VerticalBalls => Resources.VerticalBalls,
            _ => String.Format(
                            Resources.UnknownCommand,
                            ((int)code).ToString("X4")),
        };
    }

    public static int GetMaxLength(this ObjectType code)
    {
        return code switch
        {
            ObjectType.PageSkip => 0x20,
            ObjectType.EnterablePipe or ObjectType.UnenterablePipe => 8,
            ObjectType.Staircase => 9,
            ObjectType.Castle => 8,
            _ => code.IsExtendableObject() ? 0x10 : 1,
        };
    }

    public static string BaseName(this AreaSpriteCode code)
    {
        return code switch
        {
            AreaSpriteCode.AreaPointer => Resources.AreaPointer,
            AreaSpriteCode.GreenKoopaTroopa => AddDescriptor(
                Resources.KoopaTroopa,
                Resources.Green),
            AreaSpriteCode.RedKoopaTroopa => AddDescriptor(
                Resources.KoopaTroopa,
                Resources.Red),
            AreaSpriteCode.BuzzyBeetle => Resources.BuzzyBeetle,
            AreaSpriteCode.RedKoopaTroopaPatrol => AddTwoDescriptors(
                Resources.KoopaParatroopa,
                Resources.Red,
                Resources.WalksOffFloors),
            AreaSpriteCode.GreenKoopaTroopaStopped => AddTwoDescriptors(
                Resources.KoopaParatroopa,
                Resources.Green,
                Resources.WalksInPlace),
            AreaSpriteCode.HammerBros => Resources.HammerBros,
            AreaSpriteCode.Goomba => Resources.Goomba,
            AreaSpriteCode.Blooper => Resources.Blooper,
            AreaSpriteCode.BulletBill => Resources.BulletBill,
            AreaSpriteCode.YellowKoopaParatroopaStopped => AddTwoDescriptors(
                Resources.KoopaParatroopa,
                Resources.Yellow,
                Resources.FliesInPlace),
            AreaSpriteCode.GreenCheepCheep => AddDescriptor(
                Resources.CheepCheep,
                Resources.Green),
            AreaSpriteCode.RedCheepCheep => AddDescriptor(
                Resources.CheepCheep,
                Resources.Red),
            AreaSpriteCode.Podoboo => Resources.Podoboo,
            AreaSpriteCode.PiranhaPlant => Resources.PiranhaPlant,
            AreaSpriteCode.GreenKoopaParatroopaLeaping => AddTwoDescriptors(
                Resources.KoopaParatroopa,
                Resources.Green,
                Resources.Leaping),
            AreaSpriteCode.RedKoopaParatroopa => AddTwoDescriptors(
                Resources.KoopaParatroopa,
                Resources.Red,
                Resources.FliesVertically),
            AreaSpriteCode.GreenKoopaParatroopaFlying => AddTwoDescriptors(
                Resources.KoopaParatroopa,
                Resources.Green,
                Resources.FliesHorizontally),
            AreaSpriteCode.Lakitu => Resources.Lakitu,
            AreaSpriteCode.Spiny => AddDescriptor(
                Resources.Spiny,
                Resources.RandomWalkSpeed),
            AreaSpriteCode.RedFlyingCheepCheep => AddThreeDescriptors(
                Resources.CheepCheep,
                Resources.Generator,
                Resources.Red,
                Resources.Flying),
            AreaSpriteCode.BowsersFire => AddDescriptor(
                Resources.BowserFire,
                Resources.Generator),
            AreaSpriteCode.Fireworks => AddDescriptor(
                Resources.Firework,
                Resources.Crashes),
            AreaSpriteCode.BulletBillOrCheepCheeps => AddDescriptor(
                Resources.Generator,
                Resources.BulletBillOrCheepCheep),
            AreaSpriteCode.FireBarClockwise => AddDescriptor(
                Resources.FireBar,
                Resources.Clockwise),
            AreaSpriteCode.FastFireBarClockwise => AddTwoDescriptors(
                Resources.FireBar,
                Resources.Fast,
                Resources.Clockwise),
            AreaSpriteCode.FireBarCounterClockwise => AddDescriptor(
                Resources.FireBar,
                Resources.CounterClockwise),
            AreaSpriteCode.FastFireBarCounterClockwise => AddTwoDescriptors(
                Resources.FireBar,
                Resources.Fast,
                Resources.CounterClockwise),
            AreaSpriteCode.LongFireBarClockwise => AddTwoDescriptors(
                Resources.FireBar,
                Resources.Long,
                Resources.Clockwise),
            AreaSpriteCode.BalanceRopeLift => AddDescriptor(
                Resources.Lift,
                Resources.Balance),
            AreaSpriteCode.LiftDownThenUp => AddTwoDescriptors(
                Resources.Lift,
                Resources.Down,
                Resources.Up),
            AreaSpriteCode.LiftUp => AddDescriptor(
                Resources.Lift,
                Resources.Up),
            AreaSpriteCode.LiftDown => AddDescriptor(
                Resources.Lift,
                Resources.Down),
            AreaSpriteCode.LiftLeftThenRight => AddTwoDescriptors(
                Resources.Lift,
                Resources.Left,
                Resources.Right),
            AreaSpriteCode.LiftFalling => AddDescriptor(
                Resources.Lift,
                Resources.Falling),
            AreaSpriteCode.LiftRight => AddDescriptor(
                Resources.Lift,
                Resources.Right),
            AreaSpriteCode.ShortLiftUp => AddTwoDescriptors(
                Resources.Lift,
                Resources.Short,
                Resources.Up),
            AreaSpriteCode.ShortLiftDown => AddTwoDescriptors(
                Resources.Lift,
                Resources.Short,
                Resources.Down),
            AreaSpriteCode.Bowser => Resources.Bowser,
            AreaSpriteCode.WarpZoneCommand => Resources.WarpZoneCommand,
            AreaSpriteCode.ToadOrPrincess => Resources.ToadOrPrincess,
            AreaSpriteCode.TwoGoombasY10 => AddYDescriptor(Resources.Goomba2, 10),
            AreaSpriteCode.ThreeGoombasY10 => AddYDescriptor(Resources.Goomba3, 10),
            AreaSpriteCode.TwoGoombasY6 => AddYDescriptor(Resources.Goomba2, 6),
            AreaSpriteCode.ThreeGoombasY6 => AddYDescriptor(Resources.Goomba3, 6),
            AreaSpriteCode.TwoGreenKoopasY10 => AddYDescriptor(Resources.GreenKoopaTroopa2, 10),
            AreaSpriteCode.ThreeGreenKoopasY10 => AddYDescriptor(Resources.GreenKoopaTroopa3, 10),
            AreaSpriteCode.TwoGreenKoopasY6 => AddYDescriptor(Resources.GreenKoopaTroopa2, 6),
            AreaSpriteCode.ThreeGreenKoopasY6 => AddYDescriptor(Resources.GreenKoopaTroopa3, 6),
            AreaSpriteCode.ScreenJump => Resources.ScreenJump,
            _ => String.Format(
                Resources.UnknownCommand,
                ((int)code).ToString("X2")),
        };
    }

    public static string GetDescription(
        this AreaObjectCommand command,
        AreaPlatformType areaPlatformType)
    {
        var length = command.Parameter + 1;
        var code = command.ObjectType;
        switch (code)
        {
        case ObjectType.AreaSpecificPlatform:
            code = areaPlatformType.ToObjectCode();
            break;

        case ObjectType.BridgeV7:
            return AddTwoDescriptors(
                Resources.Bridge,
                YDescriptor(7),
                WidthDescriptor(length));

        case ObjectType.BridgeV8:
            return AddTwoDescriptors(
                Resources.Bridge,
                YDescriptor(8),
                WidthDescriptor(length));

        case ObjectType.BridgeV10:
            return AddTwoDescriptors(
                Resources.Bridge,
                YDescriptor(10),
                WidthDescriptor(length));

        case ObjectType.HorizontalQuestionBlocksV3:
            return AddTwoDescriptors(
                Resources.HorizontalQuestionBlocks,
                YDescriptor(3),
                WidthDescriptor(length));

        case ObjectType.HorizontalQuestionBlocksV7:
            return AddTwoDescriptors(
                Resources.HorizontalQuestionBlocks,
                YDescriptor(7),
                WidthDescriptor(length));
        }

        var baseName = code.BaseName();
        return code.IsHorizontallyExtendableObject()
            ? AddWidthDescriptor(baseName, length)
            : code.IsVerticallyExtendableObject()
            ? AddHeightDescriptor(baseName, length)
            : code == ObjectType.PageSkip
            ? AddDescriptor(baseName, PageSetDescriptor(length))
            : baseName;
    }

    private static string AddDescriptor(string item, string description)
    {
        return String.Format(
            Resources.ObjectWithDescriptor,
            item,
            description);
    }

    private static string AddTwoDescriptors(
        string item,
        string description1,
        string description2)
    {
        return String.Format(
            Resources.ObjectWithTwoDescriptors,
            item,
            description1,
            description2);
    }

    private static string AddThreeDescriptors(
        string item,
        string description1,
        string description2,
        string description3)
    {
        return String.Format(
            Resources.ObjectWithThreeDescriptors,
            item,
            description1,
            description2,
            description3);
    }

    private static string YDescriptor(object y)
    {
        return String.Format(Resources.WithSpecificYCoord, y);
    }

    private static string HeightDescriptor(object height)
    {
        return String.Format(Resources.VerticallyExtendableObject, height);
    }

    private static string WidthDescriptor(object width)
    {
        return String.Format(Resources.HorizontallyExtendableObject, width);
    }

    private static string AddYDescriptor(string item, object y)
    {
        return AddDescriptor(item, YDescriptor(y));
    }

    private static string AddHeightDescriptor(string item, object height)
    {
        return AddDescriptor(item, HeightDescriptor(height));
    }

    private static string AddWidthDescriptor(string item, object width)
    {
        return AddDescriptor(item, WidthDescriptor(width));
    }

    private static string PageSetDescriptor(int page)
    {
        return String.Format(Resources.SetPage, page);
    }

    private static string WarningDescriptor(string message)
    {
        return String.Format(Resources.WarningDescriptor, message);
    }

    private static string AddWarningDescriptor(string item, string message)
    {
        return AddDescriptor(item, WarningDescriptor(message));
    }
}
