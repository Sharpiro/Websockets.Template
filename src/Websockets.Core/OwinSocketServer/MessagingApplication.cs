using Websockets.Core.Models;

namespace Websockets.Core.OwinSocketServer
{
    public class MessagingApplication : BaseApplication
    {
        public MessagingApplication(ISocketHandler socketHandler) : base(socketHandler)
        {
            MaxUsers = 3;
        }

        public override void Start()
        {

        }

        public override void Stop()
        {

        }

        public override void AddUser(WebSocketWrapper webSocket)
        {
            base.AddUser(webSocket);

            var appMessage = $"User: '{webSocket.Id}' has logged in";
            var chatMessage = $"User: '{webSocket.Id}' has joined the chat";
            SocketHandler.BroadcastMessage(webSocket.Id, "chat", appMessage);
            SocketHandler.BroadcastMessageToApp(webSocket.Id, webSocket.ApplicationId, "chat", chatMessage);
            SocketHandler.BroadcastMessage(webSocket.Id, "allUsersCount", SocketHandler.Count);
            SocketHandler.BroadcastMessageToApp(webSocket.Id, webSocket.ApplicationId, "appUsersCount", Users.Count);
        }

        public override void RemoveUser(WebSocketWrapper webSocket)
        {
            base.RemoveUser(webSocket);

            var appMessage = $"User: '{webSocket.Id}' has logged out";
            var chatMessage = $"User: '{webSocket.Id}' has left the chat";
            SocketHandler.BroadcastMessage(webSocket.Id, "chat", appMessage);
            SocketHandler.BroadcastMessageToApp(webSocket.Id, webSocket.ApplicationId, "chat", chatMessage);
            SocketHandler.BroadcastMessage(webSocket.Id, "allUsersCount", SocketHandler.Count);
            SocketHandler.BroadcastMessageToApp(webSocket.Id, webSocket.ApplicationId, "appUsersCount", Users.Count);
        }

        public override void HandleMessage(DataTransferModel m)
        {
            SocketHandler.BroadcastMessageToApp(m.SocketId, m.ApplicationId, "chat", m.Data);
            //SocketHandler.SendMessageById(m.SocketId, "chat", $"peronal message to socket id: '{m.SocketId}'");
        }
    }
}