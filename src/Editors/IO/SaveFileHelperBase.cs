// <copyright file="SaveFileHelperBase.cs" company="Public Domain">
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

    public abstract class SaveFileHelperBase : Component
    {
        private IExceptionHandler _exceptionHandler;

        protected SaveFileHelperBase()
        {
            Associations = new Dictionary<string, SaveFileAssociation>(
                StringFuncComparer.WindowsExtensionComparer);

            Classes = new Dictionary<string, string>();
        }

        protected SaveFileHelperBase(IContainer container)
            : this()
        {
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.Add(this);
        }

        public event EventHandler ExceptionHandlerChanged;

        public event EventHandler<EditorEventArgs> EditorSaved;

        public IExceptionHandler ExceptionHandler
        {
            get
            {
                return _exceptionHandler;
            }

            set
            {
                if (ExceptionHandler == value)
                {
                    return;
                }

                _exceptionHandler = value;
                OnExceptionHandlerChanged(EventArgs.Empty);
            }
        }

        private Dictionary<string, SaveFileAssociation> Associations
        {
            get;
        }

        private Dictionary<string, string> Classes
        {
            get;
        }

        public void AddAssociation(SaveFileAssociation association)
        {
            if (association is null)
            {
                throw new ArgumentNullException(nameof(association));
            }

            Associations.Add(association.Extension, association);
        }

        public void SaveFile(IEditor editor)
        {
            if (editor is null)
            {
                throw new ArgumentNullException(nameof(editor));
            }

            SaveFile(editor, editor.Path);
        }

        public void SaveFile(IEditor editor, string path)
        {
            if (editor is null)
            {
                throw new ArgumentNullException(nameof(editor));
            }

            if (!TryGetSaveEditorCallback(path, out var saveEditor))
            {
                saveEditor = UISelectSaveEditorCallback(path);
                if (saveEditor is null)
                {
                    return;
                }
            }

            SaveFile(editor, path, saveEditor);
        }

        public void SaveFileAs(IEditor editor)
        {
            if (editor is null)
            {
                throw new ArgumentNullException(nameof(editor));
            }

            SaveFileAs(editor, editor.Path);
        }

        public void SaveFileAs(IEditor editor, string path)
        {
            if (editor is null)
            {
                throw new ArgumentNullException(nameof(editor));
            }

            var saveEditor = UISelectSaveEditorCallback(path);
            if (saveEditor is null)
            {
                return;
            }

            SaveFile(editor, path, saveEditor);
        }

        public void SaveFile(IEditor editor, SaveEditorCallback saveEditor)
        {
            if (editor is null)
            {
                throw new ArgumentNullException(nameof(editor));
            }

            SaveFile(editor, editor.Path, saveEditor);
        }

        public void SaveFile(
            IEditor editor,
            string path,
            SaveEditorCallback saveEditor)
        {
            if (editor is null)
            {
                throw new ArgumentNullException(nameof(editor));
            }

            if (saveEditor is null)
            {
                throw new ArgumentNullException(nameof(saveEditor));
            }

            if (!TrySaveEditor(editor, path, saveEditor))
            {
                return;
            }

            editor.Path = path;
            OnEditorSaved(new EditorEventArgs(editor));
        }

        public bool TryGetSaveEditorCallback(
            string path,
            out SaveEditorCallback saveEditor)
        {
            if (Associations.TryGetValue(
                path,
                out var association))
            {
                saveEditor = association.SaveEditorCallback;
                return true;
            }
            else
            {
                saveEditor = null;
                return false;
            }
        }

        public SaveFileAssociation[] GetRelatedAssociations(
            string editorClass)
        {
            var result = new List<SaveFileAssociation>();
            foreach (var association in Associations.Values)
            {
                if (association.EditorClass == editorClass)
                {
                    result.Add(association);
                }
            }

            return result.ToArray();
        }

        protected virtual bool TrySaveEditor(
            IEditor editor,
            string path,
            SaveEditorCallback saveEditor)
        {
            if (saveEditor is null)
            {
                return false;
            }

            if (editor is null)
            {
                return false;
            }

        loop:
            try
            {
                saveEditor(editor, path);
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
                return false;
            }

            return true;
        }

        protected abstract SaveEditorCallback UISelectSaveEditorCallback(
            string path);

        protected virtual void OnExceptionHandlerChanged(EventArgs e)
        {
            ExceptionHandlerChanged?.Invoke(this, e);
        }

        protected virtual void OnEditorSaved(EditorEventArgs e)
        {
            EditorSaved?.Invoke(this, e);
        }
    }
}
