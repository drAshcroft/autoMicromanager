using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using CoreDevices;
using System.Drawing.Imaging;
using System.Runtime.Serialization.Formatters.Binary;

namespace TestBed2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private bool ButtonPushed = false;
        private void Form1_Load(object sender, EventArgs e)
        {
            Ecore = new EasyCore("");
            Micromanager_net.Setup.HardwareSetup2 hs = new Micromanager_net.Setup.HardwareSetup2(Ecore, null,false );
            hs.ShowDialog();
            //Ecore = niEasyCore1.StartEcore(@"C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\csharp_core 2.1.1\MM_Net\bin\Debug\ConfigFiles\Test4.xml");
            //Ecore = niEasyCore1.StartEcore(@"");
          //  Ecore = niEasyCore1.StartEcore(@"C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\csharp_core 2.1.1\LabViewMM\Test3.xml");
            //niEasyCore1.NewHardwareConfig(Path.GetDirectoryName( Application.ExecutablePath), "test.xml");
            allDeviceHolders1.DisplayGUIs(niEasyCore1);
            Ecore.PaintSurface(pictureBoard1);
            //Stream stream = File.Open("test.osl", FileMode.Create);
            //BinaryFormatter bformatter = new BinaryFormatter();

            //Console.WriteLine("Writing Employee Information");
            //bformatter.Serialize(stream, Ecore);
            //stream.Close();

            //scriptControl1.AddObject("EasyCore", Ecore);
           // ironPythonScriptModule1.SetCore(Ecore, "IronTest");
            //CoreDevices.Devices.NormalCamera nc = (CoreDevices.Devices.NormalCamera)Ecore.GetDevice("camera");
            

        }

        private int CWidth = 500;
        private int CHeight = 100;

        private double dt ;//= .000001;
        private double TimeDilution = 100;
        private double[,] Concentration;
        private double[,] Delta;
        private Int16 [,] Display; 
        private Random rnd = new Random();
        private double DiffusionC = .0099;//    um^2/2
        private double VAverage = 5000;//  um/s 
        private double VMax ;//  um/s  Calculated later
        private Double MaxConcentration =100 * 100 * 1e9;// Particles per liter
        private double FluxArea = 100;// um ^2

        private double ConcentrationMax = 100000;//dont mess with this
        double GlobalTime = 0;
        private double D_Horizontal(double  Y)
        {
            double d=2/(double)CHeight *((double)CHeight / 2 - Y);
            double DD = VMax*dt  * (1 -  d * d);
            return DD;
        }
        private void AddDelta()
        {

            double  Max = double .MinValue;
            double  Min = double.MaxValue;
            for (int i = 1; i < CWidth-1; i++)
            {
                for (int j = 1; j < CHeight-1; j++)
                {
                    Concentration[i,j]+=Delta[i,j];
                    
                    Delta[i, j] = 0;// Concentration[i, j];
                    if (Concentration[i, j] > Max) Max = Concentration[i, j];
                    if (Concentration[i, j] < Min) Min = Concentration[i, j];
                }
            }
            /*for (int i = 0; i < CWidth ; i++)
            {
                for (int j = 0; j < CHeight ; j++)
                {
                    double b = 1000.0d * Concentration[i, j] / 10.0d;
                    Display[i, j] = (Int16)(b);
                }
            }*/

           
        
          

        }
        #region Moves
        private void MoveUp()
        {
            for (int i = 0; i < CWidth; i++)
            {
                for (int j = 1; j < CHeight; j++)
                {
                    double diff = Concentration[i, j] - Concentration[i, j - 1];//  dc/dy  (C/um)
                    if (diff > 0)
                    {
                        Delta[i, j - 1] += diff * DiffusionC*dt ;
                        Delta[i, j] -= diff * DiffusionC * dt ;
                    }
                }
            }
        }
        private void MoveDown()
        {
            for (int i = 0; i < CWidth; i++)
            {
                for (int j = 0; j < CHeight - 1; j++)
                {
                    double diff = Concentration[i, j] - Concentration[i, j + 1];
                    if (diff > 0)
                    {
                        Delta[i, j + 1] += diff * DiffusionC * dt ;
                        Delta[i, j] -= diff * DiffusionC * dt ;
                    }
                }
            }
        }
        private void MoveOver()
        {
            for (int i = 0; i < CWidth - 1; i++)
            {
                for (int j = 0; j < CHeight; j++)
                {
                    double diff = Concentration[i, j] - Concentration[i + 1, j];
                    if (diff > 0)
                    {
                        Delta[i + 1, j] += diff * DiffusionC*dt ;// D_Horizontal(j);
                        Delta[i, j] -= diff * DiffusionC * dt ;// D_Horizontal(j);
                    }
                }
            }
        }
        private void MoveBack()
        {
            for (int i = 1; i < CWidth; i++)
            {
                for (int j = 0; j < CHeight - 1; j++)
                {
                    double diff = Concentration[i, j] - Concentration[i - 1, j];
                    if (diff > 0)
                    {
                        Delta[i - 1, j] += diff * DiffusionC*dt ;// D_Horizontal(j);
                        Delta[i, j] -= diff * DiffusionC*dt;// D_Horizontal(j);
                    }
                }
            }
        }
        #endregion
        private void MassTransport()
        {
            for (int j = 0; j < CHeight; j++)
            {
                FluxCount[j] += D_Horizontal(j);
                if (FluxCount[j] >= 1)
                {
                    for (int i = CWidth - 2; i >= 0; i--)
                    {
                        //double diff = D_Horizontal(j);
                        Delta[i + 1, j] +=  Concentration[i, j];
                        Delta[i, j] -=  Concentration[i, j];
                    }
                    FluxCount[j] = 0;
                }
            }

        }
        private Double[] ParticleCount;
        private bool SimulationDone = false;
        private void Flux()
        {
            double[] FluxD = new double[CWidth];
            double Max = 0;
            int[] Profile = new int[CWidth];
            double ParticlesPerCell = 1000* MaxConcentration / 1e18 / ConcentrationMax; // convert from N/Liter to N/um3  per grid unit
            for (int i = 0; i < CWidth; i++)
            {
                FluxD[i] =ParticlesPerCell * FluxArea * Math.Abs(Concentration[i, CHeight - 1] - Concentration[i, CHeight - 3])*dt ;  //particles per second through fluxarea um^2 area
                if (FluxD[i] > Max) Max = FluxD[i];
                ParticleCount[i] += FluxD[i];
                if (ParticleCount[i] >1 )
                {
                    Concentration[i, CHeight - 1] = Concentration[i, CHeight - 2];

                    if (ParticleCount[i] < 30)
                    {
                        System.Diagnostics.Debug.Print(GlobalTime.ToString() + " \t " + i.ToString()); 

                    }
                    if (i >= (CWidth * .75))
                        SimulationDone = true;
                    ParticleCount[i] = 30;
                    
                }
            }

            for (int j = 0; j < CHeight; j++)
            {
                Profile[j] = (int)(FluxD [ j]/Max *1000);
            }
            try
            {
                lutGraph1.ShowGraph(Profile, 1000, 0, 1000, 0);
            }
            catch { }
            label1.Text = (Max/dt).ToString() + " particles per second";
        }
        private double[] FluxCount;
        private void button1_Diffusion(object sender, EventArgs e)
        {
            VMax = 3.0d/2.0d*VAverage;
            dt = 1/VMax/TimeDilution ;
            Concentration = new double[CWidth, CHeight];
            Display = new Int16 [CWidth, CHeight];
            Delta = new double[CWidth, CHeight];
            FluxCount = new double[CHeight];
            ParticleCount = new double[CWidth];

            for (int i = 0; i < CHeight; i++)
            {
                Concentration[0, i] = ConcentrationMax;
              
            }
           
            for (int Count1 = 0;SimulationDone==false ; Count1++)
            {
              
                int p = rnd.Next(4);
                if (true  )
                {
                    MoveUp();
                    MoveDown();
                    MoveOver();
                    MoveBack();
                }

                AddDelta();
                MassTransport();
                AddDelta();
                for (int i = 0; i < CWidth; i++)
                {
                    Concentration[i, 0] =  Concentration[i, 1];
                    //Concentration[i, CHeight - 1] = 0;// Concentration[i, CHeight - 2];
                }
                Flux();
                Application.DoEvents();
                GlobalTime += dt;
            }
        }

        public static Bitmap CreateGrayScaleBitmap(double [,] Data,  int Width, int Height, double  MinValue, double MaxValue, AllowedPixelFormats OutPutFormat, Color FalseColor)
        {
            //int Stride = (int)NumBytes / Height;
            Bitmap result;
            result = new Bitmap(Width, Height, PixelFormat.Format32bppRgb);

            BitmapData outdata = result.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, result.PixelFormat);
            
           
            double ValueLength = (MaxValue - MinValue);
            int PixelSize = 4;
           
                unsafe
                {
                    for (int y = 0; y < Height; y++)
                    {
                       
                        byte* row = (byte*)outdata.Scan0 + (y * outdata.Stride);

                            for (int x = 0; x < Width; x++)
                            {
                                long gray = (long)(Data[x,y]);

                                gray = (long)((gray - MinValue) / ValueLength * 256);

                                if (gray < 0) gray = 0;
                                else if (gray > 255) gray = 255;
                                byte v = (byte)gray;
                                row[x * PixelSize] = (byte)( v);
                                row[x * PixelSize + 1] = (byte)( v);
                                row[x * PixelSize + 2] = (byte)( v);

                            }
                        }
                    }
                
            // Unlock the bitmap
            result.UnlockBits(outdata);
            return result;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CoreImage c= ((CoreDevices.Devices.NormalCamera)Ecore.GetDevice("camera")).SnapOneFrame(true );
           // CoreImage[] c2=   ironPythonScriptModule1.getImage(new CoreImage[] { c });

        }

        EasyCore Ecore;
        private void button1_Click(object sender, EventArgs e)
        {
            Ecore = niEasyCore1.StartEcore(@"C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\csharp_core 2.0.1\MM_Net\bin\Debug\ConfigFiles\Test4.xml");
            
             //Stream stream = File.Open("test.osl", FileMode.Create);
             //BinaryFormatter bformatter = new BinaryFormatter();

             //Console.WriteLine("Writing Employee Information");
             //bformatter.Serialize(stream, Ecore);
             //stream.Close();

           //scriptControl1.AddObject("EasyCore", Ecore);
            //ironPythonScriptModule1.SetCore(Ecore, "IronTest");
            CoreDevices.Devices.NormalCamera nc = (CoreDevices.Devices.NormalCamera)Ecore.GetDevice("camera");
         
           // scriptControl1.AddRefrence("CoreDevices");
        }
    }
}
