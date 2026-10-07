// <copyright file="SpriteEditorDialog.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Brutario.Win.Dialogs;

using System;
using System.ComponentModel;
using System.Windows.Forms;

using BaseForms;

using Controls;

using Core;

using static System.ComponentModel.DesignerSerializationVisibility;

public class SpriteEditorDialog : DialogProxy
{
    public SpriteEditorDialog()
    {
        SpriteEditorForm = new SpriteEditorForm();
        SpriteEditorForm.AreaSpriteCommandChanged +=
            SpriteEditorForm_AreaSpriteCommandChanged;
    }

    public SpriteEditorDialog(IContainer container)
        : this()
    {
        container.Add(this);
    }

    public event EventHandler? AreaSpriteCommandChanged;

    [Browsable(false)]
    [DesignerSerializationVisibility(Hidden)]
    public UIAreaSpriteCommand AreaSpriteCommand
    {
        get
        {
            return SpriteEditorForm.UIAreaSpriteCommand;
        }

        set
        {
            SpriteEditorForm.UIAreaSpriteCommand = value;
        }
    }

    protected override Form BaseForm
    {
        get
        {
            return SpriteEditorForm;
        }
    }

    private SpriteEditorForm SpriteEditorForm
    {
        get;
    }

    private void SpriteEditorForm_AreaSpriteCommandChanged(object? sender, EventArgs e)
    {
        AreaSpriteCommandChanged?.Invoke(this, EventArgs.Empty);
    }
}
