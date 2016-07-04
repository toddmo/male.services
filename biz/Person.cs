namespace male.services.biz
{
  public abstract class Person<T> : EFObject<T> where T : Person<T>
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public override string ToString()
    {
      return $"{FirstName} {LastName}";
    }


  }
}
