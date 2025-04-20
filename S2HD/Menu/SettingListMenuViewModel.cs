// Decompiled with JetBrains decompiler
// Type: S2HD.Menu.SettingListMenuViewModel
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using System.Collections.Generic;
using System.Collections.Immutable;

namespace S2HD.Menu;

internal class SettingListMenuViewModel : ISettingListMenuViewModel, IMenuViewModel
{
  public ImmutableArray<ISetting> Items { get; }

  public object Tag { get; }

  public SettingListMenuViewModel(IEnumerable<ISetting> items, object tag = null)
    : this(items.ToImmutableArray<ISetting>(), tag)
  {
  }

  public SettingListMenuViewModel(ImmutableArray<ISetting> items, object tag = null)
  {
    this.Items = items;
    this.Tag = tag;
  }
}
