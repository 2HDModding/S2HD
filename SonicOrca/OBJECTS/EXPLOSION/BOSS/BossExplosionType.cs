// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.EXPLOSION.BOSS.BossExplosionType
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Objects.Metadata;

#nullable disable
namespace SONICORCA.OBJECTS.EXPLOSION.BOSS;

[SonicOrca.Core.Objects.Metadata.Name("Boss explosion")]
[Description("Boss explosion from Sonic 1")]
[SonicOrca.Core.Objects.Metadata.Classification(ObjectClassification.Particle)]
[ObjectInstance(typeof (BossExplosionInstance))]
public class BossExplosionType : ObjectType
{
  [Dependency]
  public const string AnimationGroupResourceKey = "/ANIGROUP";
  [Dependency]
  public const string SoundResourceKey = "SONICORCA/SOUND/BOSSEXPLOSION";
}
