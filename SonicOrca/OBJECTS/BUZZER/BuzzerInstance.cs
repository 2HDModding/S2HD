// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.BUZZER.BuzzerInstance
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
using System;

namespace SONICORCA.OBJECTS.BUZZER {

  public class BuzzerInstance : Badnik
  {
    private const int AnimationMoving = 0;
    private const int AnimationTurning = 1;
    private const int AnimationFiring = 2;
    private AnimationInstance _animation;
    private int _velocityX;
    private BuzzerInstance.StatusType _status;
    private int _flightDuration;
    private int _turnWaitDuration;
    private int _fireDuration;
    private bool _firedThisRound;

    public BuzzerInstance() => this.DesignBounds = new Rectanglei(-130, -84, 260, 168);

    protected override void OnStart()
    {
      base.OnStart();
      this._animation = new AnimationInstance(this.ResourceTree, this.Type.GetAbsolutePath("/ANIGROUP"));
      this._animation.Index = 0;
      this._velocityX = -4;
      this._status = BuzzerInstance.StatusType.Moving;
      this._flightDuration = 256 /*0x0100*/;
      this._turnWaitDuration = 0;
      this._fireDuration = 0;
      this._firedThisRound = false;
      this.CollisionRectangles = new CollisionRectangle[1]
      {
        new CollisionRectangle((ActiveObject) this, 0, -64, -32, 128 /*0x80*/, 64 /*0x40*/)
      };
    }

    protected override void OnUpdate()
    {
      base.OnUpdate();
      switch (this._status)
      {
        case BuzzerInstance.StatusType.Moving:
          this.UpdateMoving();
          break;
        case BuzzerInstance.StatusType.Firing:
          this.UpdateFiring();
          break;
      }
    }

    private void UpdateMoving()
    {
      if (!this._firedThisRound && this.CheckForCharacter())
      {
        this._animation.Index = 2;
        this._status = BuzzerInstance.StatusType.Firing;
        this._fireDuration = 50;
        this._firedThisRound = true;
      }
      else if (this._turnWaitDuration > 0)
      {
        --this._turnWaitDuration;
        if (this._turnWaitDuration != 15)
          return;
        this._velocityX *= -1;
        this._animation.Index = 1;
        this._firedThisRound = false;
      }
      else
      {
        --this._flightDuration;
        if (this._flightDuration > 0)
        {
          this.Position = this.Position + new Vector2i(this._velocityX, 0);
        }
        else
        {
          this._flightDuration = 256 /*0x0100*/;
          this._turnWaitDuration = 30;
        }
      }
    }

    private void UpdateFiring()
    {
      --this._fireDuration;
      if (this._fireDuration < 0)
      {
        this._status = BuzzerInstance.StatusType.Moving;
      }
      else
      {
        if (this._fireDuration != 20)
          return;
        this.FireProjectile();
      }
    }

    private void FireProjectile()
    {
      BuzzerInstance.Projectile projectile = this.Level.ObjectManager.AddSubObject<BuzzerInstance.Projectile>((ActiveObject) this);
      Vector2i position = this.Position;
      int x = position.X + Math.Sign(this._velocityX) * -52;
      position = this.Position;
      int y = position.Y + 96 /*0x60*/;
      projectile.Position = new Vector2i(x, y);
      projectile.Direction = Math.Sign(this._velocityX);
    }

    private bool CheckForCharacter()
    {
      foreach (ICharacter character in this.Level.ObjectManager.Characters)
      {
        Vector2i position = this.Position;
        int y1 = position.Y;
        position = character.Position;
        int y2 = position.Y;
        if (y1 < y2)
        {
          position = this.Position;
          int y3 = position.Y;
          position = character.Position;
          int num = position.Y - 1024 /*0x0400*/;
          if (y3 >= num)
          {
            position = this.Position;
            int x1 = position.X;
            position = character.Position;
            int x2 = position.X;
            if (MathX.IsBetween(160.0, (double) Math.Abs(x1 - x2), 192.0))
              return true;
            continue;
          }
        }
        return false;
      }
      return false;
    }

    protected override void OnAnimate() => this._animation.Animate();

    protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
    {
      renderer.GetObjectRenderer().Render(this._animation, this._velocityX >= 0);
    }

    private enum StatusType
    {
      Moving,
      Firing,
    }

    private class Projectile : Enemy
    {
      private const int AnimationProjectile = 3;
      private const int Speed = 6;
      private AnimationInstance _animation;
      private Vector2i _velocity;

      public int Direction
      {
        get => Math.Sign(this._velocity.X);
        set => this._velocity.X = 6 * Math.Sign(value);
      }

      protected override void OnStart()
      {
        base.OnStart();
        this._animation = new AnimationInstance(this.ResourceTree, this.Type.GetAbsolutePath("/ANIGROUP"), 3);
        this._velocity = new Vector2i(6);
        this.CollisionRectangles = new CollisionRectangle[1]
        {
          new CollisionRectangle((ActiveObject) this, 0, -16, -16, 32 /*0x20*/, 32 /*0x20*/)
        };
      }

      protected override void OnUpdate()
      {
        base.OnUpdate();
        this.Position = this.Position + this._velocity;
      }

      protected override void OnAnimate() => this._animation.Animate();

      protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
      {
        IObjectRenderer objectRenderer = renderer.GetObjectRenderer();
        objectRenderer.EmitsLight = true;
        objectRenderer.Render(this._animation, this._velocity.X >= 0);
      }

      protected override void OnStop()
      {
        base.OnStop();
        this.FinishForever();
      }
    }
  }
}