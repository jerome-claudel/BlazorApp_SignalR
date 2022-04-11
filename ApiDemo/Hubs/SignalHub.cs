using ApiDemo.Models;
using Microsoft.AspNetCore.SignalR;

namespace ApiDemo.Hubs
{
    public class SignalHub : Hub
    {
        public void BroadcastEmployee(Employee emp)
        {
            Clients.All.SendAsync("ReceiveEmployee", emp);
        }

        public void BroadcastMessage(string message)
        {
            Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
