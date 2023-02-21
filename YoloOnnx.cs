using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
//using JGCapture.GLOBAL;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using OpenCvSharp;
using OpenCvSharp.Dnn;

namespace videocapture
{
    public class YoloOnnx
    {
        private static YoloOnnx _ = null;
        private InferenceSession _Session = null;
        private int SIZE_WIDTH = 640;
        private int SIZE_HEIGHT = 640;
        private float ConfidenceThreshold = 0.3f;
        private float NmsThreshold = 0.5f;
        private Stopwatch Sw = new Stopwatch();

        private string[] _Labels = new[] { "0", "1" };

        public static YoloOnnx GetInstance()
        {
            if (_ == null)
            {
                _ = new YoloOnnx();
            }
            return _;
        }

        private YoloOnnx()
        {
            _Session = new InferenceSession(".\\Model\\s_car.onnx");
        }

        public bool Predict(Mat org, out PredictResult result)
        {
            using (Mat mat = org.Clone())
            {
                float ratio = (float)SIZE_WIDTH / Math.Max(mat.Width, mat.Height);
                int new_w = (int)(mat.Width * ratio);
                new_w = new_w > SIZE_WIDTH - 1 ? SIZE_WIDTH : new_w;
                int new_h = (int)(mat.Height * ratio);
                new_h = new_h > SIZE_HEIGHT - 1 ? SIZE_HEIGHT : new_h;
                int dw = SIZE_WIDTH - new_w;
                int dh = SIZE_HEIGHT - new_h;
                var dw1 = dw / 2;
                var dh1 = dh / 2;
                var dw2 = dw - dw1;
                var dh2 = dh - dh1;
                var newSize = new Size(new_w, new_h);
                Cv2.Resize(mat, mat, newSize, 0, 0, InterpolationFlags.Linear);
                Cv2.CopyMakeBorder(mat, mat, dh1, dh2, dw1, dw2, BorderTypes.Constant,
                    new Scalar(114, 114, 114));
                result = Predict(mat);
                TransResult(result, org);
                mat.Dispose();
            }
            if (result == null)
            {
                return false;
            }
            else
            {
                result.Targets = result.Targets.OrderBy(o => o.Box.X).ToList();
                return true;
            }
        }

        private PredictResult Predict(Mat mat)
        {
            Cv2.CvtColor(mat, mat, ColorConversionCodes.BGR2RGB);
            mat.ConvertTo(mat, MatType.CV_32FC1);
            mat = (mat / 255f).ToMat();
            var splitMats = Cv2.Split(mat);
            using (var tmp = new Mat())
            {
                Cv2.HConcat(new[] { splitMats[0].Reshape(1, 1), splitMats[1].Reshape(1, 1), splitMats[2].Reshape(1, 1) }, tmp);
                float[] buf = new float[tmp.Width * tmp.Height];
                Marshal.Copy(tmp.Data, buf, 0, buf.Length);
                Memory<float> mem = new Memory<float>(buf);
                DenseTensor<float> input = new DenseTensor<float>(mem, new[] { 1, 3, SIZE_HEIGHT, SIZE_WIDTH });
                var inputs = new List<NamedOnnxValue>
                {
                    NamedOnnxValue.CreateFromTensor(GetInputName(_Session), input)
                };
                Sw.Restart();
                IDisposableReadOnlyCollection<DisposableNamedOnnxValue> output = _Session.Run(inputs, GetOutputNames(_Session));
                Sw.Stop();
                var result = GetResultDirect(output);
                result.CostTime = Sw.ElapsedMilliseconds;
                return result;
            }
        }

        private PredictResult GetResult(
            IDisposableReadOnlyCollection<DisposableNamedOnnxValue> results)
        {
            var tmp = results.ElementAt(0).AsTensor<float>().ToArray();
            using (var output = new Mat(tmp.Length / (5 + _Labels.Length), 5 + _Labels.Length, MatType.CV_32FC1, tmp))
            {
                float[,] anchors =
                {
                    {10.0f, 13.0f, 16.0f, 30.0f, 33.0f, 23.0f}, {30.0f, 61.0f, 62.0f, 45.0f, 59.0f, 119.0f},
                    {116.0f, 90.0f, 156.0f, 198.0f, 373.0f, 326.0f}
                };
                float[] stride = { 8.0f, 16.0f, 32.0f };

                var classIds = new List<int>();
                var confidences = new List<float>();
                var probabilities = new List<float>();
                var boxes = new List<Rect2d>();

                float ratioh = 1f;//(float)image.Height / SIZE_HEIGHT;
                float ratiow = 1f;//(float)image.Width / SIZE_WIDTH;

                int row_ind = 0;
                for (int n = 0; n < 3; n++)
                {
                    int num_grid_x = (int)(SIZE_WIDTH / stride[n]);
                    int num_grid_y = (int)(SIZE_HEIGHT / stride[n]);
                    for (int q = 0; q < 3; q++)
                    {
                        float anchor_w = anchors[n, q * 2];
                        float anchor_h = anchors[n, q * 2 + 1];
                        for (int i = 0; i < num_grid_y; i++)
                        {
                            for (int j = 0; j < num_grid_x; j++)
                            {
                                var confidence = sigmoid_x(output.Get<float>(row_ind, 4));
                                if (confidence > ConfidenceThreshold)
                                {
                                    Point minLoc, maxLoc;
                                    Cv2.MinMaxLoc(output.Row(row_ind).ColRange(5, output.Cols), out minLoc,
                                        out maxLoc);
                                    var prob = sigmoid_x(output.Get<float>(row_ind, 5 + maxLoc.X));
                                    if (prob > ConfidenceThreshold)
                                    {
                                        var cx = (sigmoid_x(output.Get<float>(row_ind, 0)) * 2f - 0.5f + j) *
                                                 stride[n];
                                        var cy = (sigmoid_x(output.Get<float>(row_ind, 1)) * 2f - 0.5f + i) *
                                                 stride[n];
                                        var w = Math.Pow(sigmoid_x(output.Get<float>(row_ind, 2)) * 2f, 2f) *
                                                anchor_w;
                                        var h = Math.Pow(sigmoid_x(output.Get<float>(row_ind, 3)) * 2f, 2f) *
                                                anchor_h;
                                        var left = (cx - 0.5f * w) * ratiow;
                                        var top = (cy - 0.5f * h) * ratioh;
                                        classIds.Add(maxLoc.X);
                                        confidences.Add(confidence);
                                        probabilities.Add(prob);
                                        boxes.Add(new Rect2d(left, top, w * ratiow, h * ratioh));
                                    }
                                }
                                row_ind++;
                            }
                        }
                    }
                }

                int[] indices;
                CvDnn.NMSBoxes(boxes, confidences, ConfidenceThreshold, NmsThreshold, out indices);
                PredictResult result = new PredictResult();
                foreach (var i in indices)
                {
                    result.Targets.Add(new PredictResult.Target(_Labels[classIds[i]], classIds[i], confidences[i],
                        probabilities[i], boxes[i]));
                }
                return result;
            }
        }

        private PredictResult GetResultDirect(
           IDisposableReadOnlyCollection<DisposableNamedOnnxValue> results)
        {
            var tmp = results.ElementAt(0).AsTensor<float>().ToArray();
            using (var output = new Mat(tmp.Length / (5 + _Labels.Length), 5 + _Labels.Length, MatType.CV_32FC1, tmp))
            {
                var classIds = new List<int>();
                var confidences = new List<float>();
                var probabilities = new List<float>();
                var boxes = new List<Rect2d>();
                for (int i = 0; i < output.Rows; i++)
                {
                    var confidence = output.Get<float>(i, 4);
                    if (confidence > ConfidenceThreshold)
                    {
                        Point minLoc, maxLoc;
                        Cv2.MinMaxLoc(output.Row(i).ColRange(5, output.Cols), out minLoc,
                            out maxLoc);
                        var prob = (output.Get<float>(i, 5 + maxLoc.X));
                        if (prob > ConfidenceThreshold)
                        {
                            var cx = output.Get<float>(i, 0);
                            var cy = output.Get<float>(i, 1);
                            var w = output.Get<float>(i, 2);
                            var h = output.Get<float>(i, 3);
                            var left = cx - w / 2f;
                            var top = cy - h / 2f;
                            classIds.Add(maxLoc.X);
                            confidences.Add(confidence);
                            probabilities.Add(prob);
                            boxes.Add(new Rect2d(left, top, w, h));
                        }
                    }
                }

                int[] indices;
                CvDnn.NMSBoxes(boxes, confidences, ConfidenceThreshold, NmsThreshold, out indices);
                PredictResult result = new PredictResult();
                foreach (var i in indices)
                {
                    result.Targets.Add(new PredictResult.Target(_Labels[classIds[i]], classIds[i], confidences[i],
                        probabilities[i], boxes[i]));
                }
                return result;
            }
        }

        private float sigmoid_x(float x)
        {
            return (float)(1f / (1f + Math.Exp(-1 * x)));
        }

        private void TransResult(PredictResult result, Mat org)
        {
            float ratio = (float)SIZE_WIDTH / Math.Max(org.Width, org.Height);
            var newSize = new Size(org.Width * ratio, org.Height * ratio);
            int dw = (SIZE_WIDTH - newSize.Width) / 2;
            int dh = (SIZE_HEIGHT - newSize.Height) / 2;
            foreach (var item in result.Targets)
            {
                item.Box = new Rect2d((item.Box.X - dw) / ratio, (item.Box.Y - dh) / ratio, item.Box.Width / ratio, item.Box.Height / ratio);
                item.Box.Width = Math.Min(org.Width - item.Box.X, item.Box.Width);
                item.Box.Height = Math.Min(org.Height - item.Box.Y, item.Box.Height);
            }
        }

        private string GetInputName(InferenceSession session)
        {
            return session.InputMetadata.Keys.ElementAt(0);
        }

        private List<string> GetInputNames(InferenceSession session)
        {
            List<string> inputNames = new List<string>();
            foreach (var name in session.InputMetadata.Keys)
            {
                inputNames.Add(name);
            }
            return inputNames;
        }

        private string GetOutputName(InferenceSession session)
        {
            return session.OutputMetadata.Keys.ElementAt(0);
        }

        private List<string> GetOutputNames(InferenceSession session)
        {
            List<string> outputNames = new List<string>();
            foreach (var name in session.OutputMetadata.Keys)
            {
                outputNames.Add(name);
            }
            return outputNames;
        }

        public class PredictResult : IDisposable
        {
            public long CostTime = 0;
            public List<Target> Targets = new List<Target>();
            public Mat Img;
            public PredictResult()
            {
            }

            public PredictResult(List<Target> targets)
            {
                Targets = targets;
            }

            public class Target
            {
                public string Label;
                public int ClassId;
                public float Confidence;
                public float Probability;
                public Rect2d Box;

                public Target(string label, int classId, float confidence, float probability, Rect2d box)
                {
                    Label = label;
                    ClassId = classId;
                    Confidence = confidence;
                    Probability = probability;
                    Box = box;
                }

                public Point2d GetBoxCenter()
                {
                    return new Point2d(Box.X + Box.Width / 2, Box.Y + Box.Height / 2);
                }
            }

            ~PredictResult()
            {
            }

            public void Dispose()
            {
            }
        }
    }
}
