using System.Collections.Generic;
using Websockets.Core.Models;

namespace Cards.Core.CardApp
{
    public class Player : User
    {
        public List<Card> Hand { get; set; } = new List<Card>();
        public int? CurrentBet { get; set; }
        public PokerHand BestHand { get; set; }

        public bool HasBet()
        {
            return CurrentBet != null;
        }
    }
}