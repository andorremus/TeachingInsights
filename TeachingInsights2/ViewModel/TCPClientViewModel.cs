using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using System.Windows.Input;
using System.Net.Sockets;
using System.IO;
using System.Net;

namespace TeachingInsights2.ViewModel
{
    public class TCPClientViewModel : ViewModelBase
    {
        private ICommand StartCommunication;

        public TCPClientViewModel()
        {
        //    try
        //    {
        //        TcpClient tcpclnt = new TcpClient();
        //        Console.WriteLine("Connecting.....");

        //        tcpclnt.Connect("213.205.252.25", 8888);
        //        // use the ipaddress as in the server program

        //        Console.WriteLine("Connected");
        //        Console.Write("Enter the string to be transmitted : ");

        //        String str = Console.ReadLine();
        //        Stream stm = tcpclnt.GetStream();

        //        ASCIIEncoding asen = new ASCIIEncoding();
        //        byte[] ba = asen.GetBytes(str);
        //        Console.WriteLine("Transmitting.....");

        //        stm.Write(ba, 0, ba.Length);

        //        byte[] bb = new byte[100];
        //        int k = stm.Read(bb, 0, 100);

        //        for (int i = 0; i < k; i++)
        //            Console.Write(Convert.ToChar(bb[i]));

        //        tcpclnt.Close();
        //    }

        //    catch (Exception e)
        //    {
        //        Console.WriteLine("Error..... " + e.StackTrace);
        //    }

            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPAddress broadcast = IPAddress.Parse("127.0.0.1");
            byte[] sendbuf = Encoding.ASCII.GetBytes("ssss");
            IPEndPoint ep = new IPEndPoint(broadcast, 80);
            s.SendTo(sendbuf, ep);
           
        }

    }

}