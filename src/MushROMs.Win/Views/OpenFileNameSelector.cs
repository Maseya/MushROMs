// <copyright file="OpenFileNameSelector.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.Win.Views;

using System.ComponentModel;
using System.Windows.Forms;

public class OpenFileNameSelector : FileNameSelectorBase
{
    public OpenFileNameSelector()
        : base()
    {
        OpenFileDialog = new OpenFileDialog();
    }

    public OpenFileNameSelector(IContainer container)
        : base(container)
    {
        OpenFileDialog = new OpenFileDialog();
    }

    public OpenFileDialog OpenFileDialog
    {
        get;
    }

    protected override FileDialog FileDialog
    {
        get
        {
            return OpenFileDialog;
        }
    }
}
