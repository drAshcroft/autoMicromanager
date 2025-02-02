/////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) dotPDN LLC, Rick Brewster, Tom Jackson, and contributors.     //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See src/Resources/Files/License.txt for full licensing and attribution      //
// details.                                                                    //
// .                                                                           //
/////////////////////////////////////////////////////////////////////////////////

using System;

namespace SciImage
{
    public abstract class AppWorkspaceAction
    {
        public abstract void PerformAction(AppWorkspace appWorkspace);

        public AppWorkspaceAction()
        {
            SystemLayer.Tracing.LogFeature("AWAction(" + GetType().Name + ")");
        }
    }
}
