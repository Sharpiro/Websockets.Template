using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Websockets.Tempalte.Core
{
    public class SocketServer
    {
        public readonly TcpListener _tcpListener;

        public SocketServer()
        {
            _tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8095);
        }

        public void Start()
        {
            _tcpListener.Start();
        }

        public async Task AcceptClients()
        {
            var buffer = new byte[256];
            while (true)
            {
                try
                {
                    var client = await _tcpListener.AcceptTcpClientAsync();
                    var stream = client.GetStream();
                    await stream.ReadAsync(buffer, 0, buffer.Length);
                    var data = Encoding.UTF8.GetString(buffer).Trim('\0');
                    Console.WriteLine(data);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("exception");
                }
            }
        }
    }
}
