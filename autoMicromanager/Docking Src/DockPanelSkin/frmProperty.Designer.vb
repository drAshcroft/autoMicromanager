<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmProperty
    Inherits WeifenLuo.WinFormsUI.DockContent

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmProperty))
        Me.Properties = New PropertyGridEx.PropertyGridEx
        Me.ContextMenuDock = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.FloatingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.DockableToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.TabbedDocumentToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AutoHideToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.HideToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ContextMenuDock.SuspendLayout()
        Me.SuspendLayout()
        '
        'Properties
        '
        '
        '
        '
        Me.Properties.DocCommentDescription.AutoEllipsis = True
        Me.Properties.DocCommentDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.Properties.DocCommentDescription.Location = New System.Drawing.Point(3, 19)
        Me.Properties.DocCommentDescription.Name = ""
        Me.Properties.DocCommentDescription.Size = New System.Drawing.Size(286, 36)
        Me.Properties.DocCommentDescription.TabIndex = 1
        Me.Properties.DocCommentImage = Nothing
        '
        '
        '
        Me.Properties.DocCommentTitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.Properties.DocCommentTitle.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Properties.DocCommentTitle.Location = New System.Drawing.Point(3, 3)
        Me.Properties.DocCommentTitle.Name = ""
        Me.Properties.DocCommentTitle.Size = New System.Drawing.Size(286, 16)
        Me.Properties.DocCommentTitle.TabIndex = 0
        Me.Properties.DocCommentTitle.UseMnemonic = False
        Me.Properties.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Properties.Location = New System.Drawing.Point(0, 0)
        Me.Properties.Name = "Properties"
        Me.Properties.Size = New System.Drawing.Size(292, 266)
        Me.Properties.TabIndex = 0
        '
        '
        '
        Me.Properties.ToolStrip.AccessibleName = "ToolBar"
        Me.Properties.ToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar
        Me.Properties.ToolStrip.AllowMerge = False
        Me.Properties.ToolStrip.AutoSize = False
        Me.Properties.ToolStrip.CanOverflow = False
        Me.Properties.ToolStrip.Dock = System.Windows.Forms.DockStyle.None
        Me.Properties.ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.Properties.ToolStrip.Location = New System.Drawing.Point(0, 1)
        Me.Properties.ToolStrip.Name = ""
        Me.Properties.ToolStrip.Padding = New System.Windows.Forms.Padding(2, 0, 1, 0)
        Me.Properties.ToolStrip.Size = New System.Drawing.Size(292, 25)
        Me.Properties.ToolStrip.TabIndex = 1
        Me.Properties.ToolStrip.TabStop = True
        Me.Properties.ToolStrip.Text = "PropertyGridToolBar"
        '
        'ContextMenuDock
        '
        Me.ContextMenuDock.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FloatingToolStripMenuItem, Me.DockableToolStripMenuItem, Me.TabbedDocumentToolStripMenuItem, Me.AutoHideToolStripMenuItem, Me.HideToolStripMenuItem})
        Me.ContextMenuDock.Name = "ContextMenuStrip1"
        Me.ContextMenuDock.Size = New System.Drawing.Size(173, 114)
        Me.ContextMenuDock.Text = "Window Position"
        '
        'FloatingToolStripMenuItem
        '
        Me.FloatingToolStripMenuItem.CheckOnClick = True
        Me.FloatingToolStripMenuItem.Name = "FloatingToolStripMenuItem"
        Me.FloatingToolStripMenuItem.Size = New System.Drawing.Size(172, 22)
        Me.FloatingToolStripMenuItem.Text = "Floating"
        '
        'DockableToolStripMenuItem
        '
        Me.DockableToolStripMenuItem.Checked = True
        Me.DockableToolStripMenuItem.CheckOnClick = True
        Me.DockableToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.DockableToolStripMenuItem.Name = "DockableToolStripMenuItem"
        Me.DockableToolStripMenuItem.Size = New System.Drawing.Size(172, 22)
        Me.DockableToolStripMenuItem.Text = "Dockable"
        '
        'TabbedDocumentToolStripMenuItem
        '
        Me.TabbedDocumentToolStripMenuItem.CheckOnClick = True
        Me.TabbedDocumentToolStripMenuItem.Name = "TabbedDocumentToolStripMenuItem"
        Me.TabbedDocumentToolStripMenuItem.Size = New System.Drawing.Size(172, 22)
        Me.TabbedDocumentToolStripMenuItem.Text = "Tabbed Document"
        '
        'AutoHideToolStripMenuItem
        '
        Me.AutoHideToolStripMenuItem.CheckOnClick = True
        Me.AutoHideToolStripMenuItem.Name = "AutoHideToolStripMenuItem"
        Me.AutoHideToolStripMenuItem.Size = New System.Drawing.Size(172, 22)
        Me.AutoHideToolStripMenuItem.Text = "Auto Hide"
        '
        'HideToolStripMenuItem
        '
        Me.HideToolStripMenuItem.CheckOnClick = True
        Me.HideToolStripMenuItem.Name = "HideToolStripMenuItem"
        Me.HideToolStripMenuItem.Size = New System.Drawing.Size(172, 22)
        Me.HideToolStripMenuItem.Text = "Hide"
        '
        'frmProperty
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(292, 266)
        Me.Controls.Add(Me.Properties)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HideOnClose = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmProperty"
        Me.ShowHint = WeifenLuo.WinFormsUI.DockState.DockLeft
        Me.TabPageContextMenuStrip = Me.ContextMenuDock
        Me.TabText = "Properties"
        Me.Text = "Properties"
        Me.ContextMenuDock.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Properties As PropertyGridEx.PropertyGridEx
    Friend WithEvents ContextMenuDock As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents FloatingToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DockableToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TabbedDocumentToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AutoHideToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HideToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
