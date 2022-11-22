using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Server.classes.Uno;
using Server.components;
using Server.components.Uno;
using Server.objects;

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
            var dataByte = new byte[received];
            Buffer.BlockCopy(Program.buffer, 0, dataByte, 0, received);
            // TODO: Remove for production (Display received data)
            var data = System.Text.Encoding.ASCII.GetString(dataByte);
            Debug.WriteLine("Received Data: "+data);
            
            var json = new Json();
            var x = json.Receive(dataByte);
            switch (x.Type)
            {
                case JsonType.Turn:
                    var a = JsonConvert.DeserializeObject<Turn>(JsonConvert.SerializeObject(x.obj));
                    var player = Program.GameManager.PlayerManager.Players.First(e => e.Socket == current);
                    if (a != null)
                    {
                        if(a.PickUp)
                            player.AddCardToHand(Program.GameManager.ShuffledDeck.Pop());
                        else
                        {
                            var card = player.Hand.First(e => e.Id == a.CurrentCard.Id);
                            player.Hand.Remove(card);
                            Program.GameManager.AddDiscardPile(card);
                        }
                    }
                    var w = new Turn
                    {
                        CanPlay = true,
                        Hand = player.Hand,
                        CurrentCard = Program.GameManager.CurrentCard,
                        DeckCard = Program.GameManager._deckCard
                    };
                    var y = new Json(JsonType.Turn, w).Send();
                    Sender.Add(new Send(player, y));
                    // Program.Test(player);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            

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