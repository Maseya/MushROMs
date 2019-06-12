// <copyright file="TileMapSelection1D.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Editors.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text;
    using Maseya.TileMaps;

    public class TileMapSelection1D
    {
        public IListSelection ListSelection
        {
            get;
        }

        public TileMap1D TileMap
        {
            get;
        }

        public bool ContainsGridTile(Point gridTile)
        {
            if (!TileMap.GridContainsGridTile(gridTile))
            {
                return false;
            }

            var index = TileMap.GetGridTileIndex(gridTile);
            return ListSelection.ContainsIndex(index);
        }
    }
}
