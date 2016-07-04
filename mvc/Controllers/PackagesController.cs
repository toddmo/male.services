using male.services.biz;
using System;
using System.Web.Mvc;

namespace male.services.mvc.Controllers
{
  public class PackagesController : Controller
  {

    public ActionResult List()
    {
      ViewBag.Message = "Packages";
      return View(Package.Collection);
    }

    public ActionResult Purchase(int id)
    {
      ViewBag.Message = "Purchase Confirmation";
      var package = Package.Collection.Item(id);
      var client = Client.Collection.Item(1);
      //var p = biz.PayPal.GetPurchaseInformation(Request);
      Purchase purchase = new Purchase(package, client);
      purchase.Save();
      return View(purchase);
    }

    public ActionResult Schedule(int id)
    {
      Fulfillment fulfillment = Fulfillment.Collection.Item(id);
      return View(fulfillment);
    }

    public ActionResult SubmitCalendar(string command)
    {
      string[] commandParts = command.Split(':');

      switch (commandParts[0])
      {
        case "DayView":
          DateTime date = DateTime.Parse(commandParts[1]);
          return View("ScheduleDay");
          break;

      }
      return View();
    }
  }
}