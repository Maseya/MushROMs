// <copyright file="ObjectEditorForm.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Brutario.Win.Dialogs.BaseForms;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;

using Core;

using Maseya.Smas.Smb1;
using Maseya.Smas.Smb1.AreaData.HeaderData;
using Maseya.Smas.Smb1.AreaData.ObjectData;

internal partial class ObjectEditorForm : Form
{
    private AreaPlatformType _areaPlatformType;

    public ObjectEditorForm()
    {
        InitializeComponent();

        for (var i = 0; i < Codes.Count; i++)
        {
            _ = cbxAreaObjectCode.Items.Add(Codes[i].BaseName());
        }

        SetCommandInternal(default);
    }

    public event EventHandler? AreaPlatformTypeChanged;

    public event EventHandler? AreaObjectCommandChanged;

    public AreaPlatformType AreaPlatformType
    {
        get
        {
            return _areaPlatformType;
        }

        set
        {
            if (AreaPlatformType == value)
            {
                return;
            }

            _areaPlatformType = value;

            // Change the name of the area specific platform in the object
            // combo box to match the new value.
            var index = EnumIndexes[ObjectType.AreaSpecificPlatform];
            var code = value.ToObjectCode();
            cbxAreaObjectCode.Items[index] = code.BaseName();

            OnAreaPlatformTypeChanged(EventArgs.Empty);
        }
    }

    public UIAreaObjectCommand AreaObjectCommand
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

    private bool UICommandIsUpdating
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
            if (value == XPos)
            {
                return;
            }

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
            if (value == Page)
            {
                return;
            }

            nudPage.Value = value;
        }
    }

    private bool YPosEnabled
    {
        get
        {
            return nudY.Enabled;
        }

        set
        {
            lblY.Enabled =
            nudY.Enabled = value;
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
            var y = (int)AreaObjectCode >> 8;
            return (y is < 0x0C or 0x0F) ? (int)nudY.Value : y;
        }

        set
        {
            if (value == YPos)
            {
                return;
            }

            nudY.Value = value;
        }
    }

    private int ObjectCodeIndex
    {
        get
        {
            return cbxAreaObjectCode.SelectedIndex;
        }
    }

    private ObjectType AreaObjectCode
    {
        get
        {
            return Codes[Math.Max(ObjectCodeIndex, 0)];
        }

        set
        {
            if (ObjectCodeIndex >= 0 && value == Codes[ObjectCodeIndex])
            {
                return;
            }

            cbxAreaObjectCode.SelectedIndex =
                EnumIndexes.TryGetValue(value, out var index)
                ? index
                : -1;
        }
    }

    private int Length
    {
        get
        {
            return nudLength.Enabled ? (int)nudLength.Value : 1;
        }

        set
        {
            if (value == Length || value > MaximumLength)
            {
                return;
            }

            nudLength.Value = value;
        }
    }

    private bool LengthEnabled
    {
        get
        {
            return nudLength.Enabled;
        }

        set
        {
            lblLength.Enabled =
            nudLength.Enabled = value;
        }
    }

    private int MaximumLength
    {
        get
        {
            return (int)nudLength.Maximum;
        }

        set
        {
            nudLength.Maximum = value;
        }
    }

    private TerrainMode TerrainMode
    {
        get
        {
            return (TerrainMode)cbxTerrainMode.SelectedIndex;
        }

        set
        {
            if (value == TerrainMode)
            {
                return;
            }

            cbxTerrainMode.SelectedIndex = (int)value;
        }
    }

    private BackgroundScenery BackgroundScenery
    {
        get
        {
            return (BackgroundScenery)cbxBackgroundScenery.SelectedIndex;
        }

        set
        {
            if (value == BackgroundScenery)
            {
                return;
            }

            cbxBackgroundScenery.SelectedIndex = (int)value;
        }
    }

    private bool TerrainAndBackgroundSceneryEnabled
    {
        get
        {
            return cbxTerrainMode.Enabled;
        }

        set
        {
            lblTerrainMode.Enabled =
            cbxTerrainMode.Enabled =
            lblBackgroundScenery.Enabled =
            cbxBackgroundScenery.Enabled = value;
        }
    }

    private ForegroundScenery ForegroundScenery
    {
        get
        {
            return (ForegroundScenery)cbxForegroundScenery.SelectedIndex;
        }

        set
        {
            if (value == ForegroundScenery)
            {
                return;
            }

            cbxForegroundScenery.SelectedIndex = (int)value;
        }
    }

    private bool ForegroundSceneryEnabled
    {
        get
        {
            return cbxForegroundScenery.Enabled;
        }

        set
        {
            lblForegroundScenery.Enabled =
            cbxForegroundScenery.Enabled = value;
        }
    }

    private UIAreaObjectCommand UICommand
    {
        get
        {
            var result = default(AreaObjectCommand);
            result.Value1 |= (byte)(XPos << 4);
            switch ((int)AreaObjectCode & 0xF00)
            {
            case 0xE00:
                result.Value1 |= 0x0E;
                result.Value2 |= (byte)(((int)AreaObjectCode) & 0x40);
                if (ForegroundSceneryEnabled)
                {
                    result.Value2 |= (byte)ForegroundScenery;
                }
                else if (TerrainAndBackgroundSceneryEnabled)
                {
                    result.Value2 |= (byte)TerrainMode;
                    result.Value2 |= (byte)((int)BackgroundScenery << 4);
                }
                else
                {
                    Debug.Assert(
                        false,
                        "Scenery command but no scenery objects enabled.");
                }

                break;

            case 0xF00:
                result.Value1 |= 0x0F;
                if (YPosEnabled)
                {
                    result.Value2 |= (byte)(YPos << 4);
                }

                result.Value3 |= (byte)((int)AreaObjectCode & 0x7F);
                break;

            default:
                if (YPosEnabled)
                {
                    result.Value1 |= (byte)YPos;
                }

                result.Value1 |= (byte)((int)AreaObjectCode >> 8);
                result.Value2 |= (byte)((int)AreaObjectCode & 0x7F);
                break;
            }

            if (LengthEnabled)
            {
                result.Value2 |= (byte)(Length - 1);
            }

            return new UIAreaObjectCommand(result, Page);
        }

        set
        {
            if (value == UICommand)
            {
                return;
            }

            SetCommandInternal(value);
            OnAreaObjectCommandChanged(EventArgs.Empty);
        }
    }

    private UIAreaObjectCommand BinaryCommand
    {
        get
        {
            _ = TryGetBinaryCommand(tbxManualInput.Text, out var result);
            return result;
        }

        set
        {
            if (TryGetBinaryCommand(tbxManualInput.Text, out var result)
                && value == result)
            {
                return;
            }

            tbxManualInput.Text = value.HexString;
        }
    }

    private static ReadOnlyCollection<ObjectType> Codes
    {
        get
        {
            return Maseya.Smas.Smb1.AreaData.ObjectData.AreaObjectCommand.ValidCodes;
        }
    }

    private static ReadOnlyDictionary<ObjectType, int> EnumIndexes
    {
        get;
    } = new(
        new Dictionary<ObjectType, int>(
        Enumerable.Range(0, Codes.Count)
            .Select(i => new KeyValuePair<ObjectType, int>(Codes[i], i))));

    private static bool TryGetBinaryCommand(string text, out UIAreaObjectCommand command)
    {
        var tokens = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
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

        var result = new AreaObjectCommand(bytes[1], bytes[2], bytes[3]);
        if (!result.IsValid || bytes[0] >= 0x20
            || result.ObjectType == ObjectType.PageSkip)
        {
            command = default;
            return false;
        }

        command = new UIAreaObjectCommand(result, bytes[0]);
        return true;
    }

    private void UpdateEnabledControls(AreaObjectCommand value)
    {
        YPosEnabled = value.HasYCoord;
        LengthEnabled = value.IsExtendableObject;
        MaximumLength = value.ObjectType.GetMaxLength();
        (MinY, MaxY) = value.ObjectType.GetYBounds();
        TerrainAndBackgroundSceneryEnabled = value.IsTerrainAndBackgroundChange;
        ForegroundSceneryEnabled = value.IsForegroundChange;
    }

    private void SetCommandInternal(UIAreaObjectCommand value)
    {
        Debug.Assert(!UICommandIsUpdating, "Object command is being set recursively");

        UICommandIsUpdating = true;
        var command = value.Command;
        UpdateEnabledControls(command);

        XPos = command.X;
        Page = value.Page;
        if (YPosEnabled)
        {
            YPos = command.Y;
        }

        AreaObjectCode = command.ObjectType;
        if (TerrainAndBackgroundSceneryEnabled)
        {
            TerrainMode = command.TerrainMode;
            BackgroundScenery = command.BackgroundScenery;
        }
        else
        {
            TerrainMode = default;
            BackgroundScenery = default;
        }

        ForegroundScenery = ForegroundSceneryEnabled
            ? command.ForegroundScenery
            : default;

        Length = LengthEnabled ? 1 + command.Length : 1;

        if (!UseManualInput)
        {
            BinaryCommand = UICommand;
        }

        UICommandIsUpdating = false;
    }

    private void UpdateValidInputFlag()
    {
        // If we're using the list and check boxes, then the input is always valid by
        // their restraints. Otherwise, if we're entering the value manually, then we
        // must check that text is valid.
        IsValidInput =
            !UseManualInput || TryGetBinaryCommand(tbxManualInput.Text, out var _);
    }

    private void ManualInput_TextChanged(object? sender, EventArgs e)
    {
        UpdateValidInputFlag();
        if (!UICommandIsUpdating && IsValidInput)
        {
            UICommand = BinaryCommand;
        }
    }

    private void AreaObectCode_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (UICommandIsUpdating)
        {
            return;
        }

        switch (AreaObjectCode)
        {
        case ObjectType.ForegroundSceneryChange:
            ForegroundSceneryEnabled = true;
            break;
        case ObjectType.TerrainAndBackgroundSceneryChange:
            TerrainAndBackgroundSceneryEnabled = true;
            break;
        default:
            ForegroundSceneryEnabled = false;
            TerrainAndBackgroundSceneryEnabled = false;
            break;
        }

        UpdateEnabledControls(UICommand.Command);
        BinaryCommand = UICommand;
        OnAreaObjectCommandChanged(EventArgs.Empty);
    }

    private void Item_ValueChanged(object sender, EventArgs e)
    {
        var control = (Control)sender;
        if (!control.Enabled || UICommandIsUpdating)
        {
            return;
        }

        BinaryCommand = UICommand;
        OnAreaObjectCommandChanged(EventArgs.Empty);
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
}
