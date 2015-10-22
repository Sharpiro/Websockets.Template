using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Websockets.Template.CoreX.Models;
using Websockets.Template.CoreX.OwinSocketServer;
using Websockets.Template.CoreX.TcpListenerServer;

namespace Websockets.Template.CoreX.CardApp
{
    public sealed class CardApplication : IWebSocketApplication
    {
        public string Id { get; set; }
        public List<Card> _deck;
        private readonly List<Player> _players;
        private const int MaxPlayers = 2;

        public CardApplication()
        {
            _players = new List<Player>();
            _deck = CardDefinitions.GetDeck();
        }

        public void HandleMessage(ISocketServer server, DataTransferModel messageObject)
        {
            switch (messageObject.Data)
            {
                case "getcard":
                    GetCardResponse(server, messageObject);
                    break;
                case "player1":
                    server.SendMessageBySocketNumber(1, "player1", messageObject.Data);
                    break;
                case "player2":
                    server.SendMessageBySocketNumber(2, "player2", messageObject.Data);
                    break;
            }
        }

        public void AddPlayer(DataTransferModel messageObject)
        {
            if (_players.Count >= MaxPlayers)
                _players.RemoveAt(0);
            _players.Add(new Player
            {
                SocketId = messageObject.SocketId,
                SocketNumber = messageObject.SocketNumber
            });
        }

        private void GetCardResponse(ISocketServer server, DataTransferModel messageObject)
        {
            var card = GetCard();
            var cardJson = JsonConvert.SerializeObject(card);
            var playerNumber = messageObject.SocketNumber;
            _players[playerNumber - 1].Hand.Add(card);
            server.SendMessageById(messageObject.SocketId, messageObject.DataTitle, cardJson);
        }

        public Card GetCard()
        {
            if (_deck.Count <= 0) return null;
            var randomizer = new Random();
            var deckPosition = randomizer.Next(_deck.Count);
            var card = _deck[deckPosition];
            _deck.Remove(card);
            return card;
        }

        public void ResetDeck()
        {
            _deck = CardDefinitions.GetDeck();
        }

        public void InitializeDeck()
        {

        }

        public bool IsFull()
        {
            if (_players.Count > MaxPlayers)
                throw new Exception("too many players in game this shouldn't happen...");
            return _players.Count == MaxPlayers;
        }

        //public IEnumerable<string> GetPlayerSocketIds()
        //{
        //    return _players.Select(p => p.SocketId);
        //}
    }
}
