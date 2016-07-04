using System.Collections;
using System.Data.Entity;
using System.Linq;
using System.Reflection;

namespace male.services.biz
{
  public abstract class EFObject<T> : EFBase where T : EFObject<T>
  {

    internal static readonly string DbSetProperyName = typeof(T).Pluralize();

    public static EFCollection<T> Collection
    {
      get
      {
        using (var db = new Model1())
        {
          PropertyInfo p = db.GetType().GetProperty(DbSetProperyName);
          DbSet<T> collection = (DbSet<T>)p.GetValue(db);
          var query = collection.AsQueryable();
          foreach (var pi in typeof(T).GetProperties().Where(pi => pi.PropertyType.IsGenericType))
          {
            query = query.Include(pi.Name);
          }
          var efc = new EFCollection<T>(query);
          //set the Parent for every collection
          foreach (var t in efc)
          {
            foreach (var pi in typeof(T).GetProperties().Where(pi => pi.PropertyType.IsGenericType))
            {
              var c = pi.GetValue(t);
              c.GetType().GetField("Parent").SetValue(c, t);
            }
          }
          return efc;
        }
      }
    }

    public void Delete()
    {
      using (var db = new Model1())
      {
        PropertyInfo p = db.GetType().GetProperty(DbSetProperyName);
        DbSet<T> collection = (DbSet<T>)p.GetValue(db);
        var dbItem = collection.First(o => o.Id == Id);
        collection.Remove(dbItem);
        db.SaveChanges();
      }
    }

    public override void Insert()
    {
      using (var db = new Model1())
      {
        PropertyInfo p = db.GetType().GetProperty(DbSetProperyName);
        DbSet<T> collection = (DbSet<T>)p.GetValue(db);
        collection.Add((T)this);
        db.SaveChanges();
      }
    }

    public override void Update()
    {
      using (var db = new Model1())
      {
        PropertyInfo p = db.GetType().GetProperty(DbSetProperyName);
        DbSet<T> collection = (DbSet<T>)p.GetValue(db);
        collection.Attach((T)this);
        MarkModified(db, this);
        db.SaveChanges();
      }
    }

    private void MarkModified(Model1 db, EFBase efObject)
    {
      db.Entry(efObject).State = efObject.Id != 0 ? EntityState.Modified : EntityState.Added;
      foreach (var pi in efObject.GetType().GetProperties().Where(pi => pi.PropertyType.IsGenericType))
      {
        var col = (IEnumerable)pi.GetValue(efObject);
        if (col != null)
          foreach (EFBase item in col)
            MarkModified(db, item);
      }
    }

  }
}
