using System;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace rdp_socket_client
{
    public partial class Form1 : Form
    {
        Socket m_socket;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            socket_process();
        }

        private void socket_process()
        {
            // Connect to a remote device.  
            try
            {
                // Create a TCP/IP  socket.  
                m_socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.  
                try
                {
                    m_socket.Connect("127.0.0.1",12345);

                    Console.WriteLine("Socket connected to {0}",
                        m_socket.RemoteEndPoint.ToString());

                    // Encode the data string into a byte array.  
                    byte[] msg = Encoding.ASCII.GetBytes("asd");

                    // Send the data through the socket.  
                    int bytesSent = m_socket.Send(msg);
                    
                    timer1.Enabled = true;
                    // Release the socket.  
                    //m_socket.Shutdown(SocketShutdown.Both);
                    //m_socket.Close();

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception : {0}", e.ToString());
            }
        }

        //Byte array to Bitmap
        public Bitmap BytesToBitmap(byte[] b)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream(b);
            try
            {
                Bitmap bmp = (Bitmap)Bitmap.FromStream(ms);
                return bmp;
            }
            catch (Exception) { }
            return null;
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            byte[] bytes = new byte[1024 * 1024];//1MB
            // Receive the response from the remote device.  
            int bytesRec = m_socket.Receive(bytes);
            if (bytesRec != 0)
            {
                //Console.WriteLine("Echoed test = {0}", Encoding.ASCII.GetString(bytes, 0, bytesRec));
                Bitmap image = BytesToBitmap(bytes);
                if(image!=null)
                    pictureBox1.Image = image;
            }
        }
    }
}
