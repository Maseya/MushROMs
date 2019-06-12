// <copyright file="SharedListChildEditor.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Editors.Collections
{
    using System;
    using System.Collections.Generic;
    using Maseya.Helper;

    public abstract class SharedListChildEditor<T> :
        SharedListChild<T>, IListEditor<T>
    {
        protected SharedListChildEditor(SharedListParentEditor parent)
            : base(parent)
        {
            Parent = parent
                ?? throw new ArgumentNullException(nameof(parent));
        }

        protected SharedListChildEditor(ISharedListParent parent)
            : base(parent)
        {
            Parent = null;
        }

        public event EventHandler SizeChanged;

        public event EventHandler PathChanged;

        public event EventHandler<CancelEventArgs> BeginUndo;

        public event EventHandler UndoComplete;

        public event EventHandler<CancelEventArgs> BeginRedo;

        public event EventHandler RedoComplete;

        public event EventHandler<CancelEventArgs> BeginCut;

        public event EventHandler CutComplete;

        public event EventHandler<CancelEventArgs> BeginCopy;

        public event EventHandler CopyComplete;

        public event EventHandler<CancelEventArgs> BeginPaste;

        public event EventHandler PasteComplete;

        public event EventHandler<CancelEventArgs> BeginDelete;

        public event EventHandler DeleteComplete;

        public event EventHandler<CancelEventArgs> BeginSelectAll;

        public event EventHandler SelectAllComplete;

        public event EventHandler<CancelEventArgs> SelectionChanging;

        public event EventHandler SelectionChanged;

        public event EventHandler<CancelEventArgs> BeginWriteData;

        public event EventHandler WriteDataComplete;

        public bool CanUndo
        {
            get
            {
                return Parent?.CanUndo ?? false;
            }
        }

        public bool CanRedo
        {
            get
            {
                return Parent?.CanRedo ?? false;
            }
        }

        public bool CanCut
        {
            get
            {
                return Parent?.CanCut ?? false;
            }
        }

        public bool CanCopy
        {
            get
            {
                return Parent?.CanCopy ?? false;
            }
        }

        public bool CanPaste
        {
            get
            {
                return Parent?.CanPaste ?? false;
            }
        }

        public bool CanDelete
        {
            get
            {
                return Parent?.CanDelete ?? false;
            }
        }

        public bool CanSelectAll
        {
            get
            {
                return Parent?.CanSelectAll ?? false;
            }
        }

        public string Path
        {
            get
            {
                return Parent?.Path;
            }

            set
            {
                if (Parent is null)
                {
                    throw new InvalidOperationException();
                }

                Parent.Path = value;
            }
        }

        public IListSelectionData<T> CopyData
        {
            get;
            set;
        }

        public IListSelection CurrentSelection
        {
            get;
            set;
        }

        public bool IsInsertMode
        {
            get;
            set;
        }

        private SharedListParentEditor Parent
        {
            get;
        }

        public void Undo()
        {
            if (!CanUndo)
            {
                return;
            }

            Parent.Undo();
        }

        public void Redo()
        {
            if (!CanRedo)
            {
                return;
            }

            Parent.Redo();
        }

        public void Cut()
        {
            Cut(CurrentSelection);
        }

        public void Cut(IListSelection selection)
        {
            if (!CanCut)
            {
                return;
            }

            var e = new CancelEventArgs();
            OnBeginCut(e);
            if (e.Cancel)
            {
                return;
            }

            CopyData = this.GetSelectionData(selection);
            Parent.Cut(GetByteSelection(selection));
            OnCutComplete(EventArgs.Empty);
        }

        public void Copy()
        {
            Copy(CurrentSelection);
        }

        public void Copy(IListSelection selection)
        {
            if (!CanCopy)
            {
                return;
            }

            if (selection is null)
            {
                throw new ArgumentNullException(nameof(selection));
            }

            if (selection.Count == 0)
            {
                return;
            }

            var e = new CancelEventArgs();
            OnBeginCopy(e);
            if (e.Cancel)
            {
                return;
            }

            CopyData = this.GetSelectionData(selection);
            OnCopyComplete(EventArgs.Empty);
        }

        public void Paste()
        {
            Paste(CopyData, IsInsertMode);
        }

        public void Paste(IListSelectionData<T> values, bool insert)
        {
            if (!CanPaste)
            {
                return;
            }

            var e = new CancelEventArgs();
            OnBeginPaste(e);
            if (e.Cancel)
            {
                return;
            }

            if (insert)
            {
                InsertSelection(values);
            }
            else
            {
                WriteSelection(values);
            }

            OnPasteComplete(EventArgs.Empty);
        }

        public void Delete()
        {
            Delete(CurrentSelection);
        }

        public void Delete(IListSelection selection)
        {
            if (!CanDelete)
            {
                return;
            }

            var e = new CancelEventArgs();
            OnBeginDelete(e);
            if (e.Cancel)
            {
                return;
            }

            RemoveSelection(CurrentSelection);
            OnDeleteComplete(EventArgs.Empty);
        }

        public void SelectAll()
        {
            if (!CanSelectAll)
            {
                return;
            }

            CurrentSelection = new LinearListSelection(0, Parent.Count);
        }

        public void ModifyList(
            Action<IList<T>> action,
            Action<IList<T>> undo = null)
        {
            if (Parent is null)
            {
                action(this);
            }
            else
            {
                Parent.ModifyItemList(Create, action, undo);
            }
        }

        public void WriteData(Action action, Action undo = null)
        {
            if (Parent is null)
            {
                action();
            }
            else
            {
                Parent.WriteData(action, undo);
            }
        }

        protected abstract ISharedListChild<T> Create(
            ISharedListParent parent);

        protected virtual void OnPathChanged(EventArgs e)
        {
            PathChanged?.Invoke(this, e);
        }

        protected virtual void OnBeginUndo(CancelEventArgs e)
        {
            BeginUndo?.Invoke(this, e);
        }

        protected virtual void OnUndoComplete(EventArgs e)
        {
            UndoComplete?.Invoke(this, e);
        }

        protected virtual void OnBeginRedo(CancelEventArgs e)
        {
            BeginRedo?.Invoke(this, e);
        }

        protected virtual void OnRedoComplete(EventArgs e)
        {
            RedoComplete?.Invoke(this, e);
        }

        protected virtual void OnBeginCut(CancelEventArgs e)
        {
            BeginCut?.Invoke(this, e);
        }

        protected virtual void OnCutComplete(EventArgs e)
        {
            CutComplete?.Invoke(this, e);
        }

        protected virtual void OnBeginCopy(CancelEventArgs e)
        {
            BeginCopy?.Invoke(this, e);
        }

        protected virtual void OnCopyComplete(EventArgs e)
        {
            CopyComplete?.Invoke(this, e);
        }

        protected virtual void OnBeginPaste(CancelEventArgs e)
        {
            BeginPaste?.Invoke(this, e);
        }

        protected virtual void OnPasteComplete(EventArgs e)
        {
            PasteComplete?.Invoke(this, e);
        }

        protected virtual void OnBeginDelete(CancelEventArgs e)
        {
            BeginDelete?.Invoke(this, e);
        }

        protected virtual void OnDeleteComplete(EventArgs e)
        {
            DeleteComplete?.Invoke(this, e);
        }

        protected virtual void OnBeginSelectAll(CancelEventArgs e)
        {
            BeginSelectAll?.Invoke(this, e);
        }

        protected virtual void OnSelectAllComplete(EventArgs e)
        {
            SelectAllComplete?.Invoke(this, e);
        }

        protected virtual void OnSelectionChanged(CancelEventArgs e)
        {
            SelectionChanging?.Invoke(this, e);
        }

        protected virtual void OnSelectionChanged(EventArgs e)
        {
            SelectionChanged?.Invoke(this, e);
        }

        protected virtual void OnBeginWriteData(CancelEventArgs e)
        {
            BeginWriteData?.Invoke(this, e);
        }

        protected virtual void OnWriteDataComplete(EventArgs e)
        {
            WriteDataComplete?.Invoke(this, e);
        }
    }
}
