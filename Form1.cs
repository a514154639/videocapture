﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Threading;
using System.IO;
using Newtonsoft.Json;
//using Microsoft.VisualBasic;
using System.Runtime.InteropServices;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Web.Script.Serialization;

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
            public List<ExtraConfig> config = new List<ExtraConfig>();
        }

        public class ExtraConfig
        {
            public string ip;
            public DrawPictureCache.DrawLine line;
            //public List<DrawPictureCache.DrawLine> line = new List<DrawPictureCache.DrawLine>();
            public DrawPictureCache.DrawRectangle gGetDrawRectangle;
            public bool state = false;
            public double length;
            public int pixeldis;

        }
        
        private void Receive(string str)
        {
            IP = str;
        }
        private void readConfig()
        {
            this.drawPictureBoxVideo.drawCache.clearDrawRectangleList();
            this.drawPictureBoxVideo.drawCache.clearDrawLineList();
            try
            {
                if (File.Exists("config.json"))
                {
                    var extras = JsonConvert.DeserializeObject<TotalConfig>(File.ReadAllText("config.json"));
                    extraConfig = extras.config[flag];
                    if (extras.config[flag].gGetDrawRectangle == null || extras.config[flag].line == null)
                    {
                        MessageBox.Show("未配置参数");
                        return;
                    }
                    this.drawPictureBoxVideo.drawCache.addDrawLineList(extraConfig.line.xMin, extraConfig.line.yMin, extraConfig.line.xMax, extraConfig.line.yMax, 3, Color.Blue);
                    var rec = extraConfig.gGetDrawRectangle;
                    this.drawPictureBoxVideo.drawCache.addDrawRectangleList(rec.xMin, rec.yMin, rec.xMax, rec.yMax, 3, Color.Blue);
                    DrawPictureCache.gGetDrawRectangle = rec;
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
                //extraConfig.line = DrawPictureCache.LineList;
                extraConfig.line = DrawPictureCache.gGetDrawLine;
                extraConfig.gGetDrawRectangle = DrawPictureCache.gGetDrawRectangle;
                if (File.Exists("config.json"))
                {
                    var extras = JsonConvert.DeserializeObject<TotalConfig>(File.ReadAllText("config.json"));
                    extras.nums = this.comboBox1.SelectedIndex + 3;
                    extras.config[flag].line = DrawPictureCache.gGetDrawLine;
                    //extras.config[flag].line = DrawPictureCache.LineList;
                    extras.config[flag].gGetDrawRectangle = DrawPictureCache.gGetDrawRectangle;
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

        //private void CleanRectangle()
        //{
        //    drawRangeToolStripMenuItem.Text = drawRangeToolStripMenuItem.Text.Replace("*", "");
        //    this.drawPictureBoxVideo.mouseClickType = "";
        //    this.drawPictureBoxVideo.drawCache.clearDrawRectangleList();
        //    try
        //    {
        //        for (int index = 0; index < cutWithLineShowIndex.Count; index++)
        //        {
        //            this.drawPictureBoxVideo.drawCache.removeDrawLineList(cutWithLineShowIndex[index]);
        //        }
        //    }
        //    catch { }
        //    this.drawPictureBoxVideo.refresh();
        //    this.drawPictureBoxVideo.mouseClickType = "";
        //}

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

        //画框
        //private void drawRangeToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    if (drawRangeToolStripMenuItem.Text.Contains("*"))
        //    {
        //        CleanRectangle();
        //    }
        //    else
        //    {
        //        drawRangeToolStripMenuItem.Text += "*";
        //        this.drawPictureBoxVideo.mouseClickType = "drawRectangle";
        //    }

        //}

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
            if (can_saveconfig())
            {
                setConfig();
            }
            else
            {
                MessageBox.Show("当前线框不合理");
            }

        }

        //视频线程
        private void videoThread()
        {
            //IntPtr pt = TensorRT.Ini();
            //Console.WriteLine(pt);
            while (true)
            {
                try
                {
                    //showCurrVideoFrame(pt);
                    showCurrVideoFrame1();
                }
                catch { }
                Thread.Sleep(1);
            }
        }

        DrawPictureCache.DrawRectangle rec = DrawPictureCache.gGetDrawRectangle;

        //获取当前帧       
        private void showCurrVideoFrame(IntPtr pt)
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

                        //镜像
                        if (button5.Text.Contains("*"))
                        {
                            currBitmap = cv2Video.currFrameGetImageFlip(true, rotate);//当前帧
                            //GC.Collect();
                            if (isrotate)
                            {
                                currBitmap = cv2Video.currFrameGetImageFlip(true, rotate);//当前帧
                                GC.Collect();
                            }
                        }
                        //旋转
                        else if (isrotate)
                        {
                            currBitmap = cv2Video.currFrameGetImageRotate(rotate);//当前帧
                            //GC.Collect();
                            if (button5.Text.Contains("*"))
                            {
                                currBitmap = cv2Video.currFrameGetImageFlip(true, rotate);
                                GC.Collect();
                            }
                        }

                        else
                        {
                            currBitmap = cv2Video.currFrameGetImageFlip(false, 0);//当前帧
                        }

                        if (currBitmap != null)
                        {
                            //显示                           
                            //截取客车
                            if (true)
                            {
                                Mat mat = Cv2Mat.ImageToMat(currBitmap);
                                //YoloOnnx _YoloOnnx = YoloOnnx.GetInstance();
                                YoloOnnx.PredictResult ret;
                                int frame_num = cv2Video.getCurrFrameIndex();
                                bool jumpflag = true;
                                if (jumpflag && frame_num % 2 == 0)
                                {
                                    return;
                                }
                                Console.WriteLine(mat.Size());
                                bool res = TensorRT.PredictGPU(mat, out ret, pt);
                                //Console.WriteLine(res);
                                //bool res = _YoloOnnx.Predict(mat, out ret);
                                if (res)
                                {
                                    #region 车头车尾逻辑
                                    //var extras = JsonConvert.DeserializeObject<TotalConfig>(File.ReadAllText("config.json"));
                                    //if (ret.Targets.Count > 0)
                                    //{
                                    //    Console.WriteLine(ret.Targets.Count);

                                    //}

                                    //if (ret.Targets.Count >= 2)
                                    //{
                                    //    for (int i = 0; i < ret.Targets.Count; i++)
                                    //    {
                                    //        var box = ret.Targets[i].Box;
                                    //        if (extras.config[flag].line != null)
                                    //        {
                                    //            var line = extras.config[flag].line;
                                    //            if (box.Y + box.Height < line.yMax + (line.yMax - line.yMin) * 0.2 && box.Y + box.Height > line.yMin + (line.yMax - line.yMin) * 0.2)//LineList 按照 y值从小到大排序，车辆下沿要大于车道线下沿1.3，同时要大于上沿1.3
                                    //            {
                                    //                drawPictureBoxVideo.setImage(currBitmap);//显示
                                    //                //currBitmap.Save("car.jpg");
                                    //                isopen = false;
                                    //            }

                                    //        }
                                    //        else
                                    //        {
                                    //            drawPictureBoxVideo.setImage(currBitmap);//显示
                                    //            isopen = false;
                                    //        }

                                    //    }

                                    //}
                                    #endregion

                                    #region s_car逻辑    
                                    int preframe = 0;
                                    var extras = JsonConvert.DeserializeObject<TotalConfig>(File.ReadAllText("config.json"));
                                    if (ret.Targets.Count > 0)
                                    {
                                        Console.WriteLine(ret.Targets.Count);
                                        Console.WriteLine(ret.Targets[0].Label);
                                        int curframe = cv2Video.getCurrFrameIndex();
                                        if (curframe == stopframe && ret.Targets[0].Label == "1")
                                        {
                                            drawPictureBoxVideo.setImage(currBitmap);//显示
                                            isopen = false;
                                            //MessageBox.Show("触发");
                                        }

                                        if (ret.Targets.Count >= 2)
                                        {
                                            ret.Targets = ret.Targets.OrderByDescending(o => o.Box.X).ToList();
                                        }
                                        for (int i = 0; i < ret.Targets.Count; i++)
                                        {
                                            //车道判断
                                            //var box = ret.Targets[i].Box;
                                            //if (extras.config[flag].line != null)
                                            //{
                                            //    var line = extras.config[flag].line;
                                            //    if (box.Y + box.Height < line.yMax + (line.yMax - line.yMin) * 0.2 && box.Y + box.Height > line.yMin + (line.yMax - line.yMin) * 0.2)//LineList 按照 y值从小到大排序，车辆下沿要大于车道线下沿1.3，同时要大于上沿1.3
                                            //    {
                                            //        if (ret.Targets.Count >= 1 && ret.Targets[i].Label == "1")
                                            //        {
                                            //            preframe = cv2Video.getCurrFrameIndex();//获取触发帧序号

                                            //        }
                                            //    }

                                            //}
                                            if (ret.Targets.Count >= 1 && ret.Targets[i].Label == "1")
                                            {
                                                preframe = cv2Video.getCurrFrameIndex();//获取触发帧序号

                                            }

                                            break;
                                        }
                                        stopframe = preframe + 2;

                                    }
                                    #endregion

                                    //显示
                                    //this.Invoke(new ThreadStart(delegate
                                    //{

                                    //    drawPictureBoxVideo.setImage(currBitmap);//显示

                                    //}));
                                    drawPictureBoxVideo.setImage(currBitmap);
                                    Cv2.WaitKey(1);
                                }

                            }
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

        //onnx
        private void showCurrVideoFrame1()
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

                        //镜像
                        if (button5.Text.Contains("*"))
                        {
                            currBitmap = cv2Video.currFrameGetImageFlip(true, rotate);//当前帧
                            //GC.Collect();
                            if (isrotate)
                            {
                                currBitmap = cv2Video.currFrameGetImageFlip(true,rotate);//当前帧
                                GC.Collect();
                            }
                        }
                        //旋转
                        else if (isrotate)
                        {
                            currBitmap = cv2Video.currFrameGetImageRotate(rotate);//当前帧
                            //GC.Collect();
                            if (button5.Text.Contains("*"))
                            {
                                currBitmap = cv2Video.currFrameGetImageFlip(true, rotate);
                                GC.Collect();
                            }
                        }

                        else
                        {
                            currBitmap = cv2Video.currFrameGetImageFlip(false, 0);//当前帧
                        }
                        //int res = deepstream.Ini();

                        if (currBitmap != null)
                        {
                            //截取客车
                            if (checkBox6.Checked)
                            {
                                
                                //Mat mat = BitmapConverter.ToMat(currBitmap);
                                Mat mat = Cv2Mat.ImageToMat(currBitmap);
                                YoloOnnx _YoloOnnx = YoloOnnx.GetInstance();
                                YoloOnnx.PredictResult ret;
                                int frame_num = cv2Video.getCurrFrameIndex();
                                bool jumpflag = true;
                                if (jumpflag && frame_num % 2 == 0)
                                {
                                    return;
                                }

                                if (_YoloOnnx.Predict(mat, out ret))
                                {
                                    int preframe = 0;

                                    if (ret.Targets.Count > 0)
                                    {
                                        int curframe = cv2Video.getCurrFrameIndex();
                                        if (curframe == stopframe && ret.Targets[0].Label == "1")
                                        {
                                            isopen = false;
                                        }

                                        if (ret.Targets.Count >= 2)
                                        {
                                            ret.Targets = ret.Targets.OrderByDescending(o => o.Box.X).ToList();
                                        }
                                        for (int i = 0; i < ret.Targets.Count; i++)
                                        {

                                            if (ret.Targets.Count >= 1 && ret.Targets[i].Label == "1")
                                            {
                                                preframe = cv2Video.getCurrFrameIndex();//获取触发帧序号

                                            }
                                            break;
                                        }
                                        stopframe = preframe + 2;

                                    }

                                }

                                //Console.WriteLine(sw.ElapsedMilliseconds);
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
            bool res = cv2Video.openRtsp(str + ":554 latency = 0 ! rtph265depay ! h265parse ! omxh265dec ! video/x-raw,format=(string)BGRx, width=(int)1080,height=(int)1920, ! videoconvert ! appsink sync = false");
            //bool res = cv2Video.openVideoFile(@"demo.mp4");
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
            if (drawPictureBoxVideo.drawCache.drawLineList.Count != 0 && can_saveconfig())
            {
                if (extras.config[press].line != null)
                {
                    //DrawPictureCache.DrawRectangle rec1 = (DrawPictureCache.DrawRectangle)drawPictureBoxVideo.drawCache.drawRectangleList[0];
                    //var rec2 = extras.config[press].gGetDrawRectangle;
                    DrawPictureCache.DrawLine line1 = (DrawPictureCache.DrawLine)drawPictureBoxVideo.drawCache.drawLineList[0];
                    var line2 = extras.config[press].line;
                    //int ans1 = rec1.xMax + rec1.xMin + rec1.yMax + rec1.yMin;
                    //int ans2 = rec2.xMax + rec2.xMin + rec2.yMax + rec2.yMin;
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

        //判断当前线框是否合理
        private bool can_saveconfig()
        {
            if (drawPictureBoxVideo.drawCache.drawRectangleList.Count != 0)
            {
                DrawPictureCache.DrawRectangle rec1 = (DrawPictureCache.DrawRectangle)drawPictureBoxVideo.drawCache.drawRectangleList[0];
                DrawPictureCache.DrawLine line1 = (DrawPictureCache.DrawLine)drawPictureBoxVideo.drawCache.drawLineList[0];
                if (line1.yMax > rec1.yMax || line1.xMin < rec1.xMin || line1.xMax > rec1.xMax || line1.yMin < rec1.yMin)
                {
                    return false;
                }
                else return true;

            }
            else
            {
                MessageBox.Show("当前未画框");
                return false;
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
            flag = 0;
            if (comboBox1.Text == "")
            {
                MessageBox.Show("请先选择车道");
                return;
            }
            if (this.checkBox0.Checked == true)
            {
                buttonColor(splitContainer2.Panel2);
                CheckLine();
                string str = checkip(0);
                if (str == "rtsp://admin:wanji168@") return;
                showimage(str);
                //isopen = true;
            }
            else
            {
                MessageBox.Show("相机未启用");
                return;
            }
            press = 0;

        }

        //相机1
        private void button2_Click(object sender, EventArgs e)
        {
            flag = 1;
            if (comboBox1.Text == "")
            {
                MessageBox.Show("请先选择车道");
                return;
            }
            if (this.checkBox1.Checked == true)
            {
                buttonColor(splitContainer2.Panel2);
                CheckLine();
                string str = checkip(1);
                if (str == "rtsp://admin:wanji168@") return;
                showimage(str);
            }
            else
            {
                MessageBox.Show("相机未启用");
                return;
            }
            press = 1;

        }

        //相机2
        private void button3_Click(object sender, EventArgs e)
        {
            flag = 2;
            if (comboBox1.Text == "")
            {
                MessageBox.Show("请先选择车道");
                return;
            }
            if (this.checkBox2.Checked == true)
            {
                buttonColor(splitContainer2.Panel2);
                CheckLine();
                string str = checkip(2);
                if (str == "rtsp://admin:wanji168@") return;
                showimage(str);
            }
            else
            {
                MessageBox.Show("相机未启用");
                return;
            }
            press = 2;

        }

        //相机3
        private void button4_Click(object sender, EventArgs e)
        {
            flag = 3;
            if (comboBox1.Text == "")
            {
                MessageBox.Show("请先选择车道");
                return;
            }
            if (this.checkBox3.Checked == true)
            {
                buttonColor(splitContainer2.Panel2);
                CheckLine();
                string str = checkip(3);
                if (str == "rtsp://admin:wanji168@") return;
                showimage(str);
            }
            else
            {
                MessageBox.Show("相机未启用");
                return;
            }
            press = 3;
        }

        //相机4
        private void button6_Click(object sender, EventArgs e)
        {
            flag = 4;
            if (comboBox1.Text == "")
            {
                MessageBox.Show("请先选择车道");
                return;
            }
            if (this.checkBox4.Checked == true)
            {
                buttonColor(splitContainer2.Panel2);
                CheckLine();
                string str = checkip(4);
                if (str == "rtsp://admin:wanji168@") return;
                showimage(str);
            }
            else
            {
                MessageBox.Show("相机未启用");
                return;
            }
            press = 4;
        }

        //相机5
        private void button7_Click(object sender, EventArgs e)
        {
            flag = 5;
            if (comboBox1.Text == "")
            {
                MessageBox.Show("请先选择车道");
                return;
            }
            if (this.checkBox5.Checked == true)
            {
                buttonColor(splitContainer2.Panel2);
                CheckLine();
                string str = checkip(5);
                if (str == "rtsp://admin:wanji168@") return;
                showimage(str);
            }
            else
            {
                MessageBox.Show("相机未启用");
                return;
            }
            press = 5;
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
            if (File.Exists("config.json"))
            {
                File.Delete("config.json");
            }
            configs.Clear();
            if (this.comboBox1.SelectedItem.ToString() == "2+1车道")
            {
                this.button1.Visible = true;
                this.button2.Visible = true;
                this.button3.Visible = true;
                this.checkBox3.Visible = false;
                this.checkBox4.Visible = false;
                this.checkBox5.Visible = false;
                for (int i = 0; i < this.comboBox1.SelectedIndex + 3; i++)
                {
                    configs.Add(new ExtraConfig { ip = "" });
                }
            }
            if (this.comboBox1.SelectedItem.ToString() == "3+1车道")
            {
                this.button1.Visible = true;
                this.button2.Visible = true;
                this.button3.Visible = true;
                this.button4.Visible = true;
                this.checkBox3.Visible = true;
                this.checkBox4.Visible = false;
                this.checkBox5.Visible = false;
                for (int i = 0; i < this.comboBox1.SelectedIndex + 3; i++)
                {
                    configs.Add(new ExtraConfig { ip = "" });
                }
            }
            if (this.comboBox1.SelectedItem.ToString() == "4+1车道")
            {
                this.button1.Visible = true;
                this.button2.Visible = true;
                this.button3.Visible = true;
                this.button4.Visible = true;
                this.button6.Visible = true;
                this.checkBox3.Visible = true;
                this.checkBox4.Visible = true;
                this.checkBox5.Visible = false;
                for (int i = 0; i < this.comboBox1.SelectedIndex + 3; i++)
                {
                    configs.Add(new ExtraConfig { ip = "" });
                }
            }
            if (this.comboBox1.SelectedItem.ToString() == "5+1车道")
            {
                this.button1.Visible = true;
                this.button2.Visible = true;
                this.button3.Visible = true;
                this.button4.Visible = true;
                this.button6.Visible = true;
                this.button7.Visible = true;
                this.checkBox3.Visible = true;
                this.checkBox4.Visible = true;
                this.checkBox5.Visible = true;
                for (int i = 0; i < this.comboBox1.SelectedIndex + 3; i++)
                {
                    configs.Add(new ExtraConfig { ip = "" });
                }
            }
            totalConfig.config = configs;
            totalConfig.nums = this.comboBox1.SelectedIndex + 3;
            convert(totalConfig);
        }

        //车道配置
        private void button8_Click(object sender, EventArgs e)
        {
            if (File.Exists("config.json"))
            {
                var extras = JsonConvert.DeserializeObject<TotalConfig>(File.ReadAllText("config.json"));
                this.comboBox1.SelectedIndex = extras.nums - 3;
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
            rotate++;
            isrotate = true;
            if (rotate == 4)
            {
                rotate = 0;
                isrotate = false;
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
                if (textBox1.Text != "")
                {
                    extras.config[flag].length = double.Parse(textBox1.Text);
                    extras.config[flag].pixeldis = dis;
                    totalConfig = extras;
                    convert(totalConfig);
                }

            }
        }

        //复制参数文件到/home
        private void button12_Click(object sender, EventArgs e)
        {

        }
    }
}
