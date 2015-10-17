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

namespace Websockets.Template.CoreX
{
    public abstract class BaseSocketServer
    {
        protected Action<DataTransferModel> Handler { get; set; }
        protected readonly IList<SocketWrapper> _connections;
        protected readonly ConcurrentDictionary<string, SocketWrapper> _dictionary;
        protected readonly TcpListener _tcpListener;
        protected int _numberOfConnections;

        protected BaseSocketServer()
        {
            _tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8095);
            _dictionary = new ConcurrentDictionary<string, SocketWrapper>();
            _connections = new List<SocketWrapper>();
        }

        protected void Start()
        {
            _tcpListener.Start();
        }

        protected void Stop()
        {
            _tcpListener.Stop();
        }

#pragma warning disable 4014
        protected async void AcceptClientsAsync()
        {
            await Task.Run(async () =>
            {
                while (true)
                {
                    var socket = await _tcpListener.AcceptSocketAsync();
                    if (_connections.Count == 2)
                    {
                        var oldestSocket = _connections.FirstOrDefault();
                        oldestSocket?.Destroy();
                        _connections.Remove(oldestSocket);
                        _numberOfConnections--;
                        _connections[0].PlayerNumber = 1;
                    }
                    var socketWrapper = new SocketWrapper(socket)
                    {
                        Name = "123",
                        PlayerNumber = _numberOfConnections + 1,
                        ClientId = Guid.NewGuid().ToString(),
                    };
                    _numberOfConnections++;
                    _dictionary.TryAdd("123", socketWrapper);
                    _connections.Add(socketWrapper);
                    Task.Run(async () =>
                    {
                        while (true)
                        {
                            var data = socketWrapper.Receive();
                            if (!string.IsNullOrEmpty(data))
                            {
                                HandleMessage(data, socketWrapper.ClientId);
                            }
                            await Task.Delay(TimeSpan.FromSeconds(0));
                        }
                    });
                }
            });

        }
#pragma warning restore 4014

        protected void BroadcastMessage(string message)
        {
            foreach (var connection in _connections)
            {
                connection.SendEncoded("broadcast", "title", message);
            }
        }

        private void HandleMessage(string data, string clientId)
        {
            try
            {
                var jsonObject = JsonConvert.DeserializeObject<DataTransferModel>(data);
                jsonObject.ClientId = clientId;
                Handler?.Invoke(jsonObject);
                switch (jsonObject.DataType.ToLowerInvariant())
                {
                    case "broadcast":
                        BroadcastMessage(jsonObject.Data);
                        break;
                    case "guid":
                        SendGuid(clientId);
                        break;
                }
                Debug.WriteLine(data);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("exception");
            }
        }

        protected void SendMessage(string clientId, string title, string message)
        {
            foreach (var connection in _connections)
            {
                if (connection.ClientId == clientId)
                {
                    connection.SendEncoded("message", title, message);
                }
            }
        }

        private void SendGuid(string clientId)
        {
            foreach (var connection in _connections)
            {
                if (connection.ClientId == clientId)
                {
                    connection.SendEncoded("guid", "guid", clientId);
                }
            }
        }

        private string HandleUpdate(string data)
        {
            //var bytes = new byte[5] { 129, 3, 77, 68, 78 };
            //var baseString = Convert.ToBase64String(bytes);
            //var testString = Convert.FromBase64String(baseString);
            return data;
        }
    }
}
