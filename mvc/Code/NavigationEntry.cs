using male.services.biz;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Collections;
using System;
using System.Diagnostics;

namespace male.services.mvc
{

  public class NavigationEntry
  {
    public NavigationEntry()
    {
      parentKeys = new Dictionary<string, int>();
    }

    public NavigationEntry(IEnumerable collection) : this()
    {
      Collection = collection;
    }

    public string ControllerAction { get; private set; }
    public string Label { get; private set; }
    public string Value;

    private IEnumerable collection;
    public IEnumerable Collection
    {
      get
      {
        return collection;
      }
      set
      {
        //if (collection != null && Enumerable.SequenceEqual<EFBase>(value.Cast<EFBase>(), collection.Cast<EFBase>())) return;
        Debug.Assert(value != null);
        if (collection != null && value.Cast<EFBase>().Count() == collection.Cast<EFBase>().Count()) return;
        collection = value;
        CollectionItemType = collection.GetType().GetGenericArguments().First();
        Parent = GetParent();
      }
    }

    private Type collectionItemType;
    public Type CollectionItemType
    {
      get
      {
        return collectionItemType;
      }
      private set
      {
        if (collectionItemType != null) return;
        collectionItemType = value;
        CollectionPropertyName = collectionItemType.Pluralize();
      }
    }

    private string collectionPropertyName;
    public string CollectionPropertyName
    {
      get
      {
        return collectionPropertyName;
      }
      set
      {
        Debug.Assert(value != null);
        collectionPropertyName = value;
        SetCollection(collectionPropertyName, ParentTypeName);
        ControllerAction = "Submit" + collectionPropertyName;
        Label = GetLabel();
      }
    }

    private EFBase parent;
    public EFBase Parent
    {
      get
      {
        return parent;
      }
      private set
      {
        if (value == null || parent != null) return;
        parent = value;
        Collection = Parent.LoadCollection(CollectionPropertyName);
        ParentType = GetParentType(parent);
        Label = GetLabel();
        ParentKeys = (from PropertyInfo p
                      in ParentType.GetProperties()
                      where p.GetCustomAttributes(typeof(KeyAttribute), true).Any()
                      select p).ToDictionary(p => p.Name, p => (int)p.GetValue(Parent));
        if (collection != null)
        {
          var fi = collection.GetType().GetField("Parent");
          if (fi != null)
            fi.SetValue(Collection, Parent);
        }
      }
    }

    private Type parentType;
    public Type ParentType
    {
      get
      {
        return parentType;
      }
      private set
      {
        if (value == parentType) return;
        parentType = value;
        ParentTypeName = parentType.FullName.StripEnd();
        Parent = GetParent();
      }
    }

    private string parentTypeName;
    public string ParentTypeName
    {
      get
      {
        return parentTypeName;
      }
      set
      {
        SetCollection(CollectionPropertyName, value);
        if (value == parentTypeName) return;
        parentTypeName = value;
        ParentType = GetParentType(parentTypeName);
      }
    }

    private Dictionary<string, int> parentKeys;
    public Dictionary<string, int> ParentKeys
    {
      get
      {
        return parentKeys;
      }
      set
      {
        if (parentKeys.Keys.Contains("Id") && Parent != null) return;
        parentKeys = value;
        Parent = GetParent();
      }
    }

    private string GetLabel()
    {
      var label = $"{CollectionPropertyName}";
      if (Parent != null)
        label = $"{CollectionPropertyName} of {Parent.ToString()}";
      return label;
    }

    private EFBase GetParent()
    {
      EFBase parent = null;
      if (Collection != null)
      {
        var fi = collection.GetType().GetField("Parent");
        if (fi != null)
          parent = (EFBase)fi.GetValue(collection);
      }
      if (parent == null)
      {
        if (ParentType != null && ParentKeys.Any())
        {
          var parentCollection = (IEnumerable)ParentType.GetProperty("Collection", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy).GetValue(null, null);
          parent = (EFBase)parentCollection.GetType().GetMethod("Item").Invoke(parentCollection, new object[] { ParentKeys["Id"] });
        }
      }
      return parent;
    }

    private Type GetParentType(string name)
    {
      return typeof(EFBase).Assembly.GetType(name.StripEnd());
    }

    private Type GetParentType(EFBase parent)
    {
      // parent might be inside a dynamic proxy assembly; need to convert
      return typeof(EFBase).Assembly.GetType($"{typeof(EFBase).Namespace}.{parent.GetType().Name.StripEnd()}");
    }

    private void SetCollection(string collectionPropertyName, string parentTypeName)
    {
      if (collectionPropertyName != null && parentTypeName == null)
      {
        // it's a root collection
        Collection = EFBase.RootCollection(collectionPropertyName);
      }
    }

    public override string ToString()
    {
      return Label;
    }
  }
}