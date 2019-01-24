// <copyright file="IListEditor.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Editors.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public interface IListEditor<T> : IEditor, ISelectionList<T>
    {
        IListSelectionData<T> CopyData
        {
            get;
            set;
        }

        IListSelection CurrentSelection
        {
            get;
            set;
        }

        bool IsInsert
        {
            get;
            set;
        }

        void Copy(IListSelection selection);

        void Cut(IListSelection selection);

        void Delete(IListSelection selection);

        void Paste(IListSelectionData<T> values, bool insert);

        void ModifyList(Action<IList<T>> action, Action<IList<T>> redo);
    }

    public interface IListEditor : IEditor, ISelectionList
    {
        IListSelectionData CopyData
        {
            get;
            set;
        }

        IListSelection CurrentSelection
        {
            get;
            set;
        }

        bool IsInsert
        {
            get;
            set;
        }

        void Copy(IListSelection selection);

        void Cut(IListSelection selection);

        void Delete(IListSelection selection);

        void Paste(IListSelectionData values, bool insert);

        void ModifyList(Action<IList> action, Action<IList> redo);
    }
}
