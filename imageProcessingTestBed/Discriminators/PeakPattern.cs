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


        const double highThreshold = 180.0;

        public PeakPattern(Mat inImage, double highThresh = highThreshold)
        {
            ImageHeight = inImage.Height;
            ImageWidth = inImage.Width;

            //Should text a smaller bounding box than the peak finding threshold.
            var textRect = ProcessingTools.findTextEdge<Bgr, double>(inImage, new double[] { highThresh, highThresh, highThresh });
            

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
            throw new NotImplementedException();
        }
    }
}
