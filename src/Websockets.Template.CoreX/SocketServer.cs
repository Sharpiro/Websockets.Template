using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Websockets.Template.CoreX
{
    public class SocketServer
    {
        private readonly IList<SocketWrapper> _sockets;
        private ConcurrentDictionary<string, SocketWrapper> _dictionary;
        private readonly TcpListener _tcpListener;
        private int _numberOfConnections = 0;

        public SocketServer()
        {
            _tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8095);
            _dictionary = new ConcurrentDictionary<string, SocketWrapper>();
            _sockets = new List<SocketWrapper>();
        }

        public void Start()
        {
            _tcpListener.Start();
        }

        public void Stop()
        {
            _tcpListener.Stop();
        }

#pragma warning disable 4014
        public async void AcceptClientsAsync()
        {
            await Task.Run(async () =>
            {
                while (true)
                {
                    var socket = await _tcpListener.AcceptSocketAsync();
                    if (_sockets.Count == 2)
                    {
                        var oldestSocket = _sockets.FirstOrDefault();
                        oldestSocket?.Destroy();
                        _sockets.Remove(oldestSocket);
                        _numberOfConnections--;
                        _sockets[0].PlayerNumber = 1;
                    }
                    var socketWrapper = new SocketWrapper
                    {
                        Name = "123",
                        PlayerNumber = _numberOfConnections + 1,
                        GUID = Guid.NewGuid().ToString(),
                        Socket = socket
                    };
                    _numberOfConnections++;
                    _dictionary.TryAdd("123", socketWrapper);
                    _sockets.Add(socketWrapper);
                    Task.Run(async () =>

                    {
                        while (true)
                        {
                            var buffer = new byte[2048];
                            var dataLength = socketWrapper.Receive(ref buffer);
                            var data = Encoding.UTF8.GetString(buffer).Trim('\0');
                            if (new Regex("^GET").IsMatch(data))
                            {
                                var response = CompleteHandshake(data);
                                if (!string.IsNullOrEmpty(response))
                                    socketWrapper.Send(Encoding.UTF8.GetBytes(response));
                            }
                            else
                            {
                                var message = HandleMessage(data, buffer, dataLength);
                                socketWrapper.Send(Convert.FromBase64String(message));
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
            foreach (var socket in _sockets)
            {
                var tempMessage = Encode(message);
                socket.Send(Convert.FromBase64String(tempMessage));
            }
        }

        public async void AcceptClients()
        {
            try
            {
                var buffer = new byte[2048];
                var socket = await _tcpListener.AcceptSocketAsync();
                while (true)
                {
                    var dataLength = socket.Receive(buffer);
                    var data = Encoding.UTF8.GetString(buffer).Trim('\0');
                    if (new Regex("^GET").IsMatch(data))
                    {
                        var response = CompleteHandshake(data);
                        if (!string.IsNullOrEmpty(response))
                            socket.Send(Encoding.UTF8.GetBytes(response));
                    }
                    else
                    {
                        var message = HandleMessage(data, buffer, dataLength);
                        socket.Send(Convert.FromBase64String(message));
                    }
                    buffer = new byte[2048];
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("exception");
            }
        }

        private string Decode(byte[] data, int dataLength)
        {
            if (data[1] - 128 > 128)
                throw new InvalidOperationException("Data is too long, not supported yet...");
            var decoded = new byte[dataLength - 6];
            var encoded = data.Skip(6).Where(d => d != 0).ToArray();
            var key = new byte[4] { data[2], data[3], data[4], data[5] };
            for (var i = 0; i < encoded.Length; i++)
            {
                decoded[i] = (byte)(encoded[i] ^ key[i % 4]);
            }
            //var decodedMessage = Encoding.UTF8.GetString(decoded);
            var decodedMessage = Encoding.UTF8.GetString(decoded);
            Debug.WriteLine($"Decoded Message: {decodedMessage}");
            return decodedMessage;
        }

        private string Encode(string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data).ToList();
            var length = (byte)bytes.Count;
            bytes.Insert(0, 129);
            bytes.Insert(1, length);
            var bytesString = Convert.ToBase64String(bytes.ToArray());
            return bytesString;
        }

        private string HandleMessage(string data, byte[] buffer, int dataLength)
        {
            var plainText = Decode(buffer, dataLength);
            Debug.WriteLine(plainText);
            var response = Encode(plainText);
            return response;
        }

        private string CompleteHandshake(string data)
        {
            const string serverSecret = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
            var clientKey = new Regex("Sec-WebSocket-Key: (.*)").Match(data).Groups[1].Value.Trim();
            var bytes = Encoding.UTF8.GetBytes(clientKey + serverSecret);
            var shaHashString = Convert.ToBase64String(SHA1.Create().ComputeHash(bytes));
            var responseString = $"HTTP/1.1 101 Switching Protocols\r\n" +
                                 $"Connection: Upgrade\r\n" +
                                 $"Upgrade: websocket\r\n" +
                                 $"Sec-WebSocket-Accept: {shaHashString}\r\n\r\n";
            return responseString;
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
