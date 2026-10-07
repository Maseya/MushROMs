// <copyright file="SaveFileNameSelector.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.Win.Views;

using System.ComponentModel;
using System.Windows.Forms;

public class SaveFileNameSelector : FileNameSelectorBase
{
    public SaveFileNameSelector() : base()
    {
        SaveFileDialog = new SaveFileDialog();
    }

    public SaveFileNameSelector(IContainer container)
        : base(container)
    {
        SaveFileDialog = new SaveFileDialog();
    }

    public SaveFileDialog SaveFileDialog
    {
        get;
    }

    protected override FileDialog FileDialog
    {
        get
        {
            return SaveFileDialog;
        }
    }
}
