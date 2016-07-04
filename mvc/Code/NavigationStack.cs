using male.services.biz;
using System.Collections.Generic;
using System.Linq;

namespace male.services.mvc
{

  public class NavigationStack<T> : Stack<NavigationEntry> where T : EFObject<T>
  {
    public enum CommandTypes
    {
      None,
      Save,
      Delete,
      Add,
      Children,
      Parent,
      Ancestor
    }

    public NavigationStack() : this(CommandTypes.None.ToString(), EFObject<T>.Collection, new List<NavigationEntry>())
    {
    }

    public NavigationStack(string command, List<NavigationEntry> navigationEntries) 
    {
      Load(navigationEntries);
      Execute(command);
    }

    public NavigationStack(string command, EFCollection<T> collection, List<NavigationEntry> navigationEntries)
    {
      Load(collection, navigationEntries);
      Execute(command, collection);
    }

    private void Load(EFCollection<T> collection, List<NavigationEntry> navigationEntries)
    {
      foreach (NavigationEntry navigationEntry in navigationEntries.Skip(1).Reverse())
      {
        Push(navigationEntry);
      }
      var current = navigationEntries.FirstOrDefault();
      if(current != null)
      {
        current.Collection = collection;
      }
      else
        current = new NavigationEntry(collection);
      Push(current);
    }

    private void Load(List<NavigationEntry> navigationEntries)
    {
      foreach (NavigationEntry navigationEntry in navigationEntries.ToArray())
      {
        Push(navigationEntry);
      }
    }

    private void Execute(string command, EFCollection<T> collection = null)
    {
      string[] commandParts = command.Split(':');
      CommandTypes commandType = (CommandTypes)System.Enum.Parse(typeof(CommandTypes), commandParts[0], true);
      string collectionPropertyName = null;
      T RowItem = null;
      switch (commandType)
      {
        case CommandTypes.Children:
          collectionPropertyName = commandParts[1];
          RowItem = collection.Item(int.Parse(commandParts[2]));
          Push(new NavigationEntry(RowItem.LoadCollection(collectionPropertyName)));
          break;
        case CommandTypes.Parent:
          Pop();
          break;
        case CommandTypes.Ancestor:
          int lastIndex = int.Parse(commandParts[1]);
          int index = Count - 1;
          while (index > lastIndex)
          {
            Pop();
            index--;
          }
            
          return;
        case CommandTypes.Delete:
          RowItem = collection.Item(int.Parse(commandParts[1]));
          break;
        case CommandTypes.Save:
          RowItem = collection.Item(int.Parse(commandParts[1]));
          break;
      }

      switch (commandType)
      {
        case CommandTypes.None:
        case CommandTypes.Add:
          break;
        default:
          collection.Remove(collection.Last());
          break;
      }

      switch (commandType)
      {
        case CommandTypes.Delete:
          collection.Remove(RowItem);
          RowItem.Delete();
          break;
        case CommandTypes.Add:
        case CommandTypes.Save:
          RowItem.Save();
          break;
      }
    }

    public NavigationEntry Current
    {
      get
      {
        return Peek();
      }
    }

    public List<NavigationEntry> ToList()
    {
      return new List<NavigationEntry>(this);
    }
  }

}