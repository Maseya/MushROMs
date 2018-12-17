// <copyright file="Editor.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Editors
{
    using System;
    using System.IO;
    using Maseya.Helper;
    using static System.IO.Path;

    /// <summary>
    /// Provides a generic implementation of <see cref="IEditor"/>.
    /// </summary>
    public abstract class Editor : IEditor
    {
        /// <summary>
        /// The file path that this <see cref="Editor"/> will by default
        /// read from and write to during any file operations.
        /// </summary>
        private string _path;

        /// <summary>
        /// Initializes a new instance of the <see cref="Editor"/> class.
        /// </summary>
        /// <param name="path">
        /// The file path that this <see cref="Editor"/> will by default
        /// read from and write to during any file operations.
        /// </param>
        /// <inheritdoc cref="GetFullPath(String)" select="exception"/>
        protected Editor(string path)
        {
            Path = path;
            History = new UndoFactory();
        }

        /// <summary>
        /// Occurs when <see cref="Path"/> is changed.
        /// </summary>
        public event EventHandler PathChanged;

        /// <summary>
        /// Occurs when <see cref="WriteData(Action, Action)"/> is
        /// invoked.
        /// </summary>
        public event EventHandler DataModified;

        /// <summary>
        /// Occurs when <see cref="CanUndo"/> is <see langword="true"/>
        /// and <see cref="Undo"/> is called.
        /// </summary>
        public event EventHandler UndoApplied;

        /// <summary>
        /// Occurs when <see cref="CanRedo"/> is <see langword="true"/>
        /// and <see cref="Redo"/> is called.
        /// </summary>
        public event EventHandler RedoApplied;

        /// <summary>
        /// Gets or sets the file path that this <see cref="Editor"/>
        /// will by default read from and write to during any file
        /// operations.
        /// </summary>
        /// <inheritdoc cref="GetFullPath(String)" select="exception"/>
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
        /// Gets a value that determines whether this instance of <see
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
        /// Gets a value that determines whether this instance of <see
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
        /// Gets a value that determines whether this instance of <see
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
        /// Gets a value that determines whether this instance of <see
        /// cref="Editor"/> can invoke <see cref="Copy"/>.
        /// </summary>
        public virtual bool CanCopy
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value that determines whether this instance of <see
        /// cref="Editor"/> can invoke <see cref="Paste"/>.
        /// </summary>
        public virtual bool CanPaste
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value that determines whether this instance of <see
        /// cref="Editor"/> can invoke <see cref="Delete"/>.
        /// </summary>
        public virtual bool CanDelete
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value that determines whether this instance of <see
        /// cref="Editor"/> can invoke <see cref="SelectAll"/>.
        /// </summary>
        public virtual bool CanSelectAll
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the internal <see cref="UndoFactory"/> that controls
        /// the undo and redo operations of this <see cref="Editor"/>
        /// instance.
        /// </summary>
        private UndoFactory History
        {
            get;
        }

        /// <summary>
        /// Undoes the last call to <see cref="WriteData(Action,
        /// Action)"/>.
        /// </summary>
        public void Undo()
        {
            if (!CanUndo)
            {
                return;
            }

            History.Undo();
            OnUndoApplied(EventArgs.Empty);
        }

        /// <summary>
        /// Redoes the last call to <see cref="WriteData(Action,
        /// Action)"/>.
        /// </summary>
        public void Redo()
        {
            if (!CanRedo)
            {
                return;
            }

            History.Redo();
            OnRedoApplied(EventArgs.Empty);
        }

        /// <summary>
        /// Copies the current selection of data by calling <see
        /// cref="Copy"/>, and then deletes the selection by calling
        /// <see cref="Delete"/>.
        /// </summary>
        public virtual void Cut()
        {
            Copy();
            Delete();
        }

        /// <summary>
        /// When overridden in a derived class, copies the current
        /// selection of data.
        /// </summary>
        public abstract void Copy();

        /// <summary>
        /// When overridden in a derived class, pastes the data in the
        /// copy buffer into the current selection.
        /// </summary>
        public abstract void Paste();

        /// <summary>
        /// When overridden in a derived class, deletes the data in the
        /// current selection.
        /// </summary>
        public abstract void Delete();

        /// <summary>
        /// When overridden in a derived class, puts all of the data in
        /// this <see cref="Editor"/> instance into the current
        /// selection.
        /// </summary>
        public abstract void SelectAll();

        /// <summary>
        /// Write to the data of this <see cref="Editor"/> instance in
        /// some way and optionally provide an undo action.
        /// </summary>
        /// <param name="action">
        /// An <see cref="Action"/> to invoke to write to the internal
        /// data of this <see cref="Editor"/> instance in some way.
        /// </param>
        /// <param name="undo">
        /// An optional <see cref="Action"/> that will undo the changes
        /// to the data state that <paramref name="action"/> applied.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action"/> is <see langword="null"/>.
        /// </exception>
        public void WriteData(Action action, Action undo = null)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (undo != null)
            {
                History.Add(undo, action);
            }

            action();
            OnDataModified(EventArgs.Empty);
        }

        /// <summary>
        /// Gets the string representation of this <see cref="Editor"/>
        /// instance.
        /// </summary>
        /// <returns>The file name of <see cref="Path"/>.</returns>
        public override string ToString()
        {
            return GetFileName(Path);
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
        /// <param name="number">
        /// A reference to the current untitled number. This value will
        /// be incremented until a valid new file name is found.
        /// </param>
        /// <returns>
        /// A string to the first path generated that does not already
        /// exist.
        /// </returns>
        protected static string NextUntitledPath(
            string name,
            string extension,
            ref int number)
        {
            string path;
            do
            {
                path = Combine(
                    name,
                    extension,
                    (++number).ToString());
            }
            while (File.Exists(path));

            return path;
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

        /// <summary>
        /// Raises the <see cref="UndoApplied"/> event.
        /// </summary>
        /// <param name="e">
        /// An <see cref="EventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnUndoApplied(EventArgs e)
        {
            UndoApplied?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="RedoApplied"/> event.
        /// </summary>
        /// <param name="e">
        /// An <see cref="EventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnRedoApplied(EventArgs e)
        {
            RedoApplied?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="DataModified"/> event.
        /// </summary>
        /// <param name="e">
        /// An <see cref="EventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnDataModified(EventArgs e)
        {
            DataModified?.Invoke(this, e);
        }
    }
}
