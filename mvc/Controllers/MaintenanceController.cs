using male.services.biz;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace male.services.mvc.Controllers
{
  public class MaintenanceController : BaseController
  {
    // GET: Maintenance
    public ActionResult Index()
    {
      return View(new NavigationStack<Category>().ToList());
    }

    [HttpPost]
    public ActionResult SubmitCategories(string command, EFCollection<Category> collection, List<NavigationEntry> navigationEntries, HttpPostedFileBase image)
    {
      return Submit("Index", command, collection, navigationEntries);
    }
  }
}