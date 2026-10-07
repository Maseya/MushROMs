// <copyright file="IObjectEditorView.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.Core.Editors;

using System;

using Maseya.Smas.Smb1.AreaData.ObjectData;

public interface IObjectEditorView
{
    event EventHandler? AreaPlatformTypeChanged;

    event EventHandler? AreaObjectCommandChanged;

    AreaPlatformType AreaPlatformType
    {
        get; set;
    }

    UIAreaObjectCommand UIAreaObjectCommand
    {
        get; set;
    }

    bool PromptConfirm();
}
