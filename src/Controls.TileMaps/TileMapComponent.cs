// <copyright file="TileMapComponent.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Controls.TileMaps
{
    using System;
    using System.ComponentModel;
    using Maseya.TileMaps;

    public abstract class TileMapComponent : Component
    {
        private TileMap _tileMap;

        protected TileMapComponent()
        {
        }

        protected TileMapComponent(IContainer container)
        {
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.Add(this);
        }

        public event EventHandler TileMapChanged;

        public TileMap TileMap
        {
            get
            {
                return _tileMap;
            }

            set
            {
                _tileMap = value;
                OnTileMapChanged(EventArgs.Empty);
            }
        }

        protected virtual void OnTileMapChanged(EventArgs e)
        {
            TileMapChanged?.Invoke(this, e);
        }
    }
}
