// <copyright file="GfxPixelEditorControl.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;
    using Maseya.Controls.TileMaps;
    using Maseya.Editors.Collections;
    using Maseya.Helper.PixelFormat;
    using Maseya.TileMaps;

    public partial class GfxPixelEditorControl : TileMapUIControl
    {
        public GfxPixelEditorControl()
        {
            InitializeComponent();
        }
    }
}
