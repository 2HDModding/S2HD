// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.HTZBLOCK.HTZBlockType
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Objects.Base;
using SonicOrca.Core.Objects.Metadata;
using SonicOrca.Geometry;

namespace SONICORCA.OBJECTS.HTZBLOCK
{

    [SonicOrca.Core.Objects.Metadata.Name("Floating block")]
    [Description("Floating block from Hill Top Zone, Sonic 2")]
    [SonicOrca.Core.Objects.Metadata.Classification(ObjectClassification.Platform)]
    [ObjectInstance(typeof (HTZBlockInstance))]
    public class HTZBlockType : ObjectType
    {
      [Dependency]
      public const string AnimationGroupResourceKey = "/ANIGROUP";

      public Vector2 GetLifeRadius(Platform state)
      {
        Vector2 movementRadius = state.MovementRadius;
        double x = movementRadius.X;
        movementRadius = state.MovementRadius;
        double y = movementRadius.Y;
        return new Vector2(x, y);
      }
    }
}
