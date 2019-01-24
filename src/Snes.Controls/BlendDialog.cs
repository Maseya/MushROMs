// <copyright file="BlendDialog.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes.Controls
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;
    using Maseya.Controls;
    using Maseya.Helper;
    using static System.ComponentModel.DesignerSerializationVisibility;

    public sealed class BlendDialog : DialogProxy
    {
        public BlendDialog()
            : base()
        {
            BlendForm = new BlendForm();
        }

        public BlendDialog(IContainer container)
            : this()
        {
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.Add(this);
        }

        public event EventHandler ColorValueChanged;

        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public float Red
        {
            get
            {
                return BlendForm.Red;
            }

            set
            {
                BlendForm.Red = value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public float Green
        {
            get
            {
                return BlendForm.Green;
            }

            set
            {
                BlendForm.Green = value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public float Blue
        {
            get
            {
                return BlendForm.Blue;
            }

            set
            {
                BlendForm.Blue = value;
            }
        }

        public ColorF Color
        {
            get
            {
                return BlendForm.Color;
            }

            set
            {
                BlendForm.Color = value;
            }
        }

        public BlendMode BlendMode
        {
            get
            {
                return BlendForm.BlendMode;
            }

            set
            {
                BlendForm.BlendMode = value;
            }
        }

        public bool Preview
        {
            get
            {
                return BlendForm.Preview;
            }

            set
            {
                BlendForm.Preview = value;
            }
        }

        protected override Form BaseForm
        {
            get
            {
                return BlendForm;
            }
        }

        private BlendForm BlendForm
        {
            get;
        }

        public void ResetValues()
        {
            BlendForm.ResetValues();
        }

        private void BlendForm_ColorValueChanged(object sender, EventArgs e)
        {
            ColorValueChanged?.Invoke(this, e);
        }
    }
}
