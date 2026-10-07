// <copyright file="ObjectListForm.Designer.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.Win.Dialogs.BaseForms
{
    partial class ObjectListForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tlsMenu = new ToolStrip();
            tsbAdd = new ToolStripButton();
            tsbDelete = new ToolStripButton();
            tsbClear = new ToolStripButton();
            toolStripButton1 = new ToolStripButton();
            toolStripButton2 = new ToolStripButton();
            toolStripButton3 = new ToolStripButton();
            lvwObjects = new MushROMs.Win.Controls.ListViewNF();
            chdHex = new ColumnHeader();
            chdPage = new ColumnHeader();
            chdPosition = new ColumnHeader();
            chdType = new ColumnHeader();
            tlsMenu.SuspendLayout();
            SuspendLayout();
            //
            // tlsMenu
            //
            tlsMenu.GripStyle = ToolStripGripStyle.Hidden;
            tlsMenu.ImageScalingSize = new Size(20, 20);
            tlsMenu.Items.AddRange(new ToolStripItem[] { tsbAdd, tsbDelete, tsbClear, toolStripButton1, toolStripButton2, toolStripButton3 });
            tlsMenu.Location = new Point(0, 0);
            tlsMenu.Name = "tlsMenu";
            tlsMenu.Size = new Size(578, 27);
            tlsMenu.TabIndex = 1;
            tlsMenu.Text = "...";
            //
            // tsbAdd
            //
            tsbAdd.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbAdd.Image = Properties.Resources.plus_solid;
            tsbAdd.ImageTransparentColor = Color.Magenta;
            tsbAdd.Name = "tsbAdd";
            tsbAdd.Size = new Size(24, 24);
            tsbAdd.Text = "Add";
            tsbAdd.Click += Add_Click;
            //
            // tsbDelete
            //
            tsbDelete.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbDelete.Image = Properties.Resources.minus_solid;
            tsbDelete.ImageTransparentColor = Color.Magenta;
            tsbDelete.Name = "tsbDelete";
            tsbDelete.Size = new Size(24, 24);
            tsbDelete.Text = "Delete";
            tsbDelete.Click += Delete_Click;
            //
            // tsbClear
            //
            tsbClear.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbClear.ImageTransparentColor = Color.Magenta;
            tsbClear.Name = "tsbClear";
            tsbClear.Size = new Size(24, 24);
            tsbClear.Text = "Clear";
            tsbClear.Click += Clear_Click;
            //
            // toolStripButton1
            //
            toolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton1.ImageTransparentColor = Color.Magenta;
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Size = new Size(24, 24);
            toolStripButton1.Text = "toolStripButton1";
            toolStripButton1.Click += MoveDown_Click;
            //
            // toolStripButton2
            //
            toolStripButton2.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton2.ImageTransparentColor = Color.Magenta;
            toolStripButton2.Name = "toolStripButton2";
            toolStripButton2.Size = new Size(24, 24);
            toolStripButton2.Text = "toolStripButton2";
            toolStripButton2.Click += MoveUp_Click;
            //
            // toolStripButton3
            //
            toolStripButton3.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton3.Enabled = false;
            toolStripButton3.ImageTransparentColor = Color.Magenta;
            toolStripButton3.Name = "toolStripButton3";
            toolStripButton3.Size = new Size(24, 24);
            toolStripButton3.Text = "toolStripButton3";
            //
            // lvwObjects
            //
            lvwObjects.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lvwObjects.AutoArrange = false;
            lvwObjects.Columns.AddRange(new ColumnHeader[] { chdHex, chdPage, chdPosition, chdType });
            lvwObjects.FullRowSelect = true;
            lvwObjects.GridLines = true;
            lvwObjects.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            lvwObjects.LabelWrap = false;
            lvwObjects.Location = new Point(0, 32);
            lvwObjects.Margin = new Padding(4, 4, 4, 4);
            lvwObjects.MultiSelect = false;
            lvwObjects.Name = "lvwObjects";
            lvwObjects.ShowGroups = false;
            lvwObjects.Size = new Size(578, 353);
            lvwObjects.TabIndex = 0;
            lvwObjects.UseCompatibleStateImageBehavior = false;
            lvwObjects.View = View.Details;
            lvwObjects.ItemSelectionChanged += Objects_ItemSelectionChanged;
            lvwObjects.SelectedIndexChanged += Objects_SelectedIndexChanged;
            lvwObjects.MouseDoubleClick += Objects_MouseDoubleClick;
            //
            // chdHex
            //
            chdHex.Text = "Hex";
            chdHex.Width = 57;
            //
            // chdPage
            //
            chdPage.Text = "Page";
            chdPage.Width = 63;
            //
            // chdPosition
            //
            chdPosition.Text = "Position";
            chdPosition.Width = 70;
            //
            // chdType
            //
            chdType.Text = "Type";
            chdType.Width = 600;
            //
            // ObjectListForm
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(578, 386);
            Controls.Add(tlsMenu);
            Controls.Add(lvwObjects);
            Margin = new Padding(4, 4, 4, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(594, 421);
            Name = "ObjectListForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Object List";
            tlsMenu.ResumeLayout(false);
            tlsMenu.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Controls.ListViewNF lvwObjects;
        private System.Windows.Forms.ColumnHeader chdHex;
        private System.Windows.Forms.ColumnHeader chdPage;
        private System.Windows.Forms.ColumnHeader chdPosition;
        private System.Windows.Forms.ColumnHeader chdType;
        private System.Windows.Forms.ToolStrip tlsMenu;
        private System.Windows.Forms.ToolStripButton tsbAdd;
        private System.Windows.Forms.ToolStripButton tsbDelete;
        private System.Windows.Forms.ToolStripButton tsbClear;
        private ToolStripButton toolStripButton1;
        private ToolStripButton toolStripButton2;
        private ToolStripButton toolStripButton3;
    }
}
