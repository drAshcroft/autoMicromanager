msgbox("This script is really slow, so it may take up to a minute to load.  This is intended as an example for a COM client program")
Set MMtoolkit = CreateObject("Micromanager_net.MMToolKitComService") 
Set eCore = MMtoolkit.StartEcore("")
Set helpers = MMtoolkit.GetHelpers() 
Set camera = helpers.CreateCameraDevice("camera", "DemoStreamingCamera", "DStreamCam") 
Call camera.SetDeviceProperty("Exposure", "100") 
Set ComViewer = CreateObject("Micromanager_net.MMCOMViewer") 
ComViewer.Show 
For i = 0 To 20
    Set pic = camera.SnapOneFrame(False)
    ComViewer.ShowImage pic
Next