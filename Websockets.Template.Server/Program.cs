using System;
using Websockets.Tempalte.Core;

namespace Websockets.Template.Server
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var server = new SocketServer();
            server.Start();
            server.AcceptClients();
            Console.ReadLine();
            server.Stop();
        }
    }
}
