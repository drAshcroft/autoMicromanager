using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace SciImage.Actions
{
    public class ActionFactory
    {
        public class ActionLibEntry
        {
            /// <summary>
            /// This is the heading that shows in the top of the page
            /// </summary>
            public string MenuName;
            
            /// <summary>
            /// If this belongs to a submenu, name it here
            /// </summary>
            public string SubMenuName;
            
            /// <summary>
            /// In order to get everything into their little boxes, give the index here
            /// </summary>
            public int MenuSubGroupIndex;
            
            /// <summary>
            /// This is the same that will be used in the scripting and macro dialog
            /// </summary>
            public string ActionName;
            
            /// <summary>
            /// If a menu image is required, place it here
            /// </summary>
            public Image MenuImage;

            public double OrderParam
            {
                get { return MenuSubGroupIndex + .01 * OrderSuggestion; }
            }

            /// <summary>
            /// Assign any shortcut keys here.  This can cause conflicts, so check the already used
            /// keys manually.  A second application will overwrite the first so do not use common 
            /// keys such as ctrl+c...
            /// </summary>
            public Keys ShortCutKeys;
            public int OrderSuggestion;

            public Type Action;
            public ActionLibEntry(string MenuName, string SubMenuName, int MenuSubGroupIndex,
                string ActionName,Image MenuImage,Keys ShortCutKeys,int OrderSuggestion, Type Action)
            {
                this.MenuName = MenuName;
                this.SubMenuName = SubMenuName;
                this.MenuSubGroupIndex = MenuSubGroupIndex;
                this.MenuImage = MenuImage;
                this.ActionName = ActionName;
                this.ShortCutKeys = ShortCutKeys;
                this.OrderSuggestion = OrderSuggestion;
                this.Action=Action;
            }
            public ActionLibEntry(Type Action)
            {
                this.Action = Action;

                ConstructorInfo ci = Action.GetConstructor(Type.EmptyTypes);
                PluginAction PA = (PluginAction)ci.Invoke(null);
                            

                this.MenuName = PA.MenuName ;
                this.SubMenuName =PA.SubMenuName ;
                this.MenuSubGroupIndex = PA.MenuSubGroupIndex;
                this.MenuImage = PA.MenuImage;
                this.ActionName = PA.ActionName;
                this.ShortCutKeys = PA.ShortCutKeys;
                this.OrderSuggestion =PA. OrderSuggestion;

                PA = null;
            }

        }

        private static  List<ActionLibEntry> LoadedActions = new List<ActionLibEntry>();

        public static  Dictionary<string, List<ActionFactory.ActionLibEntry>> CreateMenuStructure()
        {
            List<ActionFactory.ActionLibEntry> Actions = SciImage.Actions.ActionFactory.GetAllAvailableActions;
            Dictionary<string,List<ActionFactory.ActionLibEntry>> MenuStructure=
                new Dictionary<string,List<ActionFactory.ActionLibEntry>>();
            foreach (ActionFactory.ActionLibEntry ale in Actions)
            {
                List<ActionFactory.ActionLibEntry > Menu;
                if (MenuStructure.ContainsKey( ale.MenuName ) ==true )
                {
                    Menu =MenuStructure [ale.MenuName ];
                    MenuStructure.Remove(ale.MenuName );
                }
                else 
                {
                    Menu=new List<ActionLibEntry>();
                    
                }
                Menu.Add (ale);
                MenuStructure.Add(ale.MenuName,Menu);
            }

            foreach (List<ActionFactory.ActionLibEntry> laa in MenuStructure.Values )
            {
                laa.Sort(delegate(ActionFactory.ActionLibEntry a1, ActionFactory.ActionLibEntry a2)
                    { return a1.OrderParam.CompareTo(a2.OrderParam ); });
            }

            return MenuStructure;
        }

        public static  string[] GetAllAvailableActionNames
        {
            get
            {
                List<string> names = new List<string>();
                foreach (ActionLibEntry ale in LoadedActions)
                    names.Add(ale.ActionName);
                return names.ToArray();
            }
                 
        }
        public static  List<ActionLibEntry> GetAllAvailableActions
        {
            get
            {
                return LoadedActions;
            }
        }

        static  ActionFactory ()
        {
            FindActions();
        }

        private static  void FindActions()
        {

            List<Assembly> assemblies = new List<Assembly>();

            
            assemblies.Add(Assembly.GetAssembly(typeof(PluginAction )));

            // TARGETDIR\Effects\*.dll
            string homeDir = Assembly.GetExecutingAssembly().CodeBase ;// System.IO.Path.GetDirectoryName(PluginPath);


            bool dirExists;

            try
            {
                dirExists = Directory.Exists(homeDir);
            }
            catch
            {
                dirExists = false;
            }

            if (dirExists)
            {
                string fileSpec = "*.dll";
                string[] filePaths = Directory.GetFiles(homeDir, fileSpec);

                foreach (string filePath in filePaths)
                {
                    Assembly pluginAssembly = null;

                    try
                    {
                        pluginAssembly = Assembly.LoadFrom(filePath);
                        assemblies.Add(pluginAssembly);
                    }
                    catch
                    {
                        //Tracing.Ping("Exception while loading " + filePath + ": " + ex.ToString());
                    }
                }

            }
            
            foreach (Assembly assembly in assemblies)
            {
                foreach (Type type in assembly.GetTypes())
                {

                    //System.Diagnostics.Debug.Print(type.ToString());
                    try
                    {
                        if (typeof(PluginAction).IsAssignableFrom(type))
                        {
                            LoadedActions.Add(new ActionLibEntry(type) );
                        }
                    }
                    catch
                    {
                    }
                }
            }
            
        }
    }
}
