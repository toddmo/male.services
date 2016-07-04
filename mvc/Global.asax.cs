using male.services.biz;
using male.services.mvc.Binders;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace male.services
{
  public class MvcApplication : System.Web.HttpApplication
  {
    protected void Application_Start()
    {
      AreaRegistration.RegisterAllAreas();
      FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
      RouteConfig.RegisterRoutes(RouteTable.Routes);
      BundleConfig.RegisterBundles(BundleTable.Bundles);
      ModelBinders.Binders.Add(typeof(Category), new CustomModelBinder());
    }
  }
}
