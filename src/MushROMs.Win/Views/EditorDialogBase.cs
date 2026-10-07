// <copyright file="EditorDialogBase.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.Win.Views;

using System;
using System.ComponentModel;
using System.Windows.Forms;

public partial class EditorDialogBase : Component
{
    private IWin32Window? _win32Window;

    public EditorDialogBase()
    {
        InitializeComponent();
    }

    public EditorDialogBase(IContainer container)
    {
        container.Add(this);

        InitializeComponent();
    }

    public event EventHandler? OwnerChanged;

    public IWin32Window? Owner
    {
        get
        {
            return _win32Window;
        }

        set
        {
            if (Owner == value)
            {
                return;
            }

            _win32Window = value;
            OnOwnerChanged(EventArgs.Empty);
        }
    }

    protected virtual void OnOwnerChanged(EventArgs e)
    {
        OwnerChanged?.Invoke(this, e);
    }
}
