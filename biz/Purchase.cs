using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace male.services.biz
{
  public class Purchase : EFObject<Purchase>
  {
    public enum PurchaseStatuses
    {
      Created,
      Funded,
      Complete
    }

    public Purchase()
    {
    }

    public Purchase(Package package, Client client)
    {
      Member_Id = package.Member_Id;
      Package_Id = package.Id;
      Client_Id = client.Id;
      Client = client;
      Price = package.Price;
      Date = DateTime.Now;
      Package = package;
      Status = biz.Purchase.PurchaseStatuses.Funded;
      Fulfillments = new EFCollection<Fulfillment>(from Promise promise in package.Promises select new Fulfillment(this, promise));
    }

    [Key]
    [Column(Order = 1)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Member_Id { get; set; }
    [Key]
    [Column(Order = 2)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Package_Id { get; set; }

    public int Client_Id { get; set; }

    public Client Client { get; set; }
    public Package Package { get; set; }
    public decimal Price { get; set; }
    public DateTime Date { get; set; }

    public PurchaseStatuses Status { get; set; }

    public EFCollection<Fulfillment> Fulfillments { get; set; }
  }
}
