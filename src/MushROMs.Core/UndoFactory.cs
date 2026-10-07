// <copyright file="UndoFactory.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.Core;

using System;
using System.Collections.Generic;

public class UndoFactory
{
    public UndoFactory()
    {
        UndoStack = new Stack<UndoElement>();
        RedoStack = new Stack<UndoElement>();
    }

    public event EventHandler<EventArgs>? Cleared;

    public event EventHandler<UndoEventArgs>? UndoComplete;

    public event EventHandler<UndoEventArgs>? RedoComplete;

    public event EventHandler<UndoEventArgs>? UndoElementAdded;

    public bool CanUndo
    {
        get
        {
            return UndoStack.Count > 0;
        }
    }

    public bool CanRedo
    {
        get
        {
            return RedoStack.Count > 0;
        }
    }

    private Stack<UndoElement> UndoStack
    {
        get;
    }

    private Stack<UndoElement> RedoStack
    {
        get;
    }

    public void Add(Action undo, Action redo)
    {
        Add(new UndoElement(undo, redo));
    }

    public void Add(UndoElement undoElement)
    {
        UndoStack.Push(undoElement);
        RedoStack.Clear();
        OnUndoElementAdded(new UndoEventArgs(undoElement));
    }

    public void Clear()
    {
        UndoStack.Clear();
        RedoStack.Clear();
        OnCleared(EventArgs.Empty);
    }

    public void Undo()
    {
        if (!CanUndo)
        {
            throw new InvalidOperationException();
        }

        var undoElement = UndoStack.Pop();
        undoElement.Undo();
        RedoStack.Push(undoElement);
        OnUndoComplete(new UndoEventArgs(undoElement));
    }

    public void Redo()
    {
        if (!CanRedo)
        {
            throw new InvalidOperationException();
        }

        var undoElement = RedoStack.Pop();
        undoElement.Redo();
        UndoStack.Push(undoElement);
        OnRedoComplete(new UndoEventArgs(undoElement));
    }

    protected virtual void OnCleared(EventArgs e)
    {
        Cleared?.Invoke(this, e);
    }

    protected virtual void OnUndoComplete(UndoEventArgs e)
    {
        UndoComplete?.Invoke(this, e);
    }

    protected virtual void OnRedoComplete(UndoEventArgs e)
    {
        RedoComplete?.Invoke(this, e);
    }

    protected virtual void OnUndoElementAdded(UndoEventArgs e)
    {
        UndoElementAdded?.Invoke(this, e);
    }
}
