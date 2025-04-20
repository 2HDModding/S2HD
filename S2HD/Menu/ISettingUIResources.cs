// Decompiled with JetBrains decompiler
// Type: S2HD.Menu.ISettingUIResources
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Audio;
using SonicOrca.Graphics;

#nullable disable
namespace S2HD.Menu;

internal interface ISettingUIResources
{
  Font Font { get; }

  ITexture AudioSliderEmptyTexture { get; }

  ITexture AudioSliderSilverTexture { get; }

  ITexture AudioSliderGoldTexture { get; }

  ITexture SelectionBar { get; set; }

  Sample NavigateSample { get; set; }

  Sample ConfirmSample { get; set; }

  Sample CancelSample { get; set; }
}
