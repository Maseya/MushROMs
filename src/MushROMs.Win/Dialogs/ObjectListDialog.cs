// <copyright file="ObjectListDialog.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.Win.Dialogs;

using System;
using System.ComponentModel;
using System.Windows.Forms;

using BaseForms;

using Controls;

using Maseya.Smas.Smb1.AreaData.ObjectData;

using static System.ComponentModel.DesignerSerializationVisibility;
using static BaseForms.ObjectListForm;

public class ObjectListDialog : DialogProxy
{
    public ObjectListDialog()
        : base()
    {
        ObjectListWindow = new ObjectListForm();
        ObjectListWindow.AreaPlatformTypeChanged += ObjectListWindow_AreaPlatformTypeChanged;
        ObjectListWindow.SelectedIndexChanged += ObjectListWindow_SelectedIndexChanged;
        ObjectListWindow.EditItem += ObjectListWindow_EditItem;
        ObjectListWindow.AddItem_Click += ObjectListWindow_AddItem_Click;
        ObjectListWindow.DeleteItem_Click += ObjectListWindow_DeleteItem_Click;
        ObjectListWindow.ClearItems_Click += ObjectListWindow_ClearItems_Click;
        ObjectListWindow.FormClosing += ObjectListWindow_FormClosing;
        ObjectListWindow.VisibleChanged += ObjectListWindow_VisibleChanged;
    }

    public ObjectListDialog(IContainer container)
        : this()
    {
        container.Add(this);
    }

    public event EventHandler? AreaPlatformTypeChanged;

    public event EventHandler? SelectedIndexChanged;

    public event EventHandler? EditItem;

    public event EventHandler? AddItem_Click;

    public event EventHandler? DeleteItem_Click;

    public event EventHandler? ClearItems_Click;

    public event EventHandler? MoveItemDown_Click;

    public event EventHandler? MoveItemUp_Click;

    public event EventHandler? VisibleChanged;

    [Browsable(false)]
    [DesignerSerializationVisibility(Hidden)]
    public AreaPlatformType AreaPlatformType
    {
        get
        {
            return ObjectListWindow.AreaPlatformType;
        }

        set
        {
            ObjectListWindow.AreaPlatformType = value;
        }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(Hidden)]
    public int SelectedIndex
    {
        get
        {
            return ObjectListWindow.SelectedIndex;
        }

        set
        {
            ObjectListWindow.SelectedIndex = value;
        }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(Hidden)]
    public ItemCollection Items
    {
        get
        {
            return ObjectListWindow.Items;
        }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(Hidden)]
    public bool Visible
    {
        get
        {
            return ObjectListWindow.Visible;
        }

        set
        {
            ObjectListWindow.Visible = value;
        }
    }

    public Form Owner
    {
        get
        {
            return ObjectListWindow.Owner;
        }

        set
        {
            ObjectListWindow.Owner = value;
        }
    }

    protected override Form BaseForm
    {
        get
        {
            return ObjectListWindow;
        }
    }

    private ObjectListForm ObjectListWindow
    {
        get;
    }

    private void ObjectListWindow_AreaPlatformTypeChanged(object? sender, EventArgs e)
    {
        AreaPlatformTypeChanged?.Invoke(this, EventArgs.Empty);
    }

    private void ObjectListWindow_SelectedIndexChanged(object? sender, EventArgs e)
    {
        SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
    }

    private void ObjectListWindow_EditItem(object? sender, EventArgs e)
    {
        EditItem?.Invoke(this, EventArgs.Empty);
    }

    private void ObjectListWindow_AddItem_Click(object? sender, EventArgs e)
    {
        AddItem_Click?.Invoke(this, EventArgs.Empty);
    }

    private void ObjectListWindow_DeleteItem_Click(object? sender, EventArgs e)
    {
        DeleteItem_Click?.Invoke(this, EventArgs.Empty);
    }

    private void ObjectListWindow_ClearItems_Click(object? sender, EventArgs e)
    {
        ClearItems_Click?.Invoke(this, EventArgs.Empty);
    }

    private void ObjectListWindow_MoveItemDown_Click(object? sender, EventArgs e)
    {
        MoveItemDown_Click?.Invoke(this, EventArgs.Empty);
    }

    private void ObjectListWindow_MoveItemUp_Click(object? sender, EventArgs e)
    {
        MoveItemUp_Click?.Invoke(this, EventArgs.Empty);
    }

    private void ObjectListWindow_VisibleChanged(object? sender, EventArgs e)
    {
        VisibleChanged?.Invoke(this, EventArgs.Empty);
    }

    private void ObjectListWindow_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (Owner != null && e.CloseReason == CloseReason.UserClosing)
        {
            ObjectListWindow.Visible = false;
            e.Cancel = true;
        }
    }
}
