using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using Client.classes.Uno;
using Client.components.Uno;
using Client.objects;
using Newtonsoft.Json;

namespace Client.classes
{
    public class Receiver
    {
        
        private static Thread _thread;
        public static bool VInit = false;

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
            var init = false;
            while (true)
            {
                if (init) continue;
                
                init = true;
            }
            
            
            // while (true)
            // {
            //     var buffer = new byte[2048];
            //     var received = Program.ClientSocket.Receive(buffer, SocketFlags.None);
            //     Debug.WriteLine("Received: " + received);
            //     if (received == 0) continue;
            //     var dataByte = new byte[received];
            //     Buffer.BlockCopy(buffer, 0, dataByte, 0, received);
            //     // TODO: Remove for production (Display received data)
            //     var data = System.Text.Encoding.ASCII.GetString(dataByte);
            //     Debug.WriteLine("Received data: "+data);
            //     
            //     var json = new Json();
            //     var x = json.Receive(dataByte);
            //     switch (x.Type)
            //     {
            //         case JsonType.Init:
            //             var w = JsonConvert.DeserializeObject<GameInfo>(JsonConvert.SerializeObject(x.obj));
            //             if (w != null)
            //             {
            //                 Program.Id = w.UserId;
            //                 Program.LongestCard = w.LongestCard;
            //             }
            //             break;
            //         case JsonType.Hand:
            //             Player.Hand = JsonConvert.DeserializeObject<List<Card>>(JsonConvert.SerializeObject(x.obj));
            //             Player.DrawHand();
            //             break;
            //         case JsonType.Turn:
            //             var a = JsonConvert.DeserializeObject<Turn>(JsonConvert.SerializeObject(x.obj));
            //             if (a != null)
            //             {
            //                 //Todo: Not forget to remove this for production
            //                 Debug.WriteLine("Can play: " + a.CanPlay);
            //                 Player.Hand = a.Hand;
            //                 GameManager.CurrenCard = a.CurrentCard;
            //                 GameManager.DeckCard = a.DeckCard;
            //                 GameManager.CanPlay = a.CanPlay;
            //                 Player.DrawHand();
            //                 Writer.Write(GameManager.CurrenCard);
            //                 Writer.Write(GameManager.DeckCard);
            //                 if (a.CanPlay)
            //                 {
            //                     GameManager.DeckCard.IsSelected = true;
            //                     Writer.Write(GameManager.DeckCard);
            //                     GameManager.CurrentGame();
            //                 }
            //             }
            //             break;
            //         default:
            //             throw new ArgumentOutOfRangeException();
            //     }
            //     
            //     
            //     
            // }
        }


        public static void ReceiveCallback(IAsyncResult ar)
        {
            var current = (Socket)ar.AsyncState;
            int received;
            // Thread.Sleep(65);
            try
            {
                Debug.WriteLine("ReceiveCallback");
                received = current.EndReceive(ar);
            }
            catch (SocketException)
            {
                Debug.WriteLine("Client forcefully disconnected");
                // Don't shutdown because the socket may be disposed and its disconnected anyway.
                current.Close();
                // clientSockets.Remove(current);
                return;
            }
            finally
            {
                Debug.WriteLine("Finally");
            }

            try
            {
                var dataByte = new byte[received];
                Buffer.BlockCopy(Program.buffer, 0, dataByte, 0, received);
                // TODO: Remove for production (Display received data)
                var data = System.Text.Encoding.ASCII.GetString(dataByte);
                Debug.WriteLine("Received Data: " + data);
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
                    case JsonType.Hand:
                        Player.Hand = JsonConvert.DeserializeObject<List<Card>>(JsonConvert.SerializeObject(x.obj));
                        Player.DrawHand();
                        break;
                    case JsonType.Turn:
                        var a = JsonConvert.DeserializeObject<Turn>(JsonConvert.SerializeObject(x.obj));
                        if (a != null)
                        {
                            //Todo: Not forget to remove this for production
                            Debug.WriteLine("Can play: " + a.CanPlay);
                            Player.Hand = a.Hand;
                            GameManager.CurrentCard = a.CurrentCard;
                            GameManager.DeckCard = a.DeckCard;
                            GameManager.CanPlay = a.CanPlay;
                            Player.DrawHand();
                            Writer.Write(GameManager.CurrentCard);
                            Writer.Write(GameManager.DeckCard);
                            if (a.CanPlay)
                            {
                                GameManager.DeckCard.IsSelected = true;
                                Writer.Write(GameManager.DeckCard);
                                GameManager.CurrentGame();
                            }
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
            // current.BeginReceive(Program.buffer, 0, Program.BUFFER_SIZE, SocketFlags.None, ReceiveCallback,current);
            Program.ClientSocket.BeginReceive(Program.buffer, 0, Program.BUFFER_SIZE, SocketFlags.None, ReceiveCallback, Program.ClientSocket);
        }
        
        
        
    }
}