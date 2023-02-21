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
    public partial class Ipinput : Form
    {
        public Ipinput()
        {
            InitializeComponent();
        }
        public delegate void SendMesg(string str);
        public event SendMesg send;

        private void button1_Click(object sender, EventArgs e)
        {
            send(this.textBox1.Text);
            DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
