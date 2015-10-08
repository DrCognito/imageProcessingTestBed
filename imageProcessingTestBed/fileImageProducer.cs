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
    class fileImageProducer : ImageProducer
    {
        public override List<Image> imageList
        {
            get;
            //{
            //    return this.imageList;
            //    //throw new NotImplementedException();
            //}

            set;
            //{
            //    this.imageList = value;
            //}
        }

        public override int time
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        //Constructer with Image structs
        public fileImageProducer(IEnumerable<Image> input)
        {
            imageList = new List<Image>();
            foreach (Image i in input)
            {
                this.imageList.Add(i);
            }
        }
        //Constructor with just filenames
        public fileImageProducer(IEnumerable<string> input)
        {
            imageList = new List<Image>();

            int fauxTime = 0;
            foreach (string i in input)
            {
                Mat imageTemp = new Mat();
                imageTemp = CvInvoke.Imread(i, LoadImageType.Unchanged);
                imageList.Add(new Image(imageTemp, "local", fauxTime));
                fauxTime++;
            }
        }
    }
}
