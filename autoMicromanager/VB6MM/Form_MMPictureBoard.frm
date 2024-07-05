VERSION 5.00
Object = "{6B7E6392-850A-101B-AFC0-4210102A8DA7}#1.3#0"; "comctl32.ocx"
Object = "{C49DC76D-76CC-48DD-895A-B5AFE4F25CF0}#1.0#0"; "micromanager_net.tlb"
Begin VB.Form Form_MMPictureBoard 
   Caption         =   "Micromanager Picture Display"
   ClientHeight    =   8760
   ClientLeft      =   60
   ClientTop       =   450
   ClientWidth     =   13440
   LinkTopic       =   "Form2"
   ScaleHeight     =   8760
   ScaleWidth      =   13440
   StartUpPosition =   3  'Windows Default
   Begin Micromanager_netCtl.MMCOMpictureBoard MMCOMpictureBoard1 
      Height          =   8415
      Left            =   360
      TabIndex        =   3
      Top             =   240
      Width           =   9735
      Object.Visible         =   "True"
      Enabled         =   "True"
      ForegroundColor =   "-2147483630"
      BackgroundColor =   "-2147483633"
      AutoSizeMode    =   "GrowAndShrink"
      BackColor       =   "Control"
      ForeColor       =   "ControlText"
      Location        =   "24, 16"
      Name            =   "MMToolKitComService"
      Size            =   "649, 561"
      Object.TabIndex        =   "0"
   End
   Begin VB.CommandButton Command1 
      Caption         =   "Take Images"
      Height          =   615
      Left            =   10440
      TabIndex        =   0
      Top             =   1800
      Width           =   1935
   End
   Begin ComctlLib.Slider Slider1 
      Height          =   255
      Left            =   10440
      TabIndex        =   1
      Top             =   720
      Width           =   2415
      _ExtentX        =   4260
      _ExtentY        =   450
      _Version        =   327682
      LargeChange     =   10
      Max             =   100
      TickFrequency   =   10
   End
   Begin VB.Label Label1 
      Caption         =   "Exposure"
      Height          =   375
      Left            =   10440
      TabIndex        =   2
      Top             =   360
      Width           =   1335
   End
End
Attribute VB_Name = "Form_MMPictureBoard"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private MMtoolkit As MMToolKitComService
Private eCore As EasyCore
Private helpers As NiHelpers
Private camera As NormalCamera

Private Sub Command1_Click()
  Dim pic As CoreImage
  For i = 0 To 20
      Set pic = camera.SnapOneFrame(False)
      Call MMCOMpictureBoard1.SendImage(pic)
      DoEvents
  Next
End Sub

Private Sub Form_Load()

 Set MMtoolkit = New MMToolKitComService
 Set eCore = MMtoolkit.StartEcore("")
 Set helpers = MMtoolkit.GetHelpers()
 Set camera = helpers.CreateCameraDevice("camera", "DemoStreamingCamera", "DStreamCam")
 Call camera.SetDeviceProperty("Exposure", "100")

 Slider1.Value = CInt(camera.getExposure())
End Sub

Private Sub Form_Unload(Cancel As Integer)
   End
End Sub

Private Sub Slider1_Click()
 Call camera.setExposure(CDbl(Slider1.Value))
End Sub

