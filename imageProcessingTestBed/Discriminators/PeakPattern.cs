using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using System.Collections;
using Emgu.CV.Structure;
using Emgu.CV.UI;

namespace imageProcessingTestBed.Discriminators
{
    class PeakPattern : IDiscResult
    {
        private int ImageWidth = 0;
        private int ImageHeight = 0;

        List<BitArray> resultList = new List<BitArray>(3);

        System.Drawing.Rectangle textRect;
        double highThreshold;


        public PeakPattern(Mat inImage, double highThresh = 180.0)
        {
            ImageHeight = inImage.Height;
            ImageWidth = inImage.Width;
            highThreshold = highThresh;

            //Should text a smaller bounding box than the peak finding threshold.
            textRect = ProcessingTools.findTextEdge<Bgr, double>(inImage, new double[] { highThreshold, highThreshold, highThreshold });

            //resultList[0] = new BitArray(ImageWidth);
            Mat croppedImage = new Mat(inImage, textRect);
            //resultList.Add(ProcessingTools.testLine<Bgr, double>(inImage, new double[] { highThreshold, highThreshold, highThreshold },
            //    textRect.Top + textRect.Height / 4));
            //Console.WriteLine(textRect.Top + textRect.Height / 2);
            //resultList.Add(ProcessingTools.testLine<Bgr, double>(inImage, new double[] { highThreshold, highThreshold, highThreshold },
            //    textRect.Top + textRect.Height / 2));
            //resultList.Add(ProcessingTools.testLine<Bgr, double>(inImage, new double[] { highThreshold, highThreshold, highThreshold },
            //    (int)(textRect.Top + textRect.Height * 0.75)));

            resultList.Add(ProcessingTools.testLine<Bgr, double>(croppedImage, new double[] { highThreshold, highThreshold, highThreshold },
                textRect.Height / 4));
//            Console.WriteLine(textRect.Top + textRect.Height / 2);
            resultList.Add(ProcessingTools.testLine<Bgr, double>(croppedImage, new double[] { highThreshold, highThreshold, highThreshold },
                textRect.Height / 2));
            resultList.Add(ProcessingTools.testLine<Bgr, double>(croppedImage, new double[] { highThreshold, highThreshold, highThreshold },
                (int)(textRect.Height * 0.75)));
            //Console.WriteLine("Disc using lines {0}, {1}, {2}",
            //    textRect.Height / 4,
            //    textRect.Height / 2,
            //    (int)(textRect.Height * 0.75));
        }

        public float CompareDiscriminators(IDiscResult OtherDiscriminator, Mat inImage)
        {
            return Math.Abs(OtherDiscriminator.ProbabilityMatch(inImage) - ProbabilityMatch(inImage));
        }

        public int MeasuredWithHeight()
        {
            return ImageHeight;
        }

        public int MeasuredWithWidth()
        {
            return ImageWidth;
        }

        public float ProbabilityMatch(Mat inImage)
        {
            List<BitArray> inFileResults = new List<BitArray>(3);

            var tempRect = ProcessingTools.findTextEdge<Bgr, double>(inImage, new double[] { highThreshold, highThreshold, highThreshold });
            //tempRect.Height = textRect.Height;
            //tempRect.Width = textRect.Width;


            //inFileResults.Add(ProcessingTools.testLine<Bgr, double>(inImage, new double[] { highThreshold, highThreshold, highThreshold },
            //    textRect.Top + textRect.Height / 4));
            //inFileResults.Add(ProcessingTools.testLine<Bgr, double>(inImage, new double[] { highThreshold, highThreshold, highThreshold },
            //    textRect.Top + textRect.Height / 2));
            //inFileResults.Add(ProcessingTools.testLine<Bgr, double>(inImage, new double[] { highThreshold, highThreshold, highThreshold },
            //    (int)(textRect.Top + textRect.Height * 0.75)));
            //CvInvoke.Rectangle(inImage, tempRect, new Bgr(0, 0, 255).MCvScalar);
            //ImageViewer.Show(inImage, "inPlaceTest");

            Mat croppedImage = new Mat(inImage, tempRect);
            inFileResults.Add(ProcessingTools.testLine<Bgr, double>(croppedImage, new double[] { highThreshold, highThreshold, highThreshold },
                tempRect.Height / 4));
            inFileResults.Add(ProcessingTools.testLine<Bgr, double>(croppedImage, new double[] { highThreshold, highThreshold, highThreshold },
                tempRect.Height / 2));
            inFileResults.Add(ProcessingTools.testLine<Bgr, double>(croppedImage, new double[] { highThreshold, highThreshold, highThreshold },
                (int)(tempRect.Height * 0.75)));

            //Console.WriteLine("Disc using lines {0}, {1}, {2}",
            //    textRect.Height / 4,
            //    textRect.Height / 2,
            //    (int)(textRect.Height * 0.75));


            float[] DotProduct = new float[] { 0, 0, 0 };

            for (int iRow = 0; iRow < 3; iRow++)
            {
                float MagIn = 0;
                float MagRes = 0;
                for (int pixel = 0; pixel < resultList[0].Length; pixel++)
                {
                    
                    if (inFileResults[iRow].Length > pixel)
                    {
                        DotProduct[iRow] += Convert.ToInt32(inFileResults[iRow][pixel]) * Convert.ToInt32(resultList[iRow][pixel]);

                        if (inFileResults[iRow][pixel]) { MagIn++; }
                        if (resultList[iRow][pixel]) { MagRes++; }
                    }
                    else { break; }
                }
                if(MagIn == 0 || MagRes == 0) { continue; }
                double temp = Math.Sqrt(Convert.ToDouble(MagIn)) * Math.Sqrt(Convert.ToDouble(MagRes));
                DotProduct[iRow] /= (float)temp;

            }

            //Return an average of the three cosines.
            return (DotProduct[0] + DotProduct[1] + DotProduct[2]) / 3.0f;
        }
    }
}
