// <copyright file="MainForm.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.MushROMs
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Forms;
    using Maseya.Controls.Editors;
    using Maseya.MushROMs.Properties;

    public sealed partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            InitializeCustomMenu();
            InitializeAssociations();
            EditorFormHelper.MainForm = this;
        }

        public MainForm(IContainer container)
            : this()
        {
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.Add(this);
        }

        public OpenFileHelper OpenFileHelper
        {
            get
            {
                return openFileHelper;
            }
        }

        public RecentFileCollection RecentFiles
        {
            get
            {
                return fileMenuHelper.RecentFiles;
            }
        }

        public string Status
        {
            get
            {
                return tssStatus.Text;
            }

            set
            {
                tssStatus.Text = value;
            }
        }

        public void AddMenus(IEditorMenu editorMenu)
        {
            InsertMenuStripItems(editorMenu.MenuStripItems);
            InsertToolStripItems(
                tspMain.Items,
                tspMain.Tag,
                editorMenu.ToolStripItems);
        }

        public void ExportImage()
        {
            ExceptionMessageBox.ShowException(
                new NotImplementedException());
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            Settings.Default.RecentFiles.Clear();
            Settings.Default.RecentFiles.AddRange(
                new List<string>(fileMenuHelper.RecentFiles).ToArray());

            Settings.Default.Save();
            base.OnFormClosed(e);
        }

        private void InitializeAssociations()
        {
            foreach (var entry in snesNewFileAssociations.GetAssociations())
            {
                NewFileHelper.AddAssociation(entry);
            }

            foreach (var entry in snesOpenFileAssociations.GetAssociations())
            {
                openFileHelper.AddOpenFileAssociation(entry);
            }
        }

        private void InitializeCustomMenu()
        {
            mnuMain.Tag = tsmTools;
            tsmFile.Tag = fileSeparator;
            tspMain.Tag = toolBarSeparator;
            AddMenus(fileMenuHelper);
            AddMenus(editMenuHelper);
        }

        private int GetToolStripMenuItem(
            ToolStripItemCollection menuItems,
            ToolStripMenuItem item)
        {
            for (var i = 0; i < menuItems.Count; i++)
            {
                var tsi = menuItems[i];
                if (StringComparer.CurrentCultureIgnoreCase.Equals(
                    tsi.Text.Replace("&", String.Empty),
                    item.Text.Replace("&", String.Empty)))
                {
                    return i;
                }
            }

            return -1;
        }

        private void InsertMenuStripItems(ToolStripItemCollection menuItems)
        {
            for (var i = 0; i < menuItems.Count; i++)
            {
                var tsi = menuItems[i] as ToolStripMenuItem;
                var index = GetToolStripMenuItem(mnuMain.Items, tsi);
                if (index == -1)
                {
                    continue;
                }

                var owner = mnuMain.Items[index] as ToolStripMenuItem;
                InsertToolStripItems(
                    owner.DropDownItems,
                    owner.Tag,
                    tsi.DropDownItems);
            }
        }

        private void InsertToolStripItems(
            IList owner,
            object tag,
            ToolStripItemCollection collection)
        {
            var index = tag is ToolStripSeparator separator
                ? owner.IndexOf(separator)
                : owner.Count;

            if (index != 0)
            {
                owner.Insert(
                    index++,
                    new ToolStripSeparator());
            }

            for (var i = collection.Count; --i >= 0;)
            {
                owner.Insert(
                    index,
                    collection[i]);
            }
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void About_Click(object sender, EventArgs e)
        {
            Program.About(this);
        }

        private void EditorFormAdded(object sender, EditorFormEventArgs e)
        {
            e.Form.MdiParent = this;
            e.Form.Show();
        }

        private void ExportImage_Click(object sender, EventArgs e)
        {
            ExportImage();
        }
    }
}
