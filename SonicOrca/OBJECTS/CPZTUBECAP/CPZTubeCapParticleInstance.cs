// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.CPZTUBECAP.CPZTubeCapParticleInstance
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Extensions;
using SonicOrca.Geometry;
using SonicOrca.Graphics;
using System.Collections.Generic;

namespace SONICORCA.OBJECTS.CPZTUBECAP;

public class CPZTubeCapParticleInstance : ActiveObject
{
  private const int AnimationParticle = 1;
  private AnimationInstance _animation;
  private Vector2 _velocity;

  public Vector2 Velocity
  {
    get => this._velocity;
    set => this._velocity = value;
  }

  public CPZTubeCapParticleInstance() => this.DesignBounds = new Rectanglei(-60, -50, 120, 101);

  protected override void OnStart()
  {
    this._animation = new AnimationInstance(this.ResourceTree, this.Type.GetAbsolutePath("/ANIGROUP"), 1);
    this._animation.CurrentFrameIndex = this.Level.Random.Next(0, ((IReadOnlyCollection<Animation.Frame>) this._animation.Animation.Frames).Count);
    this.Priority = 1512;
  }

  protected override void OnStop() => this.FinishForever();

  protected override void OnUpdate()
  {
    this._velocity.Y += 9.0 / 16.0;
    this.PositionPrecise = this.PositionPrecise + this._velocity;
  }

  protected override void OnAnimate() => this._animation.Animate();

  protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
  {
    renderer.GetObjectRenderer().Render(this._animation);
  }
}
