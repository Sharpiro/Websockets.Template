using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;
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
            Purge();
        }

        private async void Purge()
        {
            while (true)
            {
                await Task.Delay(TimeSpan.FromSeconds(30));
                Debug.WriteLine("purging applications....");
                foreach (var dictionaryItem in _applications)
                {
                    var application = dictionaryItem.Value;
                    if (!application.IsEmpty()) continue;
                    IWebSocketApplication app;
                    _applications.TryRemove(application.Id, out app);
                    app = null;
                }
                //System.GC.Collect();
                Debug.WriteLine($"# of Apps: {_applications.Count}");
            }
        }

        private void AddApplication()
        {
            var appId = Guid.NewGuid().ToString();
            var newApp = new CardApplication
            {
                Id = appId
            };
            _applications.TryAdd(appId, newApp);
            Debug.WriteLine($"# of Apps: {_applications.Count}");
        }

        public void HandleMessage(WebSocketServer webSocketServer, DataTransferModel messageObject)
        {
            switch (messageObject.DataTitle.ToLowerInvariant())
            {
                case "addplayer":
                    AddPlayer(webSocketServer, messageObject);
                    webSocketServer.SendMessageById(messageObject.SocketId, "addplayer", "success");
                    break;
                case "update":
                    GiveToApplication(webSocketServer, messageObject);
                    break;
            }
        }

        private void AddPlayer(WebSocketServer webSocketServer, DataTransferModel messageObject)
        {
            foreach (var application in _applications)
            {
                if (application.Value.IsFull()) continue;
                application.Value.AddPlayer(messageObject);
                webSocketServer.UpdateWebSocketApplicationId(messageObject.SocketId, application.Key);
                return;
            }
            AddApplication();
            AddPlayer(webSocketServer, messageObject);
        }

        private void GiveToApplication(WebSocketServer webSocketServer, DataTransferModel messageObject)
        {
            var currentAppId = webSocketServer.getApplicationIdFromSocketId(messageObject.SocketId);
            messageObject.ApplicationId = currentAppId;
            _applications[currentAppId].HandleMessage(webSocketServer, messageObject);
        }

        public void RemoveSocketFromApplication(string socketId, string applicationId)
        {
            _applications[applicationId].RemovePlayer(socketId);
        }
    }
}
