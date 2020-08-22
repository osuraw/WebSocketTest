using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using Microsoft.Extensions.DependencyInjection;

namespace WebSocketServer.Middleware
{
    public static class ConnnectionManagerService
    {
        public static IServiceCollection AddWebSocketManager(this IServiceCollection provider)
        {
            return provider.AddSingleton<IConnnectionManager,ConnnectionManager>();
        }
    }
}