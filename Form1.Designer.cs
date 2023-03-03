
namespace videocapture
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.button12 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button10 = new System.Windows.Forms.Button();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.button9 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button5 = new System.Windows.Forms.Button();
            this.drawLineToolStripMenuItem = new System.Windows.Forms.Button();
            this.setConfigToolStripMenuItem = new System.Windows.Forms.Button();
            this.readConfigToolStripMenuItem = new System.Windows.Forms.Button();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.drawPictureBoxVideo = new videocapture.DrawPictureBox();
            this.checkBox0 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.button12);
            this.splitContainer1.Panel1.Controls.Add(this.button11);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.textBox1);
            this.splitContainer1.Panel1.Controls.Add(this.button10);
            this.splitContainer1.Panel1.Controls.Add(this.checkBox6);
            this.splitContainer1.Panel1.Controls.Add(this.button9);
            this.splitContainer1.Panel1.Controls.Add(this.button8);
            this.splitContainer1.Panel1.Controls.Add(this.comboBox1);
            this.splitContainer1.Panel1.Controls.Add(this.button5);
            this.splitContainer1.Panel1.Controls.Add(this.drawLineToolStripMenuItem);
            this.splitContainer1.Panel1.Controls.Add(this.setConfigToolStripMenuItem);
            this.splitContainer1.Panel1.Controls.Add(this.readConfigToolStripMenuItem);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(958, 824);
            this.splitContainer1.SplitterDistance = 116;
            this.splitContainer1.TabIndex = 2;
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(632, 68);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(104, 39);
            this.button12.TabIndex = 18;
            this.button12.Text = "上传参数";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(147, 16);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(96, 46);
            this.button11.TabIndex = 17;
            this.button11.Text = "车长定位";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 18);
            this.label2.TabIndex = 16;
            this.label2.Text = "输入车长";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(123, 76);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(120, 28);
            this.textBox1.TabIndex = 15;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(272, 68);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(86, 39);
            this.button10.TabIndex = 14;
            this.button10.Text = "播放";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // checkBox6
            // 
            this.checkBox6.AutoSize = true;
            this.checkBox6.Location = new System.Drawing.Point(505, 76);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(106, 22);
            this.checkBox6.TabIndex = 13;
            this.checkBox6.Text = "标定模式";
            this.checkBox6.UseVisualStyleBackColor = true;
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(387, 68);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(91, 39);
            this.button9.TabIndex = 12;
            this.button9.Text = "旋转";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(632, 16);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(104, 46);
            this.button8.TabIndex = 11;
            this.button8.Text = "车道配置";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "2+0车道",
            "2+1车道",
            "3+0车道",
            "3+1车道"});
            this.comboBox1.Location = new System.Drawing.Point(789, 16);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(120, 26);
            this.comboBox1.TabIndex = 9;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(505, 16);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(91, 46);
            this.button5.TabIndex = 8;
            this.button5.Text = "水平翻转";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.Flip_Click);
            // 
            // drawLineToolStripMenuItem
            // 
            this.drawLineToolStripMenuItem.Location = new System.Drawing.Point(28, 16);
            this.drawLineToolStripMenuItem.Name = "drawLineToolStripMenuItem";
            this.drawLineToolStripMenuItem.Size = new System.Drawing.Size(89, 46);
            this.drawLineToolStripMenuItem.TabIndex = 5;
            this.drawLineToolStripMenuItem.Text = "画线";
            this.drawLineToolStripMenuItem.UseVisualStyleBackColor = true;
            this.drawLineToolStripMenuItem.Click += new System.EventHandler(this.drawLineToolStripMenuItem_Click);
            // 
            // setConfigToolStripMenuItem
            // 
            this.setConfigToolStripMenuItem.Location = new System.Drawing.Point(387, 16);
            this.setConfigToolStripMenuItem.Name = "setConfigToolStripMenuItem";
            this.setConfigToolStripMenuItem.Size = new System.Drawing.Size(86, 46);
            this.setConfigToolStripMenuItem.TabIndex = 4;
            this.setConfigToolStripMenuItem.Text = "写配置";
            this.setConfigToolStripMenuItem.UseVisualStyleBackColor = true;
            this.setConfigToolStripMenuItem.Click += new System.EventHandler(this.setConfigToolStripMenuItem_Click);
            // 
            // readConfigToolStripMenuItem
            // 
            this.readConfigToolStripMenuItem.Location = new System.Drawing.Point(272, 16);
            this.readConfigToolStripMenuItem.Name = "readConfigToolStripMenuItem";
            this.readConfigToolStripMenuItem.Size = new System.Drawing.Size(86, 46);
            this.readConfigToolStripMenuItem.TabIndex = 3;
            this.readConfigToolStripMenuItem.Text = "读配置";
            this.readConfigToolStripMenuItem.UseVisualStyleBackColor = true;
            this.readConfigToolStripMenuItem.Click += new System.EventHandler(this.readConfigToolStripMenuItem_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.drawPictureBoxVideo);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.checkBox0);
            this.splitContainer2.Panel2.Controls.Add(this.checkBox1);
            this.splitContainer2.Panel2.Controls.Add(this.checkBox2);
            this.splitContainer2.Panel2.Controls.Add(this.checkBox3);
            this.splitContainer2.Panel2.Controls.Add(this.checkBox4);
            this.splitContainer2.Panel2.Controls.Add(this.checkBox5);
            this.splitContainer2.Panel2.Controls.Add(this.button7);
            this.splitContainer2.Panel2.Controls.Add(this.button6);
            this.splitContainer2.Panel2.Controls.Add(this.button4);
            this.splitContainer2.Panel2.Controls.Add(this.button3);
            this.splitContainer2.Panel2.Controls.Add(this.button2);
            this.splitContainer2.Panel2.Controls.Add(this.button1);
            this.splitContainer2.Panel2.Controls.Add(this.pictureBox1);
            this.splitContainer2.Size = new System.Drawing.Size(958, 704);
            this.splitContainer2.SplitterDistance = 594;
            this.splitContainer2.TabIndex = 0;
            // 
            // drawPictureBoxVideo
            // 
            this.drawPictureBoxVideo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.drawPictureBoxVideo.Location = new System.Drawing.Point(0, 0);
            this.drawPictureBoxVideo.Margin = new System.Windows.Forms.Padding(4);
            this.drawPictureBoxVideo.Name = "drawPictureBoxVideo";
            this.drawPictureBoxVideo.Size = new System.Drawing.Size(594, 704);
            this.drawPictureBoxVideo.TabIndex = 0;
            // 
            // checkBox0
            // 
            this.checkBox0.AutoSize = true;
            this.checkBox0.Checked = true;
            this.checkBox0.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox0.Location = new System.Drawing.Point(55, 611);
            this.checkBox0.Name = "checkBox0";
            this.checkBox0.Size = new System.Drawing.Size(22, 21);
            this.checkBox0.TabIndex = 12;
            this.checkBox0.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(55, 495);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(22, 21);
            this.checkBox1.TabIndex = 11;
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Location = new System.Drawing.Point(55, 382);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(22, 21);
            this.checkBox2.TabIndex = 10;
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Checked = true;
            this.checkBox3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox3.Location = new System.Drawing.Point(55, 271);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(22, 21);
            this.checkBox3.TabIndex = 9;
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Checked = true;
            this.checkBox4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox4.Location = new System.Drawing.Point(55, 160);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(22, 21);
            this.checkBox4.TabIndex = 8;
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.Checked = true;
            this.checkBox5.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox5.Location = new System.Drawing.Point(55, 51);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(22, 21);
            this.checkBox5.TabIndex = 7;
            this.checkBox5.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(105, 27);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(129, 66);
            this.button7.TabIndex = 6;
            this.button7.Text = "相机5";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(105, 136);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(129, 67);
            this.button6.TabIndex = 5;
            this.button6.Text = "相机4";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(105, 247);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(129, 67);
            this.button4.TabIndex = 4;
            this.button4.Text = "相机3";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(105, 358);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(129, 67);
            this.button3.TabIndex = 3;
            this.button3.Text = "相机2";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(105, 471);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(129, 67);
            this.button2.TabIndex = 2;
            this.button2.Text = "相机1";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(105, 587);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(129, 67);
            this.button1.TabIndex = 1;
            this.button1.Text = "相机0";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(360, 704);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(796, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 28);
            this.label1.TabIndex = 19;
            this.label1.Text = "道路类型";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(958, 824);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.VideoForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button setConfigToolStripMenuItem;
        private System.Windows.Forms.Button readConfigToolStripMenuItem;
        private System.Windows.Forms.Button drawLineToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer2;
        public DrawPictureBox drawPictureBoxVideo = new DrawPictureBox();
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.CheckBox checkBox0;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.CheckBox checkBox6;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Label label1;
    }
}

