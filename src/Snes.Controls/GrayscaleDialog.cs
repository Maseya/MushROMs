// <copyright file="GrayscaleDialog.cs" company="Public Domain">
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

    public sealed class GrayscaleDialog : DialogProxy
    {
        public GrayscaleDialog()
            : base()
        {
            GrayscaleForm = new GrayscaleForm();
            GrayscaleForm.ColorValueChanged +=
                GrayscaleForm_ColorValueChanged;
        }

        public GrayscaleDialog(IContainer container)
            : this()
        {
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.Add(this);
        }

        public event EventHandler ColorValueChanged;

        public float Red
        {
            get
            {
                return GrayscaleForm.Red;
            }

            set
            {
                GrayscaleForm.Red = value;
            }
        }

        public float Green
        {
            get
            {
                return GrayscaleForm.Green;
            }

            set
            {
                GrayscaleForm.Green = value;
            }
        }

        public float Blue
        {
            get
            {
                return GrayscaleForm.Blue;
            }

            set
            {
                GrayscaleForm.Blue = value;
            }
        }

        public ColorF Color
        {
            get
            {
                return GrayscaleForm.Color;
            }

            set
            {
                GrayscaleForm.Color = value;
            }
        }

        public bool Preview
        {
            get
            {
                return GrayscaleForm.Preview;
            }

            set
            {
                GrayscaleForm.Preview = value;
            }
        }

        protected override Form BaseForm
        {
            get
            {
                return GrayscaleForm;
            }
        }

        private GrayscaleForm GrayscaleForm
        {
            get;
        }

        public void ResetValues()
        {
            GrayscaleForm.ResetValues();
        }

        private void GrayscaleForm_ColorValueChanged(
            object sender,
            EventArgs e)
        {
            ColorValueChanged?.Invoke(this, e);
        }
    }
}
