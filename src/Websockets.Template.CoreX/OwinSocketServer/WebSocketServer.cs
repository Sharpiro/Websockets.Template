using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Websockets.Template.CoreX.Models;

namespace Websockets.Template.CoreX.OwinSocketServer
{
    public class WebSocketServer
    {
        private readonly ConcurrentDictionary<string, WebSocketWrapper> _sockets;

        public WebSocketServer()
        {
            _sockets = new ConcurrentDictionary<string, WebSocketWrapper>();
        }

        public void AddSocket(WebSocketWrapper newSocket)
        {
            var socketId = Guid.NewGuid().ToString();
            newSocket.Id = socketId;
            newSocket.CancellationTokenSource = new CancellationTokenSource();
            _sockets.TryAdd(socketId, newSocket);
        }

        public async Task ListenOnSocket(string socketId)
        {
            var socket = _sockets[socketId];
            while (!socket.CancellationTokenSource.IsCancellationRequested)
            {
                try
                {
                    var result = await socket.Receive();
                    HandleReceivedData(result, socket.Id);

                }
                catch (Exception ex)
                {
                    Debug.WriteLine("err");
                }
            }
        }

        private void HandleReceivedData(string messageSource, string socketId)
        {
            var messageObject = JsonConvert.DeserializeObject<DataTransferModel>(messageSource);
            switch (messageObject.DataType.ToLowerInvariant())
            {
                case "message":
                    SendMessageById(socketId, "message", "abcdefg");
                    break;
            }
        }

        private void SendMessageById(string socketId, string dataTitle, string data)
        {
            var socket = _sockets[socketId];
            socket?.SendEncoded("message", dataTitle, data);
        }
    }
}