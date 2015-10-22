using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Features;
using Newtonsoft.Json;
using Websockets.Template.CoreX.Models;
using Websockets.Template.CoreX.TcpListenerServer;

namespace Websockets.Template.CoreX.OwinSocketServer
{
    public class WebSocketServer : ISocketServer
    {
        public int MaxSockets { get { return _maxSockets; } set { if (_numberOfConnections < 1) _maxSockets = value; } }
        private readonly ConcurrentDictionary<string, WebSocketWrapper> _sockets;
        private int _numberOfConnections;
        private int _maxSockets = 5;
        //private IWebSocketApplication _application;
        private readonly ApplicationHandler _applicationHandler;

        public WebSocketServer()
        {
            _applicationHandler = new ApplicationHandler();
            _sockets = new ConcurrentDictionary<string, WebSocketWrapper>();
        }

        public void AddSocket(WebSocketWrapper newSocket)
        {
            _numberOfConnections++;
            var socketId = Guid.NewGuid().ToString();
            newSocket.Id = socketId;
            newSocket.Number = _numberOfConnections;
            newSocket.CancellationTokenSource = new CancellationTokenSource();
            _sockets.TryAdd(socketId, newSocket);
        }

        public async Task ListenOnSocket(string socketId)
        {
            var socket = _sockets[socketId];
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

        public void UseApp<TApp>() where TApp : IWebSocketApplication
        {
            //_application = Activator.CreateInstance<TApp>();
        }

        public void UseApp(Type appType)
        {
            //_application = Activator.CreateInstance(appType) as IWebSocketApplication;
        }

        private void HandleReceivedData(string messageSource, string socketId, int socketNumber)
        {
            var messageObject = JsonConvert.DeserializeObject<DataTransferModel>(messageSource);
            messageObject.SocketId = socketId;
            messageObject.SocketNumber = socketNumber;
            switch (messageObject.DataType.ToLowerInvariant())
            {
                case "message":
                    _applicationHandler?.HandleMessage(this, messageObject);
                    break;
            }
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            foreach (var socket in _sockets)
            {
                socket.Value.Close();
            }
        }

        public void BroadcastMessage(string message)
        {
            foreach (var socket in _sockets)
            {
                socket.Value.SendEncoded("broadcast", "title", message);
            }
        }

        public void SendMessageBySocketNumber(int socketNumber, string title, string message)
        {
            var socketId = GetSocketId(socketNumber);
            SendMessageById(socketId, title, message);
        }

        public string GetSocketId(int socketNumber)
        {
            return _sockets.FirstOrDefault(c => c.Value.Number == socketNumber).Value.Id;
        }

        public WebSocketWrapper GetSocketById(string socketId)
        {
            return _sockets[socketId];
        }

        public void UpdateWebSocketApplicationId(string socketId, string applicationId)
        {
            _sockets[socketId].ApplicationId = applicationId;
        }

        public void SendMessageById(string socketId, string dataTitle, string data)
        {
            var socket = _sockets[socketId];
            socket?.SendEncoded("message", dataTitle, data);
        }

        public async Task<string> UpgradeToWebsocket(HttpContext context, IHttpUpgradeFeature upgradeFeature)
        {
            context.Features.Set<IHttpWebSocketFeature>(new UpgradeHandshake(context, upgradeFeature));
            var socket = await context.WebSockets.AcceptWebSocketAsync() as WebSocketWrapper;
            if (socket == null)
                throw new NullReferenceException("the socket returned was null!");
            AddSocket(socket);
            return socket.Id;
        }

        public string getApplicationIdFromSocketId(string socketId)
        {
            return _sockets[socketId].ApplicationId;
        }
    }
}