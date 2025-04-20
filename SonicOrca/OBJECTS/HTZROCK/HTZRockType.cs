// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.HTZROCK.HTZRockType
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Objects.Metadata;

#nullable disable
namespace SONICORCA.OBJECTS.HTZROCK;

[SonicOrca.Core.Objects.Metadata.Name("Rock")]
[Description("Rock from Hill Top Zone, Sonic 2")]
[SonicOrca.Core.Objects.Metadata.Classification(ObjectClassification.Obstacle)]
[ObjectInstance(typeof (HTZRockInstance))]
public class HTZRockType : ObjectType
{
  [Dependency]
  public const string AnimationGroupResourceKey = "/ANIGROUP";
  [Dependency]
  public const string ParticleObjectResourceKey = "/PARTICLE";
  [Dependency]
  public const string SoundResourceKey = "SONICORCA/SOUND/BREAKABLE";
}
