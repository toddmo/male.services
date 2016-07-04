using male.services.biz;
using male.services.mvc;
using System.Collections.Generic;
using System.Web.Mvc;

using PayPal.Api;
using PayPal.Sample;
using male.services.mvc.Controllers;

namespace male.services.Controllers
{
  public class HomeController : BaseController
  {
    public ActionResult Index()
    {
      return View();
    }

    public ActionResult MemberList()
    {
      ViewBag.Message = "Members";
      return View(Member.Collection);
    }

    public ActionResult About()
    {
      //TestCreditCardPayment();
      ViewBag.Message = "Your profile information.";
      return View("About", new NavigationStack<Member>().ToList());
    }

    protected void TestCreditCardPayment()
    {
      var apiContext = Configuration.GetAPIContext();

      // First vault a credit card.
      var card = new CreditCard
      {
        expire_month = 05,
        expire_year = 2021,
        number = "4032033103647306",
        type = "visa",
        cvv2 = "874"
      };
      var createdCard = card.Create(apiContext);

      // Next, create the payment authorization using the vaulted card as the funding instrument.
      var payment = new Payment
      {
        intent = "authorize",
        payer = new Payer
        {
          payment_method = "credit_card",
          funding_instruments = new List<FundingInstrument>
        {
            new FundingInstrument
            {
                credit_card_token = new CreditCardToken
                {
                    credit_card_id = createdCard.id,
                    expire_month = createdCard.expire_month,
                    expire_year = createdCard.expire_year
                }
            }
        }
        },
        transactions = new List<Transaction>
        {
            new Transaction
            {
                amount = new Amount
                {
                    currency = "USD",
                    total = "1.00"
                },
                description = "This is the payment transaction description."
            }
        }
      };
      var createdPayment = payment.Create(apiContext);
    }

    public ActionResult Contact()
    {
      ViewBag.Message = "Your contact page.";

      return View();
    }

    [HttpPost]
    public ActionResult SubmitMembers(string command, EFCollection<Member> collection, List<NavigationEntry> navigationEntries)
    {
      return Submit("About", command, collection, navigationEntries);
    }

    [HttpPost]
    public ActionResult SubmitServices(string command, EFCollection<Service> collection, List<NavigationEntry> navigationEntries)
    {
      return Submit("About", command, collection, navigationEntries);
    }

    [HttpPost]
    public ActionResult SubmitProducts(string command, EFCollection<Product> collection, List<NavigationEntry> navigationEntries)
    {
      return Submit("About", command, collection, navigationEntries);
    }

    [HttpPost]
    public ActionResult SubmitPackages(string command, EFCollection<Package> collection, List<NavigationEntry> navigationEntries)
    {
      return Submit("About", command, collection, navigationEntries);
    }

    [HttpPost]
    public ActionResult SubmitPromises(string command, EFCollection<Promise> collection, List<NavigationEntry> navigationEntries)
    {
      return Submit("About", command, collection, navigationEntries);
    }

    [HttpPost]
    public ActionResult SubmitBreadCrumbs(string command, List<NavigationEntry> navigationEntries)
    {
      var navigationStack = new NavigationStack<Member>(command, navigationEntries);
      return View("About", navigationStack.ToList());
    }
    [HttpPost]
    public ActionResult SubmitCalendar(string command)
    {
      return View("About");
    }
    
  }
}