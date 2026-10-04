// <copyright file="ObjectEditorUserControl.cs" organization="Maseya">
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
using System.Diagnostics;
using System.Windows.Forms;

using Core;

using Maseya.Smas.Smb1;
using Maseya.Smas.Smb1.AreaData.HeaderData;
using Maseya.Smas.Smb1.AreaData.ObjectData;

public partial class ObjectEditorUserControl : UserControl
{
    private static readonly ReadOnlyCollection<ObjectType> Codes =
        AreaObjectCommand.ValidCodes;

    private static readonly ReadOnlyDictionary<ObjectType, int> EnumIndexes = new(
            Enumerable.Range(0, Codes.Count).Select(
                i => new KeyValuePair<ObjectType, int>(Codes[i], i)).ToDictionary());

    private AreaPlatformType _areaPlatformType;

    public ObjectEditorUserControl()
    {
        InitializeComponent();

        for (var i = 0; i < Codes.Count; i++)
        {
            _ = cbxAreaObjectCode.Items.Add(Codes[i].BaseName());
        }

        SetCommandInternal(default);
    }

    [Category("Editor")]
    public event EventHandler? AreaPlatformTypeChanged;

    [Category("Editor")]
    public event EventHandler? AreaObjectCommandChanged;

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
            OnAreaPlatformTypeChanged(EventArgs.Empty);
        }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public UIAreaObjectCommand UIAreaObjectCommand
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

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool CommandIsUpdating
    {
        get;
        private set;
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
            (var minY, var maxY) = AreaObjectCode.GetYBounds();
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
                    result.Value2 |= (byte)(Math.Clamp(YPos, minY, maxY) << 4);
                }

                result.Value3 |= (byte)((int)AreaObjectCode & 0x7F);
                break;

            default:
                if (YPosEnabled)
                {
                    result.Value1 |= (byte)Math.Clamp(YPos, minY, maxY);
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

    protected virtual void OnAreaPlatformTypeChanged(EventArgs e)
    {
        // Change the name of the area specific platform in the object
        // combo box to match the new value.
        var index = EnumIndexes[ObjectType.AreaSpecificPlatform];
        var code = AreaPlatformType.ToObjectCode();
        cbxAreaObjectCode.Items[index] = code.BaseName();

        AreaPlatformTypeChanged?.Invoke(this, e);
    }

    protected virtual void OnAreaObjectCommandChanged(EventArgs e)
    {
        AreaObjectCommandChanged?.Invoke(this, e);
    }

    private void UpdateEnabledControls(AreaObjectCommand value)
    {
        YPosEnabled = value.HasYCoord;
        LengthEnabled = value.IsExtendableObject;
        MaximumLength = value.ObjectType.GetMaxLength();
        (MinY, MaxY) = value.ObjectType.GetYBounds();
        YPos = Math.Clamp(YPos, MinY, MaxY);
        TerrainAndBackgroundSceneryEnabled = value.IsTerrainAndBackgroundChange;
        ForegroundSceneryEnabled = value.IsForegroundChange;
    }

    private void SetCommandInternal(UIAreaObjectCommand value)
    {
        Debug.Assert(!CommandIsUpdating, "Object command is being set recursively");

        CommandIsUpdating = true;
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

        CommandIsUpdating = false;
    }

    private void AreaObectCode_SelectedIndexChanged(object? sender, EventArgs e)
    {
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

        if (CommandIsUpdating)
        {
            return;
        }

        UpdateEnabledControls(UICommand.Command);
        OnAreaObjectCommandChanged(EventArgs.Empty);
    }

    private void Item_ValueChanged(object sender, EventArgs e)
    {
        var control = (Control)sender;
        if (!control.Enabled || CommandIsUpdating)
        {
            return;
        }

        OnAreaObjectCommandChanged(EventArgs.Empty);
    }
}
