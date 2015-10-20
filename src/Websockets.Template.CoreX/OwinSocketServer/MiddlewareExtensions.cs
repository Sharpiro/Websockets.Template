using Microsoft.AspNet.Builder;

namespace Websockets.Template.CoreX.OwinSocketServer
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseOwinSocketServer(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SocketServerMiddleware>();
        }
    }
}
