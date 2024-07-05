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
using System.Windows.Forms;
using System.Collections.Generic;
using SciImage.Actions;

namespace SciImage.Menus
{
    
    public sealed class PdnMainMenu
        : MenuStripEx
    {
        private List< PdnMenuItem > MenuItems;
        
        private AppWorkspace appWorkspace;

        private bool OnCtrlF4Typed(Keys keys)
        {
            appWorkspace.PerformAction(new CloseAllWorkspacesAction());
            return true;
        }

        public AppWorkspace AppWorkspace
        {
            get
            {
                return this.appWorkspace;
            }

            set
            {
                this.appWorkspace = value;
                if (value != null)
                {
                    
                    Dictionary<string, List<ActionFactory.ActionLibEntry>> MenuStructure = Actions.ActionFactory.CreateMenuStructure();

                    List<PdnMenuItem> MainMenu = new List<PdnMenuItem>();
                    foreach (KeyValuePair<string, List<ActionFactory.ActionLibEntry>> kvp in MenuStructure)
                    {
                        List<ActionFactory.ActionLibEntry> laa = kvp.Value;
                        PdnMenuItem pmi = new PdnMenuItem();
                        pmi.Name = kvp.Key;
                        pmi.Text = kvp.Key;
                        pmi.AppWorkspace = appWorkspace;
                        List<PdnMenuItem> subMenu = new List<PdnMenuItem>();
                        int LastSubGroupIndex = 0;
                        foreach (ActionFactory.ActionLibEntry ale in laa)
                        {
                            PdnMenuItem subPmi = new PdnMenuItem();
                            subPmi.Image = ale.MenuImage;
                            subPmi.Name = ale.ActionName;
                            subPmi.MenuAction = ale.Action;
                            if (ale.ShortCutKeys != Keys.F9)
                            {
                                subPmi.ShortcutKeys = ale.ShortCutKeys;
                                subPmi.ShortcutKeyDisplayString = ConvertShortcutKeyToString(ale.ShortCutKeys);
                            }
                            if (ale.MenuSubGroupIndex != LastSubGroupIndex)
                            {
                                LastSubGroupIndex = ale.MenuSubGroupIndex;
                                pmi.DropDownItems.Add(new ToolStripSeparator());
                            }
                            subPmi.Click += new EventHandler(MenuItem_Click);
                            subPmi.AppWorkspace = appWorkspace;
                            pmi.DropDownItems.Add(subPmi);
                            pmi.DropDownOpening += new EventHandler(MenuItem_DropDownOpening);
                        }
                        //System.Diagnostics.Debug.Print( subMenu[1].Text );
                        //Range(subMenu.ToArray() );

                        if (pmi.Text.Trim() != "")
                            MainMenu.Add(pmi);

                    }

                    List<PdnMenuItem> DecematedMainMenu = new List<PdnMenuItem>();

                    foreach (PdnMenuItem pmi in MainMenu)
                    {
                        ToolStripItem[] tsi = this.Items.Find(pmi.Name, true);
                        if (tsi == null || tsi.Length == 0)
                            DecematedMainMenu.Add(pmi);
                    }
                    
                    AddMenuItem(GetMenuItem("File",  DecematedMainMenu));
                    AddMenuItem(GetMenuItem("Edit", DecematedMainMenu));

                    ViewMenu viewMenu = new ViewMenu();
                    viewMenu.AppWorkspace = appWorkspace;
                    //viewMenu.PopulateMenu( GetMenuItem("View", DecematedMainMenu));
                    //this.AddMenuItem(viewMenu);
                    PdnMenuItem viewPMI = GetMenuItem("View", DecematedMainMenu);
                    viewPMI.DropDownItems.AddRange(viewMenu.ViewItems());
                    AddMenuItem(viewPMI );


                    AddMenuItem(GetMenuItem("Image", DecematedMainMenu));
                    AddMenuItem(GetMenuItem("Layers", DecematedMainMenu));


                    PdnMenuItem Apmi= GetMenuItem("Adjustments",  DecematedMainMenu);
                    PdnMenuItem Epmi= GetMenuItem("Effects",  DecematedMainMenu);
                    PdnMenuItem Wpmi= GetMenuItem("Windows", DecematedMainMenu);

                   
                    ToolStripItem[] wtsi = this.Items.Find("Adjustments", true);
                    if (wtsi == null || wtsi.Length == 0)
                    {
                        AdjustmentsMenu am=new AdjustmentsMenu ();
                        am.AppWorkspace = appWorkspace;
                        am.PopulateEffects();
                        if (Apmi!=null)
                            am.AddSubMenuItems(Apmi);
                        am.Name = "Adjustments";
                        am.Text = "Adjustments";
                        am.AppWorkspace = appWorkspace;
                        this.Items.Add(am);
                    }
                    wtsi = this.Items.Find("Effects", true);
                    if (wtsi == null || wtsi.Length == 0)
                    {
                        EffectsMenu em = new EffectsMenu();
                        em.AppWorkspace = appWorkspace;
                        em.PopulateEffects();
                        if (Epmi!=null)
                            em.AddSubMenuItems(Epmi);
                        em.Name = "Effects";
                        em.Text = "Effects";
                        em.AppWorkspace = appWorkspace;
                        this.Items.Add(em);
                    }
                    wtsi = this.Items.Find("Windows", true);
                    if (wtsi == null || wtsi.Length == 0)
                    {
                        WindowMenu wm = new WindowMenu();
                        wm.AppWorkspace = appWorkspace;
                        if (Wpmi !=null)
                            wm.AddSubMenuItems(Wpmi);
                        wm.Name = "Windows";
                        wm.Text = "Windows";
                        wm.AppWorkspace = appWorkspace;
                        this.Items.Add(wm);
                    }

                    

                    PdnMenuItem Hpmi= GetMenuItem("Help",  DecematedMainMenu);
                    this.Items.AddRange(DecematedMainMenu.ToArray());
                    this.Items.Add(Hpmi);
                }
            }
        }

        void MenuItem_DropDownOpening(object sender, EventArgs e)
        {
            PdnMenuItem MenuItem = (PdnMenuItem)sender;
            foreach (object  O in MenuItem.DropDownItems )
            {
                if (O is PdnMenuItem )
                {
                    PdnMenuItem pmi = (PdnMenuItem)O;
                    Type action = pmi.MenuAction;
                    if (action != null)
                    {
                        SciImage.Actions.PluginAction performMe = (Actions.PluginAction)Activator.CreateInstance((Type)action);
                        PluginAction.ActionDisplayOptions ado = performMe.CheckIfEnabled(appWorkspace.ActiveDocumentWorkspace);
                        if ((ado & PluginAction.ActionDisplayOptions.Hidden) == PluginAction.ActionDisplayOptions.Hidden)
                        {
                            pmi.Visible = false;
                        }
                        else
                        {
                            if ((ado & PluginAction.ActionDisplayOptions.Enabled) == PluginAction.ActionDisplayOptions.Enabled)
                            {
                                pmi.Enabled = true;
                            }
                            else
                                pmi.Enabled = false;
                            if ((ado & PluginAction.ActionDisplayOptions.Visible ) == PluginAction.ActionDisplayOptions.Visible )
                            {
                                pmi.Visible  = true;
                            }
                            else
                                pmi.Visible = false;
                        }
                    }
                }
            }
        }

        void MenuItem_Click(object sender, EventArgs e)
        {
            PdnMenuItem pmi = (PdnMenuItem)sender;
            if (pmi.MenuAction != null)
            {
                appWorkspace.PerformAction(pmi.MenuAction);
            }
        }

       
        private void AddMenuItem(PdnMenuItem pmi)
        {
            if (pmi != null)
                this.Items.Add(pmi);
        }
        private PdnMenuItem GetMenuItem(string MenuName,  List<PdnMenuItem> OriginalMainMenu)
        {
            foreach (PdnMenuItem pmi in OriginalMainMenu)
            {
                if (pmi.Name == MenuName )
                {
                    
                    OriginalMainMenu.Remove(pmi);
                    return pmi;
                    
                }
            }
            return null;
        }


        private string ConvertShortcutKeyToString(Keys ShortCutKey)
        {
            string displayString = "";
            if ((ShortCutKey & Keys.Control) == Keys.Control)
                displayString = "Ctrl+";
            if ((ShortCutKey & Keys.Alt) == Keys.Alt)
                displayString += "Alt+";
            if ((ShortCutKey & Keys.Shift) == Keys.Shift)
                displayString += "Shift+";
            displayString += ShortCutKey.ToString();
            displayString = displayString.Replace(", Control", "");
            displayString = displayString.Replace(", Alt", "");
            displayString = displayString.Replace(", Shift", "");
            displayString = displayString.Replace("Subtract", "(-)");
            displayString = displayString.Replace("Add", "(+)");
            return displayString;
        }
       

        public PdnMainMenu()
        {
            InitializeComponent();
            PdnBaseForm.RegisterFormHotKey(Keys.Control | Keys.F4, OnCtrlF4Typed);
        }

        private void InitializeComponent()
        {
           
            SuspendLayout();
            //
            // PdnMainMenu
            //
            this.Name = "PdnMainMenu";
            this.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
           
            ResumeLayout();
        }
    }

}
