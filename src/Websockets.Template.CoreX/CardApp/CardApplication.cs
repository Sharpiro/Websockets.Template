using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Websockets.Template.CoreX.Models;
using Websockets.Template.CoreX.TcpListenerServer;

namespace Websockets.Template.CoreX.CardApp
{
    public sealed class CardApplication : BaseApplication
    {
        public List<Card> _deck;
        private List<Player> _players;
        private const int MaxPlayers = 2;


        public CardApplication(ISocketServer socketServer) : base(socketServer, MaxPlayers)
        {
            _players = new List<Player>();
            _deck = CardDefinitions.GetDeck();
        }

        protected override void HandleMessage(DataTransferModel messageObject)
        {
            switch (messageObject.DataTitle)
            {
                case "addplayer":
                    AddPlayer(messageObject);
                    break;
                case "getcard":
                    GetCardResponse(messageObject);
                    break;
                case "player1":
                    _socketServer.SendMessageBySocketNumber(1, "player1", messageObject.Data);
                    break;
                case "player2":
                    _socketServer.SendMessageBySocketNumber(2, "player2", messageObject.Data);
                    break;
            }
        }

        private void AddPlayer(DataTransferModel messageObject)
        {
            if (_players.Count >= MaxPlayers)
                _players.RemoveAt(0);
            _players.Add(new Player {Number = messageObject.SocketNumber});
        }

        private void GetCardResponse(DataTransferModel messageObject)
        {
            var card = GetCard();
            var cardJson = JsonConvert.SerializeObject(card);
            var playerNumber = messageObject.SocketNumber;
            _players[playerNumber - 1].Hand.Add(card);
            _socketServer.SendMessageById(messageObject.ClientId, messageObject.DataTitle, cardJson);
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
    }
}
