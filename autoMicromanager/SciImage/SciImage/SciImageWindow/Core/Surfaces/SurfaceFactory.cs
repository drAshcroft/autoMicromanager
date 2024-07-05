using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SciImage.Core.Surfaces
{
    public class SurfaceFactory
    {
        public static Surface ConvertSurface(Surface Source, Core.ColorsAndPixelOps.ColorPixelBase DestinationFormat)
        {
            return Source;
        }
    }
}
