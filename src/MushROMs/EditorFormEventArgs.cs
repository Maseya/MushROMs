// <copyright file="EditorFormEventArgs.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.MushROMs
{
    using System;
    using System.Windows.Forms;
    using Maseya.Editors;

    public class EditorFormEventArgs : EventArgs
    {
        public EditorFormEventArgs(Form form, IEditor editor)
        {
            Form = form ?? throw new ArgumentNullException(nameof(form));
            Editor = editor ?? throw new ArgumentNullException(nameof(editor));
        }

        public Form Form
        {
            get;
        }

        public IEditor Editor
        {
            get;
        }
    }
}
