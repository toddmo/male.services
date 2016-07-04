namespace male.services.biz
{
  public class Category:EFObject<Category>
  {
    public string Name { get; set; }
    public string Description { get; set; }

    public byte[] Image { get; set; }
  }
}
