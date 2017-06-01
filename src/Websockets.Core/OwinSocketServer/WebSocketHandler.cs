using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Websockets.Core.OwinSocketServer
{
    public class WebSocketHandler : ISocketHandler
    {
        private readonly ConcurrentDictionary<string, WebSocketWrapper> _sockets;
        private int _maxSockets = 5;
        public int MaxSockets { get => _maxSockets; set { if (_sockets.Count < 1) _maxSockets = value; } }

        public WebSocketHandler()
        {
            _sockets = new ConcurrentDictionary<string, WebSocketWrapper>();
        }

        public void AddSocket(WebSocketWrapper newSocket)
        {
            var socketId = Guid.NewGuid().ToString();
            newSocket.Id = socketId;
            _sockets.TryAdd(socketId, newSocket);
            newSocket.Number = _sockets.Count;
            newSocket.CancellationTokenSource = new CancellationTokenSource();
            Debug.WriteLine($"# of Sockets: {_sockets.Count}");
        }

        public void RemoveSocket(string oldSocketId)
        {
            if (string.IsNullOrEmpty(oldSocketId)) throw new ArgumentNullException(nameof(oldSocketId));

            var success = _sockets.TryRemove(oldSocketId, out WebSocketWrapper socket);
            if (!success) return;
            socket.Close();
        }

        public WebSocketWrapper GetSocketById(string socketId)
        {
            if (string.IsNullOrEmpty(socketId)) throw new ArgumentNullException(nameof(socketId));

            var success = _sockets.TryGetValue(socketId, out WebSocketWrapper socket);
            if (!success) throw new KeyNotFoundException($"Unable to find socket with id: '{socketId}'");

            return socket;
        }

        public string GetSocketId(int socketNumber)
        {
            return _sockets.FirstOrDefault(c => c.Value.Number == socketNumber).Value.Id;
        }

        public void UpdateWebSocketApplicationId(string socketId, string applicationId)
        {
            _sockets[socketId].ApplicationId = applicationId;
        }

        public void SendMessageById(string socketId, string dataTitle, string data)
        {
            _sockets[socketId]?.SendEncoded("message", dataTitle, data);
        }

        public string GetApplicationIdFromSocketId(string socketId)
        {
            return _sockets[socketId].ApplicationId;
        }

        public void BroadcastMessage(string message)
        {
            foreach (var socket in _sockets)
            {
                socket.Value.SendEncoded("broadcast", "broadcast", message);
            }
        }

        public void SendMessageBySocketNumber(int socketNumber, string title, string message)
        {
            var socketId = GetSocketId(socketNumber);
            SendMessageById(socketId, title, message);
        }

        public void CloseAllSockets()
        {
            foreach (var socket in _sockets)
            {
                socket.Value.Close();
            }
        }
    }
}
