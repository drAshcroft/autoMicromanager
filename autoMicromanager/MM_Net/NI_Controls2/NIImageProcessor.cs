using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace CoreDevices.NI_Controls
{
    [Serializable]
    public partial class NIImageProcessor : UserControl
    {
        public NIImageProcessor()
        {
            InitializeComponent();
        }
        public Image_Processor RegisterAsImageProcessor(CoreDevices.EasyCore EasyCore, string ProcessorName, bool Blocking)
        {
            Image_Processor ip = new Image_Processor();
            ip.RegisterAsImageProcessor(EasyCore, ProcessorName, Blocking);
            return ip;
        }
    }
    public class Image_Processor
    {
        public event NIImageProducedEvent ImageProduced;
        public delegate void MatLabImageProducedEvent(object sender, IPEventArg  e);
        public event MatLabImageProducedEvent MatLabImageProduced;

        private string ProcessorName;
        private EasyCore ECore;
        bool Blocking = false;
        public void RegisterAsImageProcessor(CoreDevices.EasyCore EasyCore, string ProcessorName, bool Blocking)
        {
            this.Blocking = Blocking;
            this.ProcessorName = ProcessorName;
            EasyCore.AddImageProcessor(ProcessorName, new ImageProcessorStep( ReportImage ));
        }

        Queue<CoreImage[]> FiFo = new Queue<CoreImage[]>();


        /// <summary>
        /// These are used for the blocking image processing to send the image out, and then return the processed image and maintain the block
        /// </summary>
        bool ImageConsumed = false;
        CoreImage[] CurrentImage;
        CoreImage[] ReturnedImage = null;

        bool Inactive = false;
        public bool InActive
        {
            get { return Inactive; }
            set { Inactive = value; }
        }
        public void UnregisterImageProcessor()
        {
            ECore.RemoveImageProcessor(ProcessorName);
        }
        public void TestEvent(CoreImage ImageIn)
        {
            if (ImageProduced != null)
            {
                List<CoreImage> c = new List<CoreImage>();
                try
                {
                    c.Add(ImageIn);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                ImageProduced(c);
            }
            else if (MatLabImageProduced != null)
            {
                if (_helpers !=null)
                    MatLabImageProduced(this,new IPEventArg( new CoreImage[] { ImageIn},ECore , (NiHelpers ) _helpers  ) );
                else
                    MatLabImageProduced(this, new IPEventArg(new CoreImage[] { ImageIn }, ECore, null ));
            }
           
        }
        private object _helpers = null;
        public object Helpers
        {
            get { return _helpers; }
            set { _helpers = value; }
        }
        public CoreImage[] ReportImage(CoreImage[] ImageIn)
        {
            //MessageBox.Show("REport Image");
            if (Inactive == true)
            {
                //MessageBox.Show("Inactive");
                return ImageIn;
            }
            else
            {
                //MessageBox.Show("Other");
                if (ImageProduced != null)
                {
                    
                    //MessageBox.Show("ImageProduced");
                    List<CoreImage> c = new List<CoreImage>(ImageIn );
                    ImageProduced(c);
                    //MessageBox.Show("Handled");
                    return c.ToArray();
                }
                else if (MatLabImageProduced != null)
                {

                    if (_helpers != null)
                        MatLabImageProduced(this, new IPEventArg(ImageIn , ECore, (NiHelpers)_helpers));
                    else
                        MatLabImageProduced(this, new IPEventArg(ImageIn , ECore, null));
                    return ImageIn;
                } 
                else
                {
                   
                   // return ImageIn;
                    if (Blocking == true)
                    {
                        try
                        {
                            //   MessageBox.Show("Blocking");
                            // if (CurrentImage ==null) MessageBox.Show("ipImageNull");
                            if (ImageIn != null)
                            {
                                //   MessageBox.Show("Blocking2");
                                CurrentImage = ImageIn;
                                ReturnedImage = new CoreImage[CurrentImage.Length];
                                ImageConsumed = false;
                                do
                                {
                                    Application.DoEvents();
                                    Thread.Sleep(5);
                                } while (ImageConsumed == false);
                                CurrentImage = null;
                            }
                            else
                                return ImageIn ;
                            return ReturnedImage;
                        }
                        catch
                        {
                            ImageConsumed = true;
                            return ImageIn;
                        }
                    }
                    else
                    {
                        FiFo.Enqueue(ImageIn);
                        return ImageIn;
                    }
                }
            }
        }
        public bool HasStoredImage()
        {
           // MessageBox.Show("HasImages");
            if (Blocking == true)
                return CurrentImage != null;
            else 
                return ( FiFo.Count > 0);
        }
        public CoreImage[] GetImages()
        {
            if (Blocking == true)
                return CurrentImage;
            else 
                return FiFo.Dequeue();
        }

        public int NumberOfStackImages()
        {
            return CurrentImage.Length;
        }
        public CoreImage GetImage(int StackImageNumber)
        {
           // MessageBox.Show("GetImage");
            return CurrentImage[StackImageNumber];
        }
        public void  PutImage(int StackImageNumber, CoreImage ProcessedImage)
        {
            ReturnedImage[StackImageNumber] = ProcessedImage;
        }
        public void ImagesAreProcessed()
        {
            CurrentImage = null; 
            ImageConsumed = true;
        }
        public void ReturnImage(CoreImage[] ProcessedImage)
        {
            ReturnedImage = ProcessedImage;
        }

        public void ReturnImageNoChange()
        {
            ReturnedImage = CurrentImage;
            ImageConsumed = true;
        }

    }
    public delegate void NIImageProducedEvent(List<CoreImage> InputImage);
    public class IPEventArg : EventArgs
    {
        private CoreImage[] _Images=null;
        private EasyCore eCore = null;
        private NiHelpers _Helpers=null;
        public EasyCore ECore
        {
            get { return eCore; }
            set { eCore = value; }
        }
        public NiHelpers Helpers
        {
            get { return _Helpers; }
            set { _Helpers = value; }
        }
        public CoreImage[] Images
        {
            get { return _Images; }
            set { _Images = value; }
        }
        public IPEventArg(CoreImage[] Images, EasyCore ECore, NiHelpers Helpers)
        {
            _Images = Images;
            _Helpers = Helpers;
            eCore = ECore;
        }
    }
}
