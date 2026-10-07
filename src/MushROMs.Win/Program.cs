// <copyright file="Program.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.Win;

using System;
using System.Windows.Forms;

using Core;

internal static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        var editor = new MushROMsEditor();
        using var form = new MainForm(editor);

        // TODO(swr): In a similar note to the above comment, perhaps the presenter
        // should handle the run logic? Not really sure.
        Application.Run(form);
    }
}
