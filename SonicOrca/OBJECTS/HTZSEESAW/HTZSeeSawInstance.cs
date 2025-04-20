// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.HTZSEESAW.HTZSeeSawInstance
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
using SonicOrca.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SONICORCA.OBJECTS.HTZSEESAW
{

    public class HTZSeeSawInstance : ActiveObject
    {
      private readonly IReadOnlyList<double> VisualAngles = (IReadOnlyList<double>) new double[3]
      {
        -1.0 * Math.PI / 8.0,
        0.0,
        Math.PI / 8.0
      };
      private const int AnimationStand = 0;
      private const int AnimationPlatform = 1;
      private AnimationInstance _animationStand;
      private AnimationInstance _animationPlatform;
      private readonly List<ICharacter> _charactersOnLeftSide = new List<ICharacter>();
      private readonly List<ICharacter> _charactersOnMiddleSide = new List<ICharacter>();
      private readonly List<ICharacter> _charactersOnRightSide = new List<ICharacter>();
      private int _bias;
      private double _visualAngle;
      private HTZSeeSawInstance.Sol _sol;
      private double _maxCharacterLandVelocity;
      private double _strengthMultiplier = 1.0;

      [StateVariable]
      private bool FlipX { get; set; }

      [StateVariable]
      private double StrengthMultiplier
      {
        get => this._strengthMultiplier;
        set => this._strengthMultiplier = value;
      }

      private IEnumerable<ICharacter> CharactersOnObject
      {
        get
        {
          return this._charactersOnLeftSide.Concat<ICharacter>((IEnumerable<ICharacter>) this._charactersOnMiddleSide).Concat<ICharacter>((IEnumerable<ICharacter>) this._charactersOnRightSide);
        }
      }

      public HTZSeeSawInstance() => this.DesignBounds = new Rectanglei(-216, -132, 432, 264);

      protected override void OnStart()
      {
        AnimationGroup loadedResource = this.ResourceTree.GetLoadedResource<AnimationGroup>((ILoadedResource) this.Type, "/ANIGROUP");
        this._animationStand = new AnimationInstance(loadedResource);
        this._animationPlatform = new AnimationInstance(loadedResource, 1);
        this.CollisionVectors = new CollisionVector[1]
        {
          new CollisionVector((ActiveObject) this, new Vector2i(), new Vector2i(), flags: CollisionFlags.Conveyor | CollisionFlags.NoAngle | CollisionFlags.NoBalance | CollisionFlags.NoAutoAlignment)
        };
        this._bias = this.FlipX ? -1 : 1;
        this._visualAngle = this.VisualAngles[this._bias + 1];
        this._sol = this.Level.ObjectManager.AddSubObject<HTZSeeSawInstance.Sol>((ActiveObject) this);
        this._sol.Position = this.Position + new Vector2i(160 /*0xA0*/ * this._bias, 32 /*0x20*/);
        this._sol.Side = this._bias;
        this._sol.LockLifetime = true;
        this.Priority = -256;
        this.ShadowInfo.MaxShadowOffset = new Vector2i(16 /*0x10*/);
      }

      protected override void OnUpdateEditor()
      {
        base.OnUpdateEditor();
        this._sol.Position = this.Position + new Vector2i(160 /*0xA0*/ * this._bias, 32 /*0x20*/);
      }

      protected override void OnUpdate()
      {
        if (this._charactersOnLeftSide.Count != 0 || this._charactersOnMiddleSide.Count != 0 || this._charactersOnRightSide.Count != 0)
        {
          int num = Math.Sign(this._charactersOnRightSide.Count - this._charactersOnLeftSide.Count);
          if (num != this._bias)
          {
            this._bias = num;
            this.OnCharacterTouch(this._bias, this._maxCharacterLandVelocity);
          }
        }
        this._maxCharacterLandVelocity = 0.0;
      }

      protected override void OnStop()
      {
        base.OnStop();
        this._sol.FinishForever();
      }

      protected override void OnCollision(CollisionEvent e)
      {
        int classification = (int) e.ActiveObject.Type.Classification;
      }

      private List<ICharacter> GetStandingCharacters()
      {
        List<ICharacter> standingCharacters = new List<ICharacter>();
        foreach (ICharacter character in this.Level.ObjectManager.Characters)
        {
          if (character.GroundVector == this.CollisionVectors[0] && !character.IsAirborne)
            standingCharacters.Add(character);
        }
        return standingCharacters;
      }

      protected override void OnUpdateCollision()
      {
        this._charactersOnLeftSide.Clear();
        this._charactersOnRightSide.Clear();
        this._charactersOnMiddleSide.Clear();
        foreach (ICharacter standingCharacter in this.GetStandingCharacters())
        {
          if (standingCharacter.Velocity.Y >= 0.0 || !standingCharacter.IsAirborne)
          {
            Vector2i position = standingCharacter.Position;
            int x1 = position.X;
            position = this.Position;
            int num1 = position.X - 32 /*0x20*/;
            int num2;
            if (x1 < num1)
            {
              num2 = -1;
              this._charactersOnLeftSide.Add(standingCharacter);
            }
            else
            {
              position = standingCharacter.Position;
              int x2 = position.X;
              position = this.Position;
              int num3 = position.X + 32 /*0x20*/;
              if (x2 > num3)
              {
                num2 = 0;
                this._charactersOnRightSide.Add(standingCharacter);
              }
              else
              {
                num2 = 1;
                this._charactersOnMiddleSide.Add(standingCharacter);
              }
            }
            if (!this._sol.Flying && num2 != this._sol.Side)
              this._maxCharacterLandVelocity = Math.Max(this._maxCharacterLandVelocity, standingCharacter.Velocity.Y);
          }
        }
        if (this._bias < -1 || this._bias > 1)
          return;
        int[] numArray1 = new int[3]{ 80 /*0x50*/, 4, -64 };
        int[] numArray2 = new int[3]{ -64, 4, 80 /*0x50*/ };
        CollisionVector collisionVector = this.CollisionVectors[0];
        collisionVector.RelativeA = new Vector2i(-192, numArray1[this._bias + 1]);
        collisionVector.RelativeB = new Vector2i(192 /*0xC0*/, numArray2[this._bias + 1]);
        this.RegisterCollisionUpdate();
        foreach (ICharacter character in this.CharactersOnObject)
        {
          int y;
          if (this.GetYIntersectAt(character.Position.X, out y))
            character.Position = new Vector2i(character.Position.X, y - character.CollisionRadius.Y - 4);
        }
      }

      private bool GetYIntersectAt(int x, out int y)
      {
        Vector2 intersection;
        if (!CollisionVector.GetIntersection(this.CollisionVectors[0], new CollisionVector(new Vector2i(x, this.Position.Y - 512 /*0x0200*/), new Vector2i(x, this.Position.Y + 512 /*0x0200*/)), out intersection))
        {
          y = 0;
          return false;
        }
        y = (int) intersection.Y - 1;
        return true;
      }

      private void OnCharacterTouch(int side, double yVelocity)
      {
        if (this._sol.Flying || side == this._sol.Side)
          return;
        Vector2 vector2 = new Vector2(69.0 / 16.0, 32.375);
        if (side != 0)
        {
          vector2 = new Vector2(51.0 / 16.0, 43.75);
          if (yVelocity > 36.0)
            vector2 = new Vector2(2.5, 56.0);
        }
        if (this._sol.Side == 1)
          vector2.X *= -1.0;
        vector2.Y *= -1.0;
        this._sol.Velocity = vector2;
        this._sol.Side = this._sol.Side == 1 ? -1 : 1;
        this._sol.Flying = true;
      }

      private void OnSolTouch(int side, double yVelocity)
      {
        IEnumerable<ICharacter> characters = side == -1 ? this._charactersOnRightSide.Concat<ICharacter>((IEnumerable<ICharacter>) this._charactersOnMiddleSide) : this._charactersOnLeftSide.Concat<ICharacter>((IEnumerable<ICharacter>) this._charactersOnMiddleSide);
        bool flag = false;
        foreach (ICharacter character in characters)
        {
          character.LeaveGround();
          character.IsAirborne = true;
          character.Velocity = new Vector2(character.Velocity.X, -yVelocity * this._strengthMultiplier);
          character.TumbleAngle = 0.0;
          character.TumbleTurns = 1;
          character.GroundVelocity = 0.0;
          flag = true;
        }
        if (!flag)
          return;
        this.Level.SoundManager.PlaySound((IActiveObject) this, this.Type.GetAbsolutePath("SONICORCA/SOUND/SPRING"));
      }

      private void AnimateVisualAngle()
      {
        this._visualAngle = MathX.GoTowards(this._visualAngle, this.VisualAngles[this._bias + 1], Math.PI / 8.0);
      }

      protected override void OnAnimate()
      {
        this.AnimateVisualAngle();
        this._animationStand.Animate();
        this._animationPlatform.Animate();
      }

      protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
      {
        IObjectRenderer objectRenderer = renderer.GetObjectRenderer();
        using (objectRenderer.BeginMatixState())
        {
          if (this._visualAngle != 0.0)
            objectRenderer.ModelMatrix *= Matrix4.CreateRotationZ(this._visualAngle);
          objectRenderer.Render(this._animationPlatform);
        }
        objectRenderer.Render(this._animationStand);
      }

      private class Sol : Enemy
      {
        private const int AnimationSol = 1;
        private const double Gravity = 0.875;
        private static readonly IReadOnlyList<int> SolRestYOffsets = (IReadOnlyList<int>) new int[5]
        {
          -32,
          -112,
          -188,
          -112,
          -32
        };
        private AnimationInstance _animation;
        private bool _facingRight;

        public Vector2 Velocity { get; set; }

        public int Side { get; set; }

        public bool Flying { get; set; }

        public HTZSeeSawInstance SeeSaw => (HTZSeeSawInstance) this.ParentObject;

        protected override void OnStart()
        {
          base.OnStart();
          this.CollisionRectangles = new CollisionRectangle[1]
          {
            new CollisionRectangle((ActiveObject) this, 0, -36, -36, 72, 72)
          };
          this._animation = new AnimationInstance(this.ResourceTree, this.Type.GetAbsolutePath("SONICORCA/OBJECTS/SOL/ANIGROUP"), 1);
          this.ShadowInfo.MaxShadowOffset = new Vector2i(16 /*0x10*/, 16 /*0x10*/);
        }

        protected override void OnUpdate()
        {
          Vector2i position;
          if (this.Level.Player.Protagonist != null)
          {
            position = this.Level.Player.Protagonist.Position;
            int x1 = position.X;
            position = this.Position;
            int x2 = position.X;
            this._facingRight = x1 >= x2;
          }
          int num = this.SeeSaw._bias + 1;
          if (this.Side == 1)
            num += 2;
          position = this.SeeSaw.Position;
          int y = position.Y + 64 /*0x40*/ + HTZSeeSawInstance.Sol.SolRestYOffsets[num];
          if (!this.Flying)
          {
            position = this.Position;
            this.Position = new Vector2i(position.X, y);
          }
          else
          {
            this.PositionPrecise = this.PositionPrecise + this.Velocity;
            this.Velocity += new Vector2(0.0, 0.875);
            if (this.Velocity.Y > 0.0)
            {
              position = this.Position;
              if (position.Y >= y)
              {
                position = this.Position;
                this.Position = new Vector2i(position.X, y);
                this.Flying = false;
                this.SeeSaw._bias = this.Side;
                this.SeeSaw.OnSolTouch(this.Side, this.Velocity.Y);
              }
            }
          }
          this.SeeSaw._charactersOnLeftSide.Clear();
          this.SeeSaw._charactersOnMiddleSide.Clear();
          this.SeeSaw._charactersOnRightSide.Clear();
        }

        protected override void OnAnimate()
        {
          base.OnAnimate();
          this._animation.Animate();
        }

        protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
        {
          renderer.GetObjectRenderer().Render(this._animation, this._facingRight);
        }
      }
    }
}
