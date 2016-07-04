using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace male.services.biz
{
  public class PayPalPurchaseInformation
  {
    public bool Success;
    public string TransactionID;
    public decimal AmountPaid; // d
    public string DeviceID; // d
    public string PayerEmail; // d
    public string Item;
    public int ItemNumber;
  }
  public class PayPal
  {
    public static PayPalPurchaseInformation GetPurchaseInformation(HttpRequestBase request)
    {
      PayPalPurchaseInformation payPalPurchaseInformation = new PayPalPurchaseInformation();

      // Receive IPN request from PayPal and parse all the variables returned
      var formVals = new Dictionary<string, string>();
      formVals.Add("cmd", "_notify-synch"); //notify-synch_notify-validate
      // https://www.paypal.com/cgi-bin/customerprofileweb?cmd=_profile-website-payments
      //formVals.Add("at", "5eIcjmaf9J_y_Ah4O3cBBMvapEe6UxDqBe-qvgpxGNSHu4Nz0XINhV8oDU4");//live
      formVals.Add("at", "7l9nWMGNRc5pN8skxkPhhj5Gr6CMD8TLW2_gZVJwKqasnScIgVVggZ6D1Iy");//sandbox
      formVals.Add("tx", request["tx"]);

      // if you want to use the PayPal sandbox change this from false to true
      string response = GetPayPalResponse(request, formVals, true);

      if (response.Contains("SUCCESS"))
      {
        payPalPurchaseInformation.Success = true;
        payPalPurchaseInformation.TransactionID = GetPDTValue(response, "txn_id"); // txn_id //d
        Decimal.TryParse(GetPDTValue(response, "mc_gross"), System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out payPalPurchaseInformation.AmountPaid); 
        payPalPurchaseInformation.DeviceID = GetPDTValue(response, "custom"); // d
        payPalPurchaseInformation.PayerEmail = GetPDTValue(response, "payer_email"); // d
        payPalPurchaseInformation.Item = GetPDTValue(response, "item_name");
        payPalPurchaseInformation.ItemNumber = int.Parse(GetPDTValue(response, "item_number")); 
      }
      return payPalPurchaseInformation;
    }

    static string GetPayPalResponse(HttpRequestBase request, Dictionary<string, string> formVals, bool useSandbox)
    {

      string paypalUrl = useSandbox ? "https://www.sandbox.paypal.com/cgi-bin/webscr"
          : "https://www.paypal.com/cgi-bin/webscr";

      HttpWebRequest req = (HttpWebRequest)WebRequest.Create(paypalUrl);
      req.Method = "POST";
      req.ContentType = "application/x-www-form-urlencoded";

      ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
           | SecurityProtocolType.Tls11
           | SecurityProtocolType.Tls12
           | SecurityProtocolType.Ssl3;

      byte[] param = request.BinaryRead(request.ContentLength);
      string strRequest = Encoding.ASCII.GetString(param);

      StringBuilder sb = new StringBuilder();
      sb.Append(strRequest);

      foreach (string key in formVals.Keys)
      {
        sb.AppendFormat("&{0}={1}", key, formVals[key]);
      }
      strRequest += sb.ToString();
      req.ContentLength = strRequest.Length;

      //for proxy
      //WebProxy proxy = new WebProxy(new Uri("http://urlort#");
      //req.Proxy = proxy;
      //Send the request to PayPal and get the response
      string response = "";
      using (StreamWriter streamOut = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII))
      {

        streamOut.Write(strRequest);
        streamOut.Close();
        using (StreamReader streamIn = new StreamReader(req.GetResponse().GetResponseStream()))
        {
          response = streamIn.ReadToEnd();
        }
      }

      return response;
    }
    static string GetPDTValue(string pdt, string key)
    {

      string[] keys = pdt.Split('\n');
      string thisVal = "";
      string thisKey = "";
      foreach (string s in keys)
      {
        string[] bits = s.Split('=');
        if (bits.Length > 1)
        {
          thisVal = bits[1];
          thisKey = bits[0];
          if (thisKey.Equals(key, StringComparison.InvariantCultureIgnoreCase))
            break;
        }
      }
      return thisVal;

    }
  }
}
