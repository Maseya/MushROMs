// <copyright file="SpriteEditorForm.cs" organization="Maseya">
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

using Maseya.Smas.Smb1.AreaData.SpriteData;

public partial class SpriteEditorForm : Form
{
    private UIAreaSpriteCommand _command;

    public SpriteEditorForm()
    {
        InitializeComponent();
    }

    public event EventHandler? AreaSpriteCommandChanged;

    public UIAreaSpriteCommand UIAreaSpriteCommand
    {
        get
        {
            return _command;
        }

        set
        {
            if (UIAreaSpriteCommand == value)
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

    private UIAreaSpriteCommand ControlCommand
    {
        get
        {
            return spriteEditorUserControl.UIAreaSpriteCommand;
        }

        set
        {
            spriteEditorUserControl.UIAreaSpriteCommand = value;
        }
    }

    private bool IsControlCommandUpdating
    {
        get;
        set;
    }

    private UIAreaSpriteCommand TextCommand
    {
        get
        {
            return spriteEditorTextBox.UIAreaSpriteCommand;
        }

        set
        {
            spriteEditorTextBox.UIAreaSpriteCommand = value;
        }
    }

    private bool IsTextCommandUpdating
    {
        get;
        set;
    }

    protected virtual void OnAreaSpriteCommandChanged(EventArgs e)
    {
        AreaSpriteCommandChanged?.Invoke(this, e);
    }
    private void SetCommandInternal(UIAreaSpriteCommand value)
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

        OnAreaSpriteCommandChanged(EventArgs.Empty);
    }

    private void SpriteEditorUserControl_AreaSpriteCommandChanged(object sender, EventArgs e)
    {
        if (!IsControlCommandUpdating)
        {
            IsControlCommandUpdating = true;
            SetCommandInternal(ControlCommand);
            IsControlCommandUpdating = false;
        }
    }

    private void SpriteEditorTextBox_AreaSpriteCommandChanged(object sender, EventArgs e)
    {
        if (!IsTextCommandUpdating)
        {
            IsTextCommandUpdating = true;
            SetCommandInternal(TextCommand);
            IsTextCommandUpdating = false;
        }
    }

    private void SpriteEditorTextBox_IsValidCommandChanged(object sender, EventArgs e)
    {
        IsValidInput = spriteEditorTextBox.IsValidCommand;
    }
}
