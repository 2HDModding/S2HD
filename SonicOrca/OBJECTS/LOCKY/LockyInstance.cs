// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.LOCKY.LockyInstance
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core.Objects.Base;
using SonicOrca.Geometry;

namespace SONICORCA.OBJECTS.LOCKY
{

    public class LockyInstance : Animal
    {
      public LockyInstance()
        : base("/ANIGROUP")
      {
        this.JumpVelocity = new Vector2(-10.0, -12.0);
      }
    }
}
