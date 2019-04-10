using System;
using System.Drawing;
using System.Windows.Forms;

namespace rdp_socket
{
    public partial class Form1 : Form
    {
        m_socket mainSocket=new m_socket { port = 12345 };

        public Form1()
        {
            InitializeComponent();
        }

        private byte[] screen_shot()
        {
            Bitmap myImage = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics g = Graphics.FromImage(myImage);
            g.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height));
            IntPtr dc1 = g.GetHdc();
            g.ReleaseHdc(dc1);
            //myImage.Save(@"screen_shot/screen" + count + ".jpg");

            int width = Convert.ToInt32(myImage.Width);
            int height = Convert.ToInt32(myImage.Height);
            Bitmap resizeImage=resize(myImage, width, height, (int)(width *0.1), (int)(height*0.1));
            //pictureBox1.Image = resizeImage;

            return BmpToBytes(resizeImage);
        }

        public byte[] BmpToBytes(Bitmap bmp)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            byte[] b = ms.GetBuffer();
            return b;
        }

        private static Bitmap resize(Bitmap originImage, int oriwidth, int oriheight, int width, int height)
        {
            Bitmap resizedbitmap = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(resizedbitmap);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.Clear(Color.Transparent);
            g.DrawImage(originImage, new Rectangle(0, 0, width, height), new Rectangle(0, 0, oriwidth, oriheight), GraphicsUnit.Pixel);
            return resizedbitmap;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mainSocket.socket_process();
            //while(true)
            //mainSocket.send(screen_shot());
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            mainSocket.send(screen_shot());
        }
    }
}
