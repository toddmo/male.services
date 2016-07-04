namespace male.services.biz
{

  public class Member :  Person<Member>
  {
    public Client[] Clients;

    public EFCollection<Service> Services { get; set; }
     
    public EFCollection<Product> Products { get; set; }

    public EFCollection<Package> Packages { get; set; }
  
  }

}