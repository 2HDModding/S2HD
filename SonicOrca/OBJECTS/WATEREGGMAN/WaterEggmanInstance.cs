// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.WATEREGGMAN.WaterEggmanInstance
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca;
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
using System.Collections.Generic;
using System.Linq;

namespace SONICORCA.OBJECTS.WATEREGGMAN
{

    public class WaterEggmanInstance : BossObject
    {
      public const int AnimationBase = 0;
      public const int AnimationBaseLightOn = 1;
      public const int AnimationBaseLightOff = 2;
      public const int AnimationArmCog = 3;
      public const int AnimationArmCogTurn = 4;
      public const int AnimationArm = 5;
      public const int AnimationTube = 6;
      public const int AnimationTubeSucking = 7;
      public const int AnimationBaseShadow = 8;
      public const int AnimationArmTap = 9;
      public const int AnimationArmTapDrip = 10;
      public const int AnimationDrip = 11;
      public const int AnimationJar = 12;
      public const int AnimationJarReflection = 13;
      public const int AnimationJarFillA = 14;
      public const int AnimationJarFillB = 15;
      public const int AnimationJarFillC = 16 /*0x10*/;
      public const int AnimationJarFillD = 17;
      public const int AnimationJarDroplet = 18;
      public const int AnimationJarLidClosed = 19;
      public const int AnimationJarLidBack = 20;
      public const int AnimationJarLidFront = 21;
      private const int AnimationEggMobileChasis = 22;
      private const int AnimationEggMobileChasisFlame = 23;
      private const int AnimationEggMobileScreen = 24;
      private const int AnimationEggMobileCap = 25;
      private const int AnimationEggMobileChasisTurn1 = 26;
      private const int AnimationEggMobileChasisTurn2 = 27;
      private const int AnimationEggMobileScreenTurn1 = 28;
      private const int AnimationEggMobileScreenTurn2 = 29;
      private const int AnimationArmTurn1 = 30;
      private const int AnimationArmTurn2 = 31 /*0x1F*/;
      private const int AnimationSplash = 32 /*0x20*/;
      private const int AnimationSplashDrop = 33;
      private const int AnimationSplashSmallDrop = 34;
      private const int AnimationSmoke = 35;
      private const int AnimationBrokenJar = 36;
      private const int AnimationBrokenBoiler = 37;
      private const int AnimationBrokenTube = 38;
      private const int AnimationBrokenBar = 39;
      private const int AnimationBrokenPieces = 40;
      private const int AnimationBrokenEggMobileChasis = 47;
      private const int AnimationBrokenEggMobileChasisFlame = 48 /*0x30*/;
      private const int AnimationBrokenEggMobileScreen = 49;
      private const int AnimationBrokenEggMobileCap = 50;
      private const int AnimationBrokenEggMobileChasisTurn1 = 51;
      private const int AnimationBrokenEggMobileChasisTurn2 = 52;
      private const int AnimationBrokenEggMobileScreenTurn1 = 53;
      private const int AnimationBrokenEggMobileScreenTurn2 = 54;
      private const int AnimationRobotnikFaceSplash = 55;
      private const int AnimationBrokenEggMobileCapTurn1 = 56;
      private const int AnimationBrokenEggMobileCapTurn2 = 57;
      private const int AnimationJarFluidBottomPrimary = 58;
      private const int AnimationJarFluidBottomSecondary = 59;
      private const int AnimationRobotnikNormal = 0;
      private const int AnimationRobotnikSmiling = 1;
      private const int AnimationRobotnikFrown = 2;
      private const int AnimationRobotnikDefeated = 3;
      private const int AnimationRobotnikTurn1 = 4;
      private const int AnimationRobotnikTurn2 = 5;
      private const int AnimationDustSmoke = 1;
      private WaterEggmanInstance.State _state;
      private AnimationGroup _animationGroup;
      private AnimationInstance _animationEggMobileChasis;
      private AnimationInstance _animationEggMobileScreen;
      private AnimationInstance _animationEggMobileCap;
      private AnimationInstance _animationRobotnik;
      private AnimationInstance _animationArmTurn;
      private AnimationInstance _animationRobotnikFaceSplash;
      private AnimationInstance _animationBase;
      private AnimationInstance _animationBaseShadow;
      private AnimationInstance _animationBaseLightOn;
      private AnimationInstance _animationBaseLightOff;
      private AnimationInstance _animationArm;
      private AnimationInstance _animationArmTap;
      private AnimationInstance _animationArmTapCog;
      private AnimationInstance _animationArmJar;
      private AnimationInstance _animationArmJarReflection;
      private AnimationInstance _animationArmJarLidClosed;
      private AnimationInstance _animationArmJarLidBack;
      private AnimationInstance _animationArmJarLidFront;
      private AnimationInstance _animationDrip;
      private AnimationInstance _animationArmJarFill;
      private AnimationInstance _animationArmJarFillPlaceholderPrimary;
      private AnimationInstance _animationArmJarFillPlaceholderSecondary;
      private AnimationInstance _animationArmJarFilled;
      private Vector2 _nonHoverPosition;
      private double _floatOffsetAngle;
      private bool _turning;
      private bool _flipX;
      private Updater _updater;
      private int _stateTicks;
      private int _armExtension;
      private int _tubeY;
      private int _fillSide;
      private bool _dripVisible;
      private Vector2i _dripPosition;
      private bool _jarEmpty;
      private bool _jarFull;
      private bool _willJarSplashRobotnik;
      private int _jarFillY;
      private bool _jarLidOpen;
      private double _velocityY;
      private WaterEggmanInstance.Boiler _boiler;
      private WaterEggmanInstance.JarBack _jarBack;
      private WaterEggmanInstance.ArmFront _armFront;
      private WaterEggmanInstance.SuctionTube _suctionTube;
      private bool _mobileExploding;
      private bool _isFaceSplash;

      public WaterEggmanInstance.State ActionState
      {
        get => this._state;
        set => this._state = value;
      }

      [HideInEditor]
      public Vector2i JarOffset
      {
        get
        {
          Vector2i jarOffset = new Vector2i(this._flipX ? 33 : -33, -186);
          jarOffset.X += this._flipX ? this._armExtension : -this._armExtension;
          return jarOffset;
        }
      }

      [StateVariable]
      public int LeftFillX { get; set; }

      [StateVariable]
      public int RightFillX { get; set; }

      protected override void OnStart()
      {
        ResourceTree resourceTree = this.Level.GameContext.ResourceTree;
        AnimationGroup animationGroup = this._animationGroup = resourceTree.GetLoadedResource<AnimationGroup>((ILoadedResource) this.Type, "/ANIGROUP");
        AnimationGroup loadedResource = resourceTree.GetLoadedResource<AnimationGroup>((ILoadedResource) this.Type, "SONICORCA/OBJECTS/ROBOTNIK/ANIGROUP");
        this._animationEggMobileChasis = new AnimationInstance(animationGroup, 23);
        this._animationEggMobileScreen = new AnimationInstance(animationGroup, 24);
        this._animationEggMobileCap = new AnimationInstance(animationGroup, 25);
        this._animationRobotnik = new AnimationInstance(loadedResource);
        this._animationArmTurn = new AnimationInstance(animationGroup, 30);
        this._animationRobotnikFaceSplash = new AnimationInstance(animationGroup, 55);
        this._animationBase = new AnimationInstance(animationGroup);
        this._animationBaseShadow = new AnimationInstance(animationGroup, 8);
        this._animationBaseLightOn = new AnimationInstance(animationGroup, 1);
        this._animationBaseLightOff = new AnimationInstance(animationGroup, 2);
        this._animationArm = new AnimationInstance(animationGroup, 5);
        this._animationArmTap = new AnimationInstance(animationGroup, 9);
        this._animationArmTapCog = new AnimationInstance(animationGroup, 3);
        this._animationArmJar = new AnimationInstance(animationGroup, 12);
        this._animationArmJarReflection = new AnimationInstance(animationGroup, 13);
        this._animationArmJarLidClosed = new AnimationInstance(animationGroup, 19);
        this._animationArmJarLidBack = new AnimationInstance(animationGroup, 20);
        this._animationArmJarLidFront = new AnimationInstance(animationGroup, 21);
        this._animationDrip = new AnimationInstance(animationGroup, 11);
        this._animationArmJarFill = new AnimationInstance(animationGroup, 14);
        this._animationArmJarFillPlaceholderPrimary = new AnimationInstance(animationGroup, 58);
        this._animationArmJarFillPlaceholderSecondary = new AnimationInstance(animationGroup, 59);
        this._animationArmJarFilled = new AnimationInstance(animationGroup, 17);
        this.ExplosionResourceKey = this.Type.GetAbsolutePath("SONICORCA/OBJECTS/EXPLOSION/BOSS");
        this.HitSoundResourceKey = this.Type.GetAbsolutePath("SONICORCA/SOUND/BOSSHIT");
        this.CollisionRectangles = new CollisionRectangle[1]
        {
          new CollisionRectangle((ActiveObject) this, 0, -100, -100, 200, 200)
        };
        this.LockLifetime = true;
        this._state = WaterEggmanInstance.State.MoveToSide;
        this.Health = 8;
        this._nonHoverPosition = (Vector2) this.Position;
        this._fillSide = 1;
        this._boiler = this.Level.ObjectManager.AddSubObject<WaterEggmanInstance.Boiler>((ActiveObject) this);
        this._jarBack = this.Level.ObjectManager.AddSubObject<WaterEggmanInstance.JarBack>((ActiveObject) this);
        this._armFront = this.Level.ObjectManager.AddSubObject<WaterEggmanInstance.ArmFront>((ActiveObject) this);
        this._suctionTube = this.Level.ObjectManager.AddSubObject<WaterEggmanInstance.SuctionTube>((ActiveObject) this);
        this._jarEmpty = true;
        this._isFaceSplash = false;
        this._updater = new Updater(this.GetUpdates());
      }

      protected override void OnUpdate()
      {
        base.OnUpdate();
        this._updater.Update();
        if (this._state != WaterEggmanInstance.State.Defeated && this._state != WaterEggmanInstance.State.Recover)
          this.Hover();
        if (!this._mobileExploding)
          return;
        this.UpdateExplosions(64 /*0x40*/);
      }

      private IEnumerable<UpdateResult> GetUpdates()
      {
        WaterEggmanInstance parentObject = this;
        while (parentObject._state < WaterEggmanInstance.State.Defeated)
        {
          switch (parentObject._state)
          {
            case WaterEggmanInstance.State.MoveToSide:
              int dest1 = parentObject._fillSide == -1 ? parentObject.LeftFillX : parentObject.RightFillX;
              parentObject._nonHoverPosition.X = MathX.GoTowards(parentObject._nonHoverPosition.X, (double) dest1, 12.0);
              parentObject._armExtension = Math.Max(0, parentObject._armExtension - 8);
              int num = (parentObject.LeftFillX + parentObject.RightFillX) / 2;
              bool flag = parentObject._nonHoverPosition.X < (double) num;
              if (parentObject._armExtension == 0)
              {
                if (parentObject._flipX != flag)
                {
                  if (parentObject._animationEggMobileChasis.Index != 26)
                  {
                    parentObject._animationEggMobileChasis.Index = 26;
                    parentObject._animationEggMobileChasis.Cycles = 0;
                    parentObject._animationEggMobileScreen.Index = 28;
                    parentObject._animationRobotnik.Index = 4;
                    parentObject._animationArmTurn.Index = 30;
                    parentObject._animationArmTurn.ResetFrame();
                    parentObject._animationEggMobileCap.Index = 56;
                    parentObject._turning = true;
                  }
                  else if (parentObject._animationEggMobileChasis.Cycles >= 1)
                  {
                    parentObject._animationEggMobileChasis.Index = 27;
                    parentObject._animationEggMobileScreen.Index = 29;
                    parentObject._animationRobotnik.Index = 5;
                    parentObject._animationArmTurn.Index = 31 /*0x1F*/;
                    parentObject._animationArmTurn.ResetFrame();
                    parentObject._animationEggMobileCap.Index = 57;
                    parentObject._flipX = flag;
                  }
                }
                else if (parentObject._animationEggMobileChasis.Cycles >= 2)
                  parentObject._turning = false;
              }
              if (parentObject._nonHoverPosition.X == (double) dest1 && parentObject._armExtension == 0 && parentObject._flipX == flag && !parentObject._turning)
              {
                parentObject._state = WaterEggmanInstance.State.LoweringTube;
                break;
              }
              break;
            case WaterEggmanInstance.State.LoweringTube:
              if (parentObject._tubeY < 396)
              {
                parentObject._tubeY += 33;
                parentObject._armExtension = Math.Max(0, parentObject._armExtension - 8);
                break;
              }
              parentObject._tubeY = 396;
              parentObject._armExtension = 0;
              parentObject._stateTicks = 0;
              parentObject._state = WaterEggmanInstance.State.FillingJar;
              break;
            case WaterEggmanInstance.State.FillingJar:
              if (parentObject._stateTicks < 236)
              {
                ++parentObject._stateTicks;
                if (parentObject._stateTicks > 100)
                  parentObject._willJarSplashRobotnik = true;
                if (parentObject._stateTicks > 176 /*0xB0*/)
                {
                  parentObject._jarFull = true;
                  break;
                }
                break;
              }
              parentObject._state = WaterEggmanInstance.State.RaisingTube;
              break;
            case WaterEggmanInstance.State.RaisingTube:
              if (parentObject._tubeY > 0)
              {
                parentObject._tubeY -= 33;
                break;
              }
              parentObject._tubeY = 0;
              parentObject._state = WaterEggmanInstance.State.Aiming;
              break;
            case WaterEggmanInstance.State.Aiming:
              parentObject._armExtension = Math.Min(256 /*0x0100*/, parentObject._armExtension + 8);
              if (parentObject._armExtension >= 24)
                parentObject._willJarSplashRobotnik = false;
              // ISSUE: explicit non-virtual call
              ICharacter protagonist = __nonvirtual (parentObject.Level).Player.Protagonist;
              int dest2 = parentObject._fillSide != -1 ? protagonist.Position.X + 256 /*0x0100*/ + 48 /*0x30*/ : protagonist.Position.X - 256 /*0x0100*/ - 48 /*0x30*/;
              parentObject._nonHoverPosition.X = MathX.GoTowards(parentObject._nonHoverPosition.X, (double) dest2, 12.0);
              if (parentObject._armExtension == 256 /*0x0100*/ && parentObject._nonHoverPosition.X == (double) dest2)
              {
                parentObject._stateTicks = 0;
                parentObject._state = WaterEggmanInstance.State.DroppingChemical;
                break;
              }
              break;
            case WaterEggmanInstance.State.DroppingChemical:
              if (parentObject._stateTicks == 2)
              {
                Vector2i vector2i = new Vector2i(parentObject._flipX ? 33 : -33, -186);
                vector2i.X += parentObject._flipX ? parentObject._armExtension : -parentObject._armExtension;
                vector2i.X += parentObject._flipX ? 8 : -8;
                vector2i.Y -= 16 /*0x10*/;
                // ISSUE: explicit non-virtual call
                WaterEggmanInstance.ChemicalDroplet chemicalDroplet = __nonvirtual (parentObject.Level).ObjectManager.AddSubObject<WaterEggmanInstance.ChemicalDroplet>((ActiveObject) parentObject);
                chemicalDroplet.FlipX = parentObject._flipX;
                // ISSUE: explicit non-virtual call
                chemicalDroplet.Position = __nonvirtual (parentObject.Position) + vector2i;
                parentObject._jarEmpty = true;
                parentObject._jarFull = false;
              }
              if (parentObject._stateTicks < 30)
              {
                ++parentObject._stateTicks;
                break;
              }
              parentObject._fillSide = parentObject._fillSide == -1 ? 1 : -1;
              parentObject._state = WaterEggmanInstance.State.MoveToSide;
              break;
          }
          yield return UpdateResult.Next();
        }
        parentObject._animationEggMobileChasis.Index = 47;
        parentObject._animationEggMobileScreen.Index = 49;
        parentObject._isFaceSplash = parentObject._willJarSplashRobotnik;
        parentObject._velocityY = 0.0;
        parentObject._mobileExploding = true;
        yield return UpdateResult.Wait(10);
        while (parentObject._suctionTube.Explode())
          yield return UpdateResult.Next();
        yield return UpdateResult.Wait(10);
        while (parentObject._boiler.Explode())
          yield return UpdateResult.Next();
        yield return UpdateResult.Wait(10);
        while (parentObject._armFront.ExplodeJar())
          yield return UpdateResult.Next();
        yield return UpdateResult.Wait(10);
        parentObject._suctionTube.Fall();
        yield return UpdateResult.Wait(10);
        while (parentObject._armFront.ExplodeBar())
          yield return UpdateResult.Next();
        yield return UpdateResult.Wait(4);
        parentObject._boiler.Fall();
        yield return UpdateResult.Wait(40);
        parentObject._mobileExploding = false;
        do
        {
          parentObject._velocityY += 0.375;
          parentObject._nonHoverPosition += new Vector2(0.0, parentObject._velocityY);
          // ISSUE: explicit non-virtual call
          __nonvirtual (parentObject.PositionPrecise) = parentObject._nonHoverPosition;
          yield return UpdateResult.Next();
        }
        while (parentObject._velocityY < 15.0);
        parentObject._isFaceSplash = false;
        parentObject.DoRobotnikNormal();
        parentObject._state = WaterEggmanInstance.State.Recover;
        for (int i = 0; i < 60; ++i)
        {
          parentObject._nonHoverPosition += new Vector2(0.0, -6.0);
          // ISSUE: explicit non-virtual call
          __nonvirtual (parentObject.PositionPrecise) = parentObject._nonHoverPosition;
          yield return UpdateResult.Next();
        }
        parentObject.Defeated = true;
        parentObject._state = WaterEggmanInstance.State.Fleeing;
        parentObject.Fleeing = true;
        // ISSUE: explicit non-virtual call
        __nonvirtual (parentObject.PositionPrecise) = parentObject._nonHoverPosition;
        int hover_failure_ticks = 0;
        while (true)
        {
          parentObject._nonHoverPosition += new Vector2(12.0, 0.0);
          if (!parentObject._turning)
          {
            switch (hover_failure_ticks)
            {
              case 7:
                parentObject.EmitSmoke(new Vector2(85.0, -160.0), new Vector2(-7.0, -5.0));
                break;
              case 11:
                parentObject.EmitSmoke(new Vector2(50.0, -70.0), new Vector2(-3.0, -5.0));
                break;
              case 21:
                parentObject.EmitSmoke(new Vector2(85.0, -160.0), new Vector2(-7.0, -5.0));
                parentObject.EmitSmoke(new Vector2(-120.0, -180.0), new Vector2(-3.0, -5.0));
                break;
              case 22:
                parentObject.EmitSmoke(new Vector2(50.0, -70.0), new Vector2(-3.0, -5.0));
                break;
              case 35:
                parentObject.EmitSmoke(new Vector2(-80.0, -90.0), new Vector2(-3.0, -5.0));
                break;
              case 45:
                parentObject.EmitSmoke(new Vector2(0.0, -90.0), new Vector2(-3.0, -7.0));
                break;
            }
          }
          if (!parentObject._flipX)
          {
            if (parentObject._animationEggMobileChasis.Index != 51)
            {
              parentObject._animationEggMobileChasis.Index = 51;
              parentObject._animationEggMobileChasis.Cycles = 0;
              parentObject._animationEggMobileScreen.Index = 53;
              parentObject._animationEggMobileCap.Index = 56;
              parentObject._turning = false;
            }
            else if (parentObject._animationEggMobileChasis.Cycles >= 1)
            {
              parentObject._animationEggMobileChasis.Index = 52;
              parentObject._animationEggMobileScreen.Index = 54;
              parentObject._animationEggMobileCap.Index = 57;
              parentObject._flipX = true;
            }
          }
          else if (parentObject._animationEggMobileChasis.Cycles >= 2 && parentObject._turning)
          {
            parentObject._turning = false;
            parentObject._animationEggMobileScreen.Index = 49;
          }
          if (!parentObject._turning && hover_failure_ticks < 15 && parentObject._flipX)
          {
            parentObject._animationEggMobileChasis.Index = hover_failure_ticks % 5 != 0 ? 47 : 48 /*0x30*/;
            ++hover_failure_ticks;
            parentObject._nonHoverPosition += new Vector2(0.0, 2.0);
          }
          else if (!parentObject._turning && hover_failure_ticks >= 15 && hover_failure_ticks < 45 && parentObject._flipX)
          {
            if (parentObject._animationEggMobileChasis.Index != 48 /*0x30*/)
              parentObject._animationEggMobileChasis.Index = 48 /*0x30*/;
            if (hover_failure_ticks < 30)
              parentObject._nonHoverPosition += new Vector2(0.0, -2.0);
            ++hover_failure_ticks;
          }
          else
            hover_failure_ticks = 0;
          if (parentObject._stateTicks > 600)
            parentObject.LockLifetime = false;
          yield return UpdateResult.Next();
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

      protected override void Hit(ICharacter character)
      {
        base.Hit(character);
        if (this.Health <= 0)
          return;
        this.DoRobotnikFrown();
      }

      protected override void Defeat()
      {
        this._state = WaterEggmanInstance.State.Defeated;
        this._stateTicks = 0;
        this._nonHoverPosition = this.PositionPrecise;
        this.DoRobotnikDefeated();
        this.CollisionRectangles = new CollisionRectangle[0];
        this.Level.Player.GainScore(1000);
        this.Level.CreateScoreObject(1000, this.Position + new Vector2i(0, (int) sbyte.MinValue));
      }

      protected override void OnHurtCharacter(ICharacter character) => this.DoRobotnikSmile();

      public void DoRobotnikSmile()
      {
        if (this._animationRobotnik.Index != 0)
          return;
        this._animationRobotnik.Index = 1;
      }

      public void DoRobotnikFrown() => this._animationRobotnik.Index = 2;

      public void DoRobotnikDefeated() => this._animationRobotnik.Index = 3;

      public void DoRobotnikNormal() => this._animationRobotnik.Index = 0;

      protected override void OnAnimate()
      {
        switch (this._state)
        {
          case WaterEggmanInstance.State.MoveToSide:
            this._animationArmTapCog.Index = 3;
            this._animationArmTap.Index = 9;
            break;
          case WaterEggmanInstance.State.LoweringTube:
            this._animationArmTapCog.Index = 3;
            this._animationArmTap.Index = 9;
            this._jarFillY = 0;
            this._jarEmpty = true;
            this._dripVisible = false;
            this._suctionTube.TubeSuctionIndex = 22f;
            break;
          case WaterEggmanInstance.State.FillingJar:
            if (this._stateTicks < 140)
            {
              this._animationArmTapCog.Index = 4;
              this._animationArmTap.Index = 10;
              this._suctionTube.TubeSuctionIndex -= 0.5f;
              if ((double) this._suctionTube.TubeSuctionIndex <= 0.0)
                this._suctionTube.TubeSuctionIndex = 22f;
            }
            else if ((double) this._suctionTube.TubeSuctionIndex > 0.0)
              this._suctionTube.TubeSuctionIndex = Math.Max(0.0f, this._suctionTube.TubeSuctionIndex - 0.5f);
            if (this._dripVisible)
              this._dripPosition.Y += 12;
            if (this._stateTicks == 20)
            {
              this._dripVisible = true;
              this._dripPosition = new Vector2i();
            }
            else if (this._stateTicks == 24)
            {
              this._dripVisible = false;
              this._animationArmJarFill.Index = 14;
              this._animationArmJarFill.ResetFrame();
              this._jarEmpty = false;
            }
            else if (this._stateTicks == 50)
            {
              this._dripVisible = true;
              this._dripPosition = new Vector2i();
            }
            else if (this._stateTicks == 54)
            {
              this._dripVisible = false;
              this._animationArmJarFill.Index = 15;
            }
            else if (this._stateTicks == 80 /*0x50*/)
            {
              this._dripVisible = true;
              this._dripPosition = new Vector2i();
            }
            else if (this._stateTicks == 84)
            {
              this._dripVisible = false;
              this._animationArmJarFill.Index = 16 /*0x10*/;
              this._jarFillY -= 20;
            }
            else if (this._stateTicks == 110)
            {
              this._dripVisible = true;
              this._dripPosition = new Vector2i();
            }
            else if (this._stateTicks == 114)
            {
              this._dripVisible = false;
              this._animationArmJarFill.Index = 16 /*0x10*/;
              this._animationArmJarFill.ResetFrame();
              this._jarFillY -= 20;
            }
            else if (this._stateTicks == 138)
            {
              this._dripVisible = true;
              this._dripPosition = new Vector2i();
            }
            else if (this._stateTicks == 139)
              this._dripVisible = false;
            else if (this._stateTicks == 140)
            {
              this._animationArmJarFill.Index = 16 /*0x10*/;
              this._animationArmJarFill.ResetFrame();
              this._jarFillY -= 20;
            }
            this._animationArmJarFill.Animate();
            this._animationArmJarFilled.Animate();
            break;
          case WaterEggmanInstance.State.DroppingChemical:
            if (this._stateTicks == 0)
            {
              this._jarLidOpen = true;
              this._animationArmJarLidBack.ResetFrame();
              this._animationArmJarLidFront.ResetFrame();
              this._animationArmJarLidBack.Cycles = 0;
              this._animationArmJarLidFront.Cycles = 0;
              break;
            }
            break;
          case WaterEggmanInstance.State.Defeated:
            if (this._isFaceSplash)
            {
              this._animationRobotnikFaceSplash.Animate();
              break;
            }
            break;
        }
        this._animationEggMobileChasis.Animate();
        this._animationEggMobileScreen.Animate();
        this._animationEggMobileCap.Animate();
        this._animationRobotnik.Animate();
        this._animationArmTurn.Animate();
        this._animationBase.Animate();
        this._animationArm.Animate();
        this._animationArmTap.Animate();
        this._animationArmTapCog.Animate();
        this._animationArmJar.Animate();
        this._animationArmJarReflection.Animate();
        this._animationArmJarLidClosed.Animate();
        this._animationArmJarLidBack.Animate();
        this._animationArmJarLidFront.Animate();
        if (this._animationArmJarLidBack.Cycles <= 0)
          return;
        this._jarLidOpen = false;
      }

      protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
      {
        this.DrawEggMobile(renderer.GetObjectRenderer());
      }

      private void DrawEggMobile(IObjectRenderer or)
      {
        using (or.BeginMatixState())
        {
          if (this._flipX)
            or.ModelMatrix *= Matrix4.CreateScale(-1.0, 1.0);
          Vector2i destination1 = new Vector2i(-26);
          Vector2i destination2 = destination1 + new Vector2i(0, 0);
          Vector2i destination3 = destination1 + new Vector2i(0, -76);
          Vector2i destination4 = destination1 + new Vector2i(25, -223);
          Vector2i destination5 = destination1 + new Vector2i(0, 0);
          or.FilterAmount *= 0.5;
          if (this.IsInvincibleFlashing)
            or.AdditiveColour = BossObject.FlashAdditiveColour;
          or.Render(this._animationEggMobileCap, (Vector2) destination2);
          or.Render(this._animationEggMobileChasis, (Vector2) destination1);
          or.AdditiveColour = Colours.Black;
          if (this._isFaceSplash)
            or.Render(this._animationRobotnikFaceSplash, (Vector2) destination4);
          else
            or.Render(this._animationRobotnik, (Vector2) destination3);
          if (this.IsInvincibleFlashing)
            or.AdditiveColour = BossObject.FlashAdditiveColour;
          or.Render(this._animationEggMobileScreen, (Vector2) destination5);
        }
        using (or.BeginMatixState())
        {
          if (this._flipX)
            or.ModelMatrix *= Matrix4.CreateScale(-1.0, 1.0);
          Vector2i vector2i1 = new Vector2i(0);
          Vector2i vector2i2 = new Vector2i(60, -171);
          if (this._animationArmTurn.Index == 30)
          {
            if (this._animationArmTurn.CurrentFrameIndex == 0)
              vector2i2.Y = -172;
            else if (this._animationArmTurn.CurrentFrameIndex > 0)
              vector2i2.Y = -195;
          }
          if (this._animationArmTurn.Index == 31 /*0x1F*/)
          {
            if (this._animationArmTurn.CurrentFrameIndex == 3)
              vector2i2.Y = -172;
            else if (this._animationArmTurn.CurrentFrameIndex == 2)
              vector2i2.Y = -172;
            else if (this._animationArmTurn.CurrentFrameIndex < 3)
              vector2i2.Y = -195;
          }
          or.ModelMatrix *= Matrix4.CreateTranslation((double) vector2i2.X, (double) vector2i2.Y);
          Vector2i vector2i3 = new Vector2i(-64, 0);
          Vector2i destination = vector2i1 + vector2i3;
          if (!this._turning || this._state >= WaterEggmanInstance.State.Defeated)
            return;
          or.Render(this._animationArmTurn, (Vector2) destination);
        }
      }

      private void ReleaseFragmentMetal(Vector2i position, double speed, int count = 1, bool explode = true)
      {
        for (int index = 0; index < count; ++index)
        {
          double num = this.Level.Random.NextRadians();
          Vector2 velocity = new Vector2(speed * Math.Cos(num), speed * Math.Sin(num));
          this.ReleaseFragmentMetal(position, velocity);
        }
        if (!explode)
          return;
        this.ExplodeAt(position);
      }

      private void ReleaseFragmentMetal(Vector2i position, Vector2 velocity)
      {
        Fragment fragment = this.Level.ObjectManager.AddSubObject<Fragment>((ActiveObject) this);
        fragment.AnimationGroup = this._animationGroup;
        fragment.AnimationIndex = this.Level.Random.Next(40, 47);
        fragment.Position = position;
        fragment.Velocity = velocity;
        fragment.AngularVelocity = Math.PI / 30.0;
        fragment.FlipX = this.Level.Random.NextBoolean();
        fragment.Initialise();
      }

      public enum State
      {
        MoveToSide,
        LoweringTube,
        FillingJar,
        RaisingTube,
        Aiming,
        DroppingChemical,
        Defeated,
        Recover,
        Fleeing,
      }

      private enum ExplodeState
      {
        NotExploding,
        Exploding,
        Exploded,
        Falling,
      }

      private class Boiler : ActiveObject
      {
        private static Vector2i OriginOffset = new Vector2i(60, -176);
        private Updater _updater;
        private bool _flipX;
        private WaterEggmanInstance.ExplodeState _explodeState;
        private AnimationInstance _explodedAnimationInstance;
        private Vector2 _velocity;
        private double _angle;

        public WaterEggmanInstance Parent => this.ParentObject as WaterEggmanInstance;

        protected override void OnStart()
        {
          this._updater = new Updater(this.GetUpdates());
          this.Priority = 300;
          this.LockLifetime = true;
        }

        protected override void OnUpdate() => this._updater.Update();

        protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
        {
          if (this.Parent._turning)
            return;
          IObjectRenderer objectRenderer = renderer.GetObjectRenderer();
          using (objectRenderer.BeginMatixState())
          {
            if (this._flipX)
              objectRenderer.ModelMatrix *= Matrix4.CreateScale(-1.0, 1.0);
            objectRenderer.ModelMatrix *= Matrix4.CreateTranslation((double) WaterEggmanInstance.Boiler.OriginOffset.X, (double) WaterEggmanInstance.Boiler.OriginOffset.Y);
            if (this._explodeState == WaterEggmanInstance.ExplodeState.Falling)
              objectRenderer.ModelMatrix = objectRenderer.ModelMatrix.RotateZ(this._angle);
            objectRenderer.Render(this.Parent._animationBaseShadow, new Vector2(3.0, 100.0));
            if (this.Parent.IsInvincibleFlashing)
              objectRenderer.AdditiveColour = BossObject.FlashAdditiveColour;
            if (this._explodeState < WaterEggmanInstance.ExplodeState.Exploded)
            {
              objectRenderer.Render(this.Parent._animationBase, new Vector2(0.0, 0.0));
              objectRenderer.Render(this.Parent._animationArmTap, new Vector2(-69.0, -68.0));
              objectRenderer.Render(this.Parent._animationArmTapCog, new Vector2(8.0, -75.0));
              if (this.Parent._jarFull)
                objectRenderer.Render(this.Parent._animationBaseLightOn, new Vector2(-42.0, 12.0));
              else
                objectRenderer.Render(this.Parent._animationBaseLightOff, new Vector2(-42.0, 12.0));
            }
            else
              objectRenderer.Render(this._explodedAnimationInstance, new Vector2(-24.0, 0.0));
          }
        }

        private Vector2i GetAbsolutePosition(Vector2i offset)
        {
          Vector2i vector2i = WaterEggmanInstance.Boiler.OriginOffset + offset;
          if (this._flipX)
            vector2i.X *= -1;
          return this.Position + vector2i;
        }

        private IEnumerable<UpdateResult> GetUpdates()
        {
          WaterEggmanInstance.Boiler boiler = this;
          while (boiler._explodeState == WaterEggmanInstance.ExplodeState.NotExploding)
          {
            boiler._flipX = boiler.Parent._flipX;
            // ISSUE: explicit non-virtual call
            __nonvirtual (boiler.Position) = boiler.Parent.Position;
            if (boiler.Parent.Finished)
            {
              // ISSUE: explicit non-virtual call
              __nonvirtual (boiler.Finish());
            }
            yield return UpdateResult.Next();
          }
          boiler.Parent.ReleaseFragmentMetal(boiler.GetAbsolutePosition(new Vector2i(4, 100)), 8.0);
          yield return UpdateResult.Wait(2);
          boiler.Parent.ReleaseFragmentMetal(boiler.GetAbsolutePosition(new Vector2i(0, 0)), 8.0);
          yield return UpdateResult.Wait(2);
          boiler.Parent.ReleaseFragmentMetal(boiler.GetAbsolutePosition(new Vector2i(-69, -68)), 8.0);
          yield return UpdateResult.Next();
          boiler._explodeState = WaterEggmanInstance.ExplodeState.Exploded;
          boiler._explodedAnimationInstance = new AnimationInstance(boiler.Parent._animationGroup, 37);
          yield return UpdateResult.Wait(60);
          int flipMultiplier = boiler._flipX ? -1 : 1;
          while (boiler._explodeState != WaterEggmanInstance.ExplodeState.Falling)
            yield return UpdateResult.Next();
          boiler.Parent.ReleaseFragmentMetal(boiler.GetAbsolutePosition(new Vector2i(0, 100)), 8.0);
          for (int i = 0; i < 12; ++i)
          {
            boiler.Move(flipMultiplier, 1);
            yield return UpdateResult.Next();
          }
          boiler.LockLifetime = false;
          bool isUnderwater = false;
          while (true)
          {
            if (!isUnderwater)
            {
              // ISSUE: explicit non-virtual call
              // ISSUE: explicit non-virtual call
              if (__nonvirtual (boiler.Level).WaterManager.IsUnderwater(__nonvirtual (boiler.Position)))
              {
                isUnderwater = true;
                boiler._velocity = new Vector2(0.0, 1.0);
                // ISSUE: explicit non-virtual call
                // ISSUE: explicit non-virtual call
                // ISSUE: explicit non-virtual call
                __nonvirtual (boiler.Level).WaterManager.CreateSplash(__nonvirtual (boiler.Layer), SplashType.Enter, __nonvirtual (boiler.Position));
              }
              else
              {
                boiler._angle += Math.PI / 30.0 * (double) flipMultiplier;
                boiler._velocity += new Vector2(0.0, 0.875);
              }
            }
            else
            {
              boiler._angle += Math.PI / 75.0 * (double) flipMultiplier;
              boiler._velocity += new Vector2(0.0, 7.0 / 16.0);
            }
            boiler.MovePrecise(boiler._velocity);
            // ISSUE: explicit non-virtual call
            if (__nonvirtual (boiler.Level).Ticks % 4 == 0)
              boiler.EmitSmoke();
            yield return UpdateResult.Next();
          }
        }

        private void EmitSmoke()
        {
          Matrix4 matrix4 = Matrix4.Identity * Matrix4.CreateTranslation((Vector2) this.Position);
          if (this._flipX)
            matrix4 *= Matrix4.CreateScale(-1.0, 1.0);
          Vector2i position = (Vector2i) ((matrix4 * Matrix4.CreateTranslation((Vector2) WaterEggmanInstance.Boiler.OriginOffset)).RotateZ(this._angle) * new Vector2(0.0, 100.0));
          (this.Level.ObjectManager.AddObject(new ObjectPlacement("SONICORCA/OBJECTS/DUST", this.Level.Map.Layers.IndexOf(this.Layer), position)) as DustInstance).SetDustAnimationIndex(1);
        }

        public bool Explode()
        {
          if (this._explodeState == WaterEggmanInstance.ExplodeState.NotExploding)
          {
            this._explodeState = WaterEggmanInstance.ExplodeState.Exploding;
            return true;
          }
          return this._explodeState == WaterEggmanInstance.ExplodeState.Exploding;
        }

        public void Fall() => this._explodeState = WaterEggmanInstance.ExplodeState.Falling;
      }

      private class JarBack : ActiveObject
      {
        public WaterEggmanInstance Parent => this.ParentObject as WaterEggmanInstance;

        protected override void OnStart()
        {
          this.Priority = 310;
          this.LockLifetime = true;
        }

        protected override void OnUpdate()
        {
          if (!this.Parent.Finished)
            return;
          this.Finish();
        }

        protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
        {
          if (this.Parent._turning)
            return;
          IObjectRenderer objectRenderer = renderer.GetObjectRenderer();
          WaterEggmanInstance parent = this.Parent;
          if (parent._state == WaterEggmanInstance.State.Fleeing)
            return;
          using (objectRenderer.BeginMatixState())
          {
            objectRenderer.ModelMatrix *= Matrix4.CreateTranslation((Vector2) parent.JarOffset);
            if (parent._flipX)
              objectRenderer.ModelMatrix *= Matrix4.CreateScale(-1.0, 1.0);
            objectRenderer.Render(parent._animationArmJarReflection);
            if (!parent._jarLidOpen)
              return;
            objectRenderer.Render(parent._animationArmJarLidBack, new Vector2(-27.0, 69.0));
          }
        }
      }

      private class ArmFront : ActiveObject
      {
        private static readonly Vector2i ArmBeginPoint = new Vector2i(10, -186);
        private Updater _updater;
        private bool _flipX;
        private WaterEggmanInstance.ExplodeState _jarExplodeState;
        private WaterEggmanInstance.ExplodeState _barExplodeState;
        private int _barExplodedLength;
        private int _armJarPlaceholderY;

        public Vector2 Velocity { get; set; }

        public WaterEggmanInstance Parent => this.ParentObject as WaterEggmanInstance;

        protected override void OnStart()
        {
          this._updater = new Updater(this.GetUpdates());
          this.Priority = 330;
          this.LockLifetime = true;
          this._armJarPlaceholderY = 0;
        }

        protected override void OnUpdate()
        {
          this._updater.Update();
          this.Parent._jarBack.Position = this.Position;
        }

        protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
        {
          if (this.Parent._turning)
            return;
          IObjectRenderer objectRenderer = renderer.GetObjectRenderer();
          this.DrawArm(objectRenderer);
          if (this._jarExplodeState != WaterEggmanInstance.ExplodeState.NotExploding)
            return;
          this.DrawJar(objectRenderer, (Vector2) this.Parent.JarOffset);
        }

        private void DrawArm(IObjectRenderer or)
        {
          if (this.Parent.IsInvincibleFlashing)
            or.AdditiveColour = BossObject.FlashAdditiveColour;
          using (or.BeginMatixState())
          {
            if (this._flipX)
              or.ModelMatrix *= Matrix4.CreateScale(-1.0, 1.0);
            foreach (Vector2i armBarPosition in this.GetArmBarPositions())
              or.Render(this.Parent._animationArm, (Vector2) armBarPosition);
          }
          or.AdditiveColour = Colours.Black;
        }

        private void DrawJar(IObjectRenderer or, Vector2 offset)
        {
          if (this.Parent._dripVisible)
            or.Render(this.Parent._animationDrip, offset + (Vector2) this.Parent._dripPosition + (Vector2) new Vector2i(this._flipX ? -8 : 8, -32), this._flipX);
          if (!this.Parent._jarEmpty)
          {
            int x = this._flipX ? 8 : -8;
            int num1 = 47;
            bool flag;
            if (this.Parent._jarFillY < 0)
            {
              flag = true;
              this._armJarPlaceholderY = num1 + this.Parent._jarFillY;
            }
            else
              flag = false;
            for (int index = num1; index > num1 + this.Parent._jarFillY; index -= 11)
            {
              Rectanglei source = this.Parent._animationArmJarFilled.CurrentFrame.Source;
              Rectanglei destination = new Rectanglei((int) offset.X + x - source.Width / 2, (int) offset.Y + index - 16 /*0x10*/ - source.Height / 2, source.Width, source.Height);
              or.Texture = this.Parent._animationArmJarFilled.CurrentTexture;
              or.Render((Rectangle) source, (Rectangle) destination, this._flipX);
            }
            int y = num1 + this.Parent._jarFillY - this.Parent._animationArmJarFill.CurrentFrame.Source.Height / 2;
            if (this.Parent._animationArmJarFill.Index == 16 /*0x10*/)
              y += 17;
            or.Render(this.Parent._animationArmJarFill, offset + new Vector2((double) x, (double) y), this._flipX);
            if (this.Parent._animationArmJarFill.Index >= 15 && flag)
            {
              int num2 = this.Parent._jarFillY / -20;
              for (int index = 0; index < num2; ++index)
              {
                switch (num2)
                {
                  case 1:
                    or.Render(this.Parent._animationArmJarFillPlaceholderPrimary, offset + new Vector2((double) x, (double) (num1 + index * -20 - 16 /*0x10*/ + Math.Sign(index) * 8 * index)), this._flipX);
                    break;
                  case 2:
                    if (index == 0)
                    {
                      or.Render(this.Parent._animationArmJarFillPlaceholderPrimary, offset + new Vector2((double) x, (double) (num1 + index * -20 - 16 /*0x10*/ + Math.Sign(index) * 8 * index)), this._flipX);
                      break;
                    }
                    or.Render(this.Parent._animationArmJarFillPlaceholderPrimary, offset + new Vector2((double) x, (double) (num1 + index * -20 - 16 /*0x10*/ + Math.Sign(index) * 8 * index)), this._flipX, true);
                    break;
                  default:
                    if (index == 0)
                    {
                      or.Render(this.Parent._animationArmJarFillPlaceholderPrimary, offset + new Vector2((double) x, (double) (num1 + index * -20 - 16 /*0x10*/ + Math.Sign(index) * 8 * index)), this._flipX);
                      break;
                    }
                    if (index == num2 - 1)
                    {
                      or.Render(this.Parent._animationArmJarFillPlaceholderPrimary, offset + new Vector2((double) x, (double) (num1 + index * -20 - 16 /*0x10*/ + Math.Sign(index) * 8 * index)), this._flipX, true);
                      break;
                    }
                    or.Render(this.Parent._animationArmJarFillPlaceholderSecondary, offset + new Vector2((double) x, (double) (num1 + index * -20 - 16 /*0x10*/ + Math.Sign(index) * 8 * index)), this._flipX);
                    break;
                }
              }
            }
          }
          if (this.Parent.IsInvincibleFlashing)
            or.AdditiveColour = BossObject.FlashAdditiveColour;
          or.Render(this.Parent._animationArmJar, offset, this._flipX);
          or.AdditiveColour = Colours.Black;
          if (this.Parent._jarLidOpen)
            or.Render(this.Parent._animationArmJarLidFront, offset + new Vector2(this._flipX ? -14.0 : 14.0, 66.0), this._flipX);
          else
            or.Render(this.Parent._animationArmJarLidClosed, offset + new Vector2(this._flipX ? 8.0 : -8.0, 53.0), this._flipX);
        }

        private IEnumerable<UpdateResult> GetUpdates()
        {
          WaterEggmanInstance.ArmFront armFront = this;
          while (armFront._jarExplodeState == WaterEggmanInstance.ExplodeState.NotExploding)
          {
            armFront._flipX = armFront.Parent._flipX;
            // ISSUE: explicit non-virtual call
            __nonvirtual (armFront.Position) = armFront.Parent.Position;
            if (armFront.Parent.Finished)
            {
              // ISSUE: explicit non-virtual call
              __nonvirtual (armFront.Finish());
            }
            yield return UpdateResult.Next();
          }
          WaterEggmanInstance.BrokenJar brokenJar = armFront.CreateBrokenJar();
          armFront._jarExplodeState = WaterEggmanInstance.ExplodeState.Exploded;
          while (armFront._barExplodeState != WaterEggmanInstance.ExplodeState.Exploding)
            yield return UpdateResult.Next();
          brokenJar.Fall();
          armFront._jarExplodeState = WaterEggmanInstance.ExplodeState.Falling;
          yield return UpdateResult.Wait(2);
          Vector2i[] vector2iArray = armFront.GetArmBarPositions().Reverse<Vector2i>().ToArray<Vector2i>();
          for (int index = 0; index < vector2iArray.Length; ++index)
          {
            Vector2i vector2i = vector2iArray[index];
            armFront._barExplodedLength += 16 /*0x10*/;
            // ISSUE: explicit non-virtual call
            armFront.EmitBar(__nonvirtual (armFront.Position) + vector2i);
            yield return UpdateResult.Wait(4);
          }
          vector2iArray = (Vector2i[]) null;
          armFront._barExplodeState = WaterEggmanInstance.ExplodeState.Exploded;
        }

        private IEnumerable<Vector2i> GetArmBarPositions()
        {
          for (Vector2i position = WaterEggmanInstance.ArmFront.ArmBeginPoint; position.X >= -this.Parent._armExtension + 4 + this._barExplodedLength; position.X -= 16 /*0x10*/)
          {
            if (this._flipX)
              yield return position;
            else
              yield return position;
          }
        }

        private WaterEggmanInstance.BrokenJar CreateBrokenJar()
        {
          this.Parent._jarBack.Finish();
          Vector2i position = this.Position + this.Parent.JarOffset;
          this.Parent.ReleaseFragmentMetal(position, 12.0, 12);
          WaterEggmanInstance.BrokenJar brokenJar = this.Level.ObjectManager.AddSubObject<WaterEggmanInstance.BrokenJar>((ActiveObject) this.Parent);
          brokenJar.Position = position;
          return brokenJar;
        }

        private void EmitBar(Vector2i position)
        {
          Vector2 vector2 = new Vector2((double) this.Level.Random.NextSign() * this.Level.Random.NextDouble(2.0, 8.0), -12.0);
          Fragment fragment = this.Level.ObjectManager.AddSubObject<Fragment>((ActiveObject) this);
          fragment.AnimationGroup = this.Parent._animationGroup;
          fragment.AnimationIndex = 39;
          fragment.Position = position;
          fragment.Velocity = vector2;
          fragment.AngularVelocity = Math.PI / 20.0;
          fragment.Initialise();
          this.Parent.ExplodeAt(position);
        }

        public bool ExplodeJar()
        {
          if (this._jarExplodeState == WaterEggmanInstance.ExplodeState.NotExploding)
          {
            this._jarExplodeState = WaterEggmanInstance.ExplodeState.Exploding;
            return true;
          }
          return this._jarExplodeState == WaterEggmanInstance.ExplodeState.Exploding;
        }

        public bool ExplodeBar()
        {
          if (this._barExplodeState == WaterEggmanInstance.ExplodeState.NotExploding)
          {
            this._barExplodeState = WaterEggmanInstance.ExplodeState.Exploding;
            return true;
          }
          return this._barExplodeState == WaterEggmanInstance.ExplodeState.Exploding;
        }
      }

      private class BrokenJar : ActiveObject
      {
        private AnimationInstance _animation;
        private bool _falling;
        private Vector2 _velocity;
        private double _angle;
        private bool _isUnderwater;

        public WaterEggmanInstance Parent => this.ParentObject as WaterEggmanInstance;

        protected override void OnStart()
        {
          this._animation = new AnimationInstance(this.Parent._animationGroup, 36);
        }

        protected override void OnUpdate()
        {
          if (!this._falling)
            return;
          int num = this.Parent._flipX ? -1 : 1;
          if (!this._isUnderwater)
          {
            if (this.Level.WaterManager.IsUnderwater(this.Position))
            {
              this._isUnderwater = true;
              this._velocity = new Vector2(0.0, 1.0);
              this.Level.WaterManager.CreateSplash(this.Layer, SplashType.Enter, this.Position);
            }
            else
            {
              this._angle -= Math.PI / 30.0 * (double) num;
              this._velocity += new Vector2(0.0, 0.875);
            }
          }
          else
          {
            this._angle -= Math.PI / 75.0 * (double) num;
            this._velocity += new Vector2(0.0, 7.0 / 16.0);
          }
          this.MovePrecise(this._velocity);
        }

        protected override void OnAnimate() => this._animation.Animate();

        protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
        {
          IObjectRenderer objectRenderer = renderer.GetObjectRenderer();
          using (objectRenderer.BeginMatixState())
          {
            objectRenderer.ModelMatrix *= Matrix4.CreateRotationZ(this._angle);
            objectRenderer.Render(this._animation, this.Parent._flipX);
          }
        }

        public void Fall() => this._falling = true;
      }

      private class SuctionTube : ActiveObject
      {
        public const int TubeMaxIndex = 22;
        private Updater _updater;
        private bool _flipX;
        private int _numSegments;
        private int[] _frameMap = new int[26];
        private WaterEggmanInstance.ExplodeState _explodeState;
        private AnimationInstance _explodedAnimationInstance;
        private Vector2 _velocity;
        private double[] _angle = new double[2];

        public float TubeSuctionIndex { get; set; }

        public WaterEggmanInstance Parent => this.ParentObject as WaterEggmanInstance;

        protected override void OnStart()
        {
          this._updater = new Updater(this.GetUpdates());
          this.Priority = 124;
          this.LockLifetime = true;
        }

        protected override void OnUpdate() => this._updater.Update();

        protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
        {
          IObjectRenderer objectRenderer = renderer.GetObjectRenderer();
          int x = this._flipX ? 14 : -14;
          int y1 = 80 /*0x50*/;
          if (this._explodeState <= WaterEggmanInstance.ExplodeState.Exploded)
          {
            Animation animation = this.Parent._animationGroup[7];
            int[] frameMap = this._frameMap;
            Array.Clear((Array) frameMap, 0, frameMap.Length);
            int tubeSuctionIndex = (int) this.TubeSuctionIndex;
            if ((double) this.TubeSuctionIndex - (double) tubeSuctionIndex < 0.5)
            {
              frameMap[tubeSuctionIndex] = 1;
              frameMap[tubeSuctionIndex + 1] = 2;
              frameMap[tubeSuctionIndex + 2] = 4;
              frameMap[tubeSuctionIndex + 3] = 6;
            }
            else
            {
              frameMap[tubeSuctionIndex + 2] = 3;
              frameMap[tubeSuctionIndex + 3] = 5;
            }
            for (int index = 0; index < this._numSegments; ++index)
            {
              Animation.Frame frame = animation.Frames[index + 4 >= 22 ? 0 : frameMap[index + 4]];
              objectRenderer.Texture = this.Parent._animationGroup.Textures[frame.TextureIndex];
              objectRenderer.Render((Rectangle) frame.Source, new Vector2((double) x, (double) y1));
              y1 += 16 /*0x10*/;
            }
          }
          else
          {
            int num = 80 /*0x50*/ + this._numSegments * 16 /*0x10*/;
            int index = 0;
            int y2 = y1 - 8;
            while (y2 < num)
            {
              Rectanglei source = this._explodedAnimationInstance.CurrentFrame.Source;
              Rectanglei destination = (Rectanglei) new Rectangle((double) (x - source.Width / 2), (double) y2, (double) source.Width, (double) source.Height);
              if (destination.Bottom > num)
              {
                destination.Bottom = num;
                source.Height = destination.Height;
              }
              using (objectRenderer.BeginMatixState())
              {
                objectRenderer.ModelMatrix = objectRenderer.ModelMatrix.Translate((double) destination.Centre.X, (double) destination.Centre.Y).RotateZ(this._angle[index]);
                destination.X = -destination.Width / 2;
                destination.Y = -destination.Height / 2;
                objectRenderer.Texture = this._explodedAnimationInstance.CurrentTexture;
                objectRenderer.Render((Rectangle) source, (Rectangle) destination);
                y2 += source.Height;
              }
              index = (index + 1) % 2;
            }
          }
        }

        private IEnumerable<UpdateResult> GetUpdates()
        {
          WaterEggmanInstance.SuctionTube suctionTube = this;
          while (suctionTube._explodeState == WaterEggmanInstance.ExplodeState.NotExploding)
          {
            suctionTube._flipX = suctionTube.Parent._flipX;
            suctionTube._numSegments = (suctionTube.Parent._tubeY - 80 /*0x50*/) / 16 /*0x10*/;
            // ISSUE: explicit non-virtual call
            __nonvirtual (suctionTube.Position) = suctionTube.Parent.Position;
            if (suctionTube.Parent.Finished)
            {
              // ISSUE: explicit non-virtual call
              __nonvirtual (suctionTube.Finish());
            }
            yield return UpdateResult.Next();
          }
          int maxY = 80 /*0x50*/ + suctionTube._numSegments * 16 /*0x10*/;
          int x = suctionTube._flipX ? 32 /*0x20*/ : -24;
          int y = 80 /*0x50*/;
          while (y < maxY)
          {
            // ISSUE: explicit non-virtual call
            Vector2i position = __nonvirtual (suctionTube.Position) + new Vector2i(x, y);
            suctionTube.Parent.ReleaseFragmentMetal(position, 4.0);
            y += 136;
            yield return UpdateResult.Wait(4);
          }
          suctionTube._explodeState = WaterEggmanInstance.ExplodeState.Exploded;
          suctionTube._explodedAnimationInstance = new AnimationInstance(suctionTube.Parent._animationGroup, 38);
          while (suctionTube._explodeState != WaterEggmanInstance.ExplodeState.Falling)
            yield return UpdateResult.Next();
          // ISSUE: explicit non-virtual call
          suctionTube.Parent.ReleaseFragmentMetal(__nonvirtual (suctionTube.Position) + new Vector2i(0, 80 /*0x50*/), 4.0);
          suctionTube.LockLifetime = false;
          while (true)
          {
            suctionTube._angle[0] += Math.PI / 30.0;
            suctionTube._angle[1] -= Math.PI / 20.0;
            suctionTube._velocity += new Vector2(0.0, 0.875);
            suctionTube.MovePrecise(suctionTube._velocity);
            yield return UpdateResult.Next();
          }
        }

        public bool Explode()
        {
          if (this._explodeState == WaterEggmanInstance.ExplodeState.NotExploding)
          {
            this._explodeState = WaterEggmanInstance.ExplodeState.Exploding;
            return true;
          }
          return this._explodeState == WaterEggmanInstance.ExplodeState.Exploding;
        }

        public void Fall() => this._explodeState = WaterEggmanInstance.ExplodeState.Falling;
      }

      private class ChemicalDroplet : Enemy
      {
        private AnimationInstance _animationInstance;
        private double _velocityY;
        private Updater _updater;
        private bool _splashed;
        public bool FlipX;

        public WaterEggmanInstance Parent => this.ParentObject as WaterEggmanInstance;

        protected override void OnStart()
        {
          base.OnStart();
          this._updater = new Updater(this.GetUpdates());
          this._animationInstance = new AnimationInstance(this.Parent._animationGroup, 18);
          this.Priority = 320;
          this.CollisionRectangles = new CollisionRectangle[1]
          {
            new CollisionRectangle((ActiveObject) this, 0, -64, -64, 128 /*0x80*/, 128 /*0x80*/)
          };
        }

        protected override void OnUpdate()
        {
          base.OnUpdate();
          this._updater.Update();
        }

        private IEnumerable<UpdateResult> GetUpdates()
        {
          WaterEggmanInstance.ChemicalDroplet chemicalDroplet = this;
          do
          {
            yield return UpdateResult.Next();
            chemicalDroplet._velocityY += 0.875;
            chemicalDroplet.MovePrecise(0.0, chemicalDroplet._velocityY);
            // ISSUE: explicit non-virtual call
            // ISSUE: explicit non-virtual call
            if (__nonvirtual (chemicalDroplet.Level).WaterManager.IsUnderwater(__nonvirtual (chemicalDroplet.Position)))
            {
              // ISSUE: explicit non-virtual call
              __nonvirtual (chemicalDroplet.Finish());
              yield break;
            }
          }
          while (!chemicalDroplet.HasHitFloor());
          chemicalDroplet.EmitProjectiles();
          chemicalDroplet.EmitSmallProjectile(new Vector2i(-64, -12));
          chemicalDroplet.EmitSmallProjectile(new Vector2i(-38, -22));
          chemicalDroplet.EmitSmallProjectile(new Vector2i(32 /*0x20*/, -22));
          chemicalDroplet.EmitSmallProjectile(new Vector2i(60, -14));
          chemicalDroplet._splashed = true;
          chemicalDroplet._animationInstance = new AnimationInstance(chemicalDroplet.Parent._animationGroup, 32 /*0x20*/);
          while (chemicalDroplet._animationInstance.Cycles == 0)
            yield return UpdateResult.Next();
          // ISSUE: explicit non-virtual call
          __nonvirtual (chemicalDroplet.Finish());
        }

        private bool HasHitFloor()
        {
          double num = double.MaxValue;
          foreach (CollisionInfo collision in CollisionVector.GetCollisions((ActiveObject) this, 64 /*0x40*/))
          {
            if (collision.Vector.Mode == CollisionMode.Top)
            {
              ActiveObject owner = collision.Vector.Owner;
              CollisionEvent e = new CollisionEvent((ActiveObject) this, collision);
              if (owner != null)
              {
                owner.Collision(e);
                if (e.IgnoreCollision)
                  continue;
              }
              num = Math.Min(num, collision.Shift);
            }
          }
          if (num == double.MaxValue)
            return false;
          this.MovePrecise(0.0, num);
          return true;
        }

        private void EmitProjectiles()
        {
          for (int index = 0; index < 5; ++index)
          {
            WaterEggmanInstance.ChemicalDropletProjectile dropletProjectile = this.Level.ObjectManager.AddSubObject<WaterEggmanInstance.ChemicalDropletProjectile>((ActiveObject) this.Parent);
            dropletProjectile.Position = this.Position;
            dropletProjectile.Velocity = new Vector2((double) (this.Level.Random.NextSign() * this.Level.Random.Next(2, 5)), -this._velocityY - (double) this.Level.Random.Next(0, 4));
          }
        }

        private void EmitSmallProjectile(Vector2i position)
        {
          WaterEggmanInstance.SmallChemicalDropletProjectile dropletProjectile = this.Level.ObjectManager.AddSubObject<WaterEggmanInstance.SmallChemicalDropletProjectile>(this.ParentObject);
          dropletProjectile.Position = this.Position + position;
          double num = Math.Atan2((double) position.Y, (double) position.X);
          dropletProjectile.Velocity = new Vector2(8.0 * Math.Cos(num), 32.0 * Math.Sin(num));
        }

        protected override void OnHurtCharacter(ICharacter character) => this.Parent.DoRobotnikSmile();

        protected override void OnAnimate()
        {
          base.OnAnimate();
          this._animationInstance.Animate();
        }

        protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
        {
          IObjectRenderer objectRenderer = renderer.GetObjectRenderer();
          using (objectRenderer.BeginMatixState())
          {
            if (this.FlipX)
              objectRenderer.ModelMatrix *= Matrix4.CreateScale(-1.0, 1.0);
            if (!this._splashed)
            {
              objectRenderer.Render(this._animationInstance);
            }
            else
            {
              if (this._animationInstance.Cycles != 0)
                return;
              Vector2 vector2 = new Vector2(32.0, 0.0);
              Rectanglei source = this._animationInstance.CurrentFrame.Source;
              Rectanglei destination = (Rectanglei) new Rectangle((double) (-source.Width / 2) + vector2.X, (double) (64 /*0x40*/ - source.Height), (double) source.Width, (double) source.Height);
              objectRenderer.Texture = this._animationInstance.CurrentTexture;
              objectRenderer.Render((Rectangle) source, (Rectangle) destination);
            }
          }
        }
      }

      private class ChemicalDropletProjectile : Enemy
      {
        private AnimationInstance _animationInstance;

        public WaterEggmanInstance Parent => this.ParentObject as WaterEggmanInstance;

        public Vector2 Velocity { get; set; }

        protected override void OnStart()
        {
          base.OnStart();
          this._animationInstance = new AnimationInstance(this.Parent._animationGroup, 34);
          this.Priority = 320;
          this.CollisionRectangles = new CollisionRectangle[1]
          {
            new CollisionRectangle((ActiveObject) this, 0, -8, -8, 16 /*0x10*/, 16 /*0x10*/)
          };
        }

        protected override void OnUpdate()
        {
          base.OnUpdate();
          this.Velocity += new Vector2(0.0, 0.875);
          this.PositionPrecise = this.PositionPrecise + this.Velocity;
          if (!this.Level.WaterManager.IsUnderwater(this.Position) && !this.HasHitFloor())
            return;
          this.Finish();
        }

        private bool HasHitFloor()
        {
          foreach (CollisionInfo collision in CollisionVector.GetCollisions((ActiveObject) this, 8))
          {
            if (collision.Vector.Mode == CollisionMode.Top)
              return true;
          }
          return false;
        }

        protected override void OnHurtCharacter(ICharacter character) => this.Parent.DoRobotnikSmile();

        protected override void OnAnimate() => this._animationInstance.Animate();

        protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
        {
          renderer.GetObjectRenderer().Render(this._animationInstance);
        }
      }

      private class SmallChemicalDropletProjectile : ParticleObject
      {
        public Vector2 Velocity { get; set; }

        public SmallChemicalDropletProjectile()
          : base("/ANIGROUP", 33, 0)
        {
        }

        protected override void OnUpdate()
        {
          this.Velocity += new Vector2(0.0, 0.875);
          this.MovePrecise(this.Velocity);
          if (this.Level.WaterManager.IsUnderwater(this.Position))
            this.Finish();
          base.OnUpdate();
        }
      }
    }
}
