using System;
using System.Linq;
using Client.components.Uno;

namespace Client.classes.Uno
{
    public static class GameAction
    {
        
        public static void UpArrow(GameManager gameManager)
        {
            if (Player.SelectedDeck) return;
            if(Player.ColorSelector) return;
            Player.SelectedDeck = true;
            Player.Hand.First(card => card.IsSelected).IsSelected = false;
        }
        
        public static void DownArrow(GameManager gameManager)
        {
            if(!Player.SelectedDeck) return;
            if(Player.ColorSelector) return;
            Player.SelectedDeck = false;
            var fCard  = Player.Hand[Player.LastCardSelected];
            if (fCard != null) fCard.IsSelected = true;
        }
        
        public static void LeftArrow(GameManager gameManager)
        {
            if (Player.ColorSelector)
            {
                ColorSelector.SelectPrevious();
            }
            else
            {
                Player.SelectPrevious();
            }
        }
        
        public static void RightArrow(GameManager gameManager)
        {
            if (Player.ColorSelector)
            {
                ColorSelector.SelectNext();
            }
            else
            {
                Player.SelectNext();
            }
        }
        
        public static void Space(GameManager gameManager)
        {
            
        }
        
        public static void Enter(GameManager gameManager)
        {
            if (Player.SelectedDeck)
            {
                Player.AddCardToHand(gameManager.ShuffledDeck.Pop());
            }
            else if (Player.ColorSelector)
            {
                GameManager.CurrenCard.Color = ColorSelector.CurrentColor();
                Writer.ObjForWrite.Add(GameManager.CurrenCard);
                Writer.ObjForClear.Add(gameManager.ColorSelector);
                Player.ColorSelector = false;
                Player.SelectedDeck = true;
            }else
            {
                var canPlay =Player.CanPlayCurrentCard(gameManager);
                if (!canPlay) return;
                var card = Player.PlayCard(true);
                if (card.Color == ConsoleColor.White)
                {
                    gameManager.ColorSelector.Draw();
                }
                gameManager.AddDiscardPile(card);
            }
        }

    }
}