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
        static int SIZE_WIDTH = 1920;
        static int SIZE_HEIGHT = 1080;
        static int frame_index1 = 0;
        static int frame_index2 = 0;

        public delegate void CallBackFunc(IntPtr data, int frameh, int framew, int fstep);
        static CallBackFunc GetCall1 = new CallBackFunc(dectcallbackfunc1);
        static CallBackFunc GetCall2 = new CallBackFunc(dectcallbackfunc2);

        //static Form1 Form1 = new Form1();

        const string DLLPATH = "libRtspClient.so";
        //-------------------declaration of Function of dll-------------------
        [DllImport(DLLPATH)]
        public static extern int createRtspClient(int id, char url, int conn_mode);

        [DllImport(DLLPATH)]
        public static extern int destoryRtspClientAll();

        [DllImport(DLLPATH)]
        public static extern void init();
        [DllImport(DLLPATH)]
        public static extern int rtsp_pause();
        [DllImport(DLLPATH)]
        public static extern void rtsp_release();

        [DllImport(DLLPATH)]
        public static extern int rtsp_set_callback(CallBackFunc func);
       

        //回调函数 获取rtsp流信息组装成mat 一个pipline对应一个函数
        public static void dectcallbackfunc1(IntPtr data, int frameh, int framew, int fstep)
        {         
            Mat frame = new Mat(frameh, framew, MatType.CV_8UC4, data, fstep);
            //size转化
            var new_size = new OpenCvSharp.Size(framew, frameh);
            Mat out_mat = new Mat(new_size, MatType.CV_8UC3);  //输出mat        
            Cv2.CvtColor(frame, out_mat, ColorConversionCodes.BGRA2BGR);
            //Console.WriteLine("mat size is:" + out_mat.Size());
            Console.WriteLine("RTSP-1:" + frame_index1++);
            Console.WriteLine("thread ID is:" + Thread.CurrentThread.ManagedThreadId.ToString());
            //GC.Collect();
            frame.Dispose();
            out_mat.Dispose();
            //Cv2.ImWrite("./out/" + (frame_index++).ToString() + ".jpg", out_mat);
            //Bitmap currBitmap = BitmapConverter.ToBitmap(out_mat);          

        }

        public static void dectcallbackfunc2(IntPtr data, int frameh, int framew, int fstep)
        {
            Mat frame = new Mat(frameh, framew, MatType.CV_8UC4, data, fstep);
            //size转化
            var new_size = new OpenCvSharp.Size(framew, frameh);
            Mat out_mat = new Mat(new_size, MatType.CV_8UC3);
            Cv2.CvtColor(frame, out_mat, ColorConversionCodes.BGRA2BGR);
            //Console.WriteLine("mat size is:" + out_mat.Size());
            Console.WriteLine("RTSP-2:" + frame_index2++);
            Console.WriteLine("thread ID is:" + Thread.CurrentThread.ManagedThreadId.ToString());
            frame.Dispose();
            out_mat.Dispose();                     

        }
     
        //初始化deepstream pipline获取对应ip的rtsp流
        //public static void Ini1()
        //{
        //    Console.WriteLine("starting ...........");           
        //    Console.WriteLine("thread 1:");
        //    int res1 = rtsp_set_callback(GetCall1);//调用回调函数
        //    if (res1 != 1)
        //    {
        //        Console.WriteLine("callback_fail");
        //    }
        //    string uri = "rtsp://admin:wanji168@10.100.8.64:554/";
        //    int res = rtsp_init_1(uri, SIZE_WIDTH, SIZE_HEIGHT, 3);//初始化pipline
        //    if (res != 1)
        //    {
        //        Console.WriteLine("init fail");
        //    }
            
        //    //for (int j = 0; j < 10; j++)
        //    //{
        //    rtsp_start();//开启rtsp流
        //    Thread.Sleep(TimeSpan.FromMinutes(30));
        //    //Thread.Sleep(TimeSpan.FromSeconds(10));
        //    //    rtsp_pause();
        //    //    Thread.Sleep(100);
        //    //}

        //    //rtsp_release();
        //    Console.WriteLine("current_frame_1:" + frame_index1);
        //    Console.WriteLine("end......");
        //    //return res;
        //}

        //public static void Ini2()
        //{
        //    Console.WriteLine("starting ...........");          
        //    Console.WriteLine("thread 2:");
        //    int res1 = rtsp_set_callback(GetCall2);
        //    if (res1 != 1)
        //    {
        //        Console.WriteLine("callback_fail");
        //    }
        //    string uri = "rtsp://admin:wanji168@10.100.8.75:554/";
        //    int res = rtsp_init_2(uri, SIZE_WIDTH, SIZE_HEIGHT, 3);
        //    if (res != 1)
        //    {
        //        Console.WriteLine("init fail");
        //    }
           
        //    //for (int j = 0; j < 10; j++)
        //    //{
        //    rtsp_start();//开启rtsp流
        //    Thread.Sleep(TimeSpan.FromMinutes(30));
        //    //Thread.Sleep(TimeSpan.FromSeconds(10));
        //    //    rtsp_pause();
        //    //    Thread.Sleep(100);
        //    //}          
        //    //rtsp_release();
        //    Console.WriteLine("current_frame_2:" + frame_index2);
        //    Console.WriteLine("end......");
        //    //return res;
        //}


        //public static void start()
        //{
        //    cv2Video = new Cv2Video();//不加这句会报错
        //    //Ini1();
        //    //Ini2();//线性执行没有问题
        //    Thread thread1 = new Thread(Ini1);
        //    //Thread thread2 = new Thread(Ini2);
        //    thread1.Start();
        //    //thread2.Start();
        //    ////Thread.Sleep(TimeSpan.FromSeconds(5));

        //}



    }
}
