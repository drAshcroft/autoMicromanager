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
using WeifenLuo.WinFormsUI.Docking;
using System.Windows.Forms;
namespace CoreDevices
{
    /// <summary>
    /// This is the interfaces that must be maintained by the MDI interface.  
    /// </summary>
    public interface ISubMDI
    {
        event EventHandler<NewDockableFormEvents> OnNewDockableForm;
        event EventHandler<NewFormEvents> OnNewForm;
        DockContent GetForm(string FormPersistenceString,string ExtraInformation);
        void CreateBasicSetup();
    }
    public class NewFormEvents:EventArgs 
    {
        private Form newForm;
        public Form  NewForm
        {
            get {return newForm;}
        }
        public NewFormEvents(Form  newForm)
        {
            this.newForm =newForm ;
        }
    }
    public class NewDockableFormEvents:EventArgs 
    {
        private DockContent newForm;
        public DockContent NewForm
        {
            get {return newForm;}
        }
        public NewDockableFormEvents(DockContent newForm)
        {
            this.newForm =newForm ;
        }
    }
}
