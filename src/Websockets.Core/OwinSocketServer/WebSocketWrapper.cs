using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Websockets.Core.Models;

namespace Websockets.Core.OwinSocketServer
{
    public class WebSocketWrapper : WebSocket
    {
        public override WebSocketCloseStatus? CloseStatus { get; }
        public override string CloseStatusDescription { get; }
        public override string SubProtocol { get; }
        public override WebSocketState State { get; }
        public CancellationTokenSource CancellationTokenSource { get; set; }
        public string Id { get; set; }
        public int Number { get; set; }
        public string ApplicationId { get; set; }
        private readonly Stream _stream;

        public WebSocketWrapper(Stream stream)
        {
            _stream = stream;
        }

        public Task<string> Receive()
        {
            try
            {
                var buffer = new byte[2048];
                var bufferSegment = new ArraySegment<byte>(buffer);
                var bufferLength = _stream.Read(bufferSegment.Array, 0, bufferSegment.Array.Length);
                string plainText;
                var firstByte = buffer.FirstOrDefault();
                if (firstByte.Equals(136))
                {
                    plainText = "close";
                }
                else if (firstByte.Equals(138))
                {
                    plainText = "keep-alive";
                }
                else if (firstByte.Equals(129))
                {
                    plainText = Decode(buffer, bufferLength);
                }
                else
                {
                    throw new InvalidDataException("the first byte was unknown");
                }
                return Task.FromResult(plainText);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Socket error");
            }
            return null;
        }

        public void SendEncoded(string dataType, string dataTitle, string data)
        {
            var dataObj = new DataTransferModel
            {
                DataType = dataType,
                DataTitle = dataTitle,
                Data = data,
                SocketId = Id,
                SocketNumber = Number,
                ApplicationId = ApplicationId
            };
            var encodedString = Encode(JsonConvert.SerializeObject(dataObj));
            var encodedBytes = Convert.FromBase64String(encodedString);
            _stream.Write(encodedBytes, 0, encodedBytes.Length);
        }

        public void Close()
        {
            CancellationTokenSource.Cancel();
            _stream.Dispose();
        }

        public override void Abort()
        {
            throw new NotImplementedException();
        }

        public override Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task CloseOutputAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }

        public override Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> bufferSegment, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private string Decode(byte[] data, int dataLength)
        {
            if (dataLength < 5)
            {
                SendEncoded("keep-alive", "keep-alive", "keep-alive");
                return null;
                //throw new Exception("Error in Decode: Data is shorter than 5...");
            }
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
            return decodedMessage;
        }

        private string Encode(string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data).ToList();
            var length = bytes.Count;
            bytes.Insert(0, 129);
            if (bytes.Count <= 127)
            {
                bytes.Insert(1, (byte)length);
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
    }
}
