// <copyright file="RecentFileToolStripItem.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.MushROMs
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    public partial class RecentFileToolStripItem : ToolStripMenuItem
    {
        private string _path;

        public RecentFileToolStripItem()
        {
            InitializeComponent();

            tsmOpen.Click += (s, e) => OnOpen(EventArgs.Empty);
            tsmOpenAs.Click += (s, e) => OnOpenAs(EventArgs.Empty);
            tsmRemove.Click += (s, e) => OnRemove(EventArgs.Empty);
        }

        public RecentFileToolStripItem(IContainer container)
            : this()
        {
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.Add(this);
        }

        public event EventHandler PathChanged;

        public event EventHandler Open;

        public event EventHandler OpenAs;

        public event EventHandler Remove;

        public string Path
        {
            get
            {
                return _path;
            }

            set
            {
                if (Path == value)
                {
                    return;
                }

                _path = value;
                OnPathChanged(EventArgs.Empty);
            }
        }

        protected virtual void OnPathChanged(EventArgs e)
        {
            PathChanged?.Invoke(this, e);
        }

        protected virtual void OnOpen(EventArgs e)
        {
            Open?.Invoke(this, e);
        }

        protected virtual void OnOpenAs(EventArgs e)
        {
            OpenAs?.Invoke(this, e);
        }

        protected virtual void OnRemove(EventArgs e)
        {
            Remove?.Invoke(this, e);
        }
    }
}
