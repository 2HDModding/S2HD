// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.CPZBLOCK.CPZBlockInstance
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca;
using SonicOrca.Core;
using SonicOrca.Core.Collision;
using SonicOrca.Core.Objects.Base;
using SonicOrca.Core.Objects.Metadata;
using SonicOrca.Extensions;
using SonicOrca.Geometry;
using SonicOrca.Graphics;
using System;

namespace SONICORCA.OBJECTS.CPZBLOCK;

public class CPZBlockInstance : Platform
{
  private AnimationInstance _animation;
  private CPZBlockInstance.BlockType _blockType = CPZBlockInstance.BlockType.Single;
  private int _movementLength = 384;
  private bool _flipX;
  private CPZBlockInstance.TurnDirection _turnDirection = CPZBlockInstance.TurnDirection.Clockwise;
  private int _initialState;
  private Vector2i _state0position;
  private CPZBlockInstance[] _siblings = new CPZBlockInstance[0];
  private bool _activated;
  private int _ticks;

  [StateVariable]
  private CPZBlockInstance.BlockType BlockKind
  {
    get => this._blockType;
    set => this._blockType = value;
  }

  [StateVariable]
  private CPZBlockInstance.TurnDirection Direction
  {
    get => this._turnDirection;
    set => this._turnDirection = value;
  }

  [StateVariable]
  private int Length
  {
    get => this._movementLength;
    set => this._movementLength = value;
  }

  [StateVariable]
  private int InitialState
  {
    get => this._initialState;
    set => this._initialState = value;
  }

  [StateVariable]
  private bool FlipX
  {
    get => this._flipX;
    set => this._flipX = value;
  }

  protected override void OnStart()
  {
    base.OnStart();
    if (this._blockType == CPZBlockInstance.BlockType.Single)
    {
      this._state0position = this.Position - this.GetStatePosition(this._initialState);
      this.DesignBounds = new Rectanglei(-64, -64, 128 /*0x80*/, 128 /*0x80*/);
    }
    else
    {
      this._state0position = this.Position;
      this._siblings = new CPZBlockInstance[3];
      for (int index = 0; index < 3; ++index)
      {
        CPZBlockInstance cpzBlockInstance = this.Level.ObjectManager.AddSubObject<CPZBlockInstance>((ActiveObject) this);
        cpzBlockInstance._blockType = CPZBlockInstance.BlockType.None;
        cpzBlockInstance.Position = this.Position + new Vector2i((index + 1) * 128 /*0x80*/, 0);
        cpzBlockInstance.LockLifetime = true;
        this._siblings[index] = cpzBlockInstance;
      }
      if (this._flipX)
      {
        this.Move(384, 0);
        this._siblings[0].Move(128 /*0x80*/, 0);
        this._siblings[1].Move((int) sbyte.MinValue, 0);
        this._siblings[2].Move(-384, 0);
      }
      this.DesignBounds = new Rectanglei(-64, -64, 512 /*0x0200*/, 128 /*0x80*/);
    }
    this._animation = new AnimationInstance(this.ResourceTree, this.Type.GetAbsolutePath("/ANIGROUP"));
    this.CollisionVectors = CollisionVector.FromRectangle((ActiveObject) this, new Rectanglei(-64, -64, 128 /*0x80*/, 128 /*0x80*/));
    this.CollisionVectors[0].Id = 0;
    this.CollisionVectors[1].Id = 1;
    this.CollisionVectors[2].Id = 2;
    this.CollisionVectors[3].Id = 3;
    this.CollisionVectors[1].Flags |= CollisionFlags.Conveyor;
    for (int index = 0; index < 4; ++index)
    {
      this.CollisionVectors[index].Flags |= CollisionFlags.Solid;
      this.CollisionVectors[index].Flags |= CollisionFlags.NoBalance;
    }
  }

  protected override void OnUpdate()
  {
    if (this.ParentObject != null && this.ParentObject.Finished)
      this.Finish();
    else
      base.OnUpdate();
  }

  protected override void OnUpdateEditor()
  {
    if (this.ParentObject != null && this.ParentObject.Finished)
    {
      this.Finish();
    }
    else
    {
      if (this._siblings == null)
        return;
      for (int index = 0; index < this._siblings.Length; ++index)
        this._siblings[index].Position = this.Position + new Vector2i((index + 1) * 128 /*0x80*/, 0);
    }
  }

  protected override void OnUpdatePrepare()
  {
    if (this.ParentObject is CPZBlockInstance)
      return;
    Vector2 positionPrecise = this.PositionPrecise;
    Vector2[] vector2Array = new Vector2[this._siblings.Length];
    for (int index = 0; index < this._siblings.Length; ++index)
      vector2Array[index] = this._siblings[index].PositionPrecise;
    this.UpdateMovement();
    for (int index = 0; index < this._siblings.Length; ++index)
    {
      this._siblings[index]._nextPositionPrecise = this._siblings[index].PositionPrecise;
      this._siblings[index]._velocityBasedOnNextPosition = this._siblings[index]._nextPositionPrecise - vector2Array[index];
    }
    this._nextPositionPrecise = this.PositionPrecise;
    this._velocityBasedOnNextPosition = this._nextPositionPrecise - positionPrecise;
    this.PositionPrecise = positionPrecise;
  }

  protected override void UpdateMovement()
  {
    switch (this._blockType)
    {
      case CPZBlockInstance.BlockType.Single:
        this.UpdateMovementSingle();
        break;
      case CPZBlockInstance.BlockType.Stairs:
        this.UpdateMovementStairs();
        break;
    }
  }

  private Vector2i GetStatePosition(int state)
  {
    switch (state)
    {
      case 1:
        return this._state0position + new Vector2i(this._movementLength, 0);
      case 2:
        return this._state0position + new Vector2i(this._movementLength, this._movementLength);
      case 3:
        return this._state0position + new Vector2i(0, this._movementLength);
      default:
        return this._state0position + new Vector2i(0, 0);
    }
  }

  private void UpdateMovementSingle()
  {
    int num = this.Level.Ticks % 720 / 180;
    int state1;
    int state2;
    if (this._turnDirection == CPZBlockInstance.TurnDirection.Clockwise)
    {
      state1 = (this._initialState + num) % 4;
      state2 = (state1 + 1) % 4;
    }
    else
    {
      state1 = (this._initialState - num + 4) % 4;
      state2 = (state1 + 3) % 4;
    }
    Vector2i statePosition1 = this.GetStatePosition(state1);
    Vector2i statePosition2 = this.GetStatePosition(state2);
    double t = 0.5 - Math.Cos((double) (this.Level.Ticks % 180) / 180.0 * Math.PI) / 2.0;
    this.PositionPrecise = new Vector2(statePosition1.X == statePosition2.X ? (double) statePosition1.X : MathX.Lerp((double) statePosition1.X, (double) statePosition2.X, t), statePosition1.Y == statePosition2.Y ? (double) statePosition1.Y : MathX.Lerp((double) statePosition1.Y, (double) statePosition2.Y, t));
  }

  private void UpdateMovementStairs()
  {
    if (!this._activated)
      return;
    int num = this._turnDirection == CPZBlockInstance.TurnDirection.Up ? -1 : 1;
    if (this._ticks < 30)
    {
      ++this._ticks;
    }
    else
    {
      if (this._ticks > 158)
        return;
      double t = (double) (this._ticks - 30) / 128.0;
      this.PositionPrecise = this.PositionPrecise with
      {
        Y = MathX.Lerp((double) this._state0position.Y, (double) (this._state0position.Y + num * 128 /*0x80*/), t)
      };
      for (int index = 0; index < 3; ++index)
      {
        Vector2 positionPrecise = this._siblings[index].PositionPrecise with
        {
          Y = MathX.Lerp((double) this._state0position.Y, (double) (this._state0position.Y + num * ((index + 2) * 128 /*0x80*/)), t)
        };
        this._siblings[index].PositionPrecise = positionPrecise;
      }
      ++this._ticks;
    }
  }

  protected override void OnCollision(CollisionEvent e)
  {
    if (e.Id == 1 && e.ActiveObject.Type.Classification == ObjectClassification.Character)
    {
      this._activated = true;
      if (this.ParentObject is CPZBlockInstance parentObject)
        parentObject._activated = true;
    }
    base.OnCollision(e);
  }

  protected override void OnAnimate() => this._animation.Animate();

  protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
  {
    renderer.GetObjectRenderer().Render(this._animation);
  }

  private enum BlockType
  {
    None,
    Single,
    Stairs,
  }

  private enum TurnDirection
  {
    Up,
    Down,
    Clockwise,
    AntiClockwise,
  }
}
