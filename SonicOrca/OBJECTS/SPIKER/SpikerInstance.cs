// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.SPIKER.SpikerInstance
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Collision;
using SonicOrca.Core.Objects;
using SonicOrca.Core.Objects.Base;
using SonicOrca.Core.Objects.Metadata;
using SonicOrca.Extensions;
using SonicOrca.Geometry;
using SonicOrca.Graphics;
using System;

namespace SONICORCA.OBJECTS.SPIKER;

public class SpikerInstance : Badnik
{
  private const int AnimationHead = 2;
  private const int AnimationBodyTurn = 3;
  private AnimationInstance _animation;
  private bool _flipX;
  private bool _flipY;
  private bool _turning;
  private int _velocityX;
  private bool _resting;
  private bool _spikeFired;
  private int _stateTimer;
  private SpikerInstance.Spike _spike;

  [StateVariable]
  private bool UpsideDown
  {
    get => this._flipY;
    set => this._flipY = value;
  }

  public SpikerInstance() => this.DesignBounds = new Rectanglei(-115, -165, 230, 330);

  protected override void OnStart()
  {
    base.OnStart();
    this._animation = new AnimationInstance(this.ResourceTree, this.Type.GetAbsolutePath("/ANIGROUP"), 2);
    this._resting = false;
    this._velocityX = 2;
    this._stateTimer = 64 /*0x40*/;
    this.CollisionRectangles = new CollisionRectangle[1]
    {
      new CollisionRectangle((ActiveObject) this, 0, -32, this._flipY ? (int) sbyte.MinValue : 48 /*0x30*/, 64 /*0x40*/, 64 /*0x40*/)
    };
    this._spike = this.Level.ObjectManager.AddSubObject<SpikerInstance.Spike>((ActiveObject) this);
    this._spike.Position = this.Position;
    this._spike.FlipY = this._flipY;
    this._spike.LockLifetime = true;
  }

  protected override void Rebound(ICharacter character)
  {
    Vector2 velocity = character.Velocity;
    if (velocity.Y < 0.0)
      velocity.Y += 4.0;
    else if (character.Position.Y >= this.Position.Y)
      velocity.Y *= -1.0;
    else
      velocity.Y *= -1.0;
    character.Velocity = velocity;
  }

  protected override void OnUpdateEditor()
  {
    base.OnUpdateEditor();
    this._spike.Position = this.Position;
  }

  protected override void OnUpdate()
  {
    base.OnUpdate();
    if (this._resting)
    {
      if (this._stateTimer-- <= 0)
      {
        this._resting = false;
        this._stateTimer = 64 /*0x40*/;
        this._velocityX *= -1;
      }
    }
    else
    {
      this.Position = this.Position + new Vector2i(this._velocityX, 0);
      if (!this._spikeFired)
      {
        SpikerInstance.Spike spike = this._spike;
        spike.Position = spike.Position + new Vector2i(this._velocityX, 0);
        this._spike._velocityX = this._velocityX;
      }
      if (this._stateTimer-- <= 0)
      {
        this._resting = true;
        this._stateTimer = 16 /*0x10*/;
      }
    }
    if (!this.CheckForCharacter())
      return;
    this._spike.Fire();
    this._spikeFired = true;
    this._spike.LockLifetime = false;
    this._spike._velocityX = 0;
  }

  private bool CheckForCharacter()
  {
    ICharacter closestCharacterTo = this.Level.ObjectManager.GetClosestCharacterTo((Vector2) this.Position);
    return closestCharacterTo != null && Math.Abs(this.Position.X - closestCharacterTo.Position.X) < 128 /*0x80*/ && Math.Abs(this.Position.Y - closestCharacterTo.Position.Y) < 384;
  }

  protected override void OnStop()
  {
    base.OnStop();
    if (this._spikeFired)
      return;
    this._spike.FinishForever();
  }

  protected override void OnAnimate()
  {
    if (!this._resting)
      this._animation.Animate();
    int num1 = this._flipX ? 1 : 0;
    bool flag = this._velocityX > 0;
    int num2 = flag ? 1 : 0;
    if (num1 == num2)
      return;
    if (this._animation.Index == 2 && !this._turning)
    {
      this._animation.Index = 3;
      this._animation.Cycles = 0;
      this._animation.ResetFrame();
      this._turning = true;
    }
    else
    {
      if (this._animation.Index != 3 || this._animation.Cycles < 1)
        return;
      this._turning = false;
      this._animation.Index = 2;
      this._animation.Cycles = 0;
      this._animation.ResetFrame();
      this._flipX = flag;
    }
  }

  protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
  {
    IObjectRenderer objectRenderer = renderer.GetObjectRenderer();
    using (objectRenderer.BeginMatixState())
    {
      objectRenderer.ModelMatrix *= Matrix4.CreateScale(this._flipX ? -1.0 : 1.0, this._flipY ? -1.0 : 1.0);
      if (this._animation.Index == 3)
      {
        objectRenderer.Render(this._animation, new Vector2(0.0, 8.0), true);
      }
      else
      {
        if (this._animation.Index != 2)
          return;
        if (this._animation.CurrentFrameIndex == 0)
          objectRenderer.Render(this._animation, new Vector2(0.0, 0.0));
        else
          objectRenderer.Render(this._animation);
      }
    }
  }

  private class Spike : Enemy
  {
    public const int AnimationSpikeLeft = 0;
    public const int AnimationSpikeRight = 4;
    private const int AnimationSpikeSpinningLeft = 1;
    private const int AnimationSpikeSpinningRight = 6;
    public const int AnimationSpikeTurnToRight = 5;
    public const int AnimationSpikeTurnToLeft = 7;
    public AnimationInstance _animation;
    private bool _fired;
    private bool _turning;
    public int _velocityX;

    public bool FlipX { get; set; }

    public bool FlipY { get; set; }

    protected override void OnStart()
    {
      base.OnStart();
      this.CollisionRectangles = new CollisionRectangle[1]
      {
        new CollisionRectangle((ActiveObject) this, 0, -32, -72, 64 /*0x40*/, 144 /*0x90*/)
      };
      this._animation = new AnimationInstance(this.ResourceTree, this.Type.GetAbsolutePath("/ANIGROUP"));
    }

    protected override void OnUpdate()
    {
      base.OnUpdate();
      if (!this._fired)
        return;
      this.Position = this.Position + new Vector2i(0, -8 * (this.FlipY ? -1 : 1));
    }

    public void Fire()
    {
      this._animation.Index = this.FlipX ? 6 : 1;
      this._fired = true;
    }

    protected override void OnStop()
    {
      base.OnStop();
      if (!this._fired)
        return;
      this.FinishForever();
    }

    protected override void OnAnimate()
    {
      base.OnAnimate();
      this._animation.Animate();
      int num1 = this.FlipX ? 1 : 0;
      bool flag = this._velocityX > 0;
      int num2 = flag ? 1 : 0;
      if (num1 == num2)
        return;
      if ((this._animation.Index == 4 || this._animation.Index == 0) && !this._turning)
      {
        this._animation.Index = !flag ? 7 : 5;
        this._animation.Cycles = 0;
        this._animation.ResetFrame();
        this._turning = true;
      }
      else
      {
        if (this._animation.Index != 7 && this._animation.Index != 5 || this._animation.Cycles < 1)
          return;
        this._turning = false;
        this.FlipX = flag;
        this._animation.Index = !this.FlipX ? 0 : 4;
        this._animation.Cycles = 0;
        this._animation.ResetFrame();
      }
    }

    protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
    {
      IObjectRenderer objectRenderer = renderer.GetObjectRenderer();
      if (this._animation.Index == 7 || this._animation.Index == 5)
      {
        int x = this._animation.Index == 7 ? -2 : 2;
        objectRenderer.Render(this._animation, (Vector2) new Vector2i(x, 0), flipY: this.FlipY);
      }
      else
      {
        int x = 0;
        if (this._animation.Index == 0 || this._animation.Index == 4)
          x = this._animation.Index == 0 ? 1 : -2;
        objectRenderer.Render(this._animation, new Vector2((double) x, 0.0), flipY: this.FlipY);
      }
    }
  }
}
