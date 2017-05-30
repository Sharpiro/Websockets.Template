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
        protected readonly ConcurrentDictionary<string, User> Players;
        protected int MaxPlayers { get; set; } = 2;
        protected ISocketHandler SocketHandler;

        protected BaseApplication(ISocketHandler socketHandler)
        {
            SocketHandler = socketHandler;
            Players = new ConcurrentDictionary<string, User>();
        }

        public void AddPlayer(DataTransferModel messageObject)
        {
            var newPlayer = new User
            {
                SocketId = messageObject.SocketId,
                SocketNumber = messageObject.SocketNumber,
                UserNumber = Players.Count + 1
            };
            Players.TryAdd(newPlayer.SocketId, newPlayer);
        }

        public void RemovePlayer(string socketId)
        {
            User player;
            Players.TryRemove(socketId, out player);
        }

        public User GetPlayer(string socketId)
        {
            User player;
            Players.TryGetValue(socketId, out player);
            return player;
        }

        public bool IsFull()
        {
            if (Players.Count > MaxPlayers)
                throw new Exception("too many players in game this shouldn't happen...");
            return Players.Count == MaxPlayers;
        }

        public bool IsEmpty()
        {
            return Players.Count == 0;
        }

        public abstract void Start();
        public abstract void Stop();
        public abstract void HandleMessage(DataTransferModel m);
    }
}
