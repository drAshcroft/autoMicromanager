/////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) dotPDN LLC, Rick Brewster, Tom Jackson, and contributors.     //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See src/Resources/Files/License.txt for full licensing and attribution      //
// details.                                                                    //
// .                                                                           //
/////////////////////////////////////////////////////////////////////////////////

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SciImage.Actions
{
    public sealed class CloseWorkspaceAction
        : SciImage.Actions.PluginAction
    {
        public override PluginAction.ActionDisplayOptions CheckIfEnabled(DocumentWorkspace documentWorkspace)
        {

            return ActionDisplayOptions.Visible | ActionDisplayOptions.Enabled;
        }
        public override string ActionName
        {
            get
            {
                return "Close Document";
            }
        }
        public override System.Drawing.Image MenuImage
        {
            get { return null; }
        }
        public override string MenuName
        {
            get { return "File"; }
        }
        public override string SubMenuName
        {
            get { return ""; }
        }
        public override System.Windows.Forms.Keys ShortCutKeys
        {
            get
            {
                return (System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W);
            }
        }
        public override int MenuSubGroupIndex
        {
            get { return 1; }
        }
        public override int OrderSuggestion
        {
            get { return 5; }
        }
        public override bool PerformAction(AppWorkspace appWorkspace, List<HistoryMemento> OptionalHistoryRecord, int TargetLayerIndex)
        {
            if (appWorkspace == null)
            {
                throw new ArgumentNullException("appWorkspace");
            }

            DocumentWorkspace dw;

           
            dw = appWorkspace.ActiveDocumentWorkspace;
            

            if (dw != null)
            {
                if (dw.Document == null)
                {
                    appWorkspace.RemoveDocumentWorkspace(dw);
                }
                else if (!dw.Document.Dirty)
                {
                    appWorkspace.RemoveDocumentWorkspace(dw);
                }
                else
                {
                    appWorkspace.ActiveDocumentWorkspace = dw;

                    TaskButton saveTB = new TaskButton(
                        PdnResources.GetImageResource("Icons.MenuFileSaveIcon.png").Reference,
                        "&Save",
                        "Save Image before exiting");

                    TaskButton dontSaveTB = new TaskButton(
                        PdnResources.GetImageResource("Icons.MenuFileCloseIcon.png").Reference,
                        "Do Not Save",
                        "Discard Image");

                    TaskButton cancelTB = new TaskButton(
                        PdnResources.GetImageResource("Icons.CancelIcon.png").Reference,
                        "Cancel Close",
                        "Do not close Program");

                    string title = "There are unsaved changed what would you like to do.";
                    string introTextFormat = "You have not saved {0} . ";
                    string introText = string.Format(introTextFormat, dw.GetFriendlyName());

                    Image thumb = appWorkspace.GetDocumentWorkspaceThumbnail(dw);

                    if (thumb == null)
                    {
                        thumb = new Bitmap(32, 32);
                    }

                    Bitmap taskImage = new Bitmap(thumb.Width + 2, thumb.Height + 2, PixelFormat.Format32bppArgb);

                    using (Graphics g = Graphics.FromImage(taskImage))
                    {
                        g.Clear(Color.Transparent);

                        g.DrawImage(
                            thumb, 
                            new Rectangle(1, 1, thumb.Width, thumb.Height), 
                            new Rectangle(0, 0, thumb.Width, thumb.Height), 
                            GraphicsUnit.Pixel);

                        Utility.DrawDropShadow1px(g, new Rectangle(0, 0, taskImage.Width, taskImage.Height));
                    }

                    Form mainForm = appWorkspace.FindForm();
                    if (mainForm != null)
                    {
                        PdnBaseForm asPDF = mainForm as PdnBaseForm;

                        if (asPDF != null)
                        {
                            asPDF.RestoreWindow();
                        }
                    }

                    Icon warningIcon;
                    ImageResource warningIconImageRes = PdnResources.GetImageResource("Icons.WarningIcon.png");

                    if (warningIconImageRes != null)
                    {
                        Image warningIconImage = warningIconImageRes.Reference;
                        warningIcon = Utility.ImageToIcon(warningIconImage, false);
                    }
                    else
                    {
                        warningIcon = null;
                    }                     

                    TaskButton clickedTB = TaskDialog.Show(
                        appWorkspace,
                        warningIcon,
                        title,
                        taskImage,
                        false,
                        introText,
                        new TaskButton[] { saveTB, dontSaveTB, cancelTB },
                        saveTB,
                        cancelTB,
                        340);                        

                    if (clickedTB == saveTB)
                    {
                        if (dw.DoSave())
                        {
                            
                            appWorkspace.RemoveDocumentWorkspace(dw);
                        }
                        else
                        {
                            
                        }
                    }
                    else if (clickedTB == dontSaveTB)
                    {
                       
                        appWorkspace.RemoveDocumentWorkspace(dw);
                    }
                    else
                    {
                        
                    }
                }
            }

            Utility.GCFullCollect();
            return true ;
        }

        public CloseWorkspaceAction()
        {
        }

      
    }
}
