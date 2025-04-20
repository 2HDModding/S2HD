// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.POINTS.PointsType
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Objects.Metadata;

namespace SONICORCA.OBJECTS.POINTS
{

    [SonicOrca.Core.Objects.Metadata.Name("Points")]
    [Description("Points from Sonic 1")]
    [SonicOrca.Core.Objects.Metadata.Classification(ObjectClassification.Particle)]
    [ObjectInstance(typeof (PointsInstance))]
    public class PointsType : ObjectType
    {
      [Dependency]
      public const string FontResourceKey = "SONICORCA/FONTS/POINTS";
    }
}
