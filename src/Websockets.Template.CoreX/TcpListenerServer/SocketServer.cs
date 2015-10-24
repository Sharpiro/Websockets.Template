using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Websockets.Template.CoreX.Models;
using Websockets.Template.CoreX.OwinSocketServer;

namespace Websockets.Template.CoreX.TcpListenerServer
{
    public class SocketServer : ISocketHandler
    {
        public Action<DataTransferModel> UserMessageHandler { get; set; }
        public int MaxSockets { get { return _maxSockets; } set { if (_numberOfConnections < 1) _maxSockets = value; } }
        private readonly IList<SocketWrapper> _connections;
        private readonly ConcurrentDictionary<string, SocketWrapper> _dictionary;
        private readonly TcpListener _tcpListener;
        private int _numberOfConnections;
        private int _maxSockets = 5;

        public SocketServer()
        {
            //var websocket = new WebSocket();
            _tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 56837);//8095
            _dictionary = new ConcurrentDictionary<string, SocketWrapper>();
            _connections = new List<SocketWrapper>();
        }

        public void Start()
        {
            _tcpListener.Start();
            AcceptClientsAsync();
        }

        public void Stop()
        {
            _tcpListener.Stop();
        }

#pragma warning disable 4014
        private async void AcceptClientsAsync()
        {
            await Task.Run(async () =>
            {
                while (true)
                {
                    var socket = await _tcpListener.AcceptSocketAsync();
                    if (_dictionary.Count == _maxSockets)
                    {
                        var oldestSocket = _connections.FirstOrDefault();
                        oldestSocket?.Destroy();
                        _connections.Remove(oldestSocket);
                        SocketWrapper temp;
                        _dictionary.TryRemove(oldestSocket.ClientId, out temp);
                        _numberOfConnections--;
                        _connections.First().SocketNumber = 1;
                        _dictionary.FirstOrDefault().Value.SocketNumber = 1;
                    }
                    var socketWrapper = new SocketWrapper(socket)
                    {
                        Name = "123",
                        SocketNumber = _numberOfConnections + 1,
                        ClientId = Guid.NewGuid().ToString(),
                    };
                    _numberOfConnections++;
                    _dictionary.TryAdd(socketWrapper.ClientId, socketWrapper);
                    _connections.Add(socketWrapper);
                    Task.Run(async () =>
                    {
                        while (true)
                        {
                            var data = socketWrapper.Receive();
                            if (!string.IsNullOrEmpty(data))
                            {
                                var jsonObject = JsonConvert.DeserializeObject<DataTransferModel>(data);
                                jsonObject.SocketId = socketWrapper.ClientId;
                                jsonObject.SocketNumber = socketWrapper.SocketNumber;
                                if (jsonObject.DataType.Equals("message"))
                                    UserMessageHandler?.Invoke(jsonObject);
                                HandleData(jsonObject);
                            }
                            await Task.Delay(TimeSpan.FromSeconds(0));
                        }
                    });
                }
            });

        }
#pragma warning restore 4014

        public void BroadcastMessage(string message)
        {
            foreach (var connection in _dictionary)
            {
                connection.Value.SendEncoded("broadcast", "title", message);
            }
        }

        private void HandleData(DataTransferModel jsonObject)
        {
            try
            {
                switch (jsonObject.DataType.ToLowerInvariant())
                {
                    case "broadcast":
                        BroadcastMessage(jsonObject.Data);
                        break;
                    case "guid":
                        SendGuid(jsonObject.SocketId);
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("exception");
            }
        }

        public void SendMessageById(string clientId, string title, string message)
        {
            SocketWrapper connection;
            _dictionary.TryGetValue(clientId, out connection);
            connection?.SendEncoded("message", title, message);
        }

        public void SendMessageBySocketNumber(int socketNumber, string title, string message)
        {
            var socketId = GetSocketId(socketNumber);
            SendMessageById(socketId, title, message);
        }

        public string GetSocketId(int playerNumber)
        {
            return _dictionary.FirstOrDefault(c => c.Value.SocketNumber == playerNumber).Value.ClientId;
        }

        public WebSocketWrapper GetSocketById(string socketId)
        {
            throw new NotImplementedException();
        }

        public void UpdateWebSocketApplicationId(string socketId, string applicationId)
        {
            throw new NotImplementedException();
        }

        public void AddSocket(WebSocketWrapper newSocket)
        {
            throw new NotImplementedException();
        }

        public void RemoveSocket(string oldSocketId)
        {
            throw new NotImplementedException();
        }

        public string GetApplicationIdFromSocketId(string socketId)
        {
            throw new NotImplementedException();
        }

        public void CloseAllSockets()
        {
            throw new NotImplementedException();
        }

        private void SendGuid(string clientId)
        {
            SocketWrapper connection;
            _dictionary.TryGetValue(clientId, out connection);
            connection?.SendEncoded("guid", "guid", clientId);
        }
    }
}
