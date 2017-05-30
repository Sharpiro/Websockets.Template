using System;
using System.Collections.Generic;
using System.Linq;
using Websockets.Core.Models;
using Websockets.Core.OwinSocketServer;

namespace Cards.Core
{
    public abstract class CardApplication : BaseApplication
    {
        private List<Card> _deck;
        //private readonly List<Card> _communityCards;
        protected int CardsToDeal { get; set; } = 2;
        private readonly Random _randomizer = new Random();
        public int Round { get; set; } = 1;
        public Dictionary<int, int> CardsPerRound;

        protected CardApplication(ISocketHandler socketHandler) : base(socketHandler)
        {
            //_communityCards = new List<Card>();
            _deck = CardDefinitions.GetDeck();
        }

        protected string AddCardToPlayer(string socketId)
        {
            var card = GetCard();
            var player = Users[socketId] as Player;
            player.Hand.Add(card);
            return card.ToString();
        }

        protected string AddCardToCommunity()
        {
            var card = GetCard();
            foreach (var player in Users.Select(p => p.Value as Player))
            {
                player.Hand.Add(card);
            }
            return card.ToString();
        }

        protected void ClearBets()
        {
            foreach (var player in Users.Select(p => p.Value as Player))
            {
                player.CurrentBet = null;
            }
        }

        protected bool HasBet(string socketId)
        {
            var player = GetUser(socketId) as Player;
            return player != null && player.HasBet();
        }

        protected bool AllUsersHaveBet()
        {
            return Users.Select(p => p.Value as Player).All(p => p.CurrentBet != null);
        }

        protected void PlaceBet(string socketId, string data)
        {
            var betAmount = int.Parse(data);
            var player = Users[socketId] as Player;
            player.CurrentBet = betAmount;
        }

        public void ResetDeck()
        {
            _deck = CardDefinitions.GetDeck();
        }

        public Card GetCard()
        {
            if (_deck.Count <= 0) return null;
            var deckPosition = _randomizer.Next(_deck.Count);
            var card = _deck[deckPosition];
            _deck.Remove(card);
            return card;
        }

        public abstract override void Start();
        public abstract override void Stop();
        public abstract override void HandleMessage(DataTransferModel m);
    }
}