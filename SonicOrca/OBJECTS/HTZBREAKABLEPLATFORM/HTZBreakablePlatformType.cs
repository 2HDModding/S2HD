// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.HTZBREAKABLEPLATFORM.HTZBreakablePlatformType
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Objects.Metadata;

namespace SONICORCA.OBJECTS.HTZBREAKABLEPLATFORM
{

    [SonicOrca.Core.Objects.Metadata.Name("Breakable platform")]
    [Description("Breakable platform from Hill Top Zone, Sonic 2")]
    [SonicOrca.Core.Objects.Metadata.Classification(ObjectClassification.Obstacle)]
    [ObjectInstance(typeof (HTZBreakablePlatformInstance))]
    public class HTZBreakablePlatformType : ObjectType
    {
      [Dependency]
      public const string AnimationGroupResourceKey = "/ANIGROUP";
      [Dependency]
      public const string ParticleObjectResourceKey = "SONICORCA/OBJECTS/HTZROCK/PARTICLE";
      [Dependency]
      public const string SoundResourceKey = "SONICORCA/SOUND/BREAKABLE";
    }
}
