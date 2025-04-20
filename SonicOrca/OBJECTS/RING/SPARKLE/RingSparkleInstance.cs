// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.RING.SPARKLE.RingSparkleInstance
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core.Objects.Base;

namespace SONICORCA.OBJECTS.RING.SPARKLE
{

    public class RingSparkleInstance : ParticleObject
    {
      public RingSparkleInstance()
        : base("/ANIGROUP")
      {
        this.AdditiveBlending = true;
        this.FilterMultiplier = 0.0;
      }
    }
}
