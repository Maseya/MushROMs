// <copyright file="AreaEditor.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.Core;

using System;

using Maseya.Smas.Smb1;
using Maseya.Smas.Smb1.AreaData;
using Maseya.Smas.Smb1.AreaData.HeaderData;
using Maseya.Snes;

public class AreaEditor
{
    public const int PageCount = 0x20;
    public const int PageWidth = 0x10;
    public const int TilemapHeight = 0x10;
    public const int TilemapWidth = PageCount * PageWidth;
    public const int TileMapLength = TilemapWidth * TilemapHeight;

    private AreaHeader _areaHeader;

    private Player _player;

    private PlayerState _playerState;

    public AreaEditor(
            GameData gameData,
        int areaNumber)
    {
        GameData = gameData;
        AreaNumber = areaNumber;

        Palette = new Color32BppArgb[0x140];
        PixelData = new byte[GfxData.TotalPixelDataSize];
        Map16Tiles = new Obj16Tile[0x100];
        TileMap = new int[TileMapLength];
        BG1 = new ObjTile[TileMapLength * 4];

        UndoFactory = new UndoFactory();

        GameData.GfxData.ReadStaticData(PixelData);
        GameData.Map16Data.ReadStaticTiles(Map16Tiles);

        AreaHeader = GameData.AreaLoader.Headers[ObjectAreaIndex];
        ObjectDataInternal = new SortedObjectListEditor(
            GameData.AreaLoader.AreaObjectData[ObjectAreaIndex]);
        SpriteDataInternal = new SortedSpriteListEditor(
            GameData.AreaLoader.AreaSpriteData[SpriteAreaIndex]);

        ReloadPaletteInternal();
        GameData!.TilemapLoaderAsm.LoadTilemap(AreaType, ObjectAreaIndex, Player);

        RenderAreaTilemapInternal();

    }

    public event EventHandler? AreaHeaderChanged;

    public event EventHandler? PlayerChanged;

    public event EventHandler? PlayerStateChanged;

    public PlayerState PlayerState
    {
        get
        {
            return _playerState;
        }

        set
        {
            if (PlayerState == value)
            {
                return;
            }

            _playerState = value;
            OnPlayerStateChanged(EventArgs.Empty);
        }
    }

    public Player Player
    {
        get
        {
            return _player;
        }

        set
        {
            if (Player == value)
            {
                return;
            }

            _player = value;
            OnPlayerChanged(EventArgs.Empty);
        }
    }

    public int AreaNumber
    {
        get;
    }

    public AreaHeader AreaHeader
    {
        get
        {
            return _areaHeader;
        }

        set
        {
            if (AreaHeader == value)
            {
                return;
            }

            _areaHeader = value;
            OnAreaHeaderChanged(EventArgs.Empty);
        }
    }

    private SortedObjectListEditor ObjectDataInternal
    {
        get;
    }

    private SortedSpriteListEditor SpriteDataInternal
    {
        get;
    }

    private Color32BppArgb[] Palette
    {
        get;
    }

    private byte[] PixelData
    {
        get;
    }

    private Obj16Tile[] Map16Tiles
    {
        get;
    }

    private int[] TileMap
    {
        get;
    }

    private ObjTile[] BG1
    {
        get;
    }

    private AreaType AreaType
    {
        get
        {
            return (AreaType)((AreaNumber & 0x7F) >> 5);
        }
    }

    private int ObjectAreaIndex
    {
        get
        {
            return GameData!.AreaLoader.GetObjectAreaIndex(AreaNumber);
        }
    }

    private int SpriteAreaIndex
    {
        get
        {
            return GameData!.AreaLoader.GetSpriteAreaIndex(AreaNumber);
        }
    }

    private UndoFactory UndoFactory
    {
        get;
    }

    private int SaveHistoryIndex
    {
        get;
        set;
    }

    private int CurrentHistoryIndex
    {
        get;
        set;
    }

    private bool IsAreaLoaded
    {
        get; set;
    }

    private GameData GameData
    {
        get;
    }

    protected virtual void OnPlayerStateChanged(EventArgs e)
    {
        PlayerStateChanged?.Invoke(this, e);
    }

    protected virtual void OnPlayerChanged(EventArgs e)
    {
        PlayerChanged?.Invoke(this, e);
    }

    protected virtual void OnAreaHeaderChanged(EventArgs e)
    {
        AreaHeaderChanged?.Invoke(this, e);
    }

    private void ReloadPaletteInternal()
    {
        // TODO(swr): Change AreaNumber requirement to tileset requirement.
        var isLuigiBonusArea = Player == Player.Luigi
            && (AreaNumber == 0x42 || AreaNumber == 0x2B);
        GameData.PaletteData.ReadPalette(
            ObjectAreaIndex,
            isLuigiBonusArea,
            Player,
            PlayerState,
            Palette);
    }

    private void RenderAreaTilemapInternal()
    {
        GameData!.AreaObjectRenderer.RenderTileMap(
            TileMap,
            AreaType,
            AreaHeader,
            [.. ObjectDataInternal.GetObjectData()],
            AreaNumber == 2);
        ReadBG1Tiles();
    }

    private void ReadBG1Tiles()
    {
        const int width = 0x200;
        var tiles = TileMap;
        var height = tiles.Length / width;
        for (var y = 0; y < height; y++)
        {
            var srcRow = y * width;
            var destRow = srcRow << 2;
            for (var srcX = 0; srcX < width; srcX++)
            {
                var destX = srcX << 1;
                var index = srcRow + srcX;
                var tileIndex = (byte)tiles[index];
                var tile = Map16Tiles[tileIndex];
                if (tileIndex is 0x56 or 0x57)
                {
                    if (srcX > 0 && ((byte)tiles[index - 1]) == 0)
                    {
                        tile.TopLeft += 4;
                        tile.BottomLeft += 4;
                    }

                    if (srcX + 1 < width && ((byte)tiles[index + 1]) == 0)
                    {
                        tile.TopRight += 4;
                        tile.BottomRight += 4;
                    }
                }

                BG1[destRow + destX] = tile.TopLeft;
                BG1[destRow + destX + 1] = tile.TopRight;
                BG1[destRow + (width << 1) + destX] = tile.BottomLeft;
                BG1[destRow + (width << 1) + destX + 1] = tile.BottomRight;
            }
        }
    }
}
