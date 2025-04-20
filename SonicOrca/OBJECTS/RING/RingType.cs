// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.RING.RingType
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Objects.Metadata;
using SonicOrca.Extensions;
using SonicOrca.Graphics;

namespace SONICORCA.OBJECTS.RING
{

    [SonicOrca.Core.Objects.Metadata.Name("Ring")]
    [Description("Ring from Sonic 1")]
    [SonicOrca.Core.Objects.Metadata.Classification(ObjectClassification.Ring)]
    [ObjectInstance(typeof (RingInstance))]
    public class RingType : ObjectType
    {
      [Dependency]
      public const string AnimationGroupResourceKey = "/ANIGROUP";
      [Dependency]
      public const string SparkleObjectResourceKey = "/SPARKLE";
      [Dependency]
      public const string CollectSoundResourceKey = "SONICORCA/SOUND/RING";
      [Dependency]
      public const string GlowParticleResourceKey = "SONICORCA/PARTICLE/GLOW";
      [Dependency]
      public const string PerfectSoundResourceKey = "SONICORCA/SOUND/PERFECT";

      public AnimationInstance AnimationInstance { get; private set; }

      public int LastAnimationTick { get; set; }

      protected override void OnStart()
      {
        this.AnimationInstance = new AnimationInstance(this.Level.GameContext.ResourceTree, this.GetAbsolutePath("/ANIGROUP"));
      }

      protected override void OnAnimate() => this.AnimationInstance.Animate();
    }
}
