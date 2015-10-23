namespace Websockets.Template.CoreX.OwinSocketServer
{
    public interface ISocketHandler
    {
        int MaxSockets { get; set; }
        void BroadcastMessage(string message);
        void SendMessageById(string clientId, string title, string message);
        void SendMessageBySocketNumber(int socketNumber, string title, string message);
        string GetSocketId(int socketNumber);
    }
}
