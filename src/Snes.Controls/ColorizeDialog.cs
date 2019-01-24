// <copyright file="ColorizeDialog.cs" company="Public Domain">
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

    public sealed class ColorizeDialog : DialogProxy
    {
        public ColorizeDialog()
            : base()
        {
            ColorizeForm = new ColorizeForm();
            ColorizeForm.ColorValueChanged +=
                ColorizeForm_ColorValueChanged;
        }

        public ColorizeDialog(IContainer container)
            : this()
        {
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.Add(this);
        }

        public event EventHandler ColorValueChanged;

        public float Hue
        {
            get
            {
                return ColorizeForm.Hue;
            }

            set
            {
                ColorizeForm.Hue = value;
            }
        }

        public float Saturation
        {
            get
            {
                return ColorizeForm.Saturation;
            }

            set
            {
                ColorizeForm.Saturation = value;
            }
        }

        public float Lightness
        {
            get
            {
                return ColorizeForm.Lightness;
            }

            set
            {
                ColorizeForm.Lightness = value;
            }
        }

        public float Weight
        {
            get
            {
                return ColorizeForm.Weight;
            }

            set
            {
                ColorizeForm.Weight = value;
            }
        }

        public ColorizeMode ColorizeMode
        {
            get
            {
                return ColorizeForm.ColorizeMode;
            }

            set
            {
                ColorizeForm.ColorizeMode = value;
            }
        }

        public bool Luma
        {
            get
            {
                return ColorizeForm.Luma;
            }

            set
            {
                ColorizeForm.Luma = value;
            }
        }

        public bool Preview
        {
            get
            {
                return ColorizeForm.Preview;
            }

            set
            {
                ColorizeForm.Preview = value;
            }
        }

        protected override Form BaseForm
        {
            get
            {
                return ColorizeForm;
            }
        }

        private ColorizeForm ColorizeForm
        {
            get;
        }

        public void ResetValues()
        {
            ColorizeForm.ResetValues();
        }

        private void ColorizeForm_ColorValueChanged(object sender, EventArgs e)
        {
            ColorValueChanged?.Invoke(this, e);
        }
    }
}
