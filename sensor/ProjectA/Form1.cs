using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO.Ports;
using System.Net;

namespace ProjectA
{

      public partial class Form1 : Form
    {

        SerialPort com = null;

        string url = "http://119.23.225.92:8080/r/";
        //string url = "http://119.23.225.92:8080/v/";
        public Form1()
        {
            InitializeComponent();

            com = new SerialPort("COM3", 115200);

            com.DataReceived += com_DataReceived;


        }

        void com_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
           // throw new NotImplementedException();

            System.Threading.Thread.Sleep(30);

            //缓冲区有多少个字节
            int bytecount = com.BytesToRead;


            byte[] buffer = new byte[bytecount];

            com.Read(buffer, 0, bytecount);

            string cmd = ASCIIEncoding.ASCII.GetString(buffer);

            string[] subCmd = cmd.Split('\n');

            //string[] vals = subCmd[0].Split('\t');

            string loser = subCmd[0];

            string vals = loser.Substring(8);

            string value = vals.Replace("V", "").Trim();
            //上传到服务器
            string newurl = url + value;

            Console.WriteLine("value:" + value);
            Console.WriteLine("newurl" + newurl);
           
            //构建访问服务器的参数列表（名，值）对
            System.Collections.Specialized.NameValueCollection collection = new System.Collections.Specialized.NameValueCollection();

            HttpPostValuesAsync(newurl, collection,  null);

            //界面刷新显示传感器的值
            UpdateCmdReceive("upload complete！value=" + value + "                ");

        }

        public delegate void textbox_delegate(string msg);

        public void UpdateCmdReceive(string msg)
        {
            if (textBox1 == null) return;
            if (textBox1.InvokeRequired)
            {
                textbox_delegate dt = new textbox_delegate(UpdateCmdReceive);
                textBox1.Invoke(dt, new object[] { msg });
            }
            else
            {
                textBox1.Text += msg;
            }
        }

        public void HttpPostValuesAsync(string url, System.Collections.Specialized.NameValueCollection collection,
        Encoding encode = null)
        {
            var webClient = new WebClient { Encoding = Encoding.UTF8 };

            if (encode != null)
                webClient.Encoding = encode;


            webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");


      
            byte[] bResponse = webClient.DownloadData(new Uri(url));
            Console.WriteLine(bResponse);

        }



        private void button1_Click(object sender, EventArgs e)
        {

            com.Open();
            this.Text = "程序启动中";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            com.Close();
            this.Text = "程序已停止";
        }

         
    }
}
