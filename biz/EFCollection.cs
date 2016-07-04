using System.Collections.Generic;

namespace male.services.biz
{
  public class EFCollection<T> : List<T> where T : EFBase
  {
    public EFCollection()
    {
    }

    public EFCollection(IEnumerable<T> collection)
    {
      AddRange(collection);
    }

    public EFBase Parent;

    public T Item(int Id)
    {
      return this.Find(o => o.Id == Id);
    }

    public void Save()
    {
      if (Parent != null)
      {
        Parent.GetType().GetProperty(typeof(T).Pluralize()).SetValue(Parent, this);
        Parent.Save();
      }
      else
        foreach (T item in this)
          item.Save();
    }
  }
}
