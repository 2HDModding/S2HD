// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.HTZROCK.HTZRockInstance
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

namespace SONICORCA.OBJECTS.HTZROCK;

public class HTZRockInstance : ActiveObject
{
  private const int AnimationRock = 0;
  private static readonly IReadOnlyCollection<Vector2> ParticleVelocities = (IReadOnlyCollection<Vector2>) new Vector2[6]
  {
    new Vector2(-8.0, -10.0),
    new Vector2(-6.0, -8.0),
    new Vector2(-2.0, -10.0),
    new Vector2(2.0, -12.0),
    new Vector2(6.0, -8.0),
    new Vector2(8.0, -10.0)
  };
  private AnimationInstance _animation;
  private bool _broken;
  private Player _playerThatBroke;

  public HTZRockInstance()
  {
    this.DesignBounds = new Rectanglei(-96, -64, 192 /*0xC0*/, 128 /*0x80*/);
  }

  protected override void OnStart()
  {
    this._animation = new AnimationInstance(this.ResourceTree, this.Type.GetAbsolutePath("/ANIGROUP"));
    this.CollisionVectors = CollisionVector.FromRectangle((ActiveObject) this, new Rectanglei(-96, -64, 192 /*0xC0*/, 128 /*0x80*/));
    this.CollisionVectors[1].Id = 1;
    this.CollisionVectors[3].Id = 1;
    this.Priority = -256;
  }

  protected override void OnUpdate()
  {
    if (!this._broken)
      return;
    foreach (Vector2 particleVelocity in (IEnumerable<Vector2>) HTZRockInstance.ParticleVelocities)
      this.Level.ObjectManager.AddObject(new ObjectPlacement(this.Type.GetAbsolutePath("/PARTICLE"), this.Level.Map.Layers.IndexOf(this.Level.Map.Layers.Last<LevelLayer>()), this.Position, (object) new
      {
        Velocity = new Vector2(particleVelocity.X, particleVelocity.Y)
      }));
    this.Level.SoundManager.PlaySound((IActiveObject) this, this.Type.GetAbsolutePath("SONICORCA/SOUND/BREAKABLE"));
    this.Level.CreateScoreObject(this._playerThatBroke.AwardChainedScore(), this.Position);
    this.Finish();
  }

  protected override void OnCollision(CollisionEvent e)
  {
    if (e.Id != 1 || e.ActiveObject.Type.Classification != ObjectClassification.Character)
      return;
    ICharacter activeObject = (ICharacter) e.ActiveObject;
    if (!activeObject.IsSpinball)
      return;
    this._broken = true;
    this._playerThatBroke = activeObject.Player;
    if (activeObject.Velocity.Y > 0.0)
      activeObject.Velocity = new Vector2(activeObject.Velocity.X, -12.0);
    e.MaintainVelocity = true;
  }

  protected override void OnAnimate() => this._animation.Animate();

  protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
  {
    renderer.GetObjectRenderer().Render(this._animation);
  }
}
