// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.SPRING.SpringInstance
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Collision;
using SonicOrca.Core.Objects.Base;
using SonicOrca.Core.Objects.Metadata;
using SonicOrca.Extensions;
using SonicOrca.Geometry;
using SonicOrca.Graphics;
using System;

namespace SONICORCA.OBJECTS.SPRING
{

    public class SpringInstance : ActiveObject
    {
      private SpringDirection _direction = SpringDirection.Right;
      private int _strength = 40;
      private AnimationInstance _animation;
      private int _bounceAnimationIndex;
      private Vector2 _bounceVelocity;

      [StateVariable]
      private SpringDirection Direction
      {
        get => this._direction;
        set => this._direction = value;
      }

      [StateVariable]
      private int Strength
      {
        get => this._strength;
        set => this._strength = value;
      }

      public SpringInstance() => this.DesignBounds = new Rectanglei(-101, -104, 202, 208 /*0xD0*/);

      protected override void OnStart()
      {
        this._animation = new AnimationInstance(this.ResourceTree, this.Type.GetAbsolutePath("/ANIGROUP"));
        this._animation.Index = this._strength >= 52 ? 0 : 2;
        if ((int) this._direction % 2 == 1)
          this._animation.Index += 4;
        this._bounceAnimationIndex = this._animation.Index + 1;
        switch (this._direction)
        {
          case SpringDirection.Up:
            this.CollisionVectors = new CollisionVector[6]
            {
              new CollisionVector((ActiveObject) this, new Vector2i(-64, 64 /*0x40*/), new Vector2i(-64, 8)),
              new CollisionVector((ActiveObject) this, new Vector2i(-64, 8), new Vector2i(-56, 0)),
              new CollisionVector((ActiveObject) this, new Vector2i(-56, 0), new Vector2i(56, 0)),
              new CollisionVector((ActiveObject) this, new Vector2i(56, 0), new Vector2i(64 /*0x40*/, 8)),
              new CollisionVector((ActiveObject) this, new Vector2i(64 /*0x40*/, 8), new Vector2i(64 /*0x40*/, 64 /*0x40*/)),
              new CollisionVector((ActiveObject) this, new Vector2i(-64, 64 /*0x40*/), new Vector2i(64 /*0x40*/, 64 /*0x40*/))
            };
            this.CollisionVectors[1].Id = 1;
            this.CollisionVectors[2].Id = 1;
            this.CollisionVectors[3].Id = 1;
            this._bounceVelocity = new Vector2(0.0, (double) -this._strength);
            break;
          case SpringDirection.UpRight:
            this.CollisionVectors = new CollisionVector[6]
            {
              new CollisionVector((ActiveObject) this, new Vector2i(-48, -48), new Vector2i(48 /*0x30*/, 48 /*0x30*/)),
              new CollisionVector((ActiveObject) this, new Vector2i(-48, -84), new Vector2i(-48, -48)),
              new CollisionVector((ActiveObject) this, new Vector2i(-84, -84), new Vector2i(-48, -84)),
              new CollisionVector((ActiveObject) this, new Vector2i(-84, 80 /*0x50*/), new Vector2i(-84, -48)),
              new CollisionVector((ActiveObject) this, new Vector2i(48 /*0x30*/, 80 /*0x50*/), new Vector2i(-84, 80 /*0x50*/)),
              new CollisionVector((ActiveObject) this, new Vector2i(48 /*0x30*/, 48 /*0x30*/), new Vector2i(48 /*0x30*/, 80 /*0x50*/))
            };
            this.CollisionVectors[0].Id = 1;
            this.CollisionVectors[1].Id = 1;
            this.CollisionVectors[2].Id = 1;
            this.CollisionVectors[5].Id = 1;
            this._bounceVelocity = new Vector2((double) this._strength, (double) -this._strength);
            break;
          case SpringDirection.Right:
            this.CollisionVectors = CollisionVector.FromRectangle((ActiveObject) this, (Rectanglei) new Rectangle(-64.0, -60.0, 64.0, 128.0));
            this.CollisionVectors[2].Id = 1;
            this._bounceVelocity = new Vector2((double) this._strength, 0.0);
            break;
          case SpringDirection.Down:
            this.CollisionVectors = CollisionVector.FromRectangle((ActiveObject) this, (Rectanglei) new Rectangle(-64.0, -69.0, 128.0, 64.0));
            this.CollisionVectors[3].Id = 1;
            this._bounceVelocity = new Vector2(0.0, (double) this._strength);
            break;
          case SpringDirection.Left:
            this.CollisionVectors = CollisionVector.FromRectangle((ActiveObject) this, (Rectanglei) new Rectangle(5.0, -64.0, 64.0, 128.0));
            this.CollisionVectors[0].Id = 1;
            this._bounceVelocity = new Vector2((double) -this._strength, 0.0);
            break;
          case SpringDirection.UpLeft:
            this.CollisionVectors = new CollisionVector[5]
            {
              new CollisionVector((ActiveObject) this, new Vector2i(-48, 48 /*0x30*/), new Vector2i(48 /*0x30*/, -48)),
              new CollisionVector((ActiveObject) this, new Vector2i(48 /*0x30*/, -48), new Vector2i(84, -48)),
              new CollisionVector((ActiveObject) this, new Vector2i(84, -48), new Vector2i(84, 80 /*0x50*/)),
              new CollisionVector((ActiveObject) this, new Vector2i(84, 80 /*0x50*/), new Vector2i(-48, 80 /*0x50*/)),
              new CollisionVector((ActiveObject) this, new Vector2i(-48, 80 /*0x50*/), new Vector2i(-48, 48 /*0x30*/))
            };
            this.CollisionVectors[0].Id = 1;
            this.CollisionVectors[1].Id = 1;
            this._bounceVelocity = new Vector2((double) -this._strength, (double) -this._strength);
            break;
        }
        this.Priority = -256;
      }

      protected override void OnCollision(CollisionEvent e)
      {
        if (e.Id != 1 || e.ActiveObject.Type.Classification != ObjectClassification.Character)
          return;
        Character activeObject = (Character) e.ActiveObject;
        switch (this.Direction)
        {
          case SpringDirection.Up:
          case SpringDirection.UpRight:
          case SpringDirection.UpLeft:
            activeObject.LeaveGround();
            break;
        }
        if (this._direction == SpringDirection.Up || this._direction == SpringDirection.UpRight || this._direction == SpringDirection.UpLeft)
        {
          activeObject.IsAirborne = true;
          activeObject.IsSpinball = false;
          if (this._direction == SpringDirection.Up)
          {
            activeObject.TumbleAngle = 0.0;
            activeObject.TumbleTurns = 1;
            activeObject.GroundVelocity = 1.0;
          }
          else
            activeObject.Animation.Index = 12;
        }
        if (activeObject.IsAirborne)
        {
          Character character = activeObject;
          Vector2 velocity;
          double x;
          if (this._bounceVelocity.X == 0.0)
          {
            velocity = activeObject.Velocity;
            x = velocity.X;
          }
          else
            x = this._bounceVelocity.X;
          double y;
          if (this._bounceVelocity.Y == 0.0)
          {
            velocity = activeObject.Velocity;
            y = velocity.Y;
          }
          else
            y = this._bounceVelocity.Y;
          Vector2 vector2 = new Vector2(x, y);
          character.Velocity = vector2;
          activeObject.IsHurt = false;
        }
        else
        {
          activeObject.GroundVelocity = this._bounceVelocity.X;
          activeObject.Facing = Math.Sign(this._bounceVelocity.X);
          activeObject.SlopeLockTicks = Math.Max(16 /*0x10*/, activeObject.SlopeLockTicks);
          activeObject.IsPushing = false;
        }
        this._animation.Index = this._bounceAnimationIndex;
        e.MaintainVelocity = true;
        this.Level.SoundManager.PlaySound((IActiveObject) this, this.Type.GetAbsolutePath("SONICORCA/SOUND/SPRING"));
      }

      protected override void OnAnimate() => this._animation.Animate();

      protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
      {
        IObjectRenderer objectRenderer = renderer.GetObjectRenderer();
        using (objectRenderer.BeginMatixState())
        {
          switch (this._direction)
          {
            case SpringDirection.Up:
              objectRenderer.Render(this._animation);
              break;
            case SpringDirection.UpRight:
              objectRenderer.Render(this._animation);
              break;
            case SpringDirection.Right:
              objectRenderer.ModelMatrix *= Matrix4.CreateRotationZ(Math.PI / 2.0);
              objectRenderer.Render(this._animation, true);
              break;
            case SpringDirection.DownRight:
              objectRenderer.Render(this._animation);
              break;
            case SpringDirection.Down:
              objectRenderer.Render(this._animation, flipY: true);
              break;
            case SpringDirection.DownLeft:
              objectRenderer.Render(this._animation, flipY: true);
              break;
            case SpringDirection.Left:
              objectRenderer.ModelMatrix *= Matrix4.CreateRotationZ(Math.PI / 2.0);
              objectRenderer.Render(this._animation, true, true);
              break;
            case SpringDirection.UpLeft:
              objectRenderer.Render(this._animation, true);
              break;
          }
        }
      }
    }
}
