using System.Net.Sockets;

namespace Websockets.Core.TcpListenerServer
{
    public class SocketClient
    {
        private readonly TcpClient _tcpClient;
        private NetworkStream _stream;

        public SocketClient()
        {
            _tcpClient = new TcpClient();
        }

        public void OpenStream()
        {
            _tcpClient.ConnectAsync("127.0.0.1", 8095);
            _stream = _tcpClient.GetStream();
        }

        public void CloseStream()
        {
            _stream.Dispose();
        }

        public void SendMessage(string message)
        {
            //var messageBytes = Encoding.UTF8.GetBytes($"{message}\r\n");
            //_stream = _tcpClient.GetStream();
            //_stream.Write(messageBytes, 0, messageBytes.Length);
            //var buffer = new byte[256];
            //_stream.Read(buffer, 0, buffer.Length);
            //var data = Encoding.UTF8.GetString(buffer).Trim('\0');
            //Debug.WriteLine($"Response: {data}");
        }

        public void Close()
        {
            _stream.Dispose();
        }
    }
}
