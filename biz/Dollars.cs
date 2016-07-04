namespace male.services.biz
{
  public class Dollars : Quantity
  {
    public Dollars() : base(Units.USD)
    {

    }
    public Dollars(decimal amount) : base(Units.USD,amount)
    {

    }


  }
}
