// <copyright file="HeaderEditorForm.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Brutario.Win.Dialogs.BaseForms;

using System;
using System.ComponentModel;
using System.Windows.Forms;

using Maseya.Smas.Smb1.AreaData.HeaderData;
using Maseya.Smas.Smb1.AreaData.ObjectData;

internal partial class HeaderEditorForm : Form
{
    public HeaderEditorForm()
    {
        InitializeComponent();
    }

    [Category("Area Header")]
    public event EventHandler? AreaHeaderChanged;

    [Category("Area Header")]
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

    [Category("Area Header")]
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

    [Category("Area Header")]
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

    [Category("Area Header")]
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

    [Category("Area Header")]
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

    [Category("Area Header")]
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

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

    protected virtual void OnAreaHeaderChanged(EventArgs e)
    {
        AreaHeaderChanged?.Invoke(this, EventArgs.Empty);
    }

    private void HeaderEditorUserControl_AreaHeaderChanged(object sender, EventArgs e)
    {
        OnAreaHeaderChanged(EventArgs.Empty);
    }
}
