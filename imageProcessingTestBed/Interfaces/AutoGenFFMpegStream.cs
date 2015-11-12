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
        public Mat Frame { get; set; }

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
                if (pFormatContext->streams[iStream]->codec->codec_type == AVMediaType.AVMEDIA_TYPE_VIDEO)
                {
                    pStream = pFormatContext->streams[iStream];
                    break;
                }
            }
            if (pStream == null)
            {
                throw new Exception("No video stream found in " + InputLocation);
            }
            AVCodecContext* pCodecContext = pStream->codec;
            AVCodecID CodecID = pCodecContext->codec_id;
            AVCodec* pCodec = FFmpegInvoke.avcodec_find_decoder(CodecID);

            if (pCodec == null)
            {
                throw new Exception("Unsupported codec in " + InputLocation);
            }


            FFmpegInvoke.avcodec_open2(pCodecContext, pCodec, null);

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


            //Try to seek to first keyframe.
            //FFmpegInvoke.av_seek_frame(pFormatContext, pStream->index, pFormatContext->start_time, 0);

            int GotPicture = 0;
            int Attempts = 0;
            //bool GoodFrame = false;

            while (GotPicture != 1 && Attempts < 100)
            {
                while (pPacket->stream_index != pStream->index)
                {
                    if (FFmpegInvoke.av_read_frame(pFormatContext, pPacket) < 0)
                    {
                        throw new Exception(String.Format("Failed reading frame in {0}, frame:{1}", InputLocation, Attempts + 1));
                    }
                    //This should check for keyframes...
                    if(pPacket->flags == 0) { Attempts++; continue; }
                }

                int Status = FFmpegInvoke.avcodec_decode_video2(pCodecContext, pFrame, &GotPicture, pPacket);
                if (Status <= 0)
                {
                    throw new Exception(string.Format("Error decoding video in {0} frame:{1}", InputLocation, Attempts + 1));
                }

                Attempts++;

                if (GotPicture == 1) { Console.WriteLine("Successfully decoded a frame at " + Attempts); }
            }
            if (GotPicture == 0)
            {
                throw new Exception("Failed to decode a good frame in " + InputLocation);
            }

            //Conversion to Mat
            //http://stackoverflow.com/questions/29263090/ffmpeg-avframe-to-opencv-mat-conversion
            //

            int FrameHeight = pFrame->height;
            int FrameWidth = pFrame->width;

            SwsContext* pConvertContext = FFmpegInvoke.sws_getContext(FrameWidth,
                FrameHeight,
                pCodecContext->pix_fmt,
                FrameWidth,
                FrameHeight,
                AVPixelFormat.PIX_FMT_BGR24,
                FFmpegInvoke.SWS_FAST_BILINEAR,
                null, null, null);

            if (pConvertContext == null)
            {
                throw new Exception("Failed to initialise Mat conversion context.");
            }

            var pConvertedFrame = (AVPicture*)FFmpegInvoke.avcodec_alloc_frame();
            
            //Setup the converted frame
            int ConvertedFrameBufferSize = FFmpegInvoke.avpicture_get_size(AVPixelFormat.PIX_FMT_BGR24, FrameWidth, FrameHeight);
            var pConvertedFrameBuffer = (byte*)FFmpegInvoke.av_malloc((uint)ConvertedFrameBufferSize);
            if(FFmpegInvoke.avpicture_fill(pConvertedFrame, pConvertedFrameBuffer, AVPixelFormat.PIX_FMT_BGR24, FrameWidth, FrameHeight) < 0)
            {
                throw new Exception("Failed to setup conversion frame");
            }

            byte** src = &pFrame->data_0;
            byte** dst = &pConvertedFrame->data_0;
            FFmpegInvoke.sws_scale(pConvertContext, src, pFrame->linesize, 0, FrameHeight,
                dst, pConvertedFrame->linesize);

            var imageBufferPtr = new IntPtr(pConvertedFrame->data_0);
            Frame = new Mat(FrameHeight, FrameWidth, Emgu.CV.CvEnum.DepthType.Cv8U, 3, imageBufferPtr, *pConvertedFrame->linesize);

            //Cleanup
            FFmpegInvoke.av_free(pFrame);
            FFmpegInvoke.av_free(pConvertedFrame);
            FFmpegInvoke.sws_freeContext(pConvertContext);
            FFmpegInvoke.avcodec_close(pCodecContext);
            FFmpegInvoke.avformat_close_input(&pFormatContext);

            ProcessedFrame = true;
            return Frame;
        }
    }
}
