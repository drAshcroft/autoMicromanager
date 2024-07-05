try
  MM= NET.addAssembly('C:\Program Files\Micromanager.NET\Micromanager_net.dll')
catch e
  e.message
  if(isa(e, 'NET.NetException'))
    e.ExceptionObject
  end
end

import CoreDevices.*
import CoreDevices.NI_Controls.*
import CWrapper.*

NIEcore = CoreDevices.NI_Controls.NIEasyCore();
ECore = NIEcore.StartEcore('');


Libs = ECore.GetDeviceLibraries();
aP=[];
for j=1:Libs.Length, 
 a=cellstr(char(Libs(j)));
 aP=[aP,a];
end 
aP


%Check out all the adapters in the libraries
Adapters= ECore.GetAllDeviceAdapters();
aP=[];
for j=1:Adapters.Length, 
 a=cellstr(char(Adapters(j)));
 aP=[aP,a];
end 
aP

%You can start the devices by hand, but it is easier to just use the 
%Helpers class for most the devices, so we request a helpers object
Helpers = NIEcore.GetHelpers();


%Create a default camera from the demo library
Camera = Helpers.CreateCameraDevice('camera', 'DemoStreamingCamera', 'DStreamCam');

%Notify the core that this is the main camera of the system
Camera.MakeOffical();

%Adjust a device property, in this case the exposure.  The property values must always be strings
Camera.SetDeviceProperty('Exposure', '100');

%We acquire one image while indicating that micromanager should not display the image. (There is no viewer attached)
MMImage = Camera.SnapOneFrame(false);

%Convert image into array for use by mathematica
MMImageArray = MMImage.GetArrayDouble();

MLImageArray = double(MMImageArray);
%Now display the image as a array graph.  This is a slow operation and only show for a tutorial
image(MLImageArray);colormap(gray);figure(gcf);