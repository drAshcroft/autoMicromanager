VERSION 5.00
Begin VB.Form StartForm 
   Caption         =   "COM Tutorials"
   ClientHeight    =   7710
   ClientLeft      =   60
   ClientTop       =   450
   ClientWidth     =   7725
   LinkTopic       =   "Form1"
   ScaleHeight     =   7710
   ScaleWidth      =   7725
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton Command3 
      Caption         =   "JavaScript Example"
      Height          =   1575
      Left            =   1080
      TabIndex        =   2
      Top             =   5280
      Width           =   5775
   End
   Begin VB.CommandButton Command2 
      Caption         =   "Micromanager Picture Display"
      Height          =   1575
      Left            =   1080
      TabIndex        =   1
      Top             =   3000
      Width           =   5775
   End
   Begin VB.CommandButton Command1 
      Caption         =   "VB Picture Display"
      Height          =   1575
      Left            =   1080
      TabIndex        =   0
      Top             =   840
      Width           =   5775
   End
End
Attribute VB_Name = "StartForm"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub Command1_Click()
  Form_NativeImage.Show
End Sub

Private Sub Command2_Click()
 Form_MMPictureBoard.Show
End Sub

Private Sub Command3_Click()
ScriptForm.Show
End Sub

