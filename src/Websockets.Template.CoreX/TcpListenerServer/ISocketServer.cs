namespace Websockets.Template.CoreX.TcpListenerServer
{
    public interface ISocketServer
    {
        int MaxSockets { get; set; }
        void Start();
        void Stop();
        void BroadcastMessage(string message);
        void SendMessageById(string clientId, string title, string message);
        void SendMessageBySocketNumber(int socketNumber, string title, string message);
        string GetSocketId(int socketNumber);
    }
}
