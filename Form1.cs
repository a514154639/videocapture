using Newtonsoft.Json;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace videocapture
{
    public partial class Form1 : Form
    {
        public int laneNum = 1;//车道数
        public int flag = 99;//相机标志
        public int press = 99;
        public static bool isopen = false;//是否播放 视频线程用 opencv
        public static bool flagpsw = true;
        public int rotate = 0;
        public bool isrotate = false;
        private Thread videoThr = null;//视频线程
        public Bitmap currBitmap = null;//当前播放的帧 

        public static ExtraConfig extraConfig = new ExtraConfig();
        public static ExtraConfig_new extraConfig_new = new ExtraConfig_new();
        public static TotalConfig totalConfig = new TotalConfig();
        public static LineConfig lineConfig = new LineConfig();

        public Cv2Video cv2Video = null;//实例
        private Ipinput ipinput = new Ipinput();
        private laneInput laneinput = new laneInput();
        public string IP = "";
        public string CAM_IP = "";
        public string ipPsw = "";
        public string Lane_Num_str = "";
        public int stopframe = 0;
        public double landwidth = 3.75;
        public double landwidthpix = 0;

        public int Struler = 0;
        public double count = 0;
        public bool line_mode = false;
        public bool cover_mode = false;
        public bool main_cam_status = false;

        // 定义IP地址和密码的成员变量
        private string ipAddress;
        private string password;
        private string username;
        private const string BaseUrl = "rtsp://admin:";
        //private const string BaseUrl = "rtsp://";

        private ManualResetEvent pauseEvent = new ManualResetEvent(true);

        public Form1()
        {
            InitializeComponent();
            //ipinput.sendip += new Ipinput.SendMesg(Receiveip);
            //ipinput.sendpassward += new Ipinput.SendMesg(Receivepsw);
        }
        private void VideoForm_Load(object sender, EventArgs e)
        {
            //主线程
            videoThr = new Thread(VideoThread);
            videoThr.IsBackground = true;
            videoThr.Start();

            pictureBox1.Load(@"road.jpg");
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        }
        public class TotalConfig
        {
            public int nums;
            public string type;
            public List<ExtraConfig> config = new List<ExtraConfig>();
            public List<ExtraConfig_new> config_news = new List<ExtraConfig_new>();
        }

        public class ExtraConfig
        {
            public string ip;
            public string psw;
            public DrawPictureCache.DrawLine line;//新的策略中line修改为List 其中包括车道号 相机信息
            public bool state = false;
            public int lane;
            public int struler;

            public bool line_x_mode = false;
            public bool cover_mode = false;

        }

        //新配置文件结构
        //line用list来表示，其中可能有多条线
        public class ExtraConfig_new
        {
            public string ip;
            public string psw;
            public List<LineConfig> line = new List<LineConfig>();
            public bool state = false;
        }

        //每条线的具体配置
        public class LineConfig
        {
            //public DrawPictureCache.DrawLine line;
            public int xMin;
            public int yMin;
            public int xMax;
            public int yMax;
            public int lane;
            public int struler;
            public bool line_x_mode = false;
            public bool cover_mode = false;
            public bool ismain_cam = false;
        }
        private void ReceiveLaneNum(string str)
        {
            if(str == "")
            {
                Lane_Num_str = "0";
            }
            else
            {
                Lane_Num_str = str;
            }
            
        }
        private void ReceiveMainCamStatus(bool sta)
        {
            main_cam_status = sta;
        }


        private void Receiveip(string str)
        {
            IP = str;
        }

        private void Receivepsw(string str)
        {
            ipPsw = str;
            ipPsw += "@";
        }
        private void ReadConfig()
        {
            //this.drawPictureBoxVideo.drawCache.clearDrawRectangleList();
            this.drawPictureBoxVideo.drawCache.clearDrawLineList();
            try
            {
                if (File.Exists("config.json"))
                {
                    var extras = JsonConvert.DeserializeObject<TotalConfig>(File.ReadAllText("config.json"));
                    
                    extraConfig_new = extras.config_news[flag];
                    if (extras.config_news[flag].line[0].xMin == 0)
                    {
                        info_box.AppendText("未配置参数\r\n");
                        //MessageBox.Show("未配置参数");
                        return;
                    }
                    info_box.AppendText("------------\r\n");
                    info_box.AppendText("当前视频尺寸：" + currBitmap.Width.ToString() + "×" + currBitmap.Height.ToString() + "\r\n");
                    info_box.AppendText("最终标尺：" + extraConfig.struler + "\r\n");
                    info_box.AppendText("------------\r\n");
                    check_line_mode.Checked = extraConfig.line_x_mode;
                    check_cover_mode.Checked = extraConfig.cover_mode;

                    for (int i = 0; i < extraConfig_new.line.Count; i++)
                    {
                        this.drawPictureBoxVideo.drawCache.addDrawLineList(extraConfig_new.line[i].xMin, 
                            extraConfig_new.line[i].yMin, 
                            extraConfig_new.line[i].xMax, 
                            extraConfig_new.line[i].yMax, 3, Color.Blue);
                    }
                        
                    //MessageBox.Show("参数读取成功");
                    info_box.AppendText("参数读取成功\r\n");
                }
                else
                {
                    info_box.AppendText("参数文件不存在\r\n");
                    //MessageBox.Show("参数文件不存在");
                }
                //this.drawPictureBoxVideo.refresh();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + "参数读取失败");
            }
        }

        //List<ExtraConfig> configs = new List<ExtraConfig>();

        private void SetConfig()
        {
            try
            {
                if (check_line_mode.Checked)
                {
                    line_mode = true;
                }
                else
                {
                    line_mode = false;
                }
                if (check_cover_mode.Checked)
                {
                    cover_mode = true;
                }
                else
                {
                    cover_mode = false;
                }

                extraConfig.line = DrawPictureCache.gGetDrawLine;

                if (File.Exists("config.json"))
                {
                    var extras = JsonConvert.DeserializeObject<TotalConfig>(File.ReadAllText("config.json"));

                    if (DrawPictureCache.LineList.Count > extras.config_news[flag].line.Count)
                    {
                        MessageBox.Show("未配置画线对应的车道号，请先进行画线配置");
                        return;
                    }
                    var currentConfig = extras.config_news[flag];
                    for (int i = 0; i < DrawPictureCache.LineList.Count; i++)
                    {
                        //extras.config_news[flag].line[i].line = DrawPictureCache.LineList[i];
                        currentConfig.line[i].xMin = DrawPictureCache.LineList[i].xMin;
                        currentConfig.line[i].yMin = DrawPictureCache.LineList[i].yMin;
                        currentConfig.line[i].xMax = DrawPictureCache.LineList[i].xMax;
                        currentConfig.line[i].yMax = DrawPictureCache.LineList[i].yMax;
                        currentConfig.line[i].struler = Struler;
                        currentConfig.line[i].line_x_mode = line_mode;
                        currentConfig.line[i].cover_mode = cover_mode;
                    }

                    totalConfig = extras;
                    Convert(totalConfig);
                }


                info_box.AppendText("参数保存成功\r\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            Cleanline();
            //CleanRectangle();

        }

        //车道号配置
        private void button_lane_Click(object sender, EventArgs e)
        {
            var extras = JsonConvert.DeserializeObject<TotalConfig>(File.ReadAllText("config.json"));
            using (laneInput laneinput = new laneInput())
            {
                laneinput.SendlaneNum += new laneInput.SendMesg(ReceiveLaneNum);
                laneinput.SendisMainCam += new laneInput.SendStatus(ReceiveMainCamStatus);
                laneinput.ShowDialog();
                if (Lane_Num_str == "")
                {
                    Lane_Num_str = "0";
                }
                //一条线直接赋值
                if (DrawPictureCache.LineList.Count == 1)
                {
                    extras.config_news[flag].line[0].lane = int.Parse(Lane_Num_str);
                    extras.config_news[flag].line[0].ismain_cam = main_cam_status;
                }
                else
                {
                    //多条线添加
                    extras.config_news[flag].line.Add(new LineConfig
                    {
                        // 在这里初始化LineConfig的属性
                        lane = int.Parse(Lane_Num_str),
                        ismain_cam = main_cam_status

                    });

                }              

            }                
            totalConfig = extras;
            Convert(totalConfig);
            info_box.AppendText("车道号配置成功\r\n");
        }


        private void Cleanline()
        {
            //清除线和点
            drawLineToolStripMenuItem.Text = drawLineToolStripMenuItem.Text.Replace("*", "");
            drawcombainframe.Text = drawcombainframe.Text.Replace("*", "");
            this.drawPictureBoxVideo.mouseClickType = "";
            this.drawPictureBoxVideo.drawCache.clearDrawLineList();
            this.drawPictureBoxVideo.drawCache.clearDrawPointList();
            this.drawPictureBoxVideo.refresh();
            this.drawPictureBoxVideo.mouseClickType = "";
        }

        //画线
        private void DrawLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (drawLineToolStripMenuItem.Text.Contains("*"))
            {
                //清除线
                Cleanline();
            }
            else
            {
                drawLineToolStripMenuItem.Text += "*";
                this.drawPictureBoxVideo.mouseClickType = "drawLineV";
                //info_box.AppendText("开始画线\r\n");
            }
        }

        //读配置
        private void ReadConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReadConfig();
            //this.drawRangeToolStripMenuItem.Text = "画区域*";
            this.drawLineToolStripMenuItem.Text = "画线*";
        }

        //写配置
        private void SetConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (can_saveconfig())
            //{
            //    setConfig();
            //}
            //else
            //{
            //    MessageBox.Show("当前线框不合理");
            //}
            SetConfig();

        }

        //视频线程
        private void VideoThread()
        {
            while (true)
            {
                try
                {
                    ShowCurrVideoFrame();
                }
                catch { }
                Thread.Sleep(10);
            }
        }

        //获取当前帧
        private void ShowCurrVideoFrame_old()
        {
            this.Invoke(new ThreadStart(delegate
            {
                //info_box.AppendText("主线程\r\n");
                try
                {
                    if (isopen)
                    {
                        if (currBitmap != null)
                        {
                            currBitmap.Dispose();


                        }
                        currBitmap = null;
                        currBitmap = cv2Video.currFrameGetImage();//当前帧

                        if (currBitmap != null)
                        {
                            using (Graphics g = Graphics.FromImage(currBitmap))
                            {
                                #region 中心线
                                var pen = new Pen(Color.Red, 10);

                                //垂直中线
                                g.DrawLine(pen, new System.Drawing.Point(currBitmap.Width / 2, 0), new System.Drawing.Point(currBitmap.Width / 2, currBitmap.Height));
                                //水平中线
                                g.DrawLine(pen, new System.Drawing.Point(0, currBitmap.Height / 2), new System.Drawing.Point(currBitmap.Width, currBitmap.Height / 2));

                                #endregion
                                #region 网格线
                                if (gridline.Text.Contains("*"))
                                {
                                    pen = new Pen(Color.Red, 2);
                                    //垂直
                                    for (int i = 0; i < currBitmap.Width; i += 60)
                                    {
                                        if (i > 0)
                                        {
                                            g.DrawLine(pen, new System.Drawing.Point(i, currBitmap.Height / 2), new System.Drawing.Point(i, currBitmap.Height));
                                        }
                                    }
                                    //水平
                                    for (int i = 0; i < currBitmap.Height / 2; i += 60)
                                    {
                                        if (i > 0)
                                        {
                                            g.DrawLine(pen, new System.Drawing.Point(0, i + currBitmap.Height / 2 - 60), new System.Drawing.Point(currBitmap.Width, i + currBitmap.Height / 2 - 60));
                                        }
                                    }
                                }
                                #endregion

                            }
                            //显示
                            this.Invoke(new ThreadStart(delegate
                            {
                                //info_box.AppendText("显示线程\r\n");                             
                                drawPictureBoxVideo.setImage(currBitmap);//显示

                            }));


                            //Cv2.WaitKey(1);
                        }
                        else
                        {
                            info_box.AppendText("当前帧为空\r\n");
                            //currBitmap = cv2Video.currFrameGetImage();
                            Cam_op(flag);
                            //count++;
                            //info_box.AppendText("重连次数 :"+ count + "\r\n");
                            //isopen = false;

                        }

                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

            }));
        }


        private void ShowCurrVideoFrame()
        {
            this.Invoke(new ThreadStart(delegate
            {
                try
                {
                    if (isopen)
                    {
                        currBitmap?.Dispose();
                        currBitmap = cv2Video.currFrameGetImage(); // 当前帧

                        if (currBitmap != null)
                        {
                            //网格线
                            graphicsUpdate(currBitmap);

                            // 显示
                            this.BeginInvoke(new Action(() =>  // 异步UI更新
                            {
                                drawPictureBoxVideo.setImage(currBitmap); // 显示

                            }));

                        }
                        else
                        {
                            info_box.AppendText("当前帧为空\r\n");
                            Cam_op(flag);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }));
        }


        private void DrawGrid(Graphics g, int cellSize)
        {
            using (var pen = new Pen(Color.Red, 1))
            {
                // 绘制垂直线
                for (int i = 0; i < currBitmap.Width; i += cellSize)
                {
                    g.DrawLine(pen, new Point(i, 0), new Point(i, currBitmap.Height));
                }

                // 绘制水平线
                for (int i = 0; i < currBitmap.Height; i += cellSize)
                {
                    g.DrawLine(pen, new Point(0, i), new Point(currBitmap.Width, i));
                }
            }
        }

        private void DrawCrosshair(Graphics g)
        {
            using (var pen = new Pen(Color.Red, 10))
            {
                // 绘制垂直中线
                g.DrawLine(pen, new Point(currBitmap.Width / 2, 0), new Point(currBitmap.Width / 2, currBitmap.Height));

                // 绘制水平中线
                g.DrawLine(pen, new Point(0, currBitmap.Height / 2), new Point(currBitmap.Width, currBitmap.Height / 2));
            }
        }

        //中心线网格线
        private void graphicsUpdate(Bitmap currBitmap)
        {
            // 在当前的位图上绘制
            using (Graphics g = Graphics.FromImage(currBitmap))
            {
                // 绘制中心线
                DrawCrosshair(g);

                // 检查是否需要绘制网格线
                if (gridline.Text.Contains("*"))
                {
                    // 假设网格线的间距是60px，你可以根据需要修改
                    DrawGrid(g, 60);
                }

                // TODO: 在这里可以添加更多自定义的绘图代码
            }

            // 必须在UI线程上执行pictureBox的Image更新
            this.Invoke(new Action(() =>  // 异步UI更新
            {
                drawPictureBoxVideo.setImage(currBitmap); // 显示
            }));
        }

        //显示图片
        private void ShowImage(string str)
        {
            cv2Video?.dispose();
            cv2Video = new Cv2Video();
            info_box.AppendText("连接中，请稍后...\r\n");
            try
            {
                //isopen = cv2Video.openRtsp($"{str}:554"); // 显示
                isopen = cv2Video.openVideoFile("demo.mp4");
                if (isopen)
                {
                    info_box.AppendText("相机" + CAM_IP);
                    info_box.AppendText("连接成功\r\n");
                    info_box.AppendText("------------\r\n");
                    flagpsw = true;
                    return;
                }
                else
                {
                    cv2Video = null;
                    info_box.AppendText("无法连接，请检查ip密码\r\n");
                    flagpsw = false;
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


        }



        //检查ip是否能通
        public bool IsPingable(string ipAddress)
        {
            try
            {
                Ping pingSender = new Ping();
                PingOptions options = new PingOptions();
                options.DontFragment = true;
                string data = "abcdefghijklmnopqrstuvwxyz1234567890";
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                int timeout = 3000;
                PingReply reply = pingSender.Send(ipAddress, timeout, buffer, options);
                if (reply.Status == IPStatus.Success)
                {
                    return true;
                }
            }
            catch (Exception)
            {
                // ignore exception and return false
            }
            return false;
        }


        //检查画的线和框有没有保存
        private void CheckLine()
        {
            var extras = JsonConvert.DeserializeObject<TotalConfig>(File.ReadAllText("config.json"));
            if (drawPictureBoxVideo.drawCache.drawLineList.Count != 0)
            {
                //if (extras.type.Contains("0"))
                //{
                //    press--;//当没有应急车道的时候相机下标需要减一 cam 1 2 3 -> confi 0 1 2
                //}
                if (extras.config_news[press].line[0] != null)
                {
                    DrawPictureCache.DrawLine line1 = (DrawPictureCache.DrawLine)drawPictureBoxVideo.drawCache.drawLineList[0];
                    var line2 = extras.config_news[press].line[0];
                    int ans3 = line1.xMax + line1.xMin + line1.yMax + line1.yMin;
                    int ans4 = line2.xMax + line2.xMin + line2.yMax + line2.yMin;
                    if (ans3 != ans4) { info_box.AppendText("当前配置未保存\r\n"); }

                }
                else
                {
                    info_box.AppendText("当前配置未保存\r\n");
                    //MessageBox.Show("当前配置未保存");
                }

            }

        }


        //相机0
        private void Cam_0_Click(object sender, EventArgs e)
        {
            if (this.checkBox0.Checked == true)
            {
                Cam_op(0);
            }
            else
            {
                info_box.AppendText("相机未启用\r\n");
                return;
            }

        }

        //相机1
        private void Cam_1_Click(object sender, EventArgs e)
        {
            if (this.checkBox0.Checked == true)
            {
                string selectedValue = this.roadtype_box.SelectedItem.ToString();
                switch (selectedValue)
                {
                    case "2+0车道":
                        Cam_op(0);
                        info_box.AppendText("选择相机2\r\n");
                        break;
                    case "服务区_入口":
                        Cam_op(1);
                        info_box.AppendText("选择相机1\r\n");
                        break;
                    case "服务区_出口":
                        Cam_op(1);
                        info_box.AppendText("选择相机1\r\n");
                        break;
                    case "2+1车道":
                        Cam_op(1);
                        info_box.AppendText("选择相机2\r\n");
                        break;
                    case "3+0车道":
                        Cam_op(0);
                        info_box.AppendText("选择相机3\r\n");
                        break;
                    case "3+1车道":
                        Cam_op(1);
                        info_box.AppendText("选择相机3\r\n");
                        break;
                }

            }
            else
            {
                info_box.AppendText("相机未启用\r\n");
                //MessageBox.Show("相机未启用");
                return;
            }
        }

        //相机2
        private void Cam_2_Click(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked == true)
            {
                string selectedValue = this.roadtype_box.SelectedItem.ToString();
                switch (selectedValue)
                {
                    case "2+0车道":
                        Cam_op(1);
                        info_box.AppendText("选择相机1\r\n");
                        break;
                    case "2+1车道":
                        Cam_op(2);
                        info_box.AppendText("选择相机1\r\n");
                        break;
                    case "3+0车道":
                        Cam_op(1);
                        info_box.AppendText("选择相机2\r\n");
                        break;
                    case "3+1车道":
                        Cam_op(2);
                        info_box.AppendText("选择相机2\r\n");
                        break;
                }

            }
            else
            {
                info_box.AppendText("相机未启用\r\n");
                //MessageBox.Show("相机未启用");
                return;
            }
        }

        //相机3
        private void Cam_3_Click(object sender, EventArgs e)
        {
            if (this.checkBox2.Checked == true)
            {
                string selectedValue = this.roadtype_box.SelectedItem.ToString();
                
                switch (selectedValue)
                {
                    case "3+0车道":
                        Cam_op(2);
                        info_box.AppendText("选择相机1\r\n");
                        break;
                    case "3+1车道":
                        Cam_op(3);
                        info_box.AppendText("选择相机1\r\n");
                        break;
                }
            }
            else
            {
                info_box.AppendText("相机未启用\r\n");
                //MessageBox.Show("相机未启用");
                return;
            }
        }

        //相机4
        private void Cam_4_Click(object sender, EventArgs e)
        {
            if (this.checkBox3.Checked == true)
            {
                Cam_op(4);
            }
            else
            {
                info_box.AppendText("相机未启用\r\n");
                //MessageBox.Show("相机未启用");
                return;
            }
        }

        /// <summary>
        /// 相机操作
        /// </summary>
        /// <param name="index"></param>
        private void Cam_op(int index)
        {
            try
            {
                flag = index;
                if (string.IsNullOrWhiteSpace(roadtype_box.Text))
                {
                    info_box.AppendText("请先选择车道\r\n");
                    return;
                }

                check_line_mode.Checked = false;
                check_cover_mode.Checked = false;

                CheckLine();
                Cleanline();

                string str = Writeip(index);
                string ipstr = $"{BaseUrl}{ipPsw}";

                if (str == ipstr)
                {
                    return;
                }
                
                ShowImage(str);


                press = index;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

        }

        //网格线
        private void Grid_Click(object sender, EventArgs e)
        {
            if (gridline.Text.Contains("*"))
            {
                gridline.Text = gridline.Text.Replace("*", "");
            }
            else
            {
                gridline.Text += "*";
            }
        }

        //车道选择 配置文件初始化
        private void Roadtypebox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cam_0.Visible = false;
            this.cam_1.Visible = false;
            this.cam_2.Visible = false;
            this.cam_3.Visible = false;
            this.checkBox0.Visible = false;
            this.checkBox1.Visible = false;
            this.checkBox2.Visible = false;
            this.checkBox3.Visible = false;

            int selectedIndex = this.roadtype_box.SelectedIndex;
            string selectedValue = this.roadtype_box.SelectedItem.ToString();
            if (File.Exists("config.json"))
            {
                File.Delete("config.json");
            }
            totalConfig.config_news.Clear();
            //configs.Clear();
            Dictionary<string, int> laneNumsDict = new Dictionary<string, int>
            {
                { "2+0车道", 2 },
                { "2+1车道", 3 },
                { "3+0车道", 3 },
                { "3+1车道", 4 },
                { "服务区_入口", 2 },
                { "服务区_出口", 2 }
            };
            int laneNums = laneNumsDict[selectedValue];
            string[] cameraNames = { "相机1", "相机2", "相机3", "相机4", "相机5", "相机6" };
            int[] cameraIndexes;
            switch (selectedValue)
            {
                case "2+0车道":
                    cameraIndexes = new int[] { 1, 0 };
                    this.cam_1.Visible = true;
                    this.cam_1.Text = cameraNames[cameraIndexes[0]];
                    this.cam_2.Visible = true;
                    this.cam_2.Text = cameraNames[cameraIndexes[1]];
                    this.checkBox1.Visible = true;
                    this.checkBox2.Visible = true;
                    this.pictureBox1.Load(@"road_2.jpeg");
                    break;
                case "服务区_入口":
                    cameraIndexes = new int[] { 1, 0 };
                    this.cam_0.Visible = true;
                    this.cam_0.Text = cameraNames[cameraIndexes[0]];
                    this.cam_1.Visible = true;
                    this.cam_1.Text = cameraNames[cameraIndexes[1]];
                    this.checkBox0.Visible = true;
                    this.checkBox1.Visible = true;
                    this.pictureBox1.Load(@"road.jpg");
                    break;
                case "服务区_出口":
                    cameraIndexes = new int[] { 1, 0 };
                    this.cam_0.Visible = true;
                    this.cam_0.Text = cameraNames[cameraIndexes[0]];
                    this.cam_1.Visible = true;
                    this.cam_1.Text = cameraNames[cameraIndexes[1]];
                    this.checkBox0.Visible = true;
                    this.checkBox1.Visible = true;
                    this.pictureBox1.Load(@"road.jpg");
                    break;
                case "2+1车道":
                    cameraIndexes = new int[] { 2, 1, 0 };
                    this.cam_0.Visible = true;
                    this.cam_0.Text = cameraNames[cameraIndexes[0]];
                    this.cam_1.Visible = true;
                    this.cam_1.Text = cameraNames[cameraIndexes[1]];
                    this.cam_2.Visible = true;
                    this.cam_2.Text = cameraNames[cameraIndexes[2]];
                    this.checkBox0.Visible = true;
                    this.checkBox1.Visible = true;
                    this.checkBox2.Visible = true;
                    this.pictureBox1.Load(@"road_2.jpeg");
                    break;
                case "3+0车道":
                    cameraIndexes = new int[] { 2, 1, 0 };
                    this.cam_1.Visible = true;
                    this.cam_1.Text = cameraNames[cameraIndexes[0]];
                    this.cam_2.Visible = true;
                    this.cam_2.Text = cameraNames[cameraIndexes[1]];
                    this.cam_3.Visible = true;
                    this.cam_3.Text = cameraNames[cameraIndexes[2]];
                    this.pictureBox1.Load(@"road_3.jpeg");
                    this.checkBox1.Visible = true;
                    this.checkBox2.Visible = true;
                    this.checkBox3.Visible = true;
                    break;
                case "3+1车道":
                    cameraIndexes = new int[] { 3, 2, 1, 0 };
                    this.cam_0.Visible = true;
                    this.cam_0.Text = cameraNames[cameraIndexes[0]];
                    this.cam_1.Visible = true;
                    this.cam_1.Text = cameraNames[cameraIndexes[1]];
                    this.cam_2.Visible = true;
                    this.cam_2.Text = cameraNames[cameraIndexes[2]];
                    this.cam_3.Visible = true;
                    this.cam_3.Text = cameraNames[cameraIndexes[3]];
                    this.pictureBox1.Load(@"road_3.jpeg");
                    this.checkBox0.Visible = true;
                    this.checkBox1.Visible = true;
                    this.checkBox2.Visible = true;
                    this.checkBox3.Visible = true;
                    break;
            }
            for (int i = 0; i < laneNums; i++)
            {
                //configs.Add(new ExtraConfig { ip = "", lane = laneNums - i, height = 6.7, landwidth = 3.75, B = 104 });
                //totalConfig.config.Add(new ExtraConfig { ip = "", lane = laneNums - i });
                totalConfig.config_news.Add(new ExtraConfig_new
                {
                    ip = "",
                    line = new List<LineConfig>
                    {
                        new LineConfig
                        {  
                            // 在这里初始化LineConfig的属性                          
                            lane = i,
                            struler = 0
                        }
                    }
                });

            }
            if (selectedValue == "服务区_入口") selectedValue = "service_in";
            if (selectedValue == "服务区_出口") selectedValue = "service_out";
            totalConfig.type = selectedValue;
            totalConfig.nums = laneNums;
            //totalConfig.config = configs;
            Convert(totalConfig);
        }

        //车道配置
        private void Roadtype_Click(object sender, EventArgs e)
        {
            if (File.Exists("config.json"))
            {
                var extras = JsonConvert.DeserializeObject<TotalConfig>(File.ReadAllText("config.json"));
                if (extras.type.Contains("2"))
                {
                    this.roadtype_box.SelectedIndex = extras.nums - 2;
                }
                if (extras.type.Contains("3"))
                {
                    this.roadtype_box.SelectedIndex = extras.nums - 1;
                }
                if (extras.type == "service_in")
                {
                    this.roadtype_box.SelectedIndex = 4;
                }
                if (extras.type == "service_out")
                {
                    this.roadtype_box.SelectedIndex = 5;
                }
                this.roadtype_box.Refresh();

                totalConfig = extras;
                //totalConfig.config.Reverse(); // 对 config 数组进行倒序排列
                Convert(totalConfig);
            }
        }

        /// <summary>
        /// 检查并写入相机ip
        /// </summary>
        /// <param name="camnum"></param>
        /// <returns></returns>
        private string Writeip(int camnum)
        {
            String str = BaseUrl;
            if (File.Exists("config.json"))
            {
                var extras = JsonConvert.DeserializeObject<TotalConfig>(File.ReadAllText("config.json"));

                if (string.IsNullOrWhiteSpace(extras.config_news[camnum].ip))//ip为空则输入
                {
                    using (Ipinput ipinput = new Ipinput())
                    {
                        ipinput.sendip += new Ipinput.SendMesg(Receiveip);
                        ipinput.sendpassward += new Ipinput.SendMesg(Receivepsw);
                        ipinput.ShowDialog();
                        if (ipinput.DialogResult == DialogResult.Cancel) // 判断是否点击了取消按钮
                        {
                            // 清空图像
                            drawPictureBoxVideo.setImage(null);

                        }
                        if (string.IsNullOrWhiteSpace(IP))
                        {
                            return str;
                        }

                        if (!CheckIp(IP) || !IsPingable(IP))
                        {
                            info_box.AppendText("输入ip有误或无法连通\r\n");
                            return str;
                        }
                        str += ipPsw;
                        str += IP;
                        extras.config_news[camnum].ip = IP;
                        extras.config_news[camnum].psw = ipPsw;
                        extras.config_news[camnum].state = true;
                        CAM_IP = extras.config_news[camnum].ip;
                        totalConfig = extras;
                        Convert(totalConfig);

                    }
                }
                else
                {
                    if (!IsPingable(extras.config_news[camnum].ip))//判断当前ip是否连通
                    {
                        isopen = false;
                        info_box.AppendText("输入ip无法连通\r\n");
                        extras.config_news[camnum].ip = IP;//无法连通则置空
                        totalConfig = extras;
                        Convert(totalConfig);
                        return str;
                    }
                    else if (flagpsw == false)
                    {
                        using (Ipinput ipinput = new Ipinput())
                        {
                            ipinput.sendip += new Ipinput.SendMesg(Receiveip);
                            ipinput.sendpassward += new Ipinput.SendMesg(Receivepsw);
                            ipinput.ShowDialog();
                            if (ipinput.DialogResult == DialogResult.Cancel) // 判断是否点击了取消按钮
                            {
                                // 清空图像
                                drawPictureBoxVideo.setImage(null);

                            }
                            if (string.IsNullOrWhiteSpace(IP))
                            {
                                return str;
                            }
                            extras.config_news[camnum].ip = IP;
                            extras.config_news[camnum].psw = ipPsw;
                            extras.config_news[camnum].state = true;
                            CAM_IP = extras.config_news[camnum].ip;
                            totalConfig = extras;
                            Convert(totalConfig);
                        }
                    }
                    str += extras.config_news[camnum].psw;
                    str += extras.config_news[camnum].ip;
                    CAM_IP = extras.config_news[camnum].ip;
                }
            }
            else
            {
                info_box.AppendText("配置文件不存在\r\n");
            }

            IP = "";
            return str;
        }


        //检查ip是否合法
        private bool CheckIp(string ip)
        {
            string pattern = @"^((25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))$";
            if (Regex.IsMatch(ip, pattern))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // 需要导入Newtonsoft.Json，这里使用的版本是4.5
        private string ConvertJsonString(string str)
        {
            //格式化json字符串
            JsonSerializer serializer = new JsonSerializer();
            TextReader tr = new StringReader(str);
            JsonTextReader jtr = new JsonTextReader(tr);
            object obj = serializer.Deserialize(jtr);
            if (obj != null)
            {
                StringWriter textWriter = new StringWriter();
                JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                {
                    Formatting = Formatting.Indented,
                    Indentation = 4,
                    IndentChar = ' '
                };
                serializer.Serialize(jsonWriter, obj);
                return textWriter.ToString();
            }
            else
            {
                return str;
            }
        }

        //json换行      
        private void Convert(TotalConfig totalConfig)
        {
            var res = JsonConvert.SerializeObject(totalConfig);
            string str = res.ToString();
            string temp = ConvertJsonString(str);
            var res1 = JsonConvert.DeserializeObject(temp);
            File.WriteAllText("config.json", res1.ToString());
        }

        /// <summary>
        /// 上传参数文件到/home/net5.0
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>      
        private void Uploadjson_Click(object sender, EventArgs e)
        {
            try
            {
                using (var dialog = new ConnectDialog())
                {
                    var res = dialog.ShowDialog();
                    if (res == DialogResult.OK)
                    {
                        ipAddress = dialog.IpAddress;
                        password = dialog.Password;
                        username = dialog.Username;
                        using (var sftpClient = new SftpClient(ipAddress, 22, username, password))
                        {
                            info_box.AppendText("文件上传中，请稍后...\r\n");
                            sftpClient.Connect();
                            string localFilePath = "config.json";
                            string remoteFilePath = "/home/net5.0/config.json";
                            using (var fileStream = new FileStream(localFilePath, FileMode.Open))
                            {
                                try
                                {
                                    sftpClient.UploadFile(fileStream, remoteFilePath);
                                    info_box.AppendText("上传成功\r\n");
                                    info_box.AppendText("------------\r\n");
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.ToString());
                                }
                            }

                            sftpClient.Disconnect();
                        }
                        using (var sshClient = new SshClient(ipAddress, 22, username, password))
                        {
                            sshClient.Connect();
                            try
                            {
                                // 执行脚本 kill当前进程
                                SshCommand command = sshClient.RunCommand("ps -ef | grep arm_video_net5.dll | grep -v grep | awk '{print $2}' | xargs kill -9");
                                info_box.AppendText("进程更新命令执行完成!\r\n");
                                info_box.AppendText("------------\r\n");
                            }
                            catch (Exception ex)
                            {
                                info_box.AppendText("命令执行失败：" + ex.Message);
                            }
                            sshClient.Disconnect();
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        //车道宽度文本框
        private void Landwidth_TextChanged(object sender, EventArgs e)
        {
            if (landwidth_box.Text != "")
            {
                landwidth = double.Parse(landwidth_box.Text);
            }
            else
            {
                landwidth = 0;
            }
            //landwidth = double.Parse(landwidth_box.Text);
        }

        //计算车道夹角
        private void Caculate_tanα()
        {
            try
            {

                if (drawPictureBoxVideo.drawCache.drawLineList.Count != 0)
                {
                    DrawPictureCache.DrawLine line = (DrawPictureCache.DrawLine)drawPictureBoxVideo.drawCache.drawLineList[0];
                    landwidthpix = line.yMax - line.yMin;
                }
                info_box.AppendText("当前视频尺寸：" + currBitmap.Width.ToString() + "×" + currBitmap.Height.ToString() + "\r\n");
                info_box.AppendText("------------\r\n");
                info_box.AppendText("车道像素宽：" + landwidthpix.ToString() + "\r\n");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        //计算最终标尺
        private void Caculate_struler()
        {
            try
            {
                Caculate_tanα();
                //Caculate_tana();
                //double temp = 0.25 * Caculate_upruler() + 0.75 * Caculate_downruler();
                double temp = landwidthpix / landwidth;//为了方便操作 修改为像素长度/车道宽
                Struler = System.Convert.ToInt32(temp);

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString() + "\r\n车道线参数有误");
            }
        }

        //标尺计算
        private void Caculate_Click(object sender, EventArgs e)
        {
            if (drawPictureBoxVideo.drawCache.drawLineList.Count == 0)
            {
                MessageBox.Show("未画车道线");
                return;
            }
            if (drawPictureBoxVideo.drawCache.drawLineList.Count > 1)
            {
                MessageBox.Show("标定车道线过多，请重新画线");
                return;
            }
            Caculate_struler();
            //Struler = caculate_struler();
            info_box.AppendText("最终标尺：" + Struler.ToString() + "\r\n");
            info_box.AppendText("------------\r\n");
        }

        //旋转
        //private void Rotate_btn_Click(object sender, EventArgs e)
        //{
        //    if (rotate_btn.Text.Contains("*"))
        //    {
        //        rotate_btn.Text = rotate_btn.Text.Replace("*", "");
        //    }
        //    else
        //    {
        //        rotate_btn.Text += "*";
        //    }
        //}

        static List<Bitmap> frameList = new List<Bitmap>();
        //拼帧按钮
        private void Concatenate_frames_Click(object sender, EventArgs e)
        {
            try
            {
                if (isopen == false)
                {
                    info_box.AppendText("相机未连接\r\n");
                    //MessageBox.Show("相机未连接");
                    return;
                }
                while (true)
                {
                    //using (var frame = currBitmap)
                    using (var frame = cv2Video.currFrameGetImage())
                    {
                        if (addframelist(frame)) break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        static Bitmap CombineFrames(List<Bitmap> frames)
        {
            var width = frames[0].Width;
            var height = frames[0].Height;

            // Create a new bitmap with combined width
            var combinedFrame = new Bitmap(width * frames.Count, height);

            // Combine frames into one bitmap
            using (var graphics = Graphics.FromImage(combinedFrame))
            {
                int x = 0;
                foreach (var frame in frames)
                {
                    graphics.DrawImage(frame, new Rectangle(x, 0, frame.Width, frame.Height));
                    x += frame.Width;
                }
            }

            return combinedFrame;
        }

        static void DisplayImage(Image image)
        {
            // Display the image in a new window
            var form = new Form();
            var pictureBox = new PictureBox();
            form.Height = Screen.PrimaryScreen.Bounds.Height;
            form.Width = Screen.PrimaryScreen.Bounds.Width;
            pictureBox.Dock = DockStyle.Fill;
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.Image = image;
            form.Controls.Add(pictureBox);
            form.ShowDialog();
        }

        //拼帧画线
        private void drawcombainframe_Click(object sender, EventArgs e)
        {
            if (drawcombainframe.Text.Contains("*"))
            {
                //清除点
                Cleanline();
            }
            else
            {
                drawcombainframe.Text += "*";
                this.drawPictureBoxVideo.mouseClickType = "drawPoint";
                //info_box.AppendText("开始画线\r\n");
            }
        }

        private bool addframelist(Bitmap frame)
        {
            if (drawPictureBoxVideo.drawCache.drawPointList.Count != 0)
            {
                DrawPictureCache.DrawPoint point = (DrawPictureCache.DrawPoint)drawPictureBoxVideo.drawCache.drawPointList[0];
                var width = frame.Width / 5;
                //var width = line.xMax - line.xMin;
                // Crop the middle 1/5 of the frame
                var cropRect = new Rectangle(point.x - width / 2, 0, width, frame.Height);
                var croppedFrame = frame.Clone(cropRect, frame.PixelFormat);

                // Add the cropped frame to a list
                frameList.Add(croppedFrame);

                // If the list contains 15 frames, combine them into one image and display it
                if (frameList.Count == 15)
                {
                    this.drawPictureBoxVideo.drawCache.clearDrawPointList();
                    this.drawPictureBoxVideo.refresh();
                    var combinedFrame = CombineFrames(frameList);
                    DisplayImage(combinedFrame);
                    frameList.Clear();
                    combinedFrame.Dispose();
                    return true;
                    //break;
                }
            }
            else
            {
                var width = frame.Width / 5;
                // Crop the middle 1/5 of the frame
                var cropRect = new Rectangle(frame.Width / 2 - width / 2, 0, width, frame.Height);
                var croppedFrame = frame.Clone(cropRect, frame.PixelFormat);

                // Add the cropped frame to a list
                frameList.Add(croppedFrame);

                // If the list contains 15 frames, combine them into one image and display it
                if (frameList.Count == 15)
                {
                    this.drawPictureBoxVideo.drawCache.clearDrawPointList();
                    this.drawPictureBoxVideo.refresh();
                    var combinedFrame = CombineFrames(frameList);
                    DisplayImage(combinedFrame);
                    frameList.Clear();
                    combinedFrame.Dispose();
                    return true;
                    //break;
                }
            }
            return false;
        }


        
    }
}
