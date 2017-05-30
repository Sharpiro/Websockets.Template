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

        public void AddUser(DataTransferModel messageObject)
        {
            var newUser = new User
            {
                SocketId = messageObject.SocketId,
                SocketNumber = messageObject.SocketNumber,
                UserNumber = Users.Count + 1
            };
            Users.TryAdd(newUser.SocketId, newUser);
        }

        public void RemoveUser(string socketId)
        {
            Users.TryRemove(socketId, out User _);
        }

        public User GetUser(string socketId)
        {
            User user;
            Users.TryGetValue(socketId, out user);
            return user;
        }

        public bool IsFull()
        {
            if (Users.Count > MaxUsers)
                throw new Exception("too many users in app this shouldn't happen...");
            return Users.Count == MaxUsers;
        }

        public bool IsEmpty()
        {
            return Users.Count == 0;
        }

        public abstract void Start();
        public abstract void Stop();
        public abstract void HandleMessage(DataTransferModel m);
    }
}