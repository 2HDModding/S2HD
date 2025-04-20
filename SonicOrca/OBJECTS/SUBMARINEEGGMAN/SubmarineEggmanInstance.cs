// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.SUBMARINEEGGMAN.SubmarineEggmanInstance
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
using SONICORCA.OBJECTS.DUST;
using SonicOrca.Resources;
using System;
using System.Collections.Generic;

namespace SONICORCA.OBJECTS.SUBMARINEEGGMAN;

public class SubmarineEggmanInstance : BossObject
{
  public const int AnimationMobileInside = 0;
  public const int AnimationMobileBody = 1;
  public const int AnimationMobileBodyExploding = 2;
  public const int AnimationMobileBodyDefeated = 3;
  public const int AnimationMobileLights = 4;
  public const int AnimationFlameThrower = 5;
  public const int AnimationFlameThrowerShot = 6;
  public const int AnimationFireball = 7;
  public const int AnimationHighlightsSpawnIntro = 8;
  public const int AnimationHighlightsIntro = 9;
  public const int AnimationHighlightsEnd = 10;
  public const int AnimationHighlightsLava = 11;
  public const int AnimationBrokenPartsLavaA = 12;
  public const int AnimationBrokenPartsLavaB = 13;
  public const int AnimationBrokenPartsLavaC = 14;
  public const int AnimationBrokenPartsLavaD = 15;
  public const int AnimationBrokenPartsLavaE = 16 /*0x10*/;
  public const int AnimationFireballTransition = 17;
  public const int AnimationFireballIntro = 18;
  public const int AnimationFireballEnd = 19;
  private const int AnimationRobotnikNormal = 0;
  private const int AnimationRobotnikSmiling = 1;
  private const int AnimationRobotnikDefeated = 2;
  private const int AnimationDustSmoke = 1;
  private const double AltitudeSpeed = 3.5;
  private const int AltitudeSpeedDefeated = 8;
  private AnimationInstance _animationInside;
  private AnimationInstance _animationRobotnik;
  private AnimationInstance _animationBody;
  private AnimationInstance _animationLights;
  private AnimationInstance _animationHighlightsSpawnIntro;
  private AnimationInstance _animationHighlightsIntro;
  private AnimationInstance _animationHighlightsEnd;
  private AnimationInstance _animationHighlightsLava;
  private Dictionary<int, Vector2i> _brokenPartsPositions = new Dictionary<int, Vector2i>();
  private Dictionary<int, int> _brokenPartsIndexOrder = new Dictionary<int, int>();
  private AnimationInstance _brokenPartsA;
  private AnimationInstance _brokenPartsB;
  private AnimationInstance _brokenPartsC;
  private AnimationInstance _brokenPartsD;
  private AnimationInstance _brokenPartsE;
  private int _leftX = -1024;
  private int _rightX;
  private int _leftPeekY = -416;
  private int _leftFireballY = -288;
  private int _leftSpawnY = 128 /*0x80*/;
  private int _rightPeekY = -528;
  private int _rightFireballY = -224;
  private int _rightSpawnY;
  private Vector2i _startPosition;
  private Vector2 _nonHoverPosition;
  private SubmarineEggmanInstance.State _state;
  private Vector2 _velocity;
  private bool _flipX;
  private bool _isRightPit;
  private int _stateTimer;
  private int _brokenPartsCounter;
  private double _floatOffsetAngle;
  private bool _firedFireball;
  private AnimationGroup _animationGroup;
  private AnimationInstance _currentHighlight;

  protected override void OnStart()
  {
    ResourceTree resourceTree = this.Level.GameContext.ResourceTree;
    AnimationGroup loadedResource1 = resourceTree.GetLoadedResource<AnimationGroup>((ILoadedResource) this.Type, "/ANIGROUP");
    AnimationGroup loadedResource2 = resourceTree.GetLoadedResource<AnimationGroup>((ILoadedResource) this.Type, "SONICORCA/OBJECTS/ROBOTNIK/ANIGROUP");
    this._animationInside = new AnimationInstance(loadedResource1);
    this._animationRobotnik = new AnimationInstance(loadedResource2);
    this._animationBody = new AnimationInstance(loadedResource1, 1);
    this._animationLights = new AnimationInstance(loadedResource1, 4);
    this._animationHighlightsSpawnIntro = new AnimationInstance(loadedResource1, 8);
    this._animationHighlightsIntro = new AnimationInstance(loadedResource1, 9);
    this._animationHighlightsEnd = new AnimationInstance(loadedResource1, 10);
    this._animationHighlightsLava = new AnimationInstance(loadedResource1, 11);
    this._brokenPartsPositions[12] = new Vector2i(-20, -64);
    this._brokenPartsPositions[14] = new Vector2i(-8, -88);
    this._brokenPartsPositions[15] = new Vector2i(-16, -112);
    this._brokenPartsPositions[13] = new Vector2i(-44, -112);
    this._brokenPartsPositions[16 /*0x10*/] = new Vector2i(-80, -112);
    this._brokenPartsIndexOrder[0] = 16 /*0x10*/;
    this._brokenPartsIndexOrder[1] = 13;
    this._brokenPartsIndexOrder[2] = 15;
    this._brokenPartsIndexOrder[3] = 14;
    this._brokenPartsIndexOrder[4] = 12;
    this._animationGroup = loadedResource1;
    this._brokenPartsA = new AnimationInstance(this._animationGroup, 12);
    this._brokenPartsB = new AnimationInstance(this._animationGroup, 13);
    this._brokenPartsC = new AnimationInstance(this._animationGroup, 14);
    this._brokenPartsD = new AnimationInstance(this._animationGroup, 15);
    this._brokenPartsE = new AnimationInstance(this._animationGroup, 16 /*0x10*/);
    this.ExplosionResourceKey = this.Type.GetAbsolutePath("SONICORCA/OBJECTS/EXPLOSION/BOSS");
    this.HitSoundResourceKey = this.Type.GetAbsolutePath("SONICORCA/SOUND/BOSSHIT");
    this.CollisionRectangles = new CollisionRectangle[1]
    {
      new CollisionRectangle((ActiveObject) this, 0, -100, -100, 200, 200)
    };
    this._startPosition = this.Position;
    this._leftX += this._startPosition.X;
    this._rightX += this._startPosition.X;
    this._leftPeekY += this._startPosition.Y;
    this._leftFireballY += this._startPosition.Y;
    this._leftSpawnY += this._startPosition.Y;
    this._rightPeekY += this._startPosition.Y;
    this._rightFireballY += this._startPosition.Y;
    this._rightSpawnY += this._startPosition.Y;
    this.LockLifetime = true;
    this._velocity = new Vector2(0.0, -3.5);
    this._isRightPit = true;
    this._state = SubmarineEggmanInstance.State.Raise;
    this.Health = 8;
  }

  private void ResetHighlights()
  {
    this._animationHighlightsSpawnIntro.ResetFrame();
    this._animationHighlightsSpawnIntro.Cycles = 0;
    this._animationHighlightsIntro.ResetFrame();
    this._animationHighlightsIntro.Cycles = 0;
    this._animationHighlightsEnd.ResetFrame();
    this._animationHighlightsEnd.Cycles = 0;
    this._currentHighlight = this._animationHighlightsSpawnIntro;
  }

  private void CleanHighlights()
  {
    this.ResetHighlights();
    this._currentHighlight = (AnimationInstance) null;
  }

  protected override void OnUpdate()
  {
    base.OnUpdate();
    if (this.Health == 1 && this.Level.Random.Next(15) == 0)
      this.EmitSmoke((Vector2) new Vector2i(this.Level.Random.Next((int) sbyte.MinValue, 128 /*0x80*/), this.Level.Random.Next(-64, -32)), new Vector2(0.0, -4.0));
    switch (this._state)
    {
      case SubmarineEggmanInstance.State.Raise:
        this.PositionPrecise = this.PositionPrecise + this._velocity;
        if (this.Position.Y > (this._isRightPit ? this._rightPeekY : this._leftPeekY))
          break;
        this._velocity.Y = 0.0;
        this._state = SubmarineEggmanInstance.State.PrepareFlameThrower;
        this._stateTimer = 60;
        this._nonHoverPosition = this.PositionPrecise;
        this._floatOffsetAngle = 0.0;
        break;
      case SubmarineEggmanInstance.State.PrepareFlameThrower:
        --this._stateTimer;
        if (this._stateTimer > 0)
        {
          this.Hover();
          break;
        }
        if (this._stateTimer == 0)
        {
          this.CreateFlameThrowerFlame();
          this.ResetHighlights();
          break;
        }
        if (this._stateTimer > -120)
          break;
        this.CreateFlameThrowerShot();
        this._currentHighlight = this._animationHighlightsIntro;
        this._stateTimer = 47;
        this._state = SubmarineEggmanInstance.State.Hover;
        break;
      case SubmarineEggmanInstance.State.Hover:
        --this._stateTimer;
        if (this._stateTimer > 0)
        {
          this.Hover();
          break;
        }
        this._currentHighlight = this._animationHighlightsEnd;
        this._velocity = new Vector2(0.0, 3.5);
        this._state = SubmarineEggmanInstance.State.Lower;
        break;
      case SubmarineEggmanInstance.State.Lower:
        this.PositionPrecise = this.PositionPrecise + this._velocity;
        if (this.Position.Y > (this._isRightPit ? this._rightFireballY : this._leftFireballY) && !this._firedFireball)
        {
          this.CreateFireball(-1);
          this.CreateFireball(1);
          this.Level.SoundManager.PlaySound(this.Position, "SONICORCA/SOUND/FIREBALL");
          this._firedFireball = true;
        }
        if (this._isRightPit)
        {
          if (this.Position.Y < this._rightSpawnY)
            break;
          this.Position = new Vector2i(this.Position.X, this._rightSpawnY);
        }
        else
        {
          if (this.Position.Y < this._leftSpawnY)
            break;
          this.Position = new Vector2i(this.Position.X, this._leftSpawnY);
        }
        this._velocity = new Vector2(0.0, -3.5);
        this._state = SubmarineEggmanInstance.State.Raise;
        this._firedFireball = false;
        ICharacter protagonist = this.Level.Player.Protagonist;
        if (protagonist.Position.X < this._startPosition.X - 512 /*0x0200*/)
        {
          this.Position = new Vector2i(this._leftX, this._leftSpawnY);
          this._isRightPit = false;
        }
        else
        {
          this.Position = new Vector2i(this._rightX, this._rightSpawnY);
          this._isRightPit = true;
        }
        this._flipX = protagonist.Position.X >= this.Position.X;
        break;
      case SubmarineEggmanInstance.State.Defeated:
        this.CleanHighlights();
        --this._stateTimer;
        if (this._stateTimer > 0)
        {
          if (this._stateTimer % 16 /*0x10*/ == 0 && this._brokenPartsCounter < 5)
          {
            int num = this._brokenPartsIndexOrder[this._brokenPartsCounter];
            this.ReleaseFragmentMetal(this.Position + this._brokenPartsPositions[num], 6.0, num);
            switch (num)
            {
              case 12:
                this._brokenPartsA = (AnimationInstance) null;
                break;
              case 13:
                this._brokenPartsB = (AnimationInstance) null;
                break;
              case 14:
                this._brokenPartsC = (AnimationInstance) null;
                break;
              case 15:
                this._brokenPartsD = (AnimationInstance) null;
                break;
              case 16 /*0x10*/:
                this._brokenPartsE = (AnimationInstance) null;
                break;
            }
            ++this._brokenPartsCounter;
          }
          this.UpdateExplosions(128 /*0x80*/);
          if (this._stateTimer >= 30)
            break;
          this.UpdateSmoke();
          break;
        }
        this._animationBody.Index = 3;
        this._animationRobotnik.Index = 0;
        this.UpdateSmoke();
        if (this._stateTimer > -60)
          break;
        this.Defeated = true;
        this.Fleeing = true;
        this.LockLifetime = false;
        this.Position = this.Position + new Vector2i(0, 8);
        break;
    }
  }

  private void EmitSmoke(Vector2 offset, Vector2 velocity)
  {
    Matrix4 matrix4 = Matrix4.Identity * Matrix4.CreateTranslation((Vector2) this.Position);
    if (this._flipX)
      matrix4 *= Matrix4.CreateScale(-1.0, 1.0);
    Vector2i position = (Vector2i) ((matrix4 * Matrix4.CreateTranslation(offset)).RotateZ(0.0) * new Vector2(0.0, 100.0));
    DustInstance dustInstance = this.Level.ObjectManager.AddObject(new ObjectPlacement("SONICORCA/OBJECTS/DUST", this.Level.Map.Layers.IndexOf(this.Layer), position)) as DustInstance;
    dustInstance.SetDustAnimationIndex(1);
    dustInstance.Velocity = velocity;
  }

  private void Hover()
  {
    this.PositionPrecise = this._nonHoverPosition + new Vector2(0.0, Math.Sin(this._floatOffsetAngle) / Math.PI * 16.0);
    this._floatOffsetAngle += Math.PI / 64.0;
  }

  private void CreateFlameThrowerFlame()
  {
    SubmarineEggmanInstance.FlameThrower flameThrower = this.Level.ObjectManager.AddSubObject<SubmarineEggmanInstance.FlameThrower>((ActiveObject) this);
    flameThrower.FlipX = this._flipX;
    flameThrower.Position = this.Position + new Vector2i(this._flipX ? 132 : -132, -121);
  }

  private void CreateFlameThrowerShot()
  {
    SubmarineEggmanInstance.FlameThrowerShot flameThrowerShot = this.Level.ObjectManager.AddSubObject<SubmarineEggmanInstance.FlameThrowerShot>((ActiveObject) this);
    flameThrowerShot.FlipX = this._flipX;
    flameThrowerShot.Position = this.Position + new Vector2i(this._flipX ? 448 : -448, -112);
    flameThrowerShot.Velocity = new Vector2i(this._flipX ? 16 /*0x10*/ : -16, 0);
  }

  private void CreateFireball(int direction)
  {
    SubmarineEggmanInstance.Fireball fireball = this.Level.ObjectManager.AddSubObject<SubmarineEggmanInstance.Fireball>((ActiveObject) this);
    fireball.IsMainFireball = true;
    fireball.SpawnDirection = (SubmarineEggmanInstance.Fireball.FireSpawn) direction;
    fireball.Position = this.Position;
    if (this._isRightPit)
    {
      if (direction == 1)
      {
        fireball.Velocity = new Vector2(7.0, -25.0);
      }
      else
      {
        if (direction != -1)
          return;
        fireball.Velocity = new Vector2(-6.85, -25.0);
      }
    }
    else if (direction == 1)
    {
      fireball.Velocity = new Vector2(6.7, -21.0);
    }
    else
    {
      if (direction != -1)
        return;
      fireball.Velocity = new Vector2(-7.0, -21.0);
    }
  }

  protected override void Hit(ICharacter character)
  {
    base.Hit(character);
    if (character.Velocity.Y < -15.125)
      character.Velocity = new Vector2(character.Velocity.X, -15.125);
    if (this.Health <= 0)
      return;
    this._animationRobotnik.Index = 1;
  }

  protected override void Defeat()
  {
    base.Defeat();
    this._state = SubmarineEggmanInstance.State.Defeated;
    this._stateTimer = 179;
    this._animationBody.Index = 2;
    this._animationRobotnik.Index = 2;
    this.CollisionRectangles = new CollisionRectangle[0];
    this.Level.Player.GainScore(1000);
    this.Level.CreateScoreObject(1000, this.Position + new Vector2i(0, (int) sbyte.MinValue));
  }

  private void UpdateSmoke()
  {
    if (this.Level.Random.Next(15) != 0)
      return;
    this.EmitSmoke((Vector2) new Vector2i(this.Level.Random.Next((int) sbyte.MinValue, 128 /*0x80*/), this.Level.Random.Next(-64, -32)), new Vector2(0.0, -4.0));
  }

  private void ReleaseFragmentMetal(Vector2i position, double speed, int index, bool explode = true)
  {
    double num1 = this.Level.Random.NextRadians();
    Vector2 velocity = new Vector2(speed * Math.Cos(num1), speed * Math.Sin(num1));
    this.ReleaseFragmentMetal(position, velocity, index);
    int num2 = explode ? 1 : 0;
  }

  private void ReleaseFragmentMetal(Vector2i position, Vector2 velocity, int index)
  {
    Fragment fragment = this.Level.ObjectManager.AddSubObject<Fragment>((ActiveObject) this);
    fragment.AnimationGroup = this._animationGroup;
    fragment.AnimationIndex = index;
    fragment.Position = position;
    fragment.Velocity = velocity;
    fragment.AngularVelocity = 2.0 * Math.PI / 15.0;
    fragment.FlipX = this.Level.Random.NextBoolean();
    fragment.Initialise();
  }

  protected override void OnAnimate()
  {
    this._animationInside.Animate();
    this._animationRobotnik.Animate();
    this._animationBody.Animate();
    this._animationLights.Animate();
    if (this._currentHighlight == null)
      return;
    if (this._currentHighlight == this._animationHighlightsEnd)
    {
      this._currentHighlight.Animate();
      if (this._currentHighlight.Cycles <= 0)
        return;
      this._currentHighlight = (AnimationInstance) null;
      this.CleanHighlights();
    }
    else
      this._currentHighlight.Animate();
  }

  protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
  {
    IObjectRenderer objectRenderer = renderer.GetObjectRenderer();
    objectRenderer.FilterAmount *= 0.5;
    objectRenderer.Render(this._animationInside, this._flipX);
    objectRenderer.Render(this._animationRobotnik, (Vector2) new Vector2i(this._flipX ? 32 /*0x20*/ : -32, -42), this._flipX);
    if (this.IsInvincibleFlashing)
      objectRenderer.AdditiveColour = BossObject.FlashAdditiveColour;
    objectRenderer.Render(this._animationBody, this._flipX);
    objectRenderer.EmitsLight = true;
    objectRenderer.Render(this._animationLights, this._flipX);
    if (this._currentHighlight != null)
      objectRenderer.Render(this._currentHighlight, (Vector2) new Vector2i(0, 0), this._flipX);
    objectRenderer.Render(this._animationHighlightsLava, (Vector2) new Vector2i(0, 0), this._flipX);
    if (this._state != SubmarineEggmanInstance.State.Defeated)
      return;
    if (this._brokenPartsA != null)
    {
      Vector2i brokenPartsPosition = this._brokenPartsPositions[12];
      objectRenderer.Render(this._brokenPartsA, (Vector2) brokenPartsPosition, this._flipX);
    }
    if (this._brokenPartsC != null)
    {
      Vector2i brokenPartsPosition = this._brokenPartsPositions[14];
      objectRenderer.Render(this._brokenPartsC, (Vector2) brokenPartsPosition, this._flipX);
    }
    if (this._brokenPartsD != null)
    {
      Vector2i brokenPartsPosition = this._brokenPartsPositions[15];
      objectRenderer.Render(this._brokenPartsD, (Vector2) brokenPartsPosition, this._flipX);
    }
    if (this._brokenPartsB != null)
    {
      Vector2i brokenPartsPosition = this._brokenPartsPositions[13];
      objectRenderer.Render(this._brokenPartsB, (Vector2) brokenPartsPosition, this._flipX);
    }
    if (this._brokenPartsE == null)
      return;
    Vector2i brokenPartsPosition1 = this._brokenPartsPositions[16 /*0x10*/];
    objectRenderer.Render(this._brokenPartsE, (Vector2) brokenPartsPosition1, this._flipX);
  }

  private enum State
  {
    Raise,
    PrepareFlameThrower,
    Hover,
    Lower,
    Defeated,
  }

  private class FlameThrower : ParticleObject
  {
    public FlameThrower()
      : base("/ANIGROUP", 5)
    {
      this.FilterMultiplier = 0.0;
    }

    protected override void OnCollision(CollisionEvent e)
    {
      if (e.ActiveObject.Type.Classification != ObjectClassification.Character)
        return;
      ((ICharacter) e.ActiveObject).Hurt(-1);
    }

    protected override void OnAnimate() => base.OnAnimate();

    protected override void OnUpdate()
    {
      base.OnUpdate();
      if (this._animationInstance.CurrentFrameIndex == 35)
      {
        if (this.CollisionRectangles != null && this.CollisionRectangles.Length != 0)
          return;
        int x = (int) ((this.FlipX ? (double) -this._animationInstance.CurrentFrame.Offset.X : (double) this._animationInstance.CurrentFrame.Offset.X) - (double) this._animationInstance.CurrentFrame.Source.Width / 2.0);
        Animation.Frame currentFrame = this._animationInstance.CurrentFrame;
        double y1 = (double) currentFrame.Offset.Y;
        currentFrame = this._animationInstance.CurrentFrame;
        Rectanglei source = currentFrame.Source;
        double num = (double) source.Height / 4.0;
        int y2 = (int) (y1 - num);
        currentFrame = this._animationInstance.CurrentFrame;
        source = currentFrame.Source;
        int width = source.Width;
        currentFrame = this._animationInstance.CurrentFrame;
        source = currentFrame.Source;
        int height = source.Height / 2;
        this.CollisionRectangles = new CollisionRectangle[1]
        {
          new CollisionRectangle((ActiveObject) this, 0, x, y2, width, height)
        };
      }
      else
      {
        if (this._animationInstance.CurrentFrameIndex != 50 || this.CollisionRectangles == null && this.CollisionRectangles.Length == 0)
          return;
        this.CollisionRectangles = new CollisionRectangle[0];
      }
    }

    protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
    {
      if (this._animationInstance.CurrentFrameIndex >= 35)
      {
        if (!this.CanDraw())
          return;
        Vector2 destination = new Vector2(12.0, 9.0);
        IObjectRenderer objectRenderer = renderer.GetObjectRenderer();
        using (objectRenderer.BeginMatixState())
        {
          if (this.FlipX)
            objectRenderer.ModelMatrix *= Matrix4.CreateScale(-1.0, 1.0);
          if (this.FlipY)
            objectRenderer.ModelMatrix *= Matrix4.CreateScale(1.0, -1.0);
          if (this.FilterMultiplier == 0.0)
            objectRenderer.Filter = 0;
          else
            objectRenderer.FilterAmount *= this.FilterMultiplier;
          objectRenderer.BlendMode = this.AdditiveBlending ? BlendMode.Additive : BlendMode.Alpha;
          objectRenderer.Render(this._animationInstance, destination);
        }
      }
      else
        base.OnDraw(renderer, viewOptions);
    }
  }

  private class FlameThrowerShot : Enemy
  {
    private AnimationInstance _animation;

    public bool FlipX { get; set; }

    public Vector2i Velocity { get; set; }

    protected override void OnStart()
    {
      this._animation = new AnimationInstance(this.ResourceTree, this.Type.GetAbsolutePath("/ANIGROUP"), 6);
      this.CollisionRectangles = new CollisionRectangle[1]
      {
        new CollisionRectangle((ActiveObject) this, 0, -16, -16, 32 /*0x20*/, 32 /*0x20*/)
      };
    }

    protected override void OnUpdate() => this.Position = this.Position + this.Velocity;

    protected override void OnAnimate() => this._animation.Animate();

    protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
    {
      IObjectRenderer objectRenderer = renderer.GetObjectRenderer();
      objectRenderer.EmitsLight = true;
      objectRenderer.Render(this._animation, this.FlipX);
    }
  }

  private class Fireball : Enemy
  {
    private const double Gravity = 0.875;
    private AnimationInstance _currentAnimation;
    private AnimationInstance _fireball;
    private AnimationInstance _fireballTransition;
    private int _ticks = 10;
    private int _fireInstances;
    private SubmarineEggmanInstance.Fireball.FireSpawn _spawnDirection = SubmarineEggmanInstance.Fireball.FireSpawn.Left;

    public Vector2 Velocity { get; set; }

    public bool IsMainFireball { get; set; }

    public SubmarineEggmanInstance.Fireball.FireSpawn SpawnDirection
    {
      get => this._spawnDirection;
      set => this._spawnDirection = value;
    }

    protected override void OnStart()
    {
      this._fireball = new AnimationInstance(this.ResourceTree, this.Type.GetAbsolutePath("/ANIGROUP"), 7);
      this._fireballTransition = new AnimationInstance(this.ResourceTree, this.Type.GetAbsolutePath("/ANIGROUP"), 17);
      this._currentAnimation = this._fireball;
      this.CollisionRectangles = new CollisionRectangle[1]
      {
        new CollisionRectangle((ActiveObject) this, 0, -32, -32, 64 /*0x40*/, 64 /*0x40*/)
      };
    }

    protected override void OnUpdate()
    {
      this.PositionPrecise = this.PositionPrecise + this.Velocity;
      this.Velocity += new Vector2(0.0, 0.875);
      if (!this.IsTouchingGround())
        return;
      this.Velocity = new Vector2(0.0, 0.0);
      if (this._currentAnimation != this._fireballTransition)
        this._currentAnimation = this._fireballTransition;
      if (!this.IsMainFireball)
        return;
      ++this._ticks;
      if (this._ticks == 15)
        this.CreateFire();
      else if (this._ticks == 20)
        this.CreateFire();
      else if (this._ticks == 25)
      {
        this.CreateFire();
      }
      else
      {
        if (this._ticks != 29)
          return;
        this.CreateFire();
      }
    }

    private void CreateFire()
    {
      SubmarineEggmanInstance.Fire fire = this.Level.ObjectManager.AddSubObject<SubmarineEggmanInstance.Fire>(this.ParentObject);
      fire.Position = this.Position + new Vector2i(this._fireInstances * fire.DesignBounds.Width * (int) this.SpawnDirection, 0);
      fire.BubbleLeftCount = 4;
      fire.BubbleRightCount = 4;
      fire.Priority = 1024 /*0x0400*/;
      fire.Layer = this.Level.Map.Layers[this.Level.Map.Layers.Count - 1];
      this.Level.SoundManager.PlaySound(this.Position, this.Type.GetAbsolutePath("SONICORCA/SOUND/FIREBURN"));
      ++this._fireInstances;
    }

    private bool IsTouchingGround()
    {
      bool flag = false;
      foreach (CollisionInfo collision in CollisionVector.GetCollisions((ActiveObject) this, 32 /*0x20*/))
      {
        if (collision.Vector.Owner == null && !collision.Vector.IsWall)
        {
          this.PositionPrecise = new Vector2(this.PositionPrecise.X, this.PositionPrecise.Y + collision.Shift);
          flag = true;
        }
      }
      return flag;
    }

    protected override void OnAnimate()
    {
      if (this._currentAnimation != null)
        this._currentAnimation.Animate();
      if (this._currentAnimation != this._fireballTransition || this._currentAnimation.Cycles <= 0)
        return;
      this._currentAnimation = (AnimationInstance) null;
      this.Finish();
    }

    protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
    {
      IObjectRenderer objectRenderer = renderer.GetObjectRenderer();
      objectRenderer.EmitsLight = true;
      if (this._currentAnimation == null)
        return;
      if (this._currentAnimation == this._fireballTransition)
      {
        bool flipX = this.SpawnDirection == SubmarineEggmanInstance.Fireball.FireSpawn.Right;
        objectRenderer.Render(this._currentAnimation, new Vector2(flipX ? 80.0 : -80.0, -8.0), flipX);
      }
      else
      {
        double angle = this.Velocity.Angle + 6.0 * Math.PI / 5.0;
        using (objectRenderer.BeginMatixState())
        {
          objectRenderer.ModelMatrix *= Matrix4.CreateRotationZ(angle);
          objectRenderer.Render(this._currentAnimation);
        }
      }
    }

    public enum FireSpawn
    {
      Left = -1, // 0xFFFFFFFF
      Right = 1,
    }
  }

  private class Fire : Enemy
  {
    private AnimationInstance _currentAnimation;
    private AnimationInstance _fireballIntro;
    private AnimationInstance _fireballEnd;
    private int _timer;

    public int BubbleLeftCount { get; set; }

    public int BubbleRightCount { get; set; }

    protected override void OnStart()
    {
      this._fireballIntro = new AnimationInstance(this.ResourceTree, this.Type.GetAbsolutePath("/ANIGROUP"), 18);
      this._fireballEnd = new AnimationInstance(this.ResourceTree, this.Type.GetAbsolutePath("/ANIGROUP"), 19);
      this._currentAnimation = this._fireballIntro;
      this.CollisionRectangles = new CollisionRectangle[1]
      {
        new CollisionRectangle((ActiveObject) this, 0, -32, -32, 64 /*0x40*/, 64 /*0x40*/)
      };
      this._timer = 74;
    }

    protected override void OnUpdate()
    {
      --this._timer;
      if (this._timer == 66)
      {
        if (this.BubbleLeftCount > 0)
          this.BubbleFire(this.Position + new Vector2i(-64, 0), this.BubbleLeftCount--, 0);
        if (this.BubbleRightCount <= 0)
          return;
        this.BubbleFire(this.Position + new Vector2i(64 /*0x40*/, 0), 0, this.BubbleRightCount--);
      }
      else
      {
        if (this._timer != 0)
          return;
        this._currentAnimation = this._fireballEnd;
      }
    }

    private void BubbleFire(Vector2i bubblePosition, int leftCount, int rightCount)
    {
      if (!this.IsGroundBelow(ref bubblePosition))
        return;
      SubmarineEggmanInstance.Fire fire = this.Level.ObjectManager.AddSubObject<SubmarineEggmanInstance.Fire>(this.ParentObject);
      fire.Position = bubblePosition;
      fire.BubbleLeftCount = leftCount;
      fire.BubbleRightCount = rightCount;
    }

    private bool IsGroundBelow(ref Vector2i postion)
    {
      bool flag = false;
      foreach (CollisionInfo collision in CollisionVector.GetCollisions((ActiveObject) this, 32 /*0x20*/))
      {
        if (collision.Vector.Owner == null && !collision.Vector.IsWall)
        {
          postion.Y += (int) collision.Shift;
          flag = true;
        }
      }
      return flag;
    }

    protected override void OnAnimate()
    {
      if (this._currentAnimation != null)
        this._currentAnimation.Animate();
      if (this._currentAnimation != this._fireballEnd || this._currentAnimation.Cycles <= 0)
        return;
      this._currentAnimation = (AnimationInstance) null;
      this.Finish();
    }

    protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
    {
      IObjectRenderer objectRenderer = renderer.GetObjectRenderer();
      objectRenderer.EmitsLight = true;
      if (this._currentAnimation == null)
        return;
      objectRenderer.Render(this._currentAnimation);
    }
  }
}
