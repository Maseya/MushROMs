// <copyright file="PaletteData.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes
{
    using System;
    using System.Collections.Generic;
    using Editors;
    using Helper.PixelFormat;

    /// <summary>
    /// Represents a list of <see cref="Color15BppBgr"/> that uses a <see
    /// cref="List{T}"/> of <see cref="Byte"/> as its internal data storage.
    /// </summary>
    public class PaletteData : EditorData<Color15BppBgr>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaletteData"/> class
        /// that is empty and has the default initial capacity.
        /// </summary>
        public PaletteData()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaletteData"/> class
        /// that is empty and has the specified initial capacity.
        /// </summary>
        /// <param name="capacity">
        /// The number of <see cref="Color15BppBgr"/> that the list can
        /// initially store.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="capacity"/> is less than 0.
        /// </exception>
        public PaletteData(int capacity)
            : base(capacity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaletteData"/> class
        /// that contains <see cref="Byte"/> data copied from a specified
        /// collection and has sufficient capacity to accommodate the number of
        /// elements copied.
        /// </summary>
        /// <param name="data">
        /// The collection to use as the internal data of this new list.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="data"/> is <see langword="null"/>.
        /// </exception>
        public PaletteData(IEnumerable<byte> data)
            : base(data)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaletteData"/> class
        /// that references an external data storage.
        /// </summary>
        /// <param name="data">
        /// The external data storage to read from.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="data"/> is <see langword="null"/>.
        /// </exception>
        private PaletteData(List<byte> data)
            : base(data)
        {
        }

        /// <summary>
        /// Gets the offset in the internal data array of a list index given
        /// the start offset.
        /// </summary>
        /// <param name="index">
        /// The zero-based list index to get the offset of.
        /// </param>
        /// <param name="startOffset">
        /// The starting offset to begin reading from in the internal data
        /// array.
        /// </param>
        /// <returns>
        /// The offset of <paramref name="index"/> starting at <paramref
        /// name="startOffset"/>.
        /// </returns>
        public override int GetOffset(int index, int startOffset)
        {
            return startOffset + (index * Color15BppBgr.SizeOf);
        }

        /// <summary>
        /// Gets the zero-based index of the object that contains the byte at a
        /// specified offset.
        /// </summary>
        /// <param name="offset">
        /// The offset of the byte to get the object index of.
        /// </param>
        /// <param name="startOffset">
        /// The starting offset to begin reading from in the internal data
        /// array.
        /// </param>
        /// <returns>
        /// The index of the object in <see cref="EditorData{T}"/> that
        /// contains the byte at <paramref name="offset"/>.
        /// </returns>
        public override int GetIndex(int offset, int startOffset)
        {
            return (offset - startOffset) / Color15BppBgr.SizeOf;
        }

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> of bytes that make up an
        /// object in the <see cref="EditorData{T}"/>.
        /// </summary>
        /// <param name="item">
        /// The object to convert to bytes
        /// </param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> of the bytes that define <paramref
        /// name="item"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="item"/> is <see langword="null"/>.
        /// </exception>
        protected override IEnumerable<byte> GetByteData(Color15BppBgr item)
        {
            yield return item.Low;
            yield return item.High;
        }

        /// <summary>
        /// Gets an instance of <see cref="Color15BppBgr"/> constructed from an
        /// <see cref="IEnumerable{T}"/> of bytes.
        /// </summary>
        /// <param name="data">
        /// The byte data to read from.
        /// </param>
        /// <returns>
        /// An instance of <see cref="Color15BppBgr"/> constructed from
        /// <paramref name="data"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="data"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="data"/> cannot properly construct the desired
        /// object.
        /// </exception>
        protected override Color15BppBgr GetItem(IEnumerable<byte> data)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var list = new List<byte>(Color15BppBgr.SizeOf);
            foreach (var item in data)
            {
                // Fail we want to move more than 2.
                if (list.Count == Color15BppBgr.SizeOf)
                {
                    throw new ArgumentException();
                }

                list.Add(item);
            }

            // Fail if we do not have exactly two bytes.
            if (list.Count != Color15BppBgr.SizeOf)
            {
                throw new ArgumentException();
            }

            return new Color15BppBgr(list[0], list[1]);
        }

        /// <summary>
        /// Gets the size, in bytes, of the item described by the internal data
        /// starting at a given offset.
        /// </summary>
        /// <param name="offset">
        /// The offset of the byte data to get the size of.
        /// </param>
        /// <returns>
        /// The size, in bytes, of the object described at <paramref
        /// name="offset"/>.
        /// </returns>
        public override int GetSizeOfItemAtOffset(int offset)
        {
            return Color15BppBgr.SizeOf;
        }

        /// <summary>
        /// Gets the size, in bytes, of a particular item.
        /// </summary>
        /// <param name="item">
        /// The <see cref="Color15BppBgr"/> to get the size of.
        /// </param>
        /// <returns>
        /// The size, in bytes, of <paramref name="item"/>.
        /// </returns>
        /// <remarks>
        /// All <see cref="Color15BppBgr"/> have a constant size value of <see
        /// cref="Color15BppBgr.SizeOf"/>.
        /// </remarks>
        public override int GetSizeOfItem(Color15BppBgr item)
        {
            return Color15BppBgr.SizeOf;
        }
    }
}
