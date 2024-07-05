/////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) dotPDN LLC, Rick Brewster, Tom Jackson, and contributors.     //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See src/Resources/Files/License.txt for full licensing and attribution      //
// details.                                                                    //
// .                                                                           //
/////////////////////////////////////////////////////////////////////////////////

using SciImage.HistoryMementos;
using SciImage.SystemLayer;
using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;

namespace SciImage.Actions
{
    public sealed class AcquireFromScannerOrCameraAction
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
                return "Acquire From Scanner or Camera";
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
                return (System.Windows.Forms.Keys.F9);
            }
        }
        public override int MenuSubGroupIndex
        {
            get { return 1; }
        }
        public override int OrderSuggestion
        {
            get { return 4; }
        }
        public override bool PerformAction(AppWorkspace appWorkspace, List<HistoryMemento> OptionalHistoryRecord, int TargetLayerIndex)
        {
            if (!ScanningAndPrinting.CanScan)
            {
                Utility.ShowWiaError(appWorkspace);
                return false ;
            }

            string tempName = Path.ChangeExtension(SystemLayer.FileSystem.GetTempFileName(), ".bmp");
            ScanResult result;

            try
            {
                result = ScanningAndPrinting.Scan(appWorkspace, tempName);
            }

            // If there was an exception, let's assume the user has already received an error dialog,
            // either from Windows or from the WIA UI, and let's /not/ present another error dialog.
            catch (Exception)
            {
                result = ScanResult.UserCancelled;
            }

            if (result == ScanResult.Success)
            {
                string errorText = null;

                try
                {
                    Image image;

                    try
                    {
                        image = PdnResources.LoadImage(tempName);
                    }

                    catch (FileNotFoundException)
                    {
                        errorText = PdnResources.GetString("LoadImage.Error.FileNotFoundException");
                        throw;
                    }

                    catch (OutOfMemoryException)
                    {
                        errorText = PdnResources.GetString("LoadImage.Error.OutOfMemoryException");
                        throw;
                    }

                    Document document;

                    try
                    {
                        document = Document.FromImage(image);
                    }

                    catch (OutOfMemoryException)
                    {
                        errorText = PdnResources.GetString("LoadImage.Error.OutOfMemoryException");
                        throw;
                    }

                    finally
                    {
                        image.Dispose();
                        image = null;
                    }

                    DocumentWorkspace dw = appWorkspace.AddNewDocumentWorkspace();

                    try
                    {
                        dw.Document = document;
                    }

                    catch (OutOfMemoryException)
                    {
                        errorText = PdnResources.GetString("LoadImage.Error.OutOfMemoryException");
                        throw;
                    }

                    document = null;
                    dw.SetDocumentSaveOptions(null, null, null);
                    dw.History.ClearAll();

                    HistoryMemento newHA = new NullHistoryMemento(
                        PdnResources.GetString("AcquireImageAction.Name"),
                        PdnResources.GetImageResource("Icons.MenuLayersAddNewLayerIcon.png"));
                    if (OptionalHistoryRecord == null)
                        dw.History.PushNewMemento(newHA);
                    else
                        OptionalHistoryRecord.Add(newHA);

                    appWorkspace.ActiveDocumentWorkspace = dw;

                    // Try to delete the temp file but don't worry if we can't
                    try
                    {
                        File.Delete(tempName);
                    }

                    catch
                    {
                    }
                }

                catch (Exception)
                {
                    if (errorText != null)
                    {
                        Utility.ErrorBox(appWorkspace, errorText);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return true ;
        }
    }
}
