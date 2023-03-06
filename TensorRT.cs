using OpenCvSharp;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace videocapture
{
    public class TensorRT
    {
        static int SIZE_WIDTH = 640;
        static int SIZE_HEIGHT = 640;
        const string DLLPATH = "libyolov5_infer.so";
        //-------------------declaration of Function of dll-------------------
        [DllImport(DLLPATH)]
        public static extern IntPtr infer_init(ModelParam model);
        [DllImport(DLLPATH)]
        public static extern int infer_run_one(byte[] mat_bytes, IntPtr res, ref int resNum, ref float waste, IntPtr pt);

        [Serializable]  // 指示可序列化
        public struct ModelParam
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string modelPath; //.engine file path
            public int classesNum;
            public float iouThres;
            public float confThres;
            public int inputWidth;
            public int inputHeight;
        };

        [Serializable]  // 指示可序列化
        public struct rect
        {
            public int left;
            public int top;
            public int width;
            public int height;
        };


        [Serializable]  // 指示可序列化
        public struct DetectRes
        {
            public int labelId;
            public float confidence;
            public rect rect;
        };

        public static IntPtr Ini()
        {
            TensorRT.ModelParam model = new TensorRT.ModelParam();
            model.modelPath = "s_car.engine";
            model.classesNum = 2;
            model.iouThres = 0.5f;
            model.confThres = 0.4f;
            model.inputWidth = SIZE_WIDTH;
            model.inputHeight = SIZE_HEIGHT;
            return TensorRT.infer_init(model);
        }

        public static bool PredictGPU(Mat mat, out YoloOnnx.PredictResult ret, IntPtr Intpt)
        {
            // 源目标参数  
            int resNum = 0;
            float waste = 0;
            int row = 2560;
            int clomn = 1440;
            int size = Marshal.SizeOf(typeof(DetectRes)) * 128;
            IntPtr pt = Marshal.AllocHGlobal(size);
            ret = new YoloOnnx.PredictResult();

            var bytes = new byte[mat.Total() * 3];//这里必须乘以通道数，不然数组越界，也可以用w*h*c，差不多
            Marshal.Copy(mat.Data, bytes, 0, bytes.Length);

            var rest = infer_run_one(bytes, pt, ref resNum, ref waste, Intpt);         
            //还原成结构体数组
            for (int i = 0; i < resNum; i++)
            {
                var dr = new DetectRes();
                IntPtr ptr = (IntPtr)(pt + i * Marshal.SizeOf(typeof(DetectRes)));
                dr = (DetectRes)Marshal.PtrToStructure(ptr, typeof(DetectRes));

                string lable = "1";
                if (dr.labelId == 0)
                {
                    lable = "0";
                }
                Rect2d box = new Rect2d(dr.rect.left, dr.rect.top, dr.rect.width, dr.rect.height);
                YoloOnnx.PredictResult.Target tar = new YoloOnnx.PredictResult.Target(lable, dr.labelId, dr.confidence, 0, box);
                ret.Targets.Add(tar);
            }
            Marshal.FreeHGlobal(pt);

            if (resNum > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
