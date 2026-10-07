// <copyright file="IObjectListView.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.Core.Views;

using System.Collections.Generic;

using Maseya.Smas.Smb1.AreaData.ObjectData;

public interface IObjectListView
{
    event EventHandler? AreaPlatformTypeChanged;

    event EventHandler? SelectedIndexChanged;

    event EventHandler? EditItem;

    event EventHandler? AddItem_Click;

    event EventHandler? DeleteItem_Click;

    event EventHandler? ClearItems_Click;

    event EventHandler? MoveItemDown_Click;

    event EventHandler? MoveItemUp_Click;

    AreaPlatformType AreaPlatformType
    {
        get; set;
    }

    int SelectedIndex
    {
        get; set;
    }

    IList<UIAreaObjectCommand> Items
    {
        get;
    }
}
