// <copyright file="HeaderEditorForm.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Brutario.Win.Dialogs.BaseForms;

using System;
using System.Windows.Forms;

using Maseya.Smas.Smb1.AreaData.HeaderData;
using Maseya.Smas.Smb1.AreaData.ObjectData;

internal partial class HeaderEditorForm : Form
{
    public HeaderEditorForm()
    {
        InitializeComponent();
    }

    public event EventHandler? AreaHeaderChanged;

    public StartTime StartTime
    {
        get
        {
            return headerEditorUserControl.StartTime;
        }

        set
        {
            headerEditorUserControl.StartTime = value;
        }
    }

    public StartYPosition StartYPosition
    {
        get
        {
            return headerEditorUserControl.StartYPosition;
        }

        set
        {
            headerEditorUserControl.StartYPosition = value;
        }
    }

    public ForegroundScenery ForegroundScenery
    {
        get
        {
            return headerEditorUserControl.ForegroundScenery;
        }

        set
        {
            headerEditorUserControl.ForegroundScenery = value;
        }
    }

    public AreaPlatformType AreaPlatformType
    {
        get
        {
            return headerEditorUserControl.AreaPlatformType;
        }

        set
        {
            headerEditorUserControl.AreaPlatformType = value;
        }
    }

    public BackgroundScenery BackgroundScenery
    {
        get
        {
            return headerEditorUserControl.BackgroundScenery;
        }

        set
        {
            headerEditorUserControl.BackgroundScenery = value;
        }
    }

    public TerrainMode TerrainMode
    {
        get
        {
            return headerEditorUserControl.TerrainMode;
        }

        set
        {
            headerEditorUserControl.TerrainMode = value;
        }
    }

    public AreaHeader AreaHeader
    {
        get
        {
            return headerEditorUserControl.AreaHeader;
        }

        set
        {
            headerEditorUserControl.AreaHeader = value;
        }
    }

    private void HeaderEditorUserControl_AreaHeaderChanged(object sender, EventArgs e)
    {
        AreaHeaderChanged?.Invoke(this, EventArgs.Empty);
    }
}
