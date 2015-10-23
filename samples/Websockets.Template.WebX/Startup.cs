using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;
using Websockets.Template.CoreX;
using Websockets.Template.CoreX.CardApp;
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
            app.Map(new PathString("/socket"), builder =>
            {
                builder.UseOwinSocketServer<CardApplication>();

            });
            app.UseDefaultFiles();
            app.UseStaticFiles();
        }
    }
}
