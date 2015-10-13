using System.Net.Sockets;
using System.Text;

namespace Websockets.Tempalte.Core
{
    public class SocketClient
    {
        public readonly TcpClient _tcpClient;

        public SocketClient()
        {
            _tcpClient = new TcpClient("127.0.0.1", 8095);
        }

        public void SendMessage(string message)
        {
            var messageBytes = Encoding.UTF8.GetBytes(message);
            var stream = _tcpClient.GetStream();
            stream.Write(messageBytes, 0, messageBytes.Length);
            stream.Close();
        }

        public void Close()
        {
            _tcpClient.Close();
        }
    }
}
