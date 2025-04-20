// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.SPIKES.SpikesType
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Objects.Metadata;

namespace SONICORCA.OBJECTS.SPIKES;

[SonicOrca.Core.Objects.Metadata.Name("Spikes")]
[Description("Spikes from Sonic 1")]
[SonicOrca.Core.Objects.Metadata.Classification(ObjectClassification.Obstacle)]
[ObjectInstance(typeof (SpikesInstance))]
public class SpikesType : ObjectType
{
  [Dependency]
  public const string AnimationGroupResourceKey = "/ANIGROUP";
  [Dependency]
  public const string SpikeMovementSoundResourceKey = "SONICORCA/SOUND/SPIKESLASH";
}
