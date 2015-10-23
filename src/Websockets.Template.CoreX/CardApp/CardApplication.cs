using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Websockets.Template.CoreX.Models;
using Websockets.Template.CoreX.OwinSocketServer;

namespace Websockets.Template.CoreX.CardApp
{
    public abstract class CardApplication : BaseApplication
    {
        private List<Card> _deck;

        protected CardApplication()
        {
            _deck = CardDefinitions.GetDeck();
        }

        protected void GetCardResponse(WebSocketWrapper socket, DataTransferModel messageObject)
        {
            var card = GetCard();
            var cardJson = JsonConvert.SerializeObject(card);
            var playerSocketId = messageObject.SocketId;
            Players[playerSocketId].Hand.Add(card);
            socket.SendEncoded(messageObject.SocketId, messageObject.DataTitle, cardJson);
            //server.SendMessageById(messageObject.SocketId, messageObject.DataTitle, cardJson);
        }

        protected void ResetDeck()
        {
            _deck = CardDefinitions.GetDeck();
        }

        protected Card GetCard()
        {
            if (_deck.Count <= 0) return null;
            var randomizer = new Random();
            var deckPosition = randomizer.Next(_deck.Count);
            var card = _deck[deckPosition];
            _deck.Remove(card);
            return card;
        }
    }
}
