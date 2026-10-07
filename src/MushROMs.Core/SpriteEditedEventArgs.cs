// <copyright file="SpriteEditedEventArgs.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.Core;

using System;

public class SpriteEditedEventArgs : EventArgs
{
    public SpriteEditedEventArgs(
        int oldIndex,
        int newIndex,
        UIAreaSpriteCommand oldCommand,
        UIAreaSpriteCommand newCommand)
    {
        OldIndex = oldIndex;
        NewIndex = newIndex;
        OldCommand = oldCommand;
        NewCommand = newCommand;
    }

    public int OldIndex
    {
        get; set;
    }

    public int NewIndex
    {
        get; set;
    }

    public UIAreaSpriteCommand OldCommand
    {
        get; set;
    }

    public UIAreaSpriteCommand NewCommand
    {
        get; set;
    }
}
