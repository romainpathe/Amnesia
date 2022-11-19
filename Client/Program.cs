using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Client.classes;
using Client.components.Uno;
using Newtonsoft.Json;

namespace Client
{
    internal class Program
    {
        private static readonly Socket ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private const int PORT = 100;
        
        public static readonly Random Random = new Random();
        public static int WindowsWidth = Console.WindowWidth;
        public static int WindowsHeight = Console.WindowHeight;
        public static void Main(string[] args)
        {
            // StartClient();
            ConnectToServer();
        }
        
        private static void ConnectToServer()
        {
            int attempts = 0;

            while (!ClientSocket.Connected)
            {
                try
                {
                    attempts++;
                    Console.WriteLine("Connection attempt " + attempts);
                    // Change IPAddress.Loopback to a remote IP to connect to a remote host.
                    ClientSocket.Connect(IPAddress.Loopback, PORT);
                }
                catch (SocketException) 
                {
                    Console.Clear();
                }
            }

            Console.Clear();
            Console.WriteLine("Connected");
        }
        
        
        
        
        
    //     public static void StartClient()
    // {
    //     byte[] bytes = new byte[1024];
    //
    //     try
    //     {
    //         // Connect to a Remote server
    //         // Get Host IP Address that is used to establish a connection
    //         // In this case, we get one IP address of localhost that is IP : 127.0.0.1
    //         // If a host has multiple addresses, you will get a list of addresses
    //         IPHostEntry host = Dns.GetHostEntry("localhost");
    //         IPAddress ipAddress = host.AddressList[0];
    //         IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);
    //
    //         // Create a TCP/IP  socket.
    //         Socket sender = new Socket(ipAddress.AddressFamily,
    //             SocketType.Stream, ProtocolType.Tcp);
    //
    //         // Connect the socket to the remote endpoint. Catch any errors.
    //         try
    //         {
    //             // Connect to Remote EndPoint
    //             sender.Connect(remoteEP);
    //
    //             Console.WriteLine("Socket connected to {0}",
    //                 sender.RemoteEndPoint.ToString());
    //
    //             // Encode the data string into a byte array.
    //             byte[] msg = Encoding.ASCII.GetBytes("This is a test<EOF>");
    //
    //             // Send the data through the socket.
    //             int bytesSent = sender.Send(msg);
    //
    //             
    //             // Receive the response from the remote device.
    //             int bytesRec = sender.Receive(bytes);
    //             var json = new Json();
    //             var x = json.Receive(bytes);
    //             Card card = JsonConvert.DeserializeObject<Card>(JsonConvert.SerializeObject(x.obj));
    //             switch (x.Type)
    //             {
    //                 case JsonType.Card:
    //                     Console.WriteLine("Card");
    //                     break;
    //                 case JsonType.Game:
    //                     Console.WriteLine("Game");
    //                     break;
    //                 case JsonType.Player:
    //                     Console.WriteLine("Player");
    //                     break;
    //                 default:
    //                     throw new ArgumentOutOfRangeException();
    //             }
    //             // Debug.WriteLine(x);
    //             // var data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
    //             //
    //             // Card deserializedProduct = JsonConvert.DeserializeObject<Card>(data);
    //             // deserializedProduct.Draw();
    //             // Debug.WriteLine("Echoed test = "+deserializedProduct);
    //
    //             // Release the socket.
    //             // sender.Shutdown(SocketShutdown.Both);
    //             // sender.Close();
    //             Console.ReadKey();
    //
    //         }
    //         catch (ArgumentNullException ane)
    //         {
    //             Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
    //         }
    //         catch (SocketException se)
    //         {
    //             Console.WriteLine("SocketException : {0}", se.ToString());
    //         }
    //         catch (Exception e)
    //         {
    //             Console.WriteLine("Unexpected exception : {0}", e.ToString());
    //         }
    //
    //     }
    //     catch (Exception e)
    //     {
    //         Console.WriteLine(e.ToString());
    //     }
    // }
        
    }
}