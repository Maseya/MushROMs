// <copyright file="HeaderEditor.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.Win.Views;

using System;
using System.ComponentModel;
using System.Windows.Forms;

using MushROMs.Core.Editors;

using Core;

using Maseya.Smas.Smb1.AreaData.HeaderData;

using static System.ComponentModel.DesignerSerializationVisibility;

public sealed partial class HeaderEditor : EditorDialogBase, IHeaderEditorView
{
    public HeaderEditor()
        : base()
    {
        InitializeComponent();
    }

    public HeaderEditor(IContainer container)
        : base(container)
    {
        InitializeComponent();
    }

    public event EventHandler? AreaHeaderChanged;

    [Browsable(false)]
    [DesignerSerializationVisibility(Hidden)]
    public AreaHeader AreaHeader
    {
        get
        {
            return headerEditorDialog.AreaHeader;
        }

        set
        {
            headerEditorDialog.AreaHeader = value;
        }
    }

    public bool Prompt()
    {
        return headerEditorDialog.ShowDialog(Owner) == DialogResult.OK;
    }

    private void HeaderEditorDialog_AreaHeaderChanged(object? sender, EventArgs e)
    {
        AreaHeaderChanged?.Invoke(this, EventArgs.Empty);
    }
}
