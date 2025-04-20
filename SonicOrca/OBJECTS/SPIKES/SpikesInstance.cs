// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.SPIKES.SpikesInstance
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
using System;

#nullable disable
namespace SONICORCA.OBJECTS.SPIKES;

public class SpikesInstance : ActiveObject
{
  private AnimationInstance _animation;
  private SpikesInstance.SpikesDirection _direction;
  private bool _moving;
  private Vector2i _velocity;
  private Vector2i _currentDisplacement;
  private Vector2i _targetDisplacement;
  private int _movementDelay;
  private bool _shouldBeAtTarget;

  [StateVariable]
  private SpikesInstance.SpikesDirection Direction
  {
    get => this._direction;
    set => this._direction = value;
  }

  [StateVariable]
  private bool Movement
  {
    get => this._moving;
    set => this._moving = value;
  }

  public SpikesInstance()
  {
    this.DesignBounds = new Rectanglei(-64, -64, 128 /*0x80*/, 128 /*0x80*/);
  }

  protected override void OnStart()
  {
    this._animation = new AnimationInstance(this.ResourceTree, this.Type.GetAbsolutePath("/ANIGROUP"));
    this.CollisionVectors = CollisionVector.FromRectangle((ActiveObject) this, (Rectanglei) new Rectangle(-64.0, -64.0, 128.0, 128.0));
    int index;
    switch (this._direction)
    {
      case SpikesInstance.SpikesDirection.Right:
        index = 2;
        this._targetDisplacement = new Vector2i(128 /*0x80*/, 0);
        this._velocity = new Vector2i(32 /*0x20*/, 0);
        break;
      case SpikesInstance.SpikesDirection.Down:
        index = 3;
        this._targetDisplacement = new Vector2i(0, (int) sbyte.MinValue);
        this._velocity = new Vector2i(0, -32);
        break;
      case SpikesInstance.SpikesDirection.Left:
        index = 0;
        this._targetDisplacement = new Vector2i((int) sbyte.MinValue, 0);
        this._velocity = new Vector2i(-32, 0);
        break;
      default:
        index = 1;
        this._targetDisplacement = new Vector2i(0, 128 /*0x80*/);
        this._velocity = new Vector2i(0, 32 /*0x20*/);
        break;
    }
    this.CollisionVectors[index].Id = 1;
    this.CollisionVectors[index].Flags |= CollisionFlags.Conveyor | CollisionFlags.Solid;
    this._movementDelay = 60;
    this._shouldBeAtTarget = true;
    this.Priority = -256;
  }

  protected override void OnUpdate()
  {
    if (!this._moving)
      return;
    --this._movementDelay;
    if (this._movementDelay <= 0)
    {
      this._movementDelay = 60;
      this._shouldBeAtTarget = !this._shouldBeAtTarget;
      this._velocity *= -1;
      this.Level.SoundManager.PlaySound((IActiveObject) this, this.Type.GetAbsolutePath("SONICORCA/SOUND/SPIKESLASH"));
    }
    if (this._shouldBeAtTarget)
    {
      if (!(this._currentDisplacement != this._targetDisplacement))
        return;
      this._currentDisplacement += this._velocity;
      this.Position = this.Position + this._velocity;
    }
    else
    {
      if (!((Vector2) this._currentDisplacement != new Vector2()))
        return;
      this._currentDisplacement += this._velocity;
      this.Position = this.Position + this._velocity;
    }
  }

  protected override void OnCollision(CollisionEvent e)
  {
    if (e.Id != 1 || e.ActiveObject.Type.Classification != ObjectClassification.Character)
      return;
    ICharacter activeObject = (ICharacter) e.ActiveObject;
    Vector2i position = activeObject.Position;
    int x1 = position.X;
    position = this.Position;
    int x2 = position.X;
    activeObject.Hurt(Math.Sign(x1 - x2), PlayerDeathCause.Spikes);
  }

  protected override void OnAnimate() => this._animation.Animate();

  protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
  {
    IObjectRenderer objectRenderer = renderer.GetObjectRenderer();
    using (objectRenderer.BeginMatixState())
    {
      if (this._direction != SpikesInstance.SpikesDirection.Up)
        objectRenderer.ModelMatrix *= Matrix4.CreateRotationZ(Math.PI / 2.0 * (double) this._direction);
      objectRenderer.Render(this._animation);
    }
  }

  private enum SpikesDirection
  {
    Up,
    Right,
    Down,
    Left,
  }
}
