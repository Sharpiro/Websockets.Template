using System.Collections.Generic;
using Websockets.Template.CoreX.CardApp;
using Websockets.Template.CoreX.TcpListenerServer;
using Xunit;

namespace Websockets.Template.Tests
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class CardApplicationTests
    {
        private readonly CardApplication _cardApplication;
        public CardApplicationTests()
        {
        }

        [Fact]
        public void GetCardTest()
        {
            var card = new Card
            {
                DeckPosition = 1,
                Number = 2,
                Suite = "diamonds",
                Value = "whatever"
            };
            var cardString = card.ToString();
        }

        [Fact]
        public void GetDeckTest()
        {
            CardDefinitions.GetDeck();
        }

        [Fact]
        public void GetPlayerTest()
        {
            var app = new PokerApplication(new SocketServer());
            var player = app.GetPlayer("12345");
            Assert.Null(player);
        }
    }
}
