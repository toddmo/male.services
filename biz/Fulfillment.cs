using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace male.services.biz
{
  public class Fulfillment : EFObject<Fulfillment>
  {
    public enum FulfillmentStatuses {
      NotFulfilled,
      Success,
      NoShow
    }

    public Fulfillment()
    {
      Date = DateTime.MaxValue;
    }

    public Fulfillment(Purchase purchase, Promise promise):this()
    {
      Member_Id = promise.Member_Id;
      Package_Id = promise.Package_Id;
      Promise_Id = promise.Id;
      Promise = promise;
      Purchase_Id = purchase.Id;
      Purchase = purchase;
      Quantity = promise.Hours;
      Status = FulfillmentStatuses.NotFulfilled;
    }

    [Key]
    [Column(Order = 1)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Member_Id { get; set; }

    [Key]
    [Column(Order = 2)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Package_Id { get; set; }

    [Key]
    [Column(Order = 3)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Promise_Id { get; set; }

    public Promise Promise { get; set; }

    public int Purchase_Id { get; set; }

    public Purchase Purchase { get; set; }

    public DateTime Date { get; set; }
    public decimal Quantity { get; set; }
    public FulfillmentStatuses Status { get; set; }
  }
}
