// <copyright file="HeaderEditorUserControl.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Brutario.Win.Controls;
using System;
using System.ComponentModel;
using System.Windows.Forms;

using Maseya.Smas.Smb1.AreaData.HeaderData;
using Maseya.Smas.Smb1.AreaData.ObjectData;

public partial class HeaderEditorUserControl : UserControl
{
    public HeaderEditorUserControl()
    {
        InitializeComponent();

        AreaHeader = default;
    }

    [Category("Area Header")]
    public event EventHandler? AreaHeaderChanged;

    [Category("Area Header")]
    public StartTime StartTime
    {
        get
        {
            return (StartTime)cbxTime.SelectedIndex;
        }

        set
        {
            cbxTime.SelectedIndex = (int)value;
        }
    }

    [Category("Area Header")]
    public StartYPosition StartYPosition
    {
        get
        {
            return (StartYPosition)cbxPosition.SelectedIndex;
        }

        set
        {
            cbxPosition.SelectedIndex = (int)value;
        }
    }

    [Category("Area Header")]
    public ForegroundScenery ForegroundScenery
    {
        get
        {
            return (ForegroundScenery)cbxForeground.SelectedIndex;
        }

        set
        {
            cbxForeground.SelectedIndex = (int)value;
        }
    }

    [Category("Area Header")]
    public AreaPlatformType AreaPlatformType
    {
        get
        {
            return (AreaPlatformType)cbxAreaPlatformType.SelectedIndex;
        }

        set
        {
            cbxAreaPlatformType.SelectedIndex = (int)value;
        }
    }

    [Category("Area Header")]
    public BackgroundScenery BackgroundScenery
    {
        get
        {
            return (BackgroundScenery)cbxBackgroundScenery.SelectedIndex;
        }

        set
        {
            cbxBackgroundScenery.SelectedIndex = (int)value;
        }
    }

    [Category("Area Header")]
    public TerrainMode TerrainMode
    {
        get
        {
            return (TerrainMode)cbxTerrainMode.SelectedIndex;
        }

        set
        {
            cbxTerrainMode.SelectedIndex = (int)value;
        }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public AreaHeader AreaHeader
    {
        get
        {
            return new AreaHeader(
                StartTime,
                StartYPosition,
                ForegroundScenery,
                AreaPlatformType,
                BackgroundScenery,
                TerrainMode);
        }

        set
        {
            StartTime = value.StartTime;
            StartYPosition = value.StartYPosition;
            ForegroundScenery = value.ForegroundScenery;
            AreaPlatformType = value.AreaPlatformType;
            BackgroundScenery = value.BackgroundScenery;
            TerrainMode = value.TerrainMode;
        }
    }

    protected virtual void OnAreaHeaderChanged(EventArgs e)
    {
        AreaHeaderChanged?.Invoke(this, EventArgs.Empty);
    }

    private void Value_SelectedIndexChanged(object? sender, EventArgs e)
    {
        OnAreaHeaderChanged(EventArgs.Empty);
    }
}
