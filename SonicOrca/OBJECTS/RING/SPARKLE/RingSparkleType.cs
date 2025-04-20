// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.RING.SPARKLE.RingSparkleType
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Objects.Metadata;

namespace SONICORCA.OBJECTS.RING.SPARKLE
{

    [SonicOrca.Core.Objects.Metadata.Name("Ring sparkle")]
    [Description("Ring sparkle from Sonic 1")]
    [SonicOrca.Core.Objects.Metadata.Classification(ObjectClassification.Particle)]
    [ObjectInstance(typeof (RingSparkleInstance))]
    public class RingSparkleType : ObjectType
    {
      [Dependency]
      public const string AnimationGroupResourceKey = "/ANIGROUP";
    }
}
