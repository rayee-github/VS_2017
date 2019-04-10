using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.IO.Ports;

namespace 主機端_車_
{
    public partial class Form1 : Form
    {
        byte []RFID_ID = new byte[7];
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (port.BytesToRead > 0)
            {
                port.Read(RFID_ID, 0, 7);
                textBox1.Text = System.Text.Encoding.UTF8.GetString(RFID_ID);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GetSerialPort();
            port = new SerialPort(Convert.ToString(ports), 115200, Parity.None, 8, StopBits.One);
            //port = new SerialPort("COM17", 115200, Parity.None, 8, StopBits.One);
            port.Open();
            timer1.Start();
        }
    }
}
