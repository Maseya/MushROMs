// <copyright file="AutoSaveSettingsUserControl.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.Win;

using System;
using System.Diagnostics;
using System.Windows.Forms;

public partial class AutoSaveSettingsUserControl : UserControl
{
    public AutoSaveSettingsUserControl()
    {
        InitializeComponent();
    }

    public bool EnableAutoSave
    {
        get
        {
            return chkAutoSave.Checked;
        }

        set
        {
            chkAutoSave.Checked = value;
        }
    }

    public bool EnablePruning
    {
        get
        {
            return chkPrune.Checked;
        }

        set
        {
            chkPrune.Checked = value;
        }
    }

    public TimeSpan AutoSaveInterval
    {
        get
        {
            var time = (int)nudTime.Value;
            var conversion = (int)Math.Pow(60, cbxUnits.SelectedIndex);
            return new TimeSpan(0, 0, time * conversion);
        }

        set
        {
            if (value.TotalMinutes < 1)
            {
                cbxUnits.SelectedIndex = 0;
                nudTime.Value = (int)value.TotalSeconds;
            }
            else if (value.TotalHours < 1)
            {
                cbxUnits.SelectedIndex = 1;
                nudTime.Value = (int)value.TotalMinutes;
            }
            else
            {
                cbxUnits.SelectedIndex = 2;
                nudTime.Value = (int)value.TotalHours;
            }
        }
    }

    public TimeSpan PruningInterval
    {
        get
        {
            var time = (int)nudCutoffTime.Value;
            switch (cbxCutoffUnits.SelectedIndex)
            {
            case 0:
                return new TimeSpan(0, time, 0);
            case 1:
                return new TimeSpan(time, 0, 0);
            case 2:
                return new TimeSpan(time, 0, 0, 0);
            case 3:
                return new TimeSpan(7 * time, 0, 0);
            default:
                Debug.Assert(false);
                return TimeSpan.Zero;
            }
        }

        set
        {
            if (value.TotalHours < 1)
            {
                cbxCutoffUnits.SelectedIndex = 0;
                nudCutoffTime.Value = (int)value.TotalMinutes;
            }
            else if (value.TotalDays < 1)
            {
                cbxCutoffUnits.SelectedIndex = 1;
                nudCutoffTime.Value = (int)value.TotalHours;
            }
            else if (value.TotalDays < 7)
            {

                cbxCutoffUnits.SelectedIndex = 2;
                nudCutoffTime.Value = (int)value.TotalDays;
            }
            else
            {
                cbxCutoffUnits.SelectedIndex = 3;
                nudCutoffTime.Value = 7 * (int)value.TotalDays;
            }
        }
    }

    public bool HardCutoff
    {
        get
        {
            return rdbConstant.Checked;
        }

        set
        {
            rdbConstant.Checked = value;
            rdbProgressive.Checked = !value;
        }
    }

    private void AutoSave_CheckedChanged(object sender, EventArgs e)
    {
        nudTime.Enabled =
        cbxUnits.Enabled = chkAutoSave.Checked;
    }

    private void Prune_CheckedChanged(object sender, EventArgs e)
    {
        nudCutoffTime.Enabled =
        cbxCutoffUnits.Enabled = chkPrune.Checked;
    }
}
