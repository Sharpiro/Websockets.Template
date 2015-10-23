using Websockets.Template.CoreX.Models;
using Websockets.Template.CoreX.TcpListenerServer;

namespace Websockets.Template.CoreX.OwinSocketServer
{
    public interface IWebSocketApplication
    {
        string Id { get; set; }
        void HandleMessage(ISocketServer server, DataTransferModel messageObject);
        void AddPlayer(DataTransferModel messageObject);
        bool IsFull();
        void RemovePlayer(string socketId);
        bool IsEmpty();
        
    }
}
