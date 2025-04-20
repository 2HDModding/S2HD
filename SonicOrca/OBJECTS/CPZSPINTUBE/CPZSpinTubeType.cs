// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.CPZSPINTUBE.CPZSpinTubeType
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Objects.Metadata;

namespace SONICORCA.OBJECTS.CPZSPINTUBE {

  [SonicOrca.Core.Objects.Metadata.Name("Spin tube path")]
  [Description("Spin tube path from Chemical Plant Zone, Sonic 2")]
  [SonicOrca.Core.Objects.Metadata.Classification(ObjectClassification.Obstacle)]
  [ObjectInstance(typeof (CPZSpinTubeInstance))]
  public class CPZSpinTubeType : ObjectType
  {
    [Dependency]
    public const string SpinballSoundResourceKey = "SONICORCA/SOUND/SPINBALL";
    [Dependency]
    public const string SpindashReleaseSoundResourceKey = "SONICORCA/SOUND/SPINDASH/RELEASE";
  }
}