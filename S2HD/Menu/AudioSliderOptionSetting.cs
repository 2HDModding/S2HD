// Decompiled with JetBrains decompiler
// Type: S2HD.Menu.AudioSliderOptionSetting
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

#nullable disable
namespace S2HD.Menu;

internal class AudioSliderOptionSetting : IAudioSliderSetting, ISetting
{
  public string Name { get; }

  public double Value { get; set; }

  public AudioSliderOptionSetting(string name, double value)
  {
    this.Name = name;
    this.Value = value;
  }
}
