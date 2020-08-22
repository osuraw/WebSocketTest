using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace WebSocketServer.Middleware
{
    public interface IConnnectionManager{
        string AddSocket(WebSocket socket);
        ConcurrentDictionary<string,WebSocket> GetAllSockets();
        WebSocket GetSocket(string id);
    }
    
    public class ConnnectionManager:IConnnectionManager
    {
        private ConcurrentDictionary<string,WebSocket> _socketCollection = new ConcurrentDictionary<string, WebSocket>();

        public ConcurrentDictionary<string,WebSocket> GetAllSockets()
        {
            return _socketCollection;
        }
        public string AddSocket(WebSocket socket)
        {
            var id = Guid.NewGuid().ToString();
            _socketCollection.TryAdd(id,socket);
            return id;
        }

        public  WebSocket GetSocket(string id)
        {
            _socketCollection.TryGetValue(id,out WebSocket socket);
            return socket;
        }
    }
}