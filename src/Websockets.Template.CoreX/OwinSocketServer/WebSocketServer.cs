using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Features;
using Newtonsoft.Json;
using Websockets.Template.CoreX.Models;

namespace Websockets.Template.CoreX.OwinSocketServer
{
    public class WebSocketServer
    {
        private readonly WebSocketHandler _socketHandler;
        private readonly ApplicationHandler _applicationHandler;

        public WebSocketServer()
        {
            _applicationHandler = new ApplicationHandler();
            _socketHandler = new WebSocketHandler();
        }

        private void HandleReceivedData(string messageSource, string socketId, int socketNumber)
        {
            if (messageSource.Equals("close"))
            {
                _socketHandler.RemoveSocket(socketId);
                var appId = _socketHandler.GetApplicationIdFromSocketId(socketId);
                _applicationHandler.RemoveSocketFromApplication(socketId, appId);
                return;
            }
            if (messageSource.Equals("keep-alive"))
            {
                _socketHandler.SendMessageById(socketId, "keep-alive", "keep-alive");
                return;
            }
            Debug.WriteLine($"Message: {messageSource}");
            var socket = _socketHandler.GetSocketById(socketId);
            var messageObject = JsonConvert.DeserializeObject<DataTransferModel>(messageSource);
            messageObject.SocketId = socketId;
            messageObject.SocketNumber = socketNumber;
            switch (messageObject.DataType.ToLowerInvariant())
            {
                case "broadcast":
                    _socketHandler.BroadcastMessage(messageObject.Data);
                    break;
                case "message":
                    _applicationHandler?.HandleMessage(_socketHandler, messageObject);
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

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            _socketHandler.CloseAllSockets();
        }

        public void UseApp<TApp>() where TApp : IWebSocketApplication
        {
            //_application = Activator.CreateInstance<TApp>();
        }

        public void UseApp(Type appType)
        {
            //_application = Activator.CreateInstance(appType) as IWebSocketApplication;
        }
    }
}