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
    public delegate void  OnStopsChanged(object Sender);
    [Guid("1514adf6-7cb1-0027-7Ebb-b75c1467149b")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Serializable]
    public class ZStage:MMDeviceBase,IZStageCom  
    {
        /// <summary>
        /// Fires when the list of named stops changes
        /// </summary>
        public event OnStopsChanged StopsChanged;

        /// <summary>
        /// Class used for the location lists to allow the stage to return to a particular named point
        /// </summary>
        public class Zlocation
        {
            public string LocationName="";
            public double Location=0;
            public Zlocation()
            { }
            public Zlocation(string lName, double pos)
            {
                LocationName = lName;
                Location = pos;
            }
        }
        private List<Zlocation> _Stops=new List<Zlocation>();
        
        /// <summary>
        /// List of all the named stops on this stage and their location
        /// </summary>
        public List<Zlocation> Stops
        {
            get { return _Stops; }
        }

        public void RenameStop(int Index, string NewName)
        {
            _Stops[Index].LocationName = NewName;
            if (StopsChanged != null) StopsChanged(this);
        }
        public void RenameStop(string OldName, string NewName)
        {
            int i = 0;
            foreach (Zlocation z in _Stops)
            {
                if (z.LocationName.ToLower() == OldName.ToLower()) RenameStop(i, NewName);
                i++;

            }
        }
        
        /// <summary>
        /// Go to a named location (index by number)
        /// </summary>
        /// <param name="LocationNumber"></param>
        public void GoToLocation(int LocationNumber)
        {
            if (LocationNumber <_Stops.Count )  SetPositionAbsolute(_Stops[LocationNumber].Location/StepSize_um );
        }

        /// <summary>
        /// Goto a named location (index by name)
        /// </summary>
        /// <param name="LocationName"></param>
        public void GoToLocation(String LocationName)
        {
            foreach (Zlocation z in _Stops)
            {
                if (z.LocationName.ToLower() == LocationName.ToLower()) SetPositionAbsolute(z.Location);

            }

        }

        private double StepSize_um_=-1;
        private double TopLimit_um_=-1;
        private double BottomLimit_um_=-1;
        
        /// <summary>
        /// Upper limit of stage.  This information is not provided by the stage automatically, so must be set by the software.
        /// </summary>
        public double   TopLimit_um
        {
           
            get {
                if (TopLimit_um_ == -1)
                {
                    return 5 * 1000;
                    //throw new Exception("You must set a top limit to the motion of the stage manually");
                }
                else 
                  return TopLimit_um_;}

        }

        /// <summary>
        /// Lower limit of stage. Set by hardware adapter
        /// </summary>
        public double   BottomLimit_um
        {
            
            get {
                if (BottomLimit_um_ == -1)
                {
                    return  -5 * 1000;
                    //throw new Exception("You must set a bottom limit to the motion of the stage manually");
                }
                else 
                    return BottomLimit_um_;}

        }
        


        /// <summary>
        /// Step size of a single step.  This value needs to be assigned in some way.
        /// </summary>
        public double StepSize_um
        {
            set { StepSize_um_ = value; }
            get { return StepSize_um_ ; }
        }

        public ZStage(EasyCore ECore,string DeviceLabel,string LibraryName,string DeviceAdapter )
        {
            SetCore(ECore, DeviceLabel, LibraryName, DeviceAdapter);
            EstimateStepSize();
        }
        public ZStage(EasyCore ECore, string DeviceLabel)
        {
            DeviceName = DeviceLabel;
            SetCore(ECore);
            EstimateStepSize();

            


        }
        //checks the property list to see if there is stepsize information and then gives a estimate otherwise.
        private void  EstimateStepSize()
        {
            
            StepSize_um_ = 0;
            try
            {
                double.TryParse(GetDevicePropertyValue("StepSize"), out StepSize_um_);
            }
            catch { StepSize_um_ = 0; }
            if (StepSize_um_ == 0)
            {
                try
                {
                    double.TryParse(GetDevicePropertyValue("stepsize"), out StepSize_um_);
                }
                catch { StepSize_um_ = 0; }

                if (StepSize_um_ == 0)
                {
                    try
                    {
                        double.TryParse(GetDevicePropertyValue("Stepsize"), out StepSize_um_);
                    }
                    catch { StepSize_um_ = 0; }
                    if (StepSize_um_ == 0)
                    {
                        try
                        {
                            double.TryParse(GetDevicePropertyValue("stepSize"), out StepSize_um_);
                        }
                        catch { StepSize_um_ = 0; }
                    }
                }
            }
            if (StepSize_um_ == 0)
                StepSize_um_ = .05;   //50 nm is just a confortable guess.

        }
        /// <summary>
        /// Used to notify core that this is the primary stage of the setup.  Should not be done if xystage is primary
        /// </summary>
        public override  void MakeOffical()
        {
            try
            {
                core.setFocusDevice(deviceName);
            }
            catch(Exception ex)
            {
                Ecore.LogMessage("zStage could not be made offical \n" + ex.Message);
            }
            ECore.MMFocusStage = this;
        }

        /// <summary>
        /// Gets the current position of the stage in steps
        /// </summary>
        /// <returns></returns>
        public int CurrentPositionSteps()
        {
            return (int)(core.getPosition(DeviceName)/StepSize_um );
        }
        /// <summary>
        /// Gets the current position of the stage in microns
        /// </summary>
        /// <returns></returns>
        public double CurrentPosition()
        {
            return core.getPosition(DeviceName);
        }

        /// <summary>
        /// Sets the position of the stage in microns.  The resultant absolute position is determined by hardware
        /// </summary>
        /// <param name="ZPos_um"></param>
        public void SetPositionAbsolute(double ZPos_um)
        {
            try
            {
                core.setPosition(DeviceName, ZPos_um);
            }
            catch (Exception ex)
            {
                Ecore.LogErrorMessage("Set Position failed in z stage control", 1);
                Ecore.LogErrorMessage(ex.Message, 3);
            }
        }

        /// <summary>
        /// Sets the position of the stage relative to its current position
        /// </summary>
        /// <param name="ZChange_Um"></param>
        public void SetPositionRelative(double ZChange_Um)
        {
            double d= core.getPosition(DeviceName);
            SetPositionAbsolute(d + ZChange_Um);
        }
        public override  void StopDevice()
        {
        }

    }

    [Guid("5A88092E-69DF-0027-AD8D-8FA83E550F20")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IZStageCom
    {
         double  BottomLimit_um { get; }
         double StepSize_um { get; }
         List<ZStage.Zlocation> Stops { get; }
         double  TopLimit_um { get; }

         event OnStopsChanged StopsChanged;

         double CurrentPosition();
         int CurrentPositionSteps();
         void GoToLocation(int LocationNumber);
         void GoToLocation(string LocationName);
         
         void RenameStop(int Index, string NewName);
         void RenameStop(string OldName, string NewName);
         void SetPositionAbsolute(double ZPos_um);
         void SetPositionRelative(double ZChange_Um);
         

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
