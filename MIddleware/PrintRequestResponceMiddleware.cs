using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace WebSocketServer.Middleware
{
    public class PrintRequestResponceMiddleware{
        private RequestDelegate _next;

        public PrintRequestResponceMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {   
            PrintRequest(context);
            await _next(context);
            PrintResponce(context);

        }

        private void PrintRequest(HttpContext context)
        {
            System.Console.WriteLine($"Method {context.Request.Method}");
            System.Console.WriteLine($"Method {context.Request.Protocol}");

            if(context.Request.Headers != null)
            {
                foreach (var header in context.Request.Headers)
                {
                    System.Console.WriteLine($"{header.Key} : {header.Value}");
                }
            }
        }


        private void PrintResponce(HttpContext context)
        {
            var res = context.Response.Body;            
        }
    }
    public static class PrintRequestResponceMiddlewareExtention{

        public static IApplicationBuilder UsePrintRequestResponceMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<PrintRequestResponceMiddleware>();
        }
    }
}