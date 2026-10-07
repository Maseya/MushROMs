// <copyright file="IMainEditor.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.Core.Editors;

using System;
using System.Collections.Generic;

using Maseya.Smas.Smb1;
using Maseya.Smas.Smb1.AreaData.HeaderData;

public interface IMainEditor
{
    event EventHandler? PathChanged;

    event EventHandler? FileOpened;

    event EventHandler? FileSaved;

    event EventHandler? FileClosed;

    event EventHandler? UndoElementAdded;

    event EventHandler? UndoComplete;

    event EventHandler? RedoComplete;

    event EventHandler? PlayerChanged;

    event EventHandler? PlayerStateChanged;

    event EventHandler? SpriteModeChanged;

    event EventHandler? AreaNumberChanged;

    event EventHandler? AreaHeaderChanged;

    event EventHandler? StartXChanged;

    event EventHandler? SelectedObjectChanged;

    event EventHandler? SelectedSpriteChanged;

    event EventHandler? Invalidated;

    event EventHandler? ObjectData_DataReset;

    event EventHandler<ObjectEditedEventArgs>? ObjectData_ItemEdited;

    event EventHandler<ItemAddedEventArgs>? ObjectData_ItemAdded;

    event EventHandler<ItemAddedEventArgs>? ObjectData_ItemRemoved;

    event EventHandler? ObjectData_DataCleared;

    string Path
    {
        get;
    }

    bool IsOpen
    {
        get;
    }

    bool HasUnsavedChanges
    {
        get;
    }

    bool CanUndo
    {
        get;
    }

    bool CanRedo
    {
        get;
    }

    /*
    int AnimationFrame { get; }
    */

    Player Player
    {
        get; set;
    }

    PlayerState PlayerState
    {
        get; set;
    }

    bool SpriteMode
    {
        get; set;
    }

    int AreaNumber
    {
        get; set;
    }

    int StartX
    {
        get; set;
    }

    AreaHeader AreaHeader
    {
        get; set;
    }

    IReadOnlyList<UIAreaObjectCommand> ObjectData
    {
        get;
    }

    IReadOnlyList<UIAreaSpriteCommand> SpriteData
    {
        get;
    }

    int SelectedObjectIndex
    {
        get; set;
    }

    int SelectedSpriteIndex
    {
        get; set;
    }

    /*
    event EventHandler AnimationFrameChanged;
    */

    void Open(string path);

    void Save();

    void SaveAs(string path);

    void Close();

    void Undo();

    void Redo();

    void SetSelectedItem(int x, int y);

    void InitializeSetAreaHeader(AreaHeader newAreaHeader);

    void FinishSetAreaHeader(AreaHeader oldAreaHeader, bool commit);
}
