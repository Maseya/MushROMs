// <copyright file="GfxEditor.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using Maseya.Editors.Collections;
    using Maseya.Editors.IO;
    using Format = GraphicsFormat;

    public class GfxEditor : SharedListChildEditor<GfxTile>, IGfxEditor
    {
        private Format _graphicsFormat;

        public GfxEditor(string path)
            : base(new SharedListParentEditor(path))
        {
        }

        public GfxEditor(
            string path,
            Format graphicsFormat,
            IEnumerable<GfxTile> collection)
            : base(new SharedListParentEditor(path, null))
        {
            GraphicsFormat = graphicsFormat;
        }

        public GfxEditor(SharedListParentEditor parent)
            : base(parent)
        {
        }

        private GfxEditor(ISharedListParent parent)
            : base(parent)
        {
        }

        public event EventHandler GraphicsFormatChanged;

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

        public int BytesPerTile
        {
            get
            {
                return GfxTileConverter.BytesPerTile(GraphicsFormat);
            }
        }

        public int ColorsPerPixel
        {
            get
            {
                return GfxTileConverter.ColorsPerPixel(GraphicsFormat);
            }
        }

        public Format GraphicsFormat
        {
            get
            {
                return _graphicsFormat;
            }

            set
            {
                if (!Enum.IsDefined(typeof(Format), (int)value))
                {
                    throw new InvalidEnumArgumentException(
                        nameof(value),
                        (int)value,
                        typeof(Format));
                }

                _graphicsFormat = value;
            }
        }

        public static OpenEditorCallback OpenByteFile(
            Format graphicsFormat)
        {
            return path =>
            {
                var parent = new SharedListParentEditor(
                    path,
                    File.ReadAllBytes(path));

                return new GfxEditor(parent)
                {
                    GraphicsFormat = graphicsFormat,
                };
            };
        }

        public override int GetIndex(int offset, int startOffset)
        {
            return (offset - startOffset) / BytesPerTile;
        }

        public override int GetOffset(int index, int startOffset)
        {
            return startOffset + (index * BytesPerTile);
        }

        public override int GetSizeOfItem(GfxTile item)
        {
            return BytesPerTile;
        }

        public override int GetSizeOfItemAtOffset(int offset)
        {
            return BytesPerTile;
        }

        public override IEnumerable<byte> GetByteData(GfxTile item)
        {
            var count = GfxTileConverter.BytesPerTile(GraphicsFormat);
            var result = new byte[count];
            GfxTileConverter.WriteTileToFormattedBytes(
                in item,
                result,
                0,
                GraphicsFormat);

            return result;
        }

        public override GfxTile GetItem(IEnumerable<byte> data)
        {
            var count = GfxTileConverter.BytesPerTile(GraphicsFormat);
            var list = new List<byte>(data.Limit(count));
            return GfxTileConverter.ReadTileFromFormattedBytes(
                list,
                0,
                GraphicsFormat);
        }

        protected override ISharedListChild<GfxTile> Create(
                            ISharedListParent parent)
        {
            var result = new GfxEditor(parent)
            {
                GraphicsFormat = GraphicsFormat,
                StartOffset = StartOffset,
            };

            return result;
        }
    }
}
