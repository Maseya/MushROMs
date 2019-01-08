// <copyright file="GfxTile.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class GfxTile :
            IList,
            IList<byte>,
            IReadOnlyList<byte>
    {
        public const int DotsPerPlane = 8;
        public const int PlanesPerTile = DotsPerPlane;
        public const int DotsPerTile = DotsPerPlane * PlanesPerTile;

        private GraphicsFormat _graphicsFormat;

        public GfxTile(
            IList<byte> source,
            int startIndex,
            GraphicsFormat graphicsFormat)
        {
            Source = source ??
                throw new ArgumentNullException(nameof(source));
        }

        public int StartIndex
        {
            get;
            set;
        }

        public GraphicsFormat GraphicsFormat
        {
            get
            {
                return _graphicsFormat;
            }

            set
            {
                if (!Enum.IsDefined(typeof(GraphicsFormat), (int)value))
                {
                    throw new InvalidEnumArgumentException(
                        nameof(value),
                        (int)value,
                        typeof(GraphicsFormat));
                }

                _graphicsFormat = value;
            }
        }

        public int BitsPerPixel
        {
            get
            {
                return GfxTileConverter.BitsPerPixel(GraphicsFormat);
            }
        }

        public int BytesPerPlane
        {
            get
            {
                return GfxTileConverter.BytesPerPlane(GraphicsFormat);
            }
        }

        public int ColorsPerPixel
        {
            get
            {
                return GfxTileConverter.ColorsPerPixel(GraphicsFormat);
            }
        }

        public int BytesPerTile
        {
            get
            {
                return GfxTileConverter.BytesPerTile(GraphicsFormat);
            }
        }

        public GfxTileConverter Converter
        {
            get
            {
                return GfxTileConverter.GetTileConverter(GraphicsFormat);
            }
        }

        private IList<byte> Source
        {
            get;
        }

        int ICollection<byte>.Count
        {
            get
            {
                return DotsPerTile;
            }
        }

        int ICollection.Count
        {
            get
            {
                return DotsPerTile;
            }
        }

        int IReadOnlyCollection<byte>.Count
        {
            get
            {
                return DotsPerTile;
            }
        }

        bool IList.IsFixedSize
        {
            get
            {
                return true;
            }
        }

        bool ICollection<byte>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        bool IList.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return false;
            }
        }

        object ICollection.SyncRoot
        {
            get;
        }

        public byte this[int index]
        {
            get
            {
                return Converter.ReadPixel(this, StartIndex, index);
            }

            set
            {
                Converter.WritePixel(value, this, StartIndex, index);
            }
        }

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }

            set
            {
                this[index] = (byte)value;
            }
        }

        public void Write(IEnumerable<byte> data)
        {
            Converter.GetBytes(data);
        }

        public bool Contains(byte value)
        {
            foreach (var x in this)
            {
                if (Equals(value, x))
                {
                    return true;
                }
            }

            return false;
        }

        bool IList.Contains(object value)
        {
            return Contains((byte)value);
        }

        public void CopyTo(byte[] array, int arrayIndex)
        {
            if (array is null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (arrayIndex < 0 || arrayIndex >= array.Length)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (arrayIndex + DotsPerTile > array.Length)
            {
                throw new ArgumentException();
            }

            for (var i = 0; i < DotsPerTile; i++)
            {
                array[arrayIndex + i] = this[i];
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            if (array is null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (index < 0 || index >= array.Length)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (index + DotsPerTile > array.Length)
            {
                throw new ArgumentException();
            }

            for (var i = 0; i < DotsPerTile; i++)
            {
                array.SetValue(index + i, this[i]);
            }
        }

        public IEnumerator<byte> GetEnumerator()
        {
            return Converter.GetPixels(this).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int IndexOf(byte value)
        {
            for (var i = 0; i < DotsPerTile; i++)
            {
                if (Equals(value, this[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        int IList.IndexOf(object value)
        {
            return IndexOf((byte)value);
        }

        void ICollection<byte>.Add(byte item)
        {
            throw new NotSupportedException();
        }

        int IList.Add(object value)
        {
            throw new NotSupportedException();
        }

        void ICollection<byte>.Clear()
        {
            throw new NotSupportedException();
        }

        void IList.Clear()
        {
            throw new NotSupportedException();
        }

        void IList<byte>.Insert(int index, byte item)
        {
            throw new NotSupportedException();
        }

        void IList.Insert(int index, object value)
        {
            throw new NotSupportedException();
        }

        bool ICollection<byte>.Remove(byte item)
        {
            throw new NotSupportedException();
        }

        void IList.Remove(object value)
        {
            throw new NotSupportedException();
        }

        void IList<byte>.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        void IList.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }
    }
}
