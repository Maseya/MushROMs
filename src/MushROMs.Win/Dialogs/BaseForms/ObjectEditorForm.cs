// <copyright file="ObjectEditorForm.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.Win.Dialogs.BaseForms;

using System;
using System.Windows.Forms;

using Core;

using Maseya.Smas.Smb1.AreaData.ObjectData;

internal partial class ObjectEditorForm : Form
{
    private UIAreaObjectCommand _command;

    public ObjectEditorForm()
    {
        InitializeComponent();
    }

    public event EventHandler? AreaPlatformTypeChanged;

    public event EventHandler? AreaObjectCommandChanged;

    public AreaPlatformType AreaPlatformType
    {
        get
        {
            return objectEditorUserControl.AreaPlatformType;
        }

        set
        {
            objectEditorUserControl.AreaPlatformType = value;
        }
    }

    public UIAreaObjectCommand UIAreaObjectCommand
    {
        get
        {
            return _command;
        }

        set
        {
            if (UIAreaObjectCommand == value)
            {
                return;
            }

            SetCommandInternal(value);
        }
    }

    private bool IsValidInput
    {
        get
        {
            return btnOK.Enabled;
        }

        set
        {
            btnOK.Enabled = value;
        }
    }

    private UIAreaObjectCommand ControlCommand
    {
        get
        {
            return objectEditorUserControl.UIAreaObjectCommand;
        }

        set
        {
            objectEditorUserControl.UIAreaObjectCommand = value;
        }
    }

    private bool IsControlCommandUpdating
    {
        get;
        set;
    }

    private UIAreaObjectCommand TextCommand
    {
        get
        {
            return objectEditorTextBox.UIAreaObjectCommand;
        }

        set
        {
            objectEditorTextBox.UIAreaObjectCommand = value;
        }
    }

    private bool IsTextCommandUpdating
    {
        get;
        set;
    }

    protected virtual void OnAreaPlatformTypeChanged(EventArgs e)
    {
        AreaPlatformTypeChanged?.Invoke(this, e);
    }

    protected virtual void OnAreaObjectCommandChanged(EventArgs e)
    {
        AreaObjectCommandChanged?.Invoke(this, e);
    }

    private void SetCommandInternal(UIAreaObjectCommand value)
    {
        _command = value;
        if (!IsControlCommandUpdating)
        {
            ControlCommand = value;
        }

        if (!IsTextCommandUpdating)
        {
            TextCommand = value;
        }

        OnAreaObjectCommandChanged(EventArgs.Empty);
    }

    private void ObjectEditorUserControl_AreaPlatformTypeChanged(object sender, EventArgs e)
    {
        OnAreaPlatformTypeChanged(EventArgs.Empty);
    }

    private void ObjectEditorUserControl_AreaObjectCommandChanged(object sender, EventArgs e)
    {
        if (!IsControlCommandUpdating)
        {
            IsControlCommandUpdating = true;
            SetCommandInternal(ControlCommand);
            IsControlCommandUpdating = false;
        }
    }

    private void ObjectEditorTextBox_AreaObjectCommandChanged(object sender, EventArgs e)
    {
        if (!IsTextCommandUpdating)
        {
            IsTextCommandUpdating = true;
            SetCommandInternal(TextCommand);
            IsTextCommandUpdating = false;
        }
    }

    private void ObjectEditorTextBox_IsValidCommandChanged(object sender, EventArgs e)
    {
        IsValidInput = objectEditorTextBox.IsValidCommand;
    }
}
