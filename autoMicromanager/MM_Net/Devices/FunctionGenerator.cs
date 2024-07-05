// DESCRIPTION:   
//                 
//                
// AUTHOR:        Brian Ashcroft, Ashcroft@physics.leidenuniv.nl, 05/27/2009
//
// COPYRIGHT:     Brian Ashcroft
// LICENSE:       This file is distributed under the  MIT license.
//                License text is included with the source distribution.
//
//                This file is distributed in the hope that it will be useful,
//                but WITHOUT ANY WARRANTY; without even the implied warranty
//                of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
//
//                IN NO EVENT SHALL THE COPYRIGHT OWNER OR
//                CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
//                INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using CWrapper;

namespace CoreDevices.Devices
{
    [Guid("1514adf6-7cb1-0032-7Ebb-b75c1467149b")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Serializable]
    public class FunctionGenerator:MMDeviceBase,IFunctionGeneratorCom  
    {
        private List<double[]> Buffers=new List<double[]>();
        private long _UpdateFreq=1000;//todo::this needs to be pulled from the card
        public override void StopDevice()
        {
            StopGenerating();
        }

        public FunctionGenerator(EasyCore ECore,string DeviceLabel,string LibraryName,string DeviceAdapter )
        {
            SetCore(ECore, DeviceLabel, LibraryName, DeviceAdapter);
        }
        public FunctionGenerator(EasyCore Ecore, string DeviceLabel)
        {
            DeviceName = DeviceLabel;
            SetCore(Ecore);
        }
        public override void MakeOffical()
        {
            ECore.MMFunctionGenerator = this;
        }
        
      

        public enum Waveforms
        {
            Gaussian_Noise,
            Pseudo_Noise,
            MersenneTwister,
            Sine,
            Cosine,
            Triangle,
            Square,
            SawTooth,
            SingleValue,
            Formula,
            UserDefined
        }

        static Random RandomNumberSeeder = new Random((int)DateTime.Now.Millisecond);

        /// <summary>
        /// This will build a waveform from the requested parameters
        /// </summary>
        /// <param name="WaveFormType">A waveform taken from the enum Waveforms</param>
        /// <param name="FrequencyWave">Desired frequency determined by the waveform type</param>
        /// <param name="Amplitude"></param>
        /// <param name="Phase"></param>
        /// <param name="Offset">if the waveform should be offset upwards from the axis</param>
        /// <param name="OutputTime">The total amount of time that the waveform should be generated(longer time/ more datapoints)</param>
        /// <param name="Formula">not yet implemented.  To be used with the Formula enum </param>
        /// <returns></returns>
        public double[] BuildBuffer(Waveforms WaveFormType, double FrequencyWave, double Amplitude, double Phase, double Offset, double OutputTime,  string Formula)
        {
            long NumPoints =(long)( OutputTime * (double)_UpdateFreq);
            double[] Buffer=new double[NumPoints  ];
            if (WaveFormType == Waveforms.Pseudo_Noise )
            {
                Random r;
                if (Phase ==-1)
                    r=new Random((int)( RandomNumberSeeder.NextDouble()*1000));
                else 
                    r=new Random();

                //MersenneTwister r = new MersenneTwister((uint)DateTime.Now.Millisecond );
                for (int i = 0; i < NumPoints; i++)
                {
                    Buffer[i] = (.5 - r.NextDouble()) * 2 * Amplitude + Offset ;
                }
            }
            else if (WaveFormType == Waveforms.SingleValue)
            {
                for (int i = 0; i < NumPoints; i++)
                {
                    Buffer[i] = (float)Amplitude;
                }

            }
            else if (WaveFormType == Waveforms.Gaussian_Noise)
            {
                Random r;
                if (Phase == -1)
                    r = new Random((int)(RandomNumberSeeder.NextDouble() * 1000));
                else
                    r = new Random();

                for (int i = 0; i < NumPoints; i++)
                {
                    Buffer[i] = (float)Micromanager_net.Generals.StatisticFunction.NORMINV((float)r.Next(int.MaxValue) / int.MaxValue, 0.0, 0.4) * Amplitude + Offset;
                }
            }
            else if (WaveFormType == Waveforms.MersenneTwister)
            {
                Generals.MersenneTwister r;
                if (Phase == -1)
                    r = new Generals.MersenneTwister((uint)(RandomNumberSeeder.NextDouble() * 1000));
                else if (Phase == 0)
                    r = new Generals.MersenneTwister(1);
                else
                    r = new Generals.MersenneTwister((uint)Phase);

                for (int i = 0; i < NumPoints; i++)
                {
                    Buffer[i] = (.5 - r.NextDouble()) * 2 * Amplitude + Offset;
                }
            }
            else if (WaveFormType == Waveforms.Sine)
            {
                double dt = 1 / (double)_UpdateFreq;
                for (int i = 0; i < NumPoints; i++)
                {
                    Buffer[i] = Amplitude * Math.Sin(i * dt * Math.PI * 2 * FrequencyWave + Phase) + Offset;
                }
            }
            else if (WaveFormType == Waveforms.Cosine)
            {
                double dt = 1 / (double)_UpdateFreq;
                for (int i = 0; i < NumPoints; i++)
                {
                    Buffer[i] = Amplitude * Math.Cos(i * dt * Math.PI * 2 * FrequencyWave + Phase) + Offset;
                }
            }
            else if (WaveFormType == Waveforms.Triangle)
            {
                double dt = FrequencyWave / (double)_UpdateFreq;
                for (int i = 0; i < NumPoints; i++)
                {
                    double t = dt * 2 * i + Phase;
                    Buffer[i] = Amplitude * (1f - 4f * (float)Math.Abs(Math.Round(t - 0.25f) - (t - 0.25f))) + Offset;
                }
            }
            else if (WaveFormType == Waveforms.Square)
            {
                double dt = 1 / (double)_UpdateFreq;
                for (int i = 0; i < NumPoints; i++)
                {
                    double ind = Math.Sin(i * dt * Math.PI * 2 * FrequencyWave + Phase);
                    if (ind > 0)
                    {
                        Buffer[i] = Amplitude + Offset;
                    }
                    else
                    {
                        Buffer[i] = Amplitude * -1 + Offset;
                    }
                }
            }
            else if (WaveFormType == Waveforms.SawTooth)
            {
                double dt = FrequencyWave / (double)_UpdateFreq;
                for (int i = 0; i < NumPoints; i++)
                {
                    Buffer[i] = 2f * (dt * i - (float)Math.Floor(dt * i + 0.5f));
                }
            }
           
            
            //Formula,
            //UserDefined
            return Buffer;

        }

        /// <summary>
        /// Clears out the saved channel data. Does not affect the DAQ
        /// </summary>
        public void ClearAllChannelData()
        {
            Buffers.Clear();
        }

        /// <summary>
        /// Adds information to the channel buffer.  Data is not yet sent to the DAQ
        /// </summary>
        /// <param name="Channel"></param>
        /// <param name="Data"></param>
        public void AddChannelData(int Channel, double[] Data)
        {
            if (Channel < Buffers.Count)
            {
                Buffers.RemoveAt(Channel);
                Buffers.Insert(Channel, Data);
            }
            else
                Buffers.Add(Data);

        }
        
        /// <summary>
        /// Removes the specified channel from the buffer.  Does not affect the DAQ
        /// </summary>
        /// <param name="Channel"></param>
        public void RemoveChannelData(int Channel)
        {
            Buffers.RemoveAt(Channel);
        }

        /// <summary>
        /// Take all the information in the buffers and send it to the DAQ
        /// </summary>
        public void StartGenerating()
        {
            SetDeviceProperty("Output Update Frequency", _UpdateFreq.ToString() );
            long nPoints =0;
            long i = 0;
            foreach (double[] b in Buffers)
            {
                nPoints += b.Length;
            }
            double[] FullBuffer = new double[nPoints];
            foreach (double[] b in Buffers)
            {
                for (int j = 0; j < b.Length; j++)
                {
                    FullBuffer[i] = b[j];
                    i++;
                }
            }
            core.SetSignalIOValue(FullBuffer,FullBuffer.Length  , DeviceName);
        }
        
        /// <summary>
        /// Stops signal generation and sets DAQ to 0
        /// </summary>
        public void StopGenerating()
        {
            long nPoints = 0;
            long i = 0;
            foreach (double[] b in Buffers)
            {
                nPoints += b.Length;
            }
            double[] FullBuffer = new double[nPoints];
            foreach (double[] b in Buffers)
            {
                for (int j = 0; j < b.Length; j++)
                {
                    FullBuffer[i] = 0;
                    i++;
                }
            }
            core.SetSignalIOValue(FullBuffer,FullBuffer.Length , DeviceName);
        }

        /// <summary>
        /// Stops the signal generation and sets DAQ to desired value
        /// </summary>
        /// <param name="EndValue">Desired constant output of DAQ</param>
        public void StopGenerating(double EndValue)
        {
            long nPoints = 0;
            long i = 0;
            foreach (double[] b in Buffers)
            {
                nPoints += b.Length;
            }
            double[] FullBuffer = new double[nPoints];
            foreach (double[] b in Buffers)
            {
                for (int j = 0; j < b.Length; j++)
                {
                    FullBuffer[i] = EndValue ;
                    i++;
                }
            }
            core.SetSignalIOValue(FullBuffer, FullBuffer.Length, DeviceName);
        }

    }

    [Guid("5A88092E-69DF-0032-AD8D-8FA83E550F20")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IFunctionGeneratorCom
    {
        void AddChannelData(int Channel, double[] Data);
        double[] BuildBuffer(FunctionGenerator.Waveforms WaveFormType, double FrequencyWave, double Amplitude, double Phase, double Offset, double OutputTime, string Formula);
        void ClearAllChannelData();
        
        void RemoveChannelData(int Channel);
        void StartGenerating();
       
        void StopGenerating();

        //MMDeviceBase Stuff
        string[] SaveableProperties
        {
            get;
            set;
        }
        bool PropertyIsSaveable(string PropName);
        string GuiPersistenceProperties
        {
            get;
            set;
        }

        void RunCommand(string MethodName, object[] Pars);


        EasyCore Ecore
        {
            get;
        }
        string deviceName
        {
            get;
            set;
        }
        void MakeOffical();
        void SetCore(EasyCore ECore, string DeviceName, string LibraryName, string DeviceAdapter);
        void SetCore(EasyCore ECore);
        [ComVisible(false)]
        Dictionary<string, PropertyInfo> GetAllDeviceProperties();
        PropertyInfo GetDevicePropertyInfo(string PropName);
        string[] GetDevicePropertyNames();
        [ComVisible(false)]
        Dictionary<string, string> GetDevicePropertyValues();
        string GetDevicePropertyValue(string PropName);
        void SetDeviceProperty(string PropName, string PropValue);
        void GetDevicePropertyInfoDetails
            (string PropName, out string _Value, out bool HasLimits, out double MinValue
            , out double MaxValue, out PropertyType tType, out bool ReadOnly
            , out bool HasAllowedValues, out string[] AllowedValues);
        void BuildPropertyList();
        void SetPropUI(IPropertyList TargetPropertyList);
        void StopDevice();
    }
}
