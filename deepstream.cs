using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;

namespace videocapture
{
    public class deepstream
    {
        public static Cv2Video cv2Video = null;
        //static int SIZE_WIDTH = 1920;
        //static int SIZE_HEIGHT = 1080;
        static int frame_index = 0;
        static int frame_index2 = 0;


        //static Form1 Form1 = new Form1();

        const string DLLPATH = "libRtspClientLib.so";
        //-------------------declaration of Function of dll-------------------

        [DllImport(DLLPATH)]
        public static extern void init();

        [DllImport(DLLPATH)]
        public static extern int createRtspClient(int id, string url, int conn_mode);

        [DllImport(DLLPATH)]
        public static extern int destoryRtspClientAll();

        [DllImport(DLLPATH)]
        public static extern int destoryRtspClient(int id);
        [DllImport(DLLPATH)]
        public static extern int isConnect(int id);

        [DllImport(DLLPATH)]
        public static extern int reConnect(int id);

        [DllImport(DLLPATH)]
        public static extern int mRead_Opencv(int id, int width, int height, int size, IntPtr data);



        //回调函数 获取rtsp流信息组装成mat 一个pipline对应一个函数
        public static void dectcallbackfunc1(IntPtr data, int frameh, int framew, int fstep)
        {
            Mat frame = new Mat(frameh, framew, MatType.CV_8UC4, data, fstep);
            //size转化
            var new_size = new OpenCvSharp.Size(framew, frameh);
            Mat out_mat = new Mat(new_size, MatType.CV_8UC3);  //输出mat        
            Cv2.CvtColor(frame, out_mat, ColorConversionCodes.BGRA2BGR);
            //Console.WriteLine("mat size is:" + out_mat.Size());
            Console.WriteLine("RTSP-1:" + frame_index++);
            Console.WriteLine("thread ID is:" + Thread.CurrentThread.ManagedThreadId.ToString());
            //GC.Collect();
            frame.Dispose();
            out_mat.Dispose();
            //Cv2.ImWrite("./out/" + (frame_index++).ToString() + ".jpg", out_mat);
            //Bitmap currBitmap = BitmapConverter.ToBitmap(out_mat);          

        }


        public static void start()
        {
            cv2Video = new Cv2Video();//不加这句会报错
            const string url1 = "rtsp://admin:wanji168@10.100.8.64:554/cam/realmonitor?channel=1&subtype=0";
            init();
            createRtspClient(0, url1, 1);
            int i = 0;
            int status = 0;

            do
            {

                if (isConnect(0) == 1)
                {

                    Thread.Sleep(500);
                    Console.WriteLine("id:0 connected\n");

                    //opencv
                    int width = 0;
                    int height = 0;
                    int size = 0;
                    IntPtr data = Marshal.AllocHGlobal(100);
                    status = mRead_Opencv(0, width, height, size, data);
                    if (status == 1)
                    {
                        Mat frame = new Mat(height, width, MatType.CV_8UC3, data);
                        var new_size = new OpenCvSharp.Size(width, height);
                        Mat out_mat = new Mat(new_size, MatType.CV_8UC3);  //输出mat        
                        Cv2.CvtColor(frame, out_mat, ColorConversionCodes.BGRA2BGR);
                        Cv2.ImWrite("./out/" + (frame_index++).ToString() + ".jpg", out_mat);
                        frame.Dispose();
                        out_mat.Dispose();
                        
                        //cv::Mat img(height, width, CV_8UC3, data);
                        //time_t timep;
                        //time(&timep);
                        //Console.WriteLine("%s", ctime(&timep));
                        //string ss = ".jpg";
                        //string filename = ctime(&timep) + ss;
                        //cv::imwrite("./out/" + filename, img);
                        //std::string name = "test.jpg";
                        //cv::imwrite( name, img);
                    }

                }
                else
                {
                    Console.WriteLine("id:0 disconnect %d \n", isConnect(0));
                    Thread.Sleep(500);
                    if (isConnect(0) == 2)
                    {
                        Thread.Sleep(500);
                        reConnect(0);
                    }
                }
                //Ini2();//线性执行没有问题
                //Thread thread1 = new Thread(Ini1);
                //Thread thread2 = new Thread(Ini2);
                //thread1.Start();
                //thread2.Start();
                ////Thread.Sleep(TimeSpan.FromSeconds(5));

            }
            while (true);


        }
    
    } 
}
