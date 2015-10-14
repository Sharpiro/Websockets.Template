using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Websockets.Tempalte.Core;

[assembly: OwinStartup(typeof(Websockets.Template.Web.Startup))]

namespace Websockets.Template.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var server = new SocketServer();
            server.Start();
            server.AcceptClientsAsync();
            Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(8));
                server.BroadcastMessage("nothing");
            });
        }
    }
}
