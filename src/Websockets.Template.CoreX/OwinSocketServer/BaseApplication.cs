using System;
using System.Collections.Concurrent;
using Websockets.Template.CoreX.CardApp;
using Websockets.Template.CoreX.Models;

namespace Websockets.Template.CoreX.OwinSocketServer
{
    public abstract class BaseApplication
    {
        public string Id { get; set; }
        public bool IsStarted { get; set; }
        protected readonly ConcurrentDictionary<string, Player> Players;
        protected int MaxPlayers { get; set; } = 2;

        protected BaseApplication()
        {
            Players = new ConcurrentDictionary<string, Player>();
        }

        public void AddPlayer(DataTransferModel messageObject)
        {
            var newPlayer = new Player
            {
                SocketId = messageObject.SocketId,
                SocketNumber = messageObject.SocketNumber,
                PlayerNumber = Players.Count + 1
            };
            Players.TryAdd(newPlayer.SocketId, newPlayer);
        }

        public void RemovePlayer(string socketId)
        {
            Player player;
            Players.TryRemove(socketId, out player);
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
    }
}
