// <copyright file="ObjectListView.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.Win.Views;

using System;
using System.Collections.Generic;
using System.ComponentModel;

using MushROMs.Core;

using Core.Views;

using Maseya.Smas.Smb1.AreaData.ObjectData;

using static System.ComponentModel.DesignerSerializationVisibility;
using static Dialogs.BaseForms.ObjectListForm;

public sealed partial class ObjectListView : Component, IObjectListView
{
    public ObjectListView()
    {
        InitializeComponent();
        InitializeComponent2();
    }

    public ObjectListView(IContainer container)
    {
        container.Add(this);

        InitializeComponent();
        InitializeComponent2();
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
    public int SelectedIndex
    {
        get
        {
            return objectListDialog.SelectedIndex;
        }

        set
        {
            objectListDialog.SelectedIndex = value;
        }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(Hidden)]
    public AreaPlatformType AreaPlatformType
    {
        get
        {
            return objectListDialog.AreaPlatformType;
        }

        set
        {
            objectListDialog.AreaPlatformType = value;
        }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(Hidden)]
    public ItemCollection Items
    {
        get
        {
            return objectListDialog.Items;
        }
    }

    IList<UIAreaObjectCommand> IObjectListView.Items
    {
        get
        {
            return Items;
        }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(Hidden)]
    public bool Visible
    {
        get
        {
            return objectListDialog.Visible;
        }

        set
        {
            objectListDialog.Visible = value;
        }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(Hidden)]
    public Form Owner
    {
        get
        {
            return objectListDialog.Owner;
        }

        set
        {
            objectListDialog.Owner = value;
        }
    }

    private void InitializeComponent2()
    {
        objectListDialog.AreaPlatformTypeChanged += ObjectListDialog_AreaPlatformTypeChanged;
        objectListDialog.SelectedIndexChanged += ObjectListDialog_SelectedIndexChanged;
    }

    private void ObjectListDialog_AreaPlatformTypeChanged(object? sender, EventArgs e)
    {
        AreaPlatformTypeChanged?.Invoke(this, EventArgs.Empty);
    }

    private void ObjectListDialog_SelectedIndexChanged(object? sender, EventArgs e)
    {
        SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
    }

    private void Add_Click(object? sender, EventArgs e)
    {
        AddItem_Click?.Invoke(this, EventArgs.Empty);
    }

    private void Delete_Click(object? sender, EventArgs e)
    {
        DeleteItem_Click?.Invoke(this, EventArgs.Empty);
    }

    private void Clear_Click(object? sender, EventArgs e)
    {
        ClearItems_Click?.Invoke(this, EventArgs.Empty);
    }

    private void MovieDown_Click(object? sender, EventArgs e)
    {
        MoveItemDown_Click?.Invoke(this, EventArgs.Empty);
    }

    private void MoveUp_Click(object? sender, EventArgs e)
    {
        MoveItemUp_Click?.Invoke(this, EventArgs.Empty);
    }

    private void List_SelectedIndexChanged(object sender, EventArgs e)
    {
        SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
    }

    private void Dialog_EditItem(object sender, EventArgs e)
    {
        EditItem?.Invoke(this, EventArgs.Empty);
    }

    private void Dialog_VisibleChanged(object sender, EventArgs e)
    {
        VisibleChanged?.Invoke(this, EventArgs.Empty);
    }
}
