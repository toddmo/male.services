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

    private void TestDataModel()
    {
      var service = new biz.Service() { Name = "Massage", Description = "Relaxing" };//
      service.Save();
      var member = new biz.Member() { FirstName = "Brad", LastName = "Pitt"};//
      member.Save();
      //var member = biz.Member.Collection[3];//
      var s = member.Services[20];
      s.Name = "Companionship2";
      member.FirstName = "Christopher";
      member.Services.AddRange(biz.Service.Collection);
      member.Save();
    }
  }
}
