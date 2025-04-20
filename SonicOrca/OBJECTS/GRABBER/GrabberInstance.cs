// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.GRABBER.GrabberInstance
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

namespace SONICORCA.OBJECTS.GRABBER
{

    public class GrabberInstance : Badnik
    {
      private const int AnimationBodyStationary = 0;
      private const int AnimationBodyStationaryWiggle = 1;
      private const int AnimationBodyTurnBegin = 2;
      private const int AnimationBodyTurnEnd = 3;
      private const int AnimationBodyOpen = 4;
      private const int AnimationBodyClose = 5;
      private const int AnimationBodyHold = 6;
      private const int AnimationLegsStationary = 7;
      private const int AnimationLegsOpen = 8;
      private const int AnimationLegsClose = 9;
      private const int AnimationLegsHold = 10;
      private const int AnimationLegsHoldWiggle = 11;
      private const int AnimationWheels = 12;
      private const int AnimationThread = 13;
      private const int AnimationBodyExplode = 14;
      private AnimationGroup _animationGroup;
      private AnimationInstance _animationBody;
      private AnimationInstance _animationLegs;
      private AnimationInstance _animationWheels;
      private AnimationInstance _animationThread;
      private AnimationInstance _animationBodyExplode;
      private bool _showLegs;
      private GrabberInstance.Legs _legsSubObject;
      private float _explodeOpacity;
      private float _explodeOpacityAngle;
      private float _explodeOpacityAngleVelocity;
      private const int MoveSpeed = 1;
      private const int LowerSpeed = 8;
      private readonly Vector2i CharacterGrabOffset = new Vector2i(-8, 64 /*0x40*/);
      private GrabberInstance.StatusType _status;
      private int _direction;
      private int _patrolTicks;
      private int _ticksRemaining;
      private int _lowerY;
      private int _regrabTicksCounter;
      private bool _canGrabAgain;
      private bool _playerEscaped;
      private int _grabEscapeProtagonistCounter;
      private int _lastEscapeProtagonistInput;
      private int _grabEscapeSidekickCounter;
      private int _lastEscapeSidekickInput;
      private int _previousCharacterLayer;
      private ICharacter _grabbedCharacter;

      public GrabberInstance()
      {
        this.DesignBounds = new Rectanglei((int) sbyte.MinValue, (int) sbyte.MinValue, 256 /*0x0100*/, 256 /*0x0100*/);
        this.CollisionRectangles = new CollisionRectangle[2]
        {
          new CollisionRectangle((ActiveObject) this, 0, -16, -16, 32 /*0x20*/, 32 /*0x20*/),
          new CollisionRectangle((ActiveObject) this, 1, this.CharacterGrabOffset.X - 16 /*0x10*/, this.CharacterGrabOffset.Y - 8, 32 /*0x20*/, 32 /*0x20*/)
        };
      }

      protected override void OnStart()
      {
        base.OnStart();
        this._regrabTicksCounter = 0;
        this._canGrabAgain = true;
        this._playerEscaped = false;
        this._grabEscapeProtagonistCounter = 0;
        this._lastEscapeProtagonistInput = 0;
        this._grabEscapeSidekickCounter = 0;
        this._lastEscapeSidekickInput = 0;
        this._patrolTicks = (int) byte.MaxValue;
        this._direction = -1;
        this._animationGroup = this.ResourceTree.GetLoadedResource<AnimationGroup>(this.Type.GetAbsolutePath("/ANIGROUP"));
        this._animationBody = new AnimationInstance(this._animationGroup);
        this._animationLegs = new AnimationInstance(this._animationGroup, 7);
        this._animationWheels = new AnimationInstance(this._animationGroup, 12);
        this._animationThread = new AnimationInstance(this._animationGroup, 13);
        this._animationBodyExplode = new AnimationInstance(this._animationGroup, 14);
        this._legsSubObject = this.Level.ObjectManager.AddSubObject<GrabberInstance.Legs>((ActiveObject) this);
      }

      protected override void OnStop()
      {
        base.OnStop();
        this.ReleaseCharacter();
      }

      protected override void OnUpdate()
      {
        base.OnUpdate();
        if (this._grabbedCharacter != null)
        {
          if (this._grabbedCharacter == this.Level.Player.Protagonist)
          {
            if (this.Level.Player.Protagonist.Input.HorizontalDirection != this._lastEscapeProtagonistInput)
            {
              this._lastEscapeProtagonistInput = this.Level.Player.Protagonist.Input.HorizontalDirection;
              ++this._grabEscapeProtagonistCounter;
            }
            if (this._grabEscapeProtagonistCounter >= 8)
              this.ReleaseCharacter();
          }
          if (this.Level.Player.Sidekick != null && this._grabbedCharacter == this.Level.Player.Sidekick)
          {
            if (this.Level.Player.Sidekick.Input.HorizontalDirection != this._lastEscapeSidekickInput)
            {
              this._lastEscapeSidekickInput = this.Level.Player.Sidekick.Input.HorizontalDirection;
              ++this._grabEscapeSidekickCounter;
            }
            if (this._lastEscapeSidekickInput >= 8)
              this.ReleaseCharacter();
          }
          if (this._grabbedCharacter == null)
          {
            this._lastEscapeSidekickInput = 0;
            this._lastEscapeProtagonistInput = 0;
            this._grabEscapeProtagonistCounter = 0;
            this._lastEscapeSidekickInput = 0;
            this._status = GrabberInstance.StatusType.Raising;
            this._regrabTicksCounter = 0;
            this._playerEscaped = true;
            this._canGrabAgain = false;
          }
        }
        if (this._playerEscaped)
        {
          if (this._regrabTicksCounter <= 30)
          {
            ++this._regrabTicksCounter;
          }
          else
          {
            this._playerEscaped = false;
            this._canGrabAgain = true;
          }
        }
        if (this._grabbedCharacter != null)
        {
          if (this._grabbedCharacter.ObjectLink != this || this._grabbedCharacter.IsDebug)
          {
            this.ReleaseCharacter();
            this._status = GrabberInstance.StatusType.Raising;
          }
          else
          {
            this._grabbedCharacter.Position = this.Position + this.CharacterGrabOffset * new Vector2i(this._direction, 1);
            this._grabbedCharacter.Facing = this._direction * -1;
          }
        }
        switch (this._status)
        {
          case GrabberInstance.StatusType.Patroling:
            if (this.DetectPlayer())
            {
              this._status = GrabberInstance.StatusType.Detected;
              this._ticksRemaining = 16 /*0x10*/;
              break;
            }
            if (this._patrolTicks == 0)
            {
              this._direction *= -1;
              this._patrolTicks = (int) byte.MaxValue;
            }
            else
              --this._patrolTicks;
            this.Move(this._direction, 0);
            break;
          case GrabberInstance.StatusType.Detected:
            --this._ticksRemaining;
            if (this._ticksRemaining > 0)
              break;
            this._status = GrabberInstance.StatusType.Lowering;
            break;
          case GrabberInstance.StatusType.Lowering:
            if (this._lowerY < 264 && this._grabbedCharacter == null)
            {
              this.Move(0, Math.Min(8, 264 - this._lowerY));
              this._lowerY += 8;
              break;
            }
            this._status = GrabberInstance.StatusType.Raising;
            if (this._grabbedCharacter == null)
              break;
            this._ticksRemaining = 20;
            break;
          case GrabberInstance.StatusType.Raising:
            if (this._ticksRemaining > 0)
            {
              --this._ticksRemaining;
              break;
            }
            if (this._lowerY > 0)
            {
              this.Move(0, -Math.Min(8, this._lowerY));
              this._lowerY -= 8;
              break;
            }
            if (this._grabbedCharacter != null)
            {
              this._status = GrabberInstance.StatusType.Exploding;
              this._ticksRemaining = 130;
              break;
            }
            this._status = GrabberInstance.StatusType.Patroling;
            this._ticksRemaining = 0;
            break;
          case GrabberInstance.StatusType.Exploding:
            --this._ticksRemaining;
            if (this._ticksRemaining != 0)
              break;
            if (this._grabbedCharacter != null)
            {
              this._grabbedCharacter.Hurt(this._direction);
              this.ReleaseCharacter();
              this.CreateExplosionObject();
              this.FinishForever();
            }
            this._status = GrabberInstance.StatusType.Patroling;
            break;
        }
      }

      private bool DetectPlayer()
      {
        foreach (ICharacter character in this.Level.ObjectManager.Characters)
        {
          if (character.ShouldReactToLevel && (double) (character.Position.Y - this.Position.Y) >= 0.0 && (double) Math.Abs(character.Position.X - this.Position.X) < (double) (this.CollisionRectangles[1].Bounds.Width / 4))
            return true;
        }
        return false;
      }

      private void GrabCharacter(ICharacter character)
      {
        this._grabbedCharacter = character;
        this._grabbedCharacter.LeaveGround();
        character.IsAirborne = true;
        character.CheckCollision = false;
        character.ObjectLink = (ActiveObject) this;
        character.Position = this.Position + this.CharacterGrabOffset;
        character.Facing = this._direction * -1;
        character.SpecialState = CharacterSpecialState.Grabbed;
        this._ticksRemaining = 0;
        this._previousCharacterLayer = this.Level.Map.Layers.IndexOf(this._grabbedCharacter.Layer);
        this._grabbedCharacter.Layer = this.Layer;
      }

      private void ReleaseCharacter()
      {
        if (this._grabbedCharacter == null)
          return;
        this._explodeOpacity = 0.0f;
        this._explodeOpacityAngleVelocity = 0.0f;
        this._animationBody.ResetFrame();
        this._animationBody.Cycles = 0;
        this._grabbedCharacter.Layer = this.Level.Map.Layers[this._previousCharacterLayer];
        this._grabbedCharacter.SpecialState = CharacterSpecialState.Normal;
        this._grabbedCharacter.Velocity = new Vector2();
        this._grabbedCharacter.IsSpinball = true;
        this._grabbedCharacter.IsAirborne = true;
        this._grabbedCharacter.CheckCollision = true;
        this._grabbedCharacter.ObjectLink = (ActiveObject) null;
        this._grabbedCharacter = (ICharacter) null;
      }

      protected override void OnCollision(CollisionEvent e)
      {
        if (e.ActiveObject.Type.Classification != ObjectClassification.Character)
          return;
        ICharacter activeObject = (ICharacter) e.ActiveObject;
        if (e.Id == 0)
        {
          if (!activeObject.IsDeadly)
            return;
          this.ReleaseCharacter();
          this.Destroy(activeObject);
        }
        else
        {
          if (this._grabbedCharacter != null || this._status == GrabberInstance.StatusType.Patroling || !this._canGrabAgain || activeObject.IsAirborne)
            return;
          this.GrabCharacter(activeObject);
          e.MaintainVelocity = true;
        }
      }

      protected override void OnAnimate()
      {
        if (this.Level.State == LevelState.Editing)
        {
          this._animationBody.Animate();
          this._animationLegs.Animate();
          this._showLegs = true;
        }
        else
        {
          switch (this._status)
          {
            case GrabberInstance.StatusType.Patroling:
              if (this._patrolTicks < this._animationGroup[2].Duration)
              {
                this._animationBody.Index = 2;
                break;
              }
              if (this._patrolTicks == (int) byte.MaxValue)
              {
                this._animationBody.Index = 3;
                break;
              }
              if ((this._patrolTicks == 84 || this._patrolTicks == 168) && this.Level.Random.Next(0, 2) == 0)
              {
                this._animationBody.Index = 1;
                break;
              }
              break;
            case GrabberInstance.StatusType.Lowering:
              this._animationBody.Index = 4;
              break;
            case GrabberInstance.StatusType.Raising:
              if (this._animationBody.Index == 4)
              {
                this._animationBody.Index = 5;
                break;
              }
              break;
            case GrabberInstance.StatusType.Exploding:
              this._explodeOpacityAngleVelocity = (float) MathX.Clamp(Math.PI / 30.0, (double) this._explodeOpacityAngleVelocity + Math.PI / 600.0, Math.PI / 4.0);
              this._explodeOpacityAngle += this._explodeOpacityAngleVelocity;
              this._explodeOpacity = (float) MathX.Clamp(0.0, (1.0 - Math.Cos((double) this._explodeOpacityAngle)) / 2.0, 1.0);
              break;
          }
          if (this._grabbedCharacter != null)
            this._animationBody.Index = 6;
          this._animationBody.Animate();
          this._animationLegs.Animate();
          if (Math.Abs(this.PositionPrecise.Y - this.LastPositionPrecise.Y) > 0.0)
            this._animationWheels.Animate();
          this._animationThread.Animate();
          switch (this._animationBody.Index)
          {
            case 1:
            case 2:
            case 3:
              this._showLegs = false;
              break;
            case 4:
              this._animationLegs.Index = 8;
              this._showLegs = true;
              break;
            case 6:
              this._animationLegs.Index = this._grabbedCharacter == null || !this._grabbedCharacter.Entry.Name.Contains("Tails") ? 11 : 10;
              this._showLegs = true;
              break;
            case 9:
              this._animationLegs.Index = 9;
              this._showLegs = true;
              break;
            default:
              this._animationLegs.Index = 7;
              this._showLegs = true;
              break;
          }
        }
      }

      protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
      {
        IObjectRenderer objectRenderer = renderer.GetObjectRenderer();
        bool flag = this._direction < 0;
        using (objectRenderer.BeginMatixState())
        {
          if (flag)
            objectRenderer.ModelMatrix *= Matrix4.CreateScale(-1.0, 1.0);
          objectRenderer.Render(this._animationBody);
          if ((double) this._explodeOpacity > 0.0)
          {
            objectRenderer.MultiplyColour = new Colour((double) this._explodeOpacity, 1.0, 1.0, 1.0);
            objectRenderer.Render(this._animationBodyExplode);
            objectRenderer.MultiplyColour = Colours.White;
          }
          if (this._showLegs)
            objectRenderer.Render(this._animationLegs);
          if (this._lowerY > 0)
          {
            Rectanglei source = this._animationThread.CurrentFrame.Source;
            objectRenderer.Render((Rectangle) source, new Rectangle((double) (-source.Width / 2 - 6), (double) (-this._lowerY - 64 /*0x40*/), (double) source.Width, (double) this._lowerY));
          }
          objectRenderer.Render(this._animationWheels);
        }
      }

      private enum StatusType
      {
        Patroling,
        Detected,
        Lowering,
        Raising,
        Exploding,
      }

      private class Legs : ActiveObject
      {
        public GrabberInstance Parent => this.ParentObject as GrabberInstance;

        protected override void OnStart()
        {
          this.Priority = 1152;
          this.LockLifetime = true;
        }

        protected override void OnUpdate()
        {
          if (this.Parent.Finished)
            this.Finish();
          else
            this.Position = this.Parent.Position;
        }

        protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
        {
          GrabberInstance parent = this.Parent;
          if (!parent._showLegs)
            return;
          IObjectRenderer objectRenderer = renderer.GetObjectRenderer();
          bool flag = parent._direction < 0;
          using (objectRenderer.BeginMatixState())
          {
            if (flag)
              objectRenderer.ModelMatrix *= Matrix4.CreateScale(-1.0, 1.0);
            objectRenderer.Render(parent._animationLegs);
          }
        }
      }
    }
}
