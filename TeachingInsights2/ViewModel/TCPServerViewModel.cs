using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using System.Net;
using System.Net.Sockets;

namespace TeachingInsights2.ViewModel
{

    public class TCPServerViewModel : ViewModelBase
    {
        public static string GetIPAddress()
        {
            IPHostEntry host;
            string localIP = "?";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                }
            }
            return localIP;
        }
        Socket m_soc_lstn;
        public TCPServerViewModel()
        {
            //try
            //{
                //m_soc_lstn = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //IPEndPoint ipLocal = new IPEndPoint(IPAddress.Any, 80);
                //m_soc_lstn.Bind(ipLocal);
                //m_soc_lstn.Listen(4);
                //m_soc_lstn.BeginAccept(new AsyncCallback(OnClientConnect), null);

            //    Console.WriteLine(GetIPAddress());
            //    IPAddress ipAd = IPAddress.Parse("194.187.250.228");
            //    // use local m/c IP address, and 
            //    // use the same in the client

            //    /* Initializes the Listener */
            //    TcpListener myList = new TcpListener(IPAddress.Any, 80);

            //    /* Start Listeneting at the specified port */
            //    myList.Start();

            //    Console.WriteLine("The server is running at port 8001...");
            //    Console.WriteLine("The local End point is  :" +
            //                      myList.LocalEndpoint);
            //    Console.WriteLine("Waiting for a connection.....");

            //    Socket s = myList.AcceptSocket();
            //    Console.WriteLine("Connection accepted from " + s.RemoteEndPoint);

            //    byte[] b = new byte[100];
            //    int k = s.Receive(b);
            //    Console.WriteLine("Recieved...");
            //    for (int i = 0; i < k; i++)
            //        Console.Write(Convert.ToChar(b[i]));

            //    ASCIIEncoding asen = new ASCIIEncoding();
            //    s.Send(asen.GetBytes("The string was recieved by the server."));
            //    Console.WriteLine("\nSent Acknowledgement");
            //    /* clean up */
            //    s.Close();
            //    myList.Stop();

            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("Error..... " + e.StackTrace);
            //}    
            bool done = false;

            UdpClient listener = new UdpClient(80);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, 80);
            try
            {
                while (!done)
                {
                    Console.WriteLine("Waiting for broadcast");
                    byte[] bytes = listener.Receive(ref groupEP);
                    Console.WriteLine("Received broadcast from {0} :\n {1}\n", groupEP.ToString(), Encoding.ASCII.GetString(bytes, 0, bytes.Length));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                listener.Close();
            }
        }

    }
}
