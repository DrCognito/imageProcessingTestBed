﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;
using Emgu.CV;
using Emgu;
using Emgu.CV.UI;
using System.Drawing;
using Emgu.CV.Structure;
using System.Collections;
using ZedGraph;
using System.Windows.Forms;
using imageProcessingTestBed.Discriminators;

namespace imageProcessingTestBed
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> tempFileList = new List<string>();

            //var h = new ROOTNET.NTH1F();
            

            //File list from Blizz Americas Championship
            //tempFileList.Add("test.png");
            //tempFileList.Add("test480.png");
            tempFileList.Add("testImages/vlcsnap-2015-09-24-01h19m07s974.png");
            tempFileList.Add("testImages/vlcsnap-2015-09-24-01h19m25s545.png");
            tempFileList.Add("testImages/vlcsnap-2015-09-24-01h19m35s253.png");
            tempFileList.Add("testImages/vlcsnap-2015-09-24-01h19m55s213.png");
            tempFileList.Add("testImages/vlcsnap-2015-09-24-01h19m59s995.png");
            tempFileList.Add("testImages/vlcsnap-2015-09-24-01h20m11s857.png");
            tempFileList.Add("testImages/vlcsnap-2015-09-24-01h20m19s473.png");
            tempFileList.Add("testImages/vlcsnap-2015-09-24-01h20m35s166.png");
            tempFileList.Add("testImages/vlcsnap-2015-09-24-01h20m38s961.png");
            tempFileList.Add("testImages/vlcsnap-2015-09-24-01h20m56s916.png");
            tempFileList.Add("testImages/vlcsnap-2015-09-24-01h21m03s792.png");
            tempFileList.Add("testImages/vlcsnap-2015-09-24-01h21m30s637.png");
            tempFileList.Add("testImages/vlcsnap-2015-09-24-01h21m36s183.png");
            tempFileList.Add("testImages/vlcsnap-2015-09-24-01h22m00s213.png");
            tempFileList.Add("testImages/vlcsnap-2015-09-24-01h22m09s622.png");
            tempFileList.Add("testImages/vlcsnap-2015-09-24-01h22m22s416.png");
            tempFileList.Add("testImages/vlcsnap-2015-09-24-01h22m33s378.png");
            tempFileList.Add("testImages/vlcsnap-2015-09-24-01h22m45s875.png");
            tempFileList.Add("testImages/vlcsnap-2015-09-24-01h22m50s183.png");
            tempFileList.Add("testImages/vlcsnap-2015-09-24-01h23m06s850.png");
            tempFileList.Add("testImages/vlcsnap-2015-09-24-01h23m11s075.png");
            tempFileList.Add("testImages/vlcsnap-2015-09-24-01h23m20s624.png");
            tempFileList.Add("testImages/vlcsnap-2015-09-24-01h23m25s391.png");
            tempFileList.Add("testImages/vlcsnap-2015-09-24-01h23m35s019.png");
            tempFileList.Add("testImages/vlcsnap-2015-09-24-01h23m39s636.png");
            tempFileList.Add("testImages/vlcsnap-2015-09-24-01h23m49s060.png");
            tempFileList.Add("testImages/vlcsnap-2015-09-24-01h24m34s479.png");
            tempFileList.Add("testImages/vlcsnap-2015-09-24-01h24m44s448.png");
            tempFileList.Add("testImages/vlcsnap-2015-09-24-01h24m56s372.png");
            tempFileList.Add("testImages/vlcsnap-2015-09-24-01h25m05s414.png");

            fileImageProducer blizzAmericas = new fileImageProducer(tempFileList);

            var newFile = new ROOTNET.NTFile("output.root", "RECREATE");
            //String win1 = "Edge output test"; //Window name
            //CvInvoke.NamedWindow(win1); //Create the window with the name persona 3 style
            Mat outImage = new Mat();

            RegionOfInterest leftBlizz = new RegionOfInterest(510f / 1280f, 0f / 720f, 100f / 1280f, 20f / 700f);
            RegionOfInterest rightBlizz = new RegionOfInterest(665f / 1280f, 0f / 720f, 100f / 1280f, 20f / 700f);

            var bestRowHisto = new ROOTNET.NTH1I("bestRows", "Top rows by discrimination", 20, 0, 20);
            //newFile.Add(bestRowHisto);

            //Text for comparison. "TEMPO STORM"
            TextSize textSizeTest = new TextSize(leftBlizz.getROIImage(blizzAmericas.ElementAt(0).loadedImage));
            PeakPattern peakPatternTest = new PeakPattern(leftBlizz.getROIImage(blizzAmericas.ElementAt(0).loadedImage), 200.0);
            //PeakPattern peakPatternTest2 = new PeakPattern(rightBlizz.getROIImage(blizzAmericas.Last().loadedImage));

            for (int iImage = 0; iImage < blizzAmericas.Count(); iImage++)
            {
                Image testc = blizzAmericas.ElementAt(iImage);
                Mat leftRegion = leftBlizz.getROIImage(testc.loadedImage);
                Mat rightRegion = rightBlizz.getROIImage(testc.loadedImage);

                List<Tuple<int, int>> rowResult = topThree(entropyList<Bgr, byte>(leftRegion, rightRegion, new byte[] { 180, 180, 180 }));

                ROOTNET.NTH1F tempLeft = drawHisto<Bgr, double>(leftRegion, rowResult[0].Item1, 0, "left " + iImage);
                ROOTNET.NTH1F tempRight = drawHisto<Bgr, double>(rightRegion, rowResult[0].Item1, 0, "right " + iImage);

                //newFile.Add(tempLeft);
                //newFile.Add(tempRight);

                bestRowHisto.Fill(rowResult[0].Item1);
                bestRowHisto.Fill(rowResult[1].Item1);
                bestRowHisto.Fill(rowResult[3].Item1);

                //Console.WriteLine("Row {0} with {1}, row {2} with {3}, row {4} with {5}.",
                //    rowResult[0].Item1, rowResult[0].Item2,
                //    rowResult[1].Item1, rowResult[1].Item2,
                //    rowResult[2].Item1, rowResult[2].Item2);
                Console.WriteLine("File {0}", tempFileList[iImage]);
                //Console.WriteLine("TextSize Probability left {0}, TextSize probability right {1}.",
                //    textSizeTest.ProbabilityMatch(leftRegion),
                //    textSizeTest.ProbabilityMatch(rightRegion));
                Console.WriteLine("PeakPattern cosine left {0}, PeakPattern cosine right {1}.",
                    peakPatternTest.ProbabilityMatch(leftRegion),
                    peakPatternTest.ProbabilityMatch(rightRegion));

            }


            //CvInvoke.Imshow(win1, testMat);
            //TesseractEngine testOCR = new TesseractEngine("langDat", "eng", Tesseract.EngineMode.Default);
            //testOCR.DefaultPageSegMode(Tesseract.PageSegMode.SingleChar);
            //testOCR.

            //ImageViewer.Show(outImage, "test");

            //Some numbers from a 720p image in paint for ROI testing.

            //ImageViewer.Show(leftBlizz.getROIImage(blizzAmericas.ElementAt(0).loadedImage), "leftName");
            //ImageViewer.Show(rightBlizz.getROIImage(blizzAmericas.ElementAt(0).loadedImage), "rightName");

            Mat testProcessing = leftBlizz.getROIImage(blizzAmericas.ElementAt(0).loadedImage);
            Mat testProcessingRight = rightBlizz.getROIImage(blizzAmericas.ElementAt(1).loadedImage);

            System.Drawing.Rectangle textRegion = ProcessingTools.findTextEdge<Bgr, double>(testProcessing, new double[] { 180.0, 180.0, 180.0 });
            System.Drawing.Rectangle textRegion2 = ProcessingTools.findTextEdge<Bgr, double>(testProcessingRight, new double[] { 180.0, 180.0, 180.0 });

            //CvInvoke.Rectangle(testProcessing, textRegion, new Bgr(0, 0, 255).MCvScalar);
            //ImageViewer.Show(leftBlizz.getROIImage(blizzAmericas.ElementAt(0).loadedImage), "leftName");

            //CvInvoke.Rectangle(testProcessingRight, textRegion2, new Bgr(0, 0, 255).MCvScalar);
            //ImageViewer.Show(testProcessingRight, "rightName");



            //ImageViewer.Show(testProcessing);
            //Mat[] leftBlizzChannels = leftBlizz.getROIImage(outImage).Split();
            //Mat[] rightBlizzChannels = rightBlizz.getROIImage(outImage).Split();

            //foreach( Mat iMat in leftBlizzChannels)
            //{
            //    ImageViewer.Show(iMat);
            //}

            Console.ReadKey(); //Wait for return to finish!
            newFile.Write();
            //newFile.Close();
        }

        static ROOTNET.NTH1F drawHisto<TColor,TDepth>(Mat inImage, int row, int channel, string histoName = "default")
            where TColor : struct, IColor
            where TDepth : IComparable<TDepth>, new()
        {
            var testHisto = new ROOTNET.NTH1F(histoName, histoName, inImage.Width, 0.0, inImage.Width);
            

            Image<TColor, TDepth> tempImage = inImage.ToImage<TColor, TDepth>();

            for (int pixel = 0; pixel < inImage.Width; pixel++)
            {
                testHisto.Fill(pixel, Convert.ToDouble(tempImage.Data[row, pixel, channel]));
                
            }

            return testHisto;
            
            
        }

        static List<Tuple<int, int>> topThree(List<int> inList)
        {
            List<Tuple<int, int>> outList = new List<Tuple<int, int>>(inList.Count());
            //Initial setup
            for (int i = 0; i < inList.Count(); i++)
            {
                outList.Add(Tuple.Create(i, inList[i]));
            }
            outList.Sort((l1, l2) => l2.Item2.CompareTo(l1.Item2));

            return outList;
        }

        static List<int> entropyList<TColor, TDepth>(Emgu.CV.Mat image1, Emgu.CV.Mat image2, TDepth[] channelThresh)
            where TColor : struct, IColor
            where TDepth : IComparable<TDepth>, new()
        {

            List<int> results = new List<int>();
            for (int row = 0; row < image1.Height; row++)
            {
                BitArray test1 = ProcessingTools.testLine<TColor, TDepth>(image1, channelThresh, row);
                BitArray test2 = ProcessingTools.testLine<TColor, TDepth>(image2, channelThresh, row);

                BitArray resArray = test1.Xor(test2);

                int nIndyBits = 0;
                foreach (bool res in resArray)
                {
                    if (res) { ++nIndyBits; }
                }
                //Console.WriteLine("Independant bits at " + row + ": " + result);
                results.Add(nIndyBits);
            }
            return results;
        }
        

        

        static Emgu.CV.Mat cannyEdges(Emgu.CV.Mat inImage)
        {
            double thresh1 = 180.0;
            double thresh2 = 160.0;

            Emgu.CV.Mat outImage = new Emgu.CV.Mat();
            CvInvoke.Canny(inImage, outImage, thresh1, thresh2);

            return outImage;

        }
    }
}
