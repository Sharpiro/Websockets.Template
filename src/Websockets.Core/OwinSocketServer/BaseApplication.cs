using System;
using System.Collections.Concurrent;
using Websockets.Core.Models;

namespace Websockets.Core.OwinSocketServer
{
    public abstract class BaseApplication
    {
        public string Id { get; set; }
        public bool IsStarted { get; set; }
        public bool UpdatingRequest { get; set; }
        protected readonly ConcurrentDictionary<string, User> Users;
        protected int MaxUsers { get; set; } = 2;
        protected ISocketHandler SocketHandler;

        protected BaseApplication(ISocketHandler socketHandler)
        {
            SocketHandler = socketHandler;
            Users = new ConcurrentDictionary<string, User>();
        }

        public virtual void AddUser(WebSocketWrapper webSocket)
        {
            var newUser = new User
            {
                SocketId = webSocket.Id,
                SocketNumber = webSocket.Number,
                UserNumber = Users.Count + 1
            };
            Users.TryAdd(newUser.SocketId, newUser);
        }

        public virtual void RemoveUser(WebSocketWrapper webSocket)
        {
            Users.TryRemove(webSocket.Id, out User _);
        }

        public User GetUser(string socketId)
        {
            Users.TryGetValue(socketId, out User user);
            return user;
        }

        public bool IsFull()
        {
            if (Users.Count > MaxUsers)
                throw new Exception("too many users in app this shouldn't happen...");
            return Users.Count == MaxUsers;
        }

        public bool IsEmpty() => Users.Count == 0;

        public abstract void Start();
        public abstract void Stop();
        public abstract void HandleMessage(DataTransferModel m);
    }
}