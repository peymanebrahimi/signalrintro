using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup(typeof(WebApi.Startup))]

namespace WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
            
            app.UseWebApi(config);
            SignalRConfig.Register();
            app.MapSignalR();
        }
    }
}
