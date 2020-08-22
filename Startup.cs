using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using WebSocketServer.Middleware;


namespace WebSocketServer
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddWebSocketManager();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
           app.UsePrintRequestResponceMiddleware(); 
           app.UseWebSockets();
           app.UseWebSocketServer();
           app.Run( async context => {
               Console.WriteLine("==================End of pipe========================");
               await context.Response.WriteAsync("Server Reaponse");
           });
        }    
    }
}
