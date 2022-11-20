using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Sockets;
using System.Text;
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

        [SuppressMessage("ReSharper.DPA", "DPA0001: Memory allocation issues")]
        public static void Receive()
        {
            while (true)
            {
                var players = Program.GameManager.PlayerManager.Players.ToList().Where(player => !player.ReceiverEnabled);
                foreach (var player in players)
                {
                    player.Socket.BeginReceive(Program.buffer, 0, Program.BUFFER_SIZE, SocketFlags.None, ReceiveCallback,player.Socket);
                    player.ReceiverEnabled = true;
                }
            }
        }
        
        
        public static void ReceiveCallback(IAsyncResult ar)
        {
            Socket current = (Socket)ar.AsyncState;
            int received;
            try
            {
                received = current.EndReceive(ar);
            }
            catch (SocketException)
            {
                Console.WriteLine("Client forcefully disconnected");
                // Don't shutdown because the socket may be disposed and its disconnected anyway.
                current.Close(); 
                Program.GameManager.PlayerManager.RemovePlayerBySocket(current);
                // clientSockets.Remove(current);
                return;
            }
            byte[] recBuf = new byte[received];
            Array.Copy(Program.buffer, recBuf, received);
            string text = Encoding.ASCII.GetString(recBuf);
            Debug.WriteLine("Received Text: " + text);
            
            

            current.BeginReceive(Program.buffer, 0, Program.BUFFER_SIZE, SocketFlags.None, ReceiveCallback, current);
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