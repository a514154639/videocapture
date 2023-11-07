
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
            this.check_cover_mode = new System.Windows.Forms.CheckBox();
            this.check_line_mode = new System.Windows.Forms.CheckBox();
            this.drawcombainframe = new System.Windows.Forms.Button();
            this.Concatenate_frames = new System.Windows.Forms.Button();
            this.caculate_rl = new System.Windows.Forms.Button();
            this.cam_angel = new System.Windows.Forms.TextBox();
            this.landwidth_box = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.upload_json = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cam_height = new System.Windows.Forms.TextBox();
            this.read_roadtype = new System.Windows.Forms.Button();
            this.roadtype_box = new System.Windows.Forms.ComboBox();
            this.gridline = new System.Windows.Forms.Button();
            this.drawLineToolStripMenuItem = new System.Windows.Forms.Button();
            this.setConfigToolStripMenuItem = new System.Windows.Forms.Button();
            this.readConfigToolStripMenuItem = new System.Windows.Forms.Button();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.cam_2 = new System.Windows.Forms.Button();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.cam_3 = new System.Windows.Forms.Button();
            this.info_box = new System.Windows.Forms.TextBox();
            this.checkBox0 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.cam_5 = new System.Windows.Forms.Button();
            this.cam_4 = new System.Windows.Forms.Button();
            this.cam_1 = new System.Windows.Forms.Button();
            this.cam_0 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.drawPictureBoxVideo = new videocapture.DrawPictureBox();
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
            this.splitContainer1.Panel1.Controls.Add(this.check_cover_mode);
            this.splitContainer1.Panel1.Controls.Add(this.check_line_mode);
            this.splitContainer1.Panel1.Controls.Add(this.drawcombainframe);
            this.splitContainer1.Panel1.Controls.Add(this.Concatenate_frames);
            this.splitContainer1.Panel1.Controls.Add(this.caculate_rl);
            this.splitContainer1.Panel1.Controls.Add(this.cam_angel);
            this.splitContainer1.Panel1.Controls.Add(this.landwidth_box);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.upload_json);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.cam_height);
            this.splitContainer1.Panel1.Controls.Add(this.read_roadtype);
            this.splitContainer1.Panel1.Controls.Add(this.roadtype_box);
            this.splitContainer1.Panel1.Controls.Add(this.gridline);
            this.splitContainer1.Panel1.Controls.Add(this.drawLineToolStripMenuItem);
            this.splitContainer1.Panel1.Controls.Add(this.setConfigToolStripMenuItem);
            this.splitContainer1.Panel1.Controls.Add(this.readConfigToolStripMenuItem);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1122, 1428);
            this.splitContainer1.SplitterDistance = 199;
            this.splitContainer1.TabIndex = 2;
            // 
            // check_cover_mode
            // 
            this.check_cover_mode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.check_cover_mode.AutoSize = true;
            this.check_cover_mode.Location = new System.Drawing.Point(845, 164);
            this.check_cover_mode.Name = "check_cover_mode";
            this.check_cover_mode.Size = new System.Drawing.Size(178, 22);
            this.check_cover_mode.TabIndex = 33;
            this.check_cover_mode.Text = "车道易被大车遮挡";
            this.check_cover_mode.UseVisualStyleBackColor = true;
            // 
            // check_line_mode
            // 
            this.check_line_mode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.check_line_mode.AutoSize = true;
            this.check_line_mode.Location = new System.Drawing.Point(845, 136);
            this.check_line_mode.Name = "check_line_mode";
            this.check_line_mode.Size = new System.Drawing.Size(142, 22);
            this.check_line_mode.TabIndex = 32;
            this.check_line_mode.Text = "画线右侧拼帧";
            this.check_line_mode.UseVisualStyleBackColor = true;
            // 
            // drawcombainframe
            // 
            this.drawcombainframe.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.drawcombainframe.Location = new System.Drawing.Point(705, 58);
            this.drawcombainframe.Name = "drawcombainframe";
            this.drawcombainframe.Size = new System.Drawing.Size(108, 64);
            this.drawcombainframe.TabIndex = 31;
            this.drawcombainframe.Text = "拼帧选点";
            this.drawcombainframe.UseVisualStyleBackColor = true;
            this.drawcombainframe.Click += new System.EventHandler(this.drawcombainframe_Click);
            // 
            // Concatenate_frames
            // 
            this.Concatenate_frames.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Concatenate_frames.Location = new System.Drawing.Point(705, 128);
            this.Concatenate_frames.Name = "Concatenate_frames";
            this.Concatenate_frames.Size = new System.Drawing.Size(108, 64);
            this.Concatenate_frames.TabIndex = 30;
            this.Concatenate_frames.Text = "拼帧测试";
            this.Concatenate_frames.UseVisualStyleBackColor = true;
            this.Concatenate_frames.Click += new System.EventHandler(this.Concatenate_frames_Click);
            // 
            // caculate_rl
            // 
            this.caculate_rl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.caculate_rl.Location = new System.Drawing.Point(159, 58);
            this.caculate_rl.Name = "caculate_rl";
            this.caculate_rl.Size = new System.Drawing.Size(115, 64);
            this.caculate_rl.TabIndex = 28;
            this.caculate_rl.Text = "计算标尺";
            this.caculate_rl.UseVisualStyleBackColor = true;
            this.caculate_rl.Click += new System.EventHandler(this.Caculate_Click);
            // 
            // cam_angel
            // 
            this.cam_angel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cam_angel.Location = new System.Drawing.Point(495, 151);
            this.cam_angel.Name = "cam_angel";
            this.cam_angel.Size = new System.Drawing.Size(49, 28);
            this.cam_angel.TabIndex = 23;
            this.cam_angel.Text = "104";
            this.cam_angel.TextChanged += new System.EventHandler(this.Camangel_TextChanged);
            // 
            // landwidth_box
            // 
            this.landwidth_box.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.landwidth_box.Location = new System.Drawing.Point(307, 151);
            this.landwidth_box.Name = "landwidth_box";
            this.landwidth_box.Size = new System.Drawing.Size(47, 28);
            this.landwidth_box.TabIndex = 22;
            this.landwidth_box.Text = "3.75";
            this.landwidth_box.TextChanged += new System.EventHandler(this.Landwidth_TextChanged);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(369, 154);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 21);
            this.label4.TabIndex = 21;
            this.label4.Text = "纵向视场角";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(974, 105);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 28);
            this.label1.TabIndex = 19;
            this.label1.Text = "道路类型";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(203, 154);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 21);
            this.label3.TabIndex = 19;
            this.label3.Text = "车道宽度";
            // 
            // upload_json
            // 
            this.upload_json.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.upload_json.Location = new System.Drawing.Point(561, 58);
            this.upload_json.Name = "upload_json";
            this.upload_json.Size = new System.Drawing.Size(108, 64);
            this.upload_json.TabIndex = 18;
            this.upload_json.Text = "上传参数";
            this.upload_json.UseVisualStyleBackColor = true;
            this.upload_json.Click += new System.EventHandler(this.Uploadjson_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(34, 154);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 21);
            this.label2.TabIndex = 16;
            this.label2.Text = "相机高度";
            // 
            // cam_height
            // 
            this.cam_height.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cam_height.Location = new System.Drawing.Point(138, 151);
            this.cam_height.Name = "cam_height";
            this.cam_height.Size = new System.Drawing.Size(49, 28);
            this.cam_height.TabIndex = 15;
            this.cam_height.Text = "6.7";
            this.cam_height.TextChanged += new System.EventHandler(this.Camheight_TextChanged);
            // 
            // read_roadtype
            // 
            this.read_roadtype.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.read_roadtype.Location = new System.Drawing.Point(833, 58);
            this.read_roadtype.Name = "read_roadtype";
            this.read_roadtype.Size = new System.Drawing.Size(108, 64);
            this.read_roadtype.TabIndex = 11;
            this.read_roadtype.Text = "车道配置";
            this.read_roadtype.UseVisualStyleBackColor = true;
            this.read_roadtype.Click += new System.EventHandler(this.Roadtype_Click);
            // 
            // roadtype_box
            // 
            this.roadtype_box.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.roadtype_box.FormattingEnabled = true;
            this.roadtype_box.Items.AddRange(new object[] {
            "2+0车道",
            "2+1车道",
            "3+0车道",
            "3+1车道",
            "服务区_入口",
            "服务区_出口"});
            this.roadtype_box.Location = new System.Drawing.Point(978, 58);
            this.roadtype_box.Name = "roadtype_box";
            this.roadtype_box.Size = new System.Drawing.Size(120, 26);
            this.roadtype_box.TabIndex = 9;
            this.roadtype_box.SelectedIndexChanged += new System.EventHandler(this.Roadtypebox_SelectedIndexChanged);
            // 
            // gridline
            // 
            this.gridline.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.gridline.Location = new System.Drawing.Point(437, 58);
            this.gridline.Name = "gridline";
            this.gridline.Size = new System.Drawing.Size(106, 64);
            this.gridline.TabIndex = 8;
            this.gridline.Text = "网格线";
            this.gridline.UseVisualStyleBackColor = true;
            this.gridline.Click += new System.EventHandler(this.Grid_Click);
            // 
            // drawLineToolStripMenuItem
            // 
            this.drawLineToolStripMenuItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.drawLineToolStripMenuItem.Location = new System.Drawing.Point(25, 58);
            this.drawLineToolStripMenuItem.Name = "drawLineToolStripMenuItem";
            this.drawLineToolStripMenuItem.Size = new System.Drawing.Size(107, 64);
            this.drawLineToolStripMenuItem.TabIndex = 5;
            this.drawLineToolStripMenuItem.Text = "画线";
            this.drawLineToolStripMenuItem.UseVisualStyleBackColor = true;
            this.drawLineToolStripMenuItem.Click += new System.EventHandler(this.DrawLineToolStripMenuItem_Click);
            // 
            // setConfigToolStripMenuItem
            // 
            this.setConfigToolStripMenuItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.setConfigToolStripMenuItem.Location = new System.Drawing.Point(300, 58);
            this.setConfigToolStripMenuItem.Name = "setConfigToolStripMenuItem";
            this.setConfigToolStripMenuItem.Size = new System.Drawing.Size(111, 64);
            this.setConfigToolStripMenuItem.TabIndex = 4;
            this.setConfigToolStripMenuItem.Text = "写配置";
            this.setConfigToolStripMenuItem.UseVisualStyleBackColor = true;
            this.setConfigToolStripMenuItem.Click += new System.EventHandler(this.SetConfigToolStripMenuItem_Click);
            // 
            // readConfigToolStripMenuItem
            // 
            this.readConfigToolStripMenuItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.readConfigToolStripMenuItem.Location = new System.Drawing.Point(561, 128);
            this.readConfigToolStripMenuItem.Name = "readConfigToolStripMenuItem";
            this.readConfigToolStripMenuItem.Size = new System.Drawing.Size(111, 64);
            this.readConfigToolStripMenuItem.TabIndex = 3;
            this.readConfigToolStripMenuItem.Text = "读配置";
            this.readConfigToolStripMenuItem.UseVisualStyleBackColor = true;
            this.readConfigToolStripMenuItem.Click += new System.EventHandler(this.ReadConfigToolStripMenuItem_Click);
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
            this.splitContainer2.Panel2.Controls.Add(this.checkBox2);
            this.splitContainer2.Panel2.Controls.Add(this.cam_2);
            this.splitContainer2.Panel2.Controls.Add(this.checkBox3);
            this.splitContainer2.Panel2.Controls.Add(this.cam_3);
            this.splitContainer2.Panel2.Controls.Add(this.info_box);
            this.splitContainer2.Panel2.Controls.Add(this.checkBox0);
            this.splitContainer2.Panel2.Controls.Add(this.checkBox1);
            this.splitContainer2.Panel2.Controls.Add(this.checkBox4);
            this.splitContainer2.Panel2.Controls.Add(this.checkBox5);
            this.splitContainer2.Panel2.Controls.Add(this.cam_5);
            this.splitContainer2.Panel2.Controls.Add(this.cam_4);
            this.splitContainer2.Panel2.Controls.Add(this.cam_1);
            this.splitContainer2.Panel2.Controls.Add(this.cam_0);
            this.splitContainer2.Panel2.Controls.Add(this.pictureBox1);
            this.splitContainer2.Size = new System.Drawing.Size(1122, 1225);
            this.splitContainer2.SplitterDistance = 525;
            this.splitContainer2.TabIndex = 0;
            // 
            // checkBox2
            // 
            this.checkBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Location = new System.Drawing.Point(47, 425);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(22, 21);
            this.checkBox2.TabIndex = 10;
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // cam_2
            // 
            this.cam_2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cam_2.Location = new System.Drawing.Point(122, 401);
            this.cam_2.Name = "cam_2";
            this.cam_2.Size = new System.Drawing.Size(335, 81);
            this.cam_2.TabIndex = 3;
            this.cam_2.Text = "相机2";
            this.cam_2.UseVisualStyleBackColor = true;
            this.cam_2.Click += new System.EventHandler(this.Cam_2_Click);
            // 
            // checkBox3
            // 
            this.checkBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox3.AutoSize = true;
            this.checkBox3.Checked = true;
            this.checkBox3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox3.Location = new System.Drawing.Point(47, 268);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(22, 21);
            this.checkBox3.TabIndex = 9;
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // cam_3
            // 
            this.cam_3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cam_3.Location = new System.Drawing.Point(122, 244);
            this.cam_3.Name = "cam_3";
            this.cam_3.Size = new System.Drawing.Size(335, 84);
            this.cam_3.TabIndex = 4;
            this.cam_3.Text = "相机3";
            this.cam_3.UseVisualStyleBackColor = true;
            this.cam_3.Click += new System.EventHandler(this.Cam_3_Click);
            // 
            // info_box
            // 
            this.info_box.Dock = System.Windows.Forms.DockStyle.Top;
            this.info_box.Location = new System.Drawing.Point(0, 0);
            this.info_box.Multiline = true;
            this.info_box.Name = "info_box";
            this.info_box.Size = new System.Drawing.Size(593, 289);
            this.info_box.TabIndex = 13;
            // 
            // checkBox0
            // 
            this.checkBox0.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox0.AutoSize = true;
            this.checkBox0.Checked = true;
            this.checkBox0.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox0.Location = new System.Drawing.Point(47, 736);
            this.checkBox0.Name = "checkBox0";
            this.checkBox0.Size = new System.Drawing.Size(22, 21);
            this.checkBox0.TabIndex = 12;
            this.checkBox0.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(47, 574);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(22, 21);
            this.checkBox1.TabIndex = 11;
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Checked = true;
            this.checkBox4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox4.Location = new System.Drawing.Point(47, 160);
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
            this.checkBox5.Location = new System.Drawing.Point(47, 51);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(22, 21);
            this.checkBox5.TabIndex = 7;
            this.checkBox5.UseVisualStyleBackColor = true;
            // 
            // cam_5
            // 
            this.cam_5.Location = new System.Drawing.Point(105, 27);
            this.cam_5.Name = "cam_5";
            this.cam_5.Size = new System.Drawing.Size(129, 66);
            this.cam_5.TabIndex = 6;
            this.cam_5.Text = "相机5";
            this.cam_5.UseVisualStyleBackColor = true;
            // 
            // cam_4
            // 
            this.cam_4.Location = new System.Drawing.Point(105, 136);
            this.cam_4.Name = "cam_4";
            this.cam_4.Size = new System.Drawing.Size(129, 67);
            this.cam_4.TabIndex = 5;
            this.cam_4.Text = "相机4";
            this.cam_4.UseVisualStyleBackColor = true;
            this.cam_4.Click += new System.EventHandler(this.Cam_4_Click);
            // 
            // cam_1
            // 
            this.cam_1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cam_1.Location = new System.Drawing.Point(122, 550);
            this.cam_1.Name = "cam_1";
            this.cam_1.Size = new System.Drawing.Size(335, 81);
            this.cam_1.TabIndex = 2;
            this.cam_1.Text = "相机1";
            this.cam_1.UseVisualStyleBackColor = true;
            this.cam_1.Click += new System.EventHandler(this.Cam_1_Click);
            // 
            // cam_0
            // 
            this.cam_0.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cam_0.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cam_0.Location = new System.Drawing.Point(122, 712);
            this.cam_0.Name = "cam_0";
            this.cam_0.Size = new System.Drawing.Size(335, 79);
            this.cam_0.TabIndex = 1;
            this.cam_0.Text = "相机0";
            this.cam_0.UseVisualStyleBackColor = true;
            this.cam_0.Click += new System.EventHandler(this.Cam_0_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(593, 1225);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // drawPictureBoxVideo
            // 
            this.drawPictureBoxVideo.AutoSize = true;
            this.drawPictureBoxVideo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.drawPictureBoxVideo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.drawPictureBoxVideo.Location = new System.Drawing.Point(0, 0);
            this.drawPictureBoxVideo.Margin = new System.Windows.Forms.Padding(4);
            this.drawPictureBoxVideo.Name = "drawPictureBoxVideo";
            this.drawPictureBoxVideo.Size = new System.Drawing.Size(525, 1225);
            this.drawPictureBoxVideo.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1122, 1428);
            this.Controls.Add(this.splitContainer1);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "车型相机标定程序v1.0";
            this.Load += new System.EventHandler(this.VideoForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
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
        private System.Windows.Forms.Button cam_2;
        private System.Windows.Forms.Button cam_1;
        private System.Windows.Forms.Button cam_0;
        private System.Windows.Forms.Button gridline;
        private System.Windows.Forms.ComboBox roadtype_box;
        private System.Windows.Forms.Button cam_4;
        private System.Windows.Forms.Button cam_3;
        private System.Windows.Forms.Button cam_5;
        private System.Windows.Forms.Button read_roadtype;
        private System.Windows.Forms.CheckBox checkBox0;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.TextBox cam_height;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button upload_json;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox cam_angel;
        private System.Windows.Forms.TextBox landwidth_box;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button caculate_rl;
        private System.Windows.Forms.TextBox info_box;
        private System.Windows.Forms.Button Concatenate_frames;
        private System.Windows.Forms.Button drawcombainframe;
        private System.Windows.Forms.CheckBox check_cover_mode;
        private System.Windows.Forms.CheckBox check_line_mode;
    }
}

