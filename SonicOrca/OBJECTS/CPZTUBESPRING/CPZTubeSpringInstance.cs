// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.CPZTUBESPRING.CPZTubeSpringInstance
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Collision;
using SonicOrca.Core.Objects;
using SonicOrca.Extensions;
using SonicOrca.Geometry;
using SonicOrca.Graphics;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace SONICORCA.OBJECTS.CPZTUBESPRING;

public class CPZTubeSpringInstance : ActiveObject
{
  private const int AnimationStationary = 0;
  private const int AnimationBounce = 1;
  private const int AnimationOpen = 2;
  private const int AnimationClose = 3;
  private AnimationInstance _animation;
  private Vector2 _bounceVelocity;

  public CPZTubeSpringInstance() => this.DesignBounds = new Rectanglei(-67, -66, 134, 132);

  protected override void OnStart()
  {
    this._animation = new AnimationInstance(this.ResourceTree, this.Type.GetAbsolutePath("/ANIGROUP"));
    this._bounceVelocity = new Vector2(0.0, -42.0);
    this.CollisionVectors = ((IEnumerable<CollisionVector>) CollisionVector.FromRectangle((ActiveObject) this, (Rectanglei) new Rectangle(-64.0, -64.0, 128.0, 64.0))).Take<CollisionVector>(3).ToArray<CollisionVector>();
    this.CollisionVectors[1].Id = 1;
    this.Priority = 1256;
  }

  protected override void OnUpdate()
  {
    bool flag = false;
    foreach (IActiveObject character in this.Level.ObjectManager.Characters)
    {
      Vector2 position = (Vector2) character.Position;
      if (position.X >= (double) (this.Position.X - 64 /*0x40*/) && position.X <= (double) (this.Position.X + 64 /*0x40*/) && position.Y >= (double) (this.Position.Y - 48 /*0x30*/) && position.Y <= (double) (this.Position.Y + 72))
      {
        flag = true;
        break;
      }
    }
    if (flag)
    {
      this._animation.Index = 2;
    }
    else
    {
      if (this._animation.Index != 2)
        return;
      this._animation.Index = 3;
    }
  }

  protected override void OnCollision(CollisionEvent e)
  {
    if (e.Id != 1 || e.ActiveObject.Type.Classification != ObjectClassification.Character || this._animation.Index == 3 || this._animation.Index == 2)
      return;
    ICharacter activeObject = (ICharacter) e.ActiveObject;
    activeObject.IsAirborne = true;
    activeObject.IsSpinball = false;
    activeObject.TumbleAngle = 0.0;
    activeObject.TumbleTurns = 1;
    activeObject.GroundVelocity = 1.0;
    ICharacter character = activeObject;
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
    this._animation.Index = 1;
    e.MaintainVelocity = true;
    this.Level.SoundManager.PlaySound((IActiveObject) this, this.Type.GetAbsolutePath("SONICORCA/SOUND/SPRING"));
  }

  protected override void OnAnimate() => this._animation.Animate();

  protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
  {
    renderer.GetObjectRenderer().Render(this._animation, new Vector2(0.0, -66.0));
  }
}
