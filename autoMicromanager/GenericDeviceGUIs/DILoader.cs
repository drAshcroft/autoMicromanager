using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GenericDeviceGUIs
{
    public class DILoader
    {
        public static double GetNumber(string inNumber)
        {
            string[] parts = inNumber.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            double d = 0;
            foreach (string s in parts)
            {
                double.TryParse(s, out d);
                if (d != 0)
                    return d;

            }
            return 0;

        }
        public static Int16 [,] LoadDI(string Filename)
        {

            byte junk = 0;
            string Line = null;
            bool TextFormat = false;
            int I = 0;
            bool DataFound = false;
            double HardScale = 0;
            double SoftScale = 0;
            int ImageWidth = 0;
            int ImageHeight = 0;
            int DataOffset = 0;
            int DataLength = 0;
            double PixelSize = 0;
            FileStream fs = File.OpenRead(Filename);

            BinaryReader IVSFile = new BinaryReader(fs);

            Line = "";
            string[] parts = null;
            bool EOF = false;

            while ((!EOF) & (!DataFound))
            {
                try
                {
                    junk = IVSFile.ReadByte();
                }
                catch
                {
                    EOF = true;
                }
                if (junk == 10)
                {
                    if (Line.Contains("\\@2:Z scale:") && HardScale == 0)
                    {
                        HardScale = GetNumber(Line);
                    }
                    if (Line.Contains("\\Bytes/pixel:") && PixelSize == 0)
                    {
                        PixelSize = GetNumber(Line);
                    }
                    if (Line.Contains("\\Samps/line:") && ImageWidth == 0)
                    {
                        ImageWidth = (int)GetNumber(Line);
                    }
                    if (Line.Contains("\\Number of lines:") && ImageHeight == 0)
                    {
                        ImageHeight = (int)GetNumber(Line);
                    }
                    if (Line.Contains("\\@Sens. Zscan:") && SoftScale == 0)
                    {
                        SoftScale = GetNumber(Line);
                    }

                    if (Line.Contains("\\Data offset:") && DataOffset == 0)
                    {
                        DataOffset = (int)GetNumber(Line);
                    }
                    if (Line.Contains("\\Data length:") && DataLength == 0)
                    {
                        DataLength = (int)GetNumber(Line);
                    }
                    if (Line.Contains("\\*File list end"))
                    {
                        DataFound = true;
                    }
                    Line = "";
                }
                else
                {
                    Line = Line + (char)junk;
                }

            }

            IVSFile.BaseStream.Position = DataOffset;
            Int16 [,] Data = new Int16  [ImageWidth, ImageHeight];
            double[] LineAverage = new double[ImageHeight];
            Int16 getInt = 0;
            Int16 FirstInt = 0;
            int x = 0;
            int y = 0;
            for (int i = 0; i < ImageWidth * ImageHeight; i++)
            {
                getInt = IVSFile.ReadInt16();
                if (x == 0) FirstInt = getInt;
                Data[x,y] = (Int16 )(getInt - FirstInt);
                LineAverage[y] +=  (getInt - FirstInt);
                x++;
                if (x >= ImageWidth)
                {
                    x = 0;
                    y++;

                }
            }
            Int16    MinI = Int16.MaxValue ;
            for (int i = 0; i < ImageHeight; i++)
            {
                double m =  (Data[ImageWidth - 1, i] - Data[0, i]) / ImageWidth;
                double b =  LineAverage[i] / ImageWidth;
                for (x = 0; x < ImageHeight; x++)
                {
                    Data[x, i] =(Int16) ( Data[x, i] - (m * x + b));
                    if (Data[x, i] < MinI) MinI =(Int16) Data[x, i];
                }
            }
            //Int16[,] OutData = new Int16[ImageWidth, ImageHeight];
            for (x = 0; x < ImageWidth; x++)
                for (y = 0; y < ImageHeight; y++)
                    Data[x, y] =(Int16)( Data[x, y] - MinI+1);

            for (x = 0; x < ImageWidth; x++)
                for (y = 0; y < ImageHeight; y++)
                    Data[y, x] = Data[x, y];
            IVSFile.Close();
            fs.Close();
            return Data ;

        }
    }
}
