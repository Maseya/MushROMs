// <copyright file="Preprocessor.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Assembler
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Preprocessor
    {
        public Preprocessor(string path)
        {
            Files = new List<string>()
            {
                path,
            };

            PathIndexes = new Dictionary<string, int>(
                StringComparer.OrdinalIgnoreCase)
            {
                { path, 0 },
            };
        }

        private List<string> Files
        {
            get;
        }

        private Dictionary<string, int> PathIndexes
        {
            get;
        }

        public string GetPath(int fileNumber)
        {
            return Files[fileNumber];
        }

        public int GetFileNumber(string path)
        {
            return PathIndexes[path];
        }
    }
}
