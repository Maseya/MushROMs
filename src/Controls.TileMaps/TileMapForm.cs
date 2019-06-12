// <copyright file="TileMapForm.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Controls.TileMaps
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using Maseya.Controls;
    using Maseya.TileMaps;
    using static System.ComponentModel.DesignerSerializationVisibility;

    public partial class TileMapForm : DesignForm
    {
        private TileMap _tileMap;

        public TileMapForm()
        {
            InitializeComponent();
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
                if (TileMap == value)
                {
                    return;
                }

                _tileMap = value;
                OnTileMapChanged(EventArgs.Empty);
            }
        }

        protected virtual void OnTileMapChanged(EventArgs e)
        {
            scrollHelper.TileMap =
            formResizeHelper.TileMap = TileMap;
            TileMapChanged?.Invoke(this, e);
        }
    }
}
