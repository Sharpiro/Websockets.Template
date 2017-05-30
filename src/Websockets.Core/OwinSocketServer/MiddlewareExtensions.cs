using Microsoft.AspNetCore.Builder;

namespace Websockets.Core.OwinSocketServer
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseOwinSocketServer(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SocketServerMiddleware>();
        }

        public static IApplicationBuilder UseOwinSocketServer<TApp>(this IApplicationBuilder builder) where TApp : BaseApplication
        {
            return builder.UseMiddleware<SocketServerMiddleware>(typeof(TApp));
        }
    }
}
