using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using Server.components;

namespace Server.classes
{
    public class Sender
    {
        
        private static Thread _thread;
        public static readonly List<Send> ObjForSend = new List<Send>();

        public static void Init()
        {
            _thread = new Thread(Send)
            {
                Name = "Sender"
            };
            _thread.Start();
        }
        
        
        // private static void SendString(string text)
        // {
        //     byte[] buffer = Encoding.ASCII.GetBytes(text);
        //     ClientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
        // }
        public static void Send()
        {
            while (true)
            {
                if (ObjForSend.Count <= 0) continue;
                var obj = ObjForSend[0];
                obj.player.Socket.Send(obj.data, 0, obj.data.Length, SocketFlags.None);
                ObjForSend.RemoveAt(0);
            }
        }
        
        
    }
}