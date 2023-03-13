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
    public partial class ConnectDialog : Form
    {
        // 定义IP地址和密码的属性
        public string IpAddress { get; private set; }
        public string Password { get; private set; }
        public string Username { get; private set; }

        private string savedIpAddress;
        private string savedPassword;
        private string savedUsername;

        public ConnectDialog()
        {
            InitializeComponent();
        }

        private void ConnectDialog_Load(object sender, EventArgs e)
        {
            savedIpAddress = Properties.Settings.Default.LastIpAddress;
            savedPassword = Properties.Settings.Default.LastPassword;
            savedUsername = Properties.Settings.Default.LastUsername;
            textBox1.Text = savedUsername;
            textBox2.Text = savedIpAddress;
            textBox3.Text = savedPassword;
        }

        private void ConnectDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.LastIpAddress = savedIpAddress;
            Properties.Settings.Default.LastPassword = savedPassword;
            Properties.Settings.Default.LastUsername = savedUsername;
            Properties.Settings.Default.Save();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            IpAddress = textBox2.Text;
            Password = textBox3.Text;
            Username = textBox1.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            savedUsername = textBox1.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            savedIpAddress = textBox2.Text;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            savedPassword = textBox3.Text;
        }
    }
}
