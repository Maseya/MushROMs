// <copyright file="HeaderEditorDialog.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.Win.Dialogs;

using System;
using System.ComponentModel;
using System.Windows.Forms;

using BaseForms;

using Controls;

using Maseya.Smas.Smb1.AreaData.HeaderData;

using static System.ComponentModel.DesignerSerializationVisibility;

public sealed class HeaderEditorDialog : DialogProxy
{
    public HeaderEditorDialog()
    {
        HeaderEditorForm = new HeaderEditorForm();
        HeaderEditorForm.AreaHeaderChanged += HeaderEditorForm_AreaHeaderChanged;
    }

    public HeaderEditorDialog(IContainer container)
        : this()
    {
        container.Add(this);
    }

    public event EventHandler? AreaHeaderChanged;

    [Browsable(false)]
    [DesignerSerializationVisibility(Hidden)]
    public AreaHeader AreaHeader
    {
        get
        {
            return HeaderEditorForm.AreaHeader;
        }

        set
        {
            HeaderEditorForm.AreaHeader = value;
        }
    }

    protected override Form BaseForm
    {
        get
        {
            return HeaderEditorForm;
        }
    }

    private HeaderEditorForm HeaderEditorForm
    {
        get;
    }

    private void HeaderEditorForm_AreaHeaderChanged(object? sender, EventArgs e)
    {
        AreaHeaderChanged?.Invoke(this, EventArgs.Empty);
    }
}
