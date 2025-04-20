// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.HTZSEESAW.HTZSeeSawType
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Objects.Metadata;
using SonicOrca.Geometry;

namespace SONICORCA.OBJECTS.HTZSEESAW
{

    [SonicOrca.Core.Objects.Metadata.Name("See Saw")]
    [Description("See Saw and Sol from Hill Top Zone, Sonic 2")]
    [SonicOrca.Core.Objects.Metadata.Classification(ObjectClassification.Platform)]
    [ObjectInstance(typeof (HTZSeeSawInstance))]
    public class HTZSeeSawType : ObjectType
    {
      [Dependency]
      public const string AnimationGroupResourceKey = "/ANIGROUP";
      [Dependency]
      public const string SolAnimationGroupResourceKey = "SONICORCA/OBJECTS/SOL/ANIGROUP";
      [Dependency]
      public const string BounceSoundResourceKey = "SONICORCA/SOUND/SPRING";

      public new Vector2 GetLifeRadius(ActiveObject state)
      {
        return (Vector2) new Vector2i(0, 2048 /*0x0800*/);
      }
    }
}
