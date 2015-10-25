using System.Collections.Generic;
using System.Linq;
using Websockets.Template.CoreX.CardApp;
using Websockets.Template.CoreX.TcpListenerServer;
using Xunit;

namespace Websockets.Template.Tests
{
    public class PokerHandsHandlerTests
    {
        [Fact]
        public void GetCardTest()
        {
            var app = new PokerApplication(new SocketServer());
            var hand = new List<Card>
            {
                app.GetCard(),
                app.GetCard(),
                app.GetCard(),
                app.GetCard(),
                app.GetCard(),
                app.GetCard(),
                app.GetCard()
            };
            while (true)
            {
                hand = new List<Card>
                {
                    app.GetCard(),
                    app.GetCard(),
                    app.GetCard(),
                    app.GetCard(),
                    app.GetCard(),
                    app.GetCard(),
                    app.GetCard()
                };
                if (hand.Any(c => c == null))
                    app.ResetDeck();
                else
                    PokerHandsHandler.GetBestHand(hand);

            }
        }
    }
}
