using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using System.Collections;
using Emgu.CV.Structure;

namespace imageProcessingTestBed.Discriminators
{
    class PeakPattern : IDiscResult
    {
        private int ImageWidth = 0;
        private int ImageHeight = 0;

        List<BitArray> resultList = new List<BitArray>(3);


        double highThreshold = 180.0;


        public PeakPattern(Mat inImage, double highThresh = 180.0)
        {
            ImageHeight = inImage.Height;
            ImageWidth = inImage.Width;
            highThreshold = highThresh;

            //Should text a smaller bounding box than the peak finding threshold.
            var textRect = ProcessingTools.findTextEdge<Bgr, double>(inImage, new double[] { highThreshold, highThreshold, highThreshold });


            resultList[0] = ProcessingTools.testLine<Bgr, double>(inImage, new double[] { highThreshold, highThreshold, highThreshold },
                textRect.Bottom + textRect.Height / 4);
            resultList[1] = ProcessingTools.testLine<Bgr, double>(inImage, new double[] { highThreshold, highThreshold, highThreshold },
                textRect.Bottom + textRect.Height / 2);
            resultList[2] = ProcessingTools.testLine<Bgr, double>(inImage, new double[] { highThreshold, highThreshold, highThreshold },
                (int)(textRect.Bottom + textRect.Height * 0.75));
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

            var textRect = ProcessingTools.findTextEdge<Bgr, double>(inImage, new double[] { highThreshold, highThreshold, highThreshold });


            inFileResults[0] = ProcessingTools.testLine<Bgr, double>(inImage, new double[] { highThreshold, highThreshold, highThreshold },
                textRect.Bottom + textRect.Height / 4);
            inFileResults[1] = ProcessingTools.testLine<Bgr, double>(inImage, new double[] { highThreshold, highThreshold, highThreshold },
                textRect.Bottom + textRect.Height / 2);
            inFileResults[2] = ProcessingTools.testLine<Bgr, double>(inImage, new double[] { highThreshold, highThreshold, highThreshold },
                (int)(textRect.Bottom + textRect.Height * 0.75));

            float[] DotProduct = new float[] { 0, 0, 0 };

            for (int iRow = 0; iRow < 3; iRow++)
            {
                for (int pixel = 0; pixel < resultList[0].Length; pixel++)
                {
                    if (inFileResults[iRow].Length > pixel)
                    {
                        DotProduct[iRow] += Convert.ToInt32(inFileResults[iRow][pixel]) * Convert.ToInt32(resultList[iRow][pixel]);
                    }
                    else { break; }
                }
                DotProduct[iRow] /= resultList[iRow].Length * inFileResults[iRow].Length;

            }

            //Return an average of the three cosines.
            return (DotProduct[0] + DotProduct[1] + DotProduct[2]) / 3.0f;
        }
    }
}
