/////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) dotPDN LLC, Rick Brewster, Tom Jackson, and contributors.     //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See src/Resources/Files/License.txt for full licensing and attribution      //
// details.                                                                    //
// .                                                                           //
/////////////////////////////////////////////////////////////////////////////////

using System;
using System.Windows.Forms;

namespace SciImage
{
    /// <summary>
    /// This class is used to hold references to many of the UI elements
    /// that are privately encapsulated in various places.
    /// This allows other program elements to access these objects while
    /// allowing these items to move around, and without breaking OO best 
    /// practices.
    /// </summary>
    public class WorkspaceWidgets
    {
        private AppWorkspace workspace;

        private DocumentStrip documentStrip;
        public DocumentStrip DocumentStrip
        {
            get
            {
                return this.documentStrip;
            }

            set
            {
                this.documentStrip = value;
            }
        }

        private ViewConfigStrip viewConfigStrip;
        public ViewConfigStrip ViewConfigStrip
        {
            get
            {
                return this.viewConfigStrip;
            }

            set
            {
                this.viewConfigStrip = value;
            }
        }

        private ToolConfigStrip toolConfigStrip;
        public ToolConfigStrip ToolConfigStrip
        {
            get
            {
                return this.toolConfigStrip;
            }

            set
            {
                this.toolConfigStrip = value;
            }
        }

        private CommonActionsStrip commonActionsStrip;
        public CommonActionsStrip CommonActionsStrip
        {
            get
            {
                return this.commonActionsStrip;
            }

            set
            {
                this.commonActionsStrip = value;
            }
        }

        private ToolsFormControl toolsForm;
        public ToolsFormControl ToolsForm
        {
            get
            {
                return this.toolsForm;
            }

            set
            {
                this.toolsForm = value;
            }
        }

        public ToolsControl ToolsControl
        {
            get
            {
                return this.toolsForm.ToolsControl ;
            }
        }

        private LayerFormControl layerForm;
        public LayerFormControl LayerForm
        {
            get
            {
                return layerForm;
            }

            set
            {
                layerForm = value;
            }
        }

        public LayerControl LayerControl
        {
            get
            {
                return this.layerForm.LayerControl;
            }
        }

        private HistoryFormControl historyForm;
        public HistoryFormControl HistoryForm
        {
            get
            {
                return this.historyForm;
            }

            set
            {
                this.historyForm = value;
            }
        }

        public HistoryControl HistoryControl
        {
            get
            {
                return this.historyForm.HistoryControl;
            }
        }

        private ColorPickers.ColorsFormControl colorsForm;
        public ColorPickers.ColorsFormControl ColorsForm
        {
            get
            {
                return this.colorsForm;
            }

            set
            {
                this.colorsForm = value;
            }
        }

        private IStatusBarProgress statusBarProgress;
        public IStatusBarProgress StatusBarProgress
        {
            get
            {
                return this.statusBarProgress;
            }

            set
            {
                this.statusBarProgress = value;
            }
        }

        public WorkspaceWidgets(AppWorkspace workspace)
        {
            this.workspace = workspace;
        }
    }
}
