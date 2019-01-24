// <copyright file="OpenFileHelper.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Controls.Editors
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Text;
    using System.Windows.Forms;
    using Maseya.Editors.IO;
    using static System.ComponentModel.DesignerSerializationVisibility;

    public class OpenFileHelper : OpenFileHelperBase
    {
        public OpenFileHelper()
            : base()
        {
            OpenFileDialog = new OpenFileDialog();
            OpenWithDialog = new OpenWithDialog();
            OpenFileAssociations = new List<OpenFileAssociation>();
        }

        public OpenFileHelper(IContainer container)
            : this()
        {
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.Add(this);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public IWin32Window Owner
        {
            get;
            set;
        }

        private OpenFileDialog OpenFileDialog
        {
            get;
        }

        private OpenWithDialog OpenWithDialog
        {
            get;
        }

        private List<OpenFileAssociation> OpenFileAssociations
        {
            get;
        }

        public void AddOpenFileAssociation(
                string extension,
                string description,
                string editorClass,
                OpenEditorCallback openEditorCallback)
        {
            var association = new OpenFileAssociation(
                extension,
                description,
                editorClass,
                openEditorCallback);

            AddOpenFileAssociation(association);
        }

        public void AddOpenFileAssociation(OpenFileAssociation association)
        {
            if (association is null)
            {
                throw new ArgumentNullException(nameof(association));
            }

            OpenFileAssociations.Add(association);
            AddAssociation(
                association.Extension,
                association.OpenEditorCallback);
        }

        public void OpenFile(IWin32Window owner)
        {
            Owner = owner;
            OpenFile();
        }

        public void OpenFile(IWin32Window owner, string path)
        {
            Owner = owner;
            OpenFile(path);
        }

        public void OpenFile(IWin32Window owner, OpenEditorCallback openEditor)
        {
            Owner = owner;
            OpenFile(openEditor);
        }

        public void OpenFile(
            IWin32Window owner,
            string path,
            OpenEditorCallback openEditor)
        {
            Owner = owner;
            OpenFile(path, openEditor);
        }

        protected override IEnumerable<string> UIChooseFiles()
        {
            var filter = CreateFilter();
            OpenFileDialog.Filter = filter;
            return OpenFileDialog.ShowDialog(Owner) == DialogResult.OK
                ? OpenFileDialog.FileNames
                : null;
        }

        protected override OpenEditorCallback UISelectOpenEditorCallback(
            string path)
        {
            return OpenWithDialog.ShowDialog(Owner) == DialogResult.OK
                ? OpenWithDialog.OpenEditorCallback
                : null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                OpenFileDialog.Dispose();
                OpenWithDialog.Dispose();
            }

            base.Dispose(disposing);
        }

        private string CreateFilter()
        {
            var dictionary = CreateFilterDictionary();
            var filter = new StringBuilder();
            var allExts = new StringBuilder();
            foreach (var kvp in dictionary)
            {
                var exts = ExtensionList(kvp.Value);
                filter.Append($"{kvp.Key} ({exts})");
                filter.Append('|');
                filter.Append(exts);
                filter.Append('|');

                allExts.Append(exts);
                allExts.Append(';');
            }

            allExts.Remove(allExts.Length - 1, 1);

            var result = new StringBuilder();
            result.Append("All Editors");
            result.Append('|');
            result.Append(allExts);
            result.Append('|');
            result.Append(filter);
            result.Append("All files");
            result.Append(" (*.*)|*.*");

            return result.ToString();

            string ExtensionList(IEnumerable<string> extensions)
            {
                var sb = new StringBuilder();
                foreach (var ext in extensions)
                {
                    sb.Append('*');
                    sb.Append(ext);
                    sb.Append(';');
                }

                sb.Remove(sb.Length - 1, 1);
                return sb.ToString();
            }
        }

        private Dictionary<string, List<string>> CreateFilterDictionary()
        {
            var dictionary = new Dictionary<string, List<string>>();
            foreach (var association in OpenFileAssociations)
            {
                var key = association.EditorClass;
                var value = association.Extension;
                if (dictionary.TryGetValue(key, out var list))
                {
                    list.Add(value);
                }
                else
                {
                    dictionary.Add(key, new List<string>() { value });
                }
            }

            return dictionary;
        }
    }
}
