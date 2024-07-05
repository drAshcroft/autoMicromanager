/////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) dotPDN LLC, Rick Brewster, Tom Jackson, and contributors.     //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See src/Resources/Files/License.txt for full licensing and attribution      //
// details.                                                                    //
// .                                                                           //
/////////////////////////////////////////////////////////////////////////////////

using SciImage.SystemLayer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using SciImage.Core.ColorsAndPixelOps;

namespace SciImage.Actions
{
    public sealed class PrintAction
        : SciImage.Actions.PluginAction
    {
        public override string ActionName
        {
            get
            {
                return "Print";
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
                return (System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P);
            }
        }
        public override int MenuSubGroupIndex
        {
            get { return 3; }
        }
        public override int OrderSuggestion
        {
            get { return 1; }
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
            DocumentWorkspace documentWorkspace = appWorkspace.ActiveDocumentWorkspace;
            if (!ScanningAndPrinting.CanPrint)
            {
                Utility.ShowWiaError(documentWorkspace);
                return false ;
            }

            using (new PushNullToolMode(documentWorkspace))
            {
                // render image to a bitmap, save it to disk
                Surface scratch = documentWorkspace.BorrowScratchSurface(this.GetType().Name + ".PerformAction()");

                try
                {
                    scratch.Clear();
                    RenderArgs ra = new RenderArgs(scratch);

                    documentWorkspace.Update();

                    using (new WaitCursorChanger(documentWorkspace))
                    {
                        ra.Surface.Clear(scratch.ColorPixelBase.WhiteColor());
                        documentWorkspace.Document.Render(ra, false);
                    }

                    string tempName = Path.GetTempFileName() + ".bmp";
                    ra.Bitmap.Save(tempName, ImageFormat.Bmp);

                    try
                    {
                        ScanningAndPrinting.Print(documentWorkspace, tempName);
                    }

                    catch (Exception ex)
                    {
                        Utility.ShowWiaError(documentWorkspace);
                        Tracing.Ping(ex.ToString());
                        // TODO: do a "better" error dialog here
                    }

                    // Try to delete the temp file but don't worry if we can't
                    bool result = FileSystem.TryDeleteFile(tempName);
                }

                finally
                {
                    documentWorkspace.ReturnScratchSurface(scratch);
                }
            }

            return true ;
        }

        public PrintAction()
            : base(ActionFlags.KeepToolActive)
        {
        }
    }
}
