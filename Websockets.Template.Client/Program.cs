using System;
using Websockets.Tempalte.Core;

namespace Websockets.Template.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var client = new SocketClient();
            client.SendMessage("hey there hi there ho there");
            Console.ReadLine();
        }
    }
}
