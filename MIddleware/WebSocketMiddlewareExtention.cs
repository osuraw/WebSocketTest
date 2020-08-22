using Microsoft.AspNetCore.Builder;

namespace WebSocketServer.Middleware
{
    public static class WebSocketMiddlewareExtention
    {
        public static IApplicationBuilder UseWebSocketServer(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<WebSocketMiddleWare>();
        }
    }
}