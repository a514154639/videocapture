using System;
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
        public event SendMesg sendip;
        public event SendMesg sendpassward;

        private void button1_Click(object sender, EventArgs e)
        {
            sendip(this.textBox1.Text);
            sendpassward(this.textBox2.Text);
            DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

    }
}
