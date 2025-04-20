// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.DRILLEGGMAN.DrillEggmanInstance
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Audio;
using SonicOrca.Core;
using SonicOrca.Core.Collision;
using SonicOrca.Core.Objects;
using SonicOrca.Core.Objects.Base;
using SonicOrca.Core.Objects.Metadata;
using SonicOrca.Extensions;
using SonicOrca.Geometry;
using SonicOrca.Graphics;
using SONICORCA.OBJECTS.DUST;
using SonicOrca.Resources;
using System;

#nullable disable
namespace SONICORCA.OBJECTS.DRILLEGGMAN;

public class DrillEggmanInstance : BossObject
{
  private const int AnimationCar = 0;
  private const int AnimationWheelFrontMoving = 1;
  private const int AnimationWheelFrontStill = 2;
  private const int AnimationWheelBackMoving = 3;
  private const int AnimationWheelBackStill = 4;
  private const int AnimationDrill = 5;
  private const int AnimationPropellerStatic = 6;
  private const int AnimationPropellerOut = 7;
  private const int AnimationPropellerSpinning = 8;
  private const int AnimationPropellerIn = 9;
  private const int AnimationCarLight = 10;
  private const int AnimationBrokenCar = 11;
  private const int AnimationBrokenVent = 12;
  private const int AnimationGlassFragment = 13;
  private const int AnimationMetalFragment = 14;
  private const int AnimationRedFragment = 15;
  private const int AnimationPipeFragment = 16 /*0x10*/;
  private const int AnimationSpark = 17;
  private const int AnimationEggMobileChasis = 18;
  private const int AnimationEggMobileChasisFlame = 19;
  private const int AnimationEggMobileScreen = 20;
  private const int AnimationEggMobileCap = 21;
  private const int AnimationEggMobileChasisTurn1 = 22;
  private const int AnimationEggMobileChasisTurn2 = 23;
  private const int AnimationEggMobileScreenTurn1 = 24;
  private const int AnimationEggMobileScreenTurn2 = 25;
  private const int AnimationBrokenEggMobileChasis = 26;
  private const int AnimationBrokenEggMobileChasisFlame = 27;
  private const int AnimationBrokenEggMobileScreen = 28;
  private const int AnimationBrokenEggMobileCap = 29;
  private const int AnimationBrokenEggMobileChasisTurn1 = 30;
  private const int AnimationBrokenEggMobileChasisTurn2 = 31 /*0x1F*/;
  private const int AnimationBrokenEggMobileScreenTurn1 = 32 /*0x20*/;
  private const int AnimationBrokenEggMobileScreenTurn2 = 33;
  private const int AnimationEggMobileCapTurn1 = 34;
  private const int AnimationEggMobileCapTurn2 = 35;
  private const int AnimationRobotnikNormal = 0;
  private const int AnimationRobotnikSmiling = 1;
  private const int AnimationRobotnikFrown = 2;
  private const int AnimationRobotnikDefeated = 3;
  private const int AnimationRobotnikTurn1 = 4;
  private const int AnimationRobotnikTurn2 = 5;
  private const int AnimationDustSmoke = 1;
  private const double Gravity = 0.875;
  private AnimationGroup _animationGroup;
  private AnimationInstance _animation;
  private AnimationInstance _carLightAnimation;
  private AnimationInstance _sparkAnimation;
  private AnimationInstance _brokenVentAnimation;
  private DrillEggmanInstance.StatusType _status;
  private Vector2i _startPosition;
  private int _stateTimer;
  private int _velocityX;
  private double _velocityY;
  private DrillEggmanInstance.EggMobile _eggMobile;
  private DrillEggmanInstance.Drill _drill;
  private DrillEggmanInstance.Wheel[] _wheels = new DrillEggmanInstance.Wheel[4];
  private float _lastHealthFlash;

  [HideInEditor]
  public DrillEggmanInstance.StatusType Status => this._status;

  [StateVariable]
  public int LeftEdge { get; set; }

  [StateVariable]
  public int RightEdge { get; set; }

  protected override void OnStart()
  {
    ResourceTree resourceTree = this.Level.GameContext.ResourceTree;
    AnimationGroup animationGroup = this._animationGroup = resourceTree.GetLoadedResource<AnimationGroup>((ILoadedResource) this.Type, "/ANIGROUP");
    resourceTree.GetLoadedResource<AnimationGroup>((ILoadedResource) this.Type, "SONICORCA/OBJECTS/ROBOTNIK/ANIGROUP");
    this._animation = new AnimationInstance(animationGroup);
    this._carLightAnimation = new AnimationInstance(animationGroup, 10);
    this._sparkAnimation = new AnimationInstance(animationGroup, 17);
    this._brokenVentAnimation = new AnimationInstance(animationGroup, 12);
    this.ExplosionResourceKey = this.Type.GetAbsolutePath("SONICORCA/OBJECTS/EXPLOSION/BOSS");
    this.HitSoundResourceKey = this.Type.GetAbsolutePath("SONICORCA/SOUND/BOSSHIT");
    this.LockLifetime = true;
    this._startPosition = this.Position;
    this.Position = this.Position + new Vector2i(0, 1176);
    this._eggMobile = this.Level.ObjectManager.AddSubObject<DrillEggmanInstance.EggMobile>((ActiveObject) this);
    this._eggMobile.Position = this._startPosition;
    for (int index = 0; index < 4; ++index)
    {
      this._wheels[index] = this.Level.ObjectManager.AddSubObject<DrillEggmanInstance.Wheel>((ActiveObject) this);
      this._wheels[index].Index = index;
      this._wheels[index].LockLifetime = true;
      this._wheels[index].AutoPositionToCar();
    }
    this._drill = this.Level.ObjectManager.AddSubObject<DrillEggmanInstance.Drill>((ActiveObject) this);
    this._drill.Position = this.Position + new Vector2i(-224, 52);
    this._drill.FlipX = false;
    this._drill.LockLifetime = true;
    this._wheels[2].Priority = 252;
    this._wheels[3].Priority = 252;
    this._eggMobile.Priority = 254;
    this.Priority = 256 /*0x0100*/;
    this._drill.Priority = 258;
    this._wheels[0].Priority = 260;
    this._wheels[1].Priority = 260;
    this.Health = 8;
  }

  protected override void OnUpdate()
  {
    base.OnUpdate();
    switch (this._status)
    {
      case DrillEggmanInstance.StatusType.InitialApproch:
        if (this._eggMobile.Position.X <= this._startPosition.X - 1152)
        {
          this._status = DrillEggmanInstance.StatusType.FinalApproach;
          break;
        }
        this.Position = this.Position + new Vector2i(-4, 0);
        DrillEggmanInstance.Drill drill1 = this._drill;
        drill1.Position = drill1.Position + new Vector2i(-4, 0);
        break;
      case DrillEggmanInstance.StatusType.FinalApproach:
        Vector2i position1 = this._eggMobile.Position;
        if (position1.Y < this._startPosition.Y + 1176)
          break;
        DrillEggmanInstance.EggMobile eggMobile1 = this._eggMobile;
        position1 = this._eggMobile.Position;
        Vector2i vector2i1 = new Vector2i(position1.X, this._startPosition.Y + 1176);
        eggMobile1.Position = vector2i1;
        this._eggMobile.BeginLoweringPropeller();
        this._status = DrillEggmanInstance.StatusType.LowerPropeller;
        this._stateTimer = 60;
        break;
      case DrillEggmanInstance.StatusType.LowerPropeller:
        if (--this._stateTimer > 0)
          break;
        this._status = DrillEggmanInstance.StatusType.Normal;
        this._velocityX = -8;
        this.CollisionRectangles = new CollisionRectangle[1]
        {
          new CollisionRectangle((ActiveObject) this, 0, (int) sbyte.MinValue, (int) sbyte.MinValue, 256 /*0x0100*/, 256 /*0x0100*/)
        };
        break;
      case DrillEggmanInstance.StatusType.Normal:
        Vector2i position2 = this._eggMobile.Position;
        if (position2.X > this.LeftEdge - 512 /*0x0200*/)
        {
          position2 = this._eggMobile.Position;
          if (position2.X < this.RightEdge + 512 /*0x0200*/)
            goto label_15;
        }
        this._velocityX *= -1;
        this._eggMobile.FlipX = this._velocityX > 0;
        if (!this._drill.Released)
        {
          DrillEggmanInstance.Drill drill2 = this._drill;
          position2 = this.Position;
          int x1 = position2.X;
          position2 = this.Position;
          int x2 = position2.X;
          position2 = this._drill.Position;
          int x3 = position2.X;
          int num = x2 - x3;
          int x4 = x1 + num;
          position2 = this._drill.Position;
          int y = position2.Y;
          Vector2i vector2i2 = new Vector2i(x4, y);
          drill2.Position = vector2i2;
          this._drill.FlipX = this._velocityX > 0;
        }
        if (this.Health == 1)
          this.ReleaseDrill();
label_15:
        DrillEggmanInstance.EggMobile eggMobile2 = this._eggMobile;
        eggMobile2.Position = eggMobile2.Position + new Vector2i(this._velocityX, 0);
        this.Position = this.Position + new Vector2i(this._velocityX, 0);
        if (!this._drill.Released)
        {
          DrillEggmanInstance.Drill drill3 = this._drill;
          drill3.Position = drill3.Position + new Vector2i(this._velocityX, 0);
        }
        if (this.Health != 1)
          break;
        if ((double) this._lastHealthFlash > 0.0)
          this._lastHealthFlash = Math.Max(0.0f, this._lastHealthFlash - 0.1f);
        if (this._sparkAnimation.Cycles > 0 && this.Level.Random.Next(60) == 0)
        {
          this._sparkAnimation.Cycles = 0;
          this._sparkAnimation.ResetFrame();
          this._lastHealthFlash = 1f;
        }
        if (this.Level.Random.Next(40) != 0)
          break;
        if (this._velocityX >= 0)
        {
          this.EmitSmoke((Vector2) new Vector2i(this.Level.Random.Next((int) sbyte.MinValue, 28), -32), new Vector2(-1.0, -4.0));
          break;
        }
        this.EmitSmoke((Vector2) new Vector2i(this.Level.Random.Next((int) sbyte.MinValue, 28), -32), new Vector2(1.0, -4.0));
        break;
      case DrillEggmanInstance.StatusType.Defeated:
        --this._stateTimer;
        if (this._stateTimer > 0)
        {
          this._eggMobile.DoEggMobileBreak();
          this.UpdateExplosions(128 /*0x80*/);
          this.UpdateCarToGroundLevel();
          break;
        }
        this._status = DrillEggmanInstance.StatusType.PostDefeatedIdleA;
        this._stateTimer = 12;
        this._eggMobile.DoRobotnikNormal();
        break;
      case DrillEggmanInstance.StatusType.PostDefeatedIdleA:
        --this._stateTimer;
        if (this._stateTimer > 0)
          break;
        this._status = DrillEggmanInstance.StatusType.PostDefeatStartPropeller;
        break;
      case DrillEggmanInstance.StatusType.PostDefeatStartPropeller:
        this.Defeated = true;
        this._stateTimer = 50;
        this._status = DrillEggmanInstance.StatusType.PostDefeatB;
        break;
      case DrillEggmanInstance.StatusType.PostDefeatB:
        if (--this._stateTimer > 0)
          break;
        this._status = DrillEggmanInstance.StatusType.Finished;
        this._stateTimer = 146;
        this.Fleeing = true;
        break;
      case DrillEggmanInstance.StatusType.Finished:
        --this._stateTimer;
        break;
    }
  }

  private void UpdateCarToGroundLevel()
  {
    Vector2i position = this._eggMobile.Position;
    int y1 = position.Y;
    position = this.Position;
    int y2 = position.Y;
    double num = (double) (y1 - y2);
    this.PositionPrecise = this.PositionPrecise + new Vector2(0.0, this._velocityY);
    this._velocityY += 0.875;
    if (this._velocityY > 0.0)
    {
      foreach (CollisionInfo collision in CollisionVector.GetCollisions((ActiveObject) this, 80 /*0x50*/))
      {
        if (collision.Vector.Owner == null && !collision.Vector.IsWall)
        {
          Vector2 positionPrecise = this.PositionPrecise;
          double x = positionPrecise.X;
          positionPrecise = this.PositionPrecise;
          double y3 = positionPrecise.Y + collision.Shift;
          this.PositionPrecise = new Vector2(x, y3);
          this._velocityY = 0.0;
        }
      }
    }
    this._eggMobile.PositionPrecise = new Vector2((double) this._eggMobile.Position.X, (double) this.Position.Y + num);
  }

  protected override void Hit(ICharacter character)
  {
    base.Hit(character);
    if (this.Health <= 0)
      return;
    this._eggMobile.DoRobotnikFrown();
  }

  protected override void Defeat()
  {
    this._status = DrillEggmanInstance.StatusType.Defeated;
    this._stateTimer = 179;
    this._eggMobile.DoRobotnikDefeated();
    this.CollisionRectangles = new CollisionRectangle[0];
    this._velocityY = -6.0;
    this._animation.Index = 11;
    this.ReleaseFragments();
    this.ReleaseWheels();
    this.ReleaseDrill();
    this.Level.Player.GainScore(1000);
    this.Level.CreateScoreObject(1000, this.Position + new Vector2i(0, (int) sbyte.MinValue));
  }

  private void EmitSmoke(Vector2 offset, Vector2 velocity)
  {
    Matrix4 matrix4 = Matrix4.Identity * Matrix4.CreateTranslation((Vector2) this.Position);
    if (this._eggMobile.FlipX)
      matrix4 *= Matrix4.CreateScale(-1.0, 1.0);
    Vector2i position = (Vector2i) ((matrix4 * Matrix4.CreateTranslation(offset)).RotateZ(0.0) * new Vector2(0.0, 0.0));
    DustInstance dustInstance = this.Level.ObjectManager.AddObject(new ObjectPlacement("SONICORCA/OBJECTS/DUST", this.Level.Map.Layers.IndexOf(this.Layer), position)) as DustInstance;
    dustInstance.Velocity = velocity;
    dustInstance.SetDustAnimationIndex(1);
    if (this.Level.Random.NextBoolean())
      dustInstance.Priority = this.Priority - 8;
    else
      dustInstance.Priority = this.Priority + 8;
  }

  private void ReleaseFragments()
  {
    this.ReleaseFragmentPipe(new Vector2i(106, -15), new Vector2(-4.0, -10.0), 2.2);
    this.ReleaseFragmentPipe(new Vector2i(118, -15), new Vector2(-2.0, -15.0), 1.4);
    this.ReleaseFragmentPipe(new Vector2i(130, -15), new Vector2(5.0, -12.0), 2.0);
    for (int index = 0; index < 16 /*0x10*/; ++index)
      this.ReleaseFragmentGlass(new Vector2i(-120, 0), new Vector2((double) -this.Level.Random.Next(1, 8), (double) -this.Level.Random.Next(-4, -1)));
    this.ReleaseFragmentMetal(new Vector2i(-14, -69), new Vector2(-10.0, -8.0));
    this.ReleaseFragmentMetal(new Vector2i(102, 0), new Vector2(-8.0, -6.0));
    this.ReleaseFragmentMetal(new Vector2i(132, 0), new Vector2(6.0, -8.0));
    this.ReleaseFragmentMetal(new Vector2i(102, -30), new Vector2(-4.0, -14.0));
    this.ReleaseFragmentMetal(new Vector2i(132, -28), new Vector2(8.0, -12.0));
    this.ReleaseFragmentMetal(new Vector2i(176 /*0xB0*/, 18), new Vector2(12.0, -6.0));
    this.ReleaseFragmentRed(new Vector2i(-48, 10), new Vector2(-3.0, -8.0));
    this.ReleaseFragmentRed(new Vector2i(-48, 10), new Vector2(5.0, -8.0));
    this.ReleaseFragmentRed(new Vector2i(36, 46), new Vector2(-4.0, -4.0));
  }

  private void ReleaseFragmentGlass(Vector2i offset, Vector2 velocity)
  {
    int num1 = this.Level.Random.Next(0, 3);
    int num2 = this._velocityX > 0 ? -1 : 1;
    offset.X *= num2;
    velocity.X *= (double) num2;
    Fragment fragment = this.Level.ObjectManager.AddSubObject<Fragment>((ActiveObject) this);
    fragment.AnimationGroup = this._animationGroup;
    fragment.AnimationIndex = 13;
    fragment.Position = fragment.Position + offset;
    fragment.Velocity = velocity;
    fragment.Scale = 1.0 - (double) num1 * 0.25;
    fragment.AngularVelocity = 2.0 * Math.PI / ((double) (num1 + 1) * 0.5 * 60.0);
    fragment.Angle = this.Level.Random.NextRadians();
    fragment.FlipX = this._velocityX > 0;
    fragment.FlipY = this.Level.Random.NextBoolean();
    fragment.Initialise();
  }

  private void ReleaseFragmentMetal(Vector2i offset, Vector2 velocity)
  {
    int num = this._velocityX > 0 ? -1 : 1;
    offset.X *= num;
    velocity.X *= (double) num;
    Fragment fragment = this.Level.ObjectManager.AddSubObject<Fragment>((ActiveObject) this);
    fragment.AnimationGroup = this._animationGroup;
    fragment.AnimationIndex = 14;
    fragment.Position = fragment.Position + offset;
    fragment.Velocity = velocity;
    fragment.AngularVelocity = Math.PI / 30.0;
    fragment.FlipX = this._velocityX > 0;
    fragment.Initialise();
  }

  private void ReleaseFragmentRed(Vector2i offset, Vector2 velocity)
  {
    int num = this._velocityX > 0 ? -1 : 1;
    offset.X *= num;
    velocity.X *= (double) num;
    Fragment fragment = this.Level.ObjectManager.AddSubObject<Fragment>((ActiveObject) this);
    fragment.AnimationGroup = this._animationGroup;
    fragment.AnimationIndex = 15;
    fragment.Position = fragment.Position + offset;
    fragment.Velocity = velocity;
    fragment.AngularVelocity = Math.PI / 15.0;
    fragment.FlipX = this._velocityX > 0;
    fragment.Initialise();
  }

  private void ReleaseFragmentPipe(
    Vector2i offset,
    Vector2 velocity,
    double angularVelocityTimePeriod)
  {
    int num = this._velocityX > 0 ? -1 : 1;
    offset.X *= num;
    velocity.X *= (double) num;
    Fragment fragment = this.Level.ObjectManager.AddSubObject<Fragment>((ActiveObject) this);
    fragment.AnimationGroup = this._animationGroup;
    fragment.AnimationIndex = 16 /*0x10*/;
    fragment.Position = fragment.Position + offset;
    fragment.Velocity = velocity;
    fragment.AngularVelocity = 2.0 * Math.PI / (angularVelocityTimePeriod * 60.0);
    fragment.FlipX = this._velocityX > 0;
    fragment.Initialise();
  }

  private void ReleaseWheels()
  {
    for (int index = 0; index < 4; ++index)
    {
      Vector2 vector2 = new Vector2(-8.0, -12.0);
      if (this._velocityX > 0)
        vector2.X *= -1.0;
      if (index == 1 || index == 3)
        vector2.X *= -1.0;
      this._wheels[index].Velocity = vector2;
      this._wheels[index].Released = true;
      this._wheels[index].LockLifetime = false;
    }
  }

  private void ReleaseDrill()
  {
    if (this._drill.Released)
      return;
    this._drill.Released = true;
    this._drill.Velocity = new Vector2i(-12 * (this._drill.FlipX ? -1 : 1), 0);
    this._drill.LockLifetime = false;
  }

  protected override void OnHurtCharacter(ICharacter character)
  {
    this._eggMobile.DoRobotnikSmile();
  }

  protected override void OnAnimate()
  {
    this._animation.Animate();
    this._carLightAnimation.Animate();
    this._sparkAnimation.Animate();
    this._brokenVentAnimation.Animate();
  }

  protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
  {
    bool flipX = this._velocityX > 0;
    Vector2i destination = new Vector2i(-32 * (flipX ? -1 : 1), 32 /*0x20*/);
    if (this.Health == 1)
    {
      int num = 4;
      double a = 2.0 * Math.PI * ((double) (this.Level.Ticks % num) / (double) num);
      destination.Y += (int) (Math.Sin(a) * 2.0);
    }
    IObjectRenderer objectRenderer = renderer.GetObjectRenderer();
    float num1 = this.Health == 1 ? this._lastHealthFlash : 0.0f;
    if (this.IsInvincibleFlashing)
      num1 = 1f;
    float num2 = num1 * 0.25f;
    objectRenderer.AdditiveColour = (double) num2 > 0.0 ? new Colour((double) num2, (double) num2, (double) num2) : Colours.Black;
    objectRenderer.Render(this._animation, (Vector2) destination, flipX);
    if (this._status == DrillEggmanInstance.StatusType.Normal)
    {
      objectRenderer.AdditiveColour = Colours.Black;
      objectRenderer.BlendMode = BlendMode.Additive;
      objectRenderer.EmitsLight = true;
      objectRenderer.Render(this._carLightAnimation, (Vector2) destination + new Vector2(flipX ? 93.0 : -93.0, -24.0), flipX);
      if (this.Health == 1 && this._sparkAnimation.Cycles == 0)
      {
        Animation.Frame currentFrame = this._sparkAnimation.CurrentFrame;
        Vector2i offset = currentFrame.Offset;
        if (flipX)
          offset.X *= -1;
        objectRenderer.Texture = this._sparkAnimation.CurrentTexture;
        objectRenderer.Render((Rectangle) currentFrame.Source, (Vector2) currentFrame.Offset, flipX);
      }
      objectRenderer.EmitsLight = false;
      objectRenderer.BlendMode = BlendMode.Alpha;
    }
    if (this._status < DrillEggmanInstance.StatusType.Defeated)
      return;
    objectRenderer.Render(this._brokenVentAnimation, (Vector2) destination + new Vector2(flipX ? -112.0 : 112.0, -16.0), flipX);
  }

  public enum StatusType
  {
    InitialApproch,
    FinalApproach,
    LowerPropeller,
    Normal,
    Defeated,
    PostDefeatedIdleA,
    PostDefeatStartPropeller,
    PostDefeatB,
    Finished,
  }

  private class EggMobile : ActiveObject
  {
    private AnimationInstance _animationEggMobileChasis;
    private AnimationInstance _animationEggMobileScreen;
    private AnimationInstance _animationPropeller;
    private AnimationInstance _animationRobotnik;
    private LevelSound _helicopterSound;
    private int _propellerOffset;
    private int _fleeTicks;

    public bool FlipX { get; set; }

    private bool Turning { get; set; }

    public DrillEggmanInstance DrillEggman => (DrillEggmanInstance) this.ParentObject;

    protected override void OnStart()
    {
      ResourceTree resourceTree = this.Level.GameContext.ResourceTree;
      AnimationGroup loadedResource1 = resourceTree.GetLoadedResource<AnimationGroup>((ILoadedResource) this.Type, "/ANIGROUP");
      AnimationGroup loadedResource2 = resourceTree.GetLoadedResource<AnimationGroup>((ILoadedResource) this.Type, "SONICORCA/OBJECTS/ROBOTNIK/ANIGROUP");
      this._animationEggMobileChasis = new AnimationInstance(loadedResource1, 19);
      this._animationEggMobileScreen = new AnimationInstance(loadedResource1, 20);
      this._animationPropeller = new AnimationInstance(loadedResource1, 8);
      this._animationRobotnik = new AnimationInstance(loadedResource2);
      this._helicopterSound = new LevelSound(this.Level, resourceTree.GetLoadedResource<SampleInfo>((ILoadedResource) this.Type, "SONICORCA/SOUND/BOSSHELICOPTER"), autoFinish: false);
      this._helicopterSound.DistanceAudible = 3840 /*0x0F00*/;
      this.Level.SoundManager.AddLevelSound(this._helicopterSound);
      this._propellerOffset = -136;
      this.LockLifetime = true;
      this.Turning = false;
    }

    protected override void OnUpdate()
    {
      switch (this.DrillEggman._status)
      {
        case DrillEggmanInstance.StatusType.InitialApproch:
          this.Position = this.Position + new Vector2i(-4, 4);
          this._helicopterSound.Play();
          break;
        case DrillEggmanInstance.StatusType.FinalApproach:
          this.Position = this.Position + new Vector2i(0, 4);
          break;
        case DrillEggmanInstance.StatusType.LowerPropeller:
          if (this.DrillEggman._stateTimer <= 41)
          {
            ++this._propellerOffset;
            break;
          }
          break;
        case DrillEggmanInstance.StatusType.Normal:
          this._animationEggMobileChasis.Index = 18;
          break;
        case DrillEggmanInstance.StatusType.PostDefeatB:
          this._animationEggMobileChasis.Index = 19;
          if (this.DrillEggman._stateTimer > 8)
          {
            --this._propellerOffset;
            break;
          }
          if (this.DrillEggman._stateTimer == 8)
          {
            this._animationPropeller.Index = 7;
            break;
          }
          break;
        case DrillEggmanInstance.StatusType.Finished:
          this._helicopterSound.Play();
          if (this.DrillEggman._stateTimer > 0)
            this.Position = this.Position + new Vector2i(0, -4);
          else if (!this.FlipX)
          {
            if (this._animationEggMobileChasis.Index != 30)
            {
              this._animationEggMobileChasis.Index = 30;
              this._animationEggMobileChasis.Cycles = 0;
              this._animationEggMobileScreen.Index = 32 /*0x20*/;
              this.Turning = false;
            }
            else if (this._animationEggMobileChasis.Cycles >= 1)
            {
              this._animationEggMobileChasis.Index = 31 /*0x1F*/;
              this._animationEggMobileScreen.Index = 33;
              this.FlipX = true;
            }
          }
          else if (this._animationEggMobileChasis.Cycles >= 2 && this.Turning)
          {
            this.Turning = false;
            this._animationEggMobileScreen.Index = 28;
          }
          else if (this.FlipX)
            this.Position = this.Position + new Vector2i(24, 0);
          ++this._fleeTicks;
          if (this._fleeTicks > 1200)
          {
            this.LockLifetime = false;
            break;
          }
          break;
      }
      this._helicopterSound.Position = this.Position;
    }

    public void BeginLoweringPropeller()
    {
      this._animationPropeller.Index = 9;
      this._helicopterSound.Stop();
    }

    public void DoEggMobileBreak()
    {
      this._animationEggMobileChasis.Index = 26;
      this._animationEggMobileScreen.Index = 28;
    }

    public void DoRobotnikSmile()
    {
      if (this._animationRobotnik.Index != 0)
        return;
      this._animationRobotnik.Index = 1;
    }

    public void DoRobotnikFrown() => this._animationRobotnik.Index = 2;

    public void DoRobotnikDefeated() => this._animationRobotnik.Index = 3;

    public void DoRobotnikNormal() => this._animationRobotnik.Index = 0;

    protected override void OnStop() => this._helicopterSound.Dispose();

    protected override void OnAnimate()
    {
      this._animationEggMobileChasis.Animate();
      this._animationEggMobileScreen.Animate();
      this._animationPropeller.Animate();
      this._animationRobotnik.Animate();
    }

    protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
    {
      IObjectRenderer objectRenderer = renderer.GetObjectRenderer();
      using (objectRenderer.BeginMatixState())
      {
        if (this.FlipX)
          objectRenderer.ModelMatrix *= Matrix4.CreateScale(-1.0, 1.0);
        Vector2i destination1 = new Vector2i(36, this._propellerOffset);
        Vector2i destination2 = new Vector2i(-8, -26);
        Vector2i vector2i = destination2 + new Vector2i(0, 0);
        Vector2i destination3 = destination2 + new Vector2i(0, -76);
        Vector2i destination4 = destination2 + new Vector2i(0, 0);
        objectRenderer.FilterAmount *= 0.5;
        if (this.DrillEggman.IsInvincibleFlashing)
          objectRenderer.AdditiveColour = BossObject.FlashAdditiveColour;
        objectRenderer.Render(this._animationPropeller, (Vector2) destination1);
        objectRenderer.Render(this._animationEggMobileChasis, (Vector2) destination2);
        objectRenderer.AdditiveColour = Colours.Black;
        objectRenderer.Render(this._animationRobotnik, (Vector2) destination3);
        if (this.DrillEggman.IsInvincibleFlashing)
          objectRenderer.AdditiveColour = BossObject.FlashAdditiveColour;
        objectRenderer.Render(this._animationEggMobileScreen, (Vector2) destination4);
      }
    }
  }

  private class Drill : Enemy
  {
    private AnimationInstance _animation;

    public Vector2i Velocity { get; set; }

    public bool FlipX { get; set; }

    public bool Released { get; set; }

    public DrillEggmanInstance DrillEggman => (DrillEggmanInstance) this.ParentObject;

    protected override void OnStart()
    {
      base.OnStart();
      this.CollisionRectangles = new CollisionRectangle[1]
      {
        new CollisionRectangle((ActiveObject) this, 0, -96, -96, 192 /*0xC0*/, 192 /*0xC0*/)
      };
      this._animation = new AnimationInstance(this.ResourceTree, this.Type.GetAbsolutePath("/ANIGROUP"), 5);
    }

    protected override void OnUpdate()
    {
      base.OnUpdate();
      this.Position = this.Position + this.Velocity;
    }

    protected override void OnCollision(CollisionEvent e)
    {
      if (this.DrillEggman._status < DrillEggmanInstance.StatusType.Normal)
        return;
      base.OnCollision(e);
    }

    protected override void OnHurtCharacter(ICharacter character)
    {
      (this.ParentObject as DrillEggmanInstance)._eggMobile.DoRobotnikSmile();
    }

    protected override void OnAnimate()
    {
      base.OnAnimate();
      if (this.DrillEggman._status < DrillEggmanInstance.StatusType.LowerPropeller)
        return;
      this._animation.OverrideDelay = this.DrillEggman._status != DrillEggmanInstance.StatusType.LowerPropeller ? new int?() : new int?(3);
      this._animation.Animate();
    }

    protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
    {
      renderer.GetObjectRenderer().Render(this._animation, this.FlipX);
    }
  }

  private class Wheel : ActiveObject
  {
    private const double Gravity = 0.875;
    private static readonly Vector2i[] LeftPositions = new Vector2i[4]
    {
      new Vector2i(-48, 80 /*0x50*/),
      new Vector2i(144 /*0x90*/, 80 /*0x50*/),
      new Vector2i(-192, 80 /*0x50*/),
      new Vector2i(0, 80 /*0x50*/)
    };
    private static readonly Vector2i[] RightPositions = new Vector2i[4]
    {
      new Vector2i(48 /*0x30*/, 80 /*0x50*/),
      new Vector2i(-144, 80 /*0x50*/),
      new Vector2i(192 /*0xC0*/, 80 /*0x50*/),
      new Vector2i(0, 80 /*0x50*/)
    };
    private AnimationInstance _animation;

    public int Index { get; set; }

    public Vector2 Velocity { get; set; }

    public bool FlipX { get; set; }

    public bool Released { get; set; }

    public DrillEggmanInstance DrillEggman => (DrillEggmanInstance) this.ParentObject;

    protected override void OnStart()
    {
      this._animation = new AnimationInstance(this.ResourceTree, this.Type.GetAbsolutePath("/ANIGROUP"));
    }

    protected override void OnUpdate()
    {
      if (this.Released)
      {
        this.PositionPrecise = this.PositionPrecise + this.Velocity;
        this.Velocity += new Vector2(0.0, 0.875);
        if (this.Velocity.Y <= 0.0 || !this.MoveToGroundLevel())
          return;
        this.Velocity += new Vector2(0.0, -8.0);
      }
      else
      {
        this.AutoPositionToCar();
        this.Position = this.Position + new Vector2i(this.DrillEggman._velocityX, 0);
        this.MoveToGroundLevel();
      }
    }

    public void AutoPositionToCar()
    {
      this.FlipX = this.DrillEggman._velocityX > 0;
      this.Position = this.DrillEggman.Position + (this.DrillEggman._velocityX <= 0 ? DrillEggmanInstance.Wheel.LeftPositions[this.Index] : DrillEggmanInstance.Wheel.RightPositions[this.Index]);
    }

    private bool MoveToGroundLevel()
    {
      bool groundLevel = false;
      foreach (CollisionInfo collision in CollisionVector.GetCollisions((ActiveObject) this, 64 /*0x40*/))
      {
        if (collision.Vector.Owner == null && !collision.Vector.IsWall)
        {
          this.PositionPrecise = new Vector2(this.PositionPrecise.X, this.PositionPrecise.Y + collision.Shift);
          groundLevel = true;
        }
      }
      return groundLevel;
    }

    protected override void OnAnimate()
    {
      bool flag = false;
      if (this.PositionPrecise - this.LastPositionPrecise != new Vector2())
        flag = true;
      this._animation.Index = this.Index == 0 || this.Index == 1 ? (flag ? 1 : 2) : (flag ? 3 : 4);
      this._animation.Animate();
    }

    protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
    {
      IObjectRenderer objectRenderer = renderer.GetObjectRenderer();
      if (this.DrillEggman.IsInvincibleFlashing)
        objectRenderer.AdditiveColour = BossObject.FlashAdditiveColour;
      objectRenderer.Render(this._animation, this.FlipX);
    }
  }
}
