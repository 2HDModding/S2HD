// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.EHZBRIDGE.STAKE.EHZBridgeStakeType
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Objects.Metadata;

namespace SONICORCA.OBJECTS.EHZBRIDGE.STAKE
{

    [SonicOrca.Core.Objects.Metadata.Name("Bridge stake")]
    [Description("Bridge stake from Emerald Hill Zone, Sonic 2")]
    [SonicOrca.Core.Objects.Metadata.Classification(ObjectClassification.Scenery)]
    [ObjectInstance(typeof (EHZBridgeStakeInstance))]
    public class EHZBridgeStakeType : ObjectType
    {
      [Dependency]
      public const string AnimationGroupResourceKey = "//ANIGROUP";
    }
}
