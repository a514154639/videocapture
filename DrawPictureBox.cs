using System;
using System.Drawing;
using System.Windows.Forms;

namespace videocapture
{
    public partial class DrawPictureBox : UserControl
    {
        /// <summary>
        /// 缓存要绘制的内容
        /// 点、线、矩形、多边形、写字
        /// </summary>
        public DrawPictureCache drawCache = new DrawPictureCache();

        public DrawPictureBox()
        {
            InitializeComponent();
        }

        public void init(DrawPictureCache drawCache)
        {
            this.drawCache = drawCache;
        }

        public int topLeftX = -1;
        public int topLeftY = -1;

        /// <summary>
        /// 刷新自定义控件在窗口中的左上坐标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void refreshTopLeft(int x, int y)
        {
            topLeftX = x;
            topLeftY = y;
        }

        private void DrawPictureBox_Load(object sender, EventArgs e)
        {
            //事件
            //this.pictureBoxMain.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBoxMain_MouseClick);
        }

        public void setImage(Bitmap bt)
        {
            try
            {
                //Bitmap bitmap = new Bitmap(bt);
                if (bt == null)
                {
                    return;
                }
                this.pictureBoxMain.Image = bt;
                imageWidth = this.pictureBoxMain.Image.Width;
                imageHeight = this.pictureBoxMain.Image.Height;
                picboxWidth = this.pictureBoxMain.Width;
                picboxHeight = this.pictureBoxMain.Height;
                //GC.Collect();
            }
            catch
            {

            }

        }

        public Image getImage()
        {
            return this.pictureBoxMain.Image;
        }

        /// <summary>
        /// 每次添加绘制后 调用一次刷新
        /// </summary>
        public void refresh()
        {
            //this.pictureBoxMain.Refresh();
        }

        public int imageWidth = 0;//图片原始值
        public int imageHeight = 0;//图片原始值
        public int picboxWidth = 0;//图片当前值
        public int picboxHeight = 0;//图片当前值

        /// <summary>
        /// 获取缩放值
        /// 图片当前值 / 图片原始值
        /// float[0]是x float[1]是y
        /// </summary>
        /// <returns></returns>
        public float[] getZoom()
        {
            float[] xyZoom = new float[2];
            if (0 == imageWidth || 0 == imageHeight)
            {
                xyZoom[0] = 1;
                xyZoom[1] = 1;
            }
            else
            {
                int currPictureboxWidth = this.pictureBoxMain.Width;
                int currPictureboxHeight = this.pictureBoxMain.Height;
                float xZoom = (float)currPictureboxWidth / (float)imageWidth;
                float yZoom = (float)currPictureboxHeight / (float)imageHeight;
                xyZoom[0] = xZoom;
                xyZoom[1] = yZoom;
            }
            return xyZoom;
        }


        public void pictureBoxMain_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                //Graphics g = Graphics.FromImage(this.pictureBoxMain.Image);
                //Graphics g = this.pictureBoxMain.CreateGraphics();
                Graphics g = e.Graphics;

                float[] xyZoom = getZoom();
                float xZoom = xyZoom[0];
                float yZoom = xyZoom[1];

                //画控件边界
                Brush b0 = new SolidBrush(Color.Green);
                Pen p0 = new Pen(b0);
                p0.Width = 3;
                g.DrawRectangle(p0, 0, 0, this.pictureBoxMain.Width, this.pictureBoxMain.Height);

                //画所有的点
                //画轮轴与地面接触点               
                for (int index = 0; index < drawCache.drawPointList.Count; index++)
                {
                    DrawPictureCache.DrawPoint one = (DrawPictureCache.DrawPoint)drawCache.drawPointList[index];
                    Brush brush = new SolidBrush(one.color);
                    Pen pen = new Pen(brush);
                    pen.Width = one.size;
                    g.DrawEllipse(pen, one.x * xZoom, one.y * yZoom, one.size, one.size);
                }

                //画所有的线                          
                for (int index = 0; index < drawCache.drawLineList.Count; index++)
                {
                    DrawPictureCache.DrawLine one = (DrawPictureCache.DrawLine)drawCache.drawLineList[index];
                    Brush brush = new SolidBrush(Color.Blue);
                    Pen pen = new Pen(brush);
                    pen.Width = 3;
                    g.DrawLine(pen, one.xMin * xZoom, one.yMin * yZoom, one.xMax * xZoom, one.yMax * yZoom);
                }



                //画所有的矩形框               
                for (int index = 0; index < drawCache.drawRectangleList.Count; index++)
                {
                    DrawPictureCache.DrawRectangle one = (DrawPictureCache.DrawRectangle)drawCache.drawRectangleList[index];
                    Brush brush = new SolidBrush(Color.Blue);
                    Pen pen = new Pen(brush);
                    pen.Width = 3;
                    g.DrawRectangle(pen, one.xMin * xZoom, one.yMin * yZoom, (one.xMax - one.xMin) * xZoom, (one.yMax - one.yMin) * yZoom);
                    //显示序号、宽度                    
                    g.DrawString("N" + (index + 1) + ":" + Math.Abs(one.xMin - one.xMax).ToString(), new Font("微软雅黑", 9, FontStyle.Bold), brush, new PointF(one.xMin * xZoom, one.yMin * yZoom));
                }


                //显示当前像素缩放
                //try
                //{
                //    string text = "  图片像素" + this.pictureBoxMain.Image.Width + "x" +
                //                       this.pictureBoxMain.Image.Height;
                //    imageWidth = this.pictureBoxMain.Image.Width;
                //    imageHeight = this.pictureBoxMain.Image.Height;

                //    text +=
                //        " 控件像素" + this.pictureBoxMain.Width + "x" + this.pictureBoxMain.Height;

                //    picboxWidth = this.pictureBoxMain.Width;
                //    picboxHeight = this.pictureBoxMain.Height;

                //    hwShow = text;
                //}
                //catch { }
            }
            catch { }
        }
        public string hwShow = "";//图片像素控件像素显示

        private void DrawPictureBox_SizeChanged(object sender, EventArgs e)
        {
            refresh();
        }

        /// <summary>
        /// 原始图片像素xy 转 当前图片像素xy
        /// </summary>
        /// <returns></returns>
        public int[] oriXY2CurrXY(int x, int y)
        {
            float[] xy = getZoom();
            float currX = x * xy[0];
            float currY = y * xy[1];
            int[] resXY = new int[2];
            resXY[0] = (int)currX;
            resXY[1] = (int)currY;
            return resXY;
        }

        /// <summary>
        ///  当前图片像素xy 转 原始图片像素xy
        /// </summary>
        /// <returns></returns>
        public int[] currXY2OriXY(int x, int y)
        {
            float[] xy = getZoom();
            float oriX = x / xy[0];
            float oriY = y / xy[1];
            int[] resXY = new int[2];
            resXY[0] = (int)oriX;
            resXY[1] = (int)oriY;
            return resXY;
        }

        /// <summary>
        /// 鼠标点击类型 如drawLine drawRectangle drawPolygon
        /// </summary>
        public string mouseClickType = "";//drawRectangle drawLine 等

        /// <summary>
        /// 画线
        /// </summary>
        private int drawPointX = -1;
        private int drawPointY = -1;

        /// <summary>
        /// 画线
        /// </summary>
        private int drawLineXmin = -1;
        private int drawLineYmin = -1;
        private int drawLineXmax = -1;
        private int drawLineYmax = -1;

        /// <summary>
        /// 画矩形
        /// </summary>
        private int drawRectangleXmin = -1;
        private int drawRectangleYmin = -1;
        private int drawRectangleXmax = -1;
        private int drawRectangleYmax = -1;

        /// <summary>
        /// 画4点多边形
        /// </summary>
        //private int drawPolygonX1 = -1;
        //private int drawPolygonY1 = -1;
        //private int drawPolygonX2 = -1;
        //private int drawPolygonY2 = -1;
        //private int drawPolygonX3 = -1;
        //private int drawPolygonY3 = -1;
        //private int drawPolygonX4 = -1;
        //private int drawPolygonY4 = -1;

        public bool pictureBoxMain_MouseClick_flag = true;//鼠标事件是否响应
        public int drawRectangleMaxNum = 1;//画框的最大数量
        public int drawLineMaxNum = 6;//画线的最大数量
        /// <summary>
        /// 画线、点、矩形、多边形、字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBoxMain_MouseClick(object sender, MouseEventArgs e)
        {
            if (!pictureBoxMain_MouseClick_flag)
            {
                return;
            }

            if ("drawLine" == mouseClickType)
            {
                if (this.drawCache.drawLineList.Count >= drawLineMaxNum)
                {
                    return;
                }

                int x = e.Location.X;
                int y = e.Location.Y;
                if (drawLineXmin == -1 || drawLineYmin == -1)
                {
                    drawLineXmin = x;
                    drawLineYmin = y;
                }
                else
                {
                    drawLineXmax = x;
                    drawLineYmax = y;
                    //int[] min = currXY2OriXY(topLeftX+xMin, topLeftY + yMin);
                    //int[] max = currXY2OriXY(topLeftX+xMax, topLeftY + yMax);
                    int[] min = currXY2OriXY(drawLineXmin, drawLineYmin);
                    int[] max = currXY2OriXY(drawLineXmax, drawLineYmax);
                    this.drawCache.addDrawLineList(min[0], min[1], max[0], max[1], 3, Color.Blue);
                    refresh();
                    drawLineXmin = -1;
                    drawLineYmin = -1;
                    drawLineXmax = -1;
                    drawLineYmax = -1;
                }
            }
            else if ("drawLineV" == mouseClickType)
            {
                if (this.drawCache.drawLineList.Count >= drawLineMaxNum)
                {
                    return;
                }
                if(this.drawCache.drawLineList.Count > 2)
                {
                    this.drawCache.clearDrawLineList();
                }

                int x = e.Location.X;
                int y = e.Location.Y;
                if (drawLineXmin == -1 || drawLineYmin == -1)
                {
                    drawLineXmin = x;
                    drawLineYmin = y;
                }
                else
                {
                    drawLineXmax = drawLineXmin;
                    drawLineYmax = y;
                    //int[] min = currXY2OriXY(topLeftX+xMin, topLeftY + yMin);
                    //int[] max = currXY2OriXY(topLeftX+xMax, topLeftY + yMax);
                    int[] min = currXY2OriXY(drawLineXmin, drawLineYmin);
                    int[] max = currXY2OriXY(drawLineXmax, drawLineYmax);
                    this.drawCache.addDrawLineList(min[0], min[1], max[0], max[1], 3, Color.Blue);
                    refresh();
                    drawLineXmin = -1;
                    drawLineYmin = -1;
                    drawLineXmax = -1;
                    drawLineYmax = -1;
                }
            }
            else if ("drawLineX" == mouseClickType)
            {
                if (this.drawCache.drawLineList.Count >= drawLineMaxNum)
                {
                    return;
                }

                int x = e.Location.X;
                int y = e.Location.Y;
                if (drawLineXmin == -1 || drawLineYmin == -1)
                {
                    drawLineXmin = x;
                    drawLineYmin = y;
                }
                else
                {
                    drawLineXmax = x;
                    drawLineYmax = drawLineYmin;
                    //int[] min = currXY2OriXY(topLeftX+xMin, topLeftY + yMin);
                    //int[] max = currXY2OriXY(topLeftX+xMax, topLeftY + yMax);
                    int[] min = currXY2OriXY(drawLineXmin, drawLineYmin);
                    int[] max = currXY2OriXY(drawLineXmax, drawLineYmax);
                    this.drawCache.addDrawLineList(min[0], min[1], max[0], max[1], 3, Color.Blue);
                    refresh();
                    drawLineXmin = -1;
                    drawLineYmin = -1;
                    drawLineXmax = -1;
                    drawLineYmax = -1;

                }
            }
            else if ("drawRectangle" == mouseClickType)
            {
                if (this.drawCache.drawRectangleList.Count >= drawRectangleMaxNum)
                {
                    return;
                }

                int x = e.Location.X;
                int y = e.Location.Y;
                if (drawRectangleXmin == -1 || drawRectangleYmin == -1)
                {
                    drawRectangleXmin = x;
                    drawRectangleYmin = y;
                }
                else
                {
                    drawRectangleXmax = x;
                    drawRectangleYmax = y;
                    //int[] min = currXY2OriXY(topLeftX+xMin, topLeftY + yMin);
                    //int[] max = currXY2OriXY(topLeftX+xMax, topLeftY + yMax);
                    int[] min = currXY2OriXY(drawRectangleXmin, drawRectangleYmin);
                    int[] max = currXY2OriXY(drawRectangleXmax, drawRectangleYmax);
                    this.drawCache.addDrawRectangleList(min[0], min[1], max[0], max[1], 3, Color.Blue);
                    refresh();
                    drawRectangleXmin = -1;
                    drawRectangleYmin = -1;
                    drawRectangleXmax = -1;
                    drawRectangleYmax = -1;
                }
            }
            else if ("drawPoint" == mouseClickType)
            {
                drawPointX = e.Location.X;
                drawPointY = e.Location.Y;
                int[] res = currXY2OriXY(drawPointX, drawPointY);
                this.drawCache.addDrawPointList(res[0], res[1], 3, Color.Blue);
                refresh();
                //if (drawPointX == -1 || drawPointY == -1)
                //{
                //    drawPointX = x;
                //    drawPointY = y;
                //}
                //else
                //{
                //    drawPointX = x;
                //    drawPointY = y;
                //    int[] res = currXY2OriXY(drawPointX, drawPointY);
                //    this.drawCache.addDrawPointList(res[0], res[1], 3, Color.Blue);
                //    refresh();
                //    drawPointX = -1;
                //    drawPointY = -1;
                //}

            }

        }

    }
}
