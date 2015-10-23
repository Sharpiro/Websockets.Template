using System;
using System.Collections.Concurrent;
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
        private List<Card> _deck;
        private readonly ConcurrentDictionary<string, Player> _players;
        private const int MaxPlayers = 2;

        public CardApplication()
        {
            _players = new ConcurrentDictionary<string, Player>();
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
            //if (_players.Count >= MaxPlayers)
            //    _players.TryRemove();
            var newPlayer = new Player
            {
                SocketId = messageObject.SocketId,
                SocketNumber = messageObject.SocketNumber,
                PlayerNumber = _players.Count + 1
            };
            _players.TryAdd(newPlayer.SocketId, newPlayer);
        }

        public void RemovePlayer(string socketId)
        {
            Player player;
            _players.TryRemove(socketId, out player);
        }

        private void GetCardResponse(ISocketServer server, DataTransferModel messageObject)
        {
            var card = GetCard();
            var cardJson = JsonConvert.SerializeObject(card);
            var playerSocketId = messageObject.SocketId;
            //_players[playerNumber - 1].Hand.Add(card);
            _players[playerSocketId].Hand.Add(card);
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

        public bool IsEmpty()
        {
            return _players.Count == 0;
        }

        //public IEnumerable<string> GetPlayerSocketIds()
        //{
        //    return _players.Select(p => p.SocketId);
        //}
    }
}
