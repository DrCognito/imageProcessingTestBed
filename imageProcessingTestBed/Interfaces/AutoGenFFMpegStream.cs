//FFMpeg that uses the autogen libraries provided at:
//https://github.com/Ruslan-B/FFmpeg.AutoGen/tree/master/FFmpeg

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFmpeg.AutoGen;
using Emgu.CV;

namespace imageProcessingTestBed.Interfaces
{
    class AutoGenFFMpegStream
    {
        static bool FFMpegInitialised = false;
        public string InputLocation { get; }
        bool ProcessedFrame = false;
        public Mat Frame { get; } = new Mat();

        static void InitFFMpeg()
        {
            if (!FFMpegInitialised)
            {
                FFmpegInvoke.av_register_all();
                FFmpegInvoke.avcodec_register_all();
                FFmpegInvoke.avformat_network_init();
            }

        }

        public AutoGenFFMpegStream(string URL)
        {
            InitFFMpeg();
            InputLocation = URL;
        }

        public unsafe Mat GetFrame()
        {
            if (ProcessedFrame)
            {
                return Frame;
            }

            AVFormatContext* pFormatContext = FFmpegInvoke.avformat_alloc_context();

            if (FFmpegInvoke.avformat_open_input(&pFormatContext, InputLocation, null, null) != 0)
            {
                throw new Exception("Failed to open " + InputLocation);
            }

            AVStream* pStream = null;
            for (int iStream = 0; iStream < pFormatContext->nb_streams; iStream++)
            {
                if(pFormatContext->streams[iStream]->codec->codec_type == AVMediaType.AVMEDIA_TYPE_VIDEO)
                {
                    pStream = pFormatContext->streams[iStream];
                    break;
                }
            }
            if(pStream == null)
            {
                throw new Exception("No video stream found in " + InputLocation);
            }
            AVCodecContext* pCodecContext = pStream->codec;

            

            var packet = new AVPacket();
            AVPacket* pPacket = &packet;
            FFmpegInvoke.av_init_packet(pPacket);


            //Read into pPacket and check.
            while (pPacket->stream_index != pStream->index)
            {
                if (FFmpegInvoke.av_read_frame(pFormatContext, pPacket) < 0)
                {
                    throw new Exception("Failed reading frame in " + InputLocation);
                }
            }

            //Do the video decode.
            AVFrame* pFrame = FFmpegInvoke.avcodec_alloc_frame();
            int GotPicture = 0;
            int Status = FFmpegInvoke.avcodec_decode_video2(pCodecContext, pFrame, &GotPicture, pPacket);
            if(GotPicture !=0 || Status < 0)
            {
                throw new Exception(string.Format("Error decoding video at {0}", InputLocation));
            }

            //Conversion to Mat
            //http://stackoverflow.com/questions/29263090/ffmpeg-avframe-to-opencv-mat-conversion
        }
    }
}
