using Websockets.Tempalte.Core;

namespace Websockets.Template.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var server = new SocketServer();
            server.Start();
            server.AcceptClients().Wait();
        }
    }
}
