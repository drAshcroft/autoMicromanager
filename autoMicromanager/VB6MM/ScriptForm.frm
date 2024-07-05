VERSION 5.00
Object = "{3B7C8863-D78F-101B-B9B5-04021C009402}#1.2#0"; "RICHTX32.OCX"
Object = "{0E59F1D2-1FBE-11D0-8FF2-00A0D10038BC}#1.0#0"; "msscript.ocx"
Begin VB.Form ScriptForm 
   Caption         =   "Form1"
   ClientHeight    =   11760
   ClientLeft      =   60
   ClientTop       =   450
   ClientWidth     =   12435
   LinkTopic       =   "Form1"
   ScaleHeight     =   11760
   ScaleWidth      =   12435
   StartUpPosition =   3  'Windows Default
   Begin MSScriptControlCtl.ScriptControl ScriptControl1 
      Left            =   10800
      Top             =   2880
      _ExtentX        =   1005
      _ExtentY        =   1005
   End
   Begin VB.CommandButton Command1 
      Caption         =   "Run"
      Height          =   855
      Left            =   10200
      TabIndex        =   1
      Top             =   240
      Width           =   1815
   End
   Begin RichTextLib.RichTextBox RichTextBox1 
      Height          =   11295
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   9855
      _ExtentX        =   17383
      _ExtentY        =   19923
      _Version        =   393217
      Enabled         =   -1  'True
      TextRTF         =   $"ScriptForm.frx":0000
   End
End
Attribute VB_Name = "ScriptForm"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub Command1_Click()
  Call Me.ScriptControl1.ExecuteStatement(Me.RichTextBox1.Text)
End Sub

Private Sub Form_Load()
 
 Dim s As String
 s = "Set MMtoolkit = CreateObject(""Micromanager_net.MMToolKitComService"") " + vbCrLf + _
     "Set eCore = MMtoolkit.StartEcore("""")" + vbCrLf + _
     "Set helpers = MMtoolkit.GetHelpers() " + vbCrLf + _
     "Set camera = helpers.CreateCameraDevice(""camera"", ""DemoStreamingCamera"", ""DStreamCam"") " + vbCrLf + _
     "Call camera.SetDeviceProperty(""Exposure"", ""100"") " + vbCrLf + _
     "Set ComViewer = CreateObject(""Micromanager_net.MMCOMViewer"") " + vbCrLf + _
     "ComViewer.Show " + vbCrLf + _
     "For i = 0 To 20" + vbCrLf + _
      "    Set pic = camera.SnapOneFrame(False)" + vbCrLf + _
      "    ComViewer.ShowImage pic" + vbCrLf + _
      "Next"
 Me.RichTextBox1.Text = s
End Sub
