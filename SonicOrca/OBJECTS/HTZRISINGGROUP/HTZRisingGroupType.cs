// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.HTZRISINGGROUP.HTZRisingGroupType
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Objects.Metadata;
using SonicOrca.Geometry;

#nullable disable
namespace SONICORCA.OBJECTS.HTZRISINGGROUP;

[SonicOrca.Core.Objects.Metadata.Name("Rising group")]
[Description("Rising group from Hill Top Zone, Sonic 2")]
[SonicOrca.Core.Objects.Metadata.Classification(ObjectClassification.Obstacle)]
[ObjectInstance(typeof (HTZRisingGroupInstance))]
public class HTZRisingGroupType : ObjectType
{
  public Vector2 GetLifeRadius(HTZRisingGroupInstance state)
  {
    Vector2i size = state.Size;
    double x = (double) size.X / 2.0;
    size = state.Size;
    double y = (double) size.Y / 2.0;
    return new Vector2(x, y);
  }
}
