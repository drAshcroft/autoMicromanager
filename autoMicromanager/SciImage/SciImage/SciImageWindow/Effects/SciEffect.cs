
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SciImage.Effects
{
    public abstract class SciEffect : Effect, ICloneable
    {
        private bool useTileImageRender = true;
        private bool cloneEffectToUse = false;
        public bool UseTileImageRender
        {
            get { return useTileImageRender; }
        }
        public bool CloneEffectToUse
        {
            get { return cloneEffectToUse; }
        }

        public abstract object Clone();

        public abstract string End(string[] AlreadyEndedKeys);

        public SciEffect(string name, Image image)
            : this(name, image, EffectFlags.None)
        {
        }

        public SciEffect(string name, Image image, EffectFlags flags)
            : this(name, image, null, flags)
        {
        }

        public SciEffect(string name, Image image, string subMenuName)
            : this(name, image, subMenuName, EffectFlags.None)
        {
        }

        public SciEffect(string name, Image image, string subMenuName, EffectFlags effectFlags, bool UseTileImageRendering, bool CloneEffectToUse)
            : base(name, image, subMenuName, effectFlags)
        {
            useTileImageRender = UseTileImageRendering;
            cloneEffectToUse = CloneEffectToUse;
        }
        public SciEffect(string name, Image image, string subMenuName, EffectFlags effectFlags)
            : base(name, image, subMenuName, effectFlags)
        {
        }
    }
}

