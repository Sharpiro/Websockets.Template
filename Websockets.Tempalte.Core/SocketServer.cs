using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Websockets.Tempalte.Core
{
    public class SocketServer
    {
        private readonly IList<Socket> _sockets;
        private readonly TcpListener _tcpListener;

        public SocketServer()
        {
            _tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8095);
            _sockets = new List<Socket>();
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
                    _sockets.Add(socket);
                    Task.Run(async () =>

                    {
                        while (true)
                        {
                            var buffer = new byte[2048];
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
                var tempMessage = HandleUpdate(message);
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
                Console.WriteLine("exception");
            }
        }

        private string Decode(byte[] data, int dataLength)
        {
            var decoded = new byte[3];
            var encoded = new byte[3] { data[dataLength - 3], data[dataLength - 2], data[dataLength - 1] };
            var key = new byte[4] { data[2], data[3], data[4], data[5] };
            for (var i = 0; i < encoded.Length; i++)
            {
                decoded[i] = (byte)(encoded[i] ^ key[i % 4]);
            }
            var decodedMessage = Encoding.UTF8.GetString(decoded);
            Trace.WriteLine($"Decoded Message: {decodedMessage}");
            return decodedMessage;
        }

        private string HandleMessage(string data, byte[] buffer, int dataLength)
        {
            var stringData = Decode(buffer, dataLength);
            Trace.WriteLine(stringData);
            var response = HandleUpdate(data);
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
            var bytes = new byte[5] { 129, 3, 77, 68, 78 };
            var baseString = Convert.ToBase64String(bytes);
            var testString = Convert.FromBase64String(baseString);
            return baseString;
        }
    }
}
