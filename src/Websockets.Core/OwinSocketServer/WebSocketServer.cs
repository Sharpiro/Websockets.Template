using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Newtonsoft.Json;
using Websockets.Core.Models;

namespace Websockets.Core.OwinSocketServer
{
    public class WebSocketServer
    {
        private readonly ISocketHandler _socketHandler;
        private readonly ApplicationHandler _applicationHandler;

        public WebSocketServer(ISocketHandler socketHandler, ApplicationHandler applicationHandler)
        {
            _socketHandler = socketHandler ?? throw new ArgumentNullException(nameof(socketHandler));
            _applicationHandler = applicationHandler ?? throw new ArgumentNullException(nameof(applicationHandler));
        }

        private void HandleReceivedData(string messageSource, string socketId, int socketNumber)
        {
            if (messageSource.Equals("close"))
            {
                var appId = _socketHandler.GetApplicationIdFromSocketId(socketId);
                _applicationHandler.RemoveSocketFromApplication(socketId, appId);
                _socketHandler.RemoveSocket(socketId);
                return;
            }
            if (messageSource.Equals("keep-alive"))
            {
                _socketHandler.SendMessageById(socketId, "keep-alive", "keep-alive");
                return;
            }
            Debug.WriteLine($"Message: {messageSource}");
            var messageObject = JsonConvert.DeserializeObject<DataTransferModel>(messageSource);
            messageObject.SocketId = socketId;
            messageObject.SocketNumber = socketNumber;
            switch (messageObject.DataType.ToLowerInvariant())
            {
                case "broadcast":
                    _socketHandler.BroadcastMessage(messageObject.Data);
                    break;
                case "message":
                    _applicationHandler?.HandleMessage(messageObject);
                    break;
            }
        }

        public async Task<string> UpgradeToWebsocket(HttpContext context, IHttpUpgradeFeature upgradeFeature)
        {
            context.Features.Set<IHttpWebSocketFeature>(new UpgradeHandshake(context, upgradeFeature));
            var socket = await context.WebSockets.AcceptWebSocketAsync() as WebSocketWrapper;
            if (socket == null)
                throw new NullReferenceException("the socket returned was null!");
            _socketHandler.AddSocket(socket);
            return socket.Id;
        }

        public async Task ListenOnSocket(string socketId)
        {
            var socket = _socketHandler.GetSocketById(socketId);
            while (!socket.CancellationTokenSource.IsCancellationRequested)
            {
                try
                {
                    var result = await socket.Receive();
                    HandleReceivedData(result, socket.Id, socket.Number);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("err");
                }
            }
        }
    }
}