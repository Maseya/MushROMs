// <copyright file="ObjectEditorTextBox.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Brutario.Win.Controls;
using System;
using System.ComponentModel;

using Brutario.Core;

internal class ObjectEditorTextBox : TextBox
{
    private UIAreaObjectCommand _areaObjectCommand;
    private bool _isValidCommand;

    public ObjectEditorTextBox() : base()
    {
        ParsedCommand = default;
        PlaceholderText = ParsedCommand.HexString;
    }

    [Category("Editor")]
    public event EventHandler? AreaObjectCommandChanged;

    [Category("Editor")]
    public event EventHandler? IsValidCommandChanged;

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public UIAreaObjectCommand UIAreaObjectCommand
    {
        get
        {
            return _areaObjectCommand;
        }

        set
        {
            if (value == UIAreaObjectCommand)
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

    private UIAreaObjectCommand ParsedCommand
    {
        get
        {
            _ = UIAreaObjectCommand.TryGetCommand(Text, out var result);
            return result;
        }

        set
        {
            if (UIAreaObjectCommand.TryGetCommand(Text, out var result)
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
        IsValidCommand = UIAreaObjectCommand.TryGetCommand(Text, out var command);
        if (IsValidCommand && !IsCommandUpdating)
        {
            IsCommandUpdating = true;
            UIAreaObjectCommand = command;
            IsCommandUpdating = false;
        }

        base.OnTextChanged(e);
    }

    protected virtual void OnAreaObjectCommandChanged(EventArgs e)
    {
        AreaObjectCommandChanged?.Invoke(this, e);
    }

    protected virtual void OnIsValidCommandChanged(EventArgs e)
    {
        IsValidCommandChanged?.Invoke(this, e);
    }

    private void SetCommandInternal(UIAreaObjectCommand value)
    {
        _areaObjectCommand = value;
        if (!IsCommandUpdating)
        {
            ParsedCommand = value;
        }

        OnAreaObjectCommandChanged(EventArgs.Empty);
    }
}
