using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Micromanager_net
{
    [Guid("5A88092E-69DF-4bb8-AD8D-8FA83E550F20")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IECoreHelper
    {
        Control GetBaseControl();
        Control GetBaseControl(string CorePathDir, string ConfigFileName);
        Control GetAllDeviceHolder();
        Control GetAllDeviceHolder(Control BaseControl);
        Control GetAllDeviceHolder(COM_BaseControl BaseControl);

        Control GetSingleDeviceHolder();
        COM_BaseControl ConvertControlToBaseControl(Control control);
        AllDeviceHolders ConvertControlToAllDeviceHolder(Control control);
        SingleDeviceHolder ConvertControltoSingleDeviceHolder(Control control);

        void InterceptCameraImages(bool Intercept);
        bool ImageReady();
        CoreDevices.CoreImage GetImage();
        void ReturnImage(CoreDevices.CoreImage coreImage);
    }


    [Guid("1514adf6-7cb1-4561-7Ebb-b75c1467149b")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class ComHelper:IECoreHelper 
    {
        private COM_BaseControl BaseControl;

        public Control GetSingleDeviceHolder()
        {
            return new SingleDeviceHolder();
        }
        public SingleDeviceHolder ConvertControltoSingleDeviceHolder(Control control)
        {
            return (SingleDeviceHolder)control;
        }
        
        public Control GetBaseControl()
        {
           BaseControl = new COM_BaseControl();
           return BaseControl;
        }
        public Control GetBaseControl(string CorePathDir, string ConfigFileName)
        {
            COM_BaseControl b =(COM_BaseControl) GetBaseControl();
            b.StartEcore(CorePathDir, ConfigFileName);
            BaseControl = b;
            return b;
        }
        public Control GetAllDeviceHolder()
        {
            return new AllDeviceHolders();
        }
        public Control GetAllDeviceHolder(Control BaseControl)
        {
            return GetAllDeviceHolder((COM_BaseControl)BaseControl);
        }
        public Control GetAllDeviceHolder(COM_BaseControl BaseControl)
        {
            AllDeviceHolders ad = (AllDeviceHolders)GetAllDeviceHolder();
            ad.DisplayGUIs(BaseControl);
            return ad;
        }
        public COM_BaseControl ConvertControlToBaseControl(Control control)
        {
            return (COM_BaseControl)control;
        }
        public AllDeviceHolders ConvertControlToAllDeviceHolder(Control control)
        {
            return (AllDeviceHolders)control;
        }

        private bool ProcessorRegistered=false ;
        private bool InterceptImages=false ;
        private bool ImageIsReady=false ;
        private bool ImageConsumed = true;
        private CoreDevices.CoreImage cImage=null;
        public void InterceptCameraImages(bool Intercept)
        {
            if (Intercept == true)
            {
                if (ProcessorRegistered == false)
                {
                    BaseControl.EasyCore.AddImageProcessor("ComProcessor", new CoreDevices.ImageProcessorStep(GetImage));
                    ProcessorRegistered = true;
                }
                InterceptImages = true;
            }
            else
            {
                InterceptImages = false;
            }

        }
        private CoreDevices.CoreImage GetImage(CoreDevices.CoreImage coreImage)
        {
            if (InterceptImages == true)
            {
                cImage = coreImage;
                ImageConsumed = false;
                ImageIsReady = true;
                while (ImageConsumed == false && InterceptImages==true )
                {
                    Application.DoEvents();
                }
                return cImage;
            }
            else
                return coreImage;
        }
        public bool ImageReady()
        {
            return ImageIsReady;
        }
        public CoreDevices.CoreImage GetImage()
        {
            return cImage;
        }
        public void ReturnImage(CoreDevices.CoreImage coreImage)
        {
            cImage = coreImage;
            ImageConsumed = true;
        }

    }
}
