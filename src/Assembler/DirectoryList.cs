// <copyright file="DirectoryList.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Assembler
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;

    public class DirectoryList : IList<string>
    {
        public int Count
        {
            get
            {
                return BaseList.Count;
            }
        }

        bool ICollection<string>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        private List<string> BaseList
        {
            get;
        }

        public string this[int index]
        {
            get
            {
                return BaseList[index];
            }

            set
            {
                BaseList[index] = value;
            }
        }

        public void Add(string item)
        {
            BaseList.Add(item);
        }

        public void Clear()
        {
            BaseList.Clear();
        }

        public bool Contains(string item)
        {
            return BaseList.Contains(item);
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            BaseList.CopyTo(array, arrayIndex);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return BaseList.GetEnumerator();
        }

        public int IndexOf(string item)
        {
            return BaseList.IndexOf(item);
        }

        public void Insert(int index, string item)
        {
            BaseList.Insert(index, item);
        }

        public bool Remove(string item)
        {
            return BaseList.Remove(item);
        }

        public void RemoveAt(int index)
        {
            BaseList.RemoveAt(index);
        }

        public string ResolvePath(string path)
        {
            if (String.IsNullOrEmpty(path))
            {
                return path;
            }

            bool isPathRooted;
            try
            {
                isPathRooted = Path.IsPathRooted(path);
            }
            catch (ArgumentException)
            {
                return path;
            }

            foreach (var directory in this)
            {
                string result;
                try
                {
                    result = Path.Combine(directory, path);
                }
                catch
                {
                    continue;
                }

                if (File.Exists(result))
                {
                    return result;
                }
            }

            return path;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
