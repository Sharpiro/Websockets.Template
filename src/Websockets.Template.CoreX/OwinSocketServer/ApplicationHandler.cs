using System;
using System.Collections.Concurrent;
using Websockets.Template.CoreX.CardApp;
using Websockets.Template.CoreX.Models;

namespace Websockets.Template.CoreX.OwinSocketServer
{
    public class ApplicationHandler
    {
        private readonly ConcurrentDictionary<string, IWebSocketApplication> _applications;

        public ApplicationHandler()
        {
            _applications = new ConcurrentDictionary<string, IWebSocketApplication>();
        }

        public void AddApplication()
        {
            var appId = Guid.NewGuid().ToString();
            _applications.TryAdd(appId, new CardApplication());
        }

        public void HandleMessage(WebSocketServer webSocketServer, DataTransferModel messageObject)
        {
            switch (messageObject.DataTitle.ToLowerInvariant())
            {
                case "addplayer":
                    AddPlayer(webSocketServer, messageObject);
                    break;
                case "update":
                    GiveToApplication(webSocketServer, messageObject);
                    break;
            }
        }

        public void AddPlayer(WebSocketServer webSocketServer, DataTransferModel messageObject)
        {
            foreach (var application in _applications)
            {
                if (!application.Value.IsFull())
                {
                    //var socket = webSocketServer.GetSocketById(messageObject.SocketId);
                    application.Value.AddPlayer(messageObject);
                    webSocketServer.UpdateWebSocketApplicationId(messageObject.SocketId, application.Key);
                    return;
                }
            }
            AddApplication();
            AddPlayer(webSocketServer, messageObject);
        }

        private void GiveToApplication(WebSocketServer webSocketServer, DataTransferModel messageObject)
        {
            var applicationId = webSocketServer.getApplicationIdFromSocketId(messageObject.SocketId);
            _applications[applicationId].HandleMessage(webSocketServer, messageObject);
        }
    }
}
