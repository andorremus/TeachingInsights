﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using System.Net;
using System.Net.Sockets;
using System.Windows.Input;
using System.Threading;
using System.ComponentModel;

namespace TeachingInsights2.ViewModel
{

    public class TCPServerViewModel : ViewModelBase
    {
        private ICommand _StartConnectionCommand;
        private ICommand _StopConnectionCommand;
        private const int portNumber = 4545;
        private static String response = String.Empty;
        private readonly BackgroundWorker worker = new BackgroundWorker();

        public ICommand StartConnectionCommand
        {
            get
            {
                if (_StartConnectionCommand == null)
                    _StartConnectionCommand = new RelayCommand(param => this.StartConnection(), null);
                return _StartConnectionCommand; ;
            }
        }

        private void StartConnection()
        {
            worker.RunWorkerAsync();
        }

        private void StopConnection()
        {
            worker.CancelAsync();
        }

        public ICommand StopConnectionCommand
        {
            get
            {
                if (_StopConnectionCommand == null)
                    _StopConnectionCommand = new RelayCommand(param => this.StopConnection(), null);
                return _StopConnectionCommand; ;
            }
        }

        //public static string GetIPAddress()
        //{
        //    IPHostEntry host;
        //    string localIP = "?";
        //    host = Dns.GetHostEntry(Dns.GetHostName());
        //    foreach (IPAddress ip in host.AddressList)
        //    {
        //        if (ip.AddressFamily == AddressFamily.InterNetwork)
        //        {
        //            localIP = ip.ToString();
        //        }
        //    }
        //    return localIP;
        //}
        //Socket m_soc_lstn;
        //public TCPServerViewModel()
        //{
        //    try
        //    {
        //        //m_soc_lstn = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //        //IPEndPoint ipLocal = new IPEndPoint(IPAddress.Any, 80);
        //        //m_soc_lstn.Bind(ipLocal);
        //        //m_soc_lstn.Listen(4);
        //        //m_soc_lstn.BeginAccept(new AsyncCallback(OnClientConnect), null);

        //        Console.WriteLine(GetIPAddress());
        //        IPAddress ipAd = IPAddress.Parse("194.187.250.228");
        //        // use local m/c IP address, and 
        //        // use the same in the client

        //        /* Initializes the Listener */
        //        TcpListener myList = new TcpListener(IPAddress.Any, 80);

        //        /* Start Listeneting at the specified port */
        //        myList.Start();

        //        Console.WriteLine("The server is running at port 8001...");
        //        Console.WriteLine("The local End point is  :" +
        //                          myList.LocalEndpoint);
        //        Console.WriteLine("Waiting for a connection.....");

        //        Socket s = myList.AcceptSocket();
        //        Console.WriteLine("Connection accepted from " + s.RemoteEndPoint);

        //        byte[] b = new byte[100];
        //        int k = s.Receive(b);
        //        Console.WriteLine("Recieved...");
        //        for (int i = 0; i < k; i++)
        //            Console.Write(Convert.ToChar(b[i]));

        //        ASCIIEncoding asen = new ASCIIEncoding();
        //        s.Send(asen.GetBytes("The string was recieved by the server."));
        //        Console.WriteLine("\nSent Acknowledgement");
        //        /* clean up */
        //        s.Close();
        //        myList.Stop();

        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("Error..... " + e.StackTrace);
        //    }
        //}


        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public TCPServerViewModel()
        {
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.WorkerSupportsCancellation = true;
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            StartListening();
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }
        public static void StartListening()
        {
            // Data buffer for incoming data.
            byte[] bytes = new Byte[1024];

            // Establish the local endpoint for the socket.
            // The DNS name of the computer
            // running the listener is "host.contoso.com".
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            // Create a TCP/IP socket.
            Socket listener = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    // Set the event to nonsignaled state.
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.
                    Console.WriteLine("Waiting for a connection...");
                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);

                    // Wait until a connection is made before continuing.
                    allDone.WaitOne();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();

        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.
            allDone.Set();

            // Get the socket that handles the client request.
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create the state object.
            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket
            // from the asynchronous state object.
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            // Read data from the client socket. 
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far.
                state.sb.Append(Encoding.ASCII.GetString(
                    state.buffer, 0, bytesRead));

                // Check for end-of-file tag. If it is not there, read 
                // more data.
                content = state.sb.ToString();
                if (content.IndexOf("<EOF>") > -1)
                {
                    // All the data has been read from the 
                    // client. Display it on the console.
                    Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                        content.Length, content);
                    // Echo the data back to the client.
                    Send(handler, content);
                }
                else
                {
                    // Not all data received. Get more.
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
                }
            }
        }

        private static void Send(Socket handler, String data)
        {
            // Convert the string data to byte data using ASCII encoding.
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
     // State object for reading client data asynchronously
    public class StateObject
    {
        // Client  socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 1024;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }
}
