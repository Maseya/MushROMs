// <copyright file="IZoomComponent.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Controls.TileMaps
{
    using System;
    using System.ComponentModel;

    public interface IZoomComponent : IComponent
    {
        event EventHandler ZoomSizeChanged;

        ZoomSize ZoomSize
        {
            get;
            set;
        }
    }
}
