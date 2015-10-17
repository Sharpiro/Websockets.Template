using Websockets.Template.CoreX.Models;

namespace Websockets.Template.CoreX
{
    public sealed class CardApplication: BaseApplication
    {
        public CardApplication(ISocketServer socketServer): base(socketServer, 2)
        {

        }

        protected override void HandleMessage(DataTransferModel messageObject)
        {
            switch (messageObject.DataTitle)
            {
                case "action1":
                    DoSomething(messageObject);
                    break;
                case "player1":
                    _socketServer.SendMessageBySocketNumber(1, "player1", messageObject.Data);
                    break;
                case "player2":
                    _socketServer.SendMessageBySocketNumber(2, "player2", messageObject.Data);
                    break;
            }
        }

        private void DoSomething(DataTransferModel messageObject)
        {
            _socketServer.SendMessageById(messageObject.ClientId, messageObject.DataTitle, "This is a card game message");
        }
    }
}
