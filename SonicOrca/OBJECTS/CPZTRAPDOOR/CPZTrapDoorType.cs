// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.CPZTRAPDOOR.CPZTrapDoorType
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Objects.Metadata;

namespace SONICORCA.OBJECTS.CPZTRAPDOOR {

  [SonicOrca.Core.Objects.Metadata.Name("Trap door")]
  [Description("Trap door from Chemical Plant Zone, Sonic 2")]
  [SonicOrca.Core.Objects.Metadata.Classification(ObjectClassification.Obstacle)]
  [ObjectInstance(typeof (CPZTrapDoorInstance))]
  public class CPZTrapDoorType : ObjectType
  {
    [Dependency]
    public const string AnimationGroupResourceKey = "/ANIGROUP";
  }
}