// <copyright file="TileMap2DRenderer.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.TileMaps
{
    using System.ComponentModel;

    public abstract class TileMap2DRenderer : TileMapRenderer
    {
        public TileMap2DRenderer()
            : base()
        {
        }

        public TileMap2DRenderer(IContainer container)
            : base(container)
        {
        }

        public new TileMap2D TileMap
        {
            get
            {
                return base.TileMap as TileMap2D;
            }

            set
            {
                base.TileMap = value;
            }
        }
    }
}
