using Websockets.Core.Models;

namespace Websockets.Core.OwinSocketServer
{
    public class MessagingApplication : BaseApplication
    {
        public MessagingApplication(ISocketHandler socketHandler) : base(socketHandler) { }

        public override void Start()
        {

        }

        public override void Stop()
        {

        }

        public override void HandleMessage(DataTransferModel m)
        {
            SocketHandler.BroadcastMessage(m.SocketId, "chat", m.Data);
            //SocketHandler.SendMessageById(m.SocketId, "chat", $"peronal message to socket id: '{m.SocketId}'");
        }
    }
}