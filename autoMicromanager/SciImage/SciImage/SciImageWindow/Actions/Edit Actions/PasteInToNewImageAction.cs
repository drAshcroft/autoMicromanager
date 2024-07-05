/////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) dotPDN LLC, Rick Brewster, Tom Jackson, and contributors.     //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See src/Resources/Files/License.txt for full licensing and attribution      //
// details.                                                                    //
// .                                                                           //
/////////////////////////////////////////////////////////////////////////////////

using SciImage.HistoryMementos;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SciImage.Actions
{
    public sealed class PasteInToNewImageAction
        : SciImage.Actions.PluginAction
    {
        public override string ActionName
        {
            get
            {
                return "Paste in New Image";
            }
        }
        public override System.Drawing.Image MenuImage
        {
            get { return null; }
        }
        public override string MenuName
        {
            get { return "Edit"; }
        }
        public override string SubMenuName
        {
            get { return ""; }
        }
        public override System.Windows.Forms.Keys ShortCutKeys
        {
            get
            {
                return (System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V | System.Windows.Forms.Keys.Alt );
            }
        }
        public override int MenuSubGroupIndex
        {
            get { return 2; }
        }
        public override int OrderSuggestion
        {
            get { return 4; }
        }
        public override PluginAction.ActionDisplayOptions CheckIfEnabled(DocumentWorkspace documentWorkspace)
        {
            if (documentWorkspace == null)
            {
                return ActionDisplayOptions.Visible;
            }
            return PluginAction.ActionDisplayOptions.Visible | ActionDisplayOptions.Enabled;

        }
        public override bool PerformAction(AppWorkspace appWorkspace, List<HistoryMemento> OptionalHistoryRecord, int TargetLayerIndex)
        {
            try
            {
                IDataObject pasted;
                Image image;

                using (new WaitCursorChanger(appWorkspace))
                {
                    Utility.GCFullCollect();
                    pasted = Clipboard.GetDataObject();
                    image = (Image)pasted.GetData(DataFormats.Bitmap);
                }

                if (image == null)
                {
                    Utility.ErrorBox(appWorkspace, PdnResources.GetString("PasteInToNewImageAction.Error.NoClipboardImage"));
                }
                else
                {
                    Size newSize = image.Size;
                    image.Dispose();
                    image = null;
                    pasted = null;

                    Document document = null;

                    using (new WaitCursorChanger(appWorkspace))
                    {
                        document = new Document(newSize);
                        DocumentWorkspace dw = appWorkspace.AddNewDocumentWorkspace();
                        dw.Document = document;

                        dw.History.PushNewMemento(new NullHistoryMemento(string.Empty, null));

                        PasteInToNewLayerAction pitnla = new PasteInToNewLayerAction();
                        bool result = pitnla.PerformAction(appWorkspace,OptionalHistoryRecord,TargetLayerIndex  );

                        if (result)
                        {
                            dw.Selection.Reset();
                            dw.SetDocumentSaveOptions(null, null, null);
                            dw.History.ClearAll();

                            dw.History.PushNewMemento(
                                new NullHistoryMemento(
                                    "New Image",
                                    PdnResources.GetImageResource("Icons.MenuLayersAddNewLayerIcon.png")));

                            appWorkspace.ActiveDocumentWorkspace = dw;
                        }
                        else
                        {
                            appWorkspace.RemoveDocumentWorkspace(dw);
                            document.Dispose();
                        }
                    }
                }
            }

            catch (ExternalException)
            {
                Utility.ErrorBox(appWorkspace, PdnResources.GetString("AcquireImageAction.Error.Clipboard.TransferError"));
                return false ;
            }

            catch (OutOfMemoryException)
            {
                Utility.ErrorBox(appWorkspace, PdnResources.GetString("AcquireImageAction.Error.Clipboard.OutOfMemory"));
                return false ;
            }

            catch (ThreadStateException)
            {
                // The ApartmentState property of the application is not set to ApartmentState.STA
                // I don't think this one will ever happen, seeing as how Main is tagged with the
                // STA attribute.
                return false ;
            }
            return true ;
        }
    }
}
