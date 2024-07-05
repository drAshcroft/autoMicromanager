VERSION 5.00
Object = "{6B7E6392-850A-101B-AFC0-4210102A8DA7}#1.3#0"; "comctl32.ocx"
Begin VB.Form Form_NativeImage 
   Caption         =   "VB Picture Display"
   ClientHeight    =   8925
   ClientLeft      =   60
   ClientTop       =   510
   ClientWidth     =   11805
   LinkTopic       =   "Form1"
   ScaleHeight     =   595
   ScaleMode       =   3  'Pixel
   ScaleWidth      =   787
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton Command1 
      Caption         =   "Take Images"
      Height          =   615
      Left            =   9240
      TabIndex        =   3
      Top             =   3120
      Width           =   1935
   End
   Begin ComctlLib.Slider Slider1 
      Height          =   255
      Left            =   9240
      TabIndex        =   1
      Top             =   2040
      Width           =   2415
      _ExtentX        =   4260
      _ExtentY        =   450
      _Version        =   327682
      LargeChange     =   10
      Max             =   100
      TickFrequency   =   10
   End
   Begin VB.PictureBox Picture1 
      AutoRedraw      =   -1  'True
      Height          =   8655
      Left            =   120
      ScaleHeight     =   573
      ScaleMode       =   3  'Pixel
      ScaleWidth      =   589
      TabIndex        =   0
      Top             =   120
      Width           =   8895
   End
   Begin VB.Label Label1 
      Caption         =   "Exposure"
      Height          =   375
      Left            =   9240
      TabIndex        =   2
      Top             =   1680
      Width           =   1335
   End
End
Attribute VB_Name = "Form_NativeImage"
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
      Call ConvertCoreImageToStdPicture(pic, Picture1)
      DoEvents
  Next
End Sub

Private Sub Form_Load()
 Picture1.ScaleMode = 3
 Picture1.AutoRedraw = True
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
