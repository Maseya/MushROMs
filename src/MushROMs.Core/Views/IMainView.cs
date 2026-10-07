// <copyright file="IMainView.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.Core.Views;

using System.Drawing;

using Maseya.Smas.Smb1;

public interface IMainView
{
    string Title
    {
        get; set;
    }

    bool EditorEnabled
    {
        get; set;
    }

    bool MapEditorEnabled
    {
        get; set;
    }

    bool SaveEnabled
    {
        get; set;
    }

    bool UndoEnabled
    {
        get; set;
    }

    bool RedoEnabled
    {
        get; set;
    }

    bool EditItemEnabled
    {
        get; set;
    }

    bool PasteEnabled
    {
        get; set;
    }

    bool DeleteAllEnabled
    {
        get; set;
    }

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

    Size DrawAreaSize
    {
        get;
    }

    void AddRecentRom(string path);

    void ClearRecentRoms();

    void Redraw();
}
