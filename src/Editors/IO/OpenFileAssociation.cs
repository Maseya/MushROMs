// <copyright file="OpenFileAssociation.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Editors.IO
{
    public class OpenFileAssociation
    {
        public OpenFileAssociation(
            string extension,
            string description,
            string editorClass,
            OpenEditorCallback openEditorCallback)
        {
            Extension = extension;
            Description = description;
            EditorClass = editorClass;
            OpenEditorCallback = openEditorCallback;
        }

        public string Extension
        {
            get;
        }

        public string Description
        {
            get;
        }

        public string EditorClass
        {
            get;
        }

        public OpenEditorCallback OpenEditorCallback
        {
            get;
        }

        public override string ToString()
        {
            return Description;
        }
    }
}
