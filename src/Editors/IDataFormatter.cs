// <copyright file="IDataFormatter.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Editors
{
    using System.Collections.Generic;

    public interface IDataFormatter<T>
    {
        IEnumerable<T> ToFormattedData(IEnumerable<byte> byteData);

        IEnumerable<byte> ToByteData(IEnumerable<T> formattedData);
    }
}
