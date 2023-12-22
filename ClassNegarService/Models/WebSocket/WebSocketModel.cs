using System;

namespace ClassNegarService.Models.WebSocket
{
    public class WebSocketModel
    {
        public int UserId { get; set; }
        public System.Net.WebSockets.WebSocket Socket { get; set; }
    }
}

