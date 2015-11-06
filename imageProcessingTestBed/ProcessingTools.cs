using Emgu.CV;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imageProcessingTestBed
{
    class ProcessingTools
    {
        public static System.Drawing.Rectangle findTextEdge<TColor, TDepth>(Mat inImage, TDepth[] channelThresh)
            where TColor : struct, IColor
            where TDepth : IComparable<TDepth>, new()
        {
            if (inImage.NumberOfChannels < channelThresh.Length)
            {  throw new IndexOutOfRangeException("Array exceeds number of channels in image."); }


            Image<TColor, TDepth> tempImage = inImage.ToImage<TColor, TDepth>();

            int rectX = 0;
            int rectY = 0;
            int rectWidth = 0;
            int rectHeight = 0;

            int rectX2 = 0;
            int rectY2 = 0;

            for (int iColumn = 0; iColumn < tempImage.Cols; iColumn++)
            {
                for (int iRow = 0; iRow < tempImage.Rows; iRow++)
                {
                    bool pixelPass = true;

                    for (int iChannel = 0; iChannel < channelThresh.Length; iChannel++)
                    {
                        if (channelThresh[iChannel].CompareTo(tempImage.Data[iRow, iColumn, iChannel]) >= 0)
                        {
                            pixelPass = false;
                            break;
                        } 
                        if(pixelPass)
                        {
                            if(rectX == 0) { rectX = iColumn; } //Left to right so set once.
                            rectX2 = iColumn;

                            if(rectY < iRow) { rectY = iRow; }
                            if(rectY2 > iRow) { rectY2 = iRow; }
                            
                        }
                    }
                }
            }
            rectY = tempImage.Rows - rectY - 1;
            rectY2 = tempImage.Rows - rectY2 - 1;
            //Console.WriteLine("Rect with x:{0}, y:{1}, x2:{2}, y2:{3}",
            //    rectX,
            //    rectY,
            //    rectX2,
            //    rectY2);
            return (new System.Drawing.Rectangle(rectX, rectY, rectX2 - rectX, rectY2 - rectY));
            //Find the top left
            //for (int width = 0; width < tempImage.Width; width++)
            //{
            //    bool done = false;
            //    for (int height = 0; height < tempImage.Height; height++)
            //    {
            //        bool pixelPass = true;
            //        for (int iChannel = 0; iChannel < channelThresh.Length; iChannel++)
            //        {
            //            if (channelThresh[iChannel].CompareTo(tempImage.Data[height, width, iChannel]) >= 0)
            //            {
            //                pixelPass = false;
            //                break;
            //            }
            //        }

            //        if (pixelPass)
            //        {
            //            rectX = width;
            //            rectY = height;
            //            done = true;
            //            break;
            //        }

            //    }
            //    if (done) { break; }
            //}
            ////Starting from the top left, continue and find the last value
            //int botRightX = rectX;
            //int botRightY = rectY;

            //for (int width = rectX; width < tempImage.Width; width++)
            //{

            //    for (int height = rectY; height < tempImage.Height; height++)
            //    {
            //        bool passPixel = true;
            //        for (int iChannel = 0; iChannel < channelThresh.Length; iChannel++)
            //        {
            //            //The inversion of the inequality is intended as this uses the opposite logic as its assumed to be mostly true instead of mostly false.
            //            passPixel &= channelThresh[iChannel].CompareTo(tempImage.Data[height, width, iChannel]) <= 0;
            //        }
            //        if (passPixel)
            //        {
            //            botRightX = width;
            //            botRightY = height;
            //        }
            //    }

            //}

            //rectWidth = botRightX - rectX;
            //rectHeight = botRightY - rectY;

            //return new System.Drawing.Rectangle(rectX, rectY, rectWidth, rectHeight);
        }

        public static BitArray testLine<TColor, TDepth>(Emgu.CV.Mat imageLine, TDepth[] channelThresh, int row = 0)
            where TColor : struct, IColor
            where TDepth : IComparable<TDepth>, new()
        {
            if (imageLine.NumberOfChannels < channelThresh.Length)
            { throw new IndexOutOfRangeException("Array exceeds number of channels in image."); }

            Image<TColor, TDepth> tempImage = imageLine.ToImage<TColor, TDepth>();
            BitArray outArray = new BitArray(imageLine.Width);

            for (int pixel = 0; pixel < imageLine.Width; pixel++)
            {
                bool temp = true;
                for (int channel = 0; channel < channelThresh.Length; channel++)
                {
                    temp &= channelThresh[channel].CompareTo(tempImage.Data[row, pixel, channel]) <= 0;
                }
                outArray[pixel] = temp;
            }
            return outArray;
        }
    }


}
