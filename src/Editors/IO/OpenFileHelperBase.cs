// <copyright file="OpenFileHelperBase.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Editors.IO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using Maseya.Helper;

    public abstract class OpenFileHelperBase : Component
    {
        private IExceptionHandler _exceptionHandler;

        protected OpenFileHelperBase()
        {
            Associations = new Dictionary<string, OpenEditorCallback>(
                StringFuncComparer.WindowsExtensionComparer);
        }

        protected OpenFileHelperBase(IContainer container)
            : this()
        {
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.Add(this);
        }

        public event EventHandler<EditorEventArgs> EditorOpened;

        public event EventHandler ExceptionHandlerChanged;

        public IExceptionHandler ExceptionHandler
        {
            get
            {
                return _exceptionHandler;
            }

            set
            {
                _exceptionHandler = value;
                OnExceptionHandlerChanged(EventArgs.Empty);
            }
        }

        private Dictionary<string, OpenEditorCallback> Associations
        {
            get;
        }

        public void AddAssociation(
            string extension,
            OpenEditorCallback openEditorCallback)
        {
            Associations.Add(extension, openEditorCallback);
        }

        public void OpenFile()
        {
            var files = UIChooseFiles();
            if (files is null)
            {
                return;
            }

            foreach (var file in files)
            {
                OpenFile(file);
            }
        }

        public void OpenFile(string path)
        {
            if (!TryGetOpenEditorCallback(path, out var openEditor))
            {
                openEditor = UISelectOpenEditorCallback(path);
                if (openEditor is null)
                {
                    return;
                }
            }

            OpenFile(path, openEditor);
        }

        public void OpenFileAs()
        {
            var files = UIChooseFiles();
            if (files is null)
            {
                return;
            }

            foreach (var file in files)
            {
                OpenFileAs(file);
            }
        }

        public void OpenFileAs(string path)
        {
            var openEditor = UISelectOpenEditorCallback(path);
            if (openEditor is null)
            {
                return;
            }

            OpenFile(path, openEditor);
        }

        public bool TryGetOpenEditorCallback(
            string path,
            out OpenEditorCallback openEditor)
        {
            return Associations.TryGetValue(
                path,
                out openEditor);
        }

        public void OpenFile(OpenEditorCallback openEditor)
        {
            var files = UIChooseFiles();
            if (files is null)
            {
                return;
            }

            foreach (var file in files)
            {
                OpenFile(file, openEditor);
            }
        }

        public void OpenFile(string path, OpenEditorCallback openEditor)
        {
            if (openEditor is null)
            {
                throw new ArgumentNullException(
                    nameof(openEditor));
            }

            if (!TryOpenFile(path, openEditor, out var editor))
            {
                return;
            }

            if (editor is null)
            {
                return;
            }

            OnEditorOpened(new EditorEventArgs(editor));
        }

        protected virtual bool TryOpenFile(
            string path,
            OpenEditorCallback openEditor,
            out IEditor editor)
        {
            if (openEditor is null)
            {
                editor = null;
                return false;
            }

        loop:
            try
            {
                editor = openEditor(path);
            }
            catch (IOException ex)
            {
                if (ExceptionHandler is null)
                {
                    throw ex;
                }

                if (ExceptionHandler.ShowExceptionAndRetry(ex))
                {
                    goto loop;
                }
                else
                {
                    editor = null;
                    return false;
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHandler is null)
                {
                    throw ex;
                }

                ExceptionHandler.ShowException(ex);

                editor = null;
                return false;
            }

            return true;
        }

        protected virtual void OnEditorOpened(EditorEventArgs e)
        {
            EditorOpened?.Invoke(this, e);
        }

        /// <summary>
        /// Prompt the user to select which files to open.
        /// </summary>
        /// <returns>
        /// A collection of file paths chosen by the user.
        /// </returns>
        protected abstract IEnumerable<string> UIChooseFiles();

        /// <summary>
        /// Prompt user to select which <see cref="OpenEditorCallback"/> to
        /// invoke based on the file path.
        /// </summary>
        /// <param name="path">
        /// The file path to attempt to open.
        /// </param>
        /// <returns>
        /// An instance of <see cref="OpenEditorCallback"/> chosen by the user.
        /// </returns>
        protected abstract OpenEditorCallback UISelectOpenEditorCallback(
            string path);

        protected virtual void OnExceptionHandlerChanged(EventArgs e)
        {
            ExceptionHandlerChanged?.Invoke(this, e);
        }
    }
}
