// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.CPZCHEMICALFALL.CPZChemicalFallType
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Objects.Metadata;
using SonicOrca.Geometry;

#nullable disable
namespace SONICORCA.OBJECTS.CPZCHEMICALFALL;

[SonicOrca.Core.Objects.Metadata.Name("Chemical Fall")]
[Description("Chemical Fall from Chemical Plant Zone, Sonic 2")]
[ObjectInstance(typeof (CPZChemicalFallInstance))]
public class CPZChemicalFallType : ObjectType
{
  [Dependency]
  public const string AnimationGroupResourceKey = "/ANIGROUP";

  public new Vector2 GetLifeRadius(ActiveObject state)
  {
    return (Vector2) new Vector2i(0, this.Level.Map.Bounds.Height);
  }
}
