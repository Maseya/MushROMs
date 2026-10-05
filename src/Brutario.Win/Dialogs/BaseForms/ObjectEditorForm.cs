// <copyright file="ObjectEditorForm.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Brutario.Win.Dialogs.BaseForms;

using System;
using System.Globalization;
using System.Windows.Forms;

using Core;

using Maseya.Smas.Smb1.AreaData.ObjectData;

internal partial class ObjectEditorForm : Form
{
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

    public UIAreaObjectCommand AreaObjectCommand
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

    private bool UseManualInput
    {
        get
        {
            return chkUseManualInput.Checked;
        }

        set
        {
            chkUseManualInput.Checked = value;
        }
    }

    private bool UICommandIsUpdating
    {
        get;
        set;
    }

    private UIAreaObjectCommand BinaryCommand
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

    private void UpdateValidInputFlag()
    {
        // If we're using the list and check boxes, then the input is always valid by
        // their restraints. Otherwise, if we're entering the value manually, then we
        // must check that text is valid.
        /*
        IsValidInput =
            !UseManualInput || TryGetBinaryCommand(tbxManualInput.Text, out var _);
        */
    }

    private void ManualInput_TextChanged(object? sender, EventArgs e)
    {
        UpdateValidInputFlag();
        if (!UICommandIsUpdating && IsValidInput)
        {
            AreaObjectCommand = BinaryCommand;
        }
    }

    private void UseManualInput_CheckedChanged(object? sender, EventArgs e)
    {
        UpdateValidInputFlag();
    }

    private void OnAreaPlatformTypeChanged(EventArgs e)
    {
        AreaPlatformTypeChanged?.Invoke(this, e);
    }

    private void OnAreaObjectCommandChanged(EventArgs e)
    {
        AreaObjectCommandChanged?.Invoke(this, e);
    }

    private void ObjectEditorUserControl_AreaPlatformTypeChanged(object sender, EventArgs e)
    {
        OnAreaPlatformTypeChanged(EventArgs.Empty);
    }

    private void ObjectEditorUserControl_AreaObjectCommandChanged(object sender, EventArgs e)
    {
        OnAreaObjectCommandChanged(EventArgs.Empty);
    }
}
