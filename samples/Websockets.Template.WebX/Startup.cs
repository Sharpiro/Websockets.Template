using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.Framework.DependencyInjection;
using Websockets.Template.CoreX;
using Websockets.Template.CoreX.OwinSocketServer;

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
            
            //app.UseIISPlatformHandler();
            app.UseOwinSocketServer();
            //app.Use(async (context, next) =>
            //{
            //    if (context.WebSockets.IsWebSocketRequest)
            //    {
            //        var socket = await context.WebSockets.AcceptWebSocketAsync();
            //        var tokenSource = new CancellationTokenSource();
            //        while (!tokenSource.IsCancellationRequested)
            //        {
            //            var buffer = new byte[2048];
            //            var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), tokenSource.Token);
            //            var plainText = SocketWrapper.Decode(buffer, result.Count);
            //        }
            //    }
            //    await next();
            //});
            app.UseDefaultFiles();
            app.UseStaticFiles();
        }
    }
}
