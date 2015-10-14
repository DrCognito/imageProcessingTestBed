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
        public float FractionalWidthUncertainty { get; }
        public float FractionalHeightUncertainty { get; }

        const double tightThreshold = 150.0;
        const double looseThreshold = 120.0;
        double denominator = Math.Sqrt(2);

        TextSize(Image inImage, double looseThresh = looseThreshold, double tightThresh = tightThreshold)
        {
            textRect = ProcessingTools.findTextEdge<Bgr, double>(inImage.loadedImage, new double[] { tightThresh, tightThresh, tightThresh });

            ImageWidth = inImage.loadedImage.Width;
            RelativeWidth = (float)textRect.Width / (float)ImageWidth;

            ImageHeight = inImage.loadedImage.Height;
            RelativeHeight = (float)textRect.Height / (float)ImageHeight;

            //Use looser rectangle to find the uncertainty.
            System.Drawing.Rectangle looseRect = ProcessingTools.findTextEdge<Bgr, double>(inImage.loadedImage, new double[] { looseThresh, looseThresh, looseThresh });
            FractionalWidthUncertainty = (looseRect.Width - textRect.Width) / (float)looseRect.Width;
            FractionalHeightUncertainty = (looseRect.Height - textRect.Height) / (float)looseRect.Height;
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
            return ProbabilityMatch(inImage, tightThreshold);
        }

        public float ProbabilityMatch(Mat inImage, double tightThresh)
        {
            System.Drawing.Rectangle ImageTextRect = ProcessingTools.findTextEdge<Bgr, double>(inImage, new double[] { tightThresh, tightThresh, tightThresh });

            float ImageRelativeWidth = ImageTextRect.Width / (float)inImage.Width;
            float ImageRelativeHeight = ImageTextRect.Height / (float)inImage.Height;

            float pixelStdError = inImage.Width * FractionalWidthUncertainty; //Should be evaluated more systematically based on the gradient i guess.

            float StdDeviations = (ImageRelativeHeight - RelativeWidth) / pixelStdError;

            return (float)Meta.Numerics.Functions.AdvancedMath.Erf(StdDeviations / denominator);

        }
    }
}
