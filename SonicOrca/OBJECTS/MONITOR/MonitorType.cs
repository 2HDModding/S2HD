// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.MONITOR.MonitorType
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Objects.Metadata;

#nullable disable
namespace SONICORCA.OBJECTS.MONITOR;

[SonicOrca.Core.Objects.Metadata.Name("Monitor")]
[Description("Monitor from Sonic 2")]
[SonicOrca.Core.Objects.Metadata.Classification(ObjectClassification.General)]
[ObjectInstance(typeof (MonitorInstance))]
public class MonitorType : ObjectType
{
  [Dependency]
  public const string AnimationGroupResourceKey = "/ANIGROUP";
  [Dependency]
  public const string ExplosionResourceKey = "SONICORCA/OBJECTS/EXPLOSION/BADNIK";
}
