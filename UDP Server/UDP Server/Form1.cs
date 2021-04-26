using System;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace UDP_Server
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

        Socket Server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        public void btnConnect_Click(object sender, EventArgs e)
        {
            int port = Convert.ToInt32(textBox1.Text);
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, port);
            Server.Bind(ipep);
            label2.Text = "已取得連線";
            Thread t = new Thread(new ThreadStart(ThreadProc));
            t.Start();
        }

        public delegate void MyInvoke(string input);

        private void ThreadProc()
        {
            while (true)
            {
                IPEndPoint send = new IPEndPoint(IPAddress.Any, 0);
                EndPoint Remote = (EndPoint)send;
                byte[] getdata = new byte[1024];
                int recv = Server.ReceiveFrom(getdata, ref Remote);
                string input = Encoding.UTF8.GetString(getdata, 0, recv);
                MyInvoke mi = new MyInvoke(UpdateForm);
                this.BeginInvoke(mi, new Object[] { input });
                Server.SendTo(Encoding.UTF8.GetBytes("OK"), Remote);
                Thread.Sleep(0);
            }
        }

        public void UpdateForm(string input)
        {
            listBox1.Items.Add("已接收: " + input + Environment.NewLine);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {

        }
    }
}
