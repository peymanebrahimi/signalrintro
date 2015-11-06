using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace WebApi.Hubs
{
    [HubName("myHub")]
    public class MyHub : Hub
    {
        public void Subscribe(string customerId)
        {
            Groups.Add(Context.ConnectionId, customerId);
        }

        public void UnSubscribe(string customerId)
        {
            Groups.Remove(Context.ConnectionId, customerId);
        }
    }
}