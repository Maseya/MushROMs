// <copyright file="EditorFormHelper.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.MushROMs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Windows.Forms;
    using Maseya.Editors;
    using Maseya.Editors.Collections;
    using Maseya.Helper.PixelFormat;
    using Maseya.Snes;
    using Maseya.Snes.Controls;

    public partial class EditorFormHelper : Component
    {
        private EditorSelector _editorSelector;

        public EditorFormHelper()
        {
            InitializeComponent();

            EditorFormCreator = new Dictionary<Type, Func<IEditor, Form>>();
            EditorForms = new Dictionary<IEditor, Form>();
            FormEditors = new Dictionary<Form, IEditor>();
            PaletteEditors = new List<IReadOnlyList<Color15BppBgr>>();

            AddCreatorFunction(
                typeof(ListEditor<Color15BppBgr>),
                CreatePaletteEditorForm);

            AddCreatorFunction(
                typeof(GfxEditor),
                CreateGfxEditorForm);
        }

        public event EventHandler<EditorFormEventArgs> EditorFormAdded;

        public event EventHandler<EditorFormEventArgs> EditorFormRemoved;

        public EditorSelector EditorSelector
        {
            get
            {
                return _editorSelector;
            }

            set
            {
                if (EditorSelector == value)
                {
                    return;
                }

                if (EditorSelector != null)
                {
                    EditorSelector.EditorAdded -= EditorAdded;
                    EditorSelector.EditorRemoved -= EditorRemoved;
                    EditorSelector.CurrentEditorChanged -=
                        CurrentEditorChanged;
                }

                _editorSelector = value;
                if (EditorSelector != null)
                {
                    EditorSelector.EditorAdded += EditorAdded;
                    EditorSelector.EditorRemoved += EditorRemoved;
                    EditorSelector.CurrentEditorChanged +=
                        CurrentEditorChanged;
                }
            }
        }

        public MainForm MainForm
        {
            get;
            set;
        }

        private Dictionary<Type, Func<IEditor, Form>> EditorFormCreator
        {
            get;
        }

        private List<IReadOnlyList<Color15BppBgr>> PaletteEditors
        {
            get;
        }

        private Dictionary<IEditor, Form> EditorForms
        {
            get;
        }

        private Dictionary<Form, IEditor> FormEditors
        {
            get;
        }

        public void AddCreatorFunction(
            Type type,
            Func<IEditor, Form> createEditorForm)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (createEditorForm is null)
            {
                throw new ArgumentNullException(nameof(createEditorForm));
            }

            if (!typeof(IEditor).IsAssignableFrom(type))
            {
                throw new ArgumentException();
            }

            EditorFormCreator.Add(type, createEditorForm);
        }

        public void AddEditor(IEditor editor)
        {
            if (EditorForms.TryGetValue(editor, out var form))
            {
                form.Focus();
                return;
            }

            if (!TryGetCreator(editor, out var createEditorForm))
            {
                throw new ArgumentException();
            }

            form = createEditorForm(editor);
            if (form is null)
            {
                throw new InvalidOperationException();
            }

            EditorForms[editor] = form;
            FormEditors[form] = editor;
            form.FormClosed += FormClosed;
            form.GotFocus += GotFocus;

            OnEditorFormAdded(new EditorFormEventArgs(form, editor));
        }

        public bool TryGetCreator(
            IEditor editor,
            out Func<IEditor, Form> createEditorForm)
        {
            if (editor is null)
            {
                throw new ArgumentNullException(nameof(editor));
            }

            return EditorFormCreator.TryGetValue(
                editor.GetType(),
                out createEditorForm);
        }

        public bool TryGetCreator(
            Type type,
            out Func<IEditor, Form> createEditorForm)
        {
            if (type.GetInterface(nameof(IEditor)) is null)
            {
                throw new InvalidCastException();
            }

            return EditorFormCreator.TryGetValue(
                type,
                out createEditorForm);
        }

        public Form GetForm(IEditor editor)
        {
            return EditorForms[editor];
        }

        protected virtual void OnEditorFormAdded(EditorFormEventArgs e)
        {
            EditorFormAdded?.Invoke(this, e);
        }

        protected virtual void OnEditorFormRemoved(EditorFormEventArgs e)
        {
            EditorFormRemoved?.Invoke(this, e);
        }

        private Form CreatePaletteEditorForm(IEditor editor)
        {
            var palette = editor as ListEditor<Color15BppBgr>;
            PaletteEditors.Add(palette);
            var result = new PaletteEditorForm
            {
                Palette = palette,
                Text = Path.GetFileName(editor.Path),
            };

            return result;
        }

        private Form CreateGfxEditorForm(IEditor editor)
        {
            var gfx = editor as IGfxEditor;
            var result = new GfxEditorForm
            {
                Gfx = gfx,
                Text = Path.GetFileName(editor.Path),
                Palette = GetFirstPalette(),
            };

            return result;
        }

        private IReadOnlyList<Color15BppBgr> GetFirstPalette()
        {
            if (PaletteEditors.Count > 0)
            {
                return PaletteEditors[0];
            }

            if (MainForm?.OpenFileHelper is null)
            {
                return null;
            }

            if (paletteOpenFileDialog.ShowDialog(MainForm) == DialogResult.OK)
            {
                MainForm.OpenFileHelper.OpenFile(
                    MainForm,
                    paletteOpenFileDialog.FileName);

                // Not sure if there's a better way to deal with this.
                return PaletteEditors.Count == 0 ? null : PaletteEditors[0];
            }

            return null;
        }

        private void GotFocus(object sender, EventArgs e)
        {
            EditorSelector.CurrentEditor = FormEditors[sender as Form];
        }

        private void FormClosed(object sender, FormClosedEventArgs e)
        {
            var form = sender as Form;
            var editor = FormEditors[form];
            EditorSelector.Items.Remove(editor);
        }

        private void CurrentEditorChanged(object sender, EditorEventArgs e)
        {
            var form = EditorForms[e.Editor];
            form.Focus();
        }

        private void EditorRemoved(object sender, EditorEventArgs e)
        {
            var form = EditorForms[e.Editor];
            EditorForms.Remove(e.Editor);
            FormEditors.Remove(form);
        }

        private void EditorAdded(object sender, EditorEventArgs e)
        {
            AddEditor(e.Editor);
        }
    }
}
