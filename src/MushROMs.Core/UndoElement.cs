// <copyright file="UndoElement.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.Core;

using System;

public class UndoElement
{
    public UndoElement(Action undo, Action redo)
    {
        Undo = undo;
        Redo = redo;
    }

    public Action Undo
    {
        get;
    }

    public Action Redo
    {
        get;
    }
}
