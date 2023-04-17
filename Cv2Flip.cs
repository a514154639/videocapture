using OpenCvSharp;

namespace videocapture
{
    public class Cv2Flip
    {
        /// <summary>
        /// 水平翻转
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Mat flipY(Mat image)
        {
            Cv2.Flip(image, image, FlipMode.Y);
            return image;
        }

        /// <summary>
        /// 垂直翻转
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Mat flipX(Mat image)
        {
            Mat dst = new Mat();
            Cv2.Flip(image, dst, FlipMode.X);
            return dst;
        }

        /// <summary>
        /// 水平垂直翻转
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Mat flipXY(Mat image)
        {
            Mat dst = new Mat();
            Cv2.Flip(image, dst, FlipMode.XY);
            return dst;
        }

        public static Mat rotate90(Mat image)
        {
            Cv2.Transpose(image, image);
            //Cv2.Flip(image, image, FlipMode.Y);
            return image;

        }

        public static Mat rotate180(Mat image)
        {
            Cv2.Flip(image, image, FlipMode.X);
            Cv2.Flip(image, image, FlipMode.Y);
            return image;

        }

        public static Mat rotate270(Mat image)
        {
            Cv2.Transpose(image, image);
            Cv2.Flip(image, image, FlipMode.X);
            return image;

        }
    }
}
