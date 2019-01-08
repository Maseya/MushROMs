// <copyright file="EditorEventArgs.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Editors
{
    using System;

    /// <summary>
    /// Provides data for events that add and remove <see cref=" IEditor"/>
    /// references.
    /// </summary>
    public class EditorEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref=" EditorEventArgs"/>
        /// class.
        /// </summary>
        /// <param name="editor">
        /// The <see cref="IEditor"/> that is being added or removed.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="editor"/> is <see langword="null"/>.
        /// </exception>
        public EditorEventArgs(IEditor editor)
        {
            Editor = editor ??
                throw new ArgumentNullException(nameof(editor));
        }

        /// <summary>
        /// Gets the <see cref="IEditor"/> that is being added or removed.
        /// </summary>
        public IEditor Editor
        {
            get;
        }
    }
}
