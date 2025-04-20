// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.HTZBREAKABLEPLATFORM.HTZBreakablePlatformInstance
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Collision;
using SonicOrca.Core.Objects;
using SonicOrca.Core.Objects.Base;
using SonicOrca.Core.Objects.Metadata;
using SonicOrca.Extensions;
using SonicOrca.Geometry;
using SonicOrca.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace SONICORCA.OBJECTS.HTZBREAKABLEPLATFORM;

public class HTZBreakablePlatformInstance : ActiveObject
{
  private static readonly IReadOnlyCollection<Vector2> ParticleVelocities = (IReadOnlyCollection<Vector2>) new Vector2[6]
  {
    new Vector2(-6.0, -14.0),
    new Vector2(-4.0, -18.0),
    new Vector2(-2.0, -22.0),
    new Vector2(2.0, -24.0),
    new Vector2(4.0, -16.0),
    new Vector2(6.0, -12.0)
  };
  private const int MaximumBlocks = 5;
  private AnimationInstance _animation;
  private bool _breakBlockOff;
  private Player _playerThatBroke;

  [StateVariable]
  private uint Paths { get; set; } = uint.MaxValue;

  [StateVariable]
  private int RemainingBlocks { get; set; } = 5;

  [StateVariable]
  private int ResetPath { get; set; } = -1;

  public HTZBreakablePlatformInstance()
  {
    this.DesignBounds = new Rectanglei(-64, -160, 128 /*0x80*/, 384);
  }

  protected override void OnStart()
  {
    this._animation = new AnimationInstance(this.ResourceTree, this.Type.GetAbsolutePath("/ANIGROUP"), 5 - this.RemainingBlocks);
    this.CollisionVectors = CollisionVector.FromRectangle((ActiveObject) this, new Rectanglei());
    this.CollisionVectors[1].Id = 1;
    this.CollisionVectors[1].Flags = CollisionFlags.NoAngle | CollisionFlags.NoBalance;
    this.UpdateBlockCollision();
  }

  protected override void OnUpdate()
  {
    if (this.RemainingBlocks == 0)
    {
      this.FinishForever();
    }
    else
    {
      if (this._breakBlockOff)
      {
        foreach (Vector2 particleVelocity in (IEnumerable<Vector2>) HTZBreakablePlatformInstance.ParticleVelocities)
          this.Level.ObjectManager.AddObject(new ObjectPlacement(this.Type.GetAbsolutePath("SONICORCA/OBJECTS/HTZROCK/PARTICLE"), this.Level.Map.Layers.IndexOf(this.Level.Map.Layers.Last<LevelLayer>()), this.Position, (object) new
          {
            Velocity = new Vector2(particleVelocity.X, particleVelocity.Y)
          }));
        this.Level.SoundManager.PlaySound((IActiveObject) this, this.Type.GetAbsolutePath("SONICORCA/SOUND/BREAKABLE"));
        this.Level.CreateScoreObject(this._playerThatBroke.AwardChainedScore(), this.Position);
        --this.RemainingBlocks;
        (this.Entry.State as HTZBreakablePlatformInstance).RemainingBlocks = this.RemainingBlocks;
        this._breakBlockOff = false;
        if (this.RemainingBlocks == 0)
        {
          this.FinishForever();
          this.CollisionVectors = new CollisionVector[0];
        }
        else
        {
          ++this._animation.Index;
          foreach (ICharacter standingCharacter in this.GetStandingCharacters())
            standingCharacter.LeaveGround();
        }
      }
      this.UpdateBlockCollision();
    }
  }

  private void UpdateBlockCollision()
  {
    if (this.RemainingBlocks <= 0 || this.CollisionVectors.Length == 0)
      return;
    int height = this.RemainingBlocks * 64 /*0x40*/;
    CollisionVector.UpdateRectangle(this.CollisionVectors, new Rectanglei(-64, 160 /*0xA0*/ - height, 128 /*0x80*/, height));
    this.RegisterCollisionUpdate();
  }

  private List<ICharacter> GetStandingCharacters()
  {
    List<ICharacter> standingCharacters = new List<ICharacter>();
    foreach (Character character in this.Level.ObjectManager.Characters)
    {
      if (character.GroundVector == this.CollisionVectors[1])
        standingCharacters.Add((ICharacter) character);
    }
    return standingCharacters;
  }

  protected override void OnCollision(CollisionEvent e)
  {
    if (e.Id != 1 || e.ActiveObject.Type.Classification != ObjectClassification.Character)
      return;
    ICharacter activeObject = (ICharacter) e.ActiveObject;
    if (this.RemainingBlocks == 5)
    {
      if (((long) this.Paths & (long) (1 << activeObject.Path)) == 0L)
        return;
      if (activeObject.IsSpinball)
      {
        this.BreakBlocks(e, activeObject);
      }
      else
      {
        if (this.ResetPath == -1)
          return;
        activeObject.Path = this.ResetPath;
        if (activeObject.Mode == CollisionMode.Top)
          return;
        activeObject.GroundVelocity = 0.0;
        activeObject.LeaveGround();
      }
    }
    else
    {
      if (!activeObject.IsSpinball)
        return;
      this.BreakBlocks(e, activeObject);
    }
  }

  private void BreakBlocks(CollisionEvent e, ICharacter character)
  {
    this._breakBlockOff = true;
    this._playerThatBroke = character.Player;
    character.LeaveGround();
    character.Velocity = new Vector2(character.Velocity.X, -1.0);
    e.MaintainVelocity = true;
  }

  protected override void OnAnimate() => this._animation.Animate();

  protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
  {
    renderer.GetObjectRenderer().Render(this._animation);
  }
}
