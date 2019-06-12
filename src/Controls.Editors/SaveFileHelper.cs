// <copyright file="SaveFileHelper.cs" company="Public Domain">
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
    using Maseya.Controls.Editors.Properties;
    using Maseya.Editors;
    using Maseya.Editors.IO;
    using static System.ComponentModel.DesignerSerializationVisibility;
    using SR = Maseya.Helper.StringHelper;

    public class SaveFileHelper : SaveFileHelperBase
    {
        public SaveFileHelper()
            : base()
        {
            SaveFileDialog = new SaveFileDialog();

            SaveAsDialog = new SaveAsDialog();
            SaveFileAssociations = new List<SaveFileAssociation>();
        }

        public SaveFileHelper(IContainer container)
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

        private SaveFileDialog SaveFileDialog
        {
            get;
        }

        private SaveAsDialog SaveAsDialog
        {
            get;
        }

        private List<SaveFileAssociation> SaveFileAssociations
        {
            get;
        }

        public void AddSaveFileAssociation(
                string extension,
                string description,
                string editorClass,
                SaveEditorCallback saveEditorCallback)
        {
            var association = new SaveFileAssociation()
            {
                Extension = extension,
                Description = description,
                EditorClass = editorClass,
                SaveEditorCallback = saveEditorCallback,
            };

            AddSaveFileAssociation(association);
        }

        public void AddSaveFileAssociation(SaveFileAssociation association)
        {
            if (association is null)
            {
                throw new ArgumentNullException(nameof(association));
            }

            SaveFileAssociations.Add(association);
            SaveAsDialog.AddAssociation(association);
            AddAssociation(association);
        }

        public void SaveFile(IWin32Window owner, IEditor editor)
        {
            Owner = owner;
            SaveFile(editor);
        }

        public void SaveFile(IWin32Window owner, string path, IEditor editor)
        {
            Owner = owner;
            SaveFile(editor, path);
        }

        public void SaveFile(
            IWin32Window owner,
            IEditor editor,
            SaveEditorCallback saveEditor)
        {
            Owner = owner;
            SaveFile(editor, saveEditor);
        }

        public void SaveFile(
            IWin32Window owner,
            IEditor editor,
            string path,
            SaveEditorCallback saveEditor)
        {
            Owner = owner;
            SaveFile(editor, path, saveEditor);
        }

        public void SaveFileAs(IWin32Window owner, IEditor editor)
        {
            Owner = owner;
            SaveFileAs(editor);
        }

        public void SaveFileAs(
            IWin32Window owner,
            IEditor editor,
            string path)
        {
            Owner = owner;
            SaveFileAs(editor, path);
        }

        protected override SaveEditorCallback UISelectSaveEditorCallback(
            string path)
        {
            throw new NotImplementedException();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                SaveFileDialog.Dispose();
                SaveAsDialog.Dispose();
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
                var filterEntry = SR.GetUIString(
                    Resources.OpenEditorFileDialogRowFormat,
                    kvp.Key,
                    exts);

                var extListEntry = SR.GetUIString(
                    Resources.OpenEditorFileDialogExtensionListFormat,
                    exts);

                filter.Append(filterEntry);

                allExts.Append(extListEntry);
            }

            if (allExts.Length > 0)
            {
                allExts.Remove(allExts.Length - 1, 1);
            }

            var result = SR.GetUIString(
                Resources.OpenEditorFileDialogFilterFormat,
                allExts,
                filter);

            return result;

            string ExtensionList(IEnumerable<string> extensions)
            {
                var sb = new StringBuilder();
                foreach (var ext in extensions)
                {
                    var entry = SR.GetUIString(
                        Resources.OpenEditorFileDialogExtensionEntryFormat,
                        ext);

                    sb.Append(entry);
                }

                sb.Remove(sb.Length - 1, 1);
                return sb.ToString();
            }
        }

        private Dictionary<string, List<string>> CreateFilterDictionary()
        {
            var dictionary = new Dictionary<string, List<string>>();
            foreach (var association in SaveFileAssociations)
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
