// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.LAVA.LavaType
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Objects.Metadata;
using SonicOrca.Geometry;

#nullable disable
namespace SONICORCA.OBJECTS.LAVA;

[SonicOrca.Core.Objects.Metadata.Name("Lava")]
[Description("Lava from Sonic 2")]
[SonicOrca.Core.Objects.Metadata.Classification(ObjectClassification.Obstacle)]
[ObjectInstance(typeof (LavaInstance))]
public class LavaType : ObjectType
{
  [Dependency]
  public const string AnimationGroupResourceKey = "/ANIGROUP";

  public Vector2 GetLifeRadius(LavaInstance state)
  {
    return (Vector2) (new Vector2i(state.Width, state.Height) / 2 * 64 /*0x40*/);
  }
}
