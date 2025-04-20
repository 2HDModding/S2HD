// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.SOL.SolInstance
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

#nullable disable
namespace SONICORCA.OBJECTS.SOL;

public class SolInstance : Badnik
{
  private const int AnimationSol = 0;
  private const int AnimationFireball = 2;
  private AnimationInstance _animation;
  private bool _waitingForCharacter;

  public SolInstance() => this.DesignBounds = new Rectanglei(-36, -36, 72, 72);

  protected override void OnStart()
  {
    base.OnStart();
    this._animation = new AnimationInstance(this.ResourceTree, this.Type.GetAbsolutePath("/ANIGROUP"));
    this.CollisionRectangles = new CollisionRectangle[1]
    {
      new CollisionRectangle((ActiveObject) this, 0, -36, -36, 72, 72)
    };
    this._waitingForCharacter = true;
    int num = 0;
    for (int index = 0; index < 4; ++index)
    {
      SolInstance.Fireball fireball = this.Level.ObjectManager.AddSubObject<SolInstance.Fireball>((ActiveObject) this);
      fireball.Angle = num;
      fireball.UpdateOrbitPosition();
      num += 64 /*0x40*/;
    }
  }

  protected override void OnStop()
  {
    base.OnStop();
    if (this._waitingForCharacter)
      return;
    this.FinishForever();
  }

  protected override void OnUpdate()
  {
    base.OnUpdate();
    this.Position = this.Position + new Vector2i(-1, 0);
    if (!this._waitingForCharacter || !this.IsCharacterCloseEnough(this.Level.Player.Protagonist))
      return;
    this._waitingForCharacter = false;
  }

  private bool IsCharacterCloseEnough(ICharacter character)
  {
    if (character.IsDebug)
      return false;
    Vector2i vector2i = character.Position - this.Position;
    vector2i.X = Math.Abs(vector2i.X);
    vector2i.Y = Math.Abs(vector2i.Y);
    return vector2i.X < 640 && vector2i.Y < 320;
  }

  protected override void OnAnimate() => this._animation.Animate();

  protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
  {
    renderer.GetObjectRenderer().Render(this._animation);
  }

  private class Fireball : Enemy
  {
    private AnimationInstance _animation;
    private bool _fired;

    public int Angle { get; set; }

    public SolInstance Sol => (SolInstance) this.ParentObject;

    protected override void OnStart()
    {
      base.OnStart();
      this._animation = new AnimationInstance(this.Sol._animation.AnimationGroup, 2);
      this.CollisionRectangles = new CollisionRectangle[1]
      {
        new CollisionRectangle((ActiveObject) this, 0, -8, -8, 16 /*0x10*/, 16 /*0x10*/)
      };
      this.LockLifetime = true;
    }

    protected override void OnUpdate()
    {
      base.OnUpdate();
      if (!this._fired)
      {
        if (this.Sol.Finished)
          this.Finish();
        else if (this.Sol._waitingForCharacter || this.Angle != 64 /*0x40*/)
        {
          this.Angle = MathX.Wrap(this.Angle + 1, 256 /*0x0100*/);
          this.UpdateOrbitPosition();
        }
        else
        {
          this._fired = true;
          this.LockLifetime = false;
        }
      }
      else
        this.PositionPrecise = this.PositionPrecise + new Vector2(-8.0, 0.0);
    }

    protected override void OnUpdateEditor()
    {
      base.OnUpdateEditor();
      this.UpdateOrbitPosition();
    }

    public void UpdateOrbitPosition()
    {
      double num = (double) this.Angle * (Math.PI / 128.0);
      this.PositionPrecise = this.Sol.PositionPrecise + new Vector2(64.0 * Math.Cos(num), 64.0 * Math.Sin(num));
    }

    protected override void OnAnimate() => this._animation.Animate();

    protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
    {
      IObjectRenderer objectRenderer = renderer.GetObjectRenderer();
      objectRenderer.EmitsLight = true;
      objectRenderer.Render(this._animation);
    }
  }
}
