namespace male.services.biz
{
  public class Client : Person<Client>
  {
    public EFCollection<Purchase> Purchases { get; set; }
  }
}
