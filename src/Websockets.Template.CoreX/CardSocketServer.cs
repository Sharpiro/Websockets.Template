using Websockets.Template.CoreX.Models;

namespace Websockets.Template.CoreX
{
    public class CardSocketServer : BaseSocketServer
    {
        public CardSocketServer()
        {
            Handler = HandleMessage;
        }

        public new void Start()
        {
            base.Start();
            base.AcceptClientsAsync();
        }

        private void HandleMessage(DataTransferModel messageObject)
        {
            switch (messageObject.DataType)
            {
                case "message":
                    DoSomething(messageObject);
                    break;
            }
        }

        private void DoSomething(DataTransferModel messageObject)
        {
            SendMessage(messageObject.ClientId, messageObject.DataTitle, "some data here yo");
        }
    }
}
