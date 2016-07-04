using male.services.biz;
using System.Collections.Generic;
using System.Web.Mvc;

namespace male.services.mvc.Controllers
{
    public abstract class BaseController : Controller
    {
    protected ViewResult Submit<T>(string viewName, string command, EFCollection<T> collection, List<NavigationEntry> navigationEntries) where T : EFObject<T>
    {
      var navigationStack = new NavigationStack<T>(command, collection, navigationEntries);
      return View(viewName, navigationStack.ToList());
    }



  }
}