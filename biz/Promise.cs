using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace male.services.biz
{
  public class Promise : EFObject<Promise>
  {
    public int Sequence { get; set; }

    public decimal Hours { get; set; }

    [Key]
    [Column(Order = 1)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Member_Id { get; set; }
    [Key]
    [Column(Order = 2)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Package_Id { get; set; }

    public int Product_Id { get; set; }

    public int Service_Id { get; set; }

    public Package Package { get; set; }

    public virtual Service Service { get; set; }

    public Product Product { get; set; }

    public EFCollection<Fulfillment> Fulfillments { get; set; }

    public Constraint[] Constraints;
    //public Dollars Allocation { get; set; }
  }
}
