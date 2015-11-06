using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Emgu.CV;
using Emgu.CV.CvEnum;

namespace imageProcessingTestBed.Interfaces
{
    class FFMpegStream
    {
        const string FFMpegPath = @"ffmpeg.exe";
        public string InputLocation { get; }

        public Emgu.CV.Mat GetFrame()
        {
            string tempFile = Path.GetTempFileName();
            using (Process p = new Process())
            {
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.FileName = FFMpegPath;
                p.StartInfo.Arguments = "-i " + InputLocation + "-frames:v 1 -f image2 " + tempFile;
                p.Start();
                p.WaitForExit();
            }

            Mat Output = CvInvoke.Imread(tempFile, LoadImageType.Unchanged);
            File.Delete(tempFile);

            return Output;
        }

        public FFMpegStream(string inName)
        {
            InputLocation = inName;
        }
    }
}
