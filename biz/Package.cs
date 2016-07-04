using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace male.services.biz
{
  public class Package: EFObject<Package>
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public EFCollection<Promise> Promises { get; set; }
    public EFCollection<Purchase> Purchases { get; set; }
    public Constraint[] Constraints;
    public decimal Price { get; set; }

    [Key]
    [Column(Order = 1)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Member_Id { get; set; }

    public Member Member { get; set; }

    public override string ToString()
    {
      return Name;
    }
  }
}
