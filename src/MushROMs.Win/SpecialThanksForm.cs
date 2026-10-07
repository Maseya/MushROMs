// <copyright file="SpecialThanksForm.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.Win;

using System.Windows.Forms;

using Properties;

public partial class SpecialThanksForm : Form
{
    public SpecialThanksForm()
    {
        InitializeComponent();
    }

    private void SpecialThanksForm_Load(object sender, EventArgs e)
    {
        var dir = Path.GetDirectoryName(Application.ExecutablePath);
        var path = Path.Combine(dir!, "Credits.rtf");
        if (!File.Exists(path))
        {
            File.WriteAllBytes(path, Resources.Credits);
        }

        rtbCredits.LoadFile(path);
    }
}
