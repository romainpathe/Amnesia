using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using Newtonsoft.Json;
using Server.classes;
using Server.classes.Uno;

namespace Server.components.Uno
{
    [Serializable()]
    public class Player
    {
        public string Name { get; }
        [JsonIgnore]
        public Socket Socket { get; }
        public string Id { get; } = Guid.NewGuid().ToString();
        [JsonIgnore]
        public bool ReceiverEnabled { get; set; }
        [JsonIgnore]
        public List<Card> Hand { get; }
        [JsonIgnore]
        public static List<Card> OldHand { get; set; }
        [JsonIgnore]
        public int WindowsHeight { get; set; }
        [JsonIgnore]
        public bool SelectedDeck { get; set; }
        [JsonIgnore]
        public int LastCardSelected { get; set; }
        [JsonIgnore]
        public bool ColorSelector { get; set; }

        public Player(Socket socket)
        {
            Socket = socket;
            SelectedDeck = true;
            // Console.WriteLine("Enter your name: ");
            // Name = Console.ReadLine();
            Hand = new List<Card>();
        }

        public void SendHand()
        {
            var x = new Json(JsonType.Hand, Hand).Send();
            Sender.Add(new Send(this, x));
        }
        
        
        
        
        public void AddCardToHand(Card card)
        {
            var y = (Console.WindowHeight / 3) - 2;
            // Debug.WriteLine(WindowsHeight);
            if (WindowsHeight != Console.WindowHeight)
            {
                WindowsHeight = Console.WindowHeight;
                foreach (var tCard in Hand)
                {
                    Writer.ObjForClear.Add(tCard.Clone());
                    tCard.Y = y;
                }
            }
            card.Y = y;
            Hand.Add(card);
            ResetPosition();
            LastCardSelected = 1;
        }

        private void ResetPosition()
        {
            var space = (Console.WindowWidth % Card.Width)/2;
            if (Card.Width * Hand.Count < Console.WindowWidth)
            {
                space = (Console.WindowWidth - Card.Width * Hand.Count) / 2;
            }
            for(var i = 0; i < Hand.Count; i++)
            {
                // Hand[i].X = i * Card.Width;
                Hand[i].X = i * Card.Width + space;
            }
        }

        public bool CanPlayCurrentCard(GameManager gameManager)
        {
            var result = false;

            var currentCard = gameManager.CurrentCard;
            Card playerCard = null;
            foreach (var card in Hand.Where(card => card.IsSelected).ToList())
            {
                playerCard = card;
            }
            if (playerCard == null) return result;
            if (playerCard.Color == ConsoleColor.White || playerCard.Color == currentCard.Color || playerCard.Value == currentCard.Value)
            {
                result = true;
            }
            return result;
        }
        
        public Card PlayCard(bool remove = false)
        {
            Card result = null;
            OldHand = new List<Card>();
            foreach (var card in Hand)
            {
                OldHand.Add((Card) card.Clone());
            }
            foreach (var card in Hand.Where(card => card.IsSelected).ToList())
            {
                result = card;
                if (!remove) continue;
                if (result.Color == ConsoleColor.White)
                {
                    ColorSelector = true;
                    Hand.First(hCard => hCard.IsSelected).IsSelected = false;
                }
                else
                {
                    SelectNext();
                }
                Hand.Remove(card);
            }
            ClearDrawHand();
            ResetPosition();
            DrawHand();
            return result;
        }
        
        public void DrawHand()
        {
            foreach (var card in Hand)
            {
                Writer.ObjForWrite.Add(card);
            }
        }

        public void SelectNext()
        {
            var needMove = false;
            for (var i = 0; i < Hand.Count; i++)
            {
                if (!Hand[i].IsSelected) continue;
                Hand[i].IsSelected = false;
                Card nextCard;
                if (i == Hand.Count - 1)
                {
                    nextCard = Hand[0];
                    nextCard.IsSelected = true;
                    if (!nextCard.IsDrawn())
                    {
                        needMove = true;
                    }
                }else
                {
                    nextCard = Hand[i + 1];
                    nextCard.IsSelected = true;
                    if (!nextCard.IsDrawn())
                    {
                        needMove = true;
                    }
                }
                LastCardSelected = Hand.IndexOf(nextCard);
                if(!needMove) Writer.ObjForWrite.Add(nextCard);
                break;
            }

            if (needMove)
            {
                var min = (Console.WindowWidth % Card.Width)/2;
                if (Card.Width * Hand.Count < Console.WindowWidth)
                {
                    min = (Console.WindowWidth - Card.Width * Hand.Count) / 2;
                }
                var max = (Hand.Count-1) * Card.Width + min;
                foreach (var card in Hand)
                {
                    card.MoveRight(min,max);
                }
                DrawHand();
            }
        }

        public void SelectPrevious()
        {
            var needMove = false;
            for (var i = 0; i < Hand.Count; i++)
            {
                if (!Hand[i].IsSelected) continue;
                Hand[i].IsSelected = false;
                Card prevCard;
                if (i == 0)
                {
                    prevCard = Hand[Hand.Count - 1];
                    prevCard.IsSelected = true;
                    if (!prevCard.IsDrawn())
                    {
                        needMove = true;
                    }
                }else
                {
                    prevCard = Hand[i - 1];
                    prevCard.IsSelected = true;
                    if (!prevCard.IsDrawn())
                    {
                        needMove = true;
                    }
                }
                LastCardSelected = Hand.IndexOf(prevCard);
                if(!needMove) Writer.ObjForWrite.Add(prevCard);
                break;
            }

            if (needMove)
            {
                var min = (Console.WindowWidth % Card.Width)/2;
                if (Card.Width * Hand.Count < Console.WindowWidth)
                {
                    min = (Console.WindowWidth - Card.Width * Hand.Count) / 2;
                }
                var max = (Hand.Count-1) * Card.Width + min;
                foreach (var card in Hand)
                {
                    card.MoveLeft(min,max);
                }
                DrawHand();
            }
        }

        private static void ClearDrawHand()
        {
            if(OldHand == null) return;
            foreach (var card in OldHand)
            {
                Writer.ObjForClear.Add(card);
            }
        }
        
        

    }
}