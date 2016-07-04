using male.services.biz;
using System.ComponentModel;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace male.services.mvc.Binders
{
  public class CustomModelBinder : DefaultModelBinder
  {

    HttpRequestBase request;

    public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    {
      request = controllerContext.HttpContext.Request;
      return base.BindModel(controllerContext, bindingContext);
    }

    protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor)
    {
      if (propertyDescriptor.PropertyType == typeof(byte[]))
      {
        HttpPostedFileWrapper httpPostedFile = (HttpPostedFileWrapper)request.Files[$"{bindingContext.ModelName}.{propertyDescriptor.Name}"];
        if (httpPostedFile.ContentLength > 0)
        {
          using (MemoryStream ms = new MemoryStream())
          {
            httpPostedFile.InputStream.CopyTo(ms);
            propertyDescriptor.SetValue(bindingContext.Model, ms.ToArray());
          }
        }
        else
        {
          base.BindProperty(controllerContext, bindingContext, bindingContext.ModelType.GetProperty("Id").PropertyDescriptor());
          ((EFBase)bindingContext.Model).LoadProperty(propertyDescriptor.Name);
        }
      }
      else
        base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
    }
  }
}