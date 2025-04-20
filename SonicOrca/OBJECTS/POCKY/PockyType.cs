// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.POCKY.PockyType
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Objects.Metadata;

namespace SONICORCA.OBJECTS.POCKY
{

    [SonicOrca.Core.Objects.Metadata.Name("Pocky")]
    [Description("Pocky from Sonic 1")]
    [SonicOrca.Core.Objects.Metadata.Classification(ObjectClassification.Animal)]
    [ObjectInstance(typeof (PockyInstance))]
    public class PockyType : ObjectType
    {
      [Dependency]
      public const string AnimationGroupResourceKey = "/ANIGROUP";
    }
}
