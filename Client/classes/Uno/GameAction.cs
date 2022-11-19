using System;
using System.Linq;
using Client.components.Uno;

namespace Client.classes.Uno
{
    public static class GameAction
    {
        
        public static void UpArrow(GameManager gameManager)
        {
            if (gameManager.CurrentPlayer().SelectedDeck) return;
            if(gameManager.CurrentPlayer().ColorSelector) return;
            gameManager.CurrentPlayer().SelectedDeck = true;
            gameManager.CurrentPlayer().Hand.First(card => card.IsSelected).IsSelected = false;
        }
        
        public static void DownArrow(GameManager gameManager)
        {
            if(!gameManager.CurrentPlayer().SelectedDeck) return;
            if(gameManager.CurrentPlayer().ColorSelector) return;
            gameManager.CurrentPlayer().SelectedDeck = false;
            var fCard  = gameManager.CurrentPlayer().Hand[gameManager.CurrentPlayer().LastCardSelected];
            if (fCard != null) fCard.IsSelected = true;
        }
        
        public static void LeftArrow(GameManager gameManager)
        {
            if (gameManager.CurrentPlayer().ColorSelector)
            {
                ColorSelector.SelectPrevious();
            }
            else
            {
                gameManager.CurrentPlayer().SelectPrevious();
            }
        }
        
        public static void RightArrow(GameManager gameManager)
        {
            if (gameManager.CurrentPlayer().ColorSelector)
            {
                ColorSelector.SelectNext();
            }
            else
            {
                gameManager.CurrentPlayer().SelectNext();
            }
        }
        
        public static void Space(GameManager gameManager)
        {
            
        }
        
        public static void Enter(GameManager gameManager)
        {
            if (gameManager.CurrentPlayer().SelectedDeck)
            {
                gameManager.CurrentPlayer().AddCardToHand(gameManager.ShuffledDeck.Pop());
            }
            else if (gameManager.CurrentPlayer().ColorSelector)
            {
                gameManager.CurrenCard.Color = ColorSelector.CurrentColor();
                Writer.ObjForWrite.Add(gameManager.CurrenCard);
                Writer.ObjForClear.Add(gameManager.ColorSelector);
                gameManager.CurrentPlayer().ColorSelector = false;
                gameManager.CurrentPlayer().SelectedDeck = true;
            }else
            {
                var canPlay =gameManager.CurrentPlayer().CanPlayCurrentCard(gameManager);
                if (!canPlay) return;
                var card = gameManager.CurrentPlayer().PlayCard(true);
                if (card.Color == ConsoleColor.White)
                {
                    gameManager.ColorSelector.Draw();
                }
                gameManager.AddDiscardPile(card);
            }
        }

    }
}