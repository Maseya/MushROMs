// <copyright file="ISharedListChildEditor.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Editors.Collections
{
    public interface ISharedListChildEditor<T> :
        ISharedListChild<T>,
        IListEditor<T>
    {
    }
}
