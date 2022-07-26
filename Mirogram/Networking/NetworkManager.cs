using MirogramServer.Network;
using MirogramServer.Network.RequestPackets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using static MirogramServer.Network.NetAndCallBackManager;

namespace Mirogram.Networking
{
    public class NetworkManager
    {

        public Socket ServerSocket { get; set; }
        public Socket MySocket { get; set; }

        public bool LastConnectCallbackStatus { get; set; }

        // ManualResetEvent instances signal completion.
        public ManualResetEvent ConnectDone { get; set; } = new ManualResetEvent(false);
        public ManualResetEvent SendDone { get; set; } = new ManualResetEvent(false);
        public ManualResetEvent ReceiveDone { get; set; } = new ManualResetEvent(false);

        private IPHostEntry ipHostInfo;
        private IPAddress ipAddress;
        private IPEndPoint remoteEP;

        public NetworkManager()
        {

            ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];
            remoteEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12714);

            MySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        }

        public void Connect()
        {
            // Connect to the remote endpoint.
            MySocket.BeginConnect(remoteEP,
                new AsyncCallback(ConnectCallback), MySocket);
  
            ConnectDone.WaitOne();
            
        }

        public void Disconnect()
        {
            if (MySocket.Connected)
            {
                MySocket.Shutdown(SocketShutdown.Both);
                MySocket.Disconnect(false);
                MySocket.Close();
                MySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            }

        }

        public void RealeaseAllThreadManulaWaits()
        {
            SendDone.Set();
            ReceiveDone.Set();
            ConnectDone.Set();
        }

        public void StartListeningToServer()
        {
            // Read data from server all the time
            new Thread(() =>
            {
                while (MySocket.Connected)
                {

                    
                    // Receive the response from the remote device.
                    Receive(MySocket);
                    ReceiveDone.WaitOne();

                    Thread.Sleep(500);

                }
            })
            { Priority = ThreadPriority.AboveNormal }.Start();
        }

        private void Receive(Socket client)
        {
            try
            {

                byte[] bytes = new byte[Packet.BufferSize];
                // Begin receiving the data from the remote device.

                client.BeginReceive(bytes, 0, Packet.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), new SocketBytesPair(client, bytes));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                var socketAndBytes = (SocketBytesPair)ar.AsyncState;
                byte[] bytesRecived = socketAndBytes.Bytes;
                ReceiveDone.Set();

                if (bytesRecived.Length > 0)
                {

                    Packet state = Packet.DeserilizedPacket(bytesRecived);
                    Socket client = socketAndBytes.Socket;

                    // Read data from the remote device.
                    int bytesRead = client.EndReceive(ar);

                    App.Current.Dispatcher.Invoke(() => {
                        NetworkActions.HandleRecivedPacket(state);
                    });

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }


        private void Send(Socket client, Packet data)
        {
            // Convert the string data to byte data using ASCII encoding.
            byte[] byteData = data.SerializedPacket();

            // Begin sending the data to the remote device.
            client.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), client);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = client.EndSend(ar);
                Console.WriteLine(">> Sent {0} bytes to server.", bytesSent);

                // Signal that all bytes have been sent.
                SendDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket client = (Socket)ar.AsyncState;

                // Complete the connection.
                client.EndConnect(ar);

                Console.WriteLine(">> Socket connected to {0}",
                    client.RemoteEndPoint.ToString());

                LastConnectCallbackStatus = true;
                // Signal that the connection has been made.
                ConnectDone.Set();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); LastConnectCallbackStatus = false;  ConnectDone.Set(); }
          
        }

        


    }
}
