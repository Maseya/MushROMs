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

        Codes = [];
        EnumIndexes = [];
        foreach (var obj in Enum.GetValues(typeof(AreaSpriteCode)))
        {
            var code = (AreaSpriteCode)obj;
            if (code == AreaSpriteCode.ScreenJump)
            {
                continue;
            }

            EnumIndexes.Add(code, Codes.Count);
            Codes.Add(code);
            _ = cbxAreaSpriteCode.Items.Add(code.BaseName());
        }
    }

    public event EventHandler? AreaSpriteCommandChanged;

    public UIAreaSpriteCommand AreaSpriteCommand
    {
        get
        {
            return UseManualInput ? BinaryCommand : UICommand;
        }

        set
        {
            BinaryCommand = UICommand = value;
            if (IsValidInput)
            {
                AreaSpriteCommandChanged?.Invoke(this, EventArgs.Empty);
            }
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

    private int XPos
    {
        get
        {
            return (int)nudX.Value;
        }

        set
        {
            nudX.Value = value;
        }
    }

    private int Page
    {
        get
        {
            return (int)nudPage.Value;
        }

        set
        {
            nudPage.Value = value;
        }
    }

    private int YPos
    {
        get
        {
            var y = (int)AreaSpriteCode >> 8;
            return y < 0x0D ? (int)nudY.Value : y;
        }

        set
        {
            if (value <= nudY.Maximum)
            {
                nudY.Value = value;
            }
        }
    }

    private AreaSpriteCode AreaSpriteCode
    {
        get
        {
            return Codes[
                cbxAreaSpriteCode.SelectedIndex >= 0
                ? cbxAreaSpriteCode.SelectedIndex
                : 0];
        }

        set
        {
            cbxAreaSpriteCode.SelectedIndex = EnumIndexes.TryGetValue(
                value,
                out var index) ? index : -1;
        }
    }

    private int DestPage
    {
        get
        {
            return nudDestPage.Enabled ? (int)nudDestPage.Value : 1;
        }

        set
        {
            if (value <= nudDestPage.Maximum)
            {
                nudDestPage.Value = value;
            }
        }
    }

    private int World
    {
        get
        {
            return nudWorld.Enabled ? (int)nudWorld.Value : 1;
        }

        set
        {
            if (value <= nudWorld.Maximum)
            {
                nudWorld.Value = value;
            }
        }
    }

    private bool HardFlag
    {
        get
        {
            return chkHardFlag.Enabled && chkHardFlag.Checked;
        }

        set
        {
            chkHardFlag.Checked = value;
        }
    }

    private int AreaNumber
    {
        get
        {
            _ = TryGetAreaNumber(tbxAreaNumber.Text, out var result);
            return tbxAreaNumber.Enabled ? result : 0;
        }

        set
        {
            tbxAreaNumber.Text = $"{value & 0xFF:X2}";
        }
    }

    private UIAreaSpriteCommand UICommand
    {
        get
        {
            var result = default(AreaSpriteCommand);
            result.Value1 |= (byte)(XPos << 4);
            switch (AreaSpriteCode)
            {
            case AreaSpriteCode.AreaPointer:
                result.Value1 |= 0x0E;
                result.Value2 |= (byte)(AreaNumber & 0x7F);
                result.Value3 |= (byte)((World - 1) << 5);
                result.Value3 |= (byte)(DestPage & 0x1F);
                break;

            default:
                result.Value1 |= (byte)YPos;
                result.Value2 |= (byte)((int)AreaSpriteCode & 0x3F);
                break;
            }

            result.HardWorldFlag |= HardFlag;

            return new UIAreaSpriteCommand(result, Page);
        }

        set
        {
            SettingUICommand = true;
            var command = value.Command;
            UpdateEnabledControls(command);

            XPos = command.X;
            AreaSpriteCode = command.Code;
            switch (command.Code)
            {
            case AreaSpriteCode.AreaPointer:
                Page = 1 + (command.Value3 & 0x1F);
                World = 1 + command.WorldLimit;
                AreaNumber = command.AreaNumber;
                break;

            default:
                YPos = command.Y;
                HardFlag = command.HardWorldFlag;
                break;
            }

            Page = value.Page;
            SettingUICommand = false;
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

    private List<AreaSpriteCode> Codes
    {
        get;
    }

    private Dictionary<AreaSpriteCode, int> EnumIndexes
    {
        get;
    }

    private static bool TryGetAreaNumber(string text, out byte result)
    {
        if (text.Length != 2)
        {
            result = 0;
            return false;
        }

        return Byte.TryParse(
            text,
            NumberStyles.HexNumber,
            CultureInfo.CurrentUICulture,
            out result);
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

    private void UpdateEnabledControls(AreaSpriteCommand value)
    {
        lblY.Enabled =
        nudY.Enabled =
        chkHardFlag.Enabled = value.Y <= 0x0D;

        lblDestPage.Enabled =
        nudDestPage.Enabled = value.Y > 0x0D;

        lblWorld.Enabled =
        nudWorld.Enabled =
        lblAreaNumber.Enabled =
        tbxAreaNumber.Enabled = value.Code == AreaSpriteCode.AreaPointer;
    }

    private void UpdateValidInput()
    {
        // If we're using the list and check boxes, then the input is always valid by
        // their restraints. Otherwise, if we're entering the value manually, then we
        // must check that text is valid.
        IsValidInput =
            !UseManualInput || TryGetCommand(tbxManualInput.Text, out var _);
    }

    private void AreaNumber_TextChanged(object? sender, EventArgs e)
    {
        IsValidInput = !tbxAreaNumber.Enabled
            || TryGetAreaNumber(tbxAreaNumber.Text, out var _);
        if (IsValidInput)
        {
            AreaSpriteCommandChanged?.Invoke(this, EventArgs.Empty);
        }
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

    private void AreaSpriteCode_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (cbxAreaSpriteCode.SelectedIndex == -1 || SettingUICommand)
        {
            return;
        }

        UpdateEnabledControls(UICommand.Command);
        BinaryCommand = UICommand;
        AreaSpriteCommandChanged?.Invoke(this, EventArgs.Empty);
    }

    private void Item_ValueChanged(object? sender, EventArgs e)
    {
        var control = sender as Control;
        if (control!.Enabled && !SettingUICommand && BinaryCommand != UICommand)
        {
            BinaryCommand = UICommand;
            AreaSpriteCommandChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void UseManualInput_CheckedChanged(object? sender, EventArgs e)
    {
        UpdateValidInput();
    }
}
