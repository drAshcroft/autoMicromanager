try
  MM = NET.addAssembly('C:\Program Files\Micromanager.NET\Micromanager_net.dll');
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

%Start the Micromanager Framework
ECore = NIEcore.StartECoreScriptGui('C:\\Program Files\\Micromanager.NET\\ConfigFiles\\Test4.xml');

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
%The helpers object has all the information for starting devices as well as methods to perform casts
Helpers = NIEcore.GetHelpers();


%Check what devices were loaded with the configuration file
LDevices= ECore.GetLoadedDeviceNames();
aP=[];
for j=1:LDevices.Length, 
 a=cellstr(char(LDevices(j)));
 aP=[aP,a];
end 
aP

%Grab the camera device from the configuation file. We will look at the loaded devices to get the name,  you can also look at the 
%configuration file
Camera = ECore.GetDevice('camera');

%Check what properties the camera has
propnames= Camera.GetDevicePropertyNames();
aP=[];
for j=1:propnames.Length, 
 a=cellstr(char(propnames(j)));
 aP=[aP,a];
end 
aP


%Now that you have the camera devices, you can adjust its properties

Camera.SetDeviceProperty('Exposure', '100')


ImageProcessor = CoreDevices.NI_Controls.Image_Processor();
ImageProcessor.Helpers=Helpers;
ImageProcessor.RegisterAsImageProcessor(ECore, 'Matlab', false);


addlistener( ImageProcessor, 'MatLabImageProduced', @ImageProducedHandler)

%If you wish to test your event handler without having to run the 
%program you can use the following two commands to trigger the event

ImageN=Camera.SnapOneFrame(false);
ImageProcessor.TestEvent(ImageN)

