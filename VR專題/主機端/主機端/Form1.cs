using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Management;

namespace 主機端
{
    public partial class Form1 : Form
    {
        string ports = "";
        private SerialPort port;
        public Form1()
        {
            InitializeComponent();
        }
        
        private void GetSerialPort()
        {
            try
            {
                ManagementObjectSearcher searcher =
                   new ManagementObjectSearcher("root\\CIMV2",
                   "SELECT * FROM Win32_PnPEntity");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    string s = string.Format("{0}", queryObj["Name"]);
                    Console.WriteLine(s);
                    string s1 = "";
                    if (s.Length >= 23)
                    {
                        s1 = s.Substring(0, s.Length - 8);
                        ports = s.Substring(18, 5);
                    }
                    //if (s == "Prolific USB-to-Serial Comm Port")
                    if (s1 == "USB-SERIAL CH340")
                    {
                        Console.WriteLine(ports);
                        break;
                    }
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }
        }

        private void send_command_Click(object sender, EventArgs e)
        {
            string command="";
            int count;
            byte[] a = new byte[9];
            a[0] = 0xAA;
            //textBox1.Text = command;
            for (int i = 1; i < 9; i++)
            {
                count = Int32.Parse(this.Controls.Find("shelf" + i, false)[0].Text);
                for (int j = 0; j < count; j++)
                {
                    a[i] += 1;
                }
            }
            port.Write(a,0,9);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GetSerialPort();
            port = new SerialPort(Convert.ToString(ports), 115200, Parity.None, 8, StopBits.One);
            port.Open();
        }
    }
}