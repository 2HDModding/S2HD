// Decompiled with JetBrains decompiler
// Type: S2HD.Menu.OnOffOptionSetting
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using System;
using System.Collections.Generic;

namespace S2HD.Menu;

internal class OnOffOptionSetting : ISpinnerSetting, ISetting
{
  private static readonly string[] ValueStrings = new string[2]
  {
    "OFF",
    "ON"
  };
  private readonly Func<bool> _getter;
  private readonly Action<bool> _setter;

  public string Name { get; }

  public IReadOnlyList<string> Values => (IReadOnlyList<string>) OnOffOptionSetting.ValueStrings;

  public int SelectedIndex
  {
    get => !this.Value ? 0 : 1;
    set => this.Value = value != 0;
  }

  public bool Value
  {
    get => this._getter();
    set => this._setter(value);
  }

  public OnOffOptionSetting(string name, Func<bool> getter, Action<bool> setter)
  {
    this.Name = name;
    this._getter = getter;
    this._setter = setter;
  }

  public override string ToString() => $"{this.Name}: {this.Values[this.SelectedIndex]}";
}
