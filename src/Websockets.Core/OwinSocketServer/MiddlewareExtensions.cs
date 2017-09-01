using Microsoft.AspNetCore.Builder;

namespace Websockets.Core.OwinSocketServer
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseOwinSocketServer(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SocketServerMiddleware>();
        }
    }
}