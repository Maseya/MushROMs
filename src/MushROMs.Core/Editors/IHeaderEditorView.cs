// <copyright file="IHeaderEditorView.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.Core.Editors;

using System;

using Maseya.Smas.Smb1.AreaData.HeaderData;

public interface IHeaderEditorView
{
    event EventHandler? AreaHeaderChanged;

    AreaHeader AreaHeader
    {
        get; set;
    }

    bool Prompt();
}
