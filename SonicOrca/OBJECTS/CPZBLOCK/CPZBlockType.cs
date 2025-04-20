// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.CPZBLOCK.CPZBlockType
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Objects.Metadata;

namespace SONICORCA.OBJECTS.CPZBLOCK {

  [SonicOrca.Core.Objects.Metadata.Name("Block")]
  [Description("Block from Chemical Plant Zone, Sonic 2")]
  [SonicOrca.Core.Objects.Metadata.Classification(ObjectClassification.Platform)]
  [ObjectInstance(typeof (CPZBlockInstance))]
  public class CPZBlockType : ObjectType
  {
    [Dependency]
    public const string AnimationGroupResourceKey = "/ANIGROUP";
  }
}