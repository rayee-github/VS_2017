using System;
using System.Windows.Forms;
using System.IO;

namespace 同時讀檔_B
{
    public partial class Form1 : Form
    {
        string path = @"D:\M480BSP-master\M480BSP-master\M480BSP-master\SampleCode\StdDriver\t-win_EADC_HSUSBD_Mass_Storage_I2C_0904\Windows Tool\Mass_Storage_test\Debug\out.bin";
        bool flag = true;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            FileStream file_WR;
            BinaryReader Reader;
            byte[] buf = new byte[1000];

            file_WR = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            Reader = new BinaryReader(file_WR);
            buf = Reader.ReadBytes(1000);
            String mystring = BitConverter.ToString(buf);
        
            textBox1.Text = mystring + "\r\n";
            file_WR.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FileStream file_WR;
            BinaryWriter Writer;
            byte[] buf = new byte[2];

            if (flag)
            {
                buf[0] = 0xFF;
                buf[1] = 0xFF;
                flag = false;
            }
            else
            {
                buf[0] = 0x00;
                buf[1] = 0x00;
                flag = true;
            }
            file_WR = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            Writer = new BinaryWriter(file_WR);
            Writer.Write(buf, 0, 2);
            file_WR.Close();
        }
    }
}
