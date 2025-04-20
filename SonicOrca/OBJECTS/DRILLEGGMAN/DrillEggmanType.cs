// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.DRILLEGGMAN.DrillEggmanType
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Objects.Metadata;

namespace SONICORCA.OBJECTS.DRILLEGGMAN
{

    [SonicOrca.Core.Objects.Metadata.Name("Drill Eggman")]
    [Description("Drill Eggman from Emerald Hill Zone, Sonic 2")]
    [ObjectInstance(typeof (DrillEggmanInstance))]
    public class DrillEggmanType : ObjectType
    {
      [Dependency]
      public const string AnimationGroupResourceKey = "/ANIGROUP";
      [Dependency]
      public const string RobotnikAnimationGroupResourceKey = "SONICORCA/OBJECTS/ROBOTNIK/ANIGROUP";
      [Dependency]
      public const string BossExplosionResourceKey = "SONICORCA/OBJECTS/EXPLOSION/BOSS";
      [Dependency]
      public const string BossHelicopterSoundResourceKey = "SONICORCA/SOUND/BOSSHELICOPTER";
      [Dependency]
      public const string BossHitSoundResourceKey = "SONICORCA/SOUND/BOSSHIT";
      [Dependency]
      public const string DefaultBrokenSmokeResourceKey = "SONICORCA/OBJECTS/DUST";
    }
}
