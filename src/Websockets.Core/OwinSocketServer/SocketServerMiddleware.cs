using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace Websockets.Core.OwinSocketServer
{
    public class SocketServerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly WebSocketServer _server;

        public SocketServerMiddleware(RequestDelegate next)
        {
            _next = next;
            var socketHandler = new WebSocketHandler();
            var applicationHandler = new ApplicationHandler(socketHandler);
            _server = new WebSocketServer(socketHandler, applicationHandler);
        }

        public async Task Invoke(HttpContext context)
        {
            var upgradeFeature = context.Features.Get<IHttpUpgradeFeature>();
            if (upgradeFeature.IsUpgradableRequest)
            {
                var socketId = await _server.UpgradeToWebsocket(context, upgradeFeature);
                await _server.ListenOnSocket(socketId);

            }
            await _next(context);
        }
    }
}