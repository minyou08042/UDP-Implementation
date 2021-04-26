using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public void Form1_Load(object sender, EventArgs e)
        {
        }

        public void btnConnect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIp.Text) == true)
            {
                MessageBox.Show("請輸入IP");
            }
            if (string.IsNullOrEmpty(txtPort.Text) == true)
            {
                MessageBox.Show("請輸入PORT");
            }
            label3.Text = "已取得連線";

            Thread t = new Thread(new ThreadStart(ThreadProc));
            t.Start();
        }

        public delegate void MyInvoke(byte[] data, int recv);

        public void ThreadProc()
        {
            byte[] data = new byte[4096];
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            EndPoint Remote = (EndPoint)sender;
            Client.Bind(Remote);
            while (true)
            {
                int recv = Client.ReceiveFrom(data, ref Remote);
                MyInvoke mi = new MyInvoke(UpdateForm);
                this.BeginInvoke(mi, new Object[] { data, recv });
                Thread.Sleep(0);
            }
        }

        public void UpdateForm(byte[] data, int recv)
        {
            listBox1.Items.Add("已接收： " + Encoding.UTF8.GetString(data, 0, recv) + Environment.NewLine);
        }

        Socket Client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        public void btnSend_Click(object sender, EventArgs e)
        {
            string input = txtMsgSend.Text;
            string ip = txtIp.Text;
            if (string.IsNullOrEmpty(ip) == true)
            {
                MessageBox.Show("請輸入IP");
            }
            IPEndPoint remoteIP = new IPEndPoint(IPAddress.Parse(ip), Convert.ToInt32(txtPort.Text));
            Client.SendTo(Encoding.UTF8.GetBytes(input), remoteIP);
            listBox1.Items.Add("已傳送： " + Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(input), 0, input.Length) + Environment.NewLine);
            txtMsgSend.Text = "";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Client.Close();
            label3.Text = "已關閉連線";
        }

        public void btnExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

    }
}