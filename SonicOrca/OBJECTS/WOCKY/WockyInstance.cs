// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.WOCKY.WockyInstance
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core.Objects.Base;
using SonicOrca.Geometry;

namespace SONICORCA.OBJECTS.WOCKY;

public class WockyInstance : Animal
{
  public WockyInstance()
    : base("/ANIGROUP")
  {
    this.JumpVelocity = new Vector2(-10.0, -5.0);
  }
}
