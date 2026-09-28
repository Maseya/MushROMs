// <copyright file="AreaSpriteRenderer.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Smas.Smb1.AreaData.SpriteData;

using System.Collections.Generic;
using System.Linq;

using ObjectData;

using Snes;

using static System.Math;

public class AreaSpriteRenderer
{
    private const int PixelStartIndex = GfxData.SpritePixelDataStartIndex / 0x40;
    private const int PlayerPixelStartIndex = GfxData.MarioPixelDataStartIndex / 0x40;

    public AreaSpriteRenderer()
    {
        Commands = new Dictionary<AreaSpriteCode, SpriteCallback>()
        {
            { AreaSpriteCode.AreaPointer, AreaPointer },
            { AreaSpriteCode.GreenKoopaTroopa, GreenKoopaTroopa },
            { AreaSpriteCode.RedKoopaTroopa, RedKoopaTroopa },
            { AreaSpriteCode.BuzzyBeetle, BuzzyBeetle },
            { AreaSpriteCode.RedKoopaTroopaPatrol, RedKoopaTroopa },
            { AreaSpriteCode.GreenKoopaTroopaStopped, GreenKoopaTroopa },
            { AreaSpriteCode.HammerBros, HammerBros },
            { AreaSpriteCode.Goomba, Goomba },
            { AreaSpriteCode.Blooper, Blooper },
            { AreaSpriteCode.BulletBill, BulletBill },
            { AreaSpriteCode.YellowKoopaParatroopaStopped, GoldKoopaParaTroopa },
            { AreaSpriteCode.GreenCheepCheep, GreenCheepCheep },
            { AreaSpriteCode.RedCheepCheep, RedCheepCheep },
            { AreaSpriteCode.Podoboo, Podoboo },
            { AreaSpriteCode.PiranhaPlant, PiranhaPlant },
            { AreaSpriteCode.GreenKoopaParatroopaLeaping, GreenKoopaParaTroopa },
            { AreaSpriteCode.RedKoopaParatroopa, RedKoopaParaTroopa },
            { AreaSpriteCode.GreenKoopaParatroopaFlying, GreenKoopaParaTroopa2 },
            { AreaSpriteCode.Lakitu, Lakitu },
            { AreaSpriteCode.Spiny, Spiny },
            { AreaSpriteCode.RedFlyingCheepCheep, RedFlyingCheepCheep },
            { AreaSpriteCode.BowsersFire, BowserFire },
            { AreaSpriteCode.Fireworks, Firework },
            { AreaSpriteCode.BulletBillOrCheepCheeps, BulletBill },
            { AreaSpriteCode.FireBarClockwise, FireBarClockwise },
            { AreaSpriteCode.FastFireBarClockwise, FireBarClockwiseFast },
            { AreaSpriteCode.FireBarCounterClockwise, FireBarCounterClockwise },
            { AreaSpriteCode.FastFireBarCounterClockwise, FireBarCounterClockwiseFast },
            { AreaSpriteCode.LongFireBarClockwise, LongFireBarClockwiseFast},
            { AreaSpriteCode.BalanceRopeLift, BalanceRopeLift },
            { AreaSpriteCode.LiftDownThenUp,  LiftDownThenUp },
            { AreaSpriteCode.LiftUp, LiftUp },
            { AreaSpriteCode.LiftDown, LiftDown },
            { AreaSpriteCode.LiftLeftThenRight, LiftLeftThenRight },
            { AreaSpriteCode.LiftFalling, LiftFalling },
            { AreaSpriteCode.LiftRight, LiftRight },
            { AreaSpriteCode.ShortLiftUp, ShortLiftUp },
            { AreaSpriteCode.ShortLiftDown, ShortLiftDown },
            { AreaSpriteCode.Bowser, Bowser },
            { AreaSpriteCode.WarpZoneCommand, WarpZone },
            { AreaSpriteCode.ToadOrPrincess, Toad },
            { AreaSpriteCode.TwoGoombasY10, TwoGoombasY10 },
            { AreaSpriteCode.ThreeGoombasY10, ThreeGoombasY10 },
            { AreaSpriteCode.TwoGoombasY6, TwoGoombasY6 },
            { AreaSpriteCode.ThreeGoombasY6, ThreeGoombasY6 },
            { AreaSpriteCode.TwoGreenKoopasY10, TwoGreenKoopasY10 },
            { AreaSpriteCode.ThreeGreenKoopasY10, ThreeGreenKoopasY10 },
            { AreaSpriteCode.TwoGreenKoopasY6, TwoGreenKoopasY6 },
            { AreaSpriteCode.ThreeGreenKoopasY6, ThreeGreenKoopasY6 },
        };

        ObjectCommands = new Dictionary<ObjectType, SpriteCallback>()
        {
            { ObjectType.EnterablePipe, PipePiranhaPlant },
            { ObjectType.UnenterablePipe, PipePiranhaPlant },
            { ObjectType.Spring, SpringBoard },
            { ObjectType.FlagPole, FlagPole },
            { ObjectType.AltFlagPole, FlagPole },
            { ObjectType.BulletBillGenerator, BulletBill },
            { ObjectType.JumpingCheepCheepGenerator, RedFlyingCheepCheep },
            { ObjectType.StopGenerator, StopGenerator },
            { ObjectType.LoopCommand, Loop },
            { ObjectType.ScrollStop, ScrollStop },
            { ObjectType.ScrollStopWarpZone, ScrollStopWarpZone },
            { ObjectType.AltScrollStop, ScrollStop },
        };
    }

    private delegate IEnumerable<Sprite> SpriteCallback(int x, int y, int frame);

    private Dictionary<AreaSpriteCode, SpriteCallback> Commands
    {
        get;
    }

    private Dictionary<ObjectType, SpriteCallback> ObjectCommands
    {
        get;
    }

    public static IEnumerable<Sprite> GetPlayerSprite(
        int x,
        int y,
        Player player,
        PlayerState state,
        int frame)
    {
        var tile = new SpriteTile(PlayerPixelStartIndex, 0x0F, 9);

        tile.TileIndex += frame * 8;

        if (player == Player.Luigi)
        {
            tile.TileIndex += 0x100;
        }

        if (state == PlayerState.Small)
        {
            tile.TileIndex += 0x60;
        }

        for (var i = 0; i < 8; i++)
        {
            yield return new Sprite(
                x + ((i & 1) << 3),
                y + ((i & 6) << 2),
                tile);
            tile.TileIndex++;
        }
    }

    public static IEnumerable<Sprite> GetObjectDataSprites(int[] tilemap)
    {
        var result = Enumerable.Empty<Sprite>();
        for (var y = 0; y < 0x0C; y++)
        {
            var index = y * 0x200;
            var pY = y << 4;
            for (var x = 0; x < 0x200; x++)
            {
                var pX = x << 4;
                switch (tilemap[index + x])
                {
                case 0xE8:
                    result = result.Concat(Powerup(pX, pY));
                    break;

                case 0x62:
                    result = result.Concat(HiddenQuestionBlock(pX, pY));
                    break;

                case 0x63:
                    result = result.Concat(HiddenBlock1UP(pX, pY));
                    break;

                case 0x58:
                    result = result.Concat(Powerup(pX, pY));
                    break;

                case 0x59:
                    result = result.Concat(BeanStalk(pX, pY));
                    break;

                case 0x5A:
                    result = result.Concat(Star(pX, pY));
                    break;

                case 0x5B:
                    result = result.Concat(Brick10Coins(pX, pY));
                    break;

                case 0x5C:
                    result = result.Concat(Brick1Up(pX, pY));
                    break;
                }
            }
        }

        return result;
    }

    public IEnumerable<Sprite> GetSprites(
        IEnumerable<AreaSpriteCommand> areaSpriteCommands,
        IEnumerable<AreaObjectCommand> areaObjectData,
        int frame,
        AreaType areaType,
        bool showPipePiranhaPlants)
    {
        return Enumerable.Concat(
            GetSprites(areaSpriteCommands, frame),
            GetSprites(areaObjectData, frame, areaType, showPipePiranhaPlants));
    }

    public IEnumerable<Sprite> GetSprites(
        IEnumerable<AreaObjectCommand> areaObjectData,
        int frame,
        AreaType areaType,
        bool showPipePiranhaPlants)
    {
        ObjectCommands[ObjectType.BulletBillGenerator] = areaType == AreaType.Water
            ? RedCheepCheep
            : BulletBill;

        var result = Enumerable.Empty<Sprite>();
        var screen = 0;
        foreach (var command in areaObjectData)
        {
            if (command.PageFlag)
            {
                screen += 0x10;
            }
            else if (command.ObjectType == ObjectType.PageSkip)
            {
                screen = command.PrimaryCommand << 4;
            }

            if (!showPipePiranhaPlants && (
                command.ObjectType == ObjectType.EnterablePipe
                || command.ObjectType == ObjectType.UnenterablePipe))
            {
                continue;
            }

            var x = (screen | command.X) << 4;
            var y = (command.Y + 2) << 4;
            if (ObjectCommands.TryGetValue(command.ObjectType, out var getSprites))
            {
                result = result.Concat(getSprites(x, y, frame));
            }
        }

        return result;
    }

    public IEnumerable<Sprite> GetSprites(
        IEnumerable<AreaSpriteCommand> areaSpriteCommands,
        int frame)
    {
        var screen = 0;
        var screenSkip = false;
        var lastAreaPointerX = -0x80;
        var areaPointerOffset = 0;

        foreach (var command in areaSpriteCommands)
        {
            if (command.ScreenFlag && !screenSkip)
            {
                screen += 0x10;
            }

            var x = (screen | command.X) << 4;
            var y = (command.Y + 1) << 4;
            if (command.Code == AreaSpriteCode.AreaPointer)
            {
                if (x - lastAreaPointerX < 0x80)
                {
                    areaPointerOffset += 8;
                    y += areaPointerOffset;
                }
                else
                {
                    areaPointerOffset = 0;
                }

                lastAreaPointerX = x;
            }

            screenSkip = command.Code == AreaSpriteCode.ScreenJump;
            if (screenSkip)
            {
                screen = command.BaseCommand << 4;
            }

            if (Commands.TryGetValue(command.Code, out var getSprites))
            {
                foreach (var sprite in getSprites(x, y, frame))
                {
                    var result = sprite;
                    if (command.HardWorldFlag)
                    {
                        result.TileProperties |= TileProperties.Invert;
                    }

                    yield return result;
                }
            }
        }
    }

    private static IEnumerable<Sprite> KoopaTroopa(int x, int y, int palette, int frame)
    {
        var index = (((frame + (x >> 1)) & 8) >> 3) * 5;
        var tile = new ChrTile(
            0xA0 + index,
            palette,
            LayerPriority.Priority2,
            TileFlip.Horizontal);

        y -= 7;
        yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));

        y += 8;
        tile.TileIndex++;
        yield return new Sprite(x + 8, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex++;
        yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));

        y += 8;
        tile.TileIndex++;
        yield return new Sprite(x + 8, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex++;
        yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));
    }

    private static IEnumerable<Sprite> KoopaParaTroopa(int x, int y, int palette, int frame)
    {
        var index = ((frame + (x >> 1)) & 8) >> 3;
        var tile = new ChrTile(
            0xA0 + (index * 5),
            palette,
            LayerPriority.Priority2,
            TileFlip.Horizontal);

        y -= 7;
        yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex = 0x69 + (index << 1);
        yield return new Sprite(x + 8, y, new SpriteTile(tile, PixelStartIndex));

        y += 8;
        tile.TileIndex = 0xA2 + (index * 5);
        yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex = 0x6A + (index * 2);
        yield return new Sprite(x + 8, y, new SpriteTile(tile, PixelStartIndex));

        y += 8;
        tile.TileIndex = 0xA3 + (index * 5);
        yield return new Sprite(x + 8, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex++;
        yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));
    }

    private static IEnumerable<Sprite> Text(int x, int y, int palette, string text)
    {
        var tile = new SpriteTile(0, palette, 11, false, false);
        foreach (var c in text.ToUpper())
        {
            if ((uint)(c - '0') <= 9)
            {
                tile.TileIndex = c - '0';
            }
            else if ((uint)(c - 'A') <= 'Z' - 'A')
            {
                tile.TileIndex = c - 'A' + 10;
            }
            else if (c == '-')
            {
                tile.TileIndex = 0x28;
            }
            else if (c == '!')
            {
                tile.TileIndex = 0x2B;
            }
            else if (c == '#')
            {
                tile.TileIndex = 0x29;
            }
            else if (c == ' ')
            {
                tile.TileIndex = 0x24;
            }

            yield return new Sprite(x, y, tile);
            x += 8;
        }
    }

    private static IEnumerable<Sprite> CheepCheep(int x, int y, int palette, int frame)
    {
        var index = ((frame + (x << 1)) & 0x10) >> 4;
        var tile = new ChrTile(
            0xB2 + (index * 4),
            palette,
            LayerPriority.Priority2,
            TileFlip.Horizontal);

        yield return new Sprite(x + 8, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex = 0xB3;
        yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex += 1 + (index * 3);
        y += 8;
        yield return new Sprite(x + 8, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex = 0xB5;
        yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));
    }

    private static IEnumerable<Sprite> FireBar(
        int x,
        int y,
        int frame,
        int rate,
        int size)
    {
        var tile = new ChrTile(
            0xBE + ((frame & 4) >> 2),
            4,
            LayerPriority.Priority2,
            (TileFlip)((frame & 0x0C) >> 2));

        x += 4;
        y -= 12;

        // The exact rotation rate and movement is just arbitrarily made by me and does
        // not yet follow what is in game. But this is where having math knowledge is
        // nice, as I can make this look pretty good. The initial angle will depend on
        // the x and y and location of the sprite, to give the visuals some variety.
        var angle = (((frame & ~3) + x + y) >> 1) % rate * 2 * PI / rate;
        for (var i = 0; i < size; i++)
        {
            yield return new Sprite(
                x + (int)(8 * i * Cos(angle)),
                y + (int)(8 * i * Sin(angle)),
                new SpriteTile(tile, PixelStartIndex));
        }
    }

    private static IEnumerable<Sprite> Lift(int x, int y, int size)
    {
        var tile = new ChrTile(0x87, 6, LayerPriority.Priority2, 0);
        var spriteTile = new SpriteTile(tile, PixelStartIndex);
        for (var i = 0; i <= 8 * size; i += 8)
        {
            yield return new Sprite(x + i, y, spriteTile);
        }
    }

    private static IEnumerable<Sprite> Powerup(int x, int y)
    {
        return Mushroom(x, y, 6);
    }

    private static IEnumerable<Sprite> Brick1Up(int x, int y)
    {
        return Mushroom(x, y, 5);
    }

    private static IEnumerable<Sprite> HiddenBlock1UP(int x, int y)
    {
        return Mushroom(x, y, 5).Concat(HiddenQuestionBlock(x, y));
    }

    private static IEnumerable<Sprite> HiddenQuestionBlock(int x, int y)
    {
        // TODO(swr): rip from tile data.
        var tile = new Obj16Tile(0x0453, 0x0455, 0x0454, 0x0456);
        yield return new Sprite(
            x, y, new SpriteTile(tile[0], 0, -1), TileProperties.Transparent);
        yield return new Sprite(
            x, y + 8, new SpriteTile(tile[2], 0, -1), TileProperties.Transparent);
        yield return new Sprite(
            x + 8, y, new SpriteTile(tile[1], 0, -1), TileProperties.Transparent);
        yield return new Sprite(
            x + 8, y + 8, new SpriteTile(tile[3], 0, -1), TileProperties.Transparent);
    }

    private static IEnumerable<Sprite> Mushroom(int x, int y, int palette)
    {
        var tile = new ChrTile(0xE9, palette, LayerPriority.Priority2, 0);

        y -= 8;
        yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));

        x += 8;
        tile.TileIndex++;
        yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));

        y += 8;
        tile.TileIndex = 0x79;
        yield return new Sprite(
            x, y, new SpriteTile(tile, PixelStartIndex), TileProperties.Transparent);

        x -= 8;
        tile.TileIndex--;
        yield return new Sprite(
            x, y, new SpriteTile(tile, PixelStartIndex), TileProperties.Transparent);
    }

    private static IEnumerable<Sprite> Coin(int x, int y)
    {
        var tile = new ChrTile(0x28, 2, LayerPriority.Priority2, 0);

        y -= 8;
        yield return new Sprite(
            x, y, new SpriteTile(tile, PixelStartIndex), TileProperties.Transparent);

        x += 8;
        tile.TileIndex++;
        yield return new Sprite(
            x, y, new SpriteTile(tile, PixelStartIndex), TileProperties.Transparent);

        y += 8;
        tile.TileIndex = 0x39;
        yield return new Sprite(
            x, y, new SpriteTile(tile, PixelStartIndex), TileProperties.Transparent);

        x -= 8;
        tile.TileIndex--;
        yield return new Sprite(
            x, y, new SpriteTile(tile, PixelStartIndex), TileProperties.Transparent);
    }

    private static IEnumerable<Sprite> HiddenCoinBlock(int x, int y)
    {
        return Coin(x, y).Concat(HiddenQuestionBlock(x, y));
    }

    private static IEnumerable<Sprite> Star(int x, int y)
    {
        var tile = new ChrTile(0x8D, 2, LayerPriority.Priority2, 0);

        y -= 8;
        yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));

        x += 8;
        tile.XFlipped = true;
        yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));

        y += 8;
        tile.TileIndex = 0xE4;
        yield return new Sprite(
            x, y, new SpriteTile(tile, PixelStartIndex), TileProperties.Transparent);

        x -= 8;
        tile.XFlipped = false;
        yield return new Sprite(
            x, y, new SpriteTile(tile, PixelStartIndex), TileProperties.Transparent);
    }

    private static int PiranhaPlantOffset(int cycle)
    {
        const int AnimationTime = 1 << 8;
        const int AnimationTimeMask = AnimationTime - 1;
        const int AscensionHeight = 0x18;
        const int WaitTime = (AnimationTime / 2) - AscensionHeight;
        const int MoveTime = AscensionHeight;
        const int WaitOutsideFrame = WaitTime + MoveTime;
        const int DescentEndFrame = WaitOutsideFrame + WaitTime;
        // The Piranha Plant movement goes through a 256-frame animation cycle of
        // coming out of the pipe, staying out, going back in, staying in, then
        // repeating. The animation is arbitrarily made by me and does not yet follow
        // whatever the animation cycle really is in-game. I just went for a reasonable
        // approximation.
        cycle &= AnimationTimeMask;

        return
            // Time to stay in the pipe
            cycle < WaitTime
            ? 0

            // Moving upward, out of the pipe.
            : cycle < WaitOutsideFrame
            ? cycle - WaitTime

            // Staying out of the pipe for a while.
            : cycle < DescentEndFrame
            ? AscensionHeight

            // Going back into the pipe.
            : AscensionHeight - (cycle - DescentEndFrame);
    }

    private static IEnumerable<Sprite> BeanStalk(int x, int y)
    {
        var tile = new ChrTile(0x12C, 5, LayerPriority.Priority3, 0);

        // Grow the bean stalk to the top of the screen.
        for (y -= 0x10; y > 0x10; y -= 0x10)
        {
            tile.TileIndex = 0x12C;
            yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));

            tile.TileIndex++;
            yield return new Sprite(x + 8, y, new SpriteTile(tile, PixelStartIndex));

            tile.TileIndex += 0x0F;
            yield return new Sprite(x, y + 8, new SpriteTile(tile, PixelStartIndex));

            tile.TileIndex++;
            yield return new Sprite(
                x + 8, y + 8, new SpriteTile(tile, PixelStartIndex));
        }

        tile = new ChrTile(0x12A, 5, LayerPriority.Priority3, 0);
        yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex++;
        yield return new Sprite(x + 8, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex += 0x0F;
        yield return new Sprite(x, y + 8, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex++;
        yield return new Sprite(x + 8, y + 8, new SpriteTile(tile, PixelStartIndex));
    }

    private static IEnumerable<Sprite> Brick10Coins(int x, int y)
    {
        x -= 4;
        y -= 8;
        var tile = new ChrTile(0x2C, 2, LayerPriority.Priority3, 0);
        yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex++;
        yield return new Sprite(x + 8, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex += 0x0F;
        yield return new Sprite(x, y + 8, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex++;
        yield return new Sprite(x + 8, y + 8, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex = 0x2E;
        x += 8;
        yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex++;
        yield return new Sprite(x + 8, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex += 0x0F;
        yield return new Sprite(x, y + 8, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex++;
        yield return new Sprite(x + 8, y + 8, new SpriteTile(tile, PixelStartIndex));
    }

    private IEnumerable<Sprite> GreenKoopaTroopa(int x, int y, int frame)
    {
        return KoopaTroopa(x, y, 5, frame);
    }

    private IEnumerable<Sprite> RedKoopaTroopa(int x, int y, int frame)
    {
        return KoopaTroopa(x, y, 6, frame);
    }

    private IEnumerable<Sprite> GreenKoopaParaTroopa(int x, int y, int frame)
    {
        return KoopaParaTroopa(x, y, 5, frame / 2);
    }

    private IEnumerable<Sprite> GreenKoopaParaTroopa2(int x, int y, int frame)
    {
        return KoopaParaTroopa(x, y, 5, frame);
    }

    private IEnumerable<Sprite> RedKoopaParaTroopa(int x, int y, int frame)
    {
        return KoopaParaTroopa(x, y, 6, frame);
    }

    private IEnumerable<Sprite> GoldKoopaParaTroopa(int x, int y, int frame)
    {
        return KoopaParaTroopa(x, y, 7, frame);
    }

    private IEnumerable<Sprite> Lakitu(int x, int y, int frame)
    {
        y -= 7;
        var tile = new ChrTile(0xB8, 5, LayerPriority.Priority2, 0);
        yield return new Sprite(x + 8, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex++;
        yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));

        y += 8;
        tile.TileIndex++;
        yield return new Sprite(x + 8, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex++;
        yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));

        y += 8;
        tile.TileIndex++;
        tile.XFlipped = true;
        yield return new Sprite(x + 8, y, new SpriteTile(tile, PixelStartIndex));

        tile.XFlipped = false;
        yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));
    }

    private IEnumerable<Sprite> HammerBros(int x, int y, int frame)
    {
        y -= 7;

        // The hammer bros animation was pretty awful. Like other animations, this
        // movement is all guess-work and does not yet emulate in-game animation.

        // Hammer Bros animation cycle.
        const int cycle = 96;

        // Maximum total horizontal displacement from end to end.
        const int distance = 8;

        // Moving the hammer bro left and right.
        var offset = frame % cycle;
        if (offset > cycle / 2)
        {
            // Move to the left until at max left displacement, then move to the right
            // until back at center.
            x += (int)(distance * ((offset - (3 * (cycle / 4.0))) / (cycle / 2.0)));
        }
        else
        {
            // Opposite of above movement.
            x += (int)(distance * ((cycle / 4.0) - offset) / (cycle / 2.0));
        }

        var tile = new ChrTile(0x7C, 5, LayerPriority.Priority2, TileFlip.Horizontal);
        yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex++;
        yield return new Sprite(x + 8, y, new SpriteTile(tile, PixelStartIndex));

        // Adds arm for throwing hammer.
        if (((frame + (x >> 1)) & 8) >> 3 == 0)
        {
            y += 8;
            tile.TileIndex = 0x88;
            yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));

            tile.TileIndex++;
            yield return new Sprite(x + 8, y, new SpriteTile(tile, PixelStartIndex));

            y += 8;
            tile.TileIndex++;
            yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));
        }
        else
        {
            y += 8;
            tile.TileIndex = 0x8C;
            yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));

            tile.TileIndex = 0xD1;
            yield return new Sprite(x + 8, y, new SpriteTile(tile, PixelStartIndex));

            y += 8;
            tile.TileIndex = 0xD2;
            yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));
        }

        tile.TileIndex++;
        yield return new Sprite(x + 8, y, new SpriteTile(tile, PixelStartIndex));
    }

    private IEnumerable<Sprite> Goomba(int x, int y, int frame)
    {
        y++;
        var tile = new ChrTile(0xC6, 1, LayerPriority.Priority2, 0);
        yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex++;
        yield return new Sprite(x + 8, y, new SpriteTile(tile, PixelStartIndex));

        y += 8;

        // Walking animation.
        if (((frame + (x >> 1)) & 8) >> 3 == 0)
        {
            tile.TileIndex++;
            yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));

            tile.TileIndex++;
            yield return new Sprite(x + 8, y, new SpriteTile(tile, PixelStartIndex));
        }
        else
        {
            tile.TileIndex += 2;
            tile.XFlipped = true;
            yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));

            tile.TileIndex--;
            yield return new Sprite(x + 8, y, new SpriteTile(tile, PixelStartIndex));
        }
    }

    private IEnumerable<Sprite> BuzzyBeetle(int x, int y, int frame)
    {
        y += 2;
        var index = ((frame + (x << 1)) & 8) >> 1;
        var tile = new ChrTile(0xAA + index, 5, LayerPriority.Priority2, TileFlip.Horizontal);

        yield return new Sprite(x + 8, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex++;
        yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex++;
        y += 8;
        yield return new Sprite(x + 8, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex++;
        yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));
    }

    private IEnumerable<Sprite> Spiny(int x, int y, int frame)
    {
        y++;
        var index = ((frame + (x << 1)) & 16) >> 2;
        var tile = new ChrTile(0x96 + index, 6, LayerPriority.Priority2, TileFlip.Horizontal);

        yield return new Sprite(x + 8, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex++;
        yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex++;
        y += 8;
        yield return new Sprite(x + 8, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex++;
        yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));
    }

    private IEnumerable<Sprite> Firework(int x, int y, int frame)
    {
        y++;
        frame = (frame + x + y) % 90;
        if (frame > 20)
        {
            //yield break;
        }

        // Maybe set up a click to explode?
        var index = 1;// frame < 2 ? 2 : frame < 4 ? 1 : 0;

        var tile = new ChrTile(0xCA + index, 4, LayerPriority.Priority2, 0);

        yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));

        tile.XFlipped = true;
        yield return new Sprite(x + 8, y, new SpriteTile(tile, PixelStartIndex));

        tile.YFlipped = true;
        yield return new Sprite(x + 8, y + 8, new SpriteTile(tile, PixelStartIndex));

        tile.XFlipped = false;
        yield return new Sprite(x, y + 8, new SpriteTile(tile, PixelStartIndex));
    }

    private IEnumerable<Sprite> Toad(int x, int y, int frame)
    {
        y -= 7;
        var tile = new ChrTile(0xCD, 6, LayerPriority.Priority2, 0);
        yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));

        tile.XFlipped = true;
        yield return new Sprite(x + 8, y, new SpriteTile(tile, PixelStartIndex));

        y += 8;
        tile.TileIndex++;
        yield return new Sprite(x + 8, y, new SpriteTile(tile, PixelStartIndex));

        tile.XFlipped = false;
        yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));

        y += 8;
        tile.TileIndex++;
        yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));

        tile.XFlipped = true;
        yield return new Sprite(x + 8, y, new SpriteTile(tile, PixelStartIndex));
    }

    private IEnumerable<Sprite> Bowser(int x, int y, int frame)
    {
        y -= 0x17;
        var tile = new ChrTile(0x1C5, 0, LayerPriority.Priority2, TileFlip.Horizontal);
        yield return new Sprite(x + 8, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex--;
        yield return new Sprite(x + 16, y, new SpriteTile(tile, PixelStartIndex));

        y += 8;
        tile.TileIndex = 0x1EF;
        yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex--;
        yield return new Sprite(x + 8, y, new SpriteTile(tile, PixelStartIndex));

        y += 8;
        tile.TileIndex += 0x10;
        yield return new Sprite(x + 8, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex++;
        yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));

        y -= 8;
        tile.TileIndex = 0x1C1;
        yield return new Sprite(x + 16, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex--;
        yield return new Sprite(x + 24, y, new SpriteTile(tile, PixelStartIndex));

        y += 8;
        tile.TileIndex += 0x10;
        yield return new Sprite(x + 24, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex++;
        yield return new Sprite(x + 16, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex += 0x12;
        for (var i = 0; i < 2; i++)
        {
            y += 8;
            for (var j = 0; j < 4; j++)
            {
                yield return new Sprite(x + (j << 3), y, new SpriteTile(tile, PixelStartIndex));
                tile.TileIndex--;
            }

            tile.TileIndex += 0x10 + 4;
        }
    }

    private IEnumerable<Sprite> BowserFire(int x, int y, int frame)
    {
        y++;
        var index = (frame + (x << 1)) / 12 % 3;
        var offsets = new int[] { 0, 3, 0x20 };

        var tile = new ChrTile(0x186 + offsets[index], 0, LayerPriority.Priority2, 0);
        yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex++;
        yield return new Sprite(x + 8, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex++;
        yield return new Sprite(x + 16, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex += 0x10;
        y += 8;
        yield return new Sprite(x + 16, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex--;
        yield return new Sprite(x + 8, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex--;
        yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));
    }

    private IEnumerable<Sprite> GreenCheepCheep(int x, int y, int frame)
    {
        return CheepCheep(x, y, 5, frame);
    }

    private IEnumerable<Sprite> RedFlyingCheepCheep(int x, int y, int frame)
    {
        return CheepCheep(x, y, 6, (int)(1.5 * frame));
    }

    private IEnumerable<Sprite> StopGenerator(int x, int y, int frame)
    {
        return Text(x, y + 8, 2, "Stop Generator");
    }

    private IEnumerable<Sprite> Loop(int x, int y, int frame)
    {
        return Text(x, y + 8, 2, "Loop");
    }

    private IEnumerable<Sprite> ScrollStop(int x, int y, int frame)
    {
        return Text(x, y, 1, "Scroll Stop");
    }

    private IEnumerable<Sprite> AreaPointer(int x, int y, int frame)
    {
        return Text(x, y, 1, "Area Pointer");
    }

    private IEnumerable<Sprite> WarpZone(int x, int y, int frame)
    {
        return Text(x, y, 1, "Warpzone");
    }

    private IEnumerable<Sprite> ScrollStopWarpZone(int x, int y, int frame)
    {
        return Text(x, y, 1, "Scroll Stop Warpzone");
    }

    private IEnumerable<Sprite> RedCheepCheep(int x, int y, int frame)
    {
        return CheepCheep(x, y, 6, frame);
    }

    private IEnumerable<Sprite> Podoboo(int x, int y, int frame)
    {
        var index = 0;// (((frame + x + y) >> 2) % 6) & ~1;
        var tile = new ChrTile(0x162 + index, 2, LayerPriority.Priority2, 0);

        yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex++;
        yield return new Sprite(x + 8, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex += 0x10;
        yield return new Sprite(x + 8, y + 8, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex--;
        yield return new Sprite(x, y + 8, new SpriteTile(tile, PixelStartIndex));
    }

    private IEnumerable<Sprite> FireBarClockwise(int x, int y, int frame)
    {
        return FireBar(x, y, frame, 0x70, 6);
    }

    private IEnumerable<Sprite> FireBarCounterClockwise(int x, int y, int frame)
    {
        return FireBar(x, y, frame, -0x70, 6);
    }

    private IEnumerable<Sprite> FireBarClockwiseFast(int x, int y, int frame)
    {
        return FireBar(x, y, frame, 0x64, 6);
    }

    private IEnumerable<Sprite> FireBarCounterClockwiseFast(int x, int y, int frame)
    {
        return FireBar(x, y, frame, -0x64, 6);
    }

    private IEnumerable<Sprite> LongFireBarClockwiseFast(int x, int y, int frame)
    {
        return FireBar(x, y, frame, 0x68, 12);
    }

    private IEnumerable<Sprite> TwoGoombasY10(int x, int y, int frame)
    {
        return Goombas(x, 2, 0xC0, frame);
    }

    private IEnumerable<Sprite> ThreeGoombasY10(int x, int y, int frame)
    {
        return Goombas(x, 3, 0xC0, frame);
    }

    private IEnumerable<Sprite> TwoGoombasY6(int x, int y, int frame)
    {
        return Goombas(x, 2, 0x80, frame);
    }

    private IEnumerable<Sprite> ThreeGoombasY6(int x, int y, int frame)
    {
        return Goombas(x, 3, 0x80, frame);
    }

    private IEnumerable<Sprite> TwoGreenKoopasY10(int x, int y, int frame)
    {
        return GreenKoopas(x, 2, 0xC0, frame);
    }

    private IEnumerable<Sprite> ThreeGreenKoopasY10(int x, int y, int frame)
    {
        return GreenKoopas(x, 3, 0xC0, frame);
    }

    private IEnumerable<Sprite> TwoGreenKoopasY6(int x, int y, int frame)
    {
        return GreenKoopas(x, 2, 0x80, frame);
    }

    private IEnumerable<Sprite> ThreeGreenKoopasY6(int x, int y, int frame)
    {
        return GreenKoopas(x, 3, 0x80, frame);
    }

    private IEnumerable<Sprite> Goombas(int x, int count, int y, int frame)
    {
        x -= 0x30;
        for (var i = 0; i < count; i++, x += 0x18)
        {
            foreach (var sprite in Goomba(x, y, frame))
            {
                yield return sprite;
            }
        }
    }

    private IEnumerable<Sprite> GreenKoopas(int x, int count, int y, int frame)
    {
        x -= 0x30;
        for (var i = 0; i < count; i++, x += 0x18)
        {
            foreach (var sprite in GreenKoopaTroopa(x, y, frame))
            {
                yield return sprite;
            }
        }
    }

    private IEnumerable<Sprite> LiftFalling(int x, int y, int frame)
    {
        return Lift(x, y - 0x0F, 5);
    }

    private IEnumerable<Sprite> LiftRight(int x, int y, int frame)
    {
        return Lift(x, y - 0x0F, 5);
    }

    private static readonly int[] LeftRightLiftOffsets = [
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x01, 0x01, 0x01,
        0x01, 0x01, 0x01, 0x01, 0x01, 0x02, 0x02, 0x02, 0x02, 0x02, 0x03, 0x03, 0x03,
        0x04, 0x04, 0x04, 0x05, 0x05, 0x05, 0x06, 0x06, 0x07, 0x07, 0x07, 0x08, 0x08,
        0x09, 0x09, 0x0A, 0x0B, 0x0B, 0x0C, 0x0C, 0x0D, 0x0E, 0x0E, 0x0F, 0x10, 0x10,
        0x11, 0x12, 0x12, 0x13, 0x14, 0x15, 0x16, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B,
        0x1C, 0x1C, 0x1D, 0x1E, 0x1F, 0x20, 0x21, 0x21, 0x22, 0x23, 0x24, 0x24, 0x25,
        0x26, 0x27, 0x27, 0x28, 0x28, 0x29, 0x2A, 0x2A, 0x2B, 0x2B, 0x2C, 0x2C, 0x2D,
        0x2D, 0x2E, 0x2E, 0x2F, 0x2F, 0x30, 0x30, 0x30, 0x31, 0x31, 0x32, 0x32, 0x32,
        0x32, 0x33, 0x33, 0x33, 0x33, 0x34, 0x34, 0x34, 0x34, 0x34, 0x34, 0x35, 0x35,
        0x35, 0x35, 0x35, 0x35, 0x35, 0x35, 0x35,
    ];

    private IEnumerable<Sprite> LiftLeftThenRight(int x, int y, int frame)
    {
        frame += x;
        frame %= 2 * LeftRightLiftOffsets.Length;
        if (frame >= LeftRightLiftOffsets.Length)
        {
            frame = (2 * LeftRightLiftOffsets.Length) - 1 - frame;
        }

        return Lift(x - LeftRightLiftOffsets[frame], y - 0x0F, 5);
    }

    private static readonly int[] UpDownLiftOffsets = [
        -0x03, -0x03, -0x03, -0x03, -0x03, -0x03, -0x03, -0x02, -0x02, -0x02, -0x02,
        -0x02, -0x02, -0x02, -0x02, -0x02, -0x01, -0x01, -0x01, -0x01, 0x00, 0x00, 0x00,
        0x00, 0x01, 0x01, 0x02, 0x02, 0x02, 0x03, 0x03, 0x04, 0x04, 0x05, 0x05, 0x06,
        0x07, 0x07, 0x08, 0x08, 0x09, 0x0A, 0x0B, 0x0B, 0x0C, 0x0D, 0x0E, 0x0E, 0x0F,
        0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x1A, 0x1B, 0x1C, 0x1D,
        0x1E, 0x1F, 0x20, 0x21, 0x23, 0x24, 0x25, 0x26, 0x28, 0x29, 0x2A, 0x2C, 0x2D,
        0x2E, 0x30, 0x31, 0x33, 0x34, 0x36, 0x37, 0x39, 0x3A, 0x3C, 0x3D, 0x3F, 0x41,
        0x42, 0x44, 0x45, 0x47, 0x49, 0x4A, 0x4C, 0x4D, 0x4F, 0x50, 0x52, 0x53, 0x54,
        0x56, 0x57, 0x58, 0x5A, 0x5B, 0x5C, 0x5E, 0x5F, 0x60, 0x61, 0x63, 0x64, 0x65,
        0x66, 0x67, 0x68, 0x69, 0x6A, 0x6B, 0x6C, 0x6D, 0x6E, 0x6F, 0x70, 0x71, 0x72,
        0x73, 0x74, 0x75, 0x76, 0x76, 0x77, 0x78, 0x79, 0x7A, 0x7A, 0x7B, 0x7C, 0x7D,
        0x7E, 0x7E, 0x7F, 0x7F, 0x80, 0x80, 0x81, 0x81, 0x82, 0x82, 0x83, 0x83, 0x84,
        0x84, 0x85, 0x85, 0x85, 0x86, 0x86, 0x86, 0x86, 0x86,
        ];

    private IEnumerable<Sprite> LiftDownThenUp(int x, int y, int frame)
    {
        y -= 0x10;
        if (y >= 0x80)
        {
            y -= 0x80;
        }

        frame += x;
        frame %= 2 * UpDownLiftOffsets.Length;
        if (frame >= UpDownLiftOffsets.Length)
        {
            frame = (2 * UpDownLiftOffsets.Length) - 1 - frame;
        }

        return Lift(x, y + UpDownLiftOffsets[frame], 5);
    }

    private IEnumerable<Sprite> LiftDown(int x, int y, int frame)
    {
        y += ((frame << 6) / 75) + (x >> 4);
        return Lift(x + 0x0C, y & 0xFF, 5);
    }

    private IEnumerable<Sprite> LiftUp(int x, int y, int frame)
    {
        y -= ((frame << 6) / 75) + (x >> 4);
        return Lift(x + 0x0C, y & 0xFF, 5);
    }

    private IEnumerable<Sprite> ShortLiftDown(int x, int y, int frame)
    {
        y += ((frame << 6) / 75) + (x >> 4);
        x += 12;
        return Lift(x, y & 0xFF, 2).Concat(Lift(x, (y + 0x80) & 0xFF, 2));
    }

    private IEnumerable<Sprite> ShortLiftUp(int x, int y, int frame)
    {
        y -= ((frame << 6) / 75) + (x >> 4);
        x += 12;
        return Lift(x, y & 0xFF, 2).Concat(Lift(x, (y + 0x80) & 0xFF, 2));
    }

    private IEnumerable<Sprite> BalanceRopeLift(int x, int y, int frame)
    {
        return Lift(x, y - 0x0F, 5);
    }

    private IEnumerable<Sprite> BulletBill(int x, int y, int frame)
    {
        var tile = new ChrTile(0x65, 5, LayerPriority.Priority2, TileFlip.Horizontal);
        yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex--;
        yield return new Sprite(x + 8, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex += 0x10;
        yield return new Sprite(x + 8, y + 8, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex++;
        yield return new Sprite(x, y + 8, new SpriteTile(tile, PixelStartIndex));
    }

    private IEnumerable<Sprite> Blooper(int x, int y, int frame)
    {
        // I'm actually kinda proud of this little animation.
        const int descentTime = 60;
        const int ascentTime = 80;
        const int totalTime = ascentTime + descentTime;
        const int distance = 12;
        var relativeFrame = (frame + x + y) % totalTime;
        var isAscending = relativeFrame >= descentTime;
        if (isAscending)
        {
            relativeFrame -= descentTime;
            y += (int)(distance * ((ascentTime - relativeFrame) / ((float)ascentTime)));
        }
        else
        {
            y += (int)(distance * (relativeFrame / (float)descentTime));
        }

        y++;
        var tile = new ChrTile(0xDC, 2, LayerPriority.Priority2, 0);
        yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));

        tile.XFlipped = true;
        yield return new Sprite(x + 8, y, new SpriteTile(tile, PixelStartIndex));

        y += 8;
        if (isAscending)
        {
            tile.TileIndex++;
            tile.XFlipped = false;
            yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));

            tile.XFlipped = true;
            yield return new Sprite(x + 8, y, new SpriteTile(tile, PixelStartIndex));

            y += 8;
            tile.TileIndex++;
            tile.XFlipped = false;
            yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));

            tile.XFlipped = true;
            yield return new Sprite(x + 8, y, new SpriteTile(tile, PixelStartIndex));
        }
        else
        {
            tile.TileIndex = 0xDF;
            tile.XFlipped = false;
            yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));

            tile.XFlipped = true;
            yield return new Sprite(x + 8, y, new SpriteTile(tile, PixelStartIndex));
        }
    }

    private IEnumerable<Sprite> PipePiranhaPlant(int x, int y, int frame)
    {
        return PiranhaPlant(x + 8, y + 8, frame);
    }

    private IEnumerable<Sprite> PiranhaPlant(int x, int y, int frame)
    {
        var index = ((frame + (x >> 1)) & 0x10) == 0 ? 0xE5 : 0xEC;
        var offset = PiranhaPlantOffset(frame - (x >> 2));
        if (offset == 0)
        {
            yield break;
        }

        y -= 8 + offset;
        var tile = new ChrTile(index, 5, LayerPriority.Priority1, 0);
        yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex++;
        yield return new Sprite(x, y + 8, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex--;
        tile.TileFlip = TileFlip.Horizontal;
        x += 8;
        yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex++;
        yield return new Sprite(x, y + 8, new SpriteTile(tile, PixelStartIndex));

        y += 0x10;
        tile.TileIndex = 0xEB;
        tile.PaletteIndex = 2;
        yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileFlip = 0;
        yield return new Sprite(x - 8, y, new SpriteTile(tile, PixelStartIndex));
    }

    private IEnumerable<Sprite> SpringBoard(int x, int y, int frame)
    {
        var tile = new ChrTile(0xF2, 6, LayerPriority.Priority2, 0);
        yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileFlip = TileFlip.Horizontal;
        yield return new Sprite(x + 8, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileFlip = TileFlip.Veritcal;
        yield return new Sprite(x, y + 0x10, new SpriteTile(tile, PixelStartIndex));

        tile.TileFlip = TileFlip.Both;
        yield return new Sprite(x + 8, y + 0x10, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex++;
        tile.TileFlip = 0;
        yield return new Sprite(x, y + 8, new SpriteTile(tile, PixelStartIndex));

        tile.TileFlip = TileFlip.Horizontal;
        yield return new Sprite(x + 8, y + 8, new SpriteTile(tile, PixelStartIndex));
    }

    private IEnumerable<Sprite> FlagPole(int x, int y, int frame)
    {
        y = 0x30;
        x += 8;

        var index = 4 - (((frame + (x >> 1)) / 12 % 3) << 1);

        var tile = new ChrTile(0x120 + index, 5, LayerPriority.Priority2, 0);
        yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex++;
        yield return new Sprite(x + 8, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex += 0x0F;
        yield return new Sprite(x, y + 8, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex++;
        yield return new Sprite(x + 8, y + 8, new SpriteTile(tile, PixelStartIndex));

        // The flag at the castle is also drawn by the flagpole. For this reason, it is
        // imperative that castle be a specific distance from the flagpole.
        x += 0x60;
        y += 0x40;
        index = ((frame + (x >> 1)) >> 2) & 6;
        tile = new ChrTile(0x104 + index, 5, LayerPriority.Priority0, 0);
        yield return new Sprite(x, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex++;
        yield return new Sprite(x + 8, y, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex += 0x0F;
        yield return new Sprite(x, y + 8, new SpriteTile(tile, PixelStartIndex));

        tile.TileIndex++;
        yield return new Sprite(x + 8, y + 8, new SpriteTile(tile, PixelStartIndex));
    }
}
