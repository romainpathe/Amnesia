using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using Client.components;

namespace Client.classes
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
        
        public static void Send()
        {
            while (true)
            {
                var count = ObjForSend.Count;
                if (count <= 0) continue;
                var obj = ObjForSend.First();
                if (obj != null)
                {
                    Program.ClientSocket.Send(obj.data, 0, obj.data.Length, SocketFlags.None);
                }
                ObjForSend.RemoveAt(0);
            }
        }
        
        
    }
}