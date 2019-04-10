using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace rdp_socket
{
    class m_socket
    {
        const int MAX_CLIENTS = 20; // 定義最大Clients 常數
        private Socket[] workerSocket = new Socket[MAX_CLIENTS];// 定義每個連線的工作類別域變數 Socket陣列
        private Socket mainSocket;
        private int Empty_channel_ID = -1;//宣告並定義空頻道的ID=-1，表示無空頻道資料
        public int port;

        public void socket_process()
        {
            mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint ipLocal = new IPEndPoint(IPAddress.Any, port);
            mainSocket.Bind(ipLocal);
            mainSocket.Listen(4);
            mainSocket.BeginAccept(new AsyncCallback(OnClientConnect), null);
        }
        public void OnClientConnect(IAsyncResult asyn)
        {
            try
            {
                //若主Socket為空則跳出
                if (mainSocket == null) return;

                Socket temp_Socket = mainSocket.EndAccept(asyn);
                //取得遠端節點的EndPoint
                EndPoint RemoteEP = temp_Socket.RemoteEndPoint;
                Empty_channel_ID = FindEmptyChannel();
                //將方才暫存的Socket交給空的 Socket接收
                workerSocket[Empty_channel_ID] = temp_Socket;
                //將暫存的Socket設為空
                temp_Socket = null;
                
                //WaitForData(workerSocket[Empty_channel_ID]);
            }
            catch (ObjectDisposedException) { Console.WriteLine("…處理已釋放記憶體的資源例外處理略..."); }
            catch (SocketException) { Console.WriteLine(" …因TCP Socket造成的例外處理略... "); }
            finally
            {
                //將方才關閉的主要Socket重新接收新的連線
                mainSocket.BeginAccept(new AsyncCallback(OnClientConnect), null);
            }
        }

        public void send(byte[] image)
        {
            byte[] image_byte = new byte[1024 * 1024];//1MB
            if (Empty_channel_ID > 0)
            {
                image_byte = image;
                workerSocket[Empty_channel_ID].Send(image_byte);
            }
        }

        // 尋找第一個空頻道的函式，傳回Channel ID或是-1 全被佔滿
        private int FindEmptyChannel()
        {
            for (int i = 0; i < MAX_CLIENTS; i++)
            {
                if (workerSocket[i] == null || !workerSocket[i].Connected)
                {
                    return i;
                }
            }
            return -1;
        }

        //宣告AsyncCallback類別的變數 pfnWorkerCallBack
        public AsyncCallback pfnWorkerCallBack;
        public void WaitForData(System.Net.Sockets.Socket soc)
        {
            try
            {
                //當pfnWorkerCallBack物件尚未實體化時，進行實體化
                if (pfnWorkerCallBack == null)
                {
                    pfnWorkerCallBack = new AsyncCallback(OnDataReceived);
                }
                //自行定義的型別 SocketPacket，附於此小節尾，內容只有一個Socket類和一個int。
                SocketPacket theSocPkt = new SocketPacket();
                //指定此一建立連線之Socket soc 給定義的 theSocPkt
                theSocPkt.m_currentSocket = soc;

                soc.BeginReceive(theSocPkt.dataBuffer, 0, theSocPkt.dataBuffer.Length, SocketFlags.None, pfnWorkerCallBack, theSocPkt);
            }
            catch (SocketException) { Console.WriteLine(" …例外處理略..."); }
        }

        //自型定義的物件封包的類別
        public class SocketPacket
        {
            //目前Activating之Socket
            public System.Net.Sockets.Socket m_currentSocket;
            public byte[] dataBuffer = new byte[1500]; //接受資料陣列
        }

        public void OnDataReceived(IAsyncResult asyn)
        {
            try
            {
                SocketPacket socketData = (SocketPacket)asyn.AsyncState;//取得接受的資料
                string msg = "";//宣告及定義訊息字串
                int iRx = 0;//宣告及定義訊息長度

                iRx = socketData.m_currentSocket.EndReceive(asyn);
                byte[] databuff = socketData.dataBuffer;
                msg = Encoding.ASCII.GetString(databuff);

                WaitForData(socketData.m_currentSocket);
            }
            catch (SocketException) { Console.WriteLine(" …例外處理略... "); }
        }
    }
}
