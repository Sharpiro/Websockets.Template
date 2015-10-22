using Microsoft.AspNet.Builder;

namespace Websockets.Template.CoreX.OwinSocketServer
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseOwinSocketServer(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SocketServerMiddleware>();
        }

        public static IApplicationBuilder UseOwinSocketServer<TApp>(this IApplicationBuilder builder) where TApp: IWebSocketApplication
        {
            return builder.UseMiddleware<SocketServerMiddleware>(typeof(TApp));
        }
    }
}
