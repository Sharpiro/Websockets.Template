using System.Net.Sockets;

namespace Websockets.Template.CoreX
{
    public class SocketWrapper
    {
        public Socket Socket { get; set; }
        public string Name { get; set; }
        public int PlayerNumber { get; set; }
        public string GUID { get; set; }

        public void Send(byte[] bytes)
        {
            Socket.Send(bytes);
        }

        public int Receive(ref byte[] buffer)
        {
            return Socket.Receive(buffer);
        }

        public void Destroy()
        {
            Socket.Disconnect(false);
            Socket.Dispose();
        }
    }
}
