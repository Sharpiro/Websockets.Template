﻿using Websockets.Template.CoreX.Models;

namespace Websockets.Template.CoreX.TcpListenerServer
{
    public abstract class BaseApplication
    {
        protected readonly ISocketServer SocketServer;

        protected BaseApplication(ISocketServer socketServer, int maxConnections)
        {
            SocketServer = socketServer;
            SocketServer.MaxSockets = maxConnections;
            //SocketServer.UserMessageHandler = HandleMessage;
        }

        public void Start()
        {
            SocketServer.Start();
        }

        /// <summary>
        /// This function is automatically called by the socket server when a data type of "message" is received
        /// </summary>
        /// <param name="messageObject">The object containing message data</param>
        protected abstract void HandleMessage(DataTransferModel messageObject);
    }
}
