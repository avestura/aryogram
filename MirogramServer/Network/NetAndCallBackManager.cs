using MirogramServer.DataBase;
using MirogramServer.Network.RequestPackets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace MirogramServer.Network
{
    public class NetAndCallBackManager
    {

        // Signal the main thread to continue.
        public ManualResetEvent AllDone { get; set; } = new ManualResetEvent(false);

        public Dictionary<string, Socket> UserSockets = new Dictionary<string, Socket>();

        private ManualResetEvent recieveDone = new ManualResetEvent(false);

        public struct SocketBytesPair
        {
            public SocketBytesPair(Socket s, byte[] b)
            {
                Socket = s; Bytes = b;
            }
            public Socket Socket { get; set; }
            public byte[] Bytes { get; set; }
        }


        public void AcceptCallback(IAsyncResult ar)
        {
            AllDone.Set();

            // Get the socket that handles the client request.
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            string socketInfo = handler.LocalEndPoint.ToString();

            Console.WriteLine($">> Client {socketInfo} accepted...");

            Thread handleClient = new Thread(() =>
            {

                while (handler.IsSocketAlive())
                {
                    
                    byte[] bytesRecived = new byte[Packet.BufferSize];
                    handler.BeginReceive(bytesRecived, 0, Packet.BufferSize, 0,
                        new AsyncCallback(ReadCallback), new SocketBytesPair(handler, bytesRecived));
                    recieveDone.WaitOne();
                    Thread.Sleep(200);
                }


            })
            { Priority = ThreadPriority.AboveNormal };
            handleClient.Start();




        }

        public void ReadCallback(IAsyncResult ar)
        {
            recieveDone.Set();

            // Retrieve the state object and the handler socket
            // from the asynchronous state object.
            var socketAndBytes = (SocketBytesPair)ar.AsyncState;

            if (socketAndBytes.Bytes.Length > 0)
            {
                try
                {

                    Packet request = Packet.DeserilizedPacket(socketAndBytes.Bytes);
                    Socket handler = socketAndBytes.Socket;

                    Console.WriteLine($"Request recived with type of: {request.Type}");

                    // Read data from the client socket. 
                    int bytesRead = handler.EndReceive(ar);




                    //if (request.Type == Packet.PacketKinds.LoginRequest || )
                    //{
                        //var data = request as LoginRequestPacket;
                        //if (data != null && DataBaseActions.UserValidiation(data.UserName, data.Password))
                        //{

                        //}
                        //Console.WriteLine($">> Request from existing client {request.UserName}...");

                        if (!UserSockets.Keys.Contains(request.UserName))
                        {
                            UserSockets.Add(request.UserName, handler);
                            Console.WriteLine($">> Request from new client {request.UserName}...");
                        }
                        else
                        {
                            UserSockets[request.UserName] = handler;
                            Console.WriteLine($">> Socket for client {request.UserName} replaced...");

                        }

                    //}

                

                    ServerActions.HandlePacket(request);




                }
                catch (Exception e)
                {
#if LOG_FAIL_CALLBACK
                                 Console.WriteLine($">> Server failed ReadCallBack: {e.Message}...");
#endif
                }

            }


        }

        public void SendPacket(Socket handler, Packet packet)
        {
            byte[] byteData = packet.SerializedPacket();

            // Begin sending the data to the remote device.
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket handler = (Socket)ar.AsyncState;
                string socketInfo = handler.LocalEndPoint.ToString();


                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine($">> Sent {bytesSent} bytes to client with address {socketInfo}...");

                //handler.Shutdown(SocketShutdown.Both);
                //handler.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public Socket FindSocketByName(string username)
        {
            return UserSockets[username];
        }


    }
}
