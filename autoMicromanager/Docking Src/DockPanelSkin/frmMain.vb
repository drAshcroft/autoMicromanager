Imports System.Windows.Forms
Imports WeifenLuo.WinFormsUI
Imports VS2005Extender

Public Class frmMain

    Private SolutionExplorer As New frmSolution
    Private PropertiesWindow As New frmProperty

    Private m_ChildFormNumber As Integer = 0
    Private oDefaultRenderer As New ToolStripProfessionalRenderer(New PropertyGridEx.CustomColorScheme)

    Private Filename() As String = New String() {"frmMain.vb", _
                                                 "frmMain.vb [Design]", _
                                                 "frmDocument.vb", _
                                                 "frmDocument.vb [Design]", _
                                                 "frmProperty.vb", _
                                                 "frmProperty.vb [Design]", _
                                                 "frmSolution.vb", _
                                                 "frmSolution.vb [Design]"}

    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim configFile As String = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config")

        ' Apply a gray professional renderer as a default renderer
        ToolStripManager.Renderer = oDefaultRenderer
        oDefaultRenderer.RoundedEdges = False

        ' Set DockPanel properties
        DockPanel.ActiveAutoHideContent = Nothing
        DockPanel.Parent = Me        
        VS2005Style.Extender.SetSchema(DockPanel, VS2005Style.Extender.Schema.FromBase)

        DockPanel.SuspendLayout(True)
        If System.IO.File.Exists(configFile) Then
            DockPanel.LoadFromXml(configFile, AddressOf ReloadContent)
        Else
            ' Load a basic layout
            CreateBasicLayout()
        End If
        DockPanel.ResumeLayout(True, True)

        ' Create a document
        ShowNewForm(sender, e)
    End Sub

    Private Sub CreateBasicLayout()
        SolutionExplorer.Show(DockPanel, WeifenLuo.WinFormsUI.DockState.DockLeft)
        PropertiesWindow.Show(DockPanel, WeifenLuo.WinFormsUI.DockState.DockLeft)
    End Sub

    Private Sub frmMain_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Dim configFile As String = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config")
        DockPanel.SaveAsXml(configFile)

        Do While DockPanel.Contents.Count > 0
            Dim dc As DockContent = DockPanel.Contents(0)
            dc.Close()
        Loop
    End Sub

    Private Function ReloadContent(ByVal persistString As String) As IDockContent

        Select Case persistString

            Case "frmDocument"
                Return Nothing

            Case "frmSolution"

                SolutionExplorer = New frmSolution
                Return SolutionExplorer

            Case "frmProperty"

                PropertiesWindow = New frmProperty
                Return PropertiesWindow

        End Select

        Return Nothing

    End Function

    Private Sub ShowNewForm(ByVal sender As Object, ByVal e As EventArgs) Handles NewToolStripMenuItem.Click, NewToolStripButton.Click, NewWindowToolStripMenuItem.Click
        ' Create a new instance of the child form.
        Dim ChildForm As New frmDocument
        ' Make it a child of this MDI form before showing it.
        Select Case m_ChildFormNumber
            Case 0
                ChildForm.TabText = Filename(m_ChildFormNumber)
                ChildForm.ToolStrip1.Visible = True
                ChildForm.DocumentText.Text = "This is a sample on how to customize the DockPanel Suite. Please refer to the component documentation and to the related web site for any question on it or on it's usage."

            Case 1, 2, 3, 4, 5, 6, 7
                ChildForm.TabText = Filename(m_ChildFormNumber)

            Case Else
                ChildForm.TabText = "Class" & m_ChildFormNumber - 7 & ".vb"
        End Select
        m_ChildFormNumber += 1

        ChildForm.Text = ChildForm.TabText
        ChildForm.Show(DockPanel)
    End Sub

    Private Sub OpenFile(ByVal sender As Object, ByVal e As EventArgs) Handles OpenToolStripMenuItem.Click, OpenToolStripButton.Click
        Dim OpenFileDialog As New OpenFileDialog
        OpenFileDialog.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
        OpenFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
        If (OpenFileDialog.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK) Then
            Dim FileName As String = OpenFileDialog.FileName
            Dim ChildForm As New frmDocument
            ChildForm.DocumentText.LoadFile(FileName, RichTextBoxStreamType.PlainText)
            ChildForm.TabText = FileName
            m_ChildFormNumber += 1
            ChildForm.Text = ChildForm.TabText
            ChildForm.Show(DockPanel)
        End If
    End Sub

    Private Sub SaveAsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SaveAsToolStripMenuItem.Click
        Dim SaveFileDialog As New SaveFileDialog
        SaveFileDialog.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
        SaveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"

        If (SaveFileDialog.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK) Then
            Dim FileName As String = SaveFileDialog.FileName
            ' TODO: Add code here to save the current contents of the form to a file.
        End If
    End Sub

    Private Sub ExitToolsStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub CutToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CutToolStripMenuItem.Click
        ' Use My.Computer.Clipboard to insert the selected text or images into the clipboard
    End Sub

    Private Sub CopyToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CopyToolStripMenuItem.Click
        ' Use My.Computer.Clipboard to insert the selected text or images into the clipboard
    End Sub

    Private Sub PasteToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles PasteToolStripMenuItem.Click
        'Use My.Computer.Clipboard.GetText() or My.Computer.Clipboard.GetData to retrieve information from the clipboard.
    End Sub

    Private Sub ToolBarToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ToolBarToolStripMenuItem.Click
        Me.ToolStrip.Visible = Me.ToolBarToolStripMenuItem.Checked
    End Sub

    Private Sub StatusBarToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles StatusBarToolStripMenuItem.Click
        Me.StatusStrip.Visible = Me.StatusBarToolStripMenuItem.Checked
    End Sub

    Private Sub CloseAllToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CloseAllToolStripMenuItem.Click
        ' Close all child forms of the parent.
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next
    End Sub

End Class
