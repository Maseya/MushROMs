// <copyright file="SaveOnClosePrompt.cs" organization="Maseya">
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

using Controls;

public partial class SaveOnClosePrompt : EditorDialogBase, ISaveOnClosePrompt
{
    private string? _text;

    private string? _caption;

    public SaveOnClosePrompt()
        : base()
    {
        InitializeComponent();
    }

    public SaveOnClosePrompt(IContainer container)
        : base(container)
    {
        InitializeComponent();
    }

    public event EventHandler? TextChanged;

    public event EventHandler? CaptionChanged;

    public string? Caption
    {
        get
        {
            return _caption;
        }

        set
        {
            if (Caption == value)
            {
                return;
            }

            _caption = value;
            OnCaptionChanged(EventArgs.Empty);
        }
    }

    public string? Text
    {
        get
        {
            return _text;
        }

        set
        {
            if (Text == value)
            {
                return;
            }

            _text = value;
            OnTextChanged(EventArgs.Empty);
        }
    }

    public PromptResult Prompt()
    {
        return RtlAwareMessageBox.Show(
            Owner,
            Text,
            Caption,
            MessageBoxButtons.YesNoCancel) switch
        {
            DialogResult.Yes => PromptResult.Yes,
            DialogResult.No => PromptResult.No,
            DialogResult.Cancel => PromptResult.Cancel,
            _ => throw new InvalidOperationException(),
        };
    }

    protected virtual void OnCaptionChanged(EventArgs e)
    {
        CaptionChanged?.Invoke(this, e);
    }

    protected virtual void OnTextChanged(EventArgs e)
    {
        TextChanged?.Invoke(this, e);
    }
}
