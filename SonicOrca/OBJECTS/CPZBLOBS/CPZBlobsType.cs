﻿// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.CPZBLOBS.CPZBlobsType
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Objects.Metadata;

namespace SONICORCA.OBJECTS.CPZBLOBS {

  [SonicOrca.Core.Objects.Metadata.Name("Blobs")]
  [Description("Blobs from Chemical Plant Zone, Sonic 2")]
  [ObjectInstance(typeof (CPZBlobsInstance))]
  public class CPZBlobsType : ObjectType
  {
    [Dependency]
    public const string AnimationGroupResourceKey = "/ANIGROUP";
    [Dependency]
    public const string SoundResourceKey = "SONICORCA/SOUND/BLOB";
  }
}