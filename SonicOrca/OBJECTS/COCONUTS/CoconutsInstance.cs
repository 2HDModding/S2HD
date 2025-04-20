// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.COCONUTS.CoconutsInstance
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Collision;
using SonicOrca.Core.Objects;
using SonicOrca.Core.Objects.Base;
using SonicOrca.Extensions;
using SonicOrca.Geometry;
using SonicOrca.Graphics;
using System;
using System.Collections.Generic;

namespace SONICORCA.OBJECTS.COCONUTS {

  public class CoconutsInstance : Badnik
  {
    private static readonly IReadOnlyList<CoconutsInstance.ClimbDataItem> ClimbData = (IReadOnlyList<CoconutsInstance.ClimbDataItem>) new CoconutsInstance.ClimbDataItem[6]
    {
      new CoconutsInstance.ClimbDataItem(-1, 32 /*0x20*/),
      new CoconutsInstance.ClimbDataItem(1, 24),
      new CoconutsInstance.ClimbDataItem(-1, 16 /*0x10*/),
      new CoconutsInstance.ClimbDataItem(1, 40),
      new CoconutsInstance.ClimbDataItem(-1, 32 /*0x20*/),
      new CoconutsInstance.ClimbDataItem(1, 16 /*0x10*/)
    };
    private const int AnimationResting = 0;
    private const int AnimationClimbing = 1;
    private const int AnimationTurning = 2;
    private const int AnimationThrowing = 3;
    private AnimationInstance _animation;
    private CoconutsInstance.StateType _state;
    private int _stateDuration;
    private int _throwWaitDuration;
    private int _climbDataIndex;
    private bool _facingRight;

    public CoconutsInstance() => this.DesignBounds = new Rectanglei(-96, -100, 192 /*0xC0*/, 200);

    protected override void OnStart()
    {
      base.OnStart();
      this._animation = new AnimationInstance(this.ResourceTree, this.Type.GetAbsolutePath("/ANIGROUP"));
      this._animation.Index = 0;
      this.CollisionRectangles = new CollisionRectangle[1]
      {
        new CollisionRectangle((ActiveObject) this, 0, -48, -64, 96 /*0x60*/, 128 /*0x80*/)
      };
    }

    protected override void OnUpdate()
    {
      base.OnUpdate();
      switch (this._state)
      {
        case CoconutsInstance.StateType.Resting:
          this.UpdateResting();
          break;
        case CoconutsInstance.StateType.Climbing:
          this.UpdateClimbing();
          break;
        case CoconutsInstance.StateType.PreThrowing:
          this.UpdatePreThrowing();
          break;
        case CoconutsInstance.StateType.PostThrowing:
          this.UpdatePostThrowing();
          break;
      }
    }

    private void UpdateResting()
    {
      ICharacter closestCharacterTo = this.Level.ObjectManager.GetClosestCharacterTo((Vector2) this.Position);
      if (closestCharacterTo != null)
      {
        bool flag = closestCharacterTo.Position.X > this.Position.X;
        if (this._facingRight != flag)
        {
          this._facingRight = flag;
          this._animation.Index = 2;
        }
        double num1 = (double) Math.Abs(this.Position.X - closestCharacterTo.Position.X);
        double num2 = (double) Math.Abs(this.Position.Y - closestCharacterTo.Position.Y);
        if (num1 < 384.0 && num2 < 400.0)
        {
          if (this._throwWaitDuration == 0)
          {
            this._state = CoconutsInstance.StateType.PreThrowing;
            this._stateDuration = 8;
            this._throwWaitDuration = 32 /*0x20*/;
            this._animation.Index = 3;
            return;
          }
          --this._throwWaitDuration;
        }
      }
      --this._stateDuration;
      if (this._stateDuration >= 0)
        return;
      this._state = CoconutsInstance.StateType.Climbing;
      this._animation.Index = 1;
      this.UpdateClimbIndex();
    }

    private void UpdateClimbing()
    {
      --this._stateDuration;
      if (this._stateDuration <= 0)
      {
        this._state = CoconutsInstance.StateType.Resting;
        this._animation.Index = 0;
        this._stateDuration = 16 /*0x10*/;
      }
      else
        this.Position = this.Position + new Vector2i(0, CoconutsInstance.ClimbData[this._climbDataIndex].VelocityY);
    }

    private void UpdatePreThrowing()
    {
      --this._stateDuration;
      if (this._stateDuration >= 0)
        return;
      this.FireProjectile();
      this._state = CoconutsInstance.StateType.PostThrowing;
      this._stateDuration = 8;
    }

    private void UpdatePostThrowing()
    {
      --this._stateDuration;
      if (this._stateDuration >= 0)
        return;
      this._state = CoconutsInstance.StateType.Climbing;
      this._stateDuration = 8;
      this._animation.Index = 1;
      this.UpdateClimbIndex();
    }

    private void UpdateClimbIndex()
    {
      ++this._climbDataIndex;
      if (this._climbDataIndex >= ((IReadOnlyCollection<CoconutsInstance.ClimbDataItem>) CoconutsInstance.ClimbData).Count)
        this._climbDataIndex = 0;
      this._stateDuration = CoconutsInstance.ClimbData[this._climbDataIndex].Duration;
    }

    private void FireProjectile()
    {
      CoconutsInstance.Projectile projectile = this.Level.ObjectManager.AddSubObject<CoconutsInstance.Projectile>((ActiveObject) this);
      Vector2i position = this.Position;
      int x = position.X + (this._facingRight ? -44 : 44);
      position = this.Position;
      int y = position.Y - 52;
      projectile.Position = new Vector2i(x, y);
      projectile.Direction = this._facingRight ? 1 : -1;
    }

    protected override void OnAnimate() => this._animation.Animate();

    protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
    {
      renderer.GetObjectRenderer().Render(this._animation, this._facingRight);
    }

    private struct ClimbDataItem(int velocityY, int duration)
    {
      private readonly int _velocityY = velocityY;
      private readonly int _duration = duration;

      public int VelocityY => this._velocityY;

      public int Duration => this._duration;

      public override string ToString()
      {
        return $"Velocity Y = {this._velocityY} Duration = {this._duration}";
      }
    }

    private enum StateType
    {
      Resting,
      Climbing,
      PreThrowing,
      PostThrowing,
    }

    private class Projectile : Enemy
    {
      private const int AnimationProjectile = 4;
      private const double InitialSpeed = 4.0;
      private const double Gravity = 0.5;
      private AnimationInstance _animation;
      private Vector2 _velocity;

      public int Direction
      {
        get => Math.Sign(this._velocity.X);
        set => this._velocity.X = 4.0 * (double) Math.Sign(value);
      }

      protected override void OnStart()
      {
        base.OnStart();
        this._animation = new AnimationInstance(this.ResourceTree, this.Type.GetAbsolutePath("/ANIGROUP"), 4);
        this._velocity = new Vector2(4.0, -4.0);
        this.CollisionRectangles = new CollisionRectangle[1]
        {
          new CollisionRectangle((ActiveObject) this, 0, -16, -16, 32 /*0x20*/, 32 /*0x20*/)
        };
      }

      protected override void OnUpdate()
      {
        base.OnUpdate();
        this.PositionPrecise = this.PositionPrecise + this._velocity;
        this._velocity.Y += 0.5;
      }

      protected override void OnAnimate() => this._animation.Animate();

      protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
      {
        renderer.GetObjectRenderer().Render(this._animation, this._velocity.X >= 0.0);
      }

      protected override void OnStop()
      {
        base.OnStop();
        this.FinishForever();
      }
    }
  }
}