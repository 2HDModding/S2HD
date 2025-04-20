// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.SONIC.SonicType
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Objects.Metadata;

namespace SONICORCA.OBJECTS.SONIC
{

    [SonicOrca.Core.Objects.Metadata.Name("Sonic")]
    [Description("Sonic from Sonic 1")]
    [SonicOrca.Core.Objects.Metadata.Classification(ObjectClassification.Character)]
    [ObjectInstance(typeof (SonicInstance))]
    [Dependency("SONICORCA/OBJECTS/DUST")]
    [Dependency("SONICORCA/OBJECTS/BARRIER/ANIGROUP")]
    [Dependency("SONICORCA/OBJECTS/BARRIER/BUBBLE/ANIGROUP")]
    [Dependency("SONICORCA/OBJECTS/BARRIER/FIRE/ANIGROUP")]
    [Dependency("SONICORCA/OBJECTS/BARRIER/LIGHTNING/ANIGROUP")]
    [Dependency("SONICORCA/OBJECTS/INVINCIBILITY/ANIGROUP")]
    [Dependency("SONICORCA/OBJECTS/SPINDASH/ANIGROUP")]
    [Dependency("SONICORCA/SOUND/BARRIER")]
    [Dependency("SONICORCA/SOUND/BRAKE")]
    [Dependency("SONICORCA/SOUND/SPINBALL")]
    [Dependency("SONICORCA/SOUND/JUMP")]
    [Dependency("SONICORCA/SOUND/SPINDASH/CHARGE")]
    [Dependency("SONICORCA/SOUND/SPINDASH/RELEASE")]
    [Dependency("SONICORCA/SOUND/HURT")]
    [Dependency("SONICORCA/SOUND/HURT/SPIKES")]
    [Dependency("SONICORCA/SOUND/RINGSCATTER")]
    [Dependency("SONICORCA/SOUND/INHALE")]
    [Dependency("SONICORCA/SOUND/SPLASH")]
    [Dependency("SONICORCA/SOUND/DROWNWARNING")]
    [Dependency("SONICORCA/SOUND/DROWN")]
    public class SonicType : ObjectType
    {
      [Dependency]
      public const string AnimationGroupResourceKey = "/ANIGROUP";
      [Dependency]
      public const string PeeloutChargeSoundResourceKey = "SONICORCA/SOUND/PEELOUT/CHARGE";
      [Dependency]
      public const string PeeloutReleaseSoundResourceKey = "SONICORCA/SOUND/PEELOUT/RELEASE";
    }
}
