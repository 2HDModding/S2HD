// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.HTZROCK.PARTICLE.HTZRockParticleInstance
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Objects.Metadata;
using SonicOrca.Extensions;
using SonicOrca.Geometry;
using SonicOrca.Graphics;
using System.Collections.Generic;

#nullable disable
namespace SONICORCA.OBJECTS.HTZROCK.PARTICLE;

public class HTZRockParticleInstance : ActiveObject
{
  private const int AnimationRockParticle = 1;
  private AnimationInstance _animation;
  private Vector2 _velocity;
  private double _scale;

  [StateVariable]
  private Vector2 Velocity
  {
    get => this._velocity;
    set => this._velocity = value;
  }

  public HTZRockParticleInstance() => this.DesignBounds = new Rectanglei(-60, -60, 120, 120);

  protected override void OnStart()
  {
    this._animation = new AnimationInstance(this.ResourceTree, this.Type.GetAbsolutePath("//ANIGROUP"), 1);
    this._animation.CurrentFrameIndex = this.Level.Random.Next(0, ((IReadOnlyCollection<Animation.Frame>) this._animation.Animation.Frames).Count);
    this._scale = this.Level.Random.NextDouble() * 0.5 + 0.5;
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
    IObjectRenderer objectRenderer = renderer.GetObjectRenderer();
    using (objectRenderer.BeginMatixState())
    {
      objectRenderer.ModelMatrix *= Matrix4.CreateScale(this._scale, this._scale);
      objectRenderer.Render(this._animation);
    }
  }
}
