// <copyright file="ObjectEditorDialog.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Brutario.Win.Dialogs;

using System;
using System.ComponentModel;
using System.Windows.Forms;

using BaseForms;

using Controls;

using Core;

using Maseya.Smas.Smb1.AreaData.ObjectData;

using static System.ComponentModel.DesignerSerializationVisibility;

public sealed class ObjectEditorDialog : DialogProxy
{
    public ObjectEditorDialog()
    {
        ObjectEditorForm = new ObjectEditorForm();
        ObjectEditorForm.AreaObjectCommandChanged +=
            ObjectEditorForm_AreaObjectCommandChanged;
        ObjectEditorForm.AreaPlatformTypeChanged +=
            ObjectEditorForm_AreaPlatformTypeChanged;
    }

    public ObjectEditorDialog(IContainer container)
        : this()
    {
        container.Add(this);
    }

    public event EventHandler? AreaPlatformTypeChanged;

    public event EventHandler? AreaObjectCommandChanged;

    [Browsable(false)]
    [DesignerSerializationVisibility(Hidden)]
    public AreaPlatformType AreaPlatformType
    {
        get
        {
            return ObjectEditorForm.AreaPlatformType;
        }

        set
        {
            ObjectEditorForm.AreaPlatformType = value;
        }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(Hidden)]
    public UIAreaObjectCommand UIAreaObjectCommand
    {
        get
        {
            return ObjectEditorForm.UIAreaObjectCommand;
        }

        set
        {
            ObjectEditorForm.UIAreaObjectCommand = value;
        }
    }

    protected override Form BaseForm
    {
        get
        {
            return ObjectEditorForm;
        }
    }

    private ObjectEditorForm ObjectEditorForm
    {
        get;
    }

    private void ObjectEditorForm_AreaPlatformTypeChanged(object? sender, EventArgs e)
    {
        AreaPlatformTypeChanged?.Invoke(this, EventArgs.Empty);
    }

    private void ObjectEditorForm_AreaObjectCommandChanged(object? sender, EventArgs e)
    {
        AreaObjectCommandChanged?.Invoke(this, EventArgs.Empty);
    }
}
