﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Features;
using Websockets.Template.CoreX.CardApp;

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

        public SocketServerMiddleware(RequestDelegate next, Type appType)
        {
            _next = next;
            _server = new WebSocketServer();
            _server.UseApp(appType);
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
