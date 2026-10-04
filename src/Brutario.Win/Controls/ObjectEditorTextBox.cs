// <copyright file="ObjectEditorTextBox.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Brutario.Win.Controls;
using System;
using System.Globalization;

using Brutario.Core;

using Maseya.Smas.Smb1.AreaData.ObjectData;

internal class ObjectEditorTextBox : TextBox
{
    public UIAreaObjectCommand BinaryCommand
    {
        get
        {
            _ = TryGetBinaryCommand(Text, out var result);
            return result;
        }

        set
        {
            if (TryGetBinaryCommand(Text, out var result)
                && value == result)
            {
                return;
            }

            Text = value.HexString;
        }
    }

    protected override void OnTextChanged(EventArgs e)
    {
        /*
        UpdateValidInputFlag();
        if (!UICommandIsUpdating && IsValidInput)
        {
            //UICommand = BinaryCommand;
        }
        */

        base.OnTextChanged(e);
    }

    private static bool TryGetBinaryCommand(string text, out UIAreaObjectCommand command)
    {
        var tokens = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (tokens.Length is not 4 and not 3)
        {
            command = default;
            return false;
        }

        var bytes = new byte[4];
        for (var i = 0; i < tokens.Length; i++)
        {
            if (tokens[i].Length != 2)
            {
                command = default;
                return false;
            }

            if (!Byte.TryParse(
                tokens[i],
                NumberStyles.HexNumber,
                CultureInfo.CurrentUICulture,
                out bytes[i]))
            {
                command = default;
                return false;
            }
        }

        var result = new AreaObjectCommand(bytes[1], bytes[2], bytes[3]);
        if (!result.IsValid || bytes[0] >= 0x20
            || result.ObjectType == ObjectType.PageSkip)
        {
            command = default;
            return false;
        }

        command = new UIAreaObjectCommand(result, bytes[0]);
        return true;
    }
}
