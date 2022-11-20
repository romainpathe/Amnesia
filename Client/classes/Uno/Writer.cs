using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Client.interfaces;

namespace Client.classes.Uno
{
    public static class Writer
    {

        public static List<object> ObjForWrite = new List<object>();
        public static List<object> ObjForClear = new List<object>();
        private static Thread _thread;

        
        public static void Init()
        {
            _thread = new Thread(WriteLoop)
            {
                Name = "Writer"
            };
            _thread.Start();
        }


        public static void Write(object obj)
        {
            ObjForWrite.Add(obj);
        }
        
        public static void Clear(object obj)
        {
            ObjForClear.Add(obj);
        }

        private static void WriteLoop()
        {
            while (true)
            {
                Thread.Sleep(1);
                object obj;
                var count = ObjForClear.Count;
                if (count > 0)
                {
                    obj = ObjForClear.First();
                    ObjForClear.Remove(obj);
                    if (obj.GetType().GetInterface(typeof(IDrawable).ToString()) == null) continue;
                    ((IDrawable) obj).Clear(true);
                }
                else
                {
                    count = ObjForWrite.Count;
                    if (count <= 0) continue;
                    obj = ObjForWrite.First();
                    ObjForWrite.Remove(obj);
                    if (obj.GetType().GetInterface(typeof(IDrawable).ToString()) == null) continue;
                    ((IDrawable) obj).Clear(false);
                    ((IDrawable) obj).Draw();
                }
            }
        }
        
    }
}