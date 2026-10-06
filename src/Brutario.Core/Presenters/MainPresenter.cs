// <copyright file="MainPresenter.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Brutario.Core.Presenters;

using System;
using System.Diagnostics;
using System.Drawing;
using System.Text;

using Brutario.Core.Editors;
using Brutario.Core.Views;

using Maseya.Smas.Smb1;

public class MainPresenter
{
    public MainPresenter(
        BrutarioEditor mainEditor,
        IMainView mainView,
        IObjectListView objectListView,
        IExceptionView exceptionHelper,
        IFileNameSelector openFileNameSelector,
        IFileNameSelector saveFileNameSelector,
        ISaveOnClosePrompt saveOnClosePrompt,
        IHeaderEditorView headerEditor,
        IObjectEditorView objectEditor,
        ISpriteEditorView spriteEditor)
    {
        MainEditor = mainEditor;
        MainView = mainView;
        //ObjectListView = objectListView;
        ExceptionHelper = exceptionHelper;
        OpenFileNameSelector = openFileNameSelector;
        SaveFileNameSelector = saveFileNameSelector;
        SaveOnClosePrompt = saveOnClosePrompt;
        HeaderEditorView = headerEditor;
        ObjectEditorView = objectEditor;
        SpriteEditorView = spriteEditor;

        MainEditor.PathChanged += MainEditor_PathChanged;
        MainEditor.FileOpened += MainEditor_FileOpened;
        MainEditor.FileSaved += MainEditor_FileSaved;
        MainEditor.FileClosed += MainEditor_FileClosed;
        MainEditor.HasUnsavedChangesChanged += MainEditor_HasUnsavedChangesChanged;
        MainEditor.UndoComplete += MainEditor_UndoComplete;
        MainEditor.RedoComplete += MainEditor_RedoComplete;
        MainEditor.ItemCopied += MainEditor_ItemCopied;
        MainEditor.SpriteModeChanged += MainEditor_SpriteModeChanged;
        MainEditor.PlayerChanged += MainEditor_PlayerChanged;
        MainEditor.PlayerStateChanged += MainEditor_PlayerStateChanged;
        MainEditor.Invalidated += MainEditor_Invalidated;
        MainEditor.AreaNumberChanged += MainEditor_AreaNumberChanged;
        MainEditor.AreaLoaded += MainEditor_AreaLoaded;
        MainEditor.StartXChanged += MainEditor_StartXChanged;
        MainEditor.HistoryCleared += MainEditor_HistoryCleared;
        MainEditor.UndoElementAdded += MainEditor_UndoElementAdded;
        MainEditor.AreaHeaderChanged += MainEditor_AreaHeaderChanged;
        MainEditor.SelectedObjectChanged +=
            MainEditor_ObjectDataSelectedIndexChanged;
        MainEditor.SelectedSpriteChanged +=
            MainEditor_SpriteDataSelectedIndexChanged;
        MainEditor.ObjectData_DataReset += MainEditor_ObjectData_DataReset;
        MainEditor.ObjectData_ItemAdded += MainEditor_ObjectData_ItemAdded;
        MainEditor.ObjectData_ItemEdited += MainEditor_ObjectData_ItemEdited;
        MainEditor.ObjectData_ItemRemoved += MainEditor_ObjectData_ItemRemoved;
        MainEditor.ObjectData_DataCleared += MainEditor_ObjectData_DataCleared;
        MainEditor.AnimationFrameChanged += MainEditor_AnimationFrameChanged;

        HeaderEditorView.AreaHeaderChanged += HeaderEditor_AreaHeaderChanged;

        /*
        ObjectListView.AddItem_Click += ObjectListView_AddItem_Click;
        ObjectListView.DeleteItem_Click += ObjectListView_DeleteItem_Click;
        ObjectListView.ClearItems_Click += ObjectListView_ClearItems_Click;
        */

        MainView.Player = MainEditor.Player;
        MainView.PlayerState = MainEditor.PlayerState;
    }

    public bool AutoSaveEnabled
    {
        get
        {
            return MainEditor.AutoSaveEnabled;
        }

        set
        {
            MainEditor.AutoSaveEnabled = value;
        }
    }

    public bool PruneAutoSavesEnabled
    {
        get
        {
            return MainEditor.PruneAutoSavesEnabled;
        }

        set
        {
            MainEditor.PruneAutoSavesEnabled = value;
        }
    }

    public TimeSpan AutoSaveInterval
    {
        get
        {
            return MainEditor.AutoSaveInterval;
        }

        set
        {
            MainEditor.AutoSaveInterval = value;
        }
    }

    public TimeSpan AutoSaveCutoffAge
    {
        get
        {
            return MainEditor.AutoSaveCutoffAge;
        }

        set
        {
            MainEditor.AutoSaveCutoffAge = value;
        }
    }

    public bool AutoSaveHardCutoff
    {
        get
        {
            return MainEditor.AutoSaveHardCutoff;
        }

        set
        {
            MainEditor.AutoSaveHardCutoff = value;
        }
    }

    private BrutarioEditor MainEditor
    {
        get;
    }

    private IMainView MainView
    {
        get;
    }

    private IExceptionView ExceptionHelper
    {
        get;
    }

    private IFileNameSelector OpenFileNameSelector
    {
        get;
    }

    private IFileNameSelector SaveFileNameSelector
    {
        get;
    }

    private ISaveOnClosePrompt SaveOnClosePrompt
    {
        get;
    }

    private IHeaderEditorView HeaderEditorView
    {
        get;
    }

    private IObjectListView ObjectListView
    {
        get;
    }

    private IObjectEditorView ObjectEditorView
    {
        get;
    }

    private ISpriteEditorView SpriteEditorView
    {
        get;
    }

    public void Open()
    {
        if (OpenFileNameSelector.Prompt() == PromptResult.Yes)
        {
            Open(OpenFileNameSelector.FileName!);
        }
    }

    public void Open(string path)
    {
        if (!CleanupBeforeClose())
        {
            return;
        }

        try
        {
            MainEditor.Open(path);
        }
        catch
        {
            ExceptionHelper.Show("Failed to load ROM.");
        }
    }

    public void Save()
    {
        while (true)
        {
            try
            {
                MainEditor.Save();
                break;
            }
            catch
            {
                if (!ExceptionHelper.ShowAndPromptRetry("Failed to save ROM"))
                {
                    break;
                }
            }
        }
    }

    public void SaveAs()
    {
        if (SaveFileNameSelector.Prompt() == PromptResult.Yes)
        {
            SaveAs(SaveFileNameSelector.FileName!);
        }
    }

    public void SaveAs(string path)
    {
        while (true)
        {
            try
            {
                MainEditor.SaveAs(path);
                break;
            }
            catch
            {
                if (!ExceptionHelper.ShowAndPromptRetry("Failed to save ROM"))
                {
                    break;
                }
            }
        }
    }

    public void AutoSave()
    {
        if (MainEditor.IsOpen)
        {
            MainEditor.TryAutoSave();
        }
    }

    public bool Close()
    {
        return CloseInternal();
    }

    public void Undo()
    {
        MainEditor.Undo();
    }

    public void Redo()
    {
        MainEditor.Redo();
    }

    public void CutCurrentItem()
    {
        MainEditor.CutCurrentItem();
    }

    public void CopyCurrentItem()
    {
        MainEditor.CopyCurrentItem();
    }

    public void Paste()
    {
        MainEditor.Paste();
    }

    public void AddItem()
    {
        if (MainView.SpriteMode)
        {
            AddSprite();
        }
        else
        {
            AddObject();
        }
    }

    public void RemoveCurrentItem()
    {
        MainEditor.RemoveCurrentItem();
    }

    public void DeleteAllItems()
    {
        MainEditor.RemoveAllItems();
    }

    public void LoadAreaByLevel()
    {
        throw new NotImplementedException();
    }

    public bool IsValidAreaNumber(int areaNumber)
    {
        return MainEditor.IsValidAreaNumber(areaNumber);
    }

    public void JumpToArea(int areaNumber)
    {
        throw new NotImplementedException();
    }

    public void ExportTileData()
    {
        throw new NotImplementedException();
    }

    public void EditHeader()
    {
        var oldHeader = MainEditor.AreaHeader;
        HeaderEditorView.AreaHeader = MainEditor.AreaHeader;

        var result = HeaderEditorView.Prompt();
        MainEditor.FinishSetAreaHeader(oldHeader, commit: result);
    }

    public void ToggleSpritMode(bool enabled)
    {
        MainEditor.SpriteMode = enabled;
    }

    public void SetPlayer(Player player)
    {
        MainEditor.Player = player;
    }

    public void SetPlayerState(PlayerState playerState)
    {
        MainEditor.PlayerState = playerState;
    }

    public void SetAreaNumber(int areaNumber)
    {
        if (!CleanupBeforeClose())
        {
            return;
        }

        MainEditor.AreaNumber = areaNumber;
    }

    public void SetStartX(int startX)
    {
        MainEditor.StartX = startX;
    }

    public void SetSelectedItem(int x, int y)
    {
        MainEditor.SetSelectedItem(x, y);
    }

    public void EditSelectedItem()
    {
        if (MainEditor.SpriteMode)
        {
            EditSelectedSprite();
        }
        else
        {
            EditSelectedObject();
        }
    }

    public void EditSelectedObject()
    {
        if (MainEditor.SelectedObjectIndex == -1)
        {
            return;
        }

        ObjectEditorView.UIAreaObjectCommand = MainEditor.SelectedObject;
        ObjectEditorView.AreaObjectCommandChanged += ObjectEditor_ItemChanged;

        var commit = ObjectEditorView.PromptConfirm();
        MainEditor.FinishEditObject(commit);

        ObjectEditorView.AreaObjectCommandChanged -= ObjectEditor_ItemChanged;

        void ObjectEditor_ItemChanged(object? sender, EventArgs e)
        {
            MainEditor.EditPreviewObject(
                ObjectEditorView.UIAreaObjectCommand);
        }
    }

    public void EditSelectedSprite()
    {
        if (MainEditor.SelectedSpriteIndex == -1)
        {
            return;
        }

        SpriteEditorView.AreaSpriteCommand = MainEditor.SelectedSprite;
        SpriteEditorView.AreaSpriteCommandChanged += ItemChanged;

        var commit = SpriteEditorView.PromptConfirm();
        MainEditor.FinishEditSprite(commit);

        void ItemChanged(object? sender, EventArgs e)
        {
            MainEditor.EditPreviewSprite(
                SpriteEditorView.AreaSpriteCommand);
        }
    }

    public void InitializeMoveItem()
    {
        MainEditor.InitializeEditItem();
    }

    public void InitializeMoveObject()
    {
        MainEditor.InitializeEditObject();
    }

    public void MoveSelectedItem(int x, int y)
    {
        MainEditor.MoveSelectedItem(x, y);
    }

    public void FinishEditItem(bool commit)
    {
        MainEditor.FinishEditItem(commit);
    }

    public void FinishMoveObject(bool commit)
    {
        MainEditor.FinishEditObject(commit);
    }

    public void FinishMoveSprite(bool commit)
    {
        MainEditor.FinishEditSprite(commit);
    }

    public void UpdateFrame(int frame)
    {
        MainEditor.AnimationFrame = frame;
    }

    public DrawData GetDrawData(
        int startX,
        Size size,
        Color separatorColor,
        Color passiveColor,
        Color selectColor)
    {
        return MainEditor.GetDrawData(
            startX,
            size,
            separatorColor,
            passiveColor,
            selectColor);
    }

    public void RunInEmulator(string emulatorPath)
    {
        try
        {
            var info = new ProcessStartInfo(emulatorPath, [MainEditor.Path])
            {
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Maximized,
            };

            // TODO(swr): Maybe enforce only one running process?
            _ = Process.Start(info);
        }
        catch (Exception ex)
        {
            ExceptionHelper.Show(ex);
        }
    }

    private void MainEditor_ObjectData_ItemAdded(object? sender, ItemAddedEventArgs e)
    {
        SetEditItemEnabled();
        var item = MainEditor.ObjectData[e.Index];
        //ObjectListView.Items.Insert(e.Index, item);
    }

    private void MainEditor_ObjectData_ItemEdited(object? sender, ObjectEditedEventArgs e)
    {
        //ObjectListView.Items.RemoveAt(e.OldIndex);
        //ObjectListView.Items.Insert(e.NewIndex, e.NewCommand);
    }

    private void MainEditor_ObjectData_DataCleared(object? sender, EventArgs e)
    {
        SetDeleteAllEnabled();
        //ObjectListView.Items.Clear();
    }

    private void MainEditor_HasUnsavedChangesChanged(object? sender, EventArgs e)
    {
        MainView.SaveEnabled = MainEditor.HasUnsavedChanges;
        SetName();
    }

    private void MainEditor_ObjectData_DataReset(object? sender, EventArgs e)
    {
        MainView.DeleteAllEnabled = MainEditor.ObjectData.Count > 0;
        /*
        ObjectListView.Items.Clear();
        foreach (var item in MainEditor.ObjectData)
        {
            ObjectListView.Items.Add(item);
        }
        */
    }

    private void MainEditor_ItemCopied(object? sender, EventArgs e)
    {
        MainView.PasteEnabled = true;
    }

    private void MainEditor_AreaLoaded(object? sender, EventArgs e)
    {
        MainView.MapEditorEnabled = true;
    }

    private void MainEditor_AnimationFrameChanged(object? sender, EventArgs e)
    {
        MainView.Redraw();
    }

    private void MainEditor_ObjectData_ItemRemoved(object? sender, ItemAddedEventArgs e)
    {
        SetEditItemEnabled();
        SetDeleteAllEnabled();
        //ObjectListView.Items.RemoveAt(e.Index);
    }

    private void MainEditor_PathChanged(object? sender, EventArgs e)
    {
        SetName();
    }

    private void MainEditor_AreaHeaderChanged(object? sender, EventArgs e)
    {
        //ObjectListView.AreaPlatformType =
        ObjectEditorView.AreaPlatformType = MainEditor.AreaHeader.AreaPlatformType;
    }

    private void MainEditor_ObjectDataSelectedIndexChanged(object? sender, EventArgs e)
    {
        SetEditItemEnabled();
    }

    private void MainEditor_SpriteDataSelectedIndexChanged(object? sender, EventArgs e)
    {
        SetEditItemEnabled();
    }

    private void MainEditor_HistoryCleared(object? sender, EventArgs e)
    {
        SetUndoRedoEnabled();
    }

    private void MainEditor_UndoElementAdded(object? sender, EventArgs e)
    {
        SetUndoRedoEnabled();
    }

    private void MainEditor_UndoComplete(object? sender, EventArgs e)
    {
        SetUndoRedoEnabled();
    }

    private void MainEditor_RedoComplete(object? sender, EventArgs e)
    {
        SetUndoRedoEnabled();
    }

    private void SetUndoRedoEnabled()
    {
        MainView.UndoEnabled = MainEditor.CanUndo;
        MainView.RedoEnabled = MainEditor.CanRedo;
    }

    private void MainEditor_StartXChanged(object? sender, EventArgs e)
    {
        MainView.StartX = MainEditor.StartX;
    }

    private void MainEditor_Invalidated(object? sender, EventArgs e)
    {
        MainView.Redraw();
    }

    private void SpriteEditor_AreaSpriteCommandChanged(object? sender, EventArgs e)
    {
        /*
        MainModel.SpriteData[MainModel.CurrentSpriteIndex] =
            SpriteEditor.AreaSpriteCommand;
        */
    }

    private bool CloseInternal()
    {
        if (!CleanupBeforeClose())
        {
            return false;
        }

        MainEditor.Close();
        return true;
    }

    private bool CleanupBeforeClose()
    {
        if (MainEditor.HasUnsavedChanges)
        {
            switch (SaveOnClosePrompt.Prompt())
            {
            case PromptResult.Yes:
                MainEditor.Save();
                break;

            case PromptResult.Cancel:
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Inserts a temporary object at the current user coordinates. A dialog is
    /// opened for the user to edit the object. The user may confirm these edits
    /// and the object will be committed, or the user may cancel the add and the
    /// temporary object will be removed (no changes to the undo history will be
    /// made in this event).
    /// </summary>
    private void AddObject()
    {
        // First we take whatever the current object is, and just add it a
        // second time.
        var item = MainEditor.DefaultPreviewObject;
        MainEditor.AddPreviewObject(item);

        ObjectEditorView.UIAreaObjectCommand = item;
        ObjectEditorView.AreaObjectCommandChanged += ObjectEditor_ItemChanged;

        var commit = ObjectEditorView.PromptConfirm();
        MainEditor.FinishAddObject(commit);

        ObjectEditorView.AreaObjectCommandChanged -= ObjectEditor_ItemChanged;

        void ObjectEditor_ItemChanged(object? sender, EventArgs e)
        {
            MainEditor.EditPreviewObject(
                ObjectEditorView.UIAreaObjectCommand);
        }
    }

    private void AddSprite()
    {
        var item = MainEditor.DefaultPreviewSprite;
        MainEditor.AddPreviewSprite(item);

        SpriteEditorView.AreaSpriteCommand = item;
        SpriteEditorView.AreaSpriteCommandChanged += SpriteEditor_ItemChanged;

        var commit = SpriteEditorView.PromptConfirm();
        MainEditor.FinishAddSprite(commit);

        SpriteEditorView.AreaSpriteCommandChanged -= SpriteEditor_ItemChanged;

        void SpriteEditor_ItemChanged(object? sender, EventArgs e)
        {
            MainEditor.EditPreviewSprite(
               SpriteEditorView.AreaSpriteCommand);
        }
    }

    private void SetName()
    {
        var sb = new StringBuilder("Brutario");
        if (MainEditor.IsOpen)
        {
            _ = sb.Append(" - ");
            _ = sb.Append(MainEditor.Path);
            if (MainEditor.HasUnsavedChanges)
            {
                _ = sb.Append('*');
            }
        }

        MainView.Title = sb.ToString();
    }

    private void HeaderEditor_AreaHeaderChanged(object? sender, EventArgs e)
    {
        MainEditor.InitializeSetAreaHeader(HeaderEditorView.AreaHeader);
    }

    private void MainEditor_FileOpened(object? sender, EventArgs e)
    {
        MainView.EditorEnabled = true;
        MainView.AddRecentRom(MainEditor.Path);
    }

    private void MainEditor_FileSaved(object? sender, EventArgs e)
    {
        MainView.SaveEnabled = false;
    }

    private void MainEditor_FileClosed(object? sender, EventArgs e)
    {
        MainView.EditorEnabled =
        MainView.MapEditorEnabled =
        MainView.SaveEnabled =
        MainView.UndoEnabled =
        MainView.RedoEnabled =
        MainView.EditItemEnabled =
        MainView.PasteEnabled =
        MainView.DeleteAllEnabled = false;
    }

    private void MainEditor_AreaNumberChanged(object? sender, EventArgs e)
    {
        MainView.AreaNumber = MainEditor.AreaNumber;
    }

    private void MainEditor_SpriteModeChanged(object? sender, EventArgs e)
    {
        MainView.SpriteMode = MainEditor.SpriteMode;
        SetEditItemEnabled();
        SetDeleteAllEnabled();
        SetPasteEnabled();
    }

    private void SetEditItemEnabled()
    {
        MainView.EditItemEnabled = MainEditor.SpriteMode
            ? MainEditor.SelectedSpriteIndex != -1
            : MainEditor.SelectedObjectIndex != -1;
    }

    private void SetDeleteAllEnabled()
    {
        MainView.DeleteAllEnabled = MainEditor.SpriteMode
            ? MainEditor.SpriteData.Count > 0
            : MainEditor.ObjectData.Count > 0;
    }

    private void SetPasteEnabled()
    {
        MainView.PasteEnabled = MainEditor.SpriteMode
            ? MainEditor.CopiedSprite is not null
            : MainEditor.CopiedObject is not null;
    }

    private void MainEditor_PlayerChanged(object? sender, EventArgs e)
    {
        MainView.Player = MainEditor.Player;
    }

    private void MainEditor_PlayerStateChanged(object? sender, EventArgs e)
    {
        MainView.PlayerState = MainEditor.PlayerState;
    }

    private void ObjectListView_AddItem_Click(object? sender, EventArgs e)
    {
        AddObject();
    }

    private void ObjectListView_DeleteItem_Click(object? sender, EventArgs e)
    {
        MainEditor.RemoveObjectAt(ObjectListView.SelectedIndex);
    }

    private void ObjectListView_ClearItems_Click(object? sender, EventArgs e)
    {
        MainEditor.ClearObjects();
    }
}
