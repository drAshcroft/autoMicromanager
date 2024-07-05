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
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Reflection;


namespace CoreDevices.DeviceControls
{
    [Serializable]
    public partial class PropertyList : UserControl,IPropertyList 
    
    {
        Dictionary<string, PropertyInfo> PropertiesDict;
        //private  CoreDevices.PropertyInfo[]  PropertiesMap=null;
        
        private EasyCore  ECore;
        //private string DeviceLabel;
        

        //private List<CoreDevices.PropertyInfo> PropertiesList;

        public PropertyList()
        {
            InitializeComponent();
        }



      


        private float GetMaxWidth(Dictionary<string, PropertyInfo> list)
        {
            Graphics g = this.CreateGraphics();

            float tWidth = 0;
            foreach (KeyValuePair<string, PropertyInfo> pi in  list)
            {
                float ttWidth = g.MeasureString(pi.Value.PropName , this.Font).Width;
                if (ttWidth > tWidth) tWidth = ttWidth;
            }
            return (tWidth  * 1.2f);


        }
        private Label CreateLabel(string lname, string Text, int x,int y)
        {
            Label l = new Label();
            l = new System.Windows.Forms.Label();
            l.AutoSize = true;
            l.Location = new System.Drawing.Point(x, y);
            l.Name = lname ;
            l.Size = new System.Drawing.Size(51, 13);
            l.TabIndex = 1;
            l.Text = Text ;
            this.PropertyPanel.Controls.Add(l);
            return l;

        }
        private TextBox CreateTextBox(string lname, string Text, int x,int y,int width)
        {
            TextBox tb = new TextBox();
            this.PropertyPanel.Controls.Add(tb);
            tb.Location = new System.Drawing.Point((int)x, y);
            tb.Name = lname ;
            tb.Size = new System.Drawing.Size((int)(width ), 22);
            tb.TabIndex = 5;
            tb.Text = Text ;
            tb.TextChanged += new EventHandler(tb_TextChanged);
            return (tb);
        }
        private ComboBox CreateComboBox(string lname, string Text, int x, int y,int width, string[] Options)
        {
            ComboBox cb = new ComboBox();
            //ListBox cb = new ListBox();
            PropertyPanel.Controls.Add(cb);
            cb.DropDownStyle = ComboBoxStyle.DropDownList;
            cb.FormattingEnabled = true;
            cb.Location = new System.Drawing.Point((int)x, y);
            cb.Name= (lname);
            cb.Size = new System.Drawing.Size((int)(width ), 20);
           // cb.Width = width;
            cb.TabIndex = 0;
            for (int j = 0; j < Options.Length ; j++)
            {
                cb.Items.Add(Options[j]);
            }
            cb.Text =Text ;
            int jj = cb.Items.IndexOf(cb.Text);
            if (jj != -1)
            {
                cb.Select(jj, 1);
            }
            else
                cb.Select(0, 1);
            cb.Refresh();
            cb.SelectionChangeCommitted += new EventHandler(cb_SelectionChangeCommitted);
            return (cb);
        }
        private SlideAndTextl  CreateColorSlider(string lname, string Text,string value,double Max,double Min, int x, int y,int width)
        {
            SlideAndTextl  cs = new SlideAndTextl ();
            this.PropertyPanel.Controls.Add(cs);
            cs.MaxValue =(float) Max;
            cs.MinValue =(float) Min;
            cs.BackColor = System.Drawing.Color.Transparent;
            cs.Size = new System.Drawing.Size(8, 8);
            cs.Location = new System.Drawing.Point((int)x, y);
            cs.Name =lname  ;
            cs.Size = new System.Drawing.Size((int)(width ), 22);
            
            cs.TabIndex = 0;
            cs.Text = Text ;
            
            cs.Visible = true;

            cs.OnValueChanged += new OnValueChangedEvent(cs_OnValueChanged);
            return (cs);
        }

        void cs_OnValueChanged(object sender, float value)
        {
            SlideAndTextl  cb = (SlideAndTextl )sender;
            string cName = cb.Name;
            //props[i] + "||T" + i;
            string[] seps = { "||" };
            string[] parts = cName.Split(seps, StringSplitOptions.RemoveEmptyEntries);
            int index = int.Parse(parts[1]);

            PropertyInfo PI = PropertiesDict[parts[0]];// PropertiesMap[index];
            float f = (float)cb.Value;
            string propValue = f.ToString();
            try
            {
                PI.Value = propValue;
                
            }
            catch
            {
                MessageBox.Show("Invalid Property Value", "", MessageBoxButtons.OK);
            }
        }

        private void LayoutProperties(Dictionary<string, PropertyInfo> Properties)
        {
            //PropertiesMap = new CoreDevices.PropertyInfo[Properties.Count];

            //Properties.CopyTo(PropertiesMap);
            this.PropertiesDict = Properties;
            int i=0;
            int tWidth=(int)GetMaxWidth(Properties );
            int lWidth = (int)(.5 * (PropertyPanel.Width - tWidth));
            int y = 0;
            //PropertiesMap = new float[2, props.Count];

            foreach (KeyValuePair<string, PropertyInfo> PI in Properties)// int i = 0; i < props.Count; i++)
            {
                
                //PropertyType pt = core.getPropertyType(DeviceLabel, props[i]);
                Label l = CreateLabel(PI.Value.PropName + "LL" + i, PI.Value.PropName, 0, y);
                
                    
                int lH = 0;
                if (PI.Value.ReadOnly )//   core.isPropertyReadOnly(DeviceLabel, props[i]))
                {
                    Label l2 = CreateLabel(PI.Value.PropName + "||" + i, PI.Value.PropName, tWidth, y);
                    
                }
                else if (PI.Value.HasAllowedValues)
                {
                    ComboBox l2 = CreateComboBox(PI.Value.PropName + "||" + i, PI.Value.Value, tWidth, y, lWidth, PI.Value.AllowedValues);
                }
                else if (PI.Value.HasLimits)
                {
                    SlideAndTextl cs = CreateColorSlider(PI.Value.PropName + "||" + i, PI.Value.Value, PI.Value.Value, PI.Value.MaxValue, PI.Value.MinValue, tWidth, y, lWidth);
                }
                else 
                {

                    TextBox tb = CreateTextBox(PI.Value.PropName + "||" + i, PI.Value.Value, tWidth, y, lWidth);
                }
                if (l.Height > lH)
                {
                    y += (int)(25 * 1.2);
                }
                else
                {
                    y += (int)(25 * 1.2);
                }
                PI.Value.OnPropUpdate += new OnPropertyUpdatedEvent(Value_OnPropUpdate);
            }
            PropertyPanel.Height = y;
        }
        private delegate void PropUpdateEvent(object sender, string PropName, string PropValue);
        private event PropUpdateEvent PropUpdate;
        void Value_OnPropUpdate(object sender, string PropName, string PropValue)
        {
            if (this.InvokeRequired)
            {
                if (PropUpdate == null) PropUpdate += new PropUpdateEvent(Value_OnPropUpdate);
                object[] pars = { sender, PropName, PropValue };
                this.Invoke(PropUpdate, pars);
            }
            else 
                UpdateProperty(PropName);
        }
        private delegate void SetCoreDel(Dictionary<string, PropertyInfo> Properties);
        public void SetCore(Dictionary<string, PropertyInfo> Properties)
        {
            if (InvokeRequired == true)
            {
                Invoke(new SetCoreDel(SetCore ),new object [] { Properties});
            }
            else
                LayoutProperties(Properties);

        }
        public void UpdateProperty(string Propname)
        {

            //string controlName = PI.PropName + "||" + i;

            PropertyInfo SelectedProp = null;

            foreach (Control c in PropertyPanel.Controls)
            {
                if (c.Name.Contains("||"))
                {
                    string cName = c.Name;
                    string[] seps = { "||" };
                    string[] parts = cName.Split(seps, StringSplitOptions.RemoveEmptyEntries);
                    int index = int.Parse(parts[1]);

                    try 
                    {
                    PropertyInfo PI = PropertiesDict[parts[0]];
                    if (PI.PropName.ToLower() == Propname.ToLower())
                    {
                        SelectedProp = PI;
                    }
                    if (c.GetType() == typeof(SlideAndTextl))
                    {

                        SlideAndTextl cs = (SlideAndTextl)c;
                        cs.Text = PI.Value;
                    }
                    else
                    {

                        if (c.GetType() == typeof(TextBox))
                        {
                            TextBox c2 = (TextBox)c;
                            if (c2.Text != PI.Value) c2.Text = PI.Value;
                        }
                        else if (c.GetType() == typeof(ComboBox))
                        {
                            ComboBox c2 = (ComboBox)c;
                            if (c2.Text != PI.Value)
                            {
                                c2.Text = PI.Value;
                                c2.SelectedIndex = c2.FindString(PI.Value);
                            }
                        }
                    }
                    }
                    catch {}
                }
            }




        }
        public void SetCore(string deviceLabel, EasyCore  core)
        {
            ECore = core;
            Dictionary<string, PropertyInfo> Pis = core.GetDevice(deviceLabel).GetAllDeviceProperties();
                //   core.GetDevicePropertyList(deviceLabel);
            SetCore (Pis);
        }

        void tb_TextChanged(object sender, EventArgs e)
        {
            TextBox cb = (TextBox)sender;
            string cName = cb.Name;
            string[] seps = { "||" };
            string[] parts = cName.Split(seps, StringSplitOptions.RemoveEmptyEntries);
            int index = int.Parse(parts[1]);

            PropertyInfo PI = PropertiesDict[parts[0]];
            try
            {
                PI.Value =cb.Text ;
                
            }
            catch
            {
                MessageBox.Show("Invalid Property Value", "", MessageBoxButtons.OK);
            }
        }

        void cb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            string cName = cb.Name;
            string[] seps = { "||" };
            string[] parts = cName.Split(seps, StringSplitOptions.RemoveEmptyEntries);
            int index = int.Parse(parts[1]);

            PropertyInfo PI = PropertiesDict [parts[0]];
            try
            {
                PI.Value = cb.Text;
                
            }
            catch
            {
                MessageBox.Show("Invalid Property Value", "", MessageBoxButtons.OK);
            }
        }

        private void PropertyList_Resize(object sender, EventArgs e)
        {
            PropertyPanel.Width = this.Width;
            PropertyPanel.Left = 0;
            PropertyPanel.Top = 0;
        }
    }
}
