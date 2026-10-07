// <copyright file="ObjectListView.Designer.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.Win.Views;

partial class ObjectListView
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
        components = new System.ComponentModel.Container();
        objectListDialog = new Dialogs.ObjectListDialog(components);
        //
        // objectListDialog
        //
        objectListDialog.Owner = null;
        objectListDialog.ShowHelp = false;
        objectListDialog.Title = "Object List";
        objectListDialog.SelectedIndexChanged += List_SelectedIndexChanged;
        objectListDialog.EditItem += Dialog_EditItem;
        objectListDialog.AddItem_Click += Add_Click;
        objectListDialog.DeleteItem_Click += Delete_Click;
        objectListDialog.ClearItems_Click += Clear_Click;
        objectListDialog.MoveItemDown_Click += MovieDown_Click;
        objectListDialog.MoveItemUp_Click += MoveUp_Click;
        objectListDialog.VisibleChanged += Dialog_VisibleChanged;
    }

    #endregion

    private Dialogs.ObjectListDialog objectListDialog;
}
