using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using System.Threading;
using System.IO;
using Newtonsoft.Json;
using Renci.SshNet;


namespace videocapture
{
    public partial class Form1 : Form
    {
        private List<int> cutWithLineShowIndex = new List<int>();
        public int laneNum = 1;//车道数
        public int flag = 99;//相机标志
        public int press = 99;
        public static bool isopen = false;//是否播放 视频线程用 opencv
        public int rotate = 0;
        public bool isrotate = false;
        private Thread videoThr = null;//视频线程
        public Bitmap currBitmap = null;//当前播放的帧 
        public static ExtraConfig extraConfig = new ExtraConfig();
        public static TotalConfig totalConfig = new TotalConfig();
        public Cv2Video cv2Video = null;//实例
        private Ipinput ipinput = new Ipinput();
        public string IP = "";
        public int stopframe = 0;

        // 定义IP地址和密码的成员变量
        private string ipAddress;
        private string password;
        private string username;

        public Form1()
        {
            InitializeComponent();
            ipinput.send += new Ipinput.SendMesg(Receive);
        }
        private void VideoForm_Load(object sender, EventArgs e)
        {
            //主线程
            videoThr = new Thread(videoThread);
            videoThr.IsBackground = true;
            videoThr.Start();
            //drawPictureBoxVideo的左上坐标 未用
            int x = this.drawPictureBoxVideo.Location.X;
            int y = this.drawPictureBoxVideo.Location.Y;
            this.drawPictureBoxVideo.refreshTopLeft(x, y);

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
            public DrawPictureCache.DrawLine line;
            public bool state = false;
            public double length;
            public int pixeldis;
            //public bool flip = false;
            public int lane;

        }
        
        private void Receive(string str)
        {
            IP = str;
        }
        private void readConfig()
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
                MessageBox.Show(ex.ToString());
            }
        }

        List<ExtraConfig> configs = new List<ExtraConfig>();

        private void setConfig()
        {
            try
            {
                extraConfig.line = DrawPictureCache.gGetDrawLine;
                if (File.Exists("config.json"))
                {
                    var extras = JsonConvert.DeserializeObject<TotalConfig>(File.ReadAllText("config.json"));
                    //extras.nums = this.comboBox1.SelectedIndex + 3;
                    if (extras.type.Contains("0"))
                    {
                        flag--;
                        extras.config[flag].line = DrawPictureCache.gGetDrawLine;
                        //if (this.button5.Text.Contains("*"))
                        //{
                        //    extras.config[flag].flip = true;
                        //}
                        //else
                        //{
                        //    extras.config[flag].flip = false;
                        //}
                        flag++;
                    }
                    else
                    {
                        extras.config[flag].line = DrawPictureCache.gGetDrawLine;
                        //if (this.button5.Text.Contains("*"))
                        //{
                        //    extras.config[flag].flip = true;
                        //}
                        //else
                        //{
                        //    extras.config[flag].flip = false;
                        //}
                    }                  
                    totalConfig = extras;
                    convert(totalConfig);
                }
                MessageBox.Show("参数保存成功");
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
            this.drawPictureBoxVideo.mouseClickType = "";
            this.drawPictureBoxVideo.drawCache.clearDrawLineList();
            this.drawPictureBoxVideo.refresh();
            this.drawPictureBoxVideo.mouseClickType = "";
        }

        //画线
        private void drawLineToolStripMenuItem_Click(object sender, EventArgs e)
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
            }
        }

        //读配置
        private void readConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            readConfig();
            //this.drawRangeToolStripMenuItem.Text = "画区域*";
            this.drawLineToolStripMenuItem.Text = "画线*";
        }

        //写配置
        private void setConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (can_saveconfig())
            //{
            //    setConfig();
            //}
            //else
            //{
            //    MessageBox.Show("当前线框不合理");
            //}
            setConfig();

        }

        //视频线程
        private void videoThread()
        {
            while (true)
            {
                try
                {
                    showCurrVideoFrame_onx();
                }
                catch { }
                Thread.Sleep(1);
            }
        }  

        DrawPictureCache.DrawRectangle rec = DrawPictureCache.gGetDrawRectangle;

        //获取当前帧
        //onnx
        private void showCurrVideoFrame_onx()
        {
            this.Invoke(new ThreadStart(delegate
            {
                try
                {
                    if (isopen)
                    {
                        rec = DrawPictureCache.gGetDrawRectangle;
                        if (currBitmap != null)
                        {
                            currBitmap.Dispose();
                        }
                        currBitmap = null;
                        currBitmap = cv2Video.currFrameGetImageFlip(false, 0);//当前帧
                        
                        if (currBitmap != null)
                        {
                            #region 截取客车
                            //if (checkBox6.Checked)
                            //{

                            //    //Mat mat = BitmapConverter.ToMat(currBitmap);
                            //    Mat mat = Cv2Mat.ImageToMat(currBitmap);
                            //    YoloOnnx _YoloOnnx = YoloOnnx.GetInstance();
                            //    YoloOnnx.PredictResult ret;
                            //    int frame_num = cv2Video.getCurrFrameIndex();
                            //    bool jumpflag = true;
                            //    if (jumpflag && frame_num % 2 == 0)
                            //    {
                            //        return;
                            //    }

                            //    if (_YoloOnnx.Predict(mat, out ret))
                            //    {
                            //        int preframe = 0;

                            //        if (ret.Targets.Count > 0)
                            //        {
                            //            int curframe = cv2Video.getCurrFrameIndex();
                            //            if (curframe == stopframe && ret.Targets[0].Label == "1")
                            //            {
                            //                isopen = false;
                            //                //currBitmap.Save(@"/out/test.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                            //            }

                            //            if (ret.Targets.Count >= 2)
                            //            {
                            //                ret.Targets = ret.Targets.OrderByDescending(o => o.Box.X).ToList();
                            //            }
                            //            for (int i = 0; i < ret.Targets.Count; i++)
                            //            {

                            //                if (ret.Targets.Count >= 1 && ret.Targets[i].Label == "1")
                            //                {
                            //                    preframe = cv2Video.getCurrFrameIndex();//获取触发帧序号

                            //                }
                            //                break;
                            //            }
                            //            stopframe = preframe + 2;

                            //        }

                            //    }

                            //    //Console.WriteLine(sw.ElapsedMilliseconds);
                            //}
                            #endregion

                            using (Graphics g = Graphics.FromImage(currBitmap))
                            {
                                #region 网格线
                                if (button5.Text.Contains("*"))
                                {
                                    var pen = new Pen(Color.Red, 2);
                                    //垂直
                                    for (int i = 0; i < currBitmap.Width; i += 60)
                                    {
                                        if (i > 0)
                                        {
                                            g.DrawLine(pen, new System.Drawing.Point(i, currBitmap.Height / 2), new System.Drawing.Point(i,currBitmap.Height));
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
                                #region 中心线
                                if (button9.Text.Contains("*"))
                                {
                                    var pen = new Pen(Color.Red, 10);
                                    //垂直中线
                                    g.DrawLine(pen, new System.Drawing.Point(currBitmap.Width / 2, 0), new System.Drawing.Point(currBitmap.Width / 2, currBitmap.Height));
                                    //水平中线
                                    g.DrawLine(pen, new System.Drawing.Point(0, currBitmap.Height / 2), new System.Drawing.Point(currBitmap.Width, currBitmap.Height / 2));
                                }
                                #endregion
                            }

                            //显示
                            this.Invoke(new ThreadStart(delegate
                            {
                                
                                drawPictureBoxVideo.setImage(currBitmap);//显示

                            }));
                            //Cv2.WaitKey(1);
                        }
                        else
                        {
                            if (cv2Video.getCurrFrameIndex() == -1)
                            {
                                isopen = false;
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    //IcyTools.Log.WriteLog("Error", "", ex.ToString());
                }

            }));
        }      

        //显示相机画面
        private void showimage(string str)
        {
            isopen = true;
            if (cv2Video != null)
            {
                cv2Video.dispose();
            }
            cv2Video = new Cv2Video();          
            //bool res = cv2Video.openRtsp(str + ":554 latency = 0 ! rtph264depay ! h264parse ! nvv4l2decoder ! nvvidconv ! video/x-raw,format=(string)BGRx, width=(int)1920,height=(int)1080 ! videoconvert ! appsink sync = false");
            bool res = cv2Video.openVideoFile(@"demo.mp4");
            //bool res = cv2Video.openRtsp(str + ":554 ");
 
            if (!res)
            {
                cv2Video = null;
                MessageBox.Show("视频格式不对");
                return;
            }
            isopen = true;
        }

        //检查画的线和框有没有保存
        private void CheckLine()
        {
            var extras = JsonConvert.DeserializeObject<TotalConfig>(File.ReadAllText("config.json"));
            if (drawPictureBoxVideo.drawCache.drawLineList.Count != 0)
            {
                if (extras.type.Contains("0"))
                {
                    press--;//当没有应急车道的时候相机下标需要减一
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
        private void buttonColor(Control control)
        {
            Control.ControlCollection collection = control.Controls;
            foreach (Button button in collection.OfType<Button>())
            {
                if (flag == 0 && button.Name == "button1")
                {
                    button.BackColor = Color.Green;
                }
                else if (flag == 1 && button.Name == "button2")
                {
                    button.BackColor = Color.Green;
                }
                else if (flag == 2 && button.Name == "button3")
                {
                    button.BackColor = Color.Green;
                }
                else if (flag == 3 && button.Name == "button4")
                {
                    button.BackColor = Color.Green;
                }
                else if (flag == 4 && button.Name == "button6")
                {
                    button.BackColor = Color.Green;
                }
                else if (flag == 5 && button.Name == "button7")
                {
                    button.BackColor = Color.Green;
                }
                else button.BackColor = Color.White;
            }
        }

        //相机0
        private void button1_Click(object sender, EventArgs e)
        {
            cam_op(0);
        }

        //相机1
        private void button2_Click(object sender, EventArgs e)
        {
            cam_op(1);
        }

        //相机2
        private void button3_Click(object sender, EventArgs e)
        {
            cam_op(2);
        }

        //相机3
        private void button4_Click(object sender, EventArgs e)
        {
            cam_op(3);
        }

        //相机4
        private void button6_Click(object sender, EventArgs e)
        {
            cam_op(4);
        }

        //相机5
        private void button7_Click(object sender, EventArgs e)
        {
            cam_op(5);
        }

        //相机操作
        private void cam_op(int index)
        {
            //复原相机角度
            if (button5.Text.Contains("*"))
            {
                button5.Text = button5.Text.Replace("*", "");
            }
            rotate = 0;
            isrotate = false;

            flag = index;
            if (comboBox1.Text == "")
            {
                MessageBox.Show("请先选择车道");
                return;
            }
            if (this.checkBox4.Checked == true)
            {
                buttonColor(splitContainer2.Panel2);
                CheckLine();
                string str = checkip(index);
                if (str == "rtsp://admin:wanji168@") return;
                showimage(str);
            }
            else
            {
                MessageBox.Show("相机未启用");
                return;
            }
            press = index;
        }

        //水平翻转
        private void Flip_Click(object sender, EventArgs e)
        {
            if (button5.Text.Contains("*"))
            {
                button5.Text = button5.Text.Replace("*", "");
            }
            else
            {
                button5.Text += "*";
            }
        }

        //车道选择 配置文件初始化
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //初始化
            this.button1.Visible = false;
            this.button2.Visible = false;
            this.button3.Visible = false;
            this.button4.Visible = false;
            this.button6.Visible = false;
            this.button7.Visible = false;
            int selectedIndex = this.comboBox1.SelectedIndex + 2;
            int lane_nums = 0;
            if (File.Exists("config.json"))
            {
                File.Delete("config.json");
            }
            configs.Clear();
            if (this.comboBox1.SelectedItem.ToString() == "2+0车道")
            {
                this.button1.Visible = false;
                this.button2.Visible = true;
                this.button2.Text = "相机2";
                this.button3.Visible = true;
                this.button3.Text = "相机1";
                this.checkBox0.Visible = false;
                this.checkBox3.Visible = false;
                this.checkBox4.Visible = false;
                this.checkBox5.Visible = false;
                this.pictureBox1.Load(@"road_2.jpeg");
                for (int i = 0; i < selectedIndex; i++)
                {
                    configs.Add(new ExtraConfig { ip = "" ,lane = selectedIndex - i});
                }
                lane_nums = selectedIndex;
                totalConfig.type = "2+0";
            }
            if (this.comboBox1.SelectedItem.ToString() == "2+1车道")
            {
                this.button1.Visible = true;
                this.button1.Text = "相机3";
                this.button2.Visible = true;
                this.button2.Text = "相机2";
                this.button3.Visible = true;
                this.button3.Text = "相机1";
                this.button4.Visible = false;
                this.checkBox0.Visible = true;
                this.checkBox3.Visible = false;
                this.checkBox4.Visible = false;
                this.checkBox5.Visible = false;
                this.pictureBox1.Load(@"road_2.jpeg");
                for (int i = 0; i < selectedIndex; i++)
                {
                    configs.Add(new ExtraConfig { ip = "", lane = selectedIndex - i });
                }
                lane_nums = selectedIndex;
                totalConfig.type = "2+1";
            }
            if (this.comboBox1.SelectedItem.ToString() == "3+0车道")
            {
                this.button1.Visible = false;
                this.button2.Visible = true;
                this.button2.Text = "相机3";
                this.button3.Visible = true;
                this.button3.Text = "相机2";
                this.button4.Visible = true;
                this.button4.Text = "相机1";
                this.button6.Visible = false;
                this.checkBox0.Visible = false;
                this.checkBox3.Visible = true;
                this.checkBox4.Visible = false;
                this.checkBox5.Visible = false;
                this.pictureBox1.Load(@"road_3.jpeg");
                for (int i = 0; i < selectedIndex - 1; i++)
                {
                    configs.Add(new ExtraConfig { ip = "", lane = selectedIndex - 1 - i });
                }
                lane_nums = selectedIndex - 1;
                totalConfig.type = "3+0";
            }
            if (this.comboBox1.SelectedItem.ToString() == "3+1车道")
            {
                this.button1.Visible = true;
                this.button1.Text = "相机4";
                this.button2.Visible = true;
                this.button2.Text = "相机3";
                this.button3.Visible = true;
                this.button3.Text = "相机2";
                this.button4.Visible = true;
                this.button4.Text = "相机1";
                this.button6.Visible = false;
                this.button7.Visible = false;
                this.checkBox0.Visible = true;
                this.checkBox3.Visible = true;
                this.checkBox4.Visible = false;
                this.checkBox5.Visible = false;
                this.pictureBox1.Load(@"road_3.jpeg");
                for (int i = 0; i < selectedIndex - 1; i++)
                {
                    configs.Add(new ExtraConfig { ip = "", lane = selectedIndex - 1 - i });
                }
                lane_nums = selectedIndex - 1;
                totalConfig.type = "3+1";
            }
            totalConfig.config = configs;
            totalConfig.nums = lane_nums;
            convert(totalConfig);
        }

        //车道配置
        private void button8_Click(object sender, EventArgs e)
        {
            if (File.Exists("config.json"))
            {
                var extras = JsonConvert.DeserializeObject<TotalConfig>(File.ReadAllText("config.json"));
                if (extras.type.Contains("2"))
                {
                    this.comboBox1.SelectedIndex = extras.nums - 2;
                }
                if (extras.type.Contains("3"))
                {
                    this.comboBox1.SelectedIndex = extras.nums - 1;
                }
                this.comboBox1.Refresh();

                totalConfig = extras;
                convert(totalConfig);
            }
        }

        //检查并写入相机ip 
        private string checkip(int camnum)
        {
            String str = "rtsp://admin:wanji168@";           
            if (File.Exists("config.json"))
            {
                var extras = JsonConvert.DeserializeObject<TotalConfig>(File.ReadAllText("config.json"));
                if (extras.type.Contains("0"))
                {
                    camnum--;//当没有应急车道的时候相机下标需要减一
                }
                if (extras.config[camnum].ip == "")
                {
                    ipinput.ShowDialog();
                    //string ip = Microsoft.VisualBasic.Interaction.InputBox("请输入相机ip", "输入ip", "");
                    if (IP == "") return str;
                    str += IP;
                    extras.config[camnum].ip = IP;
                    extras.config[camnum].state = true;                 
                    totalConfig = extras;
                    convert(totalConfig);

                }
                else
                {
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
        private void convert(TotalConfig totalConfig)
        {
            var res = JsonConvert.SerializeObject(totalConfig);
            string str = res.ToString();
            string temp = ConvertJsonString(str);
            var res1 = JsonConvert.DeserializeObject(temp);
            File.WriteAllText("config.json", res1.ToString());
        }

        //旋转按钮
        private void button9_Click(object sender, EventArgs e)
        {
            //rotate++;
            //isrotate = true;
            //if (rotate == 4)
            //{
            //    rotate = 0;
            //    isrotate = false;
            //}
            if (button9.Text.Contains("*"))
            {
                button9.Text = button9.Text.Replace("*", "");
            }
            else
            {
                button9.Text += "*";
            }
        }

        //播放按钮
        private void button10_Click(object sender, EventArgs e)
        {
            //string str = checkip(flag);
            //showimage(str);
            isopen = true;            
            //deepstream.start();

        }

        //车长文本框
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            CaculatePixeldis();
        }

        //车长画线
        private void button11_Click(object sender, EventArgs e)
        {
            if (button11.Text.Contains("*"))
            {
                button11.Text = button11.Text.Replace("*", "");
                CaculatePixeldis();
                this.drawPictureBoxVideo.mouseClickType = "";
                this.drawPictureBoxVideo.drawCache.clearDrawLineList();
                this.drawPictureBoxVideo.refresh();
                this.drawPictureBoxVideo.mouseClickType = "";
            }
            else
            {
                button11.Text += "*";
                this.drawPictureBoxVideo.mouseClickType = "drawLineX";
            }

        }

        //计算水平像素距离
        private void CaculatePixeldis()
        {
            int dis = 0;
            if (drawPictureBoxVideo.drawCache.drawLineList.Count != 0)
            {
                DrawPictureCache.DrawLine line = (DrawPictureCache.DrawLine)drawPictureBoxVideo.drawCache.drawLineList[0];
                dis = line.xMax - line.xMin;
            }
            if (File.Exists("config.json"))
            {
                var extras = JsonConvert.DeserializeObject<TotalConfig>(File.ReadAllText("config.json"));
                if (extras.type.Contains("0"))
                {
                    flag--;//当没有应急车道的时候相机下标需要减一
                    if (textBox1.Text != "")
                    {
                        extras.config[flag].length = double.Parse(textBox1.Text);
                        extras.config[flag].pixeldis = dis;
                        totalConfig = extras;
                        convert(totalConfig);
                    }
                    flag++;
                }
                else
                {
                    if (textBox1.Text != "")
                    {
                        extras.config[flag].length = double.Parse(textBox1.Text);
                        extras.config[flag].pixeldis = dis;
                        totalConfig = extras;
                        convert(totalConfig);
                    }
                }              
            }
        }

        //上传参数文件到/home
        private void button12_Click(object sender, EventArgs e)
        {
            //DialogResult result = MessageBox.Show("确认执行该操作吗？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (result == DialogResult.Yes)
            //{
            //    string sourceFilePath = @"config.json";
            //    string destinationFolderPath = "/home/net5.0";
            //    // 使用 Path 类的 GetFileName 方法获取文件名
            //    string fileName = Path.GetFileName(sourceFilePath);
            //    // 将文件复制到目标文件夹中
            //    string destinationFilePath = Path.Combine(destinationFolderPath, fileName);
            //    File.Copy(sourceFilePath, destinationFilePath, true);
            //    MessageBox.Show("上传成功");
            //}
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
                            sftpClient.Connect();
                            string localFilePath = "config.json";
                            string remoteFilePath = "/home/code/config.json";
                            using (var fileStream = new FileStream(localFilePath, FileMode.Open))
                            {
                                try
                                {
                                    sftpClient.UploadFile(fileStream, remoteFilePath);
                                    MessageBox.Show("上传成功");
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

    }
}
