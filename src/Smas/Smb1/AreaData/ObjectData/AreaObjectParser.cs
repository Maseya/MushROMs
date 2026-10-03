// <copyright file="AreaObjectParser.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Smas.Smb1.AreaData.ObjectData;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using HeaderData;

public class AreaObjectParser
{
    /// <summary>
    /// The default size of the object data buffer. This field is constant.
    /// </summary>
    public const int DefaultBufferSize = 5;

    public AreaObjectParser(
        AreaObjectRenderer areaObjectRenderer,
        IList<AreaObjectCommand> areaObjectData,
        int bufferSize = DefaultBufferSize)
    {
        AreaObjectRenderer = areaObjectRenderer;

        BufferSize = bufferSize >= 0
            ? bufferSize
            : throw new ArgumentOutOfRangeException(nameof(bufferSize));

        AreaData = new Collection<AreaObjectCommand>(areaObjectData);

        PulleyRopeTileTable = areaObjectRenderer.PulleyRopeTileTable;
        JPipeTilesTable1 = areaObjectRenderer.JPipeTilesTable1;
        JPipeTilesTable2 = areaObjectRenderer.JPipeTilesTable2;
        JPipeTilesTable3 = areaObjectRenderer.JPipeTilesTable3;
        JPipeTilesTable4 = areaObjectRenderer.JPipeTilesTable4;
        PipeTileTable = areaObjectRenderer.PipeTileTable;
        WaterSurfaceTileTable = areaObjectRenderer.WaterSurfaceTileTable;
        CoinRowTileTable = areaObjectRenderer.CoinRowTileTable;
        BrickRowTileTable = areaObjectRenderer.BrickRowTileTable;
        BlockRowTileTable = areaObjectRenderer.BlockRowTileTable;
        SingleTileObjectTable = areaObjectRenderer.SingleTileObjectTable;
        CastleTileTable = areaObjectRenderer.CastleTileTable;
        StoneStairYTable = areaObjectRenderer.BlockStairYTable;
        StoneStairHeightTable = areaObjectRenderer.BlockStairHeightTable;
        JPipeTilesTable5 = areaObjectRenderer.JPipeTilesTable5;
        JPipeTilesTable6 = areaObjectRenderer.JPipeTilesTable6;
        JPipeTilesTable7 = areaObjectRenderer.JPipeTilesTable7;

        LengthBuffer = new int[BufferSize];
        IndexBuffer = new int[BufferSize];
        TreePlatformProperties = new bool[BufferSize];
        MushroomPlatformCenterCoordinate = new int[BufferSize];
        ParserCommands = new Dictionary<ObjectType, Action>()
        {
            { ObjectType.EnterablePipe, Pipe },
            { ObjectType.AreaSpecificPlatform, AreaSpecificPlatform },
            { ObjectType.HorizontalBricks, RowOfBricks },
            { ObjectType.HorizontalBlocks, RowOfBlocks },
            { ObjectType.HorizontalCoins, RowOfCoins },
            { ObjectType.VerticalBricks, ColumnOfBricks },
            { ObjectType.VerticalBlocks, ColumnOfBlocks },
            { ObjectType.UnenterablePipe, Pipe },
            { ObjectType.Hole, Hole },
            { ObjectType.BalanceHorizontalRope, BalanceHorizontalRope },
            { ObjectType.BridgeV7, HighBridge },
            { ObjectType.BridgeV8, MidBridge },
            { ObjectType.BridgeV10, LowBridge },
            { ObjectType.HoleWithWaterOrLava, HoleWithWaterOrLava },
            { ObjectType.HorizontalQuestionBlocksV3, HighRowOfCoinBlocks },
            { ObjectType.HorizontalQuestionBlocksV7, LowRowOfCoinBlocks },
            { ObjectType.QuestionBlockPowerup, ItemBlock },
            { ObjectType.QuestionBlockCoin, ItemBlock },
            { ObjectType.HiddenBlockCoin, ItemBlock },
            { ObjectType.HiddenBlock1UP, AreaTypeBlock },
            { ObjectType.BrickPowerup, AreaTypeBlock },
            { ObjectType.BrickBeanstalk, AreaTypeBlock },
            { ObjectType.BrickStar, AreaTypeBlock },
            { ObjectType.Brick10Coins, AreaTypeBlock },
            { ObjectType.Brick1UP, AreaTypeBlock },
            { ObjectType.SidewaysPipe, SidewaysPipe },
            { ObjectType.UsedBlock, FireBarBlock },
            { ObjectType.Spring, SpringBoard },
            { ObjectType.JPipe, JPipe },
            { ObjectType.AltJPipe, JPipe },
            { ObjectType.FlagPole, FlagPole },
            { ObjectType.AltFlagPole, FlagPole },
            { ObjectType.BowserAxe, BowserAxe },
            { ObjectType.RopeForAxe, RopeForAxe },
            { ObjectType.BowserBridge, BowserBridge },
            { ObjectType.ForegroundSceneryChange, ForegroundChange },
            { ObjectType.TerrainAndBackgroundSceneryChange, TerrainModifier },
            { ObjectType.RopeForLift, RopeForLift },
            { ObjectType.PulleyRope, PulleyRope },
            { ObjectType.Castle, Castle },
            { ObjectType.CastleCeilingCap, CastleCeilingCap },
            { ObjectType.Staircase, Stairs },
            { ObjectType.CastleStairs, CastleDescendingSteps },
            { ObjectType.CastleRectangularCeilingTiles, CastleRectangularCeilingTiles },
            { ObjectType.CastleFloorRightEdge, CastleFloorRightEdge },
            { ObjectType.CastleFloorLeftEdge, CastleFloorLeftEdge },
            { ObjectType.CastleFloorLeftWall, CastleFloorLeftWall },
            { ObjectType.CastleFloorRightWall, CastleFloorRightWall },
            { ObjectType.VerticalSeaBlocks, VerticalSeaBlocks },
            { ObjectType.ExtendableJPipe, ExtendableJPipe },
            { ObjectType.VerticalBalls, VerticalClimbingObject },
        };
    }

    public AreaObjectRenderer AreaObjectRenderer
    {
        get;
    }

    public byte[] PulleyRopeTileTable
    {
        get;
    }

    public byte[] JPipeTilesTable1
    {
        get;
    }

    public byte[] JPipeTilesTable2
    {
        get;
    }

    public byte[] JPipeTilesTable3
    {
        get;
    }

    public byte[] JPipeTilesTable4
    {
        get;
    }

    public byte[] PipeTileTable
    {
        get;
    }

    public byte[] WaterSurfaceTileTable
    {
        get;
    }

    public byte[] CoinRowTileTable
    {
        get;
    }

    public byte[] BrickRowTileTable
    {
        get;
    }

    public byte[] BlockRowTileTable
    {
        get;
    }

    public byte[] SingleTileObjectTable
    {
        get;
    }

    public byte[] CastleTileTable
    {
        get;
    }

    public byte[] StoneStairYTable
    {
        get;
    }

    public byte[] StoneStairHeightTable
    {
        get;
    }

    public byte[] JPipeTilesTable5
    {
        get;
    }

    public byte[] JPipeTilesTable6
    {
        get;
    }

    public byte[] JPipeTilesTable7
    {
        get;
    }

    private AreaType AreaType
    {
        get
        {
            return AreaObjectRenderer.AreaType;
        }
    }

    /// <summary>
    /// Gets the size of the object data buffers. The default value is set to <see cref="DefaultBufferSize"/>.
    /// </summary>
    private int BufferSize
    {
        get;
    }

    /// <summary>
    /// Gets the X register, which represents the current index of the object data
    /// buffer the <see cref="AreaObjectParser"/> is on.
    /// </summary>
    private int CurrentBufferIndex
    {
        get;
        set;
    }

    /// <summary>
    /// Gets the Y register, which represents the current index of the area object data
    /// collection the <see cref="AreaObjectParser"/> is on.
    /// </summary>
    private int AreaDataIndex
    {
        get;
        set;
    }

    /// <summary>
    /// Gets $FA, which represents the pointer to the area object data.
    /// </summary>
    private Collection<AreaObjectCommand> AreaData
    {
        get;
    }

    private AreaObjectCommand CurrentObjectCommand
    {
        get
        {
            return AreaData[AreaDataIndex];
        }

        set
        {
            AreaData[AreaDataIndex] = value;
        }
    }

    private AreaObjectCommand CurrentBufferObject
    {
        get
        {
            return AreaData[CurrentBufferObjectIndex];
        }

        set
        {
            AreaData[CurrentBufferObjectIndex] = value;
        }
    }

    private ObjectType CurrentObjectType
    {
        get
        {
            return CurrentObjectCommand.ObjectType;
        }
    }

    private bool IsPageFlag
    {
        get
        {
            return CurrentObjectCommand.PageFlag;
        }
    }

    private bool IsPageJumpCommand
    {
        get
        {
            return CurrentObjectType == ObjectType.PageSkip;
        }
    }

    private AreaHeader CurrentHeader
    {
        get
        {
            return AreaObjectRenderer.CurrentHeader;
        }

        set
        {
            AreaObjectRenderer.CurrentHeader = value;
        }
    }

    /// <summary>
    /// Gets $06A1, which represents the tile data buffer.
    /// </summary>
    private int[] TileBuffer
    {
        get
        {
            return AreaObjectRenderer.TileBuffer;
        }
    }

    /// <summary>
    /// Gets $1300, which represents the current buffer object's width.
    /// </summary>
    private int[] LengthBuffer
    {
        get;
    }

    /// <summary>
    /// Gets $1300,x.
    /// </summary>
    private int CurrentBufferObjectWidth
    {
        get
        {
            return LengthBuffer[CurrentBufferIndex];
        }

        set
        {
            LengthBuffer[CurrentBufferIndex] = value;
        }
    }

    /// <summary>
    /// Gets a value that determines whether <see cref="CurrentBufferObjectWidth"/> has
    /// been set to a valid value.
    /// </summary>
    private bool CurrentBufferEnabled
    {
        get
        {
            return CurrentBufferObjectWidth >= 0;
        }
    }

    /// <summary>
    /// Gets $1305, which represents the area object data index
    /// </summary>
    private int[] IndexBuffer
    {
        get;
    }

    /// <summary>
    /// Gets $130F, which represents extra properties for the respective buffer object.
    /// </summary>
    private bool[] TreePlatformProperties
    {
        get;
    }

    private bool ObjectHasSpecialProperties
    {
        get
        {
            return TreePlatformProperties[CurrentBufferIndex];
        }

        set
        {
            TreePlatformProperties[CurrentBufferIndex] = value;
        }
    }

    private int[] MushroomPlatformCenterCoordinate
    {
        get;
    }

    private int ObjectSpecialCoordinate
    {
        get
        {
            return MushroomPlatformCenterCoordinate[CurrentBufferIndex];
        }

        set
        {
            MushroomPlatformCenterCoordinate[CurrentBufferIndex] = value;
        }
    }

    /// <summary>
    /// Gets $1305,x
    /// </summary>
    private int CurrentBufferObjectIndex
    {
        get
        {
            return IndexBuffer[CurrentBufferIndex];
        }

        set
        {
            IndexBuffer[CurrentBufferIndex] = value;
        }
    }

    /// <summary>
    /// Gets or sets $0725, which represents the page the renderer is currently on.
    /// </summary>
    private int CurrentRenderingPage
    {
        get
        {
            return AreaObjectRenderer.CurrentRenderingPage;
        }

        set
        {
            AreaObjectRenderer.CurrentRenderingPage = value;
        }
    }

    /// <summary>
    /// Gets or sets $0726, which represents the X-coordinate of the current page the
    /// renderer is currently on.
    /// </summary>
    private int CurrentRenderingPageX
    {
        get
        {
            return AreaObjectRenderer.CurrentRenderingPageX;
        }

        set
        {
            AreaObjectRenderer.CurrentRenderingPageX = value;
        }
    }

    /// <summary>
    /// Gets or sets the full X-coordinate the renderer is currently on.
    /// </summary>
    private int CurrentRenderingX
    {
        get
        {
            return AreaObjectRenderer.CurrentRenderingX;
        }

        set
        {
            AreaObjectRenderer.CurrentRenderingX = value;
        }
    }

    /// <summary>
    /// Gets or sets $072A, which represents the page the area object data starts on.
    /// </summary>
    private int CurrentObjectPage
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets $072B, which represents a flag that determines whether a page jump
    /// command has been activated for this current rendering pass.
    /// </summary>
    private bool IsPageJumpSet
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets $072C, which represents the saved area data index.
    /// </summary>
    private int StoredAreaDataIndex
    {
        get;
        set;
    }

    private bool IsEndOfArea
    {
        get
        {
            return AreaDataIndex == AreaData.Count;
        }
    }

    /// <summary>
    /// Gets $0729, which determines whether the current object is behind the rendering screen.
    /// </summary>
    private bool IsObjectBehindRenderer
    {
        get;
        set;
    }

    private int ObjectParameter
    {
        get;
        set;
    }

    private Dictionary<ObjectType, Action> ParserCommands
    {
        get;
    }

    /// <summary>
    /// Clears all buffers in <see cref="LengthBuffer"/>. This should be called every
    /// time a new screen is going to be rendered.
    /// </summary>
    public void ResetBuffer()
    {
        for (var i = BufferSize; --i >= 0;)
        {
            LengthBuffer[i] = -1;
        }
    }

    /// <summary>
    /// Parses the area data for object at <see cref="CurrentRenderingX"/>. The result
    /// is stored to <see cref="TileBuffer"/>.
    /// </summary>
    public void ParseAreaData()
    {
        do
        {
            LoadBufferData();
        }
        while (IsObjectBehindRenderer);
    }

    private static bool CanWriteTile(int tile, int currentTile)
    {
        return currentTile switch
        {
            0 => true,

            0x1B or
            0x1E or
            0x46 or
            0x4A => false,

            0x56 or
            0x57 => tile != 0x50,

            _ => currentTile <= 0xE7,
        };
    }

    private void LoadBufferData()
    {
        CurrentBufferIndex = BufferSize - 1;

        do
        {
            DecodeBufferData();
        }
        while (MoveToNextBuffer());
    }

    private void DecodeBufferData()
    {
        // Return to the current area index if a buffer changed it at any time.
        AreaDataIndex = StoredAreaDataIndex;

        IsObjectBehindRenderer = false;

        if (IsRenderableObject())
        {
            // If we have an object to render, then let's render it.
            DecodeAreaData();
        }
        else
        {
            // Go to next object if current one cannot be rendered.
            IncrementAreaDataIndex();
        }
    }

    /// <summary>
    /// Gets a value that determines whether we can decode the current object or if we
    /// should instead read the next one.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if there is an object in the buffer that can be rendered
    /// or if there is no more area data to parse; otherwise, <see langword="false"/>.
    /// </returns>
    private bool IsRenderableObject()
    {
        // If we're at the end of the array, attempt to decode any buffer objects. Or,
        // if there is a currently set buffer object, decode it right away.
        if (IsEndOfArea || CurrentBufferEnabled)
        {
            return true;
        }

        // Increment page if we encounter a page flag.
        if (IsPageFlag && !IsPageJumpSet)
        {
            CurrentObjectPage++;
            IsPageJumpSet = true;
        }

        // Update page if we encounter a page skip object.
        if (IsPageJumpCommand && !IsPageJumpSet)
        {
            CurrentObjectPage = CurrentObjectCommand.PrimaryCommand;
            IsPageJumpSet = true;
            return false;
        }

        // Object is behind render when its page before rendering page.
        IsObjectBehindRenderer = CurrentObjectPage < CurrentRenderingPage;

        // Decode area data if object is on or in front of renderer.
        return !IsObjectBehindRenderer;
    }

    /// <summary>
    /// Update the area data index and resets <see cref="IsPageJumpSet"/> for the
    /// next object.
    /// </summary>
    private void IncrementAreaDataIndex()
    {
        StoredAreaDataIndex++;
        IsPageJumpSet = false;
    }

    /// <summary>
    /// Moves to next buffer index and updates the length of the current buffer it is enabled.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if there is another buffer to read; otherwise, <see langword="false"/>.
    /// </returns>
    private bool MoveToNextBuffer()
    {
        if (CurrentBufferEnabled)
        {
            CurrentBufferObjectWidth--;
        }

        return --CurrentBufferIndex >= 0;
    }

    private void DecodeAreaData()
    {
        // Render object in buffer if we have one.
        if (CurrentBufferEnabled)
        {
            AreaDataIndex = CurrentBufferObjectIndex;
        }

        // Do not render if end of area or if we had a page jump command.
        if (IsEndOfArea || IsPageJumpCommand)
        {
            return;
        }

        // Do not render object at wrong coordinate.
        if (!IsObjectAtRenderCoordinate())
        {
            return;
        }

        // Get the render command for the specific object.
        if (ParserCommands.TryGetValue(CurrentObjectType, out var command))
        {
            command();
        }
    }

    private bool IsObjectAtRenderCoordinate()
    {
        // If we're rendering a buffer object, then it is guaranteed that it is at the
        // rendering coordinate.
        if (CurrentBufferEnabled)
        {
            return true;
        }

        // Do not render objects on wrong screen.
        if (CurrentObjectPage != CurrentRenderingPage)
        {
            return false;
        }

        // Render object if it is on the rendering X-coordinate.
        if (CurrentObjectCommand.X == CurrentRenderingPageX)
        {
            // Save this object to the current buffer.
            CurrentBufferObjectIndex = AreaDataIndex;

            // We can now start reading at the next object.
            IncrementAreaDataIndex();
            return true;
        }

        return false;
    }

    private void InitSingleTileRow(int tile)
    {
        _ = TrySetCurrentBufferObjectWidth();
        RenderSingleTile(tile);
    }

    private void RenderSingleTile(int tile)
    {
        RenderTileColumn(tile, CurrentObjectCommand.Y, 0);
    }

    private void RenderTileColumn(int tile)
    {
        (var y, var height) = GetObjectColumnProperties();
        RenderTileColumn(tile, y, height);
    }

    private void RenderTileColumn(int tile, int y, int extraHeight)
    {
        for (; extraHeight-- >= 0 && y < 0x0D; y++)
        {
            if (CanWriteTile(tile, TileBuffer[y]))
            {
                TileBuffer[y] = tile;
            }
        }
    }

    private bool TrySetCurrentBufferObjectWidth()
    {
        return TrySetCurrentBufferObjectWidth(
            CurrentBufferObject.Parameter);
    }

    private bool TrySetCurrentBufferObjectWidth(int width)
    {
        if (CurrentBufferEnabled)
        {
            return false;
        }

        CurrentBufferObjectWidth = width;
        return true;
    }

    private (int y, int parameter) GetObjectColumnProperties()
    {
        return (
            CurrentBufferObject.Y,
            CurrentBufferObject.Parameter);
    }

    private void Pipe()
    {
        _ = TrySetCurrentBufferObjectWidth(1);
        (var y, var height) = GetObjectColumnProperties();
        height &= 7;

        var index = CurrentBufferObjectWidth;
        if (CurrentObjectType == ObjectType.UnenterablePipe)
        {
            index += 4;
        }

        if (height == 0)
        {
            height++;
        }

        TileBuffer[y] = PipeTileTable[index];
        RenderTileColumn(
            PipeTileTable[index + 2],
            y + 1,
            height - 1);
    }

    private void AreaSpecificPlatform()
    {
        switch (CurrentHeader.AreaPlatformType)
        {
        case AreaPlatformType.Trees:
            RenderTreePlatform();
            break;

        case AreaPlatformType.Mushrooms:
            RenderMushroomPlatform();
            break;

        case AreaPlatformType.CloudGround:
            RenderCloudPlatform();
            break;

        case AreaPlatformType.BulletBillTurrets:
            RenderBulletBillTurrets();
            break;
        }
    }

    private void RenderTreePlatform()
    {
        var (y, width) = GetObjectColumnProperties();
        if (CurrentBufferObjectWidth == 0)
        {
            RenderSingleTile(0x1C);
        }
        else if (CurrentBufferObjectWidth > 0)
        {
            RenderTreePlatformColumn(y, CurrentBufferObjectWidth);
        }
        else
        {
            CurrentBufferObjectWidth = width;
            if (CurrentRenderingX == 0)
            {
                RenderTreePlatformColumn(y, width);
            }
            else
            {
                RenderTileColumn(0x1A, y, 0);
            }
        }
    }

    private void RenderTreePlatformColumn(int y, int width)
    {
        TileBuffer[y++] = 0x1B;
        if (--width == 0)
        {
            if (ObjectHasSpecialProperties)
            {
                ObjectHasSpecialProperties = false;
                TileBuffer[y] = 0x47;
                RenderStemColumn(0x4B, y);
            }
            else
            {
                TileBuffer[y] = 0x48;
                RenderStemColumn(0x4C, y);
            }
        }
        else
        {
            if (ObjectHasSpecialProperties)
            {
                TileBuffer[y] = 0x46;
                RenderStemColumn(0x4A, y);
            }
            else
            {
                ObjectHasSpecialProperties = true;
                TileBuffer[y] = 0x45;
                RenderStemColumn(0x49, y);
            }
        }
    }

    private void RenderStemColumn(int tile, int y)
    {
        RenderTileColumn(tile, y + 1, 0x0F);
    }

    private void RenderMushroomPlatform()
    {
        (var y, var width) = GetObjectColumnProperties();
        if (TrySetCurrentBufferObjectWidth(width))
        {
            ObjectSpecialCoordinate =
                CurrentBufferObjectWidth >> 1;

            RenderSingleTile(0x1D);
        }
        else
        {
            if (CurrentBufferObjectWidth != 0)
            {
                RenderSingleTile(0x1E);
                if (CurrentBufferObjectWidth == ObjectSpecialCoordinate)
                {
                    TileBuffer[++y] = 0x4F;
                    RenderStemColumn(0x50, y);
                }
            }
            else
            {
                RenderSingleTile(0x1F);
            }
        }
    }

    private void RenderCloudPlatform()
    {
        // Unverified
        RenderTreePlatform();
    }

    private void RenderBulletBillTurrets()
    {
        (var y, var height) = GetObjectColumnProperties();
        TileBuffer[y++] = 0x6C;
        if (--height >= 0)
        {
            TileBuffer[y++] = 0x6D;
            RenderTileColumn(0x6E, y, --height);
        }
    }

    private void RowOfBricks()
    {
        var index = AreaObjectRenderer.IsCloudPlatform
            ? 4
            : (int)AreaType;

        var tile = BrickRowTileTable[index];
        InitSingleTileRow(tile);
    }

    private void RowOfBlocks()
    {
        var tile = BlockRowTileTable[(int)AreaType];

        InitSingleTileRow(tile);
    }

    private void RowOfCoins()
    {
        var tile = CoinRowTileTable[(int)AreaType];

        InitSingleTileRow(tile);
    }

    private void ColumnOfBricks()
    {
        var tile = BrickRowTileTable[(int)AreaType];

        RenderTileColumn(tile);
    }

    private void ColumnOfBlocks()
    {
        var tile = BlockRowTileTable[(int)AreaType];

        RenderTileColumn(tile);
    }

    private void Hole()
    {
        _ = TrySetCurrentBufferObjectWidth();
        RenderTileColumn(0, 0x08, 0x0F);
    }

    private void BalanceHorizontalRope()
    {
        var index = TrySetCurrentBufferObjectWidth()
            ? 0
            : CurrentBufferObjectWidth != 0
            ? 1
            : 2;

        var tile = PulleyRopeTileTable[index];
        TileBuffer[0] = tile;
    }

    private void HighBridge()
    {
        Bridge(6);
    }

    private void MidBridge()
    {
        Bridge(7);
    }

    private void LowBridge()
    {
        Bridge(9);
    }

    private void Bridge(int y)
    {
        _ = TrySetCurrentBufferObjectWidth();
        if (CurrentBufferObjectWidth != 0)
        {
            if (!ObjectHasSpecialProperties)
            {
                ObjectHasSpecialProperties = true;
                RenderBridge(0x0E);
            }
            else
            {
                RenderBridge(0x0D);
            }
        }
        else
        {
            ObjectHasSpecialProperties = false;
            RenderBridge(0x0F);
        }

        void RenderBridge(int tile)
        {
            TileBuffer[y++] = tile;
            RenderTileColumn(0x6B, y, 0);
        }
    }

    private void HoleWithWaterOrLava()
    {
        _ = TrySetCurrentBufferObjectWidth();
        var y = AreaType == AreaType.Castle ? 0x0B : 0x0A;
        var tile = WaterSurfaceTileTable[(int)AreaType];
        TileBuffer[y++] = tile;

        tile = WaterSurfaceTileTable[4 + ((int)AreaType >> 1)];
        RenderTileColumn(tile, y, 1);
    }

    private void HighRowOfCoinBlocks()
    {
        RowOfCoinBlocks(3);
    }

    private void LowRowOfCoinBlocks()
    {
        RowOfCoinBlocks(7);
    }

    private void RowOfCoinBlocks(int y)
    {
        _ = TrySetCurrentBufferObjectWidth();
        TileBuffer[y] = 0xE7;
    }

    private void ItemBlock()
    {
        var tile = SingleTileObjectTable[CurrentObjectCommand.Parameter];
        TileBuffer[CurrentBufferObject.Y] = tile;
    }

    private void AreaTypeBlock()
    {
        var index = CurrentBufferObject.Parameter;
        if (AreaType == AreaType.Water)
        {
            index += 5;
        }

        var tile = SingleTileObjectTable[index];
        TileBuffer[CurrentBufferObject.Y] = tile;
    }

    private void SidewaysPipe()
    {
        var y = CurrentBufferObject.Y;
        TileBuffer[y++] = 0x75;
        TileBuffer[y] = 0x76;
    }

    private void FireBarBlock()
    {
        RenderSingleTile(0xFC);
    }

    private void SpringBoard()
    {
        var y = CurrentBufferObject.Y;
        TileBuffer[y++] = 0x6F;
        TileBuffer[y] = 0x70;
    }

    private void JPipe()
    {
        var drawVertical = !TrySetCurrentBufferObjectWidth(3);
        var index = CurrentBufferObjectWidth;
        var tile = JPipeTilesTable1[index];
        if (tile != 0)
        {
            RenderTileColumn(tile, 0, 8);
        }
        else
        {
            drawVertical = false;
        }

        TileBuffer[9] = JPipeTilesTable2[index];
        TileBuffer[10] = JPipeTilesTable3[index];
        if (!drawVertical)
        {
            return;
        }

        Array.Clear(TileBuffer, 0, 7);
        TileBuffer[7] = JPipeTilesTable4[index];
    }

    private void FlagPole()
    {
        TileBuffer[0] = 0x28;
        RenderTileColumn(0x29, 1, 8);
        TileBuffer[0x0A] = 0x64;
    }

    private void BowserAxe()
    {
        BowserBridge(0xFD, 6);
    }

    private void RopeForAxe()
    {
        BowserBridge(0x10, 7);
    }

    private void BowserBridge()
    {
        _ = TrySetCurrentBufferObjectWidth(0x0C);
        BowserBridge(0x8D, 8);
    }

    private void BowserBridge(int tile, int y)
    {
        RenderTileColumn(tile, y, 0);
    }

    private void ForegroundChange()
    {
        var header = CurrentHeader;
        header.ForegroundScenery =
            ((ForegroundScenery)CurrentBufferObject.PrimaryCommand).SafeValue();
        CurrentHeader = header;
    }

    private void TerrainModifier()
    {
        var header = CurrentHeader;
        header.TerrainMode = (TerrainMode)
            (CurrentBufferObject.PrimaryCommand & 0x0F);

        header.BackgroundScenery = (BackgroundScenery)
            ((CurrentBufferObject.PrimaryCommand & 0x30) >> 4);

        CurrentHeader = header;
    }

    private void RopeForLift()
    {
        RenderTileColumn(0x40, 0, 0x0F);
    }

    private void PulleyRope()
    {
        RenderTileColumn(0x44, 1, 0x0F);
        (_, var height) = GetObjectColumnProperties();

        RenderTileColumn(0x40, 1, height);
    }

    private void Castle()
    {
        var parameter = CurrentBufferObject.Parameter;
        var y = parameter;
        _ = TrySetCurrentBufferObjectWidth(y == 0 ? 9 : y + 1);
        var index = CurrentBufferObjectWidth;
        for (var i = 0x16; y != 0x0B;)
        {
            // Bounds check not in game.
            if (index >= CastleTileTable.Length || y >= TileBuffer.Length)
            {
                break;
            }

            var tile = CastleTileTable[index];
            TileBuffer[y++] = tile;
            if (i != 0)
            {
                index += 0x0A;
                i--;
            }
        }

        if (CurrentBufferObject.Parameter != 0
            && CurrentBufferObjectWidth == 0)
        {
            TileBuffer[0x0A] = 0;
        }

        if (CurrentRenderingPage != 0 && TileBuffer[0x0B] == 0x56)
        {
            TileBuffer[0x0B] = 0xFB;
        }
    }

    private void CastleCeilingCap()
    {
        (var y, _) = GetObjectColumnProperties();
        _ = TrySetCurrentBufferObjectWidth();
        var firstTile = TileBuffer[0];
        var secondTile = firstTile + (firstTile == 0x65 ? +1 : -1);
        do
        {
            // Check not in the game.
            if (y >= TileBuffer.Length)
            {
                break;
            }

            TileBuffer[y] = (y & 1) == 0 ? firstTile : secondTile;
            y++;
        }
        while (--CurrentBufferObjectWidth >= 0);
    }

    private void Stairs()
    {
        (_, var height) = GetObjectColumnProperties();
        if (TrySetCurrentBufferObjectWidth(height))
        {
            ObjectParameter = 9;
        }

        // Bounds check not in game.
        if (ObjectParameter == 0)
        {
            return;
        }

        int y = StoneStairYTable[--ObjectParameter];
        height = StoneStairHeightTable[ObjectParameter];
        RenderTileColumn(0x64, y, height);
    }

    private void CastleDescendingSteps()
    {
        _ = TrySetCurrentBufferObjectWidth();
        (var y, _) = GetObjectColumnProperties();

        // Custom check. Not in asm code.
        if (y >= 0x0D)
        {
            return;
        }

        if (CurrentBufferObjectWidth == 0)
        {
            TileBuffer[y++] = 0xF3;

            // Custom check. Not in asm code.
            if (y >= 0x0D)
            {
                return;
            }

            if (TileBuffer[y] == 0)
            {
                TileBuffer[y++] = 0xF4;

                // Custom check. Not in asm code.
                if (y >= 0x0D)
                {
                    return;
                }
            }
        }
        else if (TileBuffer[y] == 0)
        {
            TileBuffer[y] = 0xF5;
        }

        do
        {
            if (TileBuffer[y] == 0)
            {
                TileBuffer[y] = 0xF6;
            }
        }
        while (++y != 0x0D);
    }

    private void CastleRectangularCeilingTiles()
    {
        (var y, _) = GetObjectColumnProperties();

        // Custom check. Not in asm code.
        if (y >= 0x0D)
        {
            return;
        }

        _ = TrySetCurrentBufferObjectWidth();
        TileBuffer[y] = 0x67;
        for (y += 2; y < TileBuffer.Length; y += 2)
        {
            if (TileBuffer[y] is not 0x65 and not 0x66)
            {
                break;
            }

            TileBuffer[y] = 0x67;
        }
    }

    private void CastleFloorRightEdge()
    {
        _ = TrySetCurrentBufferObjectWidth();
        (var y, _) = GetObjectColumnProperties();

        // Game engine does not check y bounds here.
        if (y < TileBuffer.Length)
        {
            TileBuffer[y++] = 0xF7;
            while (y != TileBuffer.Length && TileBuffer[y] != 0xEB)
            {
                TileBuffer[y++] = 0xF8;
            }
        }
    }

    private void CastleFloorLeftEdge()
    {
        _ = TrySetCurrentBufferObjectWidth();
        (var y, _) = GetObjectColumnProperties();
        if (y >= TileBuffer.Length)
        {
            return;
        }

        if (TileBuffer[y] != 0xFC)
        {
            TileBuffer[y] = 0xF9;
        }

        while (++y != 0x0D && TileBuffer[y] != 0xF0)
        {
            TileBuffer[y] = 0xFA;
        }
    }

    private void CastleFloorLeftWall()
    {
        _ = TrySetCurrentBufferObjectWidth();
        (var y, _) = GetObjectColumnProperties();

        // Custom check. Not in asm code.
        if (y >= 0x0D)
        {
            return;
        }

        if (CurrentBufferObjectWidth == 0)
        {
            TileBuffer[y++] = 2;
            TileBuffer[y] = 0xED;
        }
        else
        {
            TileBuffer[y] = TileBuffer[y] == 0x68 ? 0xEE : 0xEB;

            // Game engine does not check y bounds here.
            if (++y < TileBuffer.Length)
            {
                TileBuffer[y] = 0xEC;

                // Game engine does not check y bounds here.
                if (++y < TileBuffer.Length)
                {
                    TileBuffer[y] = 0x69;
                }
            }
        }
    }

    private void CastleFloorRightWall()
    {
        (var y, _) = GetObjectColumnProperties();

        // Custom check. Not in asm code.
        if (y >= 0x0D)
        {
            return;
        }

        _ = TrySetCurrentBufferObjectWidth();
        if (CurrentBufferObjectWidth == 0)
        {
            TileBuffer[y] = TileBuffer[y] == 0x68 ? 0xF2 : 0xF0;
            TileBuffer[++y] = 0xF1;
            while (++y != 0x0D)
            {
                TileBuffer[y] = 0x69;
            }
        }
        else
        {
            TileBuffer[y] = 3;
            TileBuffer[y + 1] = 0xEF;
        }
    }

    private void VerticalSeaBlocks()
    {
        _ = TrySetCurrentBufferObjectWidth();
        (var y, _) = GetObjectColumnProperties();
        do
        {
            // Custom check. Not in asm code.
            if (y >= 0x0D)
            {
                CurrentBufferObjectWidth = 0;
                return;
            }

            TileBuffer[y++] = 0x71;
        }
        while (--CurrentBufferObjectWidth >= 0);
    }

    private void ExtendableJPipe()
    {
        _ = TrySetCurrentBufferObjectWidth(3);
        (_, var height) = GetObjectColumnProperties();
        height -= 2;
        var y = height + 1;
        var index = CurrentBufferObjectWidth;
        var tile = JPipeTilesTable5[index];
        if (tile != 0)
        {
            RenderTileColumn(tile, 0, height);
        }

        // Bounds checks not in the game.
        if (y >= 0 && y < TileBuffer.Length)
        {
            tile = JPipeTilesTable6[index];
            TileBuffer[y] = tile;
        }

        if (++y >= 0 && y < TileBuffer.Length)
        {
            tile = JPipeTilesTable7[index];
            TileBuffer[y] = tile;
        }
    }

    private void VerticalClimbingObject()
    {
        (var _, var height) = GetObjectColumnProperties();
        RenderTileColumn(0x77, 2, height);
    }
}
