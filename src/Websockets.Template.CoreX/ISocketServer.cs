using System;
using Websockets.Template.CoreX.Models;

namespace Websockets.Template.CoreX
{
    public interface ISocketServer
    {
        Action<DataTransferModel> UserMessageHandler { get; set; }
        int MaxConnections { get; set; }
        void Start();
        void Stop();
        void BroadcastMessage(string message);
        void SendMessageById(string clientId, string title, string message);
        void SendMessageBySocketNumber(int socketNumber, string title, string message);
        string GetClientId(int playerNumber);
    }
}
