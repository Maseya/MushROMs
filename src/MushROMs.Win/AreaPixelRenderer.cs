// <copyright file="AreaPixelRenderer.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.Win;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;

using Core;

using Maseya.Snes;

public static class AreaPixelRenderer
{
    private delegate Color32BppArgb PixelTransform(
        Color32BppArgb top,
        Color32BppArgb bottom);

    public static void DrawArea(Graphics graphics, in DrawData drawData)
    {
        DrawArea(
            graphics,
            drawData.BgColor,
            drawData.Palette,
            drawData.PixelData,
            drawData.Bg1,
            drawData.Bg2,
            drawData.Sprites,
            drawData.StartX,
            drawData.Size,
            drawData.Rectangles,
            drawData.SelectedIndex,
            drawData.SeparatorColor,
            drawData.PassiveColor,
            drawData.SelectColor);
    }

    public static void DrawArea(
        Graphics graphics,
        Color32BppArgb bgColor,
        ReadOnlySpan<Color32BppArgb> palette,
        ReadOnlySpan<byte> pixelData,
        ReadOnlySpan<ObjTile> bg1,
        ReadOnlySpan<ObjTile> bg2,
        IEnumerable<Sprite> sprites,
        int startX,
        Size size,
        IEnumerable<Rectangle> rectangles,
        int selectedIndex,
        Color separatorColor,
        Color passiveColor,
        Color selectColor)
    {
        var viewWidth = ((size.Width - 1) / 8) + 1;
        var imageWidth = viewWidth * 8;
        var pixels = RenderPixels(
            bgColor,
            palette,
            pixelData,
            bg1,
            bg2,
            sprites,
            startX,
            size);

        unsafe
        {
            fixed (Color32BppArgb* scan0 = pixels)
            {
                using var image = new Bitmap(
                    imageWidth,
                    size.Height,
                    imageWidth * 4,
                    PixelFormat.Format32bppArgb,
                    (IntPtr)scan0);

                using var imageGraphics = Graphics.FromImage(image);
                DrawPageSeparators(imageGraphics, startX, image.Size, separatorColor);
                DrawItemRectangle(
                    imageGraphics,
                    startX,
                    rectangles,
                    selectedIndex,
                    passiveColor,
                    selectColor);

                graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphics.SmoothingMode = SmoothingMode.None;
                graphics.DrawImage(image, 0, 0, image.Width, image.Height);
                graphics.Flush(FlushIntention.Sync);
            }
        }
    }

    public static void DrawItemRectangle(
        Graphics graphics,
        int startX,
        IEnumerable<Rectangle> rectangles,
        int selectedIndex,
        Color passiveColor,
        Color selectColor)
    {
        using var selectBrush = new SolidBrush(Color.FromArgb(0x7F, selectColor));
        using var passiveBrush = new SolidBrush(Color.FromArgb(0x7F, passiveColor));
        var i = 0;
        foreach (var rect in rectangles)
        {
            graphics.FillRectangle(
                i == selectedIndex ? selectBrush : passiveBrush,
                rect.X - (startX * 8),
                rect.Y,
                rect.Width,
                rect.Height);
            i++;
        }
    }

    public static void DrawPageSeparators(
        Graphics graphics,
        int startX,
        Size size,
        Color color)
    {
        var offset = (startX * 8) & 0xFF;
        using var pen = new Pen(color);
        for (var pageX = 0xFF - offset; pageX < size.Width; pageX += 0x100)
        {
            graphics.DrawLine(pen, pageX, 0, pageX, size.Height);
        }
    }

    public static Bitmap GetImage(
        Color32BppArgb bgColor,
        ReadOnlySpan<Color32BppArgb> palette,
        ReadOnlySpan<byte> pixelData,
        ReadOnlySpan<ObjTile> bg1,
        ReadOnlySpan<ObjTile> bg2,
        IEnumerable<Sprite> sprites,
        int startX,
        Size size)
    {
        var viewWidth = ((size.Width - 1) / 8) + 1;
        var imageWidth = viewWidth * 8;
        var pixels = RenderPixels(
            bgColor,
            palette,
            pixelData,
            bg1,
            bg2,
            sprites,
            startX,
            size);

        unsafe
        {
            fixed (Color32BppArgb* scan0 = pixels)
            {
                return new Bitmap(
                    imageWidth,
                    size.Height,
                    imageWidth * 4,
                    PixelFormat.Format32bppArgb,
                    (IntPtr)scan0);
            }
        }
    }

    public static Color32BppArgb[] RenderPixels(
        Color32BppArgb bgColor,
        ReadOnlySpan<Color32BppArgb> palette,
        ReadOnlySpan<byte> pixelData,
        ReadOnlySpan<ObjTile> bg1,
        ReadOnlySpan<ObjTile> bg2,
        IEnumerable<Sprite> sprites,
        int startX,
        Size size)
    {
        unsafe
        {
            fixed (Color32BppArgb* ptrPalette = palette)
            fixed (byte* ptrPixelData = pixelData)
            fixed (ObjTile* ptrBg1 = bg1)
            fixed (ObjTile* ptrBg2 = bg2)
            {
                // This is DEFINITELY bad practice. Spans are ref structs and should
                // not be passed to asynchronous functions as they keep the value alive
                // longer than it should be. However, the async methods are all
                // finished within this function call, the pointers never leave scope.
                // Ideally, these values should be allowed to be ref structs, but the
                // language is limiting us, so we need to cheat a little right now.
                var asyncPtrPalette = ptrPalette;
                var asyncPtrPixelData = ptrPixelData;
                var asyncPtrBg1 = ptrBg1;
                var asyncPtrBg2 = ptrBg2;

                var viewWidth = ((size.Width - 1) / 8) + 1;
                var viewHeight = (size.Height / 8) - 6;
                var imageWidth = viewWidth * 8;

                var result = new Color32BppArgb[imageWidth * size.Height];

                sprites = sprites.Where(IsInFrame);

                // Set the BG color to the area.
                _ = Parallel.For(
                    fromInclusive: 0,
                    toExclusive: result.Length,
                    body: i => result[i] = bgColor);

                _ = Parallel.ForEach(
                    source: sprites.Where(
                        sprite => HasLayer(sprite, LayerPriority.Priority0)),
                    body: sprite => RenderSprite(sprite));

                _ = Parallel.For(
                    fromInclusive: 0,
                    toExclusive: viewHeight,
                    body: row => RenderRow(asyncPtrBg2, row, startX >> 1, LayerPriority.Priority0));

                // HACK. P1 sprites should render before BG1P0. I'm doing this to not
                // deal with subscreens for piranha plant rendering.
                _ = Parallel.ForEach(
                    source: sprites.Where(
                        sprite => HasLayer(sprite, LayerPriority.Priority1)),
                    body: sprite => RenderSprite(sprite));

                _ = Parallel.For(
                    fromInclusive: 0,
                    toExclusive: viewHeight,
                    body: row => RenderRow(asyncPtrBg1, row, startX, LayerPriority.Priority0));

                _ = Parallel.ForEach(
                    source: sprites.Where(
                        sprite => HasLayer(sprite, LayerPriority.Priority2)),
                    body: sprite => RenderSprite(sprite));

                _ = Parallel.For(
                    fromInclusive: 0,
                    toExclusive: viewHeight,
                    body: row => RenderRow(asyncPtrBg2, row, startX >> 1, LayerPriority.Priority1));

                // HACK(swr). This renders hidden blocks in front of BG2.
                _ = Parallel.ForEach(
                    source: sprites.Where(
                        sprite => sprite.Tile.Priority == -1),
                    body: sprite => RenderSprite(sprite));

                _ = Parallel.For(
                    fromInclusive: 0,
                    toExclusive: viewHeight,
                    body: row => RenderRow(asyncPtrBg1, row, startX, LayerPriority.Priority1));

                _ = Parallel.ForEach(
                    source: sprites.Where(
                        sprite => HasLayer(sprite, LayerPriority.Priority3)),
                    body: sprite => RenderSprite(sprite));

                // TODO(swr): Why do I have this??
                _ = Parallel.ForEach(
                    source: sprites.Where(
                        sprite => HasLayer(sprite, (LayerPriority)4)),
                    body: sprite => RenderSprite(sprite));

                return result;

                void RenderRow(
                    ObjTile* tilemap,
                    int row,
                    int xOffset,
                    LayerPriority layerPriority)
                {
                    var rowIndex = Math.Min(row, 0x1C + (row & 1)) * 0x400;
                    var pixelRow = row * 8 * imageWidth;
                    for (var column = 0; column < viewWidth; column++, pixelRow += 8)
                    {
                        var tile8 = tilemap[rowIndex + column + xOffset];
                        if (tile8.Priority != layerPriority)
                        {
                            continue;
                        }

                        var xFlip = tile8.XFlipped ? 7 : 0;
                        var yFlip = tile8.YFlipped ? 7 : 0;
                        var paletteIndex = tile8.PaletteIndex * 0x10;
                        var pixelIndex = tile8.TileIndex * 0x40;

                        for (var y = 0; y < 8; y++, pixelIndex += 8)
                        {
                            var index = pixelRow + ((y ^ yFlip) * imageWidth);
                            for (var x = 0; x < 8; x++, index++)
                            {
                                var pixel = asyncPtrPixelData[
                                    pixelIndex + (x ^ xFlip)];

                                if (pixel != 0)
                                {
                                    var color = asyncPtrPalette[paletteIndex + pixel];
                                    result[index] = color;
                                }
                            }
                        }
                    }
                }

                bool IsInFrame(Sprite sprite)
                {
                    return (uint)(sprite.X - (startX * 8) + 8) <= (uint)imageWidth
                        && (uint)sprite.Y < (uint)(size.Height - 8);
                }

                bool HasLayer(Sprite sprite, LayerPriority layerPriority)
                {
                    return (sprite.Tile.Priority - 2) / 3 == (int)layerPriority;
                }

                void RenderSprite(Sprite sprite)
                {
                    var spriteX = sprite.X - (startX * 8);
                    var firstX = spriteX >= 0 ? 0 : -spriteX;

                    var tile8 = sprite.Tile;

                    var xFlip = tile8.FlipX ? 7 : 0;
                    var yFlip = tile8.FlipY ? 7 : 0;
                    var paletteIndex = tile8.PaletteIndex * 0x10;
                    var pixelIndex = tile8.TileIndex * 0x40;
                    var func = TransformPixel(sprite.TileProperties);
                    var pixelRow = (sprite.Y * imageWidth) + sprite.X - (startX * 8);

                    for (var y = 0; y < 8; y++, pixelIndex += 8)
                    {
                        var index = pixelRow + ((y ^ yFlip) * imageWidth);
                        for (var x = firstX; x < 8; x++, index++)
                        {
                            var pixel = asyncPtrPixelData[pixelIndex + (x ^ xFlip)];

                            if (pixel != 0)
                            {
                                result[index] = func(
                                    asyncPtrPalette[paletteIndex + pixel],
                                    result[index]);
                            }
                        }
                    }
                }
            }
        }
    }

    private static PixelTransform TransformPixel(TileProperties tileProperties)
    {
        return tileProperties switch
        {
            TileProperties.Invert => (pixel, bottom) =>
            {
                pixel.R ^= 0xFF;
                pixel.G ^= 0xFF;
                pixel.B ^= 0xFF;
                return pixel;
            }
            ,
            TileProperties.Red => (pixel, bottom) =>
            {
                pixel.R = 0xFF;
                return pixel;
            }
            ,
            TileProperties.Green => (pixel, bottom) =>
            {
                pixel.G = 0xFF;
                return pixel;
            }
            ,
            TileProperties.Blue => (pixel, bottom) =>
            {
                pixel.B = 0xFF;
                return pixel;
            }
            ,
            TileProperties.Yellow => (pixel, bottom) =>
            {
                pixel.R = 0xFF;
                pixel.G = 0xFF;
                return pixel;
            }

            ,
            TileProperties.Magenta => (pixel, bottom) =>
            {
                pixel.R = 0xFF;
                pixel.B = 0xFF;
                return pixel;
            }
            ,
            TileProperties.Cyan => (pixel, bottom) =>
            {
                pixel.G = 0xFF;
                pixel.B = 0xFF;
                return pixel;
            }
            ,
            TileProperties.Transparent => (pixel, bottom) =>
            {
                pixel.R = (byte)((pixel.R + (1 * bottom.R)) >> 1);
                pixel.G = (byte)((pixel.G + (1 * bottom.G)) >> 1);
                pixel.B = (byte)((pixel.B + (1 * bottom.B)) >> 1);
                return pixel;
            }
            ,
            _ => (pixel, bottom) => pixel,
        };
    }
}
