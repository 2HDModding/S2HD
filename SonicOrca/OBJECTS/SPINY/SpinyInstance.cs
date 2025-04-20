// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.SPINY.SpinyInstance
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
using System;

#nullable disable
namespace SONICORCA.OBJECTS.SPINY;

public class SpinyInstance : Badnik
{
  private const int AnimationMovingLeft = 0;
  private const int AnimationMovingRight = 1;
  private const int AnimationShooting = 2;
  private const int AnimationPlasma = 3;
  private bool _wall;
  private AnimationInstance _animation;
  private Vector2 _velocity;
  private SpinyInstance.StatusType _status;
  private int _moveTimeLeft;
  private int _holdFireTimeLeft;

  [StateVariable]
  private bool Wall
  {
    get => this._wall;
    set => this._wall = value;
  }

  public SpinyInstance() => this.DesignBounds = new Rectanglei(-110, -63, 220, 126);

  protected override void OnStart()
  {
    base.OnStart();
    this._animation = new AnimationInstance(this.ResourceTree, this.Type.GetAbsolutePath("/ANIGROUP"));
    this._velocity = this._wall ? new Vector2(0.0, -1.0) : new Vector2(-1.0, 0.0);
    this._status = SpinyInstance.StatusType.Moving;
    this._moveTimeLeft = 128 /*0x80*/;
    this.CollisionRectangles = new CollisionRectangle[1]
    {
      new CollisionRectangle((ActiveObject) this, 0, -16, -16, 32 /*0x20*/, 32 /*0x20*/)
    };
  }

  protected override void OnUpdate()
  {
    base.OnUpdate();
    switch (this._status)
    {
      case SpinyInstance.StatusType.Moving:
        this.UpdateMove();
        break;
      case SpinyInstance.StatusType.Firing:
        this.UpdateFiring();
        break;
    }
  }

  private void UpdateMove()
  {
    if (this._holdFireTimeLeft > 0)
      --this._holdFireTimeLeft;
    else if (this.DetectPlayer())
    {
      this._holdFireTimeLeft = 40;
      this._status = SpinyInstance.StatusType.Firing;
      this._animation.Index = 2;
      return;
    }
    --this._moveTimeLeft;
    if (this._moveTimeLeft <= 0)
    {
      this._moveTimeLeft = 128 /*0x80*/;
      this._velocity.X *= -1.0;
      this._velocity.Y *= -1.0;
    }
    this.PositionPrecise = this.PositionPrecise + this._velocity;
    this._animation.Index = this._wall ? (this._velocity.Y >= 0.0 ? 0 : 1) : (this._velocity.X < 0.0 ? 0 : 1);
  }

  private void UpdateFiring()
  {
    --this._holdFireTimeLeft;
    if (this._holdFireTimeLeft < 0)
    {
      this._status = SpinyInstance.StatusType.Moving;
      this._holdFireTimeLeft = 64 /*0x40*/;
    }
    else
    {
      if (this._holdFireTimeLeft != 20)
        return;
      this.FireProjectile();
    }
  }

  private bool DetectPlayer()
  {
    foreach (ICharacter character in this.Level.ObjectManager.Characters)
    {
      if (character.ShouldReactToLevel)
      {
        double num = (double) (character.Position.Y - this.Position.Y);
        if (num >= -540.0 && num <= 280.0 && (double) Math.Abs(character.Position.X - this.Position.X) < 384.0)
          return true;
      }
    }
    return false;
  }

  private void FireProjectile()
  {
    if (this._wall)
    {
      this.Level.ObjectManager.AddSubObject<SpinyInstance.PlasmaSphere>((ActiveObject) this).Velocity = new Vector2(-12.0, 0.0);
    }
    else
    {
      int num1 = -1;
      double num2 = double.MaxValue;
      foreach (IActiveObject character in this.Level.ObjectManager.Characters)
      {
        Vector2i position = character.Position;
        int x1 = position.X;
        position = this.Position;
        int x2 = position.X;
        double num3 = (double) (x1 - x2);
        double num4 = Math.Abs(num3);
        if (num4 < num2)
        {
          num2 = num4;
          num1 = Math.Sign(num3);
        }
      }
      if (num1 == 0)
        num1 = -1;
      this.Level.ObjectManager.AddSubObject<SpinyInstance.PlasmaSphere>((ActiveObject) this).Velocity = new Vector2((double) (4 * Math.Sign(num1)), -12.0);
    }
  }

  protected override void OnAnimate() => this._animation.Animate();

  protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
  {
    IObjectRenderer objectRenderer = renderer.GetObjectRenderer();
    if (this._wall)
    {
      using (objectRenderer.BeginMatixState())
      {
        objectRenderer.ModelMatrix = objectRenderer.ModelMatrix.RotateZ(MathX.ToRadians(-90.0));
        objectRenderer.Render(this._animation);
      }
    }
    else
      objectRenderer.Render(this._animation);
  }

  private enum StatusType
  {
    Moving,
    Firing,
  }

  private class PlasmaSphere : Enemy
  {
    private AnimationInstance _animation;
    private int _ticks;

    public int Direction { get; set; }

    public Vector2 Velocity { get; set; }

    protected override void OnStart()
    {
      base.OnStart();
      this._animation = new AnimationInstance(this.ResourceTree, this.Type.GetAbsolutePath("/ANIGROUP"), 3);
      this.CollisionRectangles = new CollisionRectangle[1]
      {
        new CollisionRectangle((ActiveObject) this, 0, -8, -8, 16 /*0x10*/, 16 /*0x10*/)
      };
    }

    protected override void OnUpdate()
    {
      base.OnUpdate();
      this.Velocity += new Vector2(0.0, 0.5);
      this.PositionPrecise = this.PositionPrecise + this.Velocity;
      ++this._ticks;
    }

    protected override void OnAnimate() => this._animation.Animate();

    protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
    {
      if (this._ticks < 3)
        return;
      renderer.GetObjectRenderer().Render(this._animation);
    }
  }
}
