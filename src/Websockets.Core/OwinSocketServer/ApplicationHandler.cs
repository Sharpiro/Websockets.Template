using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;
using Websockets.Core.Models;

namespace Websockets.Core.OwinSocketServer
{
    public class ApplicationHandler
    {
        private readonly ConcurrentDictionary<string, BaseApplication> _applications;
        private readonly ISocketHandler _socketHandler;
        private readonly Task _purgeTask;

        public ApplicationHandler(ISocketHandler socketHandler)
        {
            _applications = new ConcurrentDictionary<string, BaseApplication>();
            _socketHandler = socketHandler;
            _purgeTask = Purge();
        }

        private async Task Purge()
        {
            while (true)

            {
                await Task.Delay(TimeSpan.FromSeconds(30));
                Debug.WriteLine("purging applications....");
                foreach (var dictionaryItem in _applications)
                {
                    var application = dictionaryItem.Value;
                    if (!application.IsEmpty()) continue;
                    _applications.TryRemove(application.Id, out BaseApplication app);
                }
                //System.GC.Collect();
                Debug.WriteLine($"# of Apps: {_applications.Count}");
            }
        }

        private string AddApplication()
        {
            var appType = typeof(MessagingApplication);
            var instance = Activator.CreateInstance(appType, _socketHandler) as BaseApplication;
            if (instance == null) throw new InvalidCastException($"The application provided does not inherit from '{nameof(BaseApplication)}'");
            instance.Id = Guid.NewGuid().ToString();
            _applications.TryAdd(instance.Id, instance);
            Debug.WriteLine($"# of Apps: {_applications.Count}");
            return instance.Id;
        }

        public void HandleMessage(DataTransferModel messageObject)
        {
            if (messageObject.DataTitle.ToLowerInvariant().Equals("adduser"))
            {
                var appId = AddSocketToApplication(messageObject);
                TryStartApplication(appId);
                _socketHandler.SendMessageById(messageObject.SocketId, "adduser", "success");
            }
            else
            {
                GiveToApplication(messageObject);
            }
        }

        private string AddSocketToApplication(DataTransferModel messageObject)
        {
            var openApplicationId = GetOpenApplicationId() ?? AddApplication();
            var currentApp = _applications[openApplicationId];
            var socket = _socketHandler.GetSocketById(messageObject.SocketId);
            socket.ApplicationId = currentApp.Id;
            currentApp.AddUser(socket);
            return currentApp.Id;
        }

        private void TryStartApplication(string applicationId)
        {
            var application = _applications[applicationId];
            if (!application.IsFull() && !application.IsStarted)
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

        public void RemoveSocketFromApplication(WebSocketWrapper socket)
        {

            var applicationExists = _applications.TryGetValue(socket.ApplicationId, out BaseApplication application);
            if (!applicationExists) return;

            application.RemoveUser(socket);
            application.Stop();
        }
    }
}
