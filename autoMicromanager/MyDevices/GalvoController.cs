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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;
using CoreDevices;
using System.IO;

namespace Micromanager_net.UI
{
    /// <summary>
    /// These are a number of GUIs that I use my lab.  Their use is pretty straight forward and are intended as an example of the customization you can perform.
    /// This is designed to run voice coil galvos, and to perform laser scanning confocal.
    /// </summary>
    public partial class GalvoControler : UserControl, CoreDevices.UI.GUIDeviceControl
    {
        CoreDevices.Devices.FunctionGenerator FuncGen;
        CoreDevices.EasyCore ECore;
        double _UpdateFreq = 1000;//todo:: this needs to be read from the board properties
        Color[] QBColors = {Color.Red,Color.Blue,Color.Green,Color.Yellow,Color.Black,Color.Brown,Color.Cyan,Color.DarkBlue ,Color.Gold  };
        public string DeviceType() { return CWrapper.DeviceType.SignalIODevice.ToString(); }
        public string ExtraInformation
        {
            get { return ""; }
            set { }
        }
        public GalvoControler()
        {
            InitializeComponent();
            
        }
        public Control GetControl()
        {
            return this;
        }
        public string Caption()
        {
            return ("Function Generator Properties");
        }
        public void SetCore(EasyCore Ecore,string DeviceName)
        {
        //public void SetSignalIO(Devices.FunctionGenerator fg)
            ECore = Ecore;
            try
            {
                if (DeviceName != "")
                {
                    FuncGen = (CoreDevices.Devices.FunctionGenerator)Ecore.GetDevice(DeviceName);
                }
                else
                {
                    FuncGen = Ecore.MMFunctionGenerator;
                }
            }
            catch { }
            if (FuncGen != null)
            {
                FuncGen.SetPropUI((IPropertyList )propertyList1);
                //WaveFormFreqTB0.Text = FuncGen.UpdateFrequency.ToString();
            }
        }

        

        private void StartGenerationB_Click(object sender, EventArgs e)
        {
            UpdateChannels();
            FuncGen.ClearAllChannelData();
            for (int i = 0; i < ChannelData.Count ; i++)
            {
                FuncGen.AddChannelData(i, ChannelData[i]);
            }
            FuncGen.StartGenerating();
        }

        private void StopB_Click(object sender, EventArgs e)
        {
            FuncGen.StopGenerating();
        }

       

        private int NumChannels = 1;
        List<double[]> ChannelData = new List<double[]>();
        private void AddChannelB_Click(object sender, EventArgs e)
        {

            NumChannels++;
          System.Windows.Forms.TextBox WaveformPhaseTB0;
          System.Windows.Forms.Label WaveformPhaselabel0;
          System.Windows.Forms.Label WaveFormTypeLabel;
          System.Windows.Forms.ListBox WaveFormTypeLB0;
          System.Windows.Forms.Label Waveformlabel2;
          System.Windows.Forms.TextBox WaveFormAmplitudeTB0;
          System.Windows.Forms.TextBox WaveFormFreqTB0;
          System.Windows.Forms.Label WaveFormlabel1;
          System.Windows.Forms.TextBox WaveFormFilename0;
          System.Windows.Forms.Label WaveformFilenameLabel0;
          System.Windows.Forms.TextBox WaveformFormula0;
          System.Windows.Forms.Label WaveformFormulaLabel0;
         

     
          WaveformPhaseTB0 = new System.Windows.Forms.TextBox();
         WaveformPhaselabel0 = new System.Windows.Forms.Label();
         WaveFormTypeLabel = new System.Windows.Forms.Label();
         WaveFormTypeLB0 = new System.Windows.Forms.ListBox();
         Waveformlabel2 = new System.Windows.Forms.Label();
         WaveFormAmplitudeTB0 = new System.Windows.Forms.TextBox();
         WaveFormFreqTB0 = new System.Windows.Forms.TextBox();
         WaveFormlabel1 = new System.Windows.Forms.Label();
         WaveformFormulaLabel0 = new System.Windows.Forms.Label();
         WaveformFormula0 = new System.Windows.Forms.TextBox();
         WaveformFilenameLabel0 = new System.Windows.Forms.Label();
         WaveFormFilename0 = new System.Windows.Forms.TextBox();
        
         tabPage3 = new System.Windows.Forms.TabPage();

         tabPage3.SuspendLayout();

         tabControl2.Controls.Add( tabPage3);

       
         tabPage3.Controls.Add( WaveFormFilename0);
         tabPage3.Controls.Add( WaveformFilenameLabel0);
         tabPage3.Controls.Add( WaveformFormula0);
         tabPage3.Controls.Add( WaveformFormulaLabel0);
         tabPage3.Controls.Add( WaveformPhaseTB0);
         tabPage3.Controls.Add( WaveformPhaselabel0);
         tabPage3.Controls.Add( WaveFormTypeLabel);
         tabPage3.Controls.Add( WaveFormTypeLB0);
         tabPage3.Controls.Add( Waveformlabel2);
         tabPage3.Controls.Add( WaveFormAmplitudeTB0);
         tabPage3.Controls.Add( WaveFormFreqTB0);
         tabPage3.Controls.Add( WaveFormlabel1);
         tabPage3.Location = new System.Drawing.Point(4, 22);


         string ChannelString = (NumChannels - 1).ToString();

         tabPage3.Name = "tabPage3";
         tabPage3.Padding = new System.Windows.Forms.Padding(3);
         tabPage3.Size = new System.Drawing.Size(303, 229);
         tabPage3.TabIndex = 0;
         tabPage3.Text = "Channel " + ChannelString;
         tabPage3.UseVisualStyleBackColor = true;

        // 
        // WaveformPhaseTB0
        // 
         WaveformPhaseTB0.Location = new System.Drawing.Point(164, 95);
         WaveformPhaseTB0.Name = "WaveformPhaseTB" + ChannelString;
         WaveformPhaseTB0.Size = new System.Drawing.Size(89, 20);
         WaveformPhaseTB0.TabIndex = 22;
         WaveformPhaseTB0.TextChanged += new System.EventHandler( WaveformPhaseTB0_TextChanged);
        // 
        // WaveformPhaselabel0
        // 
         WaveformPhaselabel0.AutoSize = true;
         WaveformPhaselabel0.Location = new System.Drawing.Point(161, 79);
         WaveformPhaselabel0.Name = "WaveformPhaselabel" + ChannelString;
         WaveformPhaselabel0.Size = new System.Drawing.Size(37, 13);
         WaveformPhaselabel0.TabIndex = 21;
         WaveformPhaselabel0.Text = "Phase";
        // 
        // WaveFormTypeLabel
        // 
         WaveFormTypeLabel.AutoSize = true;
         WaveFormTypeLabel.Location = new System.Drawing.Point(6, 1);
         WaveFormTypeLabel.Name = "WaveFormTypeLabel" + ChannelString;
         WaveFormTypeLabel.Size = new System.Drawing.Size(66, 13);
         WaveFormTypeLabel.TabIndex = 20;
         WaveFormTypeLabel.Text = "Output Type";
        // 
        // WaveFormTypeLB0
        // 
         WaveFormTypeLB0.FormattingEnabled = true;
         WaveFormTypeLB0.Items.AddRange(new object[] {
        "Gaussian Noise",
        "Pseudo Noise",
        "MersenneTwister Noise",
        "Sine",
        "Cosine",
        "Triangle",
        "Square",
        "SawTooth",
        "Constant Value",
        "Formula",
        "UserDefined"});
         WaveFormTypeLB0.Location = new System.Drawing.Point(9, 17);
         WaveFormTypeLB0.Name = "WaveFormTypeLB" + ChannelString ;
         WaveFormTypeLB0.Size = new System.Drawing.Size(128, 160);
         WaveFormTypeLB0.TabIndex = 19;
         WaveFormTypeLB0.SelectedIndexChanged += new System.EventHandler( WaveFormTypeLB_SelectedIndexChanged);
        // 
        // Waveformlabel2
        // 
         Waveformlabel2.AutoSize = true;
         Waveformlabel2.Location = new System.Drawing.Point(161, 1);
         Waveformlabel2.Name = "Waveformlabel" + ChannelString; 
         Waveformlabel2.Size = new System.Drawing.Size(53, 13);
         Waveformlabel2.TabIndex = 18;
         Waveformlabel2.Text = "Amplitude";
        // 
        // WaveFormAmplitudeTB0
        // 
         WaveFormAmplitudeTB0.Location = new System.Drawing.Point(164, 17);
         WaveFormAmplitudeTB0.Name = "WaveFormAmplitudeTB" + ChannelString ;
         WaveFormAmplitudeTB0.Size = new System.Drawing.Size(89, 20);
         WaveFormAmplitudeTB0.TabIndex = 17;
         WaveFormAmplitudeTB0.TextChanged += new System.EventHandler( WaveFormAmplitudeTB_TextChanged);
        // 
        // WaveFormFreqTB0
        // 
         WaveFormFreqTB0.Location = new System.Drawing.Point(164, 56);
         WaveFormFreqTB0.Name = "WaveFormFreqTB" + ChannelString; 
         WaveFormFreqTB0.Size = new System.Drawing.Size(89, 20);
         WaveFormFreqTB0.TabIndex = 16;
         WaveFormFreqTB0.TextChanged += new System.EventHandler( WaveFormFreqTB0_TextChanged);
        // 
        // WaveFormlabel1
        // 
         WaveFormlabel1.AutoSize = true;
         WaveFormlabel1.Location = new System.Drawing.Point(161, 40);
         WaveFormlabel1.Name = "WaveFormlabel" + ChannelString ;
         WaveFormlabel1.Size = new System.Drawing.Size(57, 13);
         WaveFormlabel1.TabIndex = 15;
         WaveFormlabel1.Text = "Frequency";

        // 
        // WaveformFormulaLabel0
        // 
         WaveformFormulaLabel0.AutoSize = true;
         WaveformFormulaLabel0.Location = new System.Drawing.Point(161, 118);
         WaveformFormulaLabel0.Name = "WaveformFormulaLabel" + ChannelString; 
         WaveformFormulaLabel0.Size = new System.Drawing.Size(44, 13);
         WaveformFormulaLabel0.TabIndex = 23;
         WaveformFormulaLabel0.Text = "Formula";
         WaveformFormulaLabel0.Visible = true;
        // 
        // WaveformFormula0
        // 
         WaveformFormula0.Location = new System.Drawing.Point(164, 134);
         WaveformFormula0.Name = "WaveformFormula" + ChannelString; 
         WaveformFormula0.Size = new System.Drawing.Size(89, 20);
         WaveformFormula0.TabIndex = 24;
         WaveformFormula0.Visible = true ;
         WaveformFormula0.TextChanged += new System.EventHandler( WaveformFormula0_TextChanged);
        // 
        // WaveformFilenameLabel0
        // 
         WaveformFilenameLabel0.AutoSize = true;
         WaveformFilenameLabel0.Location = new System.Drawing.Point(161, 157);
         WaveformFilenameLabel0.Name = "WaveformFilenameLabel" + ChannelString; 
         WaveformFilenameLabel0.Size = new System.Drawing.Size(49, 13);
         WaveformFilenameLabel0.TabIndex = 25;
         WaveformFilenameLabel0.Text = "Filename";
         WaveformFilenameLabel0.Visible = true ;
        // 
        // WaveFormFilename0
        // 
         WaveFormFilename0.Location = new System.Drawing.Point(164, 173);
         WaveFormFilename0.Name = "WaveFormFilename" + ChannelString; 
         WaveFormFilename0.Size = new System.Drawing.Size(89, 20);
         WaveFormFilename0.TabIndex = 26;
         WaveFormFilename0.Visible = true;
         WaveFormFilename0.TextChanged += new System.EventHandler( WaveFormFilename0_TextChanged);
    
         tabPage3.ResumeLayout(false);
         tabPage3.PerformLayout();
  
        }

        
        private void WaveFormTypeLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            string junk = ((Control)sender).Name.Trim();
            string PageNumberString = junk.Substring(junk.Length - 1);
            int PageNumber=0;
            int.TryParse(PageNumberString,out PageNumber);
            ControlCollection TargetControls=  tabControl2.TabPages[PageNumber].Controls;
           
            ListBox SenderLB = (ListBox)sender;
            string SelectedItem=(string)  SenderLB.SelectedItem;
            if (SelectedItem == "Formula")
            {
                ((TextBox)ReturnControl(TargetControls, "WaveformFormula" + PageNumberString)).Enabled=true ;

                ((TextBox)ReturnControl(TargetControls, "WaveFormFilename" + PageNumberString)).Enabled = false ;


                ((TextBox)ReturnControl(TargetControls, "WaveFormAmplitudeTB" + PageNumberString)).Enabled=false  ;
                ((TextBox)ReturnControl(TargetControls, "WaveFormFreqTB" + PageNumberString)).Enabled=false  ;
                ((TextBox)ReturnControl(TargetControls, "WaveformPhaseTB" + PageNumberString)).Enabled=false  ;
                
                                        
            }
            else if (SelectedItem == "UserDefined")
            {
                ((TextBox)ReturnControl(TargetControls, "WaveFormFilename" + PageNumberString)).Enabled = true;

                ((TextBox)ReturnControl(TargetControls, "WaveformFormula" + PageNumberString)).Enabled = false;
                

                ((TextBox)ReturnControl(TargetControls, "WaveFormAmplitudeTB" + PageNumberString)).Enabled=false  ;
                ((TextBox)ReturnControl(TargetControls, "WaveFormFreqTB" + PageNumberString)).Enabled=false  ;
                ((TextBox)ReturnControl(TargetControls, "WaveformPhaseTB" + PageNumberString)).Enabled=false  ;

            }
            else if (SelectedItem.Contains("Noise")==true )
            {
                ((TextBox)ReturnControl(TargetControls, "WaveFormAmplitudeTB" + PageNumberString)).Enabled = true;
                ((TextBox)ReturnControl(TargetControls, "WaveFormFreqTB" + PageNumberString)).Enabled = false ;
                ((TextBox)ReturnControl(TargetControls, "WaveformPhaseTB" + PageNumberString)).Enabled = true;

                ((System.Windows.Forms. Label )ReturnControl(TargetControls, "WaveformPhaselabel" + PageNumberString)).Text ="Seed Value (-1 for random)"  ;
            }
            else
            {
                ((TextBox)ReturnControl(TargetControls, "WaveformFormula" + PageNumberString)).Enabled = false ;
                ((TextBox)ReturnControl(TargetControls, "WaveFormFilename" + PageNumberString)).Enabled = false ;

                ((TextBox)ReturnControl(TargetControls, "WaveFormAmplitudeTB" + PageNumberString)).Enabled = true ;
                ((TextBox)ReturnControl(TargetControls, "WaveFormFreqTB" + PageNumberString)).Enabled = true ;
                ((TextBox)ReturnControl(TargetControls, "WaveformPhaseTB" + PageNumberString)).Enabled = true ;


                ((System.Windows.Forms.Label)ReturnControl(TargetControls, "WaveformPhaselabel" + PageNumberString)).Text = "Phase";
          
            }
            UpdateChannels();
        }
        private Control ReturnControl(ControlCollection Controls, string ControlName)
        {
            foreach (Control c in Controls)
            {
                if (c.Name == ControlName)
                    return c;
            }
            return null;
        }

        private void UpdateChannels()
        {
            ChannelData.Clear();
            for (int i = 0; i < tabControl2.TabPages.Count; i++)
            {
                ControlCollection TargetControls = tabControl2.TabPages[i].Controls;

                CoreDevices.Devices.FunctionGenerator.Waveforms WaveFormType = GetWaveformType((ListBox)ReturnControl(TargetControls, "WaveFormTypeLB" + i));
                string Formula = ((TextBox)ReturnControl(TargetControls, "WaveformFormula" + i)).Text;

                double Amplitude=GetTextBoxValue(TargetControls,"WaveFormAmplitudeTB" +i );

                double Freq = GetTextBoxValue(TargetControls, "WaveFormFreqTB" + i);
                double Phase = GetTextBoxValue(TargetControls, "WaveformPhaseTB" + i); 
                double Offset=0;
                double OutputTime = 0;
                double.TryParse(WaveformOutputTime0.Text, out OutputTime);
                
                double[] Data = FuncGen.BuildBuffer(WaveFormType, Freq, Amplitude, Phase, Offset, OutputTime, Formula);

                ChannelData.Add(Data);
            }
            PlotData();
        }

        private void PlotData()
        {
            // get a reference to the GraphPane

            GraphPane myPane = this.zedGraphControl1.GraphPane;

            // Set the Titles

            myPane.Title.Text = "";
            myPane.XAxis.Title.Text = "Time (s)";
            myPane.YAxis.Title.Text = "Amplitude";

            // Make up some data arrays based on the Sine function
            myPane.CurveList.Clear();
            double x;
            for (int ppi = 0; ppi < ChannelData.Count; ppi++)
            {
                double[] Data = ChannelData[ppi];
                double dt = 1 / _UpdateFreq;
                PointPairList list1 = new PointPairList();
                for (int i = 0; i < Data.Length ; i++)
                {
                    x = (double)i*dt;
                    list1.Add(x, Data[i]);
                }

                LineItem myCurve = myPane.AddCurve("Channel " + ppi.ToString() ,
                      list1, QBColors[ppi % QBColors.Length ], SymbolType.None );
            }
           
            // Tell ZedGraph to refigure the

            // axes since the data have changed

            this.zedGraphControl1 .AxisChange();
            this.Refresh();
        }

        private double GetTextBoxValue(ControlCollection TargetControls,string TextBoxName)
        {
            double d = 0;
            string junk = ((TextBox)ReturnControl(TargetControls,TextBoxName )).Text;
            double.TryParse(junk,out d);
            return d;
        }
        private CoreDevices.Devices.FunctionGenerator.Waveforms GetWaveformType(ListBox WaveChoices)
        {
            string SelectedItem = "";
            try
            {
                SelectedItem= (string)WaveChoices.SelectedItem;
            }
            catch { }
            switch ( SelectedItem )
            {
                case "Constant Value":
                    return CoreDevices.Devices.FunctionGenerator.Waveforms.SingleValue;
               case "Gaussian Noise":
                    return CoreDevices.Devices.FunctionGenerator.Waveforms.Gaussian_Noise;
               case "Pseudo Noise":
                    return CoreDevices.Devices.FunctionGenerator.Waveforms.Pseudo_Noise;
               case "MersenneTwister Noise":
                    return CoreDevices.Devices.FunctionGenerator.Waveforms.MersenneTwister;
               case  "Sine":
                    return CoreDevices.Devices.FunctionGenerator.Waveforms.Sine;
                case "Cosine":
                    return CoreDevices.Devices.FunctionGenerator.Waveforms.Cosine;
                case "Triangle":
                    return CoreDevices.Devices.FunctionGenerator.Waveforms.Triangle;
                case "Square":
                    return CoreDevices.Devices.FunctionGenerator.Waveforms.Square;
                case "SawTooth":
                    return CoreDevices.Devices.FunctionGenerator.Waveforms.SawTooth;
                case "Formula":
                    return CoreDevices.Devices.FunctionGenerator.Waveforms.Formula;
                case "UserDefined":
                    return CoreDevices.Devices.FunctionGenerator.Waveforms.UserDefined;

        }
            return CoreDevices.Devices.FunctionGenerator.Waveforms.Sine;
     }
        #region WaveFormEvents
        private void WaveFormAmplitudeTB_TextChanged(object sender, EventArgs e)
        {
            UpdateChannels();
        }

        

        private void WaveFormFreqTB0_TextChanged(object sender, EventArgs e)
        {
            UpdateChannels();
        }

        private void WaveformPhaseTB0_TextChanged(object sender, EventArgs e)
        {
            UpdateChannels();
        }

        private void WaveformFormula0_TextChanged(object sender, EventArgs e)
        {
            UpdateChannels();
        }

        private void WaveFormFilename0_TextChanged(object sender, EventArgs e)
        {
            UpdateChannels();
        }

        private void WaveformOutputTime0_TextChanged(object sender, EventArgs e)
        {
            UpdateChannels();
        }

        private void WaveformAutoCalculate0_CheckedChanged(object sender, EventArgs e)
        {
            UpdateChannels();
        }
#endregion

        private void RemoveChannelB_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ECore.StopAcquisition();
            try
            {
                ECore.MMCamera.StopFocusMovie();
            }
            catch { }
            CoreImage MainImage = ECore.MMCamera.SnapOneFrame(true);
           
            double ImageWidthUM =0;
            double ImageHeightUM=0;
            double ImageStepSize =0;
            double.TryParse(ImageWidth.Text ,out ImageWidthUM);
            double.TryParse(ImageHeight.Text,out ImageHeightUM);
            double.TryParse(StepSize.Text ,out ImageStepSize);

            if (ImageStepSize==0) ImageStepSize=1;

            int nXLines =(int)( ImageWidthUM/ImageStepSize );
            int nYLines =(int) (ImageHeightUM /ImageStepSize);

            long[,] MainImageArray = new long[nXLines , nYLines ];
            //ECore.MMCamera.StartSequence(512, ECore.MMCamera.GetExposure(), false);
            double X0=0;
            double Y0=0;
            ECore.MMXYStage.GetStagePosition(out X0,out Y0);
            for (int x = 0; x < nXLines; x += 1)
            {
               
                for (int y=0;y<nYLines ;y++)
                {
                    // CoreImage cI = ECore.MMCamera.GetSequenceFrame();
                    CoreImage cI = ECore.MMCamera.SnapOneFrame(false );
                    long imageSum = cI.SumImage();
                    Application.DoEvents();
                    ECore.MMXYStage.MoveStageGuarantee(X0+x*ImageStepSize,Y0+y*ImageStepSize);
                    ECore.UpdatePaintSurface(cI);
                    MainImageArray[x,y]=imageSum;
                }
            }
            ECore.MMCamera.EndSequence();

            MainImage = CoreImage.CreateImageFromArray(MainImageArray);
            ECore.UpdatePaintSurface(MainImage);
            MainImage.Save("c:\\test.bmp", false);
        }

        private bool Pause = false;
      

        private void button4_Click(object sender, EventArgs e)
        {
            Pause = true ;
        }

        

       
       

       
        

    }
}
