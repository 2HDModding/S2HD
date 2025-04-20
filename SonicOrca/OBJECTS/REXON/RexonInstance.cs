// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.REXON.RexonInstance
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca;
using SonicOrca.Core;
using SonicOrca.Core.Collision;
using SonicOrca.Core.Objects;
using SonicOrca.Core.Objects.Base;
using SonicOrca.Extensions;
using SonicOrca.Geometry;
using SonicOrca.Graphics;
using SonicOrca.Resources;
using System;
using System.Collections.Generic;

namespace SONICORCA.OBJECTS.REXON;

public class RexonInstance : ActiveObject
{
  private const int AnimationBody = 0;
  private const int AnimationNeck = 1;
  private const int AnimationMouth = 2;
  private const int AnimationHead = 3;
  private const int AnimationMouthFire = 4;
  private const int AnimationHeadFire = 5;
  private const int AnimationProjectile = 6;
  private AnimationGroup _animationGroup;
  private AnimationInstance _animationBody;
  private AnimationInstance _animationNeck;
  private AnimationInstance _animationMouth;
  private AnimationInstance _animationHead;
  private Vector2 _velocity;
  private bool _headLifted;
  private bool _facingRight;
  private int _turnTicksRemaining;
  private readonly RexonInstance.Neck[] _necks = new RexonInstance.Neck[5];
  private bool _headIsAlive;

  private RexonInstance.HeadBack Head => (RexonInstance.HeadBack) this._necks[4];

  public RexonInstance() => this.DesignBounds = new Rectanglei(-64, -32, 128 /*0x80*/, 64 /*0x40*/);

  protected override void OnStart()
  {
    base.OnStart();
    this._animationGroup = this.ResourceTree.GetLoadedResource<AnimationGroup>((ILoadedResource) this.Type, "/ANIGROUP");
    this._animationBody = new AnimationInstance(this._animationGroup);
    this._animationNeck = new AnimationInstance(this._animationGroup, 1);
    this._animationMouth = new AnimationInstance(this._animationGroup, 2);
    this._animationHead = new AnimationInstance(this._animationGroup, 3);
    this.CollisionVectors = CollisionVector.FromRectangle((ActiveObject) this, new Rectanglei(-64, -32, 128 /*0x80*/, 64 /*0x40*/));
    this._velocity = new Vector2(-0.5, 0.0);
    this._turnTicksRemaining = 128 /*0x80*/;
  }

  protected override void OnUpdate()
  {
    if (!this._headLifted)
      this.WaitForCharacter();
    if (this.Head == null)
      return;
    this._headIsAlive = !this.Head.Finished;
  }

  private void WaitForCharacter()
  {
    ICharacter closestCharacterTo = this.Level.ObjectManager.GetClosestCharacterTo((Vector2) this.Position);
    if (closestCharacterTo != null && Math.Abs(this.Position.X - closestCharacterTo.Position.X) < 640)
    {
      this._facingRight = closestCharacterTo.Position.X > this.Position.X;
      this._velocity = new Vector2();
      this.CreateNeckAndHead();
    }
    else
    {
      if (this._turnTicksRemaining-- <= 0)
      {
        this._velocity.X *= -1.0;
        this._facingRight = !this._facingRight;
        this._turnTicksRemaining = 128 /*0x80*/;
      }
      this.PositionPrecise = this.PositionPrecise + this._velocity;
    }
  }

  private void CreateNeckAndHead()
  {
    this._headIsAlive = true;
    this._headLifted = true;
    RexonInstance.Neck neck1 = (RexonInstance.Neck) null;
    for (int index = 4; index >= 0; --index)
    {
      RexonInstance.Neck neck2 = index == 4 ? (RexonInstance.Neck) this.Level.ObjectManager.AddSubObject<RexonInstance.HeadBack>((ActiveObject) this) : this.Level.ObjectManager.AddSubObject<RexonInstance.Neck>((ActiveObject) this);
      this._necks[index] = neck2;
      neck2.Prepare(index);
      if (neck1 != null)
      {
        neck2.NextNeck = neck1;
        neck1.PreviousNeck = neck2;
      }
      neck1 = neck2;
    }
  }

  protected override void OnAnimate()
  {
    if (this._headIsAlive)
    {
      if (this.Head.NextProjectileTicksRemaining < this._animationGroup[4].Duration - 4)
        this._animationMouth.Index = 4;
      if (this.Head.NextProjectileTicksRemaining < this._animationGroup[5].Duration - 4)
        this._animationHead.Index = 5;
    }
    this._animationBody.Animate();
    this._animationNeck.Animate();
    this._animationMouth.Animate();
    this._animationHead.Animate();
  }

  protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
  {
    renderer.GetObjectRenderer().Render(this._animationBody, this._facingRight);
  }

  private class Neck : Badnik
  {
    private static readonly IReadOnlyList<int> NeckRaiseTicks = (IReadOnlyList<int>) new int[6]
    {
      30,
      24,
      18,
      12,
      6,
      0
    };
    private static readonly IReadOnlyList<int> UnknownA = (IReadOnlyList<int>) new int[5]
    {
      0,
      36,
      32 /*0x20*/,
      28,
      26
    };
    private static readonly IReadOnlyList<double> DeathXVelocities = (IReadOnlyList<double>) new double[5]
    {
      2.0,
      -4.0,
      4.0,
      -2.0,
      2.0
    };
    private int _index;
    private Vector2 _velocity;
    private int _oscillation;

    public RexonInstance Rexon => (RexonInstance) this.ParentObject;

    public RexonInstance.Neck PreviousNeck { get; set; }

    public RexonInstance.Neck NextNeck { get; set; }

    protected RexonInstance.Neck.NeckState State { get; set; }

    protected int StateTicksRemaining { get; set; }

    public virtual void Prepare(int index)
    {
      this._index = index;
      this.StateTicksRemaining = RexonInstance.Neck.NeckRaiseTicks[this._index];
      this._oscillation += this._index * 16 /*0x10*/;
      Vector2i vector2i = new Vector2i(-80, 64 /*0x40*/);
      if (this.Rexon._facingRight)
        vector2i.X *= -1;
      this.Position = this.Position + vector2i;
      this.Priority = this.ParentObject.Priority + 1 + this._index;
      this.CollisionRectangles = new CollisionRectangle[1]
      {
        new CollisionRectangle((ActiveObject) this, 0, -16, -16, 32 /*0x20*/, 32 /*0x20*/)
      };
      this.CanBeDestoryed = false;
    }

    protected override void OnUpdate()
    {
      base.OnUpdate();
      switch (this.State)
      {
        case RexonInstance.Neck.NeckState.PreRise:
          this.CheckHeadIsAlive();
          if (this.StateTicksRemaining-- >= 0)
            break;
          this.StartRaise();
          break;
        case RexonInstance.Neck.NeckState.Rise:
          this.CheckHeadIsAlive();
          this._velocity.X += 0.25;
          if (this.StateTicksRemaining-- < 0)
            this.StartNormalState();
          this.PositionPrecise = this.PositionPrecise + this._velocity;
          break;
        case RexonInstance.Neck.NeckState.Oscillate:
          this.CheckHeadIsAlive();
          this.Oscillate();
          break;
        case RexonInstance.Neck.NeckState.Drop:
          this._velocity.Y += 0.875;
          this.PositionPrecise = this.PositionPrecise + this._velocity;
          break;
      }
    }

    private void StartRaise()
    {
      this.State = RexonInstance.Neck.NeckState.Rise;
      this._velocity = new Vector2(-0.45, -8.0);
      this.StateTicksRemaining = RexonInstance.Neck.NeckRaiseTicks[4 - this._index];
    }

    private void StartNormalState()
    {
      this.State = RexonInstance.Neck.NeckState.Oscillate;
      this._velocity = new Vector2();
      this.StateTicksRemaining = 32 /*0x20*/;
      this._oscillation = RexonInstance.Neck.UnknownA[this._index] * 4;
    }

    private void CheckHeadIsAlive()
    {
      if (this.Rexon._headIsAlive)
        return;
      this.State = RexonInstance.Neck.NeckState.Drop;
      this._velocity.X = RexonInstance.Neck.DeathXVelocities[this._index];
    }

    private void Oscillate()
    {
      this._oscillation = (this._oscillation + 1) % 128 /*0x80*/;
      double num = (double) this._oscillation / 128.0 * (2.0 * Math.PI);
      int x = (int) (Math.Cos(num) * 24.0);
      int y = (int) (-Math.Abs(Math.Sin(num)) * 8.0) - 52;
      if (this is RexonInstance.HeadBack)
        y += 16 /*0x10*/;
      if (this.PreviousNeck != null)
        this.Position = this.PreviousNeck.Position + new Vector2i(x, y);
      if (this._index != 4)
        return;
      this.Position = this.Position + new Vector2i(this.Rexon._facingRight ? 18 : -18, 0);
    }

    protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
    {
      IObjectRenderer objectRenderer = renderer.GetObjectRenderer();
      Animation animation = this.Rexon._animationNeck.Animation;
      int count = ((IReadOnlyCollection<Animation.Frame>) animation.Frames).Count;
      Animation.Frame frame = animation.Frames[MathX.Clamp(0, (this.NextNeck._oscillation + (this.Rexon._facingRight ? 44 : 108)) % 128 /*0x80*/, count - 1)];
      Rectanglei source = frame.Source;
      objectRenderer.Texture = this.Rexon._animationNeck.AnimationGroup.Textures[frame.TextureIndex];
      objectRenderer.Render((Rectangle) source, (Vector2) new Vector2i(), this.Rexon._facingRight);
    }

    protected enum NeckState
    {
      PreRise,
      Rise,
      Oscillate,
      Drop,
    }
  }

  private class HeadBack : RexonInstance.Neck
  {
    private RexonInstance.HeadFront _headFront;

    public int NextProjectileTicksRemaining
    {
      get
      {
        return this.State != RexonInstance.Neck.NeckState.Oscillate ? (int) sbyte.MaxValue : this.StateTicksRemaining;
      }
    }

    public override void Prepare(int index)
    {
      base.Prepare(index);
      this._headFront = this.Level.ObjectManager.AddSubObject<RexonInstance.HeadFront>((ActiveObject) this);
      this.CanBeDestoryed = true;
    }

    protected override void OnUpdate()
    {
      base.OnUpdate();
      if (this.State == RexonInstance.Neck.NeckState.Oscillate)
      {
        if (this.StateTicksRemaining-- < 0)
          this.FireProjectile();
      }
      if (this._headFront == null)
        return;
      this._headFront.Position = this.Position;
    }

    private void FireProjectile()
    {
      this.StateTicksRemaining = (int) sbyte.MaxValue;
      RexonInstance.Projectile projectile = this.Level.ObjectManager.AddSubObject<RexonInstance.Projectile>(this.ParentObject);
      projectile.Position = this.Position + new Vector2i(this.Rexon._facingRight ? 16 /*0x10*/ : -16, 4);
      projectile.Velocity = new Vector2((double) (4 * (this.Rexon._facingRight ? 1 : -1)), 2.0);
    }

    protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
    {
      renderer.GetObjectRenderer().Render(this.Rexon._animationMouth, this.Rexon._facingRight);
    }
  }

  private class HeadFront : ActiveObject
  {
    public RexonInstance.Neck Parent => this.ParentObject as RexonInstance.Neck;

    protected override void OnStart()
    {
      this.LockLifetime = true;
      this.Priority = 512 /*0x0200*/;
    }

    protected override void OnUpdate()
    {
      if (!this.Parent.Finished)
        return;
      this.Finish();
    }

    protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
    {
      RexonInstance parentObject = this.ParentObject.ParentObject as RexonInstance;
      renderer.GetObjectRenderer().Render(parentObject._animationHead, parentObject._facingRight);
    }
  }

  private class Projectile : Enemy
  {
    private AnimationInstance _animation;

    public Vector2 Velocity { get; set; }

    protected override void OnStart()
    {
      base.OnStart();
      this._animation = new AnimationInstance(this.ResourceTree, this.Type.GetAbsolutePath("/ANIGROUP"), 6);
      this.CollisionRectangles = new CollisionRectangle[1]
      {
        new CollisionRectangle((ActiveObject) this, 0, -25, -25, 50, 50)
      };
      this.Priority += 32 /*0x20*/;
    }

    protected override void OnUpdate()
    {
      base.OnUpdate();
      this.PositionPrecise = this.PositionPrecise + this.Velocity;
    }

    protected override void OnAnimate() => this._animation.Animate();

    protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
    {
      IObjectRenderer objectRenderer = renderer.GetObjectRenderer();
      objectRenderer.EmitsLight = true;
      objectRenderer.Render(this._animation);
    }
  }
}
