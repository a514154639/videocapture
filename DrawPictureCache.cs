using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace videocapture
{
    public class DrawPictureCache
    {
        public ArrayList drawPointList = new ArrayList();//要绘制的所有点
        public class DrawPoint
        {
            public int x;
            public int y;
            public int size;
            public Color color;
        }

        /// <summary>
        ///  清空要绘制的点
        /// </summary>
        public void clearDrawPointList()
        {
            drawPointList.Clear();
        }
        /// <summary>
        /// 添加要绘制的点
        /// x y为原始图片的坐标
        /// </summary>
        /// <param name="x">原始图片的坐标</param>
        /// <param name="y">原始图片的坐标</param>
        /// <param name="size"></param>
        /// <param name="color"></param>
        public int addDrawPointList(int x, int y, int size, Color color)
        {
            DrawPoint drawOne = new DrawPoint();
            drawOne.x = x;
            drawOne.y = y;
            drawOne.size = size;
            drawOne.color = color;
            return drawPointList.Add(drawOne);
        }

        public ArrayList drawLineList = new ArrayList();//要绘制的所有线

        public static List<DrawLine> LineList = new List<DrawLine>();
        //public static DrawLine LineList = new DrawLine();
        public static DrawLine gGetDrawLine = new DrawLine();
        public static DrawRectangle gGetDrawRectangle = new DrawRectangle();
        public class DrawLine
        {
            public int xMin;
            public int yMin;
            public int xMax;
            public int yMax;
            //public double pixeldis;
            //public int size;
            //public Color color;
        }

        /// <summary>
        ///  清空要绘制的线
        /// </summary>
        public void clearDrawLineList()
        {
            drawLineList.Clear();
            //LineList.Clear();
        }

        public void removeDrawLineList(int index)
        {
            drawLineList.RemoveAt(index);
        }

        /// <summary>
        /// 添加要绘制的线
        /// x y为原始图片的坐标
        /// </summary>
        /// <param name="xMin">原始图片的坐标</param>
        /// <param name="yMin">原始图片的坐标</param>
        /// <param name="xMax">原始图片的坐标</param>
        /// <param name="yMax">原始图片的坐标</param>
        /// <param name="size"></param>
        /// <param name="color"></param>
        public int addDrawLineList(int xMin, int yMin, int xMax, int yMax, int size, Color color)
        {
            DrawLine drawOne = new DrawLine();
            drawOne.xMin = xMin;
            drawOne.yMin = yMin;
            drawOne.xMax = xMax;
            drawOne.yMax = yMax;
            //drawOne.size = size;
            //drawOne.color = color;
            gGetDrawLine = drawOne;
            LineList.Add(drawOne);
            //LineList = LineList.OrderBy(o => o.yMax).ToList();
            return drawLineList.Add(drawOne);
        }

        public ArrayList drawRectangleList = new ArrayList();//要绘制的所有矩形
        public class DrawRectangle
        {
            public int xMin;
            public int yMin;
            public int xMax;
            public int yMax;
            //public int size;
            //public Color color;
        }

        /// <summary>
        ///  清空要绘制的矩形
        /// </summary>
        public void clearDrawRectangleList()
        {
            drawRectangleList.Clear();
        }

        public void removeDrawRectangleList(int index)
        {
            drawRectangleList.RemoveAt(index);
        }

        /// <summary>
        /// 添加要绘制的矩形
        /// x y为原始图片的坐标
        /// </summary>
        /// <param name="xMin">原始图片的坐标</param>
        /// <param name="yMin">原始图片的坐标</param>
        /// <param name="xMax">原始图片的坐标</param>
        /// <param name="yMax">原始图片的坐标</param>
        /// <param name="size"></param>
        /// <param name="color"></param>
        public int addDrawRectangleList(int xMin, int yMin, int xMax, int yMax, int size, Color color)
        {
            DrawRectangle drawOne = new DrawRectangle();
            drawOne.xMin = xMin;
            drawOne.yMin = yMin;
            drawOne.xMax = xMax;
            drawOne.yMax = yMax;
            //drawOne.size = size;
            //drawOne.color = color;
            gGetDrawRectangle = drawOne;
            return drawRectangleList.Add(drawOne);
        }

        /// <summary>
        /// 获取DrawRectangle的元素
        /// 根据索引
        /// 原图尺寸
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public float[] getXyFromDrawRectangle(int index)
        {
            float[] xyList = new float[8];
            int xMin = ((DrawRectangle)drawRectangleList[index]).xMin;
            int yMin = ((DrawRectangle)drawRectangleList[index]).yMin;
            int xMax = ((DrawRectangle)drawRectangleList[index]).xMax;
            int yMax = ((DrawRectangle)drawRectangleList[index]).yMax;

            xyList[0] = (float)xMin;
            xyList[1] = (float)yMin;
            xyList[2] = (float)xMax;
            xyList[3] = (float)yMin;
            xyList[4] = (float)xMax;
            xyList[5] = (float)yMax;
            xyList[6] = (float)xMin;
            xyList[7] = (float)yMax;

            return xyList;
        }

    }
}
