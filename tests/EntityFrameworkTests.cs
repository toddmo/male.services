using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using male.services.biz;

namespace male.services.Tests
{
  [TestClass]
  public class EntityFrameworkTests
  {
    [TestMethod]
    public void TestDataModel()
    {
      var service = new Service() { Name = "Massage", Description = "Relaxing" };//
      service.Save();
      var member = new Member() { FirstName = "Brad", LastName = "Pitt" };//
      member.Save();
      //var member = biz.Member.Collection[3];//
      var s = member.Services[20];
      s.Name = "Companionship2";
      member.FirstName = "Christopher";
      member.Services.AddRange(biz.Service.Collection);
      member.Save();
    }
  }
}
