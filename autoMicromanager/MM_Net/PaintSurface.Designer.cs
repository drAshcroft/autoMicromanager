using PaintDotNet.Actions;
using PaintDotNet.Effects;
using PaintDotNet.Menus;
using PaintDotNet.SystemLayer;
using PaintDotNet.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Text;
using System.Threading;
using System.Windows.Forms;



namespace PaintDotNet
{
    partial class PaintSurface: UserControl, ISnapManagerHost
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

                if (this.floaterOpacityTimer != null)
                {
                    this.floaterOpacityTimer.Tick -= new System.EventHandler(this.FloaterOpacityTimer_Tick);
                    this.floaterOpacityTimer.Dispose();
                    this.floaterOpacityTimer = null;
                }

            }
     
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            try
            {
                base.Dispose(disposing);
            }

            catch { }
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
           
            this.SuspendLayout();
            // 
            // PaintSurface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "PaintSurface";
            this.Size = new System.Drawing.Size(525, 460);


            this.components = new System.ComponentModel.Container();
            this.defaultButton = new System.Windows.Forms.Button();
            UI.InitScaling(this.defaultButton);
            this.appWorkspace = new PaintDotNet.AppWorkspace();
            this.floaterOpacityTimer = new System.Windows.Forms.Timer(this.components);
            this.deferredInitializationTimer = new System.Windows.Forms.Timer(this.components);
            
            // 
            // appWorkspace
            // 
            this.appWorkspace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.appWorkspace.Location = new System.Drawing.Point(0, 0);
            this.appWorkspace.Name = "appWorkspace";
            this.appWorkspace.Size = new System.Drawing.Size(752, 648);
            this.appWorkspace.TabIndex = 2;
            this.appWorkspace.ActiveDocumentWorkspaceChanging += new EventHandler(AppWorkspace_ActiveDocumentWorkspaceChanging);
            this.appWorkspace.ActiveDocumentWorkspaceChanged += new EventHandler(AppWorkspace_ActiveDocumentWorkspaceChanged);

            this.appWorkspace.MoveUp += new MoveUpEvent(appWorkspace_MoveUp);
            this.appWorkspace.MoveDown += new MoveDownEvent(appWorkspace_MoveDown);
            this.appWorkspace.MoveRight += new MoveRightEvent(appWorkspace_MoveRight);
            this.appWorkspace.MoveLeft += new MoveLeftEvent(appWorkspace_MoveLeft);

            // 

            // floaterOpacityTimer
            // 



            this.floaterOpacityTimer.Enabled = false;
            this.floaterOpacityTimer.Interval = 25;
            this.floaterOpacityTimer.Tick += new System.EventHandler(this.FloaterOpacityTimer_Tick);
            //
            // deferredInitializationTimer
            //
            this.deferredInitializationTimer.Interval = 250;
            this.deferredInitializationTimer.Tick += new EventHandler(DeferredInitialization);
            //
            // defaultButton
            //
            this.defaultButton.Size = new System.Drawing.Size(1, 1);
            this.defaultButton.Text = "";
            this.defaultButton.Location = new Point(-100, -100);
            this.defaultButton.TabStop = false;
            this.defaultButton.Click += new EventHandler(DefaultButton_Click);
            // 
            // MainForm
            // 

            

            this.AutoScaleDimensions = new SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(950, 738);
            this.Controls.Add(this.appWorkspace);
            this.Controls.Add(this.defaultButton);

            this.Name = "MainForm";
            this.Controls.SetChildIndex(this.appWorkspace, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        void appWorkspace_MoveUp(object sender)
        {
            if (MoveUp != null) MoveUp(this);
        }

        void appWorkspace_MoveDown(object sender)
        {
            if (MoveDown != null) MoveDown(this);
        }
        void appWorkspace_MoveRight(object sender)
        {
            if (MoveRight != null) MoveRight(this);
        }
        void appWorkspace_MoveLeft(object sender)
        {
            if (MoveLeft != null) MoveLeft(this);
        }

        #endregion
    }
}
