// <copyright file="Editor.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Editors
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Maseya.Helper;
    using static System.IO.Path;

    /// <summary>
    /// Provides a generic implementation of <see cref="IEditor"/>.
    /// </summary>
    public abstract class Editor : IEditor
    {
        /// <summary>
        /// The current untitled number of each extension.
        /// </summary>
        private static readonly Dictionary<string, int> UntitledNumbers =
            new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// The file path that this <see cref="Editor"/> will by default read
        /// from and write to during any file operations.
        /// </summary>
        private string _path;

        /// <summary>
        /// Initializes a new instance of the <see cref="Editor"/> class.
        /// </summary>
        /// <param name="path">
        /// The file path that this <see cref="Editor"/> will by default read
        /// from and write to during any file operations.
        /// </param>
        /// <inheritdoc cref="GetFullPath(String)" select="exception"/>
        ///
        protected Editor(string path)
        {
            Path = path;
            History = new UndoFactory();
        }

        /// <summary>
        /// Occurs when <see cref="Path"/> is changed.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the file path that this <see cref="Editor"/> will by
        /// default read from and write to during any file operations.
        /// </summary>
        /// <inheritdoc cref="GetFullPath(String)" select="exception"/>
        ///
        public string Path
        {
            get
            {
                return _path;
            }

            set
            {
                var path = GetFullPath(value);
                var equals = String.Equals(
                    _path,
                    path,
                    StringComparison.OrdinalIgnoreCase);

                if (equals)
                {
                    return;
                }

                _path = path;
                OnPathChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance of <see
        /// cref="Editor"/> can invoke <see cref="Undo"/>.
        /// </summary>
        public virtual bool CanUndo
        {
            get
            {
                return History.CanUndo;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance of <see
        /// cref="Editor"/> can invoke <see cref="Redo"/>.
        /// </summary>
        public virtual bool CanRedo
        {
            get
            {
                return History.CanRedo;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance of <see
        /// cref="Editor"/> can invoke <see cref="Cut"/>.
        /// </summary>
        public virtual bool CanCut
        {
            get
            {
                return CanCopy & CanDelete;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance of <see
        /// cref="Editor"/> can invoke <see cref="Copy"/>.
        /// </summary>
        public abstract bool CanCopy
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether this instance of <see
        /// cref="Editor"/> can invoke <see cref="Paste"/>.
        /// </summary>
        public abstract bool CanPaste
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether this instance of <see
        /// cref="Editor"/> can invoke <see cref="Delete"/>.
        /// </summary>
        public abstract bool CanDelete
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether this instance of <see
        /// cref="Editor"/> can invoke <see cref="SelectAll"/>.
        /// </summary>
        public abstract bool CanSelectAll
        {
            get;
        }

        public abstract bool CanInsert
        {
            get;
        }

        /// <summary>
        /// Gets the internal <see cref="UndoFactory"/> that controls the undo
        /// and redo operations of this <see cref="Editor"/> instance.
        /// </summary>
        private UndoFactory History
        {
            get;
        }

        /// <summary>
        /// Gets a path name to place a new, untitled file.
        /// </summary>
        /// <param name="name">
        /// The file name to use for the new file.
        /// </param>
        /// <param name="extension">
        /// The file extension the new file should have.
        /// </param>
        /// <returns>
        /// A string to the first path generated that does not already exist.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> or <paramref name="extension"/> is <see
        /// langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="extension"/> contains one or more of the invalid
        /// path characters defined in <see cref=" InvalidPathChars"/>.
        /// </exception>
        public static string GetUntitledPath(string name, string extension)
        {
            if (!UntitledNumbers.TryGetValue(extension, out var number))
            {
                number = 0;
            }

            string path;
            do
            {
                number++;
                path = Combine(
                    $"{name}{number:D2}{GetExtension(extension)}");
            }
            while (File.Exists(path));

            UntitledNumbers[extension] = number;
            return path;
        }

        /// <summary>
        /// Undoes the last call to <see cref="WriteData(Action, Action)"/>.
        /// </summary>
        public void Undo()
        {
            if (!CanUndo)
            {
                return;
            }

            var e = new CancelEventArgs();
            OnBeginUndo(e);
            if (e.Cancel)
            {
                return;
            }

            History.Undo();
            OnUndoComplete(EventArgs.Empty);
        }

        /// <summary>
        /// Redoes the last call to <see cref="WriteData(Action, Action)"/>.
        /// </summary>
        public void Redo()
        {
            if (!CanRedo)
            {
                return;
            }

            var e = new CancelEventArgs();
            OnBeginRedo(e);
            if (e.Cancel)
            {
                return;
            }

            History.Redo();
            OnRedoComplete(EventArgs.Empty);
        }

        public abstract void Cut();

        /// <summary>
        /// When overridden in a derived class, copies the current selection of
        /// data.
        /// </summary>
        public abstract void Copy();

        /// <summary>
        /// When overridden in a derived class, pastes the data in the copy
        /// buffer into the current selection.
        /// </summary>
        public abstract void Paste();

        /// <summary>
        /// When overridden in a derived class, deletes the data in the current
        /// selection.
        /// </summary>
        public abstract void Delete();

        /// <summary>
        /// When overridden in a derived class, puts all of the data in this
        /// <see cref="Editor"/> instance into the current selection.
        /// </summary>
        public abstract void SelectAll();

        /// <summary>
        /// Write to the data of this <see cref="Editor"/> instance in some way
        /// and optionally provide an undo action.
        /// </summary>
        /// <param name="action">
        /// An <see cref="Action"/> to invoke to write to the internal data of
        /// this <see cref="Editor"/> instance in some way.
        /// </param>
        /// <param name="undo">
        /// An optional <see cref="Action"/> that will undo the changes to the
        /// data state that <paramref name="action"/> applied.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action"/> is <see langword="null"/>.
        /// </exception>
        public void WriteData(Action action, Action undo = null)
        {
            var e = new CancelEventArgs();
            OnBeginWriteData(e);
            if (e.Cancel)
            {
                return;
            }

            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (undo != null)
            {
                History.Add(undo, action);
            }

            action();
            OnWriteDataComplete(EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="PathChanged"/> event.
        /// </summary>
        /// <param name="e">
        /// An <see cref="EventArgs"/> that contains the event data.
        /// </param>
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

        protected virtual void OnSelectionChanging(CancelEventArgs e)
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
