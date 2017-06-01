using Websockets.Core.Models;

namespace Websockets.Core.OwinSocketServer
{
    public class MessagingApplication : BaseApplication
    {
        public MessagingApplication(ISocketHandler socketHandler) : base(socketHandler) { }

        public override void Start()
        {
            throw new System.NotImplementedException();
        }

        public override void Stop()
        {
            throw new System.NotImplementedException();
        }

        public override void HandleMessage(DataTransferModel m)
        {
            throw new System.NotImplementedException();
        }
    }
}