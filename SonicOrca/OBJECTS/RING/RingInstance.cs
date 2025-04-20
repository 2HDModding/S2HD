// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.RING.RingInstance
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca;
using SonicOrca.Core;
using SonicOrca.Core.Collision;
using SonicOrca.Core.Lighting;
using SonicOrca.Core.Objects;
using SonicOrca.Core.Objects.Metadata;
using SonicOrca.Extensions;
using SonicOrca.Geometry;
using SonicOrca.Graphics;
using System;
using System.Collections.Generic;

#nullable disable
namespace SONICORCA.OBJECTS.RING;

public class RingInstance : ActiveObject
{
  private static readonly IReadOnlyList<Vector2i> SparkleParticleOffsets = (IReadOnlyList<Vector2i>) new Vector2i[4]
  {
    new Vector2i(16 /*0x10*/, 4),
    new Vector2i(-16, 12),
    new Vector2i(-12, -16),
    new Vector2i(20, -24)
  };
  private bool _scatter;
  private bool _stationary;
  private bool _attracted;
  private AnimationInstance _animation;
  private Vector2 _velocity;
  private int _lifeTicks;
  private ITexture _glowTexture;
  private PointLightSource _lightSource;

  [StateVariable]
  private bool Scatter
  {
    get => this._scatter;
    set => this._scatter = value;
  }

  [StateVariable]
  private Vector2 Velocity
  {
    get => this._velocity;
    set => this._velocity = value;
  }

  public RingInstance() => this.DesignBounds = new Rectanglei(-32, -32, 64 /*0x40*/, 64 /*0x40*/);

  protected override void OnStart()
  {
    this.CollisionRectangles = new CollisionRectangle[1]
    {
      new CollisionRectangle((ActiveObject) this, 0, -16, -16, 32 /*0x20*/, 32 /*0x20*/)
    };
    if (this.Scatter)
    {
      this._stationary = false;
      this._lifeTicks = (int) byte.MaxValue;
    }
    else
    {
      this._stationary = true;
      this._lifeTicks = (int) byte.MaxValue;
      this._velocity = new Vector2();
    }
    this.Priority = 4096 /*0x1000*/;
    this._glowTexture = this.ResourceTree.GetLoadedResource<ITexture>(this.Type.GetAbsolutePath("SONICORCA/PARTICLE/GLOW"));
    this.ShadowInfo = (ShadowInfo) null;
    this._lightSource = new PointLightSource(8, this.Position);
    this.Level.LightingManager.RegisterLightSource((ILightSource) this._lightSource);
  }

  protected override void OnUpdate()
  {
    this._lightSource.Position = this.Position;
    ICharacter character1 = (ICharacter) null;
    double num1 = double.MaxValue;
    foreach (ICharacter character2 in this.Level.ObjectManager.Characters)
    {
      if (character2.Barrier == BarrierType.Lightning)
      {
        double length = (double) (character2.Position - this.Position).Length;
        if (length < num1 && (this._attracted || length < 256.0))
        {
          character1 = character2;
          num1 = length;
        }
      }
    }
    if (character1 != null)
    {
      this._stationary = false;
      Vector2i position = character1.Position;
      int x1 = position.X;
      position = this.Position;
      int x2 = position.X;
      int num2 = x1 - x2;
      if (Math.Sign(num2) != Math.Sign(this._velocity.X) && this._velocity.X != 0.0)
        this._velocity.X += 1.5 * (double) Math.Sign(num2);
      else
        this._velocity.X += 0.75 * (double) Math.Sign(num2);
      position = character1.Position;
      int y1 = position.Y;
      position = this.Position;
      int y2 = position.Y;
      int num3 = y1 - y2;
      if (Math.Sign(num3) != Math.Sign(this._velocity.Y) && this._velocity.Y != 0.0)
        this._velocity.Y += 1.5 * (double) Math.Sign(num3);
      else
        this._velocity.Y += 0.75 * (double) Math.Sign(num3);
      this.PositionPrecise = this.PositionPrecise + this._velocity;
      this._attracted = true;
    }
    else
    {
      if (this._stationary)
        return;
      if (this._animation == null)
      {
        this._animation = new AnimationInstance((this.Type as RingType).AnimationInstance.AnimationGroup);
        this._animation.OverrideDelay = new int?(0);
      }
      if (this._lifeTicks == 0)
      {
        this.FinishForever();
      }
      else
      {
        this._animation.OverrideDelay = new int?(((int) byte.MaxValue - this._lifeTicks) / 32 /*0x20*/);
        --this._lifeTicks;
        this.UpdateMovement();
      }
    }
  }

  private void UpdateMovement()
  {
    this.PositionPrecise = this.PositionPrecise + this._velocity;
    bool flag1 = Math.Abs(this._velocity.Y) < 4.0;
    bool flag2 = false;
    bool flag3 = false;
    bool flag4 = false;
    foreach (CollisionInfo collision in CollisionVector.GetCollisions((ActiveObject) this, 32 /*0x20*/))
    {
      if (!(collision.Vector.Mode != CollisionMode.Bottom & flag1))
      {
        if (collision.Vector.IsWall)
        {
          this.PositionPrecise = new Vector2(this.PositionPrecise.X + collision.Shift, this.PositionPrecise.Y);
          flag3 = true;
        }
        else
        {
          this.PositionPrecise = new Vector2(this.PositionPrecise.X, this.PositionPrecise.Y + collision.Shift);
          flag2 = collision.Vector.Mode == CollisionMode.Top;
          flag4 = collision.Vector.Mode == CollisionMode.Bottom;
        }
      }
    }
    if (flag3)
      this._velocity.X *= -0.25;
    if (flag2)
      this._velocity.Y *= -0.75;
    if (flag4)
      this._velocity.Y = 0.0;
    if (flag2 || flag4)
      return;
    this._velocity.Y += 0.375;
  }

  protected override void OnCollision(CollisionEvent e)
  {
    if (e.ActiveObject.Type.Classification != ObjectClassification.Character)
      return;
    ICharacter activeObject = (ICharacter) e.ActiveObject;
    if (!this._stationary && !this._attracted && this._lifeTicks >= 175)
      return;
    this.Collect(activeObject.Player);
  }

  private void Collect(Player player)
  {
    player.GainRings();
    this.CreateSparkles();
    ++this.Level.RingsCollected;
    if (this.Level.RingsCollected == this.Level.RingsPerfectTarget)
      this.Level.SoundManager.PlaySound((IActiveObject) this, this.Type.GetAbsolutePath("SONICORCA/SOUND/PERFECT"));
    else
      this.Level.SoundManager.PlaySound((IActiveObject) this, this.Type.GetAbsolutePath("SONICORCA/SOUND/RING"));
    this.FinishForever();
  }

  private void CreateSparkles()
  {
    int ticks = 0;
    foreach (Vector2i sparkleParticleOffset in (IEnumerable<Vector2i>) RingInstance.SparkleParticleOffsets)
    {
      ObjectPlacement objectPlacement = new ObjectPlacement(this.Type.GetAbsolutePath("/SPARKLE"), this.Level.Map.Layers.IndexOf(this.Layer), this.Position + sparkleParticleOffset);
      this.Level.SetInterval(ticks, (Action) (() => this.Level.ObjectManager.AddObject(objectPlacement)));
      ticks += 2;
    }
  }

  protected override void OnAnimate()
  {
    if (this._stationary || this._animation == null)
      return;
    this._animation.Animate();
  }

  protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
  {
    IObjectRenderer objectRenderer = renderer.GetObjectRenderer();
    objectRenderer.FilterAmount *= 0.25;
    double a1 = MathX.Clamp((double) this._lifeTicks * (16.0 / (double) byte.MaxValue), 1.0);
    double a2 = 0.25 * a1 * viewOptions.FilterAmount;
    if (a2 > 0.0)
    {
      objectRenderer.BlendMode = BlendMode.Additive;
      objectRenderer.Texture = this._glowTexture;
      objectRenderer.MultiplyColour = new Colour(a2, 1.0, 1.0, 0.0);
      objectRenderer.Render();
    }
    AnimationInstance animationInstance;
    if (this._stationary || this._animation == null)
    {
      animationInstance = (this.Type as RingType).AnimationInstance;
      objectRenderer.MultiplyColour = Colours.White;
    }
    else
    {
      animationInstance = this._animation;
      objectRenderer.MultiplyColour = new Colour(a1, 1.0, 1.0, 1.0);
    }
    objectRenderer.BlendMode = BlendMode.Alpha;
    objectRenderer.Render(animationInstance);
  }

  protected override void OnStop()
  {
    if (!this._stationary)
      this.FinishForever();
    this.Level.LightingManager.UnregisterLightSource((ILightSource) this._lightSource);
  }
}
