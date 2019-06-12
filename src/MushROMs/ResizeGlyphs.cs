// <copyright file="ResizeGlyphs.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.MushROMs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public partial class ResizeGlyphs : Component
    {
        private Control _control;

        public ResizeGlyphs()
        {
            InitializeComponent();
        }

        public ResizeGlyphs(IContainer container)
            : this()
        {
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.Add(this);
        }

        public event EventHandler ControlChanged;

        public Control Control
        {
            get
            {
                return _control;
            }

            set
            {
                if (Control == value)
                {
                    return;
                }

                _control = value;
                OnControlChanged(EventArgs.Empty);
            }
        }

        protected virtual void OnControlChanged(EventArgs e)
        {
            ControlChanged?.Invoke(this, e);
        }
    }
}
