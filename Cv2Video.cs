using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace videocapture
{
    /// <summary>
    /// cv2视频封装类
    /// </summary>
    public class Cv2Video
    {
        public double videoFps = 0;//帧率
        public int videoSleepTime = 0;//帧间隔
        public int videoFrameCount = 0;//帧总数
        public int videoWidth = 0;//宽度
        public int videoHeight = 0;//高度
        public CaptureType type = 0;//类型 File = 0,Camera = 1,NotSpecified = 2
        public double zoom = 0;//缩放
        public int posMsec = 0;//当前时间戳 毫秒

        private VideoCapture capture = null;
        private Mat currImage = new Mat();
        /// <summary>
        /// 开启视频文件
        /// </summary>
        /// <param name="videoPath"></param>
        public bool openVideoFile(string videoPath)
        {
            try
            {
                capture = new VideoCapture(videoPath);
                videoFrameCount = capture.FrameCount;
                videoWidth = capture.FrameWidth;
                videoHeight = capture.FrameHeight;
                zoom = capture.Zoom;
                type = capture.CaptureType;
                posMsec = capture.PosMsec;

                videoFps = capture.Fps;
                videoSleepTime = (int)Math.Round(1000 / capture.Fps);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 开启rtsp码流
        /// </summary>
        /// <param name="videoPath"></param>
        public bool openRtsp(string rtspPath)
        {
            try
            {
                capture = new VideoCapture(rtspPath);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 取摄像头数据
        /// camera=0
        /// </summary>
        /// <param name="camera">0</param>
        public bool openCamera()
        {
            try
            {
                capture = new VideoCapture(0);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 读取当前帧
        /// 水平翻转
        /// 返回null或Bitmap
        /// </summary>
        /// <returns></returns>
        public Bitmap currFrameGetImageFlip(bool flipY, int rotate)
        {

            //if (Form1.frameList.Count > 0)
            //{
            //    if (Form1.frameList.Count % 5 == 0 && capture.PosFrames == Form1.frameList[0][0])//每5辆调整一次视频帧，调整触发帧
            //    {
            //        positionFrameByIndex(capture.PosFrames);
            //    }
            //}
            int framepos = capture.PosFrames;
            capture.Read(currImage);
            if (capture.PosFrames != framepos + 1)
            {
                positionFrameByIndex(capture.PosFrames + 1);//视频异常时，无法读取时，跳过该帧
            }
            
            if (flipY && rotate == 0)
            {
                currImage = Cv2Flip.flipY(currImage);//水平翻转
            }
            if (flipY && rotate == 1)
            {
                currImage = Cv2Flip.flipX(currImage);//水平翻转
                currImage = Cv2Flip.rotate90(currImage);

            }
            if (flipY && rotate == 2)
            {
                currImage = Cv2Flip.flipY(currImage);//水平翻转
                currImage = Cv2Flip.rotate180(currImage);

            }
            if (flipY && rotate == 3)
            {
                currImage = Cv2Flip.flipX(currImage);//水平翻转
                currImage = Cv2Flip.rotate270(currImage);

            }
            //if (rotate == 1)
            //{
            //    currImage = Cv2Flip.rotate90(currImage);//顺时针旋转
                
            //}
            //if (rotate == 2)
            //{
            //    currImage = Cv2Flip.rotate180(currImage);//顺时针旋转
                
            //}
            //if (rotate == 3)
            //{
            //    currImage = Cv2Flip.rotate270(currImage);//顺时针旋转               
            //}

            if (currImage.Empty())
            {
                return null;
            }
            return currImage.ToBitmap();
        }

        public Bitmap currFrameGetImageRotate()
        {

            //if (Form1.frameList.Count > 0)
            //{
            //    if (Form1.frameList.Count % 5 == 0 && capture.PosFrames == Form1.frameList[0][0])//每5辆调整一次视频帧，调整触发帧
            //    {
            //        positionFrameByIndex(capture.PosFrames);
            //    }
            //}
            int framepos = capture.PosFrames;
            capture.Read(currImage);
            if (capture.PosFrames != framepos + 1)
            {
                positionFrameByIndex(capture.PosFrames + 1);//视频异常时，无法读取时，跳过该帧
            }
            currImage = Cv2Flip.rotate90(currImage);

            if (currImage.Empty())
            {
                return null;
            }
            return currImage.ToBitmap();
        }

        public Bitmap nextFrameGetImage()
        {
            int framepos = capture.PosFrames;
            capture.Read(currImage);
            if (capture.PosFrames != framepos + 1)
            {
                positionFrameByIndex(capture.PosFrames + 1);//视频异常时，无法读取时，跳过该帧
            }

            if (currImage.Empty())
            {
                return null;
            }
            return currImage.ToBitmap();
        }

        /// <summary>
        /// 读取当前帧
        /// 返回null或Bitmap
        /// </summary>
        /// <returns></returns>
        public Bitmap currFrameGetImage()
        {
            capture.Read(currImage);

            if (currImage.Empty())
            {
                return null;
            }
            return currImage.ToBitmap();
        }

        /// <summary>
        /// 获取当前时间戳
        /// 毫秒
        /// </summary>
        /// <returns></returns>
        public int currFrameGetMsec()
        {
            return capture.PosMsec;
        }

        /// <summary>
        /// 释放当前帧
        /// </summary>
        /// <returns></returns>
        public bool currFrameReleaseImage()
        {
            try
            {
                currImage.Release();
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 休息
        /// 根据帧间隔
        /// </summary>
        public void sleep()
        {
            Cv2.WaitKey(videoSleepTime);
        }

        /// <summary>
        /// 休息
        /// 根据指定毫秒值
        /// </summary>
        /// <param name="time"></param>
        public void sleep(int timeMisec)
        {
            Cv2.WaitKey(timeMisec);
        }

        /// <summary>
        /// 读取当前帧号
        /// </summary>
        /// <returns></returns>
        public int getCurrFrameIndex()
        {
            return capture.PosFrames;
        }

        /// <summary>
        /// 读取当前百分比
        /// </summary>
        /// <returns></returns>
        public CapturePosAviRatio getCurrFrameRatio()
        {
            return capture.PosAviRatio;
        }

        /// <summary>
        /// 读取当前毫秒
        /// </summary>
        /// <returns></returns>
        public int getCurrFrameMisec()
        {
            return capture.PosMsec;
        }

        /// <summary>
        /// 定位帧
        /// 根据帧号
        /// </summary>
        /// <param name="index"></param>
        public int positionFrameByIndex(int index)
        {
            //capture.Set(CaptureProperty.PosFrames, index);//opencv4.0.0
            capture.Set(VideoCaptureProperties.PosFrames, index);//opencv4.5.3
            return index; //opencv4.5.3
        }

        /// <summary>
        /// 定位帧
        /// 根据百分比
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int positionFrameByRatio(double ratio)
        {
            //capture.Set(CaptureProperty.PosAviRatio, ratio);//opencv4.0.0
            capture.Set(VideoCaptureProperties.PosAviRatio, ratio);//opencv4.5.3
            return (int)((float)videoFrameCount * ratio); //opencv4.5.3
        }

        /// <summary>
        /// 定位帧
        /// 根据毫秒
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int positionFrameByMisec(int index)
        {
            //capture.Set(CaptureProperty.PosMsec, index);//opencv4.0.0
            capture.Set(VideoCaptureProperties.PosMsec, index);//opencv4.5.3
            return index; //opencv4.5.3
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void dispose()
        {
            if (capture != null)
            {
                capture.Release();
                capture.Dispose();
            }
        }
    }
}
