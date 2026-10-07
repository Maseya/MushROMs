// <copyright file="SpriteEditorUserControl.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Brutario.Win.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

using Brutario.Core;

using Maseya.Smas.Smb1;
using Maseya.Smas.Smb1.AreaData.SpriteData;

public partial class SpriteEditorUserControl : UserControl
{
    private static readonly ReadOnlyCollection<AreaSpriteCode> Codes =
        AreaSpriteCommand.ValidCodes;

    private static readonly ReadOnlyDictionary<AreaSpriteCode, int> EnumIndexes = new(
        Enumerable.Range(0, Codes.Count).Select(
            i => new KeyValuePair<AreaSpriteCode, int>(Codes[i], i)).ToDictionary());

    private UIAreaSpriteCommand _areaSpriteCommand;

    public SpriteEditorUserControl()
    {
        InitializeComponent();

        cbxAreaSpriteCode.Items.AddRange([.. Codes.Select(x => x.BaseName())]);
        ControlCommand = default;
    }

    [Category("Editor")]
    public event EventHandler? AreaSpriteCommandChanged;

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public UIAreaSpriteCommand UIAreaSpriteCommand
    {
        get
        {
            return _areaSpriteCommand;
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

    private bool IsCommandUpdating
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

    private int Screen
    {
        get
        {
            return (int)nudScreen.Value;
        }

        set
        {
            nudScreen.Value = value;
        }
    }

    private int MinY
    {
        get
        {
            return (int)nudY.Minimum;
        }

        set
        {
            nudY.Minimum = value;
        }
    }

    private int MaxY
    {
        get
        {
            return (int)nudY.Maximum;
        }

        set
        {
            nudY.Maximum = value;
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
            nudY.Value = value;
        }
    }

    private int SpriteCodeIndex
    {
        get
        {
            return cbxAreaSpriteCode.SelectedIndex;
        }
    }

    private AreaSpriteCode AreaSpriteCode
    {
        get
        {
            return Codes[Math.Max(SpriteCodeIndex, 0)];
        }

        set
        {
            cbxAreaSpriteCode.SelectedIndex =
                EnumIndexes.TryGetValue(value, out var index)
                ? index
                : -1;
        }
    }

    private int DestPage
    {
        get
        {
            return (int)nudDestScreen.Value;
        }

        set
        {
            nudDestScreen.Value = value;
        }
    }

    private int World
    {
        get
        {
            return (int)nudWorld.Value;
        }

        set
        {
            nudWorld.Value = value;
        }
    }

    private bool HardFlag
    {
        get
        {
            return chkHardFlag.Checked;
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
            return (int)nudAreaNumber.Value;
        }

        set
        {
            nudAreaNumber.Value = value;
        }
    }

    private UIAreaSpriteCommand ControlCommand
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
                result.HardWorldFlag |= HardFlag;
                break;
            }

            return new UIAreaSpriteCommand(result, Screen);
        }

        set
        {
            Debug.Assert(!IsCommandUpdating, "Command is being set recursively");

            IsCommandUpdating = true;
            var command = value.Command;
            UpdateEnabledControls(command);

            XPos = command.X;
            Screen = value.Page;

            switch (command.Code)
            {
            case AreaSpriteCode.AreaPointer:
                Screen = 1 + (command.Value3 & 0x1F);
                World = 1 + command.WorldLimit;
                AreaNumber = command.AreaNumber;
                break;

            default:
                YPos = command.Y;
                HardFlag = command.HardWorldFlag;
                break;
            }

            AreaSpriteCode = command.Code;
            IsCommandUpdating = false;
        }
    }

    protected virtual void OnAreaSpriteCommandChanged(EventArgs e)
    {
        AreaSpriteCommandChanged?.Invoke(this, e);
    }

    private void SetCommandInternal(UIAreaSpriteCommand value)
    {
        _areaSpriteCommand = value;
        UpdateEnabledControls(value.Command);
        if (!IsCommandUpdating)
        {
            ControlCommand = value;
        }

        OnAreaSpriteCommandChanged(EventArgs.Empty);
    }

    private void UpdateEnabledControls(AreaSpriteCommand value)
    {
        lblY.Enabled =
        nudY.Enabled =
        chkHardFlag.Enabled = value.Code != AreaSpriteCode.AreaPointer;
        gbxAreaPointer.Enabled = value.Code == AreaSpriteCode.AreaPointer;
    }

    private void Item_ValueChanged(object sender, EventArgs e)
    {
        if (!IsCommandUpdating)
        {
            IsCommandUpdating = true;
            SetCommandInternal(ControlCommand);
            IsCommandUpdating = false;
        }
    }
}
