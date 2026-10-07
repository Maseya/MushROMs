// <copyright file="SpriteEditorTextBox.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Brutario.Win.Controls;
using System;
using System.ComponentModel;

using Brutario.Core;

public class SpriteEditorTextBox : TextBox
{
    private UIAreaSpriteCommand _areaSpriteCommand;
    private bool _isValidCommand;

    public SpriteEditorTextBox() : base()
    {
        ParsedCommand = default;
        PlaceholderText = ParsedCommand.HexString;
    }

    [Category("Editor")]
    public event EventHandler? AreaSpriteCommandChanged;

    [Category("Editor")]
    public event EventHandler? IsValidCommandChanged;

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public UIAreaSpriteCommand UIAreaSpriteCommand
    {
        get
        {
            return _areaSpriteCommand;
        }

        set
        {
            if (value == UIAreaSpriteCommand)
            {
                return;
            }

            SetCommandInternal(value);
        }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsValidCommand
    {
        get
        {
            return _isValidCommand;
        }

        set
        {
            if (IsValidCommand == value)
            {
                return;
            }

            _isValidCommand = value;
            OnIsValidCommandChanged(EventArgs.Empty);
        }
    }

    private UIAreaSpriteCommand ParsedCommand
    {
        get
        {
            _ = UIAreaSpriteCommand.TryGetCommand(Text, out var result);
            return result;
        }

        set
        {
            if (UIAreaSpriteCommand.TryGetCommand(Text, out var result)
                && value == result)
            {
                return;
            }

            Text = value.HexString;
        }
    }

    private bool IsCommandUpdating
    {
        get;
        set;
    }

    protected override void OnTextChanged(EventArgs e)
    {
        IsValidCommand = UIAreaSpriteCommand.TryGetCommand(Text, out var command);
        if (IsValidCommand && !IsCommandUpdating)
        {
            IsCommandUpdating = true;
            UIAreaSpriteCommand = command;
            IsCommandUpdating = false;
        }

        base.OnTextChanged(e);
    }

    protected virtual void OnAreaSpriteCommandChanged(EventArgs e)
    {
        AreaSpriteCommandChanged?.Invoke(this, e);
    }

    protected virtual void OnIsValidCommandChanged(EventArgs e)
    {
        IsValidCommandChanged?.Invoke(this, e);
    }

    private void SetCommandInternal(UIAreaSpriteCommand value)
    {
        _areaSpriteCommand = value;
        if (!IsCommandUpdating)
        {
            ParsedCommand = value;
        }

        OnAreaSpriteCommandChanged(EventArgs.Empty);
    }
}
