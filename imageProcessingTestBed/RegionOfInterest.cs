using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

//A class for providing a corrected ROI as a Rectange based on percent info
//This ROI is thus independant of the original image pixels
namespace imageProcessingTestBed
{
    class RegionOfInterest
    {
        public float percentX { get; set; }
        public float percentY { get; set; }
        public float percentWidth { get; set; }
        public float percentHeight { get; set; }

        public RegionOfInterest(float x, float y, float width, float height)
        {
            this.percentX = x;
            this.percentY = y;
            this.percentWidth = width;
            this.percentHeight = height;
        }

        public RegionOfInterest()
        {
            this.percentX = 0.0f;
            this.percentY = 0.0f;
            this.percentWidth = 0.0f;
            this.percentHeight = 0.0f;
        }

        public System.Drawing.Rectangle getRect(Int32 imageWidth, Int32 imageHeight)
        {
            Int32 outX = (int)System.Math.Floor((float)imageWidth * this.percentX);
            Int32 outY = (int)System.Math.Floor((float)imageHeight * this.percentY);
            Int32 outWidth = (int)System.Math.Ceiling((float)imageWidth * this.percentWidth);
            Int32 outHeight = (int)System.Math.Ceiling((float)imageHeight * this.percentHeight);
            return new Rectangle(outX, outY, outWidth, outHeight);
        }

        public Emgu.CV.Mat getROIImage(Emgu.CV.Mat inImage)
        {
            return new Emgu.CV.Mat(inImage, getRect(inImage.Width, inImage.Height));
        }
    }
}
