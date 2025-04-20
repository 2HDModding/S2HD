// Decompiled with JetBrains decompiler
// Type: S2HD.Menu.SettingUIResources
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Audio;
using SonicOrca.Extensions;
using SonicOrca.Graphics;
using SonicOrca.Resources;

#nullable disable
namespace S2HD.Menu;

internal class SettingUIResources : IControlResources, ISettingUIResources
{
  [ResourcePath("SONICORCA/FONTS/IMPACT/REGULAR")]
  public Font Font { get; set; }

  [ResourcePath("SONICORCA/MENU/OPTIONS/AUDIOSLIDER/EMPTY")]
  public ITexture AudioSliderEmptyTexture { get; set; }

  [ResourcePath("SONICORCA/MENU/OPTIONS/AUDIOSLIDER/SILVER")]
  public ITexture AudioSliderSilverTexture { get; set; }

  [ResourcePath("SONICORCA/MENU/OPTIONS/AUDIOSLIDER/GOLD")]
  public ITexture AudioSliderGoldTexture { get; set; }

  [ResourcePath("SONICORCA/SOUND/NAVIGATE/CURSOR")]
  public Sample NavigateSample { get; set; }

  [ResourcePath("SONICORCA/SOUND/NAVIGATE/YES")]
  public Sample ConfirmSample { get; set; }

  [ResourcePath("SONICORCA/SOUND/NAVIGATE/NO")]
  public Sample CancelSample { get; set; }

  [ResourcePath("SONICORCA/MENU/GAMEPAD/A")]
  public ITexture ButtonA { get; set; }

  [ResourcePath("SONICORCA/MENU/GAMEPAD/B")]
  public ITexture ButtonB { get; set; }

  [ResourcePath("SONICORCA/MENU/OPTIONS/V2/UI/SELECTION/BAR")]
  public ITexture SelectionBar { get; set; }

  public void PushDependencies(ResourceSession resourceSession)
  {
    resourceSession.PushDependenciesByAttribute((object) this);
  }

  public void FetchResources(ResourceTree resourceTree)
  {
    resourceTree.FullfillLoadedResourcesByAttribute((object) this);
  }
}
