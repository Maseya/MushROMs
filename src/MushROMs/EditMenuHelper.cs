// <copyright file="EditMenuHelper.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.MushROMs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Forms;
    using Maseya.Controls.Editors;
    using Maseya.Editors;
    using static System.ComponentModel.DesignerSerializationVisibility;

    public partial class EditMenuHelper : Component, IEditorMenu
    {
        private EditorSelector _editorSelector;

        public EditMenuHelper()
        {
            InitializeComponent();

            var events = new List<(ToolStripItem, Action<EventArgs>)>()
            {
                (tsmUndo, OnUndoClick),
                (tsbUndo, OnUndoClick),
                (tsmRedo, OnRedoClick),
                (tsbRedo, OnRedoClick),
                (tsmCut, OnCutClick),
                (tsbCut, OnCutClick),
                (tsmCopy, OnCopyClick),
                (tsbCopy, OnCopyClick),
                (tsmPaste, OnPasteClick),
                (tsbPaste, OnPasteClick),
                (tsmDelete, OnDeleteClick),
                (tsmSelectAll, OnSelectAllClick),
                (tsmExportImage, OnExportImageClick),
            };

            foreach (var (tsi, action) in events)
            {
                tsi.Click += (s, e) => action(EventArgs.Empty);
            }
        }

        public EditMenuHelper(IContainer container)
            : this()
        {
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.Add(this);
        }

        public event EventHandler UndoClick;

        public event EventHandler RedoClick;

        public event EventHandler CutClick;

        public event EventHandler CopyClick;

        public event EventHandler PasteClick;

        public event EventHandler DeleteClick;

        public event EventHandler SelectAllClick;

        public event EventHandler ExportImageClick;

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

                    EditorSelector.CurrentEditorRemoved -=
                        CurrentEditorRemoved;
                }

                _editorSelector = value;
                if (EditorSelector != null)
                {
                    EditorSelector.EditorAdded += EditorAdded;
                    EditorSelector.EditorRemoved += EditorRemoved;

                    EditorSelector.CurrentEditorChanged +=
                        CurrentEditorChanged;

                    EditorSelector.CurrentEditorRemoved +=
                        CurrentEditorRemoved;
                }

                UpdateMenuEnabled();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public ToolStripItemCollection MenuStripItems
        {
            get
            {
                return menuStrip.Items;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public ToolStripItemCollection ToolStripItems
        {
            get
            {
                return toolStrip.Items;
            }
        }

        protected virtual void OnUndoClick(EventArgs e)
        {
            EditorSelector?.CurrentEditor.Undo();
            UndoClick?.Invoke(this, e);
        }

        protected virtual void OnRedoClick(EventArgs e)
        {
            EditorSelector?.CurrentEditor.Redo();
            RedoClick?.Invoke(this, e);
        }

        protected virtual void OnCutClick(EventArgs e)
        {
            EditorSelector?.CurrentEditor.Cut();
            CutClick?.Invoke(this, e);
        }

        protected virtual void OnCopyClick(EventArgs e)
        {
            EditorSelector?.CurrentEditor.Copy();
            CopyClick?.Invoke(this, e);
        }

        protected virtual void OnPasteClick(EventArgs e)
        {
            EditorSelector?.CurrentEditor.Paste();
            PasteClick?.Invoke(this, e);
        }

        protected virtual void OnDeleteClick(EventArgs e)
        {
            EditorSelector?.CurrentEditor.Delete();
            DeleteClick?.Invoke(this, e);
        }

        protected virtual void OnSelectAllClick(EventArgs e)
        {
            EditorSelector?.CurrentEditor.SelectAll();
            SelectAllClick?.Invoke(this, e);
        }

        protected virtual void OnExportImageClick(EventArgs e)
        {
            ExportImageClick?.Invoke(this, e);
        }

        private void EditorRemoved(object sender, EditorEventArgs e)
        {
            e.Editor.WriteDataComplete -= Editor_StateChanged;
            e.Editor.PasteComplete -= Editor_StateChanged;
            e.Editor.RedoComplete -= Editor_StateChanged;
            e.Editor.UndoComplete -= Editor_StateChanged;
            e.Editor.CutComplete -= Editor_StateChanged;
            e.Editor.DeleteComplete -= Editor_StateChanged;
        }

        private void EditorAdded(object sender, EditorEventArgs e)
        {
            e.Editor.WriteDataComplete += Editor_StateChanged;
            e.Editor.PasteComplete += Editor_StateChanged;
            e.Editor.RedoComplete += Editor_StateChanged;
            e.Editor.UndoComplete += Editor_StateChanged;
            e.Editor.CutComplete += Editor_StateChanged;
            e.Editor.DeleteComplete += Editor_StateChanged;
        }

        private void Editor_StateChanged(object sender, EventArgs e)
        {
            UpdateMenuEnabled();
        }

        private void UpdateMenuEnabled()
        {
            var current = EditorSelector?.CurrentEditor;
            if (current == null)
            {
                tsmExportImage.Enabled =
                tsmUndo.Enabled =
                tsbUndo.Enabled =
                tsmRedo.Enabled =
                tsbRedo.Enabled =
                tsmCut.Enabled =
                tsbCut.Enabled =
                tsmCopy.Enabled =
                tsbCopy.Enabled =
                tsmPaste.Enabled =
                tsbPaste.Enabled =
                tsmDelete.Enabled =
                tsmSelectAll.Enabled = false;
            }
            else
            {
                tsmExportImage.Enabled = true;

                tsmUndo.Enabled =
                tsbUndo.Enabled = current.CanUndo;

                tsmRedo.Enabled =
                tsbRedo.Enabled = current.CanRedo;

                tsmCut.Enabled =
                tsbCut.Enabled = current.CanCut;

                tsmCopy.Enabled =
                tsbCopy.Enabled = current.CanCopy;

                tsmPaste.Enabled =
                tsbPaste.Enabled = current.CanPaste;

                tsmDelete.Enabled = current.CanDelete;
                tsmSelectAll.Enabled = current.CanSelectAll;
            }
        }

        private void CurrentEditorChanged(object sender, EditorEventArgs e)
        {
            UpdateMenuEnabled();
        }

        private void CurrentEditorRemoved(object sender, EventArgs e)
        {
            UpdateMenuEnabled();
        }
    }
}
