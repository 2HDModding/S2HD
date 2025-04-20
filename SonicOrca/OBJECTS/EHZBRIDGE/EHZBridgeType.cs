// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.EHZBRIDGE.EHZBridgeType
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Objects.Metadata;
using SonicOrca.Geometry;

namespace SONICORCA.OBJECTS.EHZBRIDGE
{

    [SonicOrca.Core.Objects.Metadata.Name("Bridge")]
    [Description("Bridge from Emerald Hill Zone, Sonic 2")]
    [SonicOrca.Core.Objects.Metadata.Classification(ObjectClassification.Platform)]
    [ObjectInstance(typeof (EHZBridgeInstance))]
    public class EHZBridgeType : ObjectType
    {
      [Dependency]
      public const string AnimationGroupResourceKey = "/ANIGROUP";

      public Vector2 GetLifeRadius(EHZBridgeInstance state)
      {
        return (Vector2) new Vector2i(state.Logs * 64 /*0x40*/ / 2, 0);
      }
    }
}
