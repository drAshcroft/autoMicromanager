using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using FreeImageAPI;

namespace Micromanager_net

{

    class FreeImageHelper
    {

        [DllImport("FreeImage.dll", EntryPoint="FreeImage_AllocateT")]
        public static extern IntPtr FreeImage_AllocateT(FREE_IMAGE_TYPE type, int width, int height,
            int bpp , int red_mask, int  green_mask , int  blue_mask );

    }
}
