// <copyright file="ObjectEditedEventArgs.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.Core;

using System;

public class ObjectEditedEventArgs : EventArgs
{
    public ObjectEditedEventArgs(
        int oldIndex,
        int newIndex,
        UIAreaObjectCommand oldCommand,
        UIAreaObjectCommand newCommand)
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

    public UIAreaObjectCommand OldCommand
    {
        get; set;
    }

    public UIAreaObjectCommand NewCommand
    {
        get; set;
    }
}
