using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;

namespace imageProcessingTestBed.Discriminators
{
    class TextSize : IDiscResult
    {
        private System.Drawing.Rectangle textRect;
        private int ImageWidth = 0;
        private int ImageHeight = 0;
        public float RelativeWidth { get; }
        public float RelativeHeight { get; }

        TextSize(Image inImage)
        {
            textRect = ProcessingTools.findTextEdge<Bgr, double>(inImage.loadedImage, new double[] { 120.0, 120.0, 120.0 });

            ImageWidth = inImage.loadedImage.Width;
            RelativeWidth = (float)textRect.Width / (float)ImageWidth;

            ImageHeight = inImage.loadedImage.Height;
            RelativeHeight = (float)textRect.Height / (float)ImageHeight;
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
            System.Drawing.Rectangle ImageTextRect = ProcessingTools.findTextEdge<Bgr, double>(inImage, new double[] { 120.0, 120.0, 120.0 });

            float ImageRelativeWidth = ImageTextRect.Width / (float)inImage.Width;
            float ImageRelativeHeight = ImageTextRect.Height / (float)inImage.Height;
        }
    }
}
