using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace male.services.biz
{
  public abstract class EFBase
  {
    #region "Properties"

    [Key]
    [Column(Order = 100 /* Id will always be Last in a compound pk */)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    private PropertyInfo[] primaryKeys;
    private PropertyInfo[] PrimaryKeys
    {
      get
      {
        if (primaryKeys == null)
          primaryKeys = GetPrimaryKeys(this.GetType());
        return primaryKeys;
      }
    }

    #endregion

    #region "Methods"

    public List<EFBase> GetForeignKeyList(PropertyInfo fkPropertyInfo)
    {
      return GetForeignKeyList(this, fkPropertyInfo);
    }

    public abstract void Insert();

    // TODO: REFACTOR
    public IEnumerable LoadCollection(string collectionPropertyName)
    {
      using (var db = new Model1())
      {
        IEnumerable rootCollection = GetRootCollection(db, this.GetType());
        rootCollection.GetType().GetMethod("Attach").Invoke(rootCollection, new object[] { this });
        var dbCollectionEntry = db.Entry(this).Collection(collectionPropertyName);
        dbCollectionEntry.Load();
        IEnumerable childCollection = (IEnumerable)dbCollectionEntry.CurrentValue;
        childCollection.GetType().GetField("Parent").SetValue(childCollection, this);
        return childCollection;
      }
    }

    public void LoadProperty(string propertyName)
    {
      if (Id == 0) return;
      using (var db = new Model1())
      {
        IEnumerable rootCollection = GetRootCollection(db, this.GetType());
        rootCollection.GetType().GetMethod("Attach").Invoke(rootCollection, new object[] { this });
        this.GetType().GetProperty(propertyName).SetValue(this, db.Entry(this).GetDatabaseValues().GetValue<object>(propertyName));
      }
    }

    // TODO: REFACTOR
    public EFBase LoadReference(string referencePropertyName)
    {
      EFBase reference = (EFBase)RealType(this.GetType()).GetProperty(referencePropertyName).GetValue(this);
      if (reference != null) return reference;

      using (var db = new Model1())
      {
        IEnumerable rootCollection = GetRootCollection(db, this.GetType());
        rootCollection.GetType().GetMethod("Attach").Invoke(rootCollection, new object[] { this });
        var dbReferenceEntry = db.Entry(this).Reference(referencePropertyName);
        dbReferenceEntry.Load();
        reference = (EFBase)dbReferenceEntry.CurrentValue;
      }
      return reference;
    }

    public void Save()
    {
      if (Id == 0)
        Insert();
      else
        Update();
    }

    public abstract void Update();

    #endregion

    #region "Static Methods"
    public static List<EFBase> GetForeignKeyList(EFBase ancestor, PropertyInfo fkPropertyInfo)
    {
      IEnumerable col = null;
      string collectionPropertyName = fkPropertyInfo.Pluralize();
      foreach (var pi in GetPrimaryKeys(ancestor.GetType()).Where(pi => pi.Name != "Id").Reverse())
      {
        string ancestorPropertyName = pi.Name.StripEnd();
        ancestor = ancestor.LoadReference(ancestorPropertyName);
        var colPropertyInfo = ancestor.GetType().GetProperty(collectionPropertyName);
        if (colPropertyInfo != null)
        {
          col = (IEnumerable)colPropertyInfo.GetValue(ancestor);
          if (col == null)
            col = ancestor.LoadCollection(collectionPropertyName);
          break;
        }
      }
      return col.Cast<EFBase>().ToList();
    }

    public static PropertyInfo[] GetPrimaryKeys(Type type)
    {
      return (from pi in type.GetProperties() where pi.GetCustomAttributes(typeof(KeyAttribute), true).Any() select pi).ToArray();
    }
    public static IEnumerable GetRootCollection(Model1 db, Type type)
    {
      return GetRootCollection(db, type.Pluralize());
    }

    public static IEnumerable GetRootCollection(Model1 db, string collectionPropertyName)
    {
      PropertyInfo p = db.GetType().GetProperty(collectionPropertyName);
      return (IEnumerable)p.GetValue(db);
    }

    public static System.Type RealType(System.Type type)
    {
      return typeof(EFBase).Assembly.GetType(typeof(EFBase).Namespace + "." + type.Name.StripEnd());
    }

    public static IEnumerable RootCollection(Type type)
    {
      return RootCollection(type.Pluralize());
    }

    public static IEnumerable RootCollection(string collectionPropertyName)
    {
      using (var db = new Model1())
      {
        var col = GetRootCollection(db, collectionPropertyName);
        Type generic = typeof(List<>);
        Type constructed = generic.MakeGenericType(col.GetType().GetGenericArguments().First());
        var list = constructed.GetConstructor(new Type[] { col.GetType() }).Invoke(new object[] { col });
        return (IEnumerable)list;
      }
    }
    #endregion
  }
}