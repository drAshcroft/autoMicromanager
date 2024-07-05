// DESCRIPTION:   
//                 
//                
// AUTHOR:        Brian Ashcroft, Ashcroft@physics.leidenuniv.nl, 05/27/2009
//
// COPYRIGHT:     Brian Ashcroft
// LICENSE:       This file is distributed under the  MIT license.
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
using System.Runtime.InteropServices;

namespace CoreDevices
{
    /// <summary>
    /// Base of any viewer that can be constructed.  This can just be a viewer or things that are even more interactive.
    /// </summary>
    [Guid("5A88092E-69DF-4bb8-0007-8FA83E550F20")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IPictureView
    {
        void ForceSave(string Filename);
        void SetCore(EasyCore Ecore);
        void SendImage(int Width, int Height, int Stride, int BPP, int ByteSize, IntPtr Data, long MaxPixelValue, long MinPixelValue);
        void SendImage(System.Drawing.Image newImage);
        void SendImage(System.Drawing.Image[] newImages);
        void SendImage(CoreImage newImage);
        void SendImage(CoreImage[] newImages);
        void UpdatesPaused();
    }
}
