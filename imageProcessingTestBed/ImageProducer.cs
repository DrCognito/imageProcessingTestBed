using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace imageProcessingTestBed
{
    abstract class ImageProducer : IEnumerable<Image>
    {
        
        public abstract List<Image> imageList
        { get; set; }

        public abstract int time
        { get; set; }

        public IEnumerator<Image> GetEnumerator()
        {
            foreach(var i in imageList)
            { yield return i; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return imageList.GetEnumerator();
        }
    }
}
