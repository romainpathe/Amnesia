using System;
using System.Linq;
using Client.components;
using Client.components.Uno;
using Client.objects;

namespace Client.classes.Uno
{
    public static class GameAction
    {
        
        public static void UpArrow()
        {
            if(!GameManager.CanPlay) return;
            if(Player.SelectedDeck) return;
            if(Player.ColorSelector) return;
            Player.SelectedDeck = true;
            Player.Hand.First(card => card.IsSelected).IsSelected = false;
        }
        
        public static void DownArrow()
        {
            if(!GameManager.CanPlay) return;
            if(!Player.SelectedDeck) return;
            if(Player.ColorSelector) return;
            Player.SelectedDeck = false;
            var fCard  = Player.Hand[Player.LastCardSelected];
            if (fCard != null) fCard.IsSelected = true;
        }
        
        public static void LeftArrow()
        {
            if(!GameManager.CanPlay) return;
            if (Player.ColorSelector)
            {
                ColorSelector.SelectPrevious();
            }
            else
            {
                Player.SelectPrevious();
            }
        }
        
        public static void RightArrow()
        {
            if(!GameManager.CanPlay) return;
            if (Player.ColorSelector)
            {
                ColorSelector.SelectNext();
            }
            else
            {
                Player.SelectNext();
            }
        }
        
        public static void Space()
        {
            
        }
        
        public static void Enter()
        {
            if (!GameManager.CanPlay) return;
            if (Player.SelectedDeck)
            {
                var turn = new Turn()
                {
                    PickUp = true,
                };
                Sender.ObjForSend.Add(new Send(new Json(JsonType.Turn, turn).Send()));
                Sender.ObjForSend.Add(new Send(new Json(JsonType.Turn, turn).Send()));
                Sender.ObjForSend.Add(new Send(new Json(JsonType.Turn, turn).Send()));
                GameManager.CanPlay = false;
                GameManager.DeckCard.IsSelected = false;
                Writer.Write(GameManager.DeckCard);
            }
            // else if (Player.ColorSelector)
            // {
            //     GameManager.CurrenCard.Color = ColorSelector.CurrentColor();
            //     Writer.ObjForWrite.Add(GameManager.CurrenCard);
            //     Writer.ObjForClear.Add(gameManager.ColorSelector);
            //     Player.ColorSelector = false;
            //     Player.SelectedDeck = true;
            // }else
            // {
            //     var canPlay =Player.CanPlayCurrentCard(gameManager);
            //     if (!canPlay) return;
            //     var card = Player.PlayCard(true);
            //     if (card.Color == ConsoleColor.White)
            //     {
            //         gameManager.ColorSelector.Draw();
            //     }
            //     gameManager.AddDiscardPile(card);
            // }
        }

    }
}