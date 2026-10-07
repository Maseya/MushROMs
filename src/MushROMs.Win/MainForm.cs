// <copyright file="MainForm.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.Win;

using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

using MushROMs.Core.Presenters;
using MushROMs.Core.Views;
using MushROMs.Win.Properties;

using Core;

using Maseya.Smas.Smb1;

public partial class MainForm : Form, IMainView
{
    // TODO(swr): BrutatioEditor needs to be an interface. It's currently not an
    // interface because it is rapidly changing and I don't want to keep
    // changing the interface.
    public MainForm(MushROMsEditor MushROMsEditor)
    {
        InitializeComponent();
        InitializeComponent2();

        Presenter = new MainPresenter(
            MushROMsEditor,
            this,
            objectListView,
            exceptionHelper,
            openFileNameSelector,
            saveFileNameSelector,
            saveOnClosePrompt,
            headerEditor,
            objectEditor,
            spriteEditor);
    }

    public string Title
    {
        get
        {
            return Text;
        }

        set
        {
            Text = value;
        }
    }

    public bool EditorEnabled
    {
        get
        {
            return tsmSaveAs.Enabled;
        }

        set
        {
            tsmSaveAs.Enabled =
            tsmClose.Enabled =
            tsmLoadArea.Enabled =
            tsbJumpToArea.Enabled =
            ttbJumpToArea.Enabled = value;
            //tsbLoadAreaByLevel.Enabled = value;
            SetRunEmulatorEnabled();
        }
    }

    public bool MapEditorEnabled
    {
        get
        {
            return areaControl.Enabled;
        }

        set
        {
            areaControl.Enabled =
            cmsMain.Enabled =
            hsbStartX.Enabled =
            cmiAddItem.Enabled =
            tsmAddItem.Enabled =
            tsbAddItem.Enabled =
            tsmExportTileData.Enabled =
            tsmEditHeader.Enabled = value;

            UpdateScrollBar();
        }
    }

    public bool SaveEnabled
    {
        get
        {
            return tsmSave.Enabled;
        }

        set
        {
            tsmSave.Enabled =
            tsbSave.Enabled = value;
        }
    }

    public bool UndoEnabled
    {
        get
        {
            return tsmUndo.Enabled;
        }

        set
        {
            tsmUndo.Enabled =
            tsbUndo.Enabled = value;
        }
    }

    public bool RedoEnabled
    {
        get
        {
            return tsmRedo.Enabled;
        }

        set
        {
            tsmRedo.Enabled =
            tsbRedo.Enabled = value;
        }
    }

    public bool EditItemEnabled
    {
        get
        {
            return tsmCopy.Enabled;
        }

        set
        {
            cmiCut.Enabled =
            tsmCut.Enabled =
            tsbCut.Enabled =
            cmiCopy.Enabled =
            tsmCopy.Enabled =
            tsbCopy.Enabled =
            cmiRemoveItem.Enabled =
            tsmRemoveItem.Enabled =
            tsbRemoveItem.Enabled = value;
        }
    }

    public bool PasteEnabled
    {
        get
        {
            return tsmPaste.Enabled;
        }
        set
        {
            tsmPaste.Enabled =
            tsbPaste.Enabled =
            cmiPaste.Enabled = value;
        }
    }

    public bool DeleteAllEnabled
    {
        get
        {
            return tsmDeleteAll.Enabled;
        }

        set
        {
            cmiDeleteAll.Enabled =
            tsmDeleteAll.Enabled =
            tsbDeleteAll.Enabled = value;
        }
    }

    public bool SpriteMode
    {
        get
        {
            return tsmSpriteMode.Checked;
        }

        set
        {
            tsmSpriteMode.Checked =
            tsbSpriteMode.Checked = value;
        }
    }

    public Player Player
    {
        get
        {
            return tsmMario.Checked ? Player.Mario : Player.Luigi;
        }

        set
        {
            tsmMario.Checked = value == Player.Mario;
            tsmLuigi.Checked = value == Player.Luigi;
        }
    }

    public PlayerState PlayerState
    {
        get
        {
            return tsmSmall.Checked
                ? PlayerState.Small
                : tsmBig.Checked
                ? PlayerState.Big
                : PlayerState.Fire;
        }

        set
        {
            tsmSmall.Checked = value == PlayerState.Small;
            tsmBig.Checked = value == PlayerState.Big;
            tsmFire.Checked = value == PlayerState.Fire;
        }
    }

    public Size DrawAreaSize
    {
        get
        {
            return areaControl.ClientSize;
        }
    }

    public int AreaNumber
    {
        get
        {
            return Int32.TryParse(
                    ttbJumpToArea.Text,
                    NumberStyles.HexNumber,
                    CultureInfo.CurrentUICulture,
                    out var areaNumber)
                ? areaNumber
                : -1;
        }

        set
        {
            ttbJumpToArea.Text = $"{value:X2}";
        }
    }

    public bool JumpToAreaEnabled
    {
        get
        {
            return tsbJumpToArea.Enabled;
        }

        set
        {
            tsbJumpToArea.Enabled = value;
        }
    }

    public int StartX
    {
        get
        {
            return hsbStartX.Value;
        }

        set
        {
            hsbStartX.Value = value;
        }
    }

    public bool AnimationsEnabled
    {
        get
        {
            return tsmToggleAnimations.Checked;
        }

        set
        {
            tsmToggleAnimations.Checked = value;
        }
    }

    public MainPresenter Presenter
    {
        get;
    }

    private string? LastRom
    {
        get
        {
            return Settings.Default.RecentRoms.Count > 0
                ? Settings.Default.RecentRoms[0]
                : null;
        }
    }

    private DateTime StartTime
    {
        get; set;
    }

    private TimeSpan ElapsedTime
    {
        get
        {
            return DateTime.Now - StartTime;
        }
    }

    private int LastFrame
    {
        get;
        set;
    }

    private int CurrentFrame
    {
        get
        {
            // TODO(swr): Remove frame constants here.
            return (int)(ElapsedTime.TotalMilliseconds * (60 / 1000.0)) + LastFrame;
        }
    }

    private bool RunEmulatorEnabled
    {
        get
        {
            return tsmRunEmulator.Enabled;
        }

        set
        {
            tsmRunEmulator.Enabled = value;
        }
    }

    public void Redraw()
    {
        areaControl.Invalidate();
    }

    public void AddRecentRom(string path)
    {
        while (true)
        {
            var index = Settings.Default.RecentRoms.IndexOf(path);
            if (index == -1)
            {
                break;
            }

            Settings.Default.RecentRoms.RemoveAt(index);
        }

        Settings.Default.RecentRoms.Insert(0, path);
        while (Settings.Default.RecentRoms.Count > 10)
        {
            Settings.Default.RecentRoms.RemoveAt(Settings.Default.RecentRoms.Count - 1);
        }

        Settings.Default.Save();
        InitializeRecentRoms(Settings.Default.RecentRoms);
    }

    public void ClearRecentRoms()
    {
        Settings.Default.RecentRoms.Clear();
        Settings.Default.Save();
        InitializeRecentRoms(Settings.Default.RecentRoms);
    }

    /// <summary>
    /// Initialize remaining components that could not be initialized in
    /// <see cref="InitializeComponent"/> through the designer.
    /// </summary>
    private void InitializeComponent2()
    {
        tsmMario.Tag = Player.Mario;
        tsmLuigi.Tag = Player.Luigi;

        tsmSmall.Tag = PlayerState.Small;
        tsmBig.Tag = PlayerState.Big;
        tsmFire.Tag = PlayerState.Fire;

        openFileNameSelector.OpenFileDialog.Title = "Open ROM";
        saveFileNameSelector.SaveFileDialog.Title = "Save ROM As...";

        openFileNameSelector.OpenFileDialog.DefaultExt =
        saveFileNameSelector.SaveFileDialog.DefaultExt = "sfc";

        openFileNameSelector.OpenFileDialog.Filter =
        saveFileNameSelector.SaveFileDialog.Filter =
            "ROM Files (*.sfc;*.smc;*.fig)|*.sfc;*.smc;*.fig|All Files (*.*)|*.*";

        autoSaveTimer.Interval = (int)new TimeSpan(0, 0, 3).TotalMilliseconds;
    }

    private void InitializeRecentRoms(System.Collections.IList paths)
    {
        InitializeRecentRoms(paths, tsmOpenRecent.DropDownItems);
        InitializeRecentRoms(paths, cmsRecentRoms.Items);
    }

    private void InitializeRecentRoms(
        System.Collections.IList paths,
        ToolStripItemCollection dest)
    {
        var items = new ToolStripItem[paths.Count + 2];
        for (var i = 0; i < paths.Count; i++)
        {
            items[i] = new ToolStripMenuItem
            {
                Text = $"{i + 1}: {paths[i]}",
                Tag = paths[i],
            };
            items[i].Click += RecentRom_Click;
        }

        for (var i = 0; i < dest.Count - 2; i++)
        {
            dest[i].Click -= RecentRom_Click;
        }

        items[^2] = dest[^2];
        items[^1] = dest[^1];
        dest.Clear();
        dest.AddRange(items);
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
        if (Settings.Default.RecentRoms is null)
        {
            Settings.Default.RecentRoms = [];
        }

        InitializeRecentRoms(Settings.Default.RecentRoms);

        if (Presenter.AutoSaveEnabled = Settings.Default.AutoSaveEnabled)
        {
            Presenter.AutoSaveInterval = Settings.Default.AutoSaveInterval;
        }

        if (Presenter.PruneAutoSavesEnabled = Settings.Default.AutoSavePruningEnabled)
        {
            Presenter.AutoSaveCutoffAge = Settings.Default.AutoSavePruningCutoff;
        }

        Presenter.AutoSaveHardCutoff = Settings.Default.AutoSaveHardCutoff;

        AnimationsEnabled = Settings.Default.AnimationsEnabled;

        // TODO(swr): Maybe start timer when rom is opened?
        StartTime = DateTime.Now;
        animationTimer.Start();

        autoSaveTimer.Start();

        var args = Environment.GetCommandLineArgs();
        if (args.Length == 2)
        {
            Presenter.Open(args[1]);
        }
        else if (Settings.Default.LoadLastOpenedRom && Settings.Default.RecentRoms.Count > 0)
        {
            Presenter.Open(Settings.Default.RecentRoms[0]!);
        }
    }

    private void Open_Click(object? sender, EventArgs e)
    {
        // TODO(swr): Should these presenter commands (or some work inside of them) be
        // asynchronous? Open is a good first consideration because it needs to block
        // for the open file dialog. In fact, showDialog cannot even be done
        // asynchronously. So we cannot await here. Awaiting needs to happen inside the
        // presenter logic. Put another way, the presenter logic must take place on the
        // UI thread.
        //
        // Another caveat is whether the presenter should handle the open file dialog
        // logic. Open_Click could have an open file dialog and handle all that logic
        // here. However, other dialogs I've made do not play nicely with this idea
        // since they change some visuals for preview stuff. But those changes are sent
        // through events, so maybe it's ok? MVP is painful.
        //
        // Let's look for another dialog command and make considerations there.
        if (LastRom is not null)
        {
            openFileNameSelector.OpenFileDialog.FileName = Path.GetFileName(LastRom);
            openFileNameSelector.OpenFileDialog.InitialDirectory = Path.GetDirectoryName(LastRom);
        }

        Presenter.Open();
    }

    private void RecentRom_Click(object? sender, EventArgs e)
    {
        Presenter.Open(((sender as ToolStripMenuItem)!.Tag as string)!);
    }

    private void Save_Click(object? sender, EventArgs e)
    {
        Presenter.Save();
    }

    private void SaveAs_Click(object? sender, EventArgs e)
    {
        if (LastRom is not null)
        {
            saveFileNameSelector.SaveFileDialog.FileName = Path.GetFileName(LastRom);
            saveFileNameSelector.SaveFileDialog.InitialDirectory = Path.GetDirectoryName(LastRom);
        }

        Presenter.SaveAs();
    }

    private void Close_Click(object? sender, EventArgs e)
    {
        _ = Presenter.Close();
    }

    private void Exit_Click(object? sender, EventArgs e)
    {
        Close();
    }

    private void Undo_Click(object? sender, EventArgs e)
    {
        Presenter.Undo();
    }

    private void Redo_Click(object? sender, EventArgs e)
    {
        Presenter.Redo();
    }

    private void Cut_Click(object? sender, EventArgs e)
    {
        Presenter.CutCurrentItem();
    }

    private void Copy_Click(object? sender, EventArgs e)
    {
        Presenter.CopyCurrentItem();
    }

    private void Paste_Click(object? sender, EventArgs e)
    {
        Presenter.Paste();
    }

    private void AddItem_Click(object? sender, EventArgs e)
    {
        Presenter.AddItem();
    }

    private void RemoveItem_Click(object? sender, EventArgs e)
    {
        Presenter.RemoveCurrentItem();
    }

    private void DeleteAll_Click(object? sender, EventArgs e)
    {
        Presenter.DeleteAllItems();
    }

    private void ExportTileData_Click(object? sender, EventArgs e)
    {
        try
        {
            Presenter.ExportTileData();
        }
        catch (Exception ex)
        {
            _ = MessageBox.Show(ex.Message);
        }
    }

    private void EditHeader_Click(object? sender, EventArgs e)
    {
        Presenter.EditHeader();
    }

    private void SpriteMode_Click(object? sender, EventArgs e)
    {
        if (sender is ToolStripMenuItem tsm)
        {
            // For some reason, menu items get checked before the click event...
            Presenter.ToggleSpritMode(tsm.Checked);
        }
        else if (sender is ToolStripButton tsb)
        {
            // but tool strip buttons get checked after. >_<
            Presenter.ToggleSpritMode(!tsb.Checked);
        }
    }

    private void Player_Click(object? sender, EventArgs e)
    {
        Presenter.SetPlayer((Player)(sender as ToolStripMenuItem)!.Tag!);
    }

    private void PlayerState_Click(object? sender, EventArgs e)
    {
        Presenter.SetPlayerState(
            (PlayerState)(sender as ToolStripMenuItem)!.Tag!);
    }

    private void SpecialThanks_Click(object? sender, EventArgs e)
    {
        // TODO(swr): Here's another example of whether this should be in the control
        // logic. My guess is no, as that implies attribution should be part of the API.
        using var dialog = new SpecialThanksForm();
        _ = dialog.ShowDialog(this);
    }

    private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (e.CloseReason != CloseReason.TaskManagerClosing && !Presenter.Close())
        {
            e.Cancel = true;
        }
    }

    private void StartX_ValueChanged(object? sender, EventArgs e)
    {
        Presenter.SetStartX(StartX);
    }

    private void AreaControl_SizeChanged(object? sender, EventArgs e)
    {
        UpdateScrollBar();
    }

    private void AreaControl_Paint(object? sender, PaintEventArgs e)
    {
        if (areaControl.Enabled)
        {
            var drawData = Presenter.GetDrawData(
                startX: StartX,
                size: areaControl.Size,
                separatorColor: Color.Blue,
                passiveColor: Color.LightGreen,
                selectColor: Color.White);
            AreaPixelRenderer.DrawArea(e.Graphics, drawData);
        }
    }

    private void AreaControl_MouseClick(object? sender, MouseEventArgs e)
    {
        switch (e.Button)
        {
        case MouseButtons.Right:
            Presenter.SetSelectedItem((e.X + (StartX * 8)) >> 4, e.Y >> 4);
            if (cmsMain.Enabled)
            {
                cmsMain.Show(areaControl, e.Location);
            }

            break;
        }
    }

    private void AreaControl_MouseDown(object? sender, MouseEventArgs e)
    {
        switch (e.Button)
        {
        case MouseButtons.Left:
            Presenter.SetSelectedItem((e.X + (StartX * 8)) >> 4, e.Y >> 4);
            Presenter.InitializeMoveItem();
            break;
        }
    }

    private void AreaControl_MouseMove(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            Presenter.MoveSelectedItem((e.X + (StartX * 8)) >> 4, e.Y >> 4);
        }
    }

    private void AreaControl_MouseUp(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            Presenter.FinishEditItem(commit: true);
        }
    }

    private void AreaControl_MouseDoubleClick(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            Presenter.EditSelectedItem();
        }
    }

    private void JumpToArea_TextChanged(object? sender, EventArgs e)
    {
        JumpToAreaEnabled = Presenter.IsValidAreaNumber(AreaNumber);
    }

    private void JumpToArea_Click(object? sender, EventArgs e)
    {
        if (AreaNumber != -1)
        {
            Presenter.SetAreaNumber(AreaNumber);
        }
    }

    private void JumpToArea_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter && tsbJumpToArea.Enabled)
        {
            tsbJumpToArea.PerformClick();
            e.SuppressKeyPress = true;
        }
    }

    private void UpdateScrollBar()
    {
        // TODO(swr): There are quite a few assumption here. Let's remove some
        // of these constants.
        var viewWidth = ((areaControl.ClientSize.Width - 1) / 8) + 1;
        hsbStartX.Maximum = 0x400 - viewWidth + 0x1F;
        hsbStartX.SmallChange = 1;
        hsbStartX.LargeChange = 0x20;
        hsbStartX.Value = Math.Min(hsbStartX.Value, 0x400 - viewWidth);
    }

    private void Timer_Elapsed(object? sender, EventArgs e)
    {
        // TODO(swr): Should animation be UI-controlled?
        if (AnimationsEnabled)
        {
            Presenter.UpdateFrame(CurrentFrame);
        }
    }

    private void LoadArea_Click(object sender, EventArgs e)
    {
        _ = MessageBox.Show("Not yet implemented");
    }

    private void ObjectListView_AddItem_Click(object sender, EventArgs e)
    {
        Presenter.AddItem();
    }

    private void ViewObjectList_CheckedChanged(object sender, EventArgs e)
    {
        objectListView.Visible = tsmViewObjectList.Checked;
    }

    private void ObjectListView_VisibleChanged(object sender, EventArgs e)
    {
        tsmViewObjectList.Checked = objectListView.Visible;
    }

    private void AutoSaveTimer_Tick(object sender, EventArgs e)
    {
        Presenter.AutoSave();
    }

    private void Settings_Click(object sender, EventArgs e)
    {
        using var dialog = new SettingsForm();

        if (dialog.EnableAutoSave = Presenter.AutoSaveEnabled)
        {
            dialog.AutoSaveInterval = Presenter.AutoSaveInterval;
        }

        if (dialog.EnablePruning = Presenter.PruneAutoSavesEnabled)
        {
            dialog.PruningInterval = Presenter.AutoSaveCutoffAge;
        }

        dialog.LoadLastOpenedRomOnStartup = Settings.Default.LoadLastOpenedRom;

        dialog.HardCutoff = Presenter.AutoSaveHardCutoff;
        if (dialog.ShowDialog(this) == DialogResult.OK)
        {
            if (Settings.Default.AutoSaveEnabled = Presenter.AutoSaveEnabled = dialog.EnableAutoSave)
            {
                Settings.Default.AutoSaveInterval = Presenter.AutoSaveInterval = dialog.AutoSaveInterval;
            }

            if (Settings.Default.AutoSavePruningEnabled = Presenter.PruneAutoSavesEnabled = dialog.EnablePruning)
            {
                Settings.Default.AutoSavePruningCutoff = Presenter.AutoSaveCutoffAge = dialog.PruningInterval;
            }

            Settings.Default.AutoSaveHardCutoff = Presenter.AutoSaveHardCutoff = dialog.HardCutoff;

            Settings.Default.LoadLastOpenedRom = dialog.LoadLastOpenedRomOnStartup;

            Settings.Default.Save();
        }
    }

    private void SetupEmulator_Click(object sender, EventArgs e)
    {
        if (selectEmulatorDialog.ShowDialog() == DialogResult.OK)
        {
            Settings.Default.EmulatorPath = selectEmulatorDialog.FileName;
            Settings.Default.Save();
            SetRunEmulatorEnabled();
        }
    }

    private void SetRunEmulatorEnabled()
    {
        tsmRunEmulator.Enabled = File.Exists(Settings.Default.EmulatorPath)
            && EditorEnabled;
        ;
    }

    private void RunEmulator_Click(object sender, EventArgs e)
    {
        Presenter.RunInEmulator(Settings.Default.EmulatorPath);
    }

    private void ClearRecentRoms_Click(object sender, EventArgs e)
    {
        ClearRecentRoms();
    }

    private void OpenRecent_Click(object sender, EventArgs e)
    {
        var location = tsbOpenRecent.Bounds.Location;
        location.X += tsbOpenRecent.Bounds.Width / 4;
        location.Y += tsbOpenRecent.Bounds.Height;
        cmsRecentRoms.Show(toolStrip, location);
    }

    private void ToggleAnimations_CheckedChanged(object sender, EventArgs e)
    {
        Settings.Default.AnimationsEnabled = AnimationsEnabled;
        Settings.Default.Save();
        if (AnimationsEnabled)
        {
            StartTime = DateTime.Now;
        }
        else
        {
            LastFrame = CurrentFrame;
        }
    }
}
