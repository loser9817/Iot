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
using Newtonsoft.Json;


namespace ProjectC
{
    public partial class Form1 : Form
    {

        SerialPort com = null;

        string url = "http://119.23.225.92:8080/new";

        byte[] led_on = { 0x01,0x99,0x10,0x10,0x99};
        byte[] led_off = { 0x01, 0x99, 0x11, 0x11, 0x99 };

        public Form1()
        {
            InitializeComponent();

            com = new SerialPort("COM4", 9600);

            com.DataReceived += com_DataReceived;

            com.Open();
        }

        void com_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // throw new NotImplementedException();

            System.Threading.Thread.Sleep(30);

            //缓冲区有多少个字节
            int bytecount = com.BytesToRead;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //调用服务器（云）接口
            //UploadValuesCompletedEventHandler handler = new UploadValuesCompletedEventHandler(AckHandler3);
            //构建访问服务器的参数列表（名，值）对
            System.Collections.Specialized.NameValueCollection collection = new System.Collections.Specialized.NameValueCollection();

            HttpPostValuesAsync(url, collection,  null);

        }

        //mark
        public void HttpPostValuesAsync(string url, System.Collections.Specialized.NameValueCollection collection,
            Encoding encode = null)
        {
            var webClient = new WebClient { Encoding = Encoding.UTF8 };

            if (encode != null)
                webClient.Encoding = encode;


            webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");


            //if (callBackUploadDataCompleted != null)
            //    webClient.UploadValuesCompleted += callBackUploadDataCompleted;

            //webClient.UploadValuesAsync(new Uri(url), "GET", collection);
            byte[] b = webClient.DownloadData(new Uri(url));

            String serverAck = UTF8Encoding.UTF8.GetString(b);

            Console.WriteLine(serverAck);

            try
            {
                string[] left=serverAck.Split('[');// 一定是单引 
                string[] last = left[left.Length - 1].Split(']');// 一定是单引 
                Console.WriteLine("left" + left[left.Length - 1]);
                Console.WriteLine("last" + last[0]);
                DataReulst result = JsonConvert.DeserializeObject<DataReulst>(last[0]);

                //把服务器接口返回来的传感器的字符串sensorvalue转换为数值
                double data = double.Parse(result.value);

                if (data > 1)
                {
                    UpdateCmdReceive(result.value + "亮灯");
                    com.Write(led_on, 0, led_on.Length);
                }
                else
                {
                    UpdateCmdReceive(result.value + "灭灯");
                    com.Write(led_off, 0, led_on.Length);
                }
            }catch(Exception e){}
        }

        //public void AckHandler3(object sender, UploadValuesCompletedEventArgs e)
        //{


        //    if (e != null)
        //    {
        //        try
        //        {
        //            String serverAck = UTF8Encoding.UTF8.GetString(e.Result);

        //             DataReulst result=JsonConvert.DeserializeObject<DataReulst>(serverAck);

        //             //把服务器接口返回来的传感器的字符串sensorvalue转换为数值
        //             double data = double.Parse(result.sensorvalue);

        //             if (data > 1)
        //             {
        //                 UpdateCmdReceive(result.sensorvalue+"亮灯");
        //                 com.Write(led_on, 0, led_on.Length);
        //             }
        //             else
        //             {
        //                 UpdateCmdReceive(result.sensorvalue + "灭灯");
        //                 com.Write(led_off, 0, led_on.Length);
        //             }
        //        }catch(Exception ex)
        //        {
                    
        //        }
        //    }



        //}


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
                textBox1.Text = msg;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //启动定时器
            timer1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }

    }
}
