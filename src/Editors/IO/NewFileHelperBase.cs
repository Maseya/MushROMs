// <copyright file="NewFileHelperBase.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Editors.IO
{
    using System;
    using System.ComponentModel;
    using Maseya.Helper;

    public abstract class NewFileHelperBase : Component
    {
        private IExceptionHandler _exceptionHandler;

        protected NewFileHelperBase()
        {
        }

        protected NewFileHelperBase(IContainer container)
            : this()
        {
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.Add(this);
        }

        public event EventHandler<EditorEventArgs> EditorCreated;

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

        /// <summary>
        /// Prompt user about which file to create and then create it.
        /// </summary>
        public void NewFile()
        {
            IEditor editor = null;
            while (!TryCreateEditor(out editor))
            {
                continue;
            }

            if (editor is null)
            {
                return;
            }

            OnEditorCreated(new EditorEventArgs(editor));
        }

        public void NewFile(CreateEditorCallback createEditor)
        {
            if (createEditor is null)
            {
                throw new ArgumentNullException(nameof(createEditor));
            }

            if (!TryCreateEditor(createEditor, out var editor))
            {
                return;
            }

            if (editor is null)
            {
                return;
            }

            OnEditorCreated(new EditorEventArgs(editor));
        }

        protected virtual void OnEditorCreated(EditorEventArgs e)
        {
            EditorCreated?.Invoke(this, e);
        }

        protected bool TryCreateEditor(out IEditor editor)
        {
            var createEditor = UISelectCreateEditorCallback();
            if (createEditor is null)
            {
                editor = null;
                return true;
            }

            return TryCreateEditor(createEditor, out editor);
        }

        protected virtual bool TryCreateEditor(
            CreateEditorCallback createEditor,
            out IEditor editor)
        {
            if (createEditor is null)
            {
                editor = null;
                return false;
            }

            try
            {
                editor = createEditor();
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

        /// <summary>
        /// Prompt user to select which <see cref="CreateEditorCallback"/> to
        /// invoke to create an instance of <see cref="IEditor"/>.
        /// </summary>
        /// <returns>
        /// An instance of <see cref="CreateEditorCallback"/> that will be
        /// invoked in <see cref="NewFile()"/> to create an instance of <see
        /// cref="IEditor"/>.
        /// </returns>
        protected abstract CreateEditorCallback UISelectCreateEditorCallback();

        protected virtual void OnExceptionHandlerChanged(EventArgs e)
        {
            ExceptionHandlerChanged?.Invoke(this, e);
        }
    }
}
