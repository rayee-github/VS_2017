using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 檔案加密
{
    public partial class Form1 : Form
    {
        [DllImport("Dll1.dll", EntryPoint = "decode", CallingConvention = CallingConvention.Cdecl)]
        static extern double decode(string filename, int parameter);
        [DllImport("Dll1.dll", EntryPoint = "encode", CallingConvention = CallingConvention.Cdecl)]
        static extern double encode(string filename, int parameter);

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)  //加密
        {
            string filename = label1.Text;
            if(filename == "")
                MessageBox.Show("請選擇檔案");
            else
                encode(filename, int.Parse(textBox1.Text));

            File.Move(filename, filename + ".abcde");
            label1.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)  //解密
        {
            string filename = label1.Text;
            if (filename == "")
                MessageBox.Show("請選擇檔案");
            else
            {
                if (Path.GetExtension(filename) == ".abcde")
                    decode(filename, int.Parse(textBox1.Text));
                else
                    MessageBox.Show("無法解密此檔案");
            }

            string rename= filename.Replace(".abcde", "");
            File.Move(filename, rename);
            label1.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Select file";
            dialog.InitialDirectory = ".\\";
            dialog.Filter = "files (*.*)|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                label1.Text=dialog.FileName;
            }
        }
    }
}
