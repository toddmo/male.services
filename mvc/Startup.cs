using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(male.services.Startup))]
namespace male.services
{
  public partial class Startup
  {
    public void Configuration(IAppBuilder app)
    {
      ConfigureAuth(app);

      var hubConfiguration = new HubConfiguration();
      hubConfiguration.EnableDetailedErrors = true;
      app.MapSignalR(hubConfiguration);
    }
  }
}
