using System;
using Temboo.Core;
using System.IO.Ports;
using Temboo.Library.Google.Gmail;
using System.Text;

namespace temboo
{
    class Program
    {
        static void Main(string[] args)
        {
            int i = 0;
            SendEmailResultSet sendEmailResults;
            SerialPort comport, comport2, comport3;
            // Instantiate the Choreo, using a previously instantiated TembooSession object, eg:
            TembooSession session = new TembooSession("wqeasd", "myFirstApp", "lFo4hEJT4Jvv6migVdTfMuhMrzEIHDEz");
            SendEmail sendEmailChoreo = new SendEmail(session);
            int counter = 0;
            string line;

            ///////////////////讀黨/////////////////// 
            System.IO.StreamReader file =
                new System.IO.StreamReader(@"c:\test.txt", System.Text.Encoding.Default);
            while ((line = file.ReadLine()) != null)
            {
                System.Console.WriteLine(line);
            }
            file.Close();

            

            /////////////uart//////
            comport = new SerialPort("COM18", 9600, Parity.None, 8, StopBits.One);
            comport2 = new SerialPort("COM12", 9600, Parity.None, 8, StopBits.One);
            comport3 = new SerialPort("COM23", 115200, Parity.None, 8, StopBits.One);
            

            comport.Open();
            while (true)
            {
                byte[] b = new byte[10];
                char[] c = new char[10];
                comport.Read(b, 0, b.Length);
                c = Encoding.Unicode.GetChars(b);
                if (c[0] == 'a')
                {
                    Console.WriteLine("搜尋中.....");
                    while (true)
                    {
                        i++;
                        if (i == 1000000000)
                            break;
                    }
                    // Set inputs
                    sendEmailChoreo.setUsername("as21253679@gmail.com");
                    sendEmailChoreo.setSubject("您周遭有失蹤老人需要幫忙");
                    sendEmailChoreo.setToAddress("40343243@gm.nfu.edu.tw");
                    sendEmailChoreo.setMessageBody("地址:新北市 姓名:小名");
                    sendEmailChoreo.setPassword("nzefycgfbbxtbhqe");
                    sendEmailResults = sendEmailChoreo.execute();
                   

                    System.IO.StreamReader file2 =
                new System.IO.StreamReader(@"c:\test.txt", System.Text.Encoding.Default);
                    while ((line = file2.ReadLine()) != null)
                    {
                        counter++;
                        if(counter==1)
                            System.Console.WriteLine(line);
                    }
                    file2.Close();

                    comport3.Open();
                    byte[] send3 = new byte[10];
                    send3 = Encoding.ASCII.GetBytes("B");
                    comport3.Write(send3, 0, send3.Length);
                    comport3.Close();
                    break;
                }
                else if (c[0] == 'b')
                {
                    Console.WriteLine("搜尋中.....");
                    while (true)
                    {
                        i++;
                        if (i == 1000000000)
                            break;
                    }
                    // Set inputs
                    sendEmailChoreo.setUsername("as21253679@gmail.com");
                    sendEmailChoreo.setSubject("您周遭有失蹤老人需要幫忙");
                    sendEmailChoreo.setToAddress("40343243@gm.nfu.edu.tw");
                    sendEmailChoreo.setMessageBody("地址:台北市 姓名:小王");
                    sendEmailChoreo.setPassword("nzefycgfbbxtbhqe");
                    sendEmailResults = sendEmailChoreo.execute();
                   

                    System.IO.StreamReader file2 =
                new System.IO.StreamReader(@"c:\test.txt", System.Text.Encoding.Default);
                    while ((line = file2.ReadLine()) != null)
                    {
                        counter++;
                        if (counter == 2)
                            System.Console.WriteLine(line);
                    }
                    file2.Close();
                    comport3.Open();
                    byte[] send3 = new byte[10];
                    send3 = Encoding.ASCII.GetBytes("B");
                    comport3.Write(send3, 0, send3.Length);
                    comport3.Close();
                    break;
                }
                else if (c[0] == 'c')
                {
                    Console.WriteLine("搜尋中.....");
                    while (true)
                    {
                        i++;
                        if (i == 1000000000)
                            break;
                    }
                    // Set inputs
                    sendEmailChoreo.setUsername("as21253679@gmail.com");
                    sendEmailChoreo.setSubject("您周遭有失蹤老人需要幫忙");
                    sendEmailChoreo.setToAddress("40343243@gm.nfu.edu.tw");
                    sendEmailChoreo.setMessageBody("地址:雲林縣 姓名:小陳");
                    sendEmailChoreo.setPassword("nzefycgfbbxtbhqe");
                    sendEmailResults = sendEmailChoreo.execute();
                    

                    System.IO.StreamReader file2 =
                new System.IO.StreamReader(@"c:\test.txt", System.Text.Encoding.Default);
                    while ((line = file2.ReadLine()) != null)
                    {
                        counter++;
                        if (counter == 3)
                            System.Console.WriteLine(line);
                    }
                    file2.Close();
                    comport3.Open();
                    byte[] send3 = new byte[10];
                    send3 = Encoding.ASCII.GetBytes("B");
                    comport3.Write(send3, 0, send3.Length);
                    comport3.Close();
                    break;
                }
                counter = 0;
            }
            comport.Close();

            comport2.Open();
            byte[] send = new byte[10];
            send = Encoding.ASCII.GetBytes("b");
            comport2.Write(send, 0, send.Length);
            comport2.Close();

            comport.Open();
            byte[] send2 = new byte[10];
            send2 = Encoding.ASCII.GetBytes("z");
            comport.Write(send2, 0, send2.Length);
            comport.Close();

            // Print results
            //Console.WriteLine(sendEmailResults.Success);
            Console.ReadLine();
        }
    }
}