namespace ClassNegarService.Services.WebSocket
{
    using System;
    using System.Net.WebSockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using ClassNegarService.Models.WebSocket;

    public class WebSocketService
    {
        private readonly List<WebSocketModel> _webSockets = new();

        public async Task HandleWebSocketAsync(HttpContext context, int userId)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();

                _webSockets.RemoveAll(w => w.UserId == userId);
                _webSockets.Add(new WebSocketModel { Socket = webSocket, UserId = userId });
                await Receive(webSocket, (result, buffer) =>
                {
                });
            }
            else
            {
                context.Response.StatusCode = 400;
            }
        }

        private async Task Receive(WebSocket webSocket, Action<WebSocketReceiveResult, byte[]> handleMessage)
        {
            var buffer = new byte[1024 * 4];
            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                handleMessage(result, buffer);
            }
        }

        public async Task SendToSocketAsync(string message, int userId)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            var segment = new ArraySegment<byte>(buffer);

            foreach (var socket in _webSockets)
            {
                if (socket.UserId == userId)
                {
                    if (socket.Socket.State == WebSocketState.Open)
                        await socket.Socket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
    }

}

