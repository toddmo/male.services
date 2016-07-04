namespace male.services.biz
{
  public class CustomMessage : EFObject<CustomMessage>
  {
    public string UserName { get; set; }
    public string Message { get; set; }
  }
}
