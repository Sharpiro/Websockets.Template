using Websockets.Template.CoreX.Models;
using Websockets.Template.CoreX.OwinSocketServer;

namespace Websockets.Template.CoreX.CardApp
{
    public sealed class PokerApplication : CardApplication, IWebSocketApplication
    {
        public PokerApplication()
        {
            MaxPlayers = 2;
        }

        public void HandleMessage(WebSocketHandler socketHandler, DataTransferModel messageObject)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (messageObject.Data)
            {
                //case "getcard":
                //    GetCardResponse(socket, messageObject);
                //    break;
                //case "resetdeck":
                //    ResetDeck();
                //    break;
            }
        }

        public void Start(ISocketHandler handler)
        {
            foreach (var playerEntry in Players)
            {
                var player = playerEntry.Value;
                var card1 = GetCard().ToString();
                var card2 = GetCard().ToString();
                handler.SendMessageById(player.SocketId, "update", card1);
                handler.SendMessageById(player.SocketId, "update", card2);
            }
            IsStarted = true;
        }
    }
}
