using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using Newtonsoft.Json;
using Renci.SshNet;
using System.Text.RegularExpressions;
using System.Net.NetworkInformation;
using OpenCvSharp;

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
        public static TotalConfig totalConfig = new TotalConfig();
        public Cv2Video cv2Video = null;//实例
        private Ipinput ipinput = new Ipinput();
        public string IP = "";
        public string ipPsw = "";
        public int stopframe = 0;
        public double height = 6.7;
        public double landwidth = 3.75;
        public double B = 104;
        public double landwidthpix = 0;
        public double Tanα = 0;
        public double Tana = 0;
        public int Struler = 0;
        public double Downruler = 0;
        public double Upruler = 0;
        public double angleInDegrees = 0;
        //private int skipFramesCount = 0; // 跳过的帧数
        //private int skipFramesInterval = 5; // 跳帧的间隔

        // 定义IP地址和密码的成员变量
        private string ipAddress;
        private string password;
        private string username;
        private const string BaseUrl = "rtsp://admin:";

        public Form1()
        {
            InitializeComponent();
            ipinput.sendip += new Ipinput.SendMesg(Receiveip);
            ipinput.sendpassward += new Ipinput.SendMesg(Receivepsw);
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
        }

        public class ExtraConfig
        {
            public string ip;
            public string psw;
            public DrawPictureCache.DrawLine line;
            public bool state = false;
            //public double length;
            //public int pixeldis;
            //public bool flip = false;
            public int lane;
            public double height;
            public double A;
            public double B;

            public double tanα;
            public double tana;
            public double landwidth;
            public int landwidthpix;
            public double downruler;
            public double upruler;
            public int struler;

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
                    if (extras.type.Contains("0"))
                    {
                        flag--;//当没有应急车道的时候相机下标需要减一
                        extraConfig = extras.config[flag];
                        if (extras.config[flag].line == null)
                        {
                            MessageBox.Show("未配置参数");
                            return;
                        }
                        info_box.AppendText("------------\r\n");
                        info_box.AppendText("当前视频尺寸：" + currBitmap.Width.ToString() + "×" + currBitmap.Height.ToString() + "\r\n");
                        info_box.AppendText("------------\r\n");
                        info_box.AppendText("车道像素宽：" + extraConfig.landwidthpix + "\r\n");
                        info_box.AppendText("上沿标尺：" + extraConfig.upruler + "\r\n");
                        info_box.AppendText("下沿标尺：" + extraConfig.downruler + "\r\n");
                        info_box.AppendText("最终标尺：" + extraConfig.struler + "\r\n");
                        info_box.AppendText("------------\r\n");
                        cam_angel.Text = extraConfig.B.ToString();
                        cam_height.Text = extraConfig.height.ToString();
                        flag++;
                    }
                    else
                    {
                        extraConfig = extras.config[flag];
                        if (extras.config[flag].line == null)
                        {
                            MessageBox.Show("未配置参数");
                            return;
                        }
                        info_box.AppendText("------------\r\n");
                        info_box.AppendText("当前视频尺寸：" + currBitmap.Width.ToString() + "×" + currBitmap.Height.ToString() + "\r\n");
                        info_box.AppendText("------------\r\n");
                        info_box.AppendText("车道像素宽：" + extraConfig.landwidthpix + "\r\n");
                        info_box.AppendText("上沿标尺：" + extraConfig.upruler + "\r\n");
                        info_box.AppendText("下沿标尺：" + extraConfig.downruler + "\r\n");
                        info_box.AppendText("最终标尺：" + extraConfig.struler + "\r\n");
                        info_box.AppendText("------------\r\n");
                        cam_angel.Text = extraConfig.B.ToString();
                        cam_height.Text = extraConfig.height.ToString();
                    }
                    this.drawPictureBoxVideo.drawCache.addDrawLineList(extraConfig.line.xMin, extraConfig.line.yMin, extraConfig.line.xMax, extraConfig.line.yMax, 3, Color.Blue);
                    MessageBox.Show("参数读取成功");
                }
                else
                {
                    MessageBox.Show("参数文件不存在");
                }
                //this.drawPictureBoxVideo.refresh();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + "参数读取失败");
            }
        }

        List<ExtraConfig> configs = new List<ExtraConfig>();

        private void SetConfig()
        {
            try
            {
                extraConfig.line = DrawPictureCache.gGetDrawLine;
                if (File.Exists("config.json"))
                {
                    var extras = JsonConvert.DeserializeObject<TotalConfig>(File.ReadAllText("config.json"));
                    //extras.nums = this.comboBox1.SelectedIndex + 3;
                    if (extras.type.Contains("0"))//2+0 3+0车道
                    {
                        flag--;
                        extras.config[flag].line = DrawPictureCache.gGetDrawLine;
                        extras.config[flag].height = height;
                        extras.config[flag].landwidth = landwidth;
                        extras.config[flag].B = B;
                        extras.config[flag].A = Math.Round(angleInDegrees, 1);
                        extras.config[flag].tana = Tana;
                        extras.config[flag].tanα = Tanα;
                        extras.config[flag].struler = Struler;
                        extras.config[flag].downruler = Downruler;
                        extras.config[flag].upruler = Upruler;
                        extras.config[flag].landwidthpix = (int)landwidthpix;
                        flag++;
                    }
                    else
                    {
                        extras.config[flag].line = DrawPictureCache.gGetDrawLine;
                        extras.config[flag].height = height;
                        extras.config[flag].landwidth = landwidth;
                        extras.config[flag].B = B;
                        extras.config[flag].A = Math.Round(angleInDegrees, 1);
                        extras.config[flag].tana = Tana;
                        extras.config[flag].tanα = Tanα;
                        extras.config[flag].struler = Struler;
                        extras.config[flag].downruler = Downruler;
                        extras.config[flag].upruler = Upruler;
                        extras.config[flag].landwidthpix = (int)landwidthpix;
                    }
                    totalConfig = extras;
                    Convert(totalConfig);
                }
                info_box.AppendText("参数保存成功\r\n");
                //MessageBox.Show("参数保存成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            Cleanline();
            //CleanRectangle();

        }

        private void Cleanline()
        {
            //清除线
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
                Thread.Sleep(1);
            }
        }

        //获取当前帧
        //onnx
        private void ShowCurrVideoFrame()
        {
            this.Invoke(new ThreadStart(delegate
            {
                try
                {
                    if (isopen)
                    {
                        if (currBitmap != null)
                        {
                            currBitmap.Dispose();
                            
                        }
                        currBitmap = null;
                        //旋转
                        if (rotate_btn.Text.Contains("*"))
                        {
                            currBitmap = cv2Video.currFrameGetImageRotate();//当前帧                              
                        }

                        else
                        {
                            currBitmap = cv2Video.currFrameGetImage();//当前帧
                        }

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

                            //drawPictureBoxVideo.setImage(currBitmap);//显示
                            //Cv2.WaitKey(1);
                            //显示
                            this.Invoke(new ThreadStart(delegate
                            {

                                drawPictureBoxVideo.setImage(currBitmap);//显示


                            }));
                            //Cv2.WaitKey(1);
                        }
                        else
                        {
                            //if (cv2Video.getCurrFrameIndex() == -1)
                            //{

                            //}
                            currBitmap = cv2Video.currFrameGetImage();
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

        //显示图片
        private void Showimage(string str)
        {
            
            if (cv2Video != null)
            {
                cv2Video.dispose();
            }
            cv2Video = new Cv2Video();
            info_box.AppendText("连接中，请稍后...\r\n");

            try
            {
                //isopen = cv2Video.openVideoFile(@"demo.mp4");
                //isopen = cv2Video.openCamera();
                isopen = cv2Video.openRtsp(str + ":554/h265/ch1/main");
                info_box.AppendText("连接成功\r\n");
                if (!isopen)
                {
                    cv2Video = null;
                    info_box.AppendText("无法连接，请检查ip密码\r\n");
                    flagpsw = false;
                    //ipinput.ShowDialog();
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            isopen = true;
            flagpsw = true;

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
                if (extras.type.Contains("0"))
                {
                    press--;//当没有应急车道的时候相机下标需要减一 cam 1 2 3 -> confi 0 1 2
                }
                if (extras.config[press].line != null)
                {
                    DrawPictureCache.DrawLine line1 = (DrawPictureCache.DrawLine)drawPictureBoxVideo.drawCache.drawLineList[0];
                    var line2 = extras.config[press].line;
                    int ans3 = line1.xMax + line1.xMin + line1.yMax + line1.yMin;
                    int ans4 = line2.xMax + line2.xMin + line2.yMax + line2.yMin;
                    if (ans3 != ans4) { MessageBox.Show("当前配置未保存"); }

                }
                else
                {
                    MessageBox.Show("当前配置未保存");
                }

            }

        }

        //相机按钮背景颜色设置      
        private void ButtonColor(Control control)
        {
            Control.ControlCollection collection = control.Controls;
            string[] buttonNames = { "cam_0", "cam_1", "cam_2", "cam_3", "cam_4", "cam_5" };
            foreach (Button button in collection.OfType<Button>())
            {
                if (buttonNames.Contains(button.Name) && button.Name == buttonNames[flag])
                {
                    button.BackColor = Color.Green;
                }
                else
                {
                    button.BackColor = Color.White;
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
                MessageBox.Show("相机未启用");
                return;
            }

        }

        //相机1
        private void Cam_1_Click(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked == true)
            {
                Cam_op(1);
            }
            else
            {
                MessageBox.Show("相机未启用");
                return;
            }
        }

        //相机2
        private void Cam_2_Click(object sender, EventArgs e)
        {
            if (this.checkBox2.Checked == true)
            {
                Cam_op(2);
            }
            else
            {
                MessageBox.Show("相机未启用");
                return;
            }
        }

        //相机3
        private void Cam_3_Click(object sender, EventArgs e)
        {
            if (this.checkBox3.Checked == true)
            {
                Cam_op(3);
            }
            else
            {
                MessageBox.Show("相机未启用");
                return;
            }
        }

        //相机4
        private void Cam_4_Click(object sender, EventArgs e)
        {
            if (this.checkBox4.Checked == true)
            {
                Cam_op(4);
            }
            else
            {
                MessageBox.Show("相机未启用");
                return;
            }
        }
       

        //相机操作
        private void Cam_op(int index)
        {
            flag = index;
            if (string.IsNullOrWhiteSpace(roadtype_box.Text))
            {
                MessageBox.Show("请先选择车道");
                return;
            }
            ButtonColor(splitContainer2.Panel2);
            CheckLine();
            string str = Writeip(index);
            string ipstr = BaseUrl + ipPsw;
            if (str == ipstr)
            {
                return;
            }
            Showimage(str);
            info_box.Clear();
            press = index;
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
            this.cam_4.Visible = false;
            this.cam_5.Visible = false;
            this.checkBox0.Visible = false;
            this.checkBox3.Visible = false;
            this.checkBox4.Visible = false;
            this.checkBox5.Visible = false;

            int selectedIndex = this.roadtype_box.SelectedIndex;
            string selectedValue = this.roadtype_box.SelectedItem.ToString();
            if (File.Exists("config.json"))
            {
                File.Delete("config.json");
            }
            configs.Clear();
            Dictionary<string, int> laneNumsDict = new Dictionary<string, int>
            {
                { "2+0车道", 2 },
                { "2+1车道", 3 },
                { "3+0车道", 3 },
                { "3+1车道", 4 }
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
                    this.pictureBox1.Load(@"road_2.jpeg");
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
                    this.checkBox3.Visible = true;
                    this.checkBox0.Visible = true;
                    break;
            }
            for (int i = 0; i < laneNums; i++)
            {
                configs.Add(new ExtraConfig { ip = "", lane = laneNums - i, height = 6.7, landwidth = 3.75, B = 104 });
            }
            totalConfig.type = selectedValue;
            totalConfig.nums = laneNums;
            totalConfig.config = configs;
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
                this.roadtype_box.Refresh();

                totalConfig = extras;
                //totalConfig.config.Reverse(); // 对 config 数组进行倒序排列
                Convert(totalConfig);
            }
        }

        //检查并写入相机ip 
        private string Writeip(int camnum)
        {
            String str = BaseUrl;
            if (File.Exists("config.json"))
            {
                var extras = JsonConvert.DeserializeObject<TotalConfig>(File.ReadAllText("config.json"));
                if (extras.type.Contains("0"))
                {
                    camnum--;//当没有应急车道的时候相机下标需要减一
                }
                if (string.IsNullOrWhiteSpace(extras.config[camnum].ip))//ip为空则输入
                {
                    ipinput.ShowDialog();
                    if (string.IsNullOrWhiteSpace(IP))
                    {
                        return str;
                    }
                    if (!CheckIp(IP))
                    {
                        info_box.AppendText("输入ip有误\r\n");
                        //MessageBox.Show("输入ip有误");
                        return str;
                    }
                    if (!IsPingable(IP))
                    {
                        info_box.AppendText("输入ip无法连通\r\n");
                        //MessageBox.Show("输入ip无法连通");
                        return str;
                    }
                    str += ipPsw;
                    str += IP;
                    extras.config[camnum].ip = IP;
                    extras.config[camnum].psw = ipPsw;
                    extras.config[camnum].state = true;
                    totalConfig = extras;
                    Convert(totalConfig);

                }
                else
                {
                    if (!IsPingable(extras.config[camnum].ip))//判断当前ip是否连通
                    {
                        isopen = false;
                        //MessageBox.Show("输入ip无法连通");
                        info_box.AppendText("输入ip无法连通\r\n");
                        extras.config[camnum].ip = IP;//无法连通则置空
                        totalConfig = extras;
                        Convert(totalConfig);
                        return str;
                    }
                    else if(flagpsw == false)
                    {
                        ipinput.ShowDialog();
                        extras.config[camnum].ip = IP;
                        extras.config[camnum].psw = ipPsw;
                        extras.config[camnum].state = true;
                        totalConfig = extras;
                        Convert(totalConfig);

                    }
                    str += extras.config[camnum].psw;
                    str += extras.config[camnum].ip;

                }
            }
            else
            {
                MessageBox.Show("配置文件不存在");
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

        //相机高度文本框
        private void Camheight_TextChanged(object sender, EventArgs e)
        {
            if (cam_height.Text != "")
            {
                height = double.Parse(cam_height.Text);
            }
            else
            {
                height = 0;
            }
            //CaculatePixeldis();

        }

        //上传参数文件到/home
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
                            string remoteFilePath = "/home/config.json";
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

        //视场角文本框
        private void Camangel_TextChanged(object sender, EventArgs e)
        {
            if (cam_angel.Text != "")
            {
                B = double.Parse(cam_angel.Text);
            }
            else
            {
                B = 0;
            }
            //B = double.Parse(cam_angel.Text);
        }

        //计算车道夹角
        private void Caculate_tanα()
        {
            try {

                if (drawPictureBoxVideo.drawCache.drawLineList.Count != 0)
                {
                    DrawPictureCache.DrawLine line = (DrawPictureCache.DrawLine)drawPictureBoxVideo.drawCache.drawLineList[0];
                    landwidthpix = line.yMax - line.yMin;
                }
                info_box.AppendText("当前视频尺寸：" + currBitmap.Width.ToString() + "×" + currBitmap.Height.ToString() + "\r\n");
                info_box.AppendText("------------\r\n");
                info_box.AppendText("车道像素宽：" + landwidthpix.ToString() + "\r\n");
                double α = (landwidthpix / currBitmap.Height) * B;
                info_box.AppendText("车道夹角：" + Math.Round(α, 1).ToString() + "\r\n");
                double temp = α * Math.PI / 180;
                Tanα = Math.Tan(temp);
                string strNumber = Tanα.ToString("F2");
                Tanα = double.Parse(strNumber);

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }

        //计算下车道线夹角
        private void Caculate_tana()
        {
            double A = height * Tanα;
            double B = landwidth * Tanα;
            double C = height * Tanα - landwidth;
            Tana = (Math.Sqrt(B * B - 4 * A * C) - B) / (2 * A);
            double a = Math.Atan(Tana);
            string strNumber = Tana.ToString("F2");
            Tana = double.Parse(strNumber);
            angleInDegrees = a * 180 / Math.PI; // 将弧度转换为角度
            info_box.AppendText("下车道线夹角：" + Math.Round(angleInDegrees, 1).ToString() + "\r\n");
        }

        //计算下车道线标尺
        private double Caculate_downruler()
        {
            double placedown = height * Tana;
            double j1 = Math.Atan((placedown - 0.5) / height);
            j1 = j1 * 180 / Math.PI;
            double j2 = Math.Atan((placedown + 0.5) / height);
            j2 = j2 * 180 / Math.PI;
            Downruler = (Math.Abs(j2 - j1)) / B * currBitmap.Height;
            string strNumber = Downruler.ToString("F2");
            Downruler = double.Parse(strNumber);
            info_box.AppendText("下沿标尺：" + strNumber + "\r\n");
            return Downruler;
        }

        //计算上车道线标尺
        private double Caculate_upruler()
        {
            double placedown = height * Tana;
            double placeup = placedown + landwidth;
            double j1 = Math.Atan((placeup - 0.5) / height);
            j1 = j1 * 180 / Math.PI;
            double j2 = Math.Atan((placeup + 0.5) / height);
            j2 = j2 * 180 / Math.PI;
            Upruler = (Math.Abs(j2 - j1)) / B * currBitmap.Height;
            string strNumber = Upruler.ToString("F2");
            Upruler = double.Parse(strNumber);
            info_box.AppendText("上沿标尺：" + strNumber + "\r\n");
            return Upruler;
        }

        //计算最终标尺
        private void Caculate_struler()
        {
            try
            {
                Caculate_tanα();
                Caculate_tana();
                double temp = 0.25 * Caculate_upruler() + 0.75 * Caculate_downruler();
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
        private void Rotate_btn_Click(object sender, EventArgs e)
        {
            if (rotate_btn.Text.Contains("*"))
            {
                rotate_btn.Text = rotate_btn.Text.Replace("*", "");
            }
            else
            {
                rotate_btn.Text += "*";
            }
        }

        static List<Bitmap> frameList = new List<Bitmap>();
        //拼帧按钮
        private void Concatenate_frames_Click(object sender, EventArgs e)
        {
            try
            {
                if (isopen == false)
                {
                    MessageBox.Show("相机未连接");
                    return;
                }
                while (true)
                {
                    //using (var frame = currBitmap)
                    if (rotate_btn.Text.Contains("*"))
                    {
                        using (var frame = cv2Video.currFrameGetImageRotate())
                        {
                            if (addframelist(frame)) break;
                        }
                    }
                    else
                    {
                        using (var frame = cv2Video.currFrameGetImage())
                        {
                            if (addframelist(frame)) break;
                        }
                    }
                }
            }
            catch(Exception ex)
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
