Imports WeifenLuo.WinFormsUI

Public Class frmSolution
    Inherits DockContent

#Region "ContextMenuStrip Window Position"
    Private Sub FloatingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FloatingToolStripMenuItem.Click
        Me.DockState = WeifenLuo.WinFormsUI.DockState.Float
    End Sub

    Private Sub DockableToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DockableToolStripMenuItem.Click
        Me.DockState = Me.ShowHint
    End Sub

    Private Sub HideToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HideToolStripMenuItem.Click
        Me.Hide()
    End Sub

    Private Sub AutoHideToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoHideToolStripMenuItem.Click
        Select Case Me.DockState
            Case WeifenLuo.WinFormsUI.DockState.DockBottom
                DockState = WeifenLuo.WinFormsUI.DockState.DockBottomAutoHide
            Case WeifenLuo.WinFormsUI.DockState.DockTop
                DockState = WeifenLuo.WinFormsUI.DockState.DockTopAutoHide
            Case WeifenLuo.WinFormsUI.DockState.DockLeft
                DockState = WeifenLuo.WinFormsUI.DockState.DockLeftAutoHide
            Case WeifenLuo.WinFormsUI.DockState.DockRight
                DockState = WeifenLuo.WinFormsUI.DockState.DockRightAutoHide
        End Select
    End Sub

    Private Sub TabbedDocumentToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabbedDocumentToolStripMenuItem.Click
        DockState = WeifenLuo.WinFormsUI.DockState.Document
    End Sub
#End Region

    Private Sub frmSolution_DockStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DockStateChanged
        For Each item As ToolStripMenuItem In ContextMenuDock.Items
            item.Checked = False
        Next
        Select Case Me.DockState

            Case WeifenLuo.WinFormsUI.DockState.Document
                TabbedDocumentToolStripMenuItem.Checked = True

            Case WeifenLuo.WinFormsUI.DockState.Float
                FloatingToolStripMenuItem.Checked = True

            Case WeifenLuo.WinFormsUI.DockState.DockBottomAutoHide, _
                 WeifenLuo.WinFormsUI.DockState.DockLeftAutoHide, _
                 WeifenLuo.WinFormsUI.DockState.DockRightAutoHide, _
                 WeifenLuo.WinFormsUI.DockState.DockTopAutoHide

                AutoHideToolStripMenuItem.Checked = True

            Case WeifenLuo.WinFormsUI.DockState.DockBottom, _
                 WeifenLuo.WinFormsUI.DockState.DockLeft, _
                 WeifenLuo.WinFormsUI.DockState.DockRight, _
                 WeifenLuo.WinFormsUI.DockState.DockTop

                DockableToolStripMenuItem.Checked = True

        End Select
    End Sub

End Class