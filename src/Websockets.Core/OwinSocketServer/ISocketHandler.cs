namespace Websockets.Core.OwinSocketServer
{
    public interface ISocketHandler
    {
        int MaxSockets { get; set; }
        int Count { get; }
        void BroadcastMessage(string socketId, string title, object message);
        void BroadcastMessageToApp(string socketId, string appId, string title, object message);
        void SendMessageById(string clientId, string title, string message);
        void SendMessageBySocketNumber(int socketNumber, string title, string message);
        string GetSocketId(int socketNumber);
        WebSocketWrapper GetSocketById(string socketId);
        void UpdateWebSocketApplicationId(string socketId, string applicationId);
        void AddSocket(WebSocketWrapper newSocket);
        WebSocketWrapper RemoveSocket(string oldSocketId);
        string GetApplicationIdFromSocketId(string socketId);
        void CloseAllSockets();
    }
}
