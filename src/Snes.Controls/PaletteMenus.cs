// <copyright file="PaletteMenus.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Forms;
    using Maseya.Controls.Editors;
    using static System.ComponentModel.DesignerSerializationVisibility;

    public partial class PaletteMenus : Component, IEditorMenu
    {
        public PaletteMenus()
        {
            InitializeComponent();

            var events = new List<(ToolStripItem, Action<EventArgs>)>()
            {
                (tsmInvertColors, OnInvertColorsClick),
                (tsbInvertColors, OnInvertColorsClick),
                (tsmBlend, OnBlendClick),
                (tsbBlend, OnBlendClick),
                (tsmColorize, OnColorizeClick),
                (tsbColorize, OnColorizeClick),
                (tsmGrayscale, OnGrayscaleClick),
                (tsbGrayscale, OnGrayscaleClick),
                (tsmHorizontalGradient, OnHorizontalGradientClick),
                (tsbHorizontalGradient, OnHorizontalGradientClick),
                (tsmVerticalGradient, OnVerticalGradientClick),
                (tsbVerticalGradient, OnVerticalGradientClick),
            };

            foreach (var (tsi, action) in events)
            {
                tsi.Click += (s, e) => action(EventArgs.Empty);
            }
        }

        public PaletteMenus(IContainer container)
            : this()
        {
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.Add(this);
        }

        public event EventHandler InvertColorsClick;

        public event EventHandler BlendClick;

        public event EventHandler ColorizeClick;

        public event EventHandler GrayscaleClick;

        public event EventHandler HorizontalGradientClick;

        public event EventHandler VerticalGradientClick;

        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public ToolStripItemCollection MenuStripItems
        {
            get
            {
                return menuStrip.Items;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public ToolStripItemCollection ToolStripItems
        {
            get
            {
                return toolStrip.Items;
            }
        }

        public bool CanExportImageEnabled
        {
            get
            {
                return tsmExportImage.Enabled;
            }

            set
            {
                tsmExportImage.Enabled = value;
            }
        }

        public bool InvertColorsEnabled
        {
            get
            {
                return tsmInvertColors.Enabled;
            }

            set
            {
                tsmInvertColors.Enabled =
                tsbInvertColors.Enabled = value;
            }
        }

        public bool BlendEnabled
        {
            get
            {
                return tsmBlend.Enabled;
            }

            set
            {
                tsmBlend.Enabled =
                tsbBlend.Enabled = value;
            }
        }

        public bool ColorizeEnabled
        {
            get
            {
                return tsmColorize.Enabled;
            }

            set
            {
                tsmColorize.Enabled =
                tsbColorize.Enabled = value;
            }
        }

        public bool GrayscaleEnabled
        {
            get
            {
                return tsmGrayscale.Enabled;
            }

            set
            {
                tsmGrayscale.Enabled =
                tsbGrayscale.Enabled = value;
            }
        }

        public bool HorizontalGradientEnabled
        {
            get
            {
                return tsmHorizontalGradient.Enabled;
            }

            set
            {
                tsmHorizontalGradient.Enabled =
                tsbHorizontalGradient.Enabled = value;
            }
        }

        public bool VerticalGradientEnabled
        {
            get
            {
                return tsmVerticalGradient.Enabled;
            }

            set
            {
                tsmVerticalGradient.Enabled =
                tsbVerticalGradient.Enabled = value;
            }
        }

        protected virtual void OnInvertColorsClick(EventArgs e)
        {
            InvertColorsClick?.Invoke(this, e);
        }

        protected virtual void OnBlendClick(EventArgs e)
        {
            BlendClick?.Invoke(this, e);
        }

        protected virtual void OnColorizeClick(EventArgs e)
        {
            ColorizeClick?.Invoke(this, e);
        }

        protected virtual void OnGrayscaleClick(EventArgs e)
        {
            GrayscaleClick?.Invoke(this, e);
        }

        protected virtual void OnHorizontalGradientClick(EventArgs e)
        {
            HorizontalGradientClick?.Invoke(this, e);
        }

        protected virtual void OnVerticalGradientClick(EventArgs e)
        {
            VerticalGradientClick?.Invoke(this, e);
        }
    }
}
