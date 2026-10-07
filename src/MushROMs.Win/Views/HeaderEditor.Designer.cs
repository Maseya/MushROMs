// <copyright file="HeaderEditor.Designer.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.Win.Views
{
    partial class HeaderEditor
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.headerEditorDialog = new MushROMs.Win.Dialogs.HeaderEditorDialog(this.components);
            //
            // headerEditorDialog
            //
            this.headerEditorDialog.ShowHelp = false;
            this.headerEditorDialog.Title = "Edit Header";
            this.headerEditorDialog.AreaHeaderChanged += new System.EventHandler(this.HeaderEditorDialog_AreaHeaderChanged);

        }

        #endregion

        private Dialogs.HeaderEditorDialog headerEditorDialog;
    }
}
