function ImageProducedHandler(sender, args)
%UNTITLED Summary of this function goes here
%   Detailed explanation goes here 
  disp('Image Received')
  Images=args.Images;
  PixelBright = double( Images(1).GetCenterPoint());
  disp(PixelBright)
  %You must allow Micromanager_net to update the controls,
  %this is the command
  args.Helpers.DoNetEvents();
  
  %If you wish to manipulate the microscope in response to the image
  %there is a handy reference to EasyCore stored in the args
  %camera = args.ECore.GetDevice('çamera');
  %Convert image into array for use by mathematica
    
  MMImageArray = Images(1).GetArrayDouble();

  MLImageArray = double(MMImageArray);
  %Now display the image as a array graph.  This is a slow operation and only show for a tutorial
  image(MLImageArray);
  colormap(gray);
  figure(gcf);
  
end

