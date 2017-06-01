namespace Websockets.Core.OwinSocketServer
{
    public interface ISocketHandler
    {
        int MaxSockets { get; set; }
        void BroadcastMessage(string socketId, string title, string message);
        void SendMessageById(string clientId, string title, string message);
        void SendMessageBySocketNumber(int socketNumber, string title, string message);
        string GetSocketId(int socketNumber);
        WebSocketWrapper GetSocketById(string socketId);
        void UpdateWebSocketApplicationId(string socketId, string applicationId);
        void AddSocket(WebSocketWrapper newSocket);
        void RemoveSocket(string oldSocketId);
        string GetApplicationIdFromSocketId(string socketId);
        void CloseAllSockets();
    }
}
