using System;
using System.Collections.Generic;
using System.Linq;
using Websockets.Template.CoreX.OwinSocketServer;

namespace Websockets.Template.CoreX.CardApp
{
    public abstract class CardApplication : BaseApplication
    {
        private List<Card> _deck;
        private readonly List<Card> _communityCards;
        protected int CardsToDeal { get; set; } = 2;
        private readonly Random _randomizer = new Random();
        public int Round { get; set; } = 1;
        public Dictionary<int, int> CardsPerRound;

        protected CardApplication(ISocketHandler socketHandler) : base(socketHandler)
        {
            _communityCards = new List<Card>();
            _deck = CardDefinitions.GetDeck();
        }

        protected string AddCardToPlayer(string socketId)
        {
            var card = GetCard();
            var player = Players[socketId];
            player.Hand.Add(card);
            return card.ToString();
        }

        protected string AddCardToCommunity()
        {
            var card = GetCard();
            _communityCards.Add(card);
            return card.ToString();
        }

        protected void ClearBets()
        {
            foreach (var player in Players)
            {
                player.Value.CurrentBet = null;
            }
        }

        protected bool AllPlayersHaveBet()
        {
            return Players.All(p => p.Value.CurrentBet != null);
        }

        protected void PlaceBet(string socketId, string data)
        {
            var betAmount = int.Parse(data);
            Players[socketId].CurrentBet = betAmount;
        }

        protected void ResetDeck()
        {
            _deck = CardDefinitions.GetDeck();
        }

        private Card GetCard()
        {
            if (_deck.Count <= 0) return null;
            var deckPosition = _randomizer.Next(_deck.Count);
            var card = _deck[deckPosition];
            _deck.Remove(card);
            return card;
        }
    }
}
