using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace WebSocketServer.Middleware
{
    public class WebSocketMiddleWare
    {
        private RequestDelegate _next;
        private readonly IConnnectionManager _manager;

        public WebSocketMiddleWare(RequestDelegate next,IConnnectionManager manager)
        {
            _next = next;
            _manager = manager;
        }   

        public async Task InvokeAsync(HttpContext context)
        {
            if(context.WebSockets.IsWebSocketRequest)
               {
                   var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                   var id = _manager.AddSocket(webSocket);

                   await SendIdAsync(webSocket,id);
                   Console.WriteLine("websocket connection added : "+ id);

                   await ReceiveMessage(webSocket,async (result,buffer) => {
                       if(result.MessageType == WebSocketMessageType.Text)
                       {
                           System.Console.WriteLine("Message Received");
                           var message = Encoding.UTF8.GetString(buffer,0,result.Count);
                           Console.WriteLine($"Message : {message}");
                           await RoutingMessage(message);
                           return;
                       }
                       else if(result.MessageType == WebSocketMessageType.Close)
                       {
                           System.Console.WriteLine("Connection Closed");
                           await webSocket.CloseAsync(result.CloseStatus.Value,result.CloseStatusDescription,CancellationToken.None);
                           return;                           
                       }
                   });
               }
               else
               {
                   await _next(context);
               }
        }

        private async Task ReceiveMessage(WebSocket socket, Action<WebSocketReceiveResult,byte []> handleMessage)
        {
            var buffer = new byte[1024*4];
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer),CancellationToken.None);
                handleMessage(result,buffer);
            }
        }

        private async Task SendIdAsync(WebSocket socket,string id)
        {
            var buffer = Encoding.UTF8.GetBytes("ConnectionId: "+id);
            await socket.SendAsync(buffer,WebSocketMessageType.Text,true,CancellationToken.None);
        }


        public async Task RoutingMessage(string message)
        {
            var obj = JsonConvert.DeserializeObject<Message>(message);

            if(Guid.TryParse(obj.To,out Guid id))
            {
                await _manager.GetSocket(id.ToString()).SendAsync(Encoding.UTF8.GetBytes(obj.Data),WebSocketMessageType.Text,true,CancellationToken.None);
            }
            else
            {
                System.Console.WriteLine("Broadcast");
                foreach (var socket in _manager.GetAllSockets())
                {
                    if(socket.Value.State == WebSocketState.Open)
                       await socket.Value.SendAsync(Encoding.UTF8.GetBytes(obj.Data),WebSocketMessageType.Text,true,CancellationToken.None);

                }
            }
             
        }
    }



    class Message
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Data { get; set; }
    }
}