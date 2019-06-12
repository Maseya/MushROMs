// <copyright file="FileMenuHelper.cs" company="Public Domain">
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
    using Maseya.Controls.Editors;
    using Maseya.Editors;
    using Maseya.Helper;
    using Maseya.MushROMs.Properties;
    using static System.ComponentModel.DesignerSerializationVisibility;
    using SR = Maseya.Helper.StringHelper;

    public partial class FileMenuHelper : Component, IEditorMenu
    {
        private NewFileHelper _newFileHelper;

        private OpenFileHelper _openFileHelper;

        private EditorSelector _editorSelector;

        public FileMenuHelper()
        {
            InitializeComponent();
            tsmClearRecentFiles.Click += ClearRecentFiles_Click;
            cmiClearRecentFiles.Click += ClearRecentFiles_Click;

            RecentFiles = new RecentFileCollection(
                Settings.Default.MaxRecentFiles);

            RecentFiles.CollectionChanged +=
                RecentFileCollectionChanged;

            var events = new List<(ToolStripItem, Action<EventArgs>)>()
            {
                (tsmNewFile, OnNewFileClick),
                (tsbNewFile, OnNewFileClick),
                (tsmOpenFile, OnOpenFileClick),
                (tsbOpenFile, OnOpenFileClick),
                (tsmOpenFileAs, OnOpenFileAsClick),
                (tsmCloseFile, OnCloseFileClick),
                (tsmSaveFile, OnSaveFileClick),
                (tsbSaveFile, OnSaveFileClick),
                (tsmSaveFileAs, OnSaveFileAsClick),
                (tsmSaveAll, OnSaveAllClick),
                (tsbSaveAll, OnSaveAllClick),
            };

            foreach (var (tsi, action) in events)
            {
                tsi.Click += (s, e) => action(EventArgs.Empty);
            }
        }

        public FileMenuHelper(IContainer container)
            : this()
        {
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.Add(this);
        }

        public event EventHandler NewFileClick;

        public event EventHandler OpenFileClick;

        public event EventHandler OpenFileAsClick;

        public event EventHandler<PathEventArgs> OpenRecentClick;

        public event EventHandler<PathEventArgs> OpenRecentAsClick;

        public event EventHandler<PathEventArgs> RemoveRecentFileClick;

        public event EventHandler CloseFileClick;

        public event EventHandler SaveFileClick;

        public event EventHandler SaveFileAsClick;

        public event EventHandler SaveAllClick;

        public NewFileHelper NewFileHelper
        {
            get
            {
                return _newFileHelper;
            }

            set
            {
                if (NewFileHelper == value)
                {
                    return;
                }

                if (NewFileHelper != null)
                {
                    NewFileHelper.EditorCreated -= EditorCreated;
                }

                _newFileHelper = value;
                if (NewFileHelper != null)
                {
                    NewFileHelper.EditorCreated += EditorCreated;
                }
            }
        }

        public OpenFileHelper OpenFileHelper
        {
            get
            {
                return _openFileHelper;
            }

            set
            {
                if (OpenFileHelper == value)
                {
                    return;
                }

                if (OpenFileHelper != null)
                {
                    OpenFileHelper.EditorOpened -= EditorCreated;
                }

                _openFileHelper = value;
                if (OpenFileHelper != null)
                {
                    OpenFileHelper.EditorOpened += EditorCreated;
                }
            }
        }

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
                }

                _editorSelector = value;
                if (EditorSelector != null)
                {
                    EditorSelector.EditorAdded += EditorAdded;
                    EditorSelector.EditorRemoved += EditorRemoved;
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public RecentFileCollection RecentFiles
        {
            get;
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

        protected virtual void OnNewFileClick(EventArgs e)
        {
            NewFileClick?.Invoke(this, e);
            NewFileHelper?.NewFile();
        }

        protected virtual void OnOpenFileClick(EventArgs e)
        {
            OpenFileClick?.Invoke(this, e);
            OpenFileHelper?.OpenFile();
        }

        protected virtual void OnOpenFileAsClick(EventArgs e)
        {
            OpenFileAsClick?.Invoke(this, e);
            OpenFileHelper?.OpenFileAs();
        }

        protected virtual void OnOpenRecentClick(PathEventArgs e)
        {
            if (e is null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            OpenRecentClick?.Invoke(this, e);
            OpenFileHelper?.OpenFile(e.Path);
        }

        protected virtual void OnOpenRecentAsClick(PathEventArgs e)
        {
            if (e is null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            OpenRecentAsClick?.Invoke(this, e);
            OpenFileHelper?.OpenFileAs(e.Path);
        }

        protected virtual void OnRemoveRecentFileClick(PathEventArgs e)
        {
            if (e is null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            RemoveRecentFileClick?.Invoke(this, e);
            RecentFiles.Remove(e.Path);
        }

        protected virtual void OnCloseFileClick(EventArgs e)
        {
            CloseFileClick?.Invoke(this, e);
            EditorSelector?.RemoveCurrentEditor();
        }

        protected virtual void OnSaveFileClick(EventArgs e)
        {
            SaveFileClick?.Invoke(this, e);
        }

        protected virtual void OnSaveFileAsClick(EventArgs e)
        {
            SaveFileAsClick?.Invoke(this, e);
        }

        protected virtual void OnSaveAllClick(EventArgs e)
        {
            SaveAllClick?.Invoke(this, e);
        }

        private void EditorAdded(object sender, EditorEventArgs e)
        {
            UpdateMenuEnabled();
        }

        private void EditorRemoved(object sender, EditorEventArgs e)
        {
            UpdateMenuEnabled();
        }

        private void EditorCreated(object sender, EditorEventArgs e)
        {
            AddEditor(e.Editor);
        }

        private void AddEditor(IEditor editor)
        {
            if (editor is null)
            {
                throw new ArgumentNullException(nameof(editor));
            }

            var path = editor.Path;
            if (EditorSelector.Items.TryGetValue(path, out var oldEditor))
            {
                EditorSelector.CurrentEditor = oldEditor;
            }
            else
            {
                EditorSelector.Items.Add(path, editor);
            }

            if (File.Exists(path))
            {
                RecentFiles.Add(path);
            }
        }

        private void UpdateMenuEnabled()
        {
            tsmSaveFile.Enabled =
            tsbSaveFile.Enabled =
            tsmSaveFileAs.Enabled =
            tsmSaveAll.Enabled =
            tsbSaveAll.Enabled =
            tsmCloseFile.Enabled = EditorSelector.Items.Count > 0;
        }

        private void UpdateRecentFiles()
        {
            tsmOpenRecent.Enabled =
            tsbOpenRecent.Enabled = RecentFiles.Count > 0;

            Clear(tsmOpenRecent.DropDownItems);
            AddItems(
                tsmOpenRecent.DropDownItems,
                tsmOpenRecent.Tag as string);

            Clear(tsbOpenRecent.DropDownItems);
            AddItems(
                tsbOpenRecent.DropDownItems,
                tsbOpenRecent.Tag as string);

            void AddItems(
                ToolStripItemCollection collection,
                string format)
            {
                for (var i = 0; i < RecentFiles.Count; i++)
                {
                    var path = RecentFiles[i];
                    var text = SR.GetString(
                        format,
                        i + 1,
                        path);

                    var tsi = new RecentFileToolStripItem
                    {
                        Text = text,
                    };

                    tsi.Click += (sender, e) =>
                    {
                        tsbOpenRecent.DropDown.Hide();
                        OnOpenRecentClick(new PathEventArgs(path));
                    };

                    tsi.Open += (sender, e) =>
                    {
                        tsbOpenRecent.DropDown.Hide();
                        OnOpenRecentClick(new PathEventArgs(path));
                    };

                    tsi.OpenAs += (sender, e) =>
                    {
                        tsbOpenRecent.DropDown.Hide();
                        OnOpenRecentAsClick(new PathEventArgs(path));
                    };

                    tsi.Remove += (sender, e) =>
                    {
                        tsbOpenRecent.DropDown.Hide();
                        OnRemoveRecentFileClick(new PathEventArgs(path));
                    };

                    collection.Insert(i, tsi);
                }
            }

            void Clear(ToolStripItemCollection collection)
            {
                for (var i = collection.Count - 2; --i >= 0;)
                {
                    collection.RemoveAt(i);
                }
            }
        }

        private void RecentFileCollectionChanged(object sender, EventArgs e)
        {
            UpdateRecentFiles();
        }

        private void ClearRecentFiles_Click(object sender, EventArgs e)
        {
            RecentFiles.Clear();
        }
    }
}
