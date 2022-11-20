using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using Client.classes.Uno;
using Client.components.Uno;
using Newtonsoft.Json;

namespace Client.classes
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
                var buffer = new byte[2048];
                var received = Program.ClientSocket.Receive(buffer, SocketFlags.None);
                if (received == 0) continue;
                var dataByte = new byte[received];
                Buffer.BlockCopy(buffer, 0, dataByte, 0, received);
                // TODO: Remove for production (Display received data)
                var data = System.Text.Encoding.ASCII.GetString(dataByte);
                Debug.WriteLine(data);
                
                var json = new Json();
                var x = json.Receive(dataByte);
                switch (x.Type)
                {
                    case JsonType.Init:
                        var w = JsonConvert.DeserializeObject<GameInfo>(JsonConvert.SerializeObject(x.obj));
                        if (w != null)
                        {
                            Program.Id = w.UserId;
                            Program.LongestCard = w.LongestCard;
                        }
                        break;
                    case JsonType.CurrentCard:
                        GameManager.CurrenCard = JsonConvert.DeserializeObject<Card>(JsonConvert.SerializeObject(x.obj));
                        if (GameManager.CurrenCard != null) Writer.Write(GameManager.CurrenCard);
                        break;
                    case JsonType.Deck:
                        GameManager.DeckCard = JsonConvert.DeserializeObject<Card>(JsonConvert.SerializeObject(x.obj));
                        if (GameManager.DeckCard != null) Writer.Write(GameManager.DeckCard);
                        break;
                    case JsonType.Hand:
                        Player.Hand = JsonConvert.DeserializeObject<List<Card>>(JsonConvert.SerializeObject(x.obj));
                        Player.DrawHand();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                
                
            }
        }
        
    }
}