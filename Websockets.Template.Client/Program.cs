using System;
using Websockets.Tempalte.Core;

namespace Websockets.Template.Client
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var client = new SocketClient();
            client.OpenStream();
            client.SendMessage("hey there hi there ho there");
            client.SendMessage("a second message");
            client.SendMessage("and a third");
            client.CloseStream();
            Console.ReadLine();
        }
    }
}
