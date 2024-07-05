/////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) dotPDN LLC, Rick Brewster, Tom Jackson, and contributors.     //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See src/Resources/Files/License.txt for full licensing and attribution      //
// details.                                                                    //
// .                                                                           //
/////////////////////////////////////////////////////////////////////////////////

using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace SciImage.Actions
{
    public sealed class SendFeedbackAction
        : SciImage.Actions.PluginAction
    {
        public override string ActionName
        {
            get
            {
                return "Send Feedback";
            }
        }
        public override System.Drawing.Image MenuImage
        {
            get { return null; }
        }
        public override string MenuName
        {
            get { return "Help"; }
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
            get { return 2; }
        }
        public override int OrderSuggestion
        {
            get { return 6; }
        }
        private string GetEmailLaunchString(string email, string subject, string body)
        {
            const string emailFormat = "mailto:{0}?subject={1}&body={2}";
            string bodyUE = body.Replace("\r\n", "%0D%0A");
            string launchString = string.Format(emailFormat, email, subject, bodyUE);
            return launchString;
        }
        public override PluginAction.ActionDisplayOptions CheckIfEnabled(DocumentWorkspace documentWorkspace)
        {

            return ActionDisplayOptions.Visible | ActionDisplayOptions.Enabled;
        }
        public override bool PerformAction(AppWorkspace appWorkspace, List<HistoryMemento> OptionalHistoryRecord, int TargetLayerIndex)
        {
            string email = InvariantStrings.FeedbackEmail;
            string subjectFormat = PdnResources.GetString("SendFeedback.Email.Subject.Format");
            string subject = string.Format(subjectFormat, PdnInfo.GetFullAppName());
            string body = PdnResources.GetString("SendFeedback.Email.Body");
            string launchMe = GetEmailLaunchString(email, subject, body);
            launchMe = launchMe.Substring(0, Math.Min(1024, launchMe.Length));

            try
            {
                Process.Start(launchMe);
            }
                 
            catch (Exception)
            {
                Utility.ErrorBox(appWorkspace, PdnResources.GetString("LaunchLink.Error"));
            }
            return true ;
        }

        public SendFeedbackAction()
        {
        }
    }
}
