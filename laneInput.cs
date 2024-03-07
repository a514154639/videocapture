using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace videocapture
{
    public partial class laneInput : Form
    {
        public laneInput()
        {
            InitializeComponent();
        }
        public delegate void SendMesg(string str);
        public delegate void SendStatus(bool status);
        public event SendMesg SendlaneNum;
        public event SendStatus SendisMainCam;

        private void button1_Click(object sender, EventArgs e)
        {
            SendlaneNum(this.textBox1.Text);
            SendisMainCam(this.checkBox1.Checked);
            DialogResult = DialogResult.OK;

        }
    }
}
