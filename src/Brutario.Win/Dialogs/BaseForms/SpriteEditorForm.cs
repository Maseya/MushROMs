// <copyright file="SpriteEditorForm.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Brutario.Win.Dialogs.BaseForms;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

using Core;

using Maseya.Smas.Smb1;
using Maseya.Smas.Smb1.AreaData.SpriteData;

public partial class SpriteEditorForm : Form
{
    public SpriteEditorForm()
    {
        InitializeComponent();
    }

    public event EventHandler? AreaSpriteCommandChanged;

    public UIAreaSpriteCommand AreaSpriteCommand
    {
        get
        {
            return UICommand;
        }

        set
        {
            UICommand = value;
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

    /// <summary>
    /// Returns true when the UICommand is being updated by its set accessor.
    /// </summary>
    private bool SettingUICommand
    {
        get;
        set;
    }

    private UIAreaSpriteCommand UICommand
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

    private UIAreaSpriteCommand BinaryCommand
    {
        get
        {
            _ = TryGetCommand(tbxManualInput.Text, out var result);
            return result;
        }

        set
        {
            tbxManualInput.Text = value.HexString;
        }
    }

    protected virtual void OnAreaSpriteCommandChanged(EventArgs e)
    {
        AreaSpriteCommandChanged?.Invoke(this, e);
    }

    private static bool TryGetCommand(string text, out UIAreaSpriteCommand command)
    {
        var tokens = text.Split(' ');
        if (tokens.Length is not 4 and not 3)
        {
            command = default;
            return false;
        }

        var bytes = new byte[4];
        for (var i = 0; i < tokens.Length; i++)
        {
            if (tokens[i].Length != 2)
            {
                command = default;
                return false;
            }

            if (!Byte.TryParse(
                    tokens[i],
                    NumberStyles.HexNumber,
                    CultureInfo.CurrentUICulture,
                    out bytes[i]))
            {
                command = default;
                return false;
            }
        }

        var result = new AreaSpriteCommand(bytes[1], bytes[2], bytes[3]);
        if (!result.IsValid || bytes[0] >= 0x20
            || result.Code == AreaSpriteCode.ScreenJump)
        {
            command = default;
            return false;
        }

        command = new UIAreaSpriteCommand(result, bytes[0]);
        return true;
    }

    private void UpdateValidInput()
    {
        // If we're using the list and check boxes, then the input is always valid by
        // their restraints. Otherwise, if we're entering the value manually, then we
        // must check that text is valid.
        IsValidInput =
            !UseManualInput || TryGetCommand(tbxManualInput.Text, out var _);
    }

    private void ManualInput_TextChanged(object? sender, EventArgs e)
    {
        UpdateValidInput();
        if (!SettingUICommand && btnOK.Enabled && UICommand != BinaryCommand)
        {
            UICommand = BinaryCommand;
            AreaSpriteCommandChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void UseManualInput_CheckedChanged(object? sender, EventArgs e)
    {
        UpdateValidInput();
    }

    private void SpriteEditorUserControl_AreaSpriteCommandChanged(object sender, EventArgs e)
    {
        OnAreaSpriteCommandChanged(EventArgs.Empty);
    }
}
