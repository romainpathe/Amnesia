﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using Server.components;

namespace Server.classes
{
    public static class Sender
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

        public static void Add(Send send)
        {
            ObjForSend.Add(send);
        }
        
        private static void Send()
        {
            while (true)
            {
                // Sleep because data send in the same time, and client can't read it
                // Todo: If Client can't read data, increase sleep time
                Thread.Sleep(65);
                if (ObjForSend.Count <= 0) continue;
                var obj = ObjForSend[0];
                try
                {
                    obj?.player.Socket.Send(obj.data);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
                ObjForSend.RemoveAt(0);
            }
            // ReSharper disable once FunctionNeverReturns
        }
        
        
    }
}