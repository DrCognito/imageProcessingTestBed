using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imageProcessingTestBed
{
    class HLSImageProducer
    {
        public List<Tuple<TimeSpan, string>> VideoSegmentsList { get; }
        public float ReportedSeconds { get; } = 0.0f;
        public TimeSpan TotalTime { get; } = new TimeSpan(0, 0, 0);

        public HLSImageProducer(string M3U8In)
        {
            string[] Lines = M3U8In.Split('\n');

            for (int iLine = 0; iLine < Lines.Length; iLine++)
            {
                string[] prefix = Lines[iLine].Split(':');

                switch (prefix[0])
                {

                    case @"#EXTINF":
                        //Expecting "#EXTINF:<time>," format
                        TimeSpan TimeSegment = new TimeSpan();
                        TimeSpan.TryParseExact(prefix[1], @"s\.fff\,", new CultureInfo("en-US"), out TimeSegment);
                        iLine++;
                        if (Lines[iLine] != null)
                        {
                            VideoSegmentsList.Add(new Tuple<TimeSpan, string>(TimeSegment, Lines[iLine]));
                            TotalTime += TimeSegment;
                        }
                        break;

                    case @"#EXT-X-TWITCH-TOTAL-SECS":
                        ReportedSeconds = float.Parse(prefix[1]);
                        break;
                    default:
                        break;
                }

            }
        }

        
        public int? SeekTimeIndex(TimeSpan inTime, out TimeSpan TimeInFile)
        {
            
            if(inTime > TotalTime)
            {
                TimeInFile = new TimeSpan(0, 0, 0);
                return null;
            }

            TimeSpan TempTime = new TimeSpan(0, 0, 0);

            for (int iSegment = 0; iSegment < VideoSegmentsList.Count; iSegment++)
            {
                TempTime += VideoSegmentsList[iSegment].Item1;
                if(TempTime >= inTime)
                {
                    TimeInFile = TempTime - inTime;
                    return iSegment;
                }
            }

            //This should never happen
            Console.WriteLine("SeekTimeIndex outside of TimeSpan segment return loop.");
            TimeInFile = new TimeSpan(0, 0, 0);
            return null;

        }
    }
}
