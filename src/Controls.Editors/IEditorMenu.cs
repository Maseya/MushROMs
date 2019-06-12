// <copyright file="IEditorMenu.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Controls.Editors
{
    using System.ComponentModel;
    using System.Windows.Forms;

    public interface IEditorMenu : IComponent
    {
        ToolStripItemCollection MenuStripItems
        {
            get;
        }

        ToolStripItemCollection ToolStripItems
        {
            get;
        }
    }
}
