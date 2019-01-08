namespace Maseya.Snes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Editors;
    using Format = GraphicsFormat;

    public class GfxData : EditorData<GfxTile>
    {
        private Format _graphicsFormat;

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

        public override int GetSizeOfItem(GfxTile item)
        {
            return BytesPerTile;
        }

        public override int GetSizeOfItemAtOffset(int offset)
        {
            return BytesPerTile;
        }

        protected override IEnumerable<byte> GetByteData(GfxTile item)
        {
            return GfxTileConverter.GetBytes(item, GraphicsFormat);
        }

        protected override GfxTile GetItem(IEnumerable<byte> data)
        {
            var list = new List<byte>(
                GfxTileConverter.GetPixels(data, GraphicsFormat));

            return new GfxTile(list, 0, GraphicsFormat);
        }

        public override int GetIndex(int offset, int startOffset)
        {
            return (offset - startOffset) / BytesPerTile;
        }

        public override int GetOffset(int index, int startOffset)
        {
            return startOffset + (index * BytesPerTile);
        }
    }
}
