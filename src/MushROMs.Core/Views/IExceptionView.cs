// <copyright file="IExceptionView.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.Core.Views;

using System;

public interface IExceptionView
{
    void Show(Exception ex);

    void Show(string? message);

    bool ShowAndPromptRetry(Exception ex);

    bool ShowAndPromptRetry(string? message);
}
