using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Micromanager_net
{
    partial class PictureBox
	{
		// Private variables for storing the values at the wheels etc.
		private string m_FloatFormat = "F2";
		private double m_FrontL = 0;
		private double m_FrontR = 0;
		private double m_RearL = 0;
		private double m_RearR = 0;

		/*
		 * These are public properties, they are there to make the values read/writeable by
		 * external code, also can perform validation etc, in this instance once the property
		 * is set it redraws the control by calling the Invalidate method.
		 */
        private System.Drawing. Bitmap MyImage = null;

        public void SetImage(System.Drawing. Bitmap Image)
        {
            MyImage = Image;
            
             
            this.Invalidate();
            
        }

        public System.Drawing.Bitmap DispImage
        {
            set { 
                MyImage = value;
                
                this.Invalidate();
            }
        }
		public string FloatFormat
		{
			get { return this.m_FloatFormat; }
			set
			{
				this.m_FloatFormat = value;
				this.Invalidate();
			}
		}
		public double FrontL
		{
			get { return this.m_FrontL; }
			set
			{
				this.m_FrontL = value;

				this.Invalidate();
			}
		}
		public double FrontR
		{
			get { return this.m_FrontR; }
			set
			{
				this.m_FrontR = value;
				this.Invalidate();
			}
		}
		public double RearL
		{
			get { return this.m_RearL; }
			set
			{
				this.m_RearL = value;
                
				this.Invalidate();
			}
		}
		public double RearR
		{
			get { return this.m_RearR; }
			set
			{
				this.m_RearR = value;
				this.Invalidate();
			}
		}

		/*
		 * These properties do not use the private variables but calculate themselves from
		 * the other properties (when they are accessed), thus eliminating the need to update
		 * multiple variables.  They are hidden from the .NET PropertyGrid by the
		 * Browsable(false) attribute.
		 */
		#region Calculated

		[Browsable(false)]
		public double Cross1
		{
			get { return (this.FrontL + this.RearR); }
		}
		[Browsable(false)]
		public double Cross2
		{
			get { return (this.FrontR + this.RearL); }
		}
		[Browsable(false)]
		public double Front
		{
			get { return (this.FrontL + this.FrontR); }
		}
		[Browsable(false)]
		public double Back
		{
			get { return (this.RearL + this.RearR); }
		}
		[Browsable(false)]
		public double Total
		{
			get { return (this.FrontL + this.FrontR + this.RearL + this.RearR); }
		}

		#endregion
	}
}