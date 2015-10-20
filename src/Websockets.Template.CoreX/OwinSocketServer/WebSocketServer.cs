using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Threading;

namespace Websockets.Template.CoreX.OwinSocketServer
{
    public class WebSocketServer
    {
        //public List<WebSocket> Sockets { get; set; } = new List<WebSocket>();
        private readonly ConcurrentDictionary<string, WebSocket> _sockets;

        public WebSocketServer()
        {
            _sockets = new ConcurrentDictionary<string, WebSocket>();
        }

        public void AddSocket(WebSocketWrapper newSocket)
        {
            var socketId = Guid.NewGuid().ToString();
            newSocket.CancellationTokenSource = new CancellationTokenSource();
            _sockets.TryAdd(socketId, newSocket);
        }
    }
}