using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;

namespace imageProcessingTestBed
{
    interface DiscResult
    {
        float ProbabilityMatch(Mat inImage);
        float CompareDiscriminators(DiscResult OtherDiscriminator, Mat inImage);
        int MeasuredWithWidth();
        int MeasuredWithHeight();
    }
}
