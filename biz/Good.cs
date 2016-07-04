using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace male.services.biz
{
  public abstract class Good<T> : EFObject<T> where T : Good<T>
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public Units Unit { get; set; }

    [Key]
    [Column(Order = 1)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Member_Id { get; set; }

    public Member Member { get; set; }

    public EFCollection<Promise> Promises { get; set; }

    public override string ToString()
    {
      return Name;
    }
  }
}
