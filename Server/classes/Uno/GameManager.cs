using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Server.components;
using Server.components.Uno;
using Server.objects;

namespace Server.classes.Uno
{
    public class GameManager
    {
        public readonly Stack<Card> ShuffledDeck = CardManager.CreateDeck("random");
        public Card _deckCard = new Card();
        
        public readonly ColorSelector ColorSelector = new ColorSelector();
        private Stack<Card> _discardPile = new Stack<Card>();
        public Card CurrentCard { get; set; }
        public readonly PlayerManager PlayerManager = new PlayerManager();
        internal int CurrentPlayerIndex { get; set; } = 0;
        private int _direction = 1;
        private int _currentColor = 0;

        public const int NumberOfCardsToDraw = 2;
        public bool IsGameFinished { get; private set; }

        public void StartGame()
        {
            DistributeCards();
            CurrentCard = ShuffledDeck.Pop();
            AddDiscardPile(CurrentCard);
            
            foreach (var player in PlayerManager.Players)
            {
                _deckCard.IsSelected = false;
                _deckCard.X = Console.WindowWidth / 2 + 2;
                _deckCard.Y = 1;
                _deckCard.Value = ShuffledDeck.Count.ToString();
                
                
                var x = new Turn
                {
                    CanPlay = true,
                    Hand = player.Hand,
                    CurrentCard = CurrentCard,
                    DeckCard = _deckCard,
                    PickUp = false
                };
                var y = new Json(JsonType.Turn, x).Send();
                Sender.Add(new Send(player, y));
            }
            PlayerManager.Players.First().Hand.First().IsSelected = true;
            
            var w = new Turn
            {
                CanPlay = true,
                Hand = PlayerManager.Players.First().Hand,
                CurrentCard = Program.GameManager.CurrentCard,
                DeckCard = Program.GameManager._deckCard
            };
            var a = new Json(JsonType.Turn, w).Send();
            Sender.Add(new Send(PlayerManager.Players.First(), a));
            PlayerManager.Players.First().Hand.First().Value = "0";
            w = new Turn
            {
                CanPlay = true,
                Hand = PlayerManager.Players.First().Hand,
                CurrentCard = Program.GameManager.CurrentCard,
                DeckCard = Program.GameManager._deckCard
            };
            a = new Json(JsonType.Turn, w).Send();
            Sender.Add(new Send(PlayerManager.Players.First(), a));
            PlayerManager.Players.First().Hand.First().Value = "1";
            w = new Turn
            {
                CanPlay = true,
                Hand = PlayerManager.Players.First().Hand,
                CurrentCard = Program.GameManager.CurrentCard,
                DeckCard = Program.GameManager._deckCard
            };
            a = new Json(JsonType.Turn, w).Send();
            Sender.Add(new Send(PlayerManager.Players.First(), a));
            PlayerManager.Players.First().Hand.First().Value = "2";
            w = new Turn
            {
                CanPlay = true,
                Hand = PlayerManager.Players.First().Hand,
                CurrentCard = Program.GameManager.CurrentCard,
                DeckCard = Program.GameManager._deckCard
            };
            a = new Json(JsonType.Turn, w).Send();
            Sender.Add(new Send(PlayerManager.Players.First(), a));
            PlayerManager.Players.First().Hand.First().Value = "3";
            w = new Turn
            {
                CanPlay = true,
                Hand = PlayerManager.Players.First().Hand,
                CurrentCard = Program.GameManager.CurrentCard,
                DeckCard = Program.GameManager._deckCard
            };
            a = new Json(JsonType.Turn, w).Send();
            Sender.Add(new Send(PlayerManager.Players.First(), a));
            PlayerManager.Players.First().Hand.First().Value = "4";
            w = new Turn
            {
                CanPlay = true,
                Hand = PlayerManager.Players.First().Hand,
                CurrentCard = Program.GameManager.CurrentCard,
                DeckCard = Program.GameManager._deckCard
            };
            a = new Json(JsonType.Turn, w).Send();
            Sender.Add(new Send(PlayerManager.Players.First(), a));
            
        }

        public void InitGame()
        {
            foreach (var player in PlayerManager.Players)
            {
                var game = new InitGame
                {
                    UserId = player.Id
                };
                var x = new Json(JsonType.Init, game);
                Sender.Add(new Send(player, x.Send() ));
            }
            StartGame();
        }
        
        
        private void CurrentGame()
        {
            // DrawDeck();
            // while (true)
            // {
            //     switch (Console.ReadKey(true).Key)
            //     {
            //         case ConsoleKey.LeftArrow:
            //             GameAction.LeftArrow(this);
            //             break;
            //         case ConsoleKey.RightArrow:
            //             GameAction.RightArrow(this);
            //             break;
            //         case ConsoleKey.DownArrow:
            //             GameAction.DownArrow(this);
            //             DrawDeck();
            //             break;
            //         case ConsoleKey.UpArrow:
            //             GameAction.UpArrow(this);
            //             DrawDeck();
            //             break;
            //         case ConsoleKey.Enter:
            //             GameAction.Enter(this);
            //             DrawDeck();
            //             break;
            //         case ConsoleKey.Escape:
            //             IsGameFinished = true;
            //             break;
            //         case ConsoleKey.Backspace:
            //         case ConsoleKey.Tab:
            //         case ConsoleKey.Clear:
            //         case ConsoleKey.Pause:
            //         case ConsoleKey.Spacebar:
            //         case ConsoleKey.PageUp:
            //         case ConsoleKey.PageDown:
            //         case ConsoleKey.End:
            //         case ConsoleKey.Home:
            //         case ConsoleKey.Select:
            //         case ConsoleKey.Print:
            //         case ConsoleKey.Execute:
            //         case ConsoleKey.PrintScreen:
            //         case ConsoleKey.Insert:
            //         case ConsoleKey.Delete:
            //         case ConsoleKey.Help:
            //         case ConsoleKey.D0:
            //         case ConsoleKey.D1:
            //         case ConsoleKey.D2:
            //         case ConsoleKey.D3:
            //         case ConsoleKey.D4:
            //         case ConsoleKey.D5:
            //         case ConsoleKey.D6:
            //         case ConsoleKey.D7:
            //         case ConsoleKey.D8:
            //         case ConsoleKey.D9:
            //         case ConsoleKey.A:
            //         case ConsoleKey.B:
            //         case ConsoleKey.C:
            //         case ConsoleKey.D:
            //         case ConsoleKey.E:
            //         case ConsoleKey.F:
            //         case ConsoleKey.G:
            //         case ConsoleKey.H:
            //         case ConsoleKey.I:
            //         case ConsoleKey.J:
            //         case ConsoleKey.K:
            //         case ConsoleKey.L:
            //         case ConsoleKey.M:
            //         case ConsoleKey.N:
            //         case ConsoleKey.O:
            //         case ConsoleKey.P:
            //         case ConsoleKey.Q:
            //         case ConsoleKey.R:
            //         case ConsoleKey.S:
            //         case ConsoleKey.T:
            //         case ConsoleKey.U:
            //         case ConsoleKey.V:
            //         case ConsoleKey.W:
            //         case ConsoleKey.X:
            //         case ConsoleKey.Y:
            //         case ConsoleKey.Z:
            //         case ConsoleKey.LeftWindows:
            //         case ConsoleKey.RightWindows:
            //         case ConsoleKey.Applications:
            //         case ConsoleKey.Sleep:
            //         case ConsoleKey.NumPad0:
            //         case ConsoleKey.NumPad1:
            //         case ConsoleKey.NumPad2:
            //         case ConsoleKey.NumPad3:
            //         case ConsoleKey.NumPad4:
            //         case ConsoleKey.NumPad5:
            //         case ConsoleKey.NumPad6:
            //         case ConsoleKey.NumPad7:
            //         case ConsoleKey.NumPad8:
            //         case ConsoleKey.NumPad9:
            //         case ConsoleKey.Multiply:
            //         case ConsoleKey.Add:
            //         case ConsoleKey.Separator:
            //         case ConsoleKey.Subtract:
            //         case ConsoleKey.Decimal:
            //         case ConsoleKey.Divide:
            //         case ConsoleKey.F1:
            //         case ConsoleKey.F2:
            //         case ConsoleKey.F3:
            //         case ConsoleKey.F4:
            //         case ConsoleKey.F5:
            //         case ConsoleKey.F6:
            //         case ConsoleKey.F7:
            //         case ConsoleKey.F8:
            //         case ConsoleKey.F9:
            //         case ConsoleKey.F10:
            //         case ConsoleKey.F11:
            //         case ConsoleKey.F12:
            //         case ConsoleKey.F13:
            //         case ConsoleKey.F14:
            //         case ConsoleKey.F15:
            //         case ConsoleKey.F16:
            //         case ConsoleKey.F17:
            //         case ConsoleKey.F18:
            //         case ConsoleKey.F19:
            //         case ConsoleKey.F20:
            //         case ConsoleKey.F21:
            //         case ConsoleKey.F22:
            //         case ConsoleKey.F23:
            //         case ConsoleKey.F24:
            //         case ConsoleKey.BrowserBack:
            //         case ConsoleKey.BrowserForward:
            //         case ConsoleKey.BrowserRefresh:
            //         case ConsoleKey.BrowserStop:
            //         case ConsoleKey.BrowserSearch:
            //         case ConsoleKey.BrowserFavorites:
            //         case ConsoleKey.BrowserHome:
            //         case ConsoleKey.VolumeMute:
            //         case ConsoleKey.VolumeDown:
            //         case ConsoleKey.VolumeUp:
            //         case ConsoleKey.MediaNext:
            //         case ConsoleKey.MediaPrevious:
            //         case ConsoleKey.MediaStop:
            //         case ConsoleKey.MediaPlay:
            //         case ConsoleKey.LaunchMail:
            //         case ConsoleKey.LaunchMediaSelect:
            //         case ConsoleKey.LaunchApp1:
            //         case ConsoleKey.LaunchApp2:
            //         case ConsoleKey.Oem1:
            //         case ConsoleKey.OemPlus:
            //         case ConsoleKey.OemComma:
            //         case ConsoleKey.OemMinus:
            //         case ConsoleKey.OemPeriod:
            //         case ConsoleKey.Oem2:
            //         case ConsoleKey.Oem3:
            //         case ConsoleKey.Oem4:
            //         case ConsoleKey.Oem5:
            //         case ConsoleKey.Oem6:
            //         case ConsoleKey.Oem7:
            //         case ConsoleKey.Oem8:
            //         case ConsoleKey.Oem102:
            //         case ConsoleKey.Process:
            //         case ConsoleKey.Packet:
            //         case ConsoleKey.Attention:
            //         case ConsoleKey.CrSel:
            //         case ConsoleKey.ExSel:
            //         case ConsoleKey.EraseEndOfFile:
            //         case ConsoleKey.Play:
            //         case ConsoleKey.Zoom:
            //         case ConsoleKey.NoName:
            //         case ConsoleKey.Pa1:
            //         case ConsoleKey.OemClear:
            //         default:
            //             break;
            //     }
            //     if (!IsGameFinished) continue;
            //     break;
            // }
        }

        public void AddDiscardPile(Card card)
        {
            // if (CurrentCard != null)
            // {
            //     Writer.ObjForClear.Add(CurrentCard);
            // }
            CurrentCard = card;
            _discardPile.Push(card);
            card.X = Console.WindowWidth / 2 - Card.Width;
            card.Y = 1;
            // Writer.ObjForWrite.Add(card);
        }
        private void DrawDeck()
        {
            _deckCard.IsSelected = CurrentPlayer().SelectedDeck;
            _deckCard.X = Console.WindowWidth / 2 + 2;
            _deckCard.Y = 1;
            _deckCard.Value = ShuffledDeck.Count.ToString();
            Writer.ObjForWrite.Add(_deckCard);
        }

        
        private void DistributeCards()
        {
            for (var i = 0; i < NumberOfCardsToDraw; i++)
            {
                foreach (var player in PlayerManager.Players)
                {
                    var card = ShuffledDeck.Pop();
                    player.AddCardToHand(card);
                }
            }
        }

        public Player CurrentPlayer()
        {
            return PlayerManager.Players[CurrentPlayerIndex];
        }

    }
}