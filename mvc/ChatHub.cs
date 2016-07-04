using Microsoft.AspNet.SignalR;
using male.services.biz;

namespace male.services.mvc
{
  public class ChatHub : Hub
  {

    public void Send(string userName, string message)
    {
      var m = new CustomMessage() { UserName = userName, Message = message };
      m.Save();
      // Call the broadcastMessage method to update clients.
      Clients.All.broadcastMessage(m);
    }
  }
}
