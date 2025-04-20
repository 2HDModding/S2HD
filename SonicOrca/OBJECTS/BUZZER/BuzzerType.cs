// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.BUZZER.BuzzerType
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Objects.Metadata;
using SonicOrca.Geometry;

namespace SONICORCA.OBJECTS.BUZZER {

  [SonicOrca.Core.Objects.Metadata.Name("Buzzer")]
  [Description("Buzzer from Sonic 2")]
  [SonicOrca.Core.Objects.Metadata.Classification(ObjectClassification.Badnik)]
  [ObjectInstance(typeof (BuzzerInstance))]
  public class BuzzerType : ObjectType
  {
    [Dependency]
    public const string AnimationGroupResourceKey = "/ANIGROUP";

    public new Vector2 GetLifeRadius(ActiveObject state)
    {
      return (Vector2) new Vector2i(1024 /*0x0400*/, 0);
    }
  }
}