using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;

namespace WebApi
{
    public static class SignalRConfig
    {
        public static void Register()
        {
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new SignalRContractResolver();
            var serializer = JsonSerializer.Create(settings);
            GlobalHost.DependencyResolver.Register(typeof(JsonSerializer), () => serializer);
        }
    }
}