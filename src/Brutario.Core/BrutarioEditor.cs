// <copyright file="BrutarioEditor.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Brutario.Core;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;

using Brutario.Core.Editors;

using Maseya.Smas.Smb1;
using Maseya.Smas.Smb1.AreaData;
using Maseya.Smas.Smb1.AreaData.HeaderData;
using Maseya.Smas.Smb1.AreaData.ObjectData;
using Maseya.Smas.Smb1.AreaData.SpriteData;
using Maseya.Snes;

using static System.Math;

public class BrutarioEditor : IMainEditor
{
    public const int ScreenCount = 0x20;
    public const int ScreenWidth = 0x10;
    public const int ScreenHeight = 0x10;
    public const int TilemapWidth = ScreenCount * ScreenWidth;
    public const int TileMapLength = TilemapWidth * ScreenHeight;

    private string _path = String.Empty;

    private int _areaNumber;

    private AreaHeader _areaHeader;

    private int _selectedObjectIndex;

    private int _selectedSpriteIndex;

    private int _startX;

    private bool _spriteMode;

    private Player _player;

    private PlayerState _playerState;

    private int _animationFrame;

    private int _saveHistoryIndex;

    private int _currentHistoryIndex;

    private bool _hasUnsavedChanges;

    private UIAreaObjectCommand? _copiedObject;

    private UIAreaSpriteCommand? _copiedSprite;

    public BrutarioEditor()
    {
        Path = String.Empty;
        Player = Player.Mario;
        PlayerState = PlayerState.Big;

        Palette = new Color32BppArgb[0x140];
        PixelData = new byte[GfxData.TotalPixelDataSize];
        Map16Tiles = new Obj16Tile[0x100];
        BG1TileMap = new int[TileMapLength];
        BG2TileMap = new int[TileMapLength];
        BG1 = new ObjTile[TileMapLength * 4];
        BG2 = new ObjTile[TileMapLength * 4];

        ObjectData = [];
        ObjectData.DataReset += (s, e) => OnObjectData_DataReset(e);
        ObjectData.ItemEdited += (s, e) => OnObjectData_ItemEdited(e);
        ObjectData.ItemAdded += (s, e) => OnObjectData_ItemAdded(e);
        ObjectData.ItemRemoved += (s, e) => OnObjectData_ItemRemoved(e);
        ObjectData.DataCleared += (s, e) => OnObjectData_DataCleared(e);

        SpriteData = [];

        UndoFactory = new UndoFactory();
        UndoFactory.Cleared += UndoFactory_Cleared;
        UndoFactory.UndoElementAdded += UndoFactory_UndoElementAdded;
        UndoFactory.UndoComplete += UndoFactory_UndoComplete;
        UndoFactory.RedoComplete += UndoFactory_RedoComplete;
    }

    public event EventHandler? PathChanged;

    public event EventHandler? FileOpened;

    public event EventHandler? FileSaved;

    public event EventHandler? FileClosed;

    public event EventHandler? HasUnsavedChangesChanged;

    public event EventHandler? HistoryCleared;

    public event EventHandler? UndoElementAdded;

    public event EventHandler? UndoComplete;

    public event EventHandler? RedoComplete;

    public event EventHandler? ItemCopied;

    public event EventHandler? CopiedSpriteChanged;

    public event EventHandler? CopiedObjectChanged;

    public event EventHandler? AreaNumberChanged;

    public event EventHandler? AreaLoaded;

    public event EventHandler? AreaHeaderChanged;

    public event EventHandler? SelectedObjectChanged;

    public event EventHandler? SelectedSpriteChanged;

    public event EventHandler? StartXChanged;

    public event EventHandler? SpriteModeChanged;

    public event EventHandler? PlayerChanged;

    public event EventHandler? PlayerStateChanged;

    public event EventHandler? Invalidated;

    public event EventHandler? ObjectData_DataReset;

    public event EventHandler<ObjectEditedEventArgs>? ObjectData_ItemEdited;

    public event EventHandler<ItemAddedEventArgs>? ObjectData_ItemAdded;

    public event EventHandler<ItemAddedEventArgs>? ObjectData_ItemRemoved;

    public event EventHandler? ObjectData_DataCleared;

    public event EventHandler? AnimationFrameChanged;

    public bool EditSelectedObjectEnabled
    {
        get; set;
    }

    public bool EditSelectedSpriteEnabled
    {
        get; set;
    }

    public int SelectedObjectIndex
    {
        get
        {
            return _selectedObjectIndex;
        }

        set
        {
            if (SelectedObjectIndex == value)
            {
                return;
            }

            _selectedObjectIndex = value;
            OnSelectedObjectChanged(EventArgs.Empty);
        }
    }

    public UIAreaObjectCommand SelectedObject
    {
        get
        {
            return ObjectData[SelectedObjectIndex];
        }
    }

    public UIAreaObjectCommand DefaultPreviewObject
    {
        get
        {
            var command = default(AreaObjectCommand);
            command.X = PreferredX & 0x0F;
            command.Y = PreferredY;
            return new UIAreaObjectCommand(command, PreferredX >> 4);
        }
    }

    public int SelectedSpriteIndex
    {
        get
        {
            return _selectedSpriteIndex;
        }

        set
        {
            if (SelectedSpriteIndex == value)
            {
                return;
            }

            _selectedSpriteIndex = value;
            OnSelectedSpriteChanged(EventArgs.Empty);
        }
    }

    public UIAreaSpriteCommand SelectedSprite
    {
        get
        {
            return SpriteData[SelectedSpriteIndex];
        }
    }

    public UIAreaSpriteCommand DefaultPreviewSprite
    {
        get
        {
            var command = default(AreaSpriteCommand);
            command.X = PreferredX & 0x0F;
            command.Y = PreferredY;
            return new UIAreaSpriteCommand(command, PreferredX >> 4);
        }
    }

    public UIAreaObjectCommand? CopiedObject
    {
        get
        {
            return _copiedObject;
        }

        set
        {
            if (CopiedObject == value)
            {
                return;
            }

            _copiedObject = value;
            OnCopiedObjectChanged(EventArgs.Empty);
        }
    }

    public UIAreaSpriteCommand? CopiedSprite
    {
        get
        {
            return _copiedSprite;
        }

        set
        {
            if (CopiedSprite == value)
            {
                return;
            }

            _copiedSprite = value;
            OnCopiedSpriteChanged(EventArgs.Empty);
        }
    }

    public string Path
    {
        get
        {
            return _path;
        }

        private set
        {
            if (Path == value)
            {
                return;
            }

            _path = value;
            OnPathChanged(EventArgs.Empty);
        }
    }

    public bool IsOpen
    {
        get
        {
            return Rom is not null;
        }
    }

    public bool HasUnsavedChanges
    {
        get
        {
            return _hasUnsavedChanges;
        }

        private set
        {
            if (HasUnsavedChanges == value)
            {
                return;
            }

            _hasUnsavedChanges = value;
            OnHasUnsavedChangesChanged(EventArgs.Empty);
        }
    }

    public bool CanUndo
    {
        get
        {
            return UndoFactory.CanUndo;
        }
    }

    public bool CanRedo
    {
        get
        {
            return UndoFactory.CanRedo;
        }
    }

    public int AreaNumber
    {
        get
        {
            return _areaNumber;
        }

        set
        {
            if (GameData is null)
            {
                throw new InvalidOperationException();
            }

            if (AreaNumber == value)
            {
                return;
            }

            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(AreaNumber),
                    "Area number cannot be negative");
            }

            if (!GameData.AreaLoader.IsValidAreaNumberForObjectData(value))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(AreaNumber),
                    "Area number points to an object area index that would be invalid.");
            }

            if (!GameData.AreaLoader.IsValidAreaNumberForSpriteData(value))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(AreaNumber),
                    "Area number points to a sprite area index that would be invalid.");
            }

            SetAreaNumberInternal(
                oldAreaNumber: AreaNumber,
                newAreaNumber: value);
        }
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

            SetAreaHeaderInternal(
                oldAreaHeader: AreaHeader,
                newAreaHeader: value);
        }
    }

    public int StartX
    {
        get
        {
            return _startX;
        }

        set
        {
            if (StartX == value)
            {
                return;
            }

            _startX = value;
            OnStartXChanged(EventArgs.Empty);
        }
    }

    public bool SpriteMode
    {
        get
        {
            return _spriteMode;
        }

        set
        {
            if (SpriteMode == value)
            {
                return;
            }

            _spriteMode = value;
            OnSpriteModeChanged(EventArgs.Empty);
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

    public int AnimationFrame
    {
        get
        {
            return _animationFrame;
        }

        set
        {
            if (AnimationFrame == value)
            {
                return;
            }

            _animationFrame = value;
            OnAnimationFrameChanged(EventArgs.Empty);
        }
    }

    public SortedObjectListEditor ObjectData
    {
        get;
    }

    public SortedSpriteListEditor SpriteData
    {
        get;
    }

    IReadOnlyList<UIAreaObjectCommand> IMainEditor.ObjectData
    {
        get
        {
            return ObjectData;
        }
    }

    IReadOnlyList<UIAreaSpriteCommand> IMainEditor.SpriteData
    {
        get
        {
            return SpriteData;
        }
    }

    private Rom? Rom
    {
        get;
        set;
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

    private int[] BG1TileMap
    {
        get;
    }

    private int[] BG2TileMap
    {
        get;
    }

    private ObjTile[] BG1
    {
        get;
    }

    private ObjTile[] BG2
    {
        get;
    }

    private bool IsAreaLoaded
    {
        get;
        set;
    }

    private UndoFactory UndoFactory
    {
        get;
    }

    private int SaveHistoryIndex
    {
        get
        {
            return _saveHistoryIndex;
        }
        set
        {
            _saveHistoryIndex = value;
            HasUnsavedChanges = SaveHistoryIndex != CurrentHistoryIndex;
        }
    }

    private int CurrentHistoryIndex
    {
        get
        {
            return _currentHistoryIndex;
        }
        set
        {
            _currentHistoryIndex = value;
            HasUnsavedChanges = SaveHistoryIndex != CurrentHistoryIndex;
        }
    }

    private int OldObjectIndex
    {
        get; set;
    }

    private UIAreaObjectCommand OldObject
    {
        get; set;
    }

    private int OldSpriteIndex
    {
        get; set;
    }

    private UIAreaSpriteCommand OldSprite
    {
        get; set;
    }

    private int PreferredX
    {
        get; set;
    }

    private int PreferredY
    {
        get; set;
    }

    private GameData? GameData
    {
        get; set;
    }

    private string? AutoSavePath
    {
        get; set;
    }

    public bool AutoSaveEnabled
    {
        get; set;
    }

    public bool PruneAutoSavesEnabled
    {
        get; set;
    }

    public TimeSpan AutoSaveInterval
    {
        get; set;
    }

    public TimeSpan AutoSaveCutoffAge
    {
        get; set;
    }

    public bool AutoSaveHardCutoff
    {
        get; set;
    }

    private DateTime LastAutoSaveTime
    {
        get; set;
    }

    private byte[]? LastAutoSaveData
    {
        get; set;
    }

    public void Open(string path)
    {
        // We initialize these as locals first so that if either throws, we don't mess
        // up the last valid state of the editor. Making this its own struct/class may
        // be a good idea.
        var rom = new Rom(File.ReadAllBytes(path));
        var gameData = new GameData(rom);

        if (GameData is not null)
        {
            GameData.TilemapLoaderAsm.LoadTileset -= TilemapLoaderAsm_LoadTileset;
        }

        // These should never throw.
        Rom = rom;
        Path = path;
        GameData = gameData;
        GameData.GfxData.ReadStaticData(PixelData);
        GameData.GfxData.ReadAnimationFrame(0, PixelData);
        GameData.Map16Data.ReadStaticTiles(Map16Tiles);
        GameData.TilemapLoaderAsm.LoadTileset += TilemapLoaderAsm_LoadTileset;

        // Internally, these should never throw. However, they call events, which we
        // cannot control. If an event throws, it could mess up our state. But if this
        // is true, then it becomes the responsibility of the event caller to fix this.
        OnFileOpened(EventArgs.Empty);
        SetAreaNumberInternal(
            oldAreaNumber: 0,
            newAreaNumber: GameData.AreaLoader.GetAreaNumber(world: 0, level: 0),
            discardHistory: true);
    }

    private void TilemapLoaderAsm_LoadTileset(object? sender, TilesetEventArgs e)
    {
        // HACK(swr): This tileset gets loaded at the start of W8-4, but not for this
        // area itself.
        if (AreaNumber == 2 && e.Tileset == Tileset.UnderwaterRuinsBackground)
        {
            GameData!.GfxData.ReadTileSet(
                Tileset.CastleBackgroundAndForeground1,
                PixelData);
        }

        GameData!.GfxData.ReadTileSet(e.Tileset, PixelData);
    }

    public void Save()
    {
        SaveAs(Path);
    }

    public void SaveAs(string path)
    {
        if (Rom is null)
        {
            throw new InvalidOperationException();
        }

        WriteToGameData();
        File.WriteAllBytes(path, Rom.GetData());
        Path = path;
        OnFileSaved(EventArgs.Empty);
    }

    public void TryAutoSave()
    {
        if (!AutoSaveEnabled)
        {
            return;
        }

        if (Rom is null)
        {
            throw new InvalidOperationException();
        }

        if (DateTime.Now.ToUniversalTime() - LastAutoSaveTime < AutoSaveInterval)
        {
            return;
        }

        // TODO(swr): This may need to be asynchronous
        WriteToGameData();

        var data = Rom.GetData();
        if (LastAutoSaveData is not null
            && Enumerable.SequenceEqual(data, LastAutoSaveData))
        {
            return;
        }

        AutoSaveHelper.AutoSave(AutoSavePath!, data);
        LastAutoSaveData = data;

        PruneOldAutoSaves();
    }

    private void PruneOldAutoSaves()
    {
        if (!PruneAutoSavesEnabled)
        {
            return;
        }

        if (Rom is null)
        {
            throw new InvalidOperationException();
        }

        AutoSaveHelper.PruneOldAutoSaves(AutoSavePath!, AutoSaveCutoffAge, AutoSaveHardCutoff);
    }

    public void Close()
    {
        Path = String.Empty;
        Rom = null;
        ClearHistory();
        FileClosed?.Invoke(this, EventArgs.Empty);
    }

    public bool IsValidAreaNumber(int areaNumber)
    {
        return GameData is not null
            && GameData.AreaLoader.IsValidAreaNumberForObjectData(areaNumber)
            && GameData.AreaLoader.IsValidAreaNumberForSpriteData(areaNumber);
    }

    public void Undo()
    {
        UndoFactory.Undo();
    }

    public void Redo()
    {
        UndoFactory.Redo();
    }

    public void CutCurrentItem()
    {
        CopyCurrentItem();
        RemoveCurrentItem();
    }

    public void CopyCurrentItem()
    {
        if (SpriteMode)
        {
            CopiedSprite = SelectedSpriteIndex != -1
                ? SelectedSprite
                : throw new InvalidOperationException();
        }
        else
        {
            CopiedObject = SelectedObjectIndex != -1
                ? SelectedObject
                : throw new InvalidOperationException();
        }
    }

    public void Paste()
    {
        Paste(PreferredX, PreferredY);
    }

    public void Paste(int x, int y)
    {
        if (SpriteMode)
        {
            if (CopiedSprite is null)
            {
                throw new InvalidOperationException();
            }

            var sprite = (UIAreaSpriteCommand)CopiedSprite!;
            sprite.X = x;
            sprite.Y = y;
            SelectedSpriteIndex = AddSpriteInternal(sprite);
        }
        else
        {
            if (CopiedObject is null)
            {
                throw new InvalidOperationException();
            }

            var item = (UIAreaObjectCommand)CopiedObject!;
            item.X = x;
            item.Y = y;
            SelectedObjectIndex = AddObjectInternal(item);
        }
    }

    public void AddPreviewObject(UIAreaObjectCommand item)
    {
        // We add an object to the object list, but do not persist its changes
        // to the history. The user may be changing the object properties a lot
        // as they design the object to their liking, and we don't want all of
        // this in the history, just whatever the final product is. There's lots
        // of rooms for bugs in this command, as we change the state of the
        // editor pretty hard. We can't add multiple preview objects at once, we
        // can't switch to sprite, we probably shouldn't allow edits to other
        // things such as header or other area data. If the user decides to go
        // to another area, we need to exit gracefully from preview object mode.
        // Overall, just be careful with this command, as it makes things very
        // different.
        SelectedObjectIndex = AddObjectInternal(item, discardHistory: true);
    }

    public void BeginEditObject()
    {
        OldObjectIndex = SelectedObjectIndex;
        OldObject = SelectedObject;
    }

    public void EditPreviewObject(UIAreaObjectCommand newItem)
    {
        SelectedObjectIndex = EditObjectInternal(
            SelectedObjectIndex, newItem, discardHistory: true);
    }

    public void FinishAddObject(bool commit)
    {
        if (commit)
        {
            PushObjectAdded(SelectedObjectIndex);
        }
        else
        {
            RemoveObjectInternal(SelectedObjectIndex, discardHistory: true);
        }
    }

    public void AddPreviewSprite(UIAreaSpriteCommand item)
    {
        SelectedSpriteIndex = AddSpriteInternal(item, discardHistory: true);
    }

    public void EditPreviewSprite(
        UIAreaSpriteCommand newItem)
    {
        SelectedSpriteIndex = EditSpriteInternal(
            SelectedSpriteIndex, newItem, discardHistory: true);
    }

    public void FinishAddSprite(bool commit)
    {
        if (commit)
        {
            PushSpriteAdded(SelectedSpriteIndex);
        }
        else
        {
            RemoveSpriteInternal(SelectedSpriteIndex, discardHistory: true);
        }
    }

    public void RemoveCurrentItem()
    {
        if (SpriteMode)
        {
            if (SelectedSpriteIndex != -1)
            {
                RemoveSpriteInternal(SelectedSpriteIndex);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
        else
        {
            if (SelectedObjectIndex != -1)
            {
                RemoveObjectInternal(SelectedObjectIndex);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }

    public void RemoveObject(UIAreaObjectCommand item)
    {
        var index = ObjectData.IndexOf(item);
        if (index != -1)
        {
            RemoveObjectInternal(index);
        }
    }

    public void RemoveObjectAt(int index)
    {
        RemoveObjectInternal(index);
    }

    public void RemoveSprite(int index)
    {
        RemoveSpriteInternal(index);
    }

    public void RemoveAllItems()
    {
        if (SpriteMode)
        {
            ClearSprites();
        }
        else
        {
            ClearObjects();
        }
    }

    public void ClearObjects()
    {
        ClearObjectsInternal();
    }

    public void ClearSprites()
    {
        ClearSpritesInternal();
    }

    public byte[] ExportTileMap()
    {
        var data = new byte[0x20 * 0x10 * 0x0D];
        for (var i = 0; i < data.Length; i++)
        {
            data[i] = (byte)BG1TileMap[i + (2 * 0x20 * 0x10)];
        }

        return data;
    }

    public void InitializeSetAreaHeader(AreaHeader newAreaHeader)
    {
        SetAreaHeaderInternal(default, newAreaHeader, discardHistory: true);
    }

    public void FinishSetAreaHeader(AreaHeader oldAreaHeader, bool commit)
    {
        if (!commit)
        {
            SetAreaHeaderInternal(
                oldAreaHeader: AreaHeader,
                newAreaHeader: oldAreaHeader,
                discardHistory: true);
        }
        else if (oldAreaHeader != AreaHeader)
        {
            PushAreaHeaderChanged(
                oldAreaHeader: oldAreaHeader,
                newAreaHeader: AreaHeader);
        }
    }

    public void InitializeEditItem()
    {
        if (SpriteMode)
        {
            InitializeEditSprite();
        }
        else
        {
            InitializeEditObject();
        }
    }

    public void InitializeEditObject()
    {
        // Do not override initialization if one already exists.
        if (EditSelectedObjectEnabled)
        {
            return;
        }

        if (SelectedObjectIndex != -1)
        {
            OldObjectIndex = SelectedObjectIndex;
            OldObject = SelectedObject;
            EditSelectedObjectEnabled = true;
        }
    }

    private void InitializeEditSprite()
    {
        if (EditSelectedSpriteEnabled)
        {
            return;
        }

        if (SelectedSpriteIndex != -1)
        {
            OldSpriteIndex = SelectedSpriteIndex;
            OldSprite = SelectedSprite;
            EditSelectedSpriteEnabled = true;

        }
    }

    public void FinishEditItem(bool commit)
    {
        if (SpriteMode)
        {
            FinishEditSprite(commit);
        }
        else
        {
            FinishEditObject(commit);
        }
    }

    public void FinishEditObject(bool commit)
    {
        if (!EditSelectedObjectEnabled || SelectedObjectIndex == -1 || OldObjectIndex == -1)
        {
            return;
        }

        if (!commit)
        {
            SelectedObjectIndex = EditObjectInternal(
                SelectedObjectIndex,
                OldObject,
                discardHistory: true);
        }
        else if (OldObject != SelectedObject)
        {
            PushObjectEdited(
                oldIndex: OldObjectIndex,
                oldItem: OldObject,
                newIndex: SelectedObjectIndex,
                newItem: SelectedObject);
        }

        OldObjectIndex = -1;
        EditSelectedObjectEnabled = false;
    }

    public void FinishEditSprite(bool commit)
    {
        if (!EditSelectedSpriteEnabled || SelectedSpriteIndex == -1 || OldSpriteIndex == -1)
        {
            return;
        }

        if (!commit)
        {
            SelectedSpriteIndex = EditSpriteInternal(
                SelectedSpriteIndex,
                OldSprite,
                discardHistory: true);
        }
        else if (OldSprite != SelectedSprite)
        {
            PushSpriteEdited(
                oldIndex: OldSpriteIndex,
                oldItem: OldSprite,
                newIndex: SelectedSpriteIndex,
                newItem: SelectedSprite);
        }

        OldSpriteIndex = -1;
        EditSelectedSpriteEnabled = false;
    }

    public void SetSelectedItem(int x, int y)
    {

        if (SpriteMode)
        {
            SelectedSpriteIndex = SpriteData.AtCoords(x, y);
            if (SelectedSpriteIndex == -1)
            {
                PreferredX = x;
                PreferredY = y;
            }
        }
        else
        {
            SelectedObjectIndex = ObjectData.AtCoords(x, y - 2);
            if (SelectedObjectIndex == -1)
            {
                PreferredX = x;
                PreferredY = y - 2;
            }
        }
    }

    /// <summary>
    /// Moves either <see cref="SelectedObject"/> or <see cref="SelectedSprite"/> (based
    /// on <see cref="SpriteMode"/> to the designated coordinates.
    /// </summary>
    /// <param name="x">
    /// The new screen X-coordinate to send the item to.
    /// </param>
    /// <param name="y">
    /// The new screen Y-coordinate to send the item to.
    /// </param>
    public void MoveSelectedItem(int x, int y)
    {
        if (SpriteMode)
        {
            if (!EditSelectedSpriteEnabled)
            {
                return;
            }
        }
        else if (!EditSelectedObjectEnabled)
        {
            return;
        }

        if (SpriteMode && SelectedSpriteIndex != -1)
        {
            var offset = UIAreaSpriteCommand.Offset(SelectedSprite.Command.Code);
            var spriteX = x - offset.Width;
            var spriteY = y - offset.Height;

            spriteX = Clamp(spriteX, 0, 0x200 - 1);
            spriteY = SelectedSprite.Command.Y > 0x0D
                ? SelectedSprite.Command.Y
                : Clamp(spriteY, 0x00, 0x0D);

            var item = SelectedSprite;
            item.X = spriteX + offset.Width;
            item.Y = spriteY + offset.Height;

            SelectedSpriteIndex = EditSpriteInternal(
                SelectedSpriteIndex,
                item,
                discardHistory: true);
        }
        else if (!SpriteMode && SelectedObjectIndex != -1)
        {
            x = Clamp(x, 0, 0x200 - 1);
            y = SelectedObject.Command.IsThreeByteCommand
                ? Clamp(y, 0x00 + 2, 0x0F + 2)
                : (uint)(SelectedObject.Command.Y - 0x0C) <= (0x0E - 0x0C)
                ? 2 + SelectedObject.Command.Y
                : Clamp(y, 0x00 + 2, 0x0B + 2);

            var item = SelectedObject;
            item.X = x;
            item.Y = y - 2;

            SelectedObjectIndex = EditObjectInternal(
                SelectedObjectIndex,
                item,
                discardHistory: true);
        }
    }

    public DrawData GetDrawData(
        int startX,
        Size size,
        Color separatorColor,
        Color passiveColor,
        Color selectColor)
    {
        var rectangles = (SpriteMode ? GetSpriteRectangles() : GetObjectRectangles())
            .ToArray();
        var selectedIndex = SpriteMode
            ? SelectedSpriteIndex
            : SelectedObjectIndex;

        return new DrawData(
            new Color32BppArgb(0xFF, 0, 0, 0),
            Palette,
            PixelData,
            BG1,
            BG2,
            EnumerateSprites(AnimationFrame),
            startX,
            size,
            rectangles,
            selectedIndex,
            separatorColor,
            passiveColor,
            selectColor);
    }

    protected virtual void OnFileOpened(EventArgs e)
    {
        LastAutoSaveTime = DateTime.Now.ToUniversalTime();
        LastAutoSaveData = Rom!.GetData();
        FileOpened?.Invoke(this, e);
    }

    protected virtual void OnFileSaved(EventArgs e)
    {
        SaveHistoryIndex = CurrentHistoryIndex;
        LastAutoSaveTime = DateTime.Now.ToUniversalTime();
        LastAutoSaveData = Rom!.GetData();
        FileSaved?.Invoke(this, e);
    }

    protected virtual void OnAreaLoaded(EventArgs e)
    {
        AreaLoaded?.Invoke(this, e);
    }

    protected virtual void OnHasUnsavedChangesChanged(EventArgs e)
    {
        HasUnsavedChangesChanged?.Invoke(this, e);
    }

    protected virtual void UndoFactory_UndoComplete(object? sender, UndoEventArgs e)
    {
        CurrentHistoryIndex--;
        UndoComplete?.Invoke(this, e);
    }

    protected virtual void UndoFactory_RedoComplete(object? sender, UndoEventArgs e)
    {
        CurrentHistoryIndex++;
        RedoComplete?.Invoke(this, e);
    }

    private void UndoFactory_Cleared(object? sender, EventArgs e)
    {
        HistoryCleared?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void UndoFactory_UndoElementAdded(object? sender, UndoEventArgs e)
    {
        UndoElementAdded?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnItemCopied(EventArgs e)
    {
        ItemCopied?.Invoke(this, e);
    }

    protected virtual void OnPathChanged(EventArgs e)
    {
        var dir = System.IO.Path.GetDirectoryName(Path) ?? String.Empty;
        var name = System.IO.Path.GetFileNameWithoutExtension(Path);
        AutoSavePath = System.IO.Path.Combine(
            dir,
            name + "_AutoSaves",
            System.IO.Path.GetFileName(Path));
        PathChanged?.Invoke(this, e);
    }

    protected virtual void OnCopiedSpriteChanged(EventArgs e)
    {
        CopiedSpriteChanged?.Invoke(this, e);
        OnItemCopied(EventArgs.Empty);
    }

    protected virtual void OnCopiedObjectChanged(EventArgs e)
    {
        CopiedObjectChanged?.Invoke(this, e);
        OnItemCopied(EventArgs.Empty);
    }

    protected virtual void OnAreaNumberChanged(EventArgs e)
    {
        AreaNumberChanged?.Invoke(this, e);
        if (IsOpen)
        {
            ReloadAreaInternal();
            ClearHistory();
        }
    }

    private void ClearHistory()
    {
        UndoFactory.Clear();
        _saveHistoryIndex = 0;
        _currentHistoryIndex = 0;
        HasUnsavedChanges = false;
    }

    protected virtual void OnAreaHeaderChanged(EventArgs e)
    {
        AreaHeaderChanged?.Invoke(this, e);
        if (IsAreaLoaded)
        {
            RenderAreaTilemap();
            Invalidate();
        }
    }

    protected virtual void OnSpriteModeChanged(EventArgs e)
    {
        SpriteModeChanged?.Invoke(this, e);

        if (IsAreaLoaded)
        {
            Invalidate();
        }
    }

    protected virtual void OnStartXChanged(EventArgs e)
    {
        StartXChanged?.Invoke(this, e);

        Invalidated?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnPlayerChanged(EventArgs e)
    {
        PlayerChanged?.Invoke(this, e);
        if (IsAreaLoaded)
        {
            ReloadPalette();
            ReloadTilemap();
            Invalidate();
        }
    }

    protected virtual void OnPlayerStateChanged(EventArgs e)
    {
        PlayerStateChanged?.Invoke(this, e);
        if (IsAreaLoaded)
        {
            ReloadPalette();
            Invalidate();
        }
    }

    protected virtual void OnSelectedObjectChanged(EventArgs e)
    {
        SelectedObjectChanged?.Invoke(this, e);
        if (!SpriteMode && IsAreaLoaded)
        {
            Invalidate();
        }
    }

    protected virtual void OnObjectData_DataReset(EventArgs e)
    {
        ObjectData_DataReset?.Invoke(this, e);
        SelectedObjectIndex = ObjectData.Count > 0 ? 0 : -1;
        OnSelectedObjectChanged(EventArgs.Empty);
        if (IsAreaLoaded)
        {
            RenderAreaTilemap();
            Invalidate();
        }
    }

    protected virtual void OnObjectData_ItemEdited(ObjectEditedEventArgs e)
    {
        ObjectData_ItemEdited?.Invoke(this, e);
        if (IsAreaLoaded)
        {
            RenderAreaTilemap();
            Invalidate();
        }
    }

    protected virtual void OnObjectData_ItemAdded(ItemAddedEventArgs e)
    {
        ObjectData_ItemAdded?.Invoke(this, e);
        if (IsAreaLoaded)
        {
            RenderAreaTilemap();
            Invalidate();
        }
    }

    protected virtual void OnObjectData_ItemRemoved(ItemAddedEventArgs e)
    {
        ObjectData_ItemRemoved?.Invoke(this, e);
        if (SelectedObjectIndex >= ObjectData.Count)
        {
            SelectedObjectIndex = ObjectData.Count - 1;
        }

        RenderAreaTilemap();
        Invalidate();
    }

    protected virtual void OnObjectData_DataCleared(EventArgs e)
    {
        ObjectData_DataCleared?.Invoke(this, e);
        RenderAreaTilemap();
        Invalidate();
    }

    protected virtual void OnSelectedSpriteChanged(EventArgs e)
    {
        SelectedSpriteChanged?.Invoke(this, e);
        if (SpriteMode)
        {
            Invalidate();
        }
    }

    protected virtual void OnAnimationFrameChanged(EventArgs e)
    {
        GameData?.GfxData.ReadAnimationFrame(AnimationFrame, PixelData);

        AnimationFrameChanged?.Invoke(this, e);
        Invalidate();
    }

    protected virtual void OnInvalidated(EventArgs e)
    {
        if (IsAreaLoaded)
        {
            Invalidated?.Invoke(this, e);
        }
    }

    private static int PlayerPose(StartYPosition position)
    {
        return position switch
        {
            StartYPosition.Y00 => 4,
            StartYPosition.Y00FromOtherArea => 4,
            StartYPosition.YB0 => 2,
            StartYPosition.Y50 => 2,
            StartYPosition.Alt1Y00 => 4,
            StartYPosition.Alt2Y00 => 4,
            StartYPosition.PipeIntroYB0 => 0,
            StartYPosition.AltPipeIntroYB0 => 0,
            _ => 6,
        };
    }

    private void WriteToGameData()
    {
        WriteHeader();
        WriteObjectData();
        WriteSpriteData();
        GameData!.WriteToGameData(Rom!);
    }

    private void SetAreaNumberInternal(
        int oldAreaNumber,
        int newAreaNumber,
        bool discardHistory = false)
    {
        _areaNumber = newAreaNumber;
        OnAreaNumberChanged(EventArgs.Empty);
    }

    private int AddObjectInternal(
        UIAreaObjectCommand item,
        bool discardHistory = false)
    {
        var index = ObjectData.Add(item);
        if (!discardHistory)
        {
            PushObjectAdded(index);
        }

        return index;
    }

    private int EditObjectInternal(
        int index,
        UIAreaObjectCommand newItem,
        bool discardHistory = false)
    {
        var oldItem = ObjectData[index];
        var newIndex = ObjectData.Edit(index, newItem);
        if (!discardHistory)
        {
            PushObjectEdited(
                oldIndex: index,
                oldItem: oldItem,
                newIndex: newIndex,
                newItem: newItem);
        }

        return newIndex;
    }

    private void PushObjectAdded(int index)
    {
        var item = ObjectData[index];
        PushUndoAction(
            undo: () => ObjectData.RemoveAt(index),
            redo: () => ObjectData.Add(item));
    }

    private int AddSpriteInternal(
        UIAreaSpriteCommand item,
        bool discardHistory = false)
    {
        var index = SpriteData.Add(item);
        if (!discardHistory)
        {
            PushSpriteAdded(index);
        }

        return index;
    }

    private void PushSpriteAdded(int index)
    {
        var item = SpriteData[index];
        PushUndoAction(
            undo: () => SpriteData.RemoveAt(index),
            redo: () => SpriteData.Add(item));
    }

    private void PushObjectEdited(
        int oldIndex,
        UIAreaObjectCommand oldItem,
        int newIndex,
        UIAreaObjectCommand newItem)
    {
        PushUndoAction(undo, redo);

        void undo()
        {
            var index = ObjectData.Edit(newIndex, oldItem);
            Debug.Assert(index == oldIndex);

            if (SelectedObjectIndex == newIndex)
            {
                SelectedObjectIndex = oldIndex;
            }
            else if (IsAscending(newIndex, SelectedObjectIndex, oldIndex))
            {
                SelectedObjectIndex--;
            }
            else if (IsAscending(oldIndex, SelectedObjectIndex, newIndex))
            {
                SelectedObjectIndex++;
            }
        }

        void redo()
        {
            var index = ObjectData.Edit(oldIndex, newItem);
            Debug.Assert(index == newIndex);

            if (SelectedObjectIndex == oldIndex)
            {
                SelectedObjectIndex = index;
            }
            else if (IsAscending(oldIndex, SelectedObjectIndex, newIndex))
            {
                SelectedObjectIndex--;
            }
            else if (IsAscending(newIndex, SelectedObjectIndex, oldIndex))
            {
                SelectedObjectIndex++;
            }
        }
    }

    private static bool IsAscending<T>(params T[] values)
    {
        return IsAscending(Comparer<T>.Default, values);
    }

    private static bool IsAscending<T>(IComparer<T>? comparer, params T[] values)
    {
        comparer ??= Comparer<T>.Default;

        if (values.Length < 2)
        {
            return true;
        }

        for (var i = 1; i < values.Length; i++)
        {
            if (comparer.Compare(values[i - 1], values[i]) > 0)
            {
                return false;
            }
        }

        return true;
    }

    private int EditSpriteInternal(
        int index,
        UIAreaSpriteCommand newItem,
        bool discardHistory = false)
    {
        var oldItem = SpriteData[index];
        var newIndex = SpriteData.Edit(index, newItem);
        if (!discardHistory)
        {
            PushSpriteEdited(
                oldIndex: index,
                oldItem: oldItem,
                newIndex: newIndex,
                newItem: newItem);
        }

        return newIndex;
    }

    private void PushSpriteEdited(
        int oldIndex,
        UIAreaSpriteCommand oldItem,
        int newIndex,
        UIAreaSpriteCommand newItem)
    {
        PushUndoAction(undo, redo);

        void undo()
        {
            var index = SpriteData.Edit(newIndex, oldItem);
            Debug.Assert(index == oldIndex);

            if (SelectedSpriteIndex == newIndex)
            {
                SelectedSpriteIndex = oldIndex;
            }
            else if (IsAscending(newIndex, SelectedSpriteIndex, oldIndex))
            {
                SelectedSpriteIndex--;
            }
            else if (IsAscending(oldIndex, SelectedSpriteIndex, newIndex))
            {
                SelectedSpriteIndex++;
            }
        }

        void redo()
        {
            var index = SpriteData.Edit(oldIndex, newItem);
            Debug.Assert(index == newIndex);

            if (SelectedSpriteIndex == oldIndex)
            {
                SelectedSpriteIndex = index;
            }
            else if (IsAscending(oldIndex, SelectedSpriteIndex, newIndex))
            {
                SelectedSpriteIndex--;
            }
            else if (IsAscending(newIndex, SelectedSpriteIndex, oldIndex))
            {
                SelectedSpriteIndex++;
            }
        }
    }

    private void RemoveObjectInternal(
        int index,
        bool discardHistory = false)
    {
        var oldItem = ObjectData[index];
        ObjectData.RemoveAt(index);
        if (!discardHistory)
        {
            PushObjectRemoved(index, oldItem);
        }
    }

    private void PushObjectRemoved(int index, UIAreaObjectCommand item)
    {
        PushUndoAction(
            undo: () => ObjectData.Add(item),
            redo: () => ObjectData.RemoveAt(index));

    }

    private void RemoveSpriteInternal(
        int index,
        bool discardHistory = false)
    {
        var oldItem = SpriteData[index];
        SpriteData.RemoveAt(index);
        if (!discardHistory)
        {
            PushSpriteRemoved(index, oldItem);
        }
    }

    private void PushSpriteRemoved(int index, UIAreaSpriteCommand item)
    {
        PushUndoAction(
            undo: () => SpriteData.Add(item),
            redo: () => SpriteData.RemoveAt(index));
    }

    private void ClearObjectsInternal(bool discardHistory = false)
    {
        var items = ObjectData.ToArray();
        ObjectData.Clear();
        SelectedObjectIndex = -1;
        if (!discardHistory)
        {
            PushObjectsCleared(items);
        }
    }

    private void PushObjectsCleared(UIAreaObjectCommand[] items)
    {
        PushUndoAction(
            undo: () => ObjectData.Reset(items),
            redo: ObjectData.Clear);
    }

    private void ClearSpritesInternal(bool discardHistory = false)
    {
        var items = SpriteData.ToArray();
        SpriteData.Clear();
        SelectedSpriteIndex = -1;
        if (!discardHistory)
        {
            PushSpritesCleared(items);
        }
    }

    private void PushSpritesCleared(UIAreaSpriteCommand[] items)
    {
        PushUndoAction(
            undo: () => SpriteData.Reset(items),
            redo: SpriteData.Clear);
    }

    private void SetAreaHeaderInternal(
        AreaHeader oldAreaHeader,
        AreaHeader newAreaHeader,
        bool discardHistory = false)
    {
        _areaHeader = newAreaHeader;
        OnAreaHeaderChanged(EventArgs.Empty);
        if (!discardHistory)
        {
            PushAreaHeaderChanged(oldAreaHeader, newAreaHeader);
        }
    }

    private void SetObjectInternal(
        int index,
        UIAreaObjectCommand newItem,
        bool discardHistory = false)
    {
        var oldItem = ObjectData[index];
        _selectedObjectIndex = ObjectData.Edit(index, newItem);
        if (!discardHistory)
        {
            PushObjectEdited(
                oldIndex: index,
                oldItem: oldItem,
                newIndex: SelectedObjectIndex,
                newItem: newItem);
        }
    }

    private void PushAreaHeaderChanged(
        AreaHeader oldAreaHeader,
        AreaHeader newAreaHeader)
    {
        PushUndoAction(
            undo: () => SetAreaHeaderInternal(
                oldAreaHeader: newAreaHeader,
                newAreaHeader: oldAreaHeader,
                discardHistory: true),
            redo: () => SetAreaHeaderInternal(
                oldAreaHeader: oldAreaHeader,
                newAreaHeader: newAreaHeader,
                discardHistory: true));
    }

    private void PushUndoAction(Action undo, Action redo)
    {
        CurrentHistoryIndex++;
        UndoFactory.Add(undo, redo);
    }

    private void ReloadAreaInternal()
    {
        IsAreaLoaded = false;
        SetAreaHeaderInternal(
            AreaHeader,
            GameData!.AreaLoader.Headers[ObjectAreaIndex],
            discardHistory: true);
        ResetObjectData(discardHistory: true);
        ResetSpriteData(discardHistory: true);
        ReloadPalette();
        ReloadTilemap();
        StartX = 0;
        IsAreaLoaded = true;
        RenderAreaTilemap();
        GameData!.TilemapLoaderAsm.WriteTilemap(BG2);
        OnAreaLoaded(EventArgs.Empty);
        Invalidate();
    }

    private void ReloadPalette()
    {
        // TODO(swr): Change AreaNumber requirement to tileset requirement.
        var isLuigiBonusArea = Player == Player.Luigi
            && (AreaNumber == 0x42 || AreaNumber == 0x2B);
        GameData!.PaletteData.ReadPalette(
            ObjectAreaIndex,
            isLuigiBonusArea,
            Player,
            PlayerState,
            Palette);
    }

    private void ReloadTilemap()
    {
        GameData!.TilemapLoaderAsm.LoadTilemap(AreaType, ObjectAreaIndex, Player);
    }

    private void ResetObjectData(bool discardHistory = false)
    {
        if (!discardHistory)
        {
            throw new NotImplementedException();
        }

        ObjectData.Reset(GameData!.AreaLoader.AreaObjectData[ObjectAreaIndex]);
    }

    private void WriteObjectData()
    {
        GameData!.AreaLoader.AreaObjectData[ObjectAreaIndex] = [.. ObjectData.GetObjectData()];
    }

    private void WriteSpriteData()
    {
        GameData!.AreaLoader.AreaSpriteData[SpriteAreaIndex] = [.. SpriteData.GetSpriteData()];
    }

    private void WriteHeader()
    {
        GameData!.AreaLoader.Headers[ObjectAreaIndex] = AreaHeader;
    }

    private void ResetSpriteData(bool discardHistory = false)
    {
        if (!discardHistory)
        {
            throw new NotImplementedException();
        }

        SpriteData.Reset(GameData!.AreaLoader.AreaSpriteData[SpriteAreaIndex]);
    }

    private void RenderAreaTilemap()
    {
        GameData!.AreaObjectRenderer.RenderTileMap(
            BG1TileMap,
            AreaType,
            AreaHeader,
            [.. ObjectData.GetObjectData()],
            //GameData!.AreaLoader.AreaObjectData[ObjectAreaIndex],
            AreaNumber == 2);
        ReadBGTiles(BG1TileMap, BG1);
    }

    private void RenderBG2Tilemap()
    {
        ReadBGTiles(BG2TileMap, BG2);
    }

    private void ReadBGTiles(int[] tilemap, ObjTile[] bg)
    {
        const int width = 0x200;
        var tiles = tilemap;
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

                bg[destRow + destX] = tile.TopLeft;
                bg[destRow + destX + 1] = tile.TopRight;
                bg[destRow + (width << 1) + destX] = tile.BottomLeft;
                bg[destRow + (width << 1) + destX + 1] = tile.BottomRight;
            }
        }
    }

    private IEnumerable<Rectangle> GetObjectRectangles()
    {
        return ObjectData.Select(p => p.SelectionRectangle);
    }

    private IEnumerable<Rectangle> GetSpriteRectangles()
    {
        return SpriteData.Select(p => p.SelectionRectangle);
    }

    private IEnumerable<Sprite> EnumerateSprites(int frame)
    {
        var areaDataSprites = GameData!.AreaSpriteRenderer.GetSprites(
            [.. SpriteData.GetSpriteData()],
            [.. ObjectData.GetObjectData()],
            frame,
            AreaType,
            showPipePiranhaPlants: AreaNumber != 0x25);

        var tilemapSprites = AreaSpriteRenderer.GetObjectDataSprites(BG1TileMap);

        var playerSprite = AreaSpriteRenderer.GetPlayerSprite(
            x: 0x28,
            AreaHeader.StartYPixel,
            Player,
            PlayerState,
            PlayerPose(AreaHeader.StartYPosition));

        return areaDataSprites.Concat(tilemapSprites).Concat(playerSprite);
    }

    private void Invalidate()
    {
        OnInvalidated(EventArgs.Empty);
    }
}
