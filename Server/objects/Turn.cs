using System.Collections.Generic;
using Newtonsoft.Json;
using Server.components.Uno;

namespace Server.objects
{
    public class Turn
    {
        public bool CanPlay { get; set; }
        public List<Card> Hand { get; set; }
        public Card CurrentCard { get; set; }
        public Card DeckCard { get; set; }
        public bool PickUp { get; set; }
        
        public Turn(){}
        
        [JsonConstructor]
        public Turn(bool canPlay, List<Card> hand, Card currentCard, Card deckCard, bool pickUp)
        {
            CanPlay = canPlay;
            Hand = hand;
            CurrentCard = currentCard;
            DeckCard = deckCard;
            PickUp = pickUp;
        }
    }
}