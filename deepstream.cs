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
        static int frame_index = 0;
        public static int isSuccess = 0;

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
        public static extern int mRead_Opencv(int id, ref int width, ref int height, ref int size, IntPtr data);
            

        public static void start()
        {
            const string url1 = "rtsp://admin:wanji168@10.100.8.64:554/0";
            //初始化
            init();
            //创建rtsp实例 mode TCP_CONN_MODE = 1 DP_CONN_MODE = 2
            createRtspClient(0, url1, 1);
            int status = 0;
            
            do
            {
                Console.WriteLine("id:0 connected\n");
                
                if (isConnect(0) == 1)
                {
                    //Thread.Sleep(500);
                    Console.WriteLine("id:0 connected\n");
                    //opencv
                    int width = 0;
                    int height = 0;
                    int size = 0;
                    byte[] buffer = new byte[SIZE_WIDTH * SIZE_HEIGHT * 3];
                    GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                    IntPtr data = handle.AddrOfPinnedObject();
 
                    status = mRead_Opencv(0,ref width,ref height,ref size, data);
                    if (status == 1)
                    {
                        Mat frame = new Mat(height, width, MatType.CV_8UC3, data);
                        var new_size = new OpenCvSharp.Size(width, height);
                        Mat out_mat = new Mat(new_size, MatType.CV_8UC3);  //输出mat        
                        Cv2.CvtColor(frame, out_mat, ColorConversionCodes.BGRA2BGR);
                        Console.WriteLine(out_mat.Size());
                        Cv2.ImWrite("/out/" + (frame_index++).ToString() + ".jpg", out_mat);
                        frame.Dispose();
                        out_mat.Dispose();

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

            }
            while (true);


        }

    } 
}
