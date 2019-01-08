// <copyright file="TileFlip.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes
{
    using System;

    [Flags]
    public enum TileFlip
    {
        None = 0,
        Horizontal = 1,
        Veritcal = 2,
        Both = Horizontal | Veritcal
    }
}
