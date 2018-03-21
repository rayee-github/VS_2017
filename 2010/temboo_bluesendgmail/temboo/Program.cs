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
            SendEmailResultSet sendEmailResults;
            SerialPort comport;
            byte[] b = new byte[10];
            char[] c = new char[10];
            // Instantiate the Choreo, using a previously instantiated TembooSession object, eg:
            TembooSession session = new TembooSession("wqeasd", "myFirstApp", "EkvbsPvapm3sNXyoAVoIV9uwfRHZY8wY");
            SendEmail sendEmailChoreo = new SendEmail(session);

            // Set inputs
            sendEmailChoreo.setUsername("as21253679@gmail.com");
            sendEmailChoreo.setSubject("123");
            sendEmailChoreo.setToAddress("40343245@gm.nfu.edu.tw");
            sendEmailChoreo.setMessageBody("123");
            sendEmailChoreo.setPassword("nzefycgfbbxtbhqe");
            sendEmailResults = sendEmailChoreo.execute();
            /////////////uart//////
            /*comport = new SerialPort("COM20", 9600, Parity.None, 8, StopBits.One);
            comport.Open();
            while (true)
            {
                comport.Read(b, 0, b.Length);
                c = Encoding.Unicode.GetChars(b);
                Console.WriteLine(c);
                if (c[0] == 'a')
                {
                    // Execute Choreo
                    sendEmailResults = sendEmailChoreo.execute();
                    break;
                }
            }
            comport.Close();*/
            
            // Print results
            Console.WriteLine(sendEmailResults.Success);
            Console.ReadLine();
        }
    }
}