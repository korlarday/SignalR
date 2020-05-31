using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace SignalRServer.Hubs
{
    public class ChatHub : Hub
    {
        public override Task OnConnectedAsync(){
            Console.WriteLine("Connection Established" + Context.ConnectionId);
            Clients.Client(Context.ConnectionId).SendAsync("RecieveConnId", Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public async Task SendMessageAsync(string message){
            var routeOb = JsonConvert.DeserializeObject<dynamic>(message);
            string toClient = routeOb.To;
            string msg = routeOb.Message;
            Console.WriteLine("Message recieved on: " + Context.ConnectionId);

            if(toClient == string.Empty)
            {
                await Clients.All.SendAsync("RecieveMessage", msg);
            }
            else
            {
                Console.WriteLine(toClient);
                await Clients.Client(toClient).SendAsync("RecieveMessage", msg);
            }
        }
    }
}