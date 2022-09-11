using Microsoft.AspNetCore.SignalR;

namespace Agems.Hubs
{
    public class CanvasHub : Hub
    {

        public async Task SendDraw(int x, int y, int w, object color, bool isdraw )
        {
            await Clients.All.SendAsync("ReceiveDraw", x, y, w, color, isdraw);
        }
        public async Task Clear()
        {
            await Clients.All.SendAsync("Clear");
        }
    }
}
