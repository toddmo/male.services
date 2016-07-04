namespace male.services.biz
{
  public enum Units
  {
    Each,
    Hour,
    USD
  }

  public class Quantity
  {
    public Quantity(Units unit)
    {
      Unit = unit;
    }

    public Quantity(Units unit, decimal amount):this(unit)
    {
      Amount = amount;
    }

    public readonly Units Unit;
    public decimal Amount;
  }
}
