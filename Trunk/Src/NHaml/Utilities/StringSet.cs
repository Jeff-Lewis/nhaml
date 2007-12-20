using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace Mindscape.NHaml.Utilities
{
  [SuppressMessage("Microsoft.Naming", "CA1710")]
  public sealed class StringSet : Collection<string>
  {
    public StringSet()
    {
    }

    public StringSet(IEnumerable<string> items)
    {
      int i = 0;

      foreach (string item in items)
      {
        base.InsertItem(i++, item);
      }
    }

    protected override void InsertItem(int index, string item)
    {
      if (!Contains(item))
      {
        base.InsertItem(index, item);
      }
    }

    protected override void SetItem(int index, string item)
    {
    }
  }
}