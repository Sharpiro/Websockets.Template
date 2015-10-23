using Websockets.Template.CoreX.Models;
using Websockets.Template.CoreX.TcpListenerServer;

namespace Websockets.Template.CoreX.OwinSocketServer
{
    public interface IWebSocketApplication
    {
        string Id { get; set; }
        bool IsStarted { get; set; }
        void HandleMessage(WebSocketHandler socketHandler, DataTransferModel messageObject);
        void AddPlayer(DataTransferModel messageObject);
        bool IsFull();
        void RemovePlayer(string socketId);
        bool IsEmpty();
        void Start(ISocketHandler handler);
    }
}
