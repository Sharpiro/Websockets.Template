using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Features;
using Websockets.Template.CoreX.TcpListenerServer;

namespace Websockets.Template.CoreX.OwinSocketServer
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class SocketServerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly WebSocketServer _server;

        public SocketServerMiddleware(RequestDelegate next)
        {
            _next = next;
            _server = new WebSocketServer();
        }

        public async Task Invoke(HttpContext context)
        {
            var upgradeFeature = context.Features.Get<IHttpUpgradeFeature>();
            if (upgradeFeature.IsUpgradableRequest)
            {
                context.Features.Set<IHttpWebSocketFeature>(new UpgradeHandshake(context, upgradeFeature));
                var socket = await context.WebSockets.AcceptWebSocketAsync() as WebSocketWrapper;
                if (socket == null)
                    throw new NullReferenceException("the socket returned was null!");
                _server.AddSocket(socket);
                while (!socket.CancellationTokenSource.IsCancellationRequested)
                {
                    var result = await socket.Receive();
                    await socket.SendEncoded(result);
                }
            }
            await _next(context);
        }
    }
}
