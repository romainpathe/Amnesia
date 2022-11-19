using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using Server.components;

namespace Server.classes
{
    public class Receiver
    {
        
        private static Thread _thread;

        public static void Init()
        {
            _thread = new Thread(Receive)
            {
                Name = "Receiver"
            };
            _thread.Start();
        }

        public static void Receive()
        {
            while (true)
            {
                foreach (var player in Program.GameManager.PlayerManager.Players)
                {
                    var buffer = new byte[2048];
                    var received = player.Socket.Receive(buffer, SocketFlags.None);
                    if (received == 0) continue;
                    var dataByte = new byte[received];
                    System.Buffer.BlockCopy(buffer, 0, dataByte, 0, received);
                    var data = System.Text.Encoding.ASCII.GetString(dataByte);
                    Debug.WriteLine(data);
                    
                }
            }
        }
        
        // private static void ReceiveResponse()
        // {
        //     var buffer = new byte[2048];
        //     int received = ClientSocket.Receive(buffer, SocketFlags.None);
        //     if (received == 0) return;
        //     var data = new byte[received];
        //     Array.Copy(buffer, data, received);
        //     string text = Encoding.ASCII.GetString(data);
        //     Console.WriteLine(text);
        // }
    }
}