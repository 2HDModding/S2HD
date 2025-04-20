// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.SIGNPOST.SignpostType
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Objects.Metadata;
using SonicOrca.Geometry;

namespace SONICORCA.OBJECTS.SIGNPOST;

[SonicOrca.Core.Objects.Metadata.Name("Signpost")]
[Description("Signpost from Sonic 1")]
[SonicOrca.Core.Objects.Metadata.Classification(ObjectClassification.General)]
[ObjectInstance(typeof (SignpostInstance))]
public class SignpostType : ObjectType
{
  [Dependency]
  public const string AnimationGroupResourceKey = "/ANIGROUP";
  [Dependency]
  public const string SparkleObjectResourceKey = "SONICORCA/OBJECTS/RING/SPARKLE";
  [Dependency]
  public const string SoundResourceKey = "SONICORCA/SOUND/SIGNPOST";

  public new Vector2 GetLifeRadius(ActiveObject state)
  {
    return (Vector2) new Vector2i(0, this.Level.Bounds.Height);
  }
}
