// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.MONITOR.MonitorInstance
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Collision;
using SonicOrca.Core.Objects;
using SonicOrca.Core.Objects.Metadata;
using SonicOrca.Extensions;
using SonicOrca.Geometry;
using SonicOrca.Graphics;
using SonicOrca.Resources;

namespace SONICORCA.OBJECTS.MONITOR
{

    public class MonitorInstance : ActiveObject
    {
      private const int AnimationShell = 0;
      private const int AnimationShellOverlay = 1;
      private const int AnimationBrokenShell = 2;
      private const int AnimationBrokenShellOverlay = 3;
      private const int AnimationScreenOverlay = 4;
      private const int AnimationScreenContentsLifeSonic = 5;
      private const int AnimationScreenContentsLifeTails = 6;
      private const int AnimationScreenContentsRobotnik = 7;
      private const int AnimationScreenContentsRing = 8;
      private const int AnimationScreenContentsSpeedShoesSonic = 9;
      private const int AnimationScreenContentsSpeedShoesTails = 10;
      private const int AnimationScreenContentsBarrier = 11;
      private const int AnimationScreenContentsInvincibility = 12;
      private const int AnimationScreenContentsSwapPlaces = 13;
      private const int AnimationScreenContentsRandom = 14;
      private const int AnimationContentsLifeSonic = 15;
      private const int AnimationContentsLifeTails = 16 /*0x10*/;
      private const int AnimationContentsRobotnik = 17;
      private const int AnimationContentsRing = 18;
      private const int AnimationContentsSpeedShoesSonic = 19;
      private const int AnimationContentsSpeedShoesTails = 20;
      private const int AnimationContentsBarrier = 21;
      private const int AnimationContentsInvincibility = 22;
      private const int AnimationContentsSwapPlaces = 23;
      private const int AnimationContentsRandom = 24;
      private MonitorContents _contents;
      private bool _broken;
      private AnimationInstance _shellAnimation;
      private AnimationInstance _shellOverlayAnimation;
      private AnimationInstance _screenAnimation;
      private AnimationInstance _screenOverlayAnimation;
      private bool _stationary;
      private Vector2 _velocity;
      private AnimationInstance _contentsAnimation;
      private Vector2 _contentsVelocity;
      private Vector2 _contentsPosition;
      private bool _raisingContents;
      private ICharacter _breaker;

      private static int GetScreenContentsAnimationIndex(MonitorContents contents, int character)
      {
        switch (contents)
        {
          case MonitorContents.Life:
            return 5 + character - 1;
          case MonitorContents.Ring:
            return 8;
          case MonitorContents.SpeedShoes:
            return 9 + character - 1;
          case MonitorContents.Barrier:
            return 11;
          case MonitorContents.Invincibility:
            return 12;
          case MonitorContents.Robotnik:
            return 7;
          case MonitorContents.SwapPlaces:
            return 13;
          default:
            return 14;
        }
      }

      private static int GetContentsAnimationIndex(MonitorContents contents, int character)
      {
        return MonitorInstance.GetScreenContentsAnimationIndex(contents, character);
      }

      [StateVariable]
      private MonitorContents Contents
      {
        get => this._contents;
        set => this._contents = value;
      }

      [StateVariable]
      private bool Broken
      {
        get => this._broken;
        set => this._broken = value;
      }

      public MonitorInstance() => this.DesignBounds = new Rectanglei(-60, -60, 120, 120);

      protected override void OnStart()
      {
        this._stationary = true;
        AnimationGroup loadedResource = this.ResourceTree.GetLoadedResource<AnimationGroup>((ILoadedResource) this.Type, "/ANIGROUP");
        this._shellAnimation = new AnimationInstance(loadedResource);
        this._shellOverlayAnimation = new AnimationInstance(loadedResource);
        this._screenAnimation = new AnimationInstance(loadedResource);
        this._screenOverlayAnimation = new AnimationInstance(loadedResource);
        this._shellAnimation.Index = this._broken ? 2 : 0;
        this._shellOverlayAnimation.Index = this._broken ? 3 : 1;
        this._screenAnimation.Index = MonitorInstance.GetScreenContentsAnimationIndex(this._contents, (int) this.Level.Player.ProtagonistCharacterType);
        this._screenOverlayAnimation.Index = 4;
        if (this._broken)
          return;
        this._contentsAnimation = new AnimationInstance(loadedResource);
        this._contentsAnimation.Index = MonitorInstance.GetContentsAnimationIndex(this._contents, (int) this.Level.Player.ProtagonistCharacterType);
        this.CollisionVectors = CollisionVector.FromRectangle((ActiveObject) this, (Rectanglei) new Rectangle(-60.0, -60.0, 120.0, 120.0));
        this.CollisionVectors[3].Id = 1;
      }

      protected override void OnUpdate()
      {
        if (!this._stationary)
        {
          this.PositionPrecise = this.PositionPrecise + this._velocity;
          bool flag = false;
          foreach (CollisionInfo collision in CollisionVector.GetCollisions((ActiveObject) this, 60))
          {
            if (collision.Vector.Owner == null && !collision.Vector.IsWall)
            {
              Vector2 positionPrecise = this.PositionPrecise;
              double x = positionPrecise.X;
              positionPrecise = this.PositionPrecise;
              double y = positionPrecise.Y + collision.Shift;
              this.PositionPrecise = new Vector2(x, y);
              flag = true;
            }
          }
          if (flag)
          {
            this.Entry.Position = this.Position;
            this._stationary = true;
          }
          else
            this._velocity += new Vector2(0.0, 0.875);
        }
        if (!this._raisingContents)
          return;
        this._contentsPosition += this._contentsVelocity;
        this._contentsVelocity.Y += 0.375;
        if (this._contentsVelocity.Y < 0.0)
          return;
        this._raisingContents = false;
        this.InvokeContents();
      }

      protected override void OnCollision(CollisionEvent e)
      {
        if (e.ActiveObject.Type.Classification != ObjectClassification.Character)
          return;
        ICharacter activeObject = (ICharacter) e.ActiveObject;
        if (e.Id == 1)
        {
          this._stationary = false;
          this._velocity.Y = -6.0;
          activeObject.Velocity = new Vector2(activeObject.Velocity.X, activeObject.Velocity.Y * -1.0);
          e.MaintainVelocity = true;
        }
        else
        {
          if (!this.CanCharacterBreak(activeObject))
            return;
          this.Break(activeObject);
          e.IgnoreCollision = true;
        }
      }

      private bool CanCharacterBreak(ICharacter character)
      {
        return character.Velocity.Y >= 0.0 && !character.IsSidekick && character.IsSpinball;
      }

      private void Break(ICharacter character)
      {
        this._breaker = character;
        ICharacter character1 = character;
        Vector2 velocity = character.Velocity;
        double x = velocity.X;
        velocity = character.Velocity;
        double y = velocity.Y * -1.0;
        Vector2 vector2 = new Vector2(x, y);
        character1.Velocity = vector2;
        if (character.Jumped)
          character.IsJumping = true;
        this._broken = true;
        (this.Entry.State as MonitorInstance).Broken = true;
        this._shellAnimation.Index = 2;
        this._shellOverlayAnimation.Index = 3;
        this.CollisionVectors = new CollisionVector[0];
        this.CollisionRectangles = new CollisionRectangle[0];
        this.RegisterCollisionUpdate();
        this.CreateExplosionObject();
        this.CreateRaisingContents();
        this.LockLifetime = true;
      }

      private void CreateExplosionObject()
      {
        this.Level.ObjectManager.AddObject(new ObjectPlacement(this.Type.GetAbsolutePath("SONICORCA/OBJECTS/EXPLOSION/BADNIK"), this.Level.Map.Layers.IndexOf(this.Layer), this.Position));
      }

      private void CreateRaisingContents()
      {
        this._contentsVelocity = new Vector2(0.0, -12.0);
        this._contentsPosition = new Vector2(0.0, -13.0);
        this._raisingContents = true;
      }

      private void InvokeContents()
      {
        this.LockLifetime = false;
        switch (this._contents)
        {
          case MonitorContents.Life:
            this._breaker.Player.GainLives();
            break;
          case MonitorContents.Ring:
            this._breaker.Player.GainRings(10);
            this.Level.SoundManager.PlaySound((IActiveObject) this._breaker, "SONICORCA/SOUND/RING");
            break;
          case MonitorContents.SpeedShoes:
            this._breaker.Player.GiveSpeedShoes();
            break;
          case MonitorContents.Barrier:
            this._breaker.Player.GiveBarrier(BarrierType.Classic);
            this.Level.SoundManager.PlaySound((IActiveObject) this._breaker, "SONICORCA/SOUND/BARRIER");
            break;
          case MonitorContents.Invincibility:
            this._breaker.Player.GiveInvincibility();
            break;
        }
      }

      protected override void OnAnimate()
      {
        this._shellAnimation.Animate();
        this._shellOverlayAnimation.Animate();
        if (!this._broken)
        {
          this._screenAnimation.Animate();
          this._screenOverlayAnimation.Animate();
        }
        if (!this._raisingContents)
          return;
        this._contentsAnimation.Animate();
      }

      protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
      {
        IObjectRenderer objectRenderer = renderer.GetObjectRenderer();
        objectRenderer.Render(this._shellAnimation);
        if (viewOptions.Shadows)
          return;
        objectRenderer.FilterAmount *= 0.25;
        if (!this._broken)
        {
          objectRenderer.Render(this._screenAnimation, new Vector2(2.0, -10.0));
          objectRenderer.Render(this._screenOverlayAnimation, new Vector2(0.0, -13.0));
        }
        objectRenderer.Render(this._shellOverlayAnimation);
        if (!this._raisingContents)
          return;
        objectRenderer.Render(this._contentsAnimation, this._contentsPosition);
      }
    }
}
