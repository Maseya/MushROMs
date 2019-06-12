// <copyright file="TileMap1DRenderer.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.TileMaps
{
    using System.ComponentModel;

    public abstract class TileMap1DRenderer : TileMapRenderer
    {
        public TileMap1DRenderer()
            : base()
        {
        }

        public TileMap1DRenderer(IContainer container)
            : base(container)
        {
        }

        public new TileMap1D TileMap
        {
            get
            {
                return base.TileMap as TileMap1D;
            }

            set
            {
                base.TileMap = value;
            }
        }
    }
}
