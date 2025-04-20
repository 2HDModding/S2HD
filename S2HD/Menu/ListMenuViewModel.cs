// Decompiled with JetBrains decompiler
// Type: S2HD.Menu.ListMenuViewModel
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using System.Collections.Generic;
using System.Collections.Immutable;

#nullable disable
namespace S2HD.Menu;

internal class ListMenuViewModel : IListMenuViewModel, IMenuViewModel
{
  public ImmutableArray<IMenuItem> Items { get; }

  public object Tag { get; }

  public int HighlightedIndex { get; set; }

  public ListMenuViewModel(IEnumerable<IMenuItem> items, object tag = null)
    : this(items.ToImmutableArray<IMenuItem>(), tag)
  {
  }

  public ListMenuViewModel(ImmutableArray<IMenuItem> items, object tag = null)
  {
    this.Items = items;
    this.Tag = tag;
  }

  public override string ToString()
  {
    return string.Join<IMenuItem>(", ", (IEnumerable<IMenuItem>) this.Items);
  }
}
