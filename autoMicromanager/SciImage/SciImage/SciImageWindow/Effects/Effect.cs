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
using System.Windows.Forms;
using SciImage.IndirectUI;
using SciImage.PropertySystem;
using System.Collections.Generic;

namespace SciImage.Effects
{
    public enum EffectCategory
    {
        /// <summary>
        /// The default category for an effect. This will place effects in to the "Effects" menu.
        /// </summary>
        Effect,

        /// <summary>
        /// Signifies that this effect should be an "Image Adjustment", placing the effect in
        /// the "Adjustments" submenu in the "Layers" menu.
        /// These types of effects are typically quick to execute. They are also preferably 
        /// "unary" (see EffectTypeHint) but are not required to be.
        /// </summary>
        Adjustment,

        /// <summary>
        /// Signifies that this effect should not be displayed in any menu.
        /// </summary>
        DoNotDisplay
    }

    public abstract class Effect 
    {
        protected EffectEnvironmentParameters envParams;
        protected EffectFlags effectFlags;
        private bool setRenderInfoCalled = false;
        protected string name;
        protected Image image;
        protected string subMenuName;
        protected bool SetRenderInfoCalled
        {
            get
            {
                return this.setRenderInfoCalled;
            }
        }


        /// <summary>
        /// Returns the category of the effect. If there is no EffectCategoryAttribute
        /// applied to the runtime type, then the default category, EffectCategory.Effect,
        /// will be returned.
        /// </summary>
        /// <remarks>
        /// This controls which menu in the user interface the effect is placed in to.
        /// </remarks>
        public EffectCategory Category
        {
            get
            {
                object[] attributes = this.GetType().GetCustomAttributes(true);

                foreach (Attribute attribute in attributes)
                {
                    if (attribute is EffectCategoryAttribute)
                    {
                        return ((EffectCategoryAttribute)attribute).Category;
                    }
                }

                return EffectCategory.Effect;
            }
        }

        public EffectEnvironmentParameters EnvironmentParameters 
        {
            get 
            {
                return this.envParams;
            }

            set 
            {
                this.envParams = value;
            }
        }

        public EffectFlags EffectFlags
        {
            get
            {
                return this.effectFlags;
            }
        }

        public bool CheckForEffectFlags(EffectFlags flags)
        {
            return (EffectFlags & flags) == flags;
        }

        public string SubMenuName
        {
            get
            {
                return this.subMenuName;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public Image Image
        {
            get
            {
                return this.image;
            }
        }

        public void SetRenderInfo(EffectConfigToken parameters, RenderArgs dstArgs, RenderArgs srcArgs)
        {
            this.setRenderInfoCalled = true;
            OnSetRenderInfo(parameters, dstArgs, srcArgs);
        }

        protected virtual void OnSetRenderInfo(EffectConfigToken parameters, RenderArgs dstArgs, RenderArgs srcArgs)
        {
        }

        /// <summary>
        /// Performs the effect's rendering. The source is to be treated as read-only,
        /// and only the destination pixels within the given rectangle-of-interest are
        /// to be written to. However, in order to compute the destination pixels,
        /// any pixels from the source may be utilized.
        /// </summary>
        /// <param name="parameters">The parameters to the effect. If IsConfigurable is true, then this must not be null.</param>
        /// <param name="dstArgs">Describes the destination RGB32_Surface.</param>
        /// <param name="srcArgs">Describes the source RGB32_Surface.</param>
        /// <param name="rois">The list of rectangles that describes the region of interest.</param>
        /// <param name="startIndex">The index within roi to start enumerating from.</param>
        /// <param name="length">The number of rectangles to enumerate from roi.</param>
        public abstract void Render(EffectConfigToken parameters, RenderArgs dstArgs, RenderArgs srcArgs, Rectangle[] rois, int startIndex, int length);

        public void Render(EffectConfigToken parameters, RenderArgs dstArgs, RenderArgs srcArgs, Rectangle[] rois)
        {
            Render(parameters, dstArgs, srcArgs, rois, 0, rois.Length);
        }

        public void Render(EffectConfigToken parameters, RenderArgs dstArgs, RenderArgs srcArgs, PdnRegion roi)
        {
            Rectangle[] scans = roi.GetRegionScansReadOnlyInt();
            Render(parameters, dstArgs, srcArgs, scans, 0, scans.Length);
        }

      /*  public virtual EffectConfigDialog CreateConfigDialog()
        {
            if (CheckForEffectFlags(EffectFlags.Configurable))
            {
                throw new NotImplementedException("If IsConfigurable is true, then CreateConfigDialog() must be implemented");
            }
            else
            {
                return null;
            }
        }
        */
       
        /// <summary>
        /// This is a helper function. It allows you to render an effect "in place."
        /// That is, you don't need both a destination and a source Surface.
        /// </summary>
        public void RenderInPlace(RenderArgs srcAndDstArgs, PdnRegion roi)
        {
            using (Surface renderSurface = new Surface(srcAndDstArgs.Surface.Size,srcAndDstArgs.Surface.ColorPixelBase  ))
            {
                using (RenderArgs renderArgs = new RenderArgs(renderSurface))
                {
                    Rectangle[] scans = roi.GetRegionScansReadOnlyInt();
                    Render(null, renderArgs, srcAndDstArgs, scans);
                    srcAndDstArgs.Surface.CopySurface(renderSurface, roi);
                }
            }
        }

        public void RenderInPlace(RenderArgs srcAndDstArgs, Rectangle roi)
        {
            using (PdnRegion region = new PdnRegion(roi))
            {
                RenderInPlace(srcAndDstArgs, region);
            }
        }

        public Effect(string name, Image image)
            : this(name, image, EffectFlags.None)
        {
        }

        public Effect(string name, Image image, EffectFlags flags)
            : this(name, image, null, flags)
        {
        }

        

        public Effect(string name, Image image, string subMenuName)
            : this(name, image, subMenuName, EffectFlags.None)
        {
        }

 

        public Effect(string name, Image image, string subMenuName, EffectFlags effectFlags)
        {
            this.name = name;
            this.image = image;
            this.subMenuName = subMenuName;
            this.effectFlags = effectFlags;
            this.envParams = EffectEnvironmentParameters.DefaultParameters;
        }


        protected abstract PropertyCollection OnCreatePropertyCollection();

        public PropertyCollection CreatePropertyCollection()
        {
            return OnCreatePropertyCollection();
        }

        protected virtual ControlInfo OnCreateConfigUI(PropertyCollection props)
        {
            return CreateDefaultConfigUI(props);
        }

        public ControlInfo CreateConfigUI(PropertyCollection props)
        {
            PropertyCollection props2 = props.Clone();

            using (props2.__Internal_BeginEventAddMoratorium())
            {
                ControlInfo configUI1 = OnCreateConfigUI(props2);
                ControlInfo configUI2 = configUI1.Clone();
                return configUI2;
            }
        }

        public static ControlInfo CreateDefaultConfigUI(IEnumerable<Property> props)
        {
            PanelControlInfo configUI = new PanelControlInfo();

            foreach (Property property in props)
            {
                PropertyControlInfo propertyControlInfo = PropertyControlInfo.CreateFor(property);
                propertyControlInfo.ControlProperties[ControlInfoPropertyNames.DisplayName].Value = property.Name;
                configUI.AddChildControl(propertyControlInfo);
            }

            return configUI;
        }

        private string GetConfigDialogTitle()
        {
            return this.Name;
        }

        private Icon GetConfigDialogIcon()
        {
            Image image = this.Image;

            Icon icon = null;

            if (image != null)
            {
                icon = Utility.ImageToIcon(image);
            }

            return icon;
        }

        protected virtual void OnCustomizeConfigUIWindowProperties(PropertyCollection props)
        {
            return;
        }

        public EffectConfigDialog CreateConfigDialog()
        {
            PropertyCollection props1 = CreatePropertyCollection();
            PropertyCollection props2 = props1.Clone();
            PropertyCollection props3 = props1.Clone();

            ControlInfo configUI2 = CreateConfigUI(props2);
            //ControlInfo configUI2 = configUI1.Clone();

            PropertyCollection windowProps = EffectConfigDialog.CreateWindowProperties();
            windowProps[ControlInfoPropertyNames.WindowTitle].Value = this.Name;
            OnCustomizeConfigUIWindowProperties(windowProps);
            PropertyCollection windowProps2 = windowProps.Clone();

            EffectConfigDialog pbecd = new EffectConfigDialog(props3, configUI2, windowProps2);

            pbecd.Icon = GetConfigDialogIcon();

            return pbecd;
        }

       
    }
}
