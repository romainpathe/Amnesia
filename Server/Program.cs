using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Server.classes;
using Server.classes.Uno;
using Server.components.Uno;

namespace Server
{
    internal class Program
    {
        private static readonly Socket ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public const int BUFFER_SIZE = 2048;
        private const int PORT = 1000;
        public static readonly byte[] buffer = new byte[BUFFER_SIZE];
        public static readonly Random Random = new Random();
        public static int WindowsWidth = Console.WindowWidth;
        public static int WindowsHeight = Console.WindowHeight;
        public static GameManager GameManager = new GameManager();
        public static void Main(string[] args)
        {
            
            // StartServer();
            
            SetupServer();
            Sender.Init();
            Receiver.Init();
            // Writer.Init();
            // Console.CursorVisible = false;
            //
            // var gameManager = new GameManager();
            // gameManager.StartGame();
            // // ResizeWindow.Init(gameManager);
            //
            //
            //
            // Console.ReadKey();

        }
        private static void SetupServer()
        {
            Console.WriteLine("Setting up server...");
            ServerSocket.Bind(new IPEndPoint(IPAddress.Any, PORT));
            ServerSocket.Listen(0);
            ServerSocket.BeginAccept(AcceptCallback, null);
            Console.WriteLine("Server setup complete");
        }
        
        private static void AcceptCallback(IAsyncResult AR)
        {
            Socket socket;
            try
            {
                socket = ServerSocket.EndAccept(AR);
            }
            catch (ObjectDisposedException) // I cannot seem to avoid this (on exit when properly closing sockets)
            {
                return;
            }

            var player = new Player(socket);
            GameManager.PlayerManager.AddPlayer(player);
            // socket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, Receiver.ReceiveCallback, socket);
            // Sender.ObjForSend.Add(new Send(player, Encoding.ASCII.GetBytes(player.Id)));
            Console.WriteLine("Client connected, waiting for request...");
            if (GameManager.PlayerManager.Players.Count  < 1)
            {
                ServerSocket.BeginAccept(AcceptCallback, null);
            }
            else
            {
                Debug.WriteLine("Game started");
                GameManager.InitGame();
            }
        }
        
        // private static void ReceiveCallback(IAsyncResult AR)
        // {
        //     Socket current = (Socket)AR.AsyncState;
        //     int received;
        //
        //     try
        //     {
        //         received = current.EndReceive(AR);
        //     }
        //     catch (SocketException)
        //     {
        //         Console.WriteLine("Client forcefully disconnected");
        //         // Don't shutdown because the socket may be disposed and its disconnected anyway.
        //         current.Close();
        //         GameManager.PlayerManager.Players.Remove(GameManager.PlayerManager.Players.Find(x => x.Socket == current));
        //         return;
        //     }
        //
        //     var recBuf = new byte[received];
        //     Array.Copy(buffer, recBuf, received);
        //     var text = Encoding.ASCII.GetString(recBuf);
        //     Console.WriteLine("Received Text: " + text);
        //
        //     switch (text.ToLower())
        //     {
        //         // Client requested time
        //         case "get time":
        //         {
        //             Console.WriteLine("Text is a get time request");
        //             byte[] data = Encoding.ASCII.GetBytes(DateTime.Now.ToLongTimeString());
        //             current.Send(data);
        //             Console.WriteLine("Time sent to client");
        //             break;
        //         }
        //         // Client wants to exit gracefully
        //         case "exit":
        //             // Always Shutdown before closing
        //             current.Shutdown(SocketShutdown.Both);
        //             current.Close();
        //             GameManager.PlayerManager.Players.Remove(GameManager.PlayerManager.Players.Find(x => x.Socket == current));
        //             Console.WriteLine("Client disconnected");
        //             return;
        //         default:
        //         {
        //             Console.WriteLine("Text is an invalid request");
        //             byte[] data = Encoding.ASCII.GetBytes("Invalid request");
        //             current.Send(data);
        //             Console.WriteLine("Warning Sent");
        //             break;
        //         }
        //     }
        //
        //     current.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, current);
        // }
        
        
        
        
        // public static void StartServer()
        // {
        //     // Get Host IP Address that is used to establish a connection
        //     // In this case, we get one IP address of localhost that is IP : 127.0.0.1
        //     // If a host has multiple addresses, you will get a list of addresses
        //     IPHostEntry host = Dns.GetHostEntry("localhost");
        //     IPAddress ipAddress = host.AddressList[0];
        //     IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);
        //
        //     try {
        //
        //         // Create a Socket that will use Tcp protocol
        //         Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        //         // A Socket must be associated with an endpoint using the Bind method
        //         listener.Bind(localEndPoint);
        //         // Specify how many requests a Socket can listen before it gives Server busy response.
        //         // We will listen 10 requests at a time
        //         listener.Listen(10);
        //         
        //
        //
        //         Console.WriteLine("Waiting for a connection...");
        //         Socket handler = listener.Accept();
        //         
        //         
        //         Card card = new Card();
        //         var json = new Json(JsonType.Card, card);
        //         handler.Send(json.Send());
        //         // json.Send();
        //         
        //         
        //        // Incoming data from the client.
        //         string data = null;
        //         byte[] bytes = null;
        //
        //         while (true)
        //         {
        //             bytes = new byte[1024];
        //             int bytesRec = handler.Receive(bytes);
        //             data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
        //             if (data.IndexOf("<EOF>") > -1)
        //             {
        //                 break;
        //             }
        //         }
        //
        //         Console.WriteLine("Text received : {0}", data);
        //
        //         byte[] msg = Encoding.ASCII.GetBytes(data);
        //         // handler.Send(msg);
        //         // handler.Shutdown(SocketShutdown.Both);
        //         // handler.Close();
        //     }
        //     catch (Exception e)
        //     {
        //         Console.WriteLine(e.ToString());
        //     }
        //
        //     Console.WriteLine("\n Press any key to continue...");
        //     Console.ReadKey();
        // }
        
        
        
        
        
    }
}