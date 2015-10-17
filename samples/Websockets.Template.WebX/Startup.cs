using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.Framework.DependencyInjection;
using Websockets.Template.CoreX;

namespace Websockets.Template.WebX
{
    public class Startup
    {
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app)
        {
            Console.WriteLine("lol");
            app.UseDefaultFiles();
            app.UseStaticFiles();
            var server = new SocketServer();
            server.Start();
            server.AcceptClientsAsync();
            //Task.Run(async () =>
            //{
            //    while (true)
            //    {
            //        await Task.Delay(TimeSpan.FromSeconds(8));
            //        server.BroadcastMessage("nothing");
            //    }
            //});
        }
    }
}
