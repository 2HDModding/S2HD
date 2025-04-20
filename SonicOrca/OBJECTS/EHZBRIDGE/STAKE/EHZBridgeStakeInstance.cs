// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.EHZBRIDGE.STAKE.EHZBridgeStakeInstance
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core.Objects.Base;
using SonicOrca.Geometry;

namespace SONICORCA.OBJECTS.EHZBRIDGE.STAKE;

public class EHZBridgeStakeInstance : Scenery
{
  public EHZBridgeStakeInstance()
    : base("//ANIGROUP")
  {
    this.DesignBounds = new Rectanglei(-32, -32, 64 /*0x40*/, 64 /*0x40*/);
  }

  protected override void OnStart()
  {
    base.OnStart();
    this.Priority = 1280 /*0x0500*/;
  }
}
