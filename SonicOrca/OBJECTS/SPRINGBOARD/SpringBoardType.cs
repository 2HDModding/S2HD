// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.SPRINGBOARD.SpringBoardType
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Objects.Metadata;

namespace SONICORCA.OBJECTS.SPRINGBOARD
{

    [SonicOrca.Core.Objects.Metadata.Name("Spring board")]
    [Description("Spring board from Sonic 2")]
    [SonicOrca.Core.Objects.Metadata.Classification(ObjectClassification.General)]
    [ObjectInstance(typeof (SpringBoardInstance))]
    public class SpringBoardType : ObjectType
    {
      [Dependency]
      public const string AnimationGroupResourceKey = "/ANIGROUP";
      [Dependency]
      public const string SoundResourceKey = "SONICORCA/SOUND/SPRING";
    }
}
