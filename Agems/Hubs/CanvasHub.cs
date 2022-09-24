using Microsoft.AspNetCore.SignalR;

namespace Agems.Hubs
{
    public struct Draw
    {
        public int X0 { get; set; }
        public int Y0 { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public object Color { get; set; }
    }

    public class CanvasHub : Hub
    {
        private static List<Draw> draws = new List<Draw>();
        public static int count  = 0;
        public async override Task OnConnectedAsync()
        {
            count++; // this I want to set in Application variable
            await Clients.All.SendAsync("Count", count);
            foreach (Draw draw in draws)
            {
                await Clients.All.SendAsync("ReceiveDraw", draw);
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            count--;
            await Clients.All.SendAsync("Count", count);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendDraw( int x0, int y0, int x, int y, int width, string color )
        {
            Draw draw = new Draw { X0 = x0, Y0 = y0, X = x, Y = y, Width = width, Color = color };
            draws.Add( draw );
            await Clients.All.SendAsync("ReceiveDraw", draw);
        }
        public async Task Clear()
        {
            draws.Clear();
            await Clients.All.SendAsync("Clear");
        }
    }
}
