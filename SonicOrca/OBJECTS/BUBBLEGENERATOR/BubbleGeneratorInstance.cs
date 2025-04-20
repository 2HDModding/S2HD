// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.BUBBLEGENERATOR.BubbleGeneratorInstance
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Extensions;
using SonicOrca.Graphics;

namespace SONICORCA.OBJECTS.BUBBLEGENERATOR {

  public class BubbleGeneratorInstance : ActiveObject
  {
    private AnimationInstance _animation;
    private int _nextLargeBubbleTime;
    private int _nextSmallBubbleTime;

    protected override void OnStart()
    {
      this._animation = new AnimationInstance(this.ResourceTree, this.Type.GetAbsolutePath("/ANIGROUP"));
    }

    protected override void OnUpdate()
    {
      if (this._nextLargeBubbleTime-- <= 0)
      {
        this._nextLargeBubbleTime = 960;
        this.Level.WaterManager.CreateBubble(this.Level.Map.Layers.IndexOf(this.Layer), this.Position, 2);
      }
      if (this._nextSmallBubbleTime-- > 0)
        return;
      this._nextSmallBubbleTime = this.Level.Random.Next(64 /*0x40*/, 128 /*0x80*/);
      this.Level.WaterManager.CreateBubble(this.Level.Map.Layers.IndexOf(this.Layer), this.Position, this.Level.Random.Next(0, 2));
    }

    protected override void OnAnimate() => this._animation.Animate();

    protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
    {
      renderer.GetObjectRenderer().Render(this._animation);
    }
  }
}