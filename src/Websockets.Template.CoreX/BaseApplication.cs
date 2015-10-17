using Websockets.Template.CoreX.Models;

namespace Websockets.Template.CoreX
{
    public abstract class BaseApplication
    {
        protected readonly ISocketServer _socketServer;

        protected BaseApplication(ISocketServer socketServer, int maxConnections)
        {
            _socketServer = socketServer;
            _socketServer.MaxConnections = maxConnections;
            _socketServer.UserMessageHandler = HandleMessage;
        }

        public void Start()
        {
            _socketServer.Start();
        }

        /// <summary>
        /// This function is automatically called by the socket server when a data type of "message" is received
        /// </summary>
        /// <param name="messageObject">The object containing message data</param>
        protected abstract void HandleMessage(DataTransferModel messageObject);
    }
}
