// <copyright file="SpriteEditor.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.Win.Views;

using System;
using System.ComponentModel;
using System.Windows.Forms;

using MushROMs.Core.Editors;

using Core;

using static System.ComponentModel.DesignerSerializationVisibility;

public sealed partial class SpriteEditor : EditorDialogBase, ISpriteEditorView
{
    public SpriteEditor()
        : base()
    {
        InitializeComponent();
    }

    public SpriteEditor(IContainer container)
        : base(container)
    {
        InitializeComponent();
    }

    public event EventHandler? AreaSpriteCommandChanged;

    [Browsable(false)]
    [DesignerSerializationVisibility(Hidden)]
    public UIAreaSpriteCommand AreaSpriteCommand
    {
        get
        {
            return spriteEditorDialog.AreaSpriteCommand;
        }

        set
        {
            spriteEditorDialog.AreaSpriteCommand = value;
        }
    }

    public bool PromptConfirm()
    {
        return spriteEditorDialog.ShowDialog(Owner) == DialogResult.OK;
    }

    private void SpriteEditorDialog_AreaSpriteCommandChanged(
        object? sender,
        EventArgs e)
    {
        AreaSpriteCommandChanged?.Invoke(this, EventArgs.Empty);
    }
}
