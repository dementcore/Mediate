using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Mediate.Samples.Shared.Hubs
{
    public class SignalRSampleHub : Hub
    {

        public async Task SendMessage(string message)
        {
            await Clients.Others.SendAsync("ReceiveMessage", message);
        }
    }
}
