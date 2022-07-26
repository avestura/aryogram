using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using MirogramServer.Network;
using MirogramServer.DataBase;

namespace MirogramServer
{
    class Server
    {


        public static NetAndCallBackManager NetManager { get; set; } = new NetAndCallBackManager();


        static void Main(string[] args)
        {

            Console.WriteLine($"DNS is being initialized on hostname: {Dns.GetHostName()}...");
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress address = ipHost.AddressList[0];
            Console.WriteLine($"IP Address {address}/{address.AddressFamily} is being initialized...");
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12714);
            Console.WriteLine($"Server is being runned on port {localEndPoint.Port}...");
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                
                listener.Bind(localEndPoint);
                Console.WriteLine($"Listener Socket binded on {localEndPoint.Address}:{localEndPoint.Port}...");
                listener.Listen(100);
                Console.WriteLine($"Listener backlog limited to 100...");

                Console.WriteLine();

                while (true)
                {
                    
                    // Start an asynchronous socket to listen for connections.
                    Console.WriteLine(">> Waiting for a connection...");

                    // Set the event to nonsignaled state.
                    NetManager.AllDone.Reset();

                    

                    listener.BeginAccept(
                    new AsyncCallback(NetManager.AcceptCallback),
                    listener);

                    // Wait until a connection is made before continuing.
                    NetManager.AllDone.WaitOne();
                }


            }
            catch (Exception e) { Console.WriteLine(e.Message); }



            Console.ReadLine();
            
        }

        public static Socket FindSocketByName(string username)
        {
            return NetManager.FindSocketByName(username);
        }



    }
}
