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
            _cardApplication = new CardApplication(new SocketServer());
        }

        [Fact]
        public void GetCardTest()
        {
            Card card;
            while ((card = _cardApplication.GetCard()) != null)
            {
                var temp = _cardApplication._deck.Count;
            }
        }

        [Fact]
        public void GetDeckTest()
        {
            CardDefinitions.GetDeck();
        }
    }
}
