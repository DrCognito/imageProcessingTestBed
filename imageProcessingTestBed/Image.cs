using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace imageProcessingTestBed
{
    struct Image
    {
        public Mat loadedImage;
        public string qualityDisc; //Some sort of appropriate quality indicator? Unsure how to present
        public int time; //Not sure how to store this

        public Image(Mat x, string y, int t)
        {
            loadedImage = x;
            qualityDisc = y;
            time = t;
        }
        
    }
}
