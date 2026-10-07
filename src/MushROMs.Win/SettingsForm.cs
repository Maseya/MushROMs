// <copyright file="SettingsForm.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.Win;

using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;

public partial class SettingsForm : Form
{
    public SettingsForm()
    {
        InitializeComponent();
    }

    public bool EnableAutoSave
    {
        get
        {
            return autoSaveSettings.EnableAutoSave;
        }

        set
        {
            autoSaveSettings.EnableAutoSave = value;
        }
    }

    public bool EnablePruning
    {
        get
        {
            return autoSaveSettings.EnablePruning;
        }

        set
        {
            autoSaveSettings.EnablePruning = value;
        }
    }

    public TimeSpan AutoSaveInterval
    {
        get
        {
            return autoSaveSettings.AutoSaveInterval;
        }

        set
        {
            autoSaveSettings.AutoSaveInterval = value;
        }
    }

    public TimeSpan PruningInterval
    {
        get
        {
            return autoSaveSettings.PruningInterval;
        }

        set
        {
            autoSaveSettings.PruningInterval = value;
        }
    }

    public bool HardCutoff
    {
        get
        {
            return autoSaveSettings.HardCutoff;
        }

        set
        {
            autoSaveSettings.HardCutoff = value;
        }
    }

    public bool LoadLastOpenedRomOnStartup
    {
        get
        {
            return chkLoadLastOpenedRom.Checked;
        }

        set
        {
            chkLoadLastOpenedRom.Checked = value;
        }
    }
}
