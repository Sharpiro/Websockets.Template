using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Websockets.Core.Models;

namespace Websockets.Core.TcpListenerServer
{
    public class SocketWrapper
    {
        public string Name { get; set; }
        public int SocketNumber { get; set; }
        public string ClientId { get; set; }
        public bool IsConnected { get; set; }

        private readonly Socket _socket;

        public SocketWrapper(Socket socket)
        {
            _socket = socket;
        }

        public void SendPlain(string plainData)
        {
            var bytes = Encoding.UTF8.GetBytes(plainData);
            _socket.Send(bytes);
        }

        public void SendEncoded(string dataType, string dataTitle, string data)
        {
            var dataObj = new DataTransferModel
            {
                DataType = dataType,
                DataTitle = dataTitle,
                Data = data
            };
            var encodedData = Encode(JsonConvert.SerializeObject(dataObj));
            var bytes = Convert.FromBase64String(encodedData);
            _socket.Send(bytes);
        }

        public string Receive()
        {
            var buffer = new byte[2048];
            var bufferLength = _socket.Receive(buffer);
            if (IsConnected)
            {
                return Decode(buffer, bufferLength);
            }
            var data = Encoding.UTF8.GetString(buffer, 0, bufferLength).Trim('\0');
            Connect(data);
            return null;
        }

        public void Destroy()
        {
            //_socket.Disconnect(false);
            _socket.Dispose();
        }

        private void Connect(string data)
        {
            if (!new Regex("^GET").IsMatch(data))
                throw new InvalidOperationException("The data provided cannot be used to create a websocket connection");
            var response = CompleteHandshake(data);
            SendPlain(response);
            IsConnected = true;
        }

        private string CompleteHandshake(string data)
        {
            const string serverSecret = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
            var clientKey = new Regex("Sec-WebSocket-Key: (.*)").Match(data).Groups[1].Value.Trim();
            var bytes = Encoding.UTF8.GetBytes(clientKey + serverSecret);
            var shaHashString = Convert.ToBase64String(SHA1.Create().ComputeHash(bytes));
            if (string.IsNullOrEmpty(shaHashString))
                throw new NullReferenceException("the sha string computed was null or empty.  This hash must have a value");
            var responseString = $"HTTP/1.1 101 Switching Protocols\r\n" +
                                 $"Connection: Upgrade\r\n" +
                                 $"Upgrade: websocket\r\n" +
                                 $"Sec-WebSocket-Accept: {shaHashString}\r\n\r\n";
            return responseString;
        }

        private string Encode(string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data).ToList();
            var length = (byte)bytes.Count;
            bytes.Insert(0, 129);
            if (bytes.Count <= 127)
            {
                bytes.Insert(1, length);
            }
            else if (bytes.Count > 127)
            {
                bytes.Insert(1, 126);
                bytes.Insert(2, (byte)((length >> 8) & 255));
                bytes.Insert(3, (byte)(length & 255));
            }
            var bytesString = Convert.ToBase64String(bytes.ToArray());
            return bytesString;
        }

        private string Decode(byte[] data, int dataLength)
        {
            if (data[1] - 128 > 125)
                throw new InvalidOperationException("Data is too long, not supported yet...");
            var decoded = new byte[dataLength - 6];
            var encoded = data.Skip(6).Take(decoded.Length).ToArray();
            var key = new byte[4] { data[2], data[3], data[4], data[5] };
            for (var i = 0; i < encoded.Length; i++)
            {
                decoded[i] = (byte)(encoded[i] ^ key[i % 4]);
            }
            var decodedMessage = Encoding.UTF8.GetString(decoded, 0, decoded.Length);
            Debug.WriteLine($"Decoded Message: {decodedMessage}");
            return decodedMessage;
        }
    }
}
