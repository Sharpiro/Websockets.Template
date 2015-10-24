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
        private readonly ISocketHandler _socketHandler;

        public ApplicationHandler(ISocketHandler socketHandler)
        {
            _applications = new ConcurrentDictionary<string, IWebSocketApplication>();
            _socketHandler = socketHandler;
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
                }
                //System.GC.Collect();
                Debug.WriteLine($"# of Apps: {_applications.Count}");
            }
        }

        private string AddApplication()
        {
            var appId = Guid.NewGuid().ToString();
            var newApp = new PokerApplication(_socketHandler)
            {
                Id = appId
            };
            _applications.TryAdd(appId, newApp);
            Debug.WriteLine($"# of Apps: {_applications.Count}");
            return appId;
        }

        public void HandleMessage(DataTransferModel messageObject)
        {
            if (messageObject.DataTitle.ToLowerInvariant().Equals("addplayer"))
            {
                var appId = AddSocketToApplication(messageObject);
                TryStartApplication(appId);
                _socketHandler.SendMessageById(messageObject.SocketId, "addplayer", "success");
            }
            else
            {
                GiveToApplication(messageObject);
            }
        }

        /// <summary>
        /// Tries to add a socket to an existing application.  If none are found it creates a new one and adds the socket.
        /// </summary>
        /// <param name="socketHandler"></param>
        /// <param name="messageObject"></param>
        /// <returns>The id of the application that received the socket</returns>
        private string AddSocketToApplication(DataTransferModel messageObject)
        {
            var openApplicationId = GetOpenApplicationId() ?? AddApplication();
            var currentApp = _applications[openApplicationId];
            currentApp.AddPlayer(messageObject);
            _socketHandler.UpdateWebSocketApplicationId(messageObject.SocketId, currentApp.Id);
            return currentApp.Id;
        }

        private void TryStartApplication(string applicationId)
        {
            var application = _applications[applicationId];
            if (application.IsFull() && !application.IsStarted)
                application.Start();
        }

        private string GetOpenApplicationId()
        {
            string openApplicationId = null;
            foreach (var application in _applications)
            {
                if (application.Value.IsFull()) continue;
                openApplicationId = application.Value.Id;
                break;
            }
            return openApplicationId;
        }

        private void GiveToApplication(DataTransferModel messageObject)
        {
            var socket = _socketHandler.GetSocketById(messageObject.SocketId);
            var currentAppId = socket.ApplicationId;
            //var currentAppId = webSocketServer.getApplicationIdFromSocketId(messageObject.SocketId);
            messageObject.ApplicationId = currentAppId;
            _applications[currentAppId].HandleMessage(messageObject);
        }

        public void RemoveSocketFromApplication(string socketId, string applicationId)
        {
            _applications[applicationId].RemovePlayer(socketId);
            _applications[applicationId].Stop();
        }
    }
}
