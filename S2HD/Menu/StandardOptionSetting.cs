// Decompiled with JetBrains decompiler
// Type: S2HD.Menu.StandardOptionSetting
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using System;
using System.Collections.Generic;

#nullable disable
namespace S2HD.Menu;

internal class StandardOptionSetting : ISpinnerSetting, ISetting
{
  private readonly string[] _valueStrings;
  private readonly Func<int> _getter;
  private readonly Action<int> _setter;

  public string Name { get; }

  public IReadOnlyList<string> Values => (IReadOnlyList<string>) this._valueStrings;

  public int SelectedIndex
  {
    get => this._getter();
    set => this._setter(value);
  }

  public StandardOptionSetting(string name, string[] values, Func<int> getter, Action<int> setter)
  {
    this.Name = name;
    this._valueStrings = values;
    this._getter = getter;
    this._setter = setter;
  }

  public override string ToString() => $"{this.Name}: {this.Values[this.SelectedIndex]}";
}
