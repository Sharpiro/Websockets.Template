using System;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Features;

namespace Websockets.Template.CoreX.OwinSocketServer
{
    public class UpgradeHandshake : IHttpWebSocketFeature
    {
        public bool IsWebSocketRequest { get; } = true;
        private HttpContext _context;
        private IHttpUpgradeFeature _upgradeFeature;

        public UpgradeHandshake(HttpContext context, IHttpUpgradeFeature upgradFeature)
        {
            _context = context;
            _upgradeFeature = upgradFeature;
        }

        public async Task<WebSocket> AcceptAsync(WebSocketAcceptContext acceptContext)
        {
            var clientKey = _context.Request.Headers["Sec-WebSocket-Key"].ToString();
            var hash = GetShaHash(clientKey);
            _context.Response.Headers["Sec-WebSocket-Accept"] = hash;
            var stream = await _upgradeFeature.UpgradeAsync();
            return new WebSocketWrapper(stream);
        }

        private string GetShaHash(string clientKey)
        {
            const string serverSecret = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
            var bytes = Encoding.UTF8.GetBytes(clientKey + serverSecret);
            var shaHashString = Convert.ToBase64String(SHA1.Create().ComputeHash(bytes));
            if (string.IsNullOrEmpty(shaHashString))
                throw new NullReferenceException("the sha string computed was null or empty.  This hash must have a value");
            return shaHashString;
        }
    }
}
