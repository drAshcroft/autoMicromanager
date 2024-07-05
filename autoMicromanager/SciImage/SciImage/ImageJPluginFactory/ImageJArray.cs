///////////////////////////////////////////////////////////////////////////////
// 
// PROJECT:       Micro-Manager
// SUBSYSTEM:     Effects
//-----------------------------------------------------------------------------
// DESCRIPTION:   An Experiment to link code to ImageJ using memorymaps
//                 
//                
// AUTHOR:        Brian Ashcroft, Ashcroft@physics.leidenuniv.nl, 05/27/2009
//
// COPYRIGHT:     Brian Ashcroft
// LICENSE:       This file is distributed under the BSD license.
//                License text is included with the source distribution.
//
//                This file is distributed in the hope that it will be useful,
//                but WITHOUT ANY WARRANTY; without even the implied warranty
//                of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
//
//                IN NO EVENT SHALL THE COPYRIGHT OWNER OR
//                CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
//                INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SciImage.Effects;
namespace ImageJPluginFactory
{
    public class ImageJArray: IEffectTypeArray 
    {

        public static readonly ImageJInterface Smooth = new ImageJInterface("Smooth");
        public static readonly ImageJInterface Sharpen = new ImageJInterface("Sharpen");
        public static readonly ImageJInterface EnhanceContrast = new ImageJInterface("Enhance Contrast");
        public static readonly ImageJInterface FindEdges = new ImageJInterface("Find Edges");

        private static Effect[] EffectTypes = new Effect [] { Smooth,Sharpen,EnhanceContrast,FindEdges };

        

        public Effect[] GetEffectTypeInstances()
        {
            return (Effect[])EffectTypes.Clone();
        }
        
    }
}
