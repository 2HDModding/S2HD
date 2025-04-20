// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.CPZDOOR.CPZDoorInstance
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
using System.Linq;

namespace SONICORCA.OBJECTS.CPZDOOR;

public class CPZDoorInstance : ActiveObject
{
  private const int BarrierMoveSpeed = 32 /*0x20*/;
  private AnimationInstance _animation;
  private Vector2i _initialPosition;
  private int _barrierOffset;
  private bool _closed;
  private int _direction = 1;

  [StateVariable]
  private int Direction
  {
    get => this._direction;
    set => this._direction = value;
  }

  public CPZDoorInstance()
  {
    this.DesignBounds = new Rectanglei(-32, (int) sbyte.MinValue, 64 /*0x40*/, 256 /*0x0100*/);
  }

  protected override void OnStart()
  {
    this._animation = new AnimationInstance(this.ResourceTree, this.Type.GetAbsolutePath("/ANIGROUP"));
    this._initialPosition = this.Position;
    this.CollisionVectors = CollisionVector.FromRectangle((ActiveObject) this, new Rectanglei(-32, (int) sbyte.MinValue, 64 /*0x40*/, 256 /*0x0100*/));
    foreach (CollisionVector collisionVector in this.CollisionVectors)
      collisionVector.Flags |= CollisionFlags.Solid;
    this.Priority = (int) sbyte.MinValue;
  }

  protected override void OnUpdate()
  {
    this._closed = this._barrierOffset == 0;
    this._barrierOffset = this.Level.ObjectManager.Characters.Any<ICharacter>((Func<ICharacter, bool>) (x => this.CheckCharacter(x))) ? Math.Max(this._barrierOffset - 32 /*0x20*/, -256) : Math.Min(this._barrierOffset + 32 /*0x20*/, 0);
    this.Position = this._initialPosition + new Vector2i(0, this._barrierOffset);
  }

  private bool CheckCharacter(ICharacter character)
  {
    if (!character.ShouldReactToLevel)
      return false;
    int num1 = this._closed ? 0 : character.CollisionRadius.X + 32 /*0x20*/ + 4;
    int num2 = 1080;
    return (this._direction != -1 ? Rectanglei.FromLTRB(this._initialPosition.X - 2048 /*0x0800*/, this._initialPosition.Y - num2, this._initialPosition.X + num1, this._initialPosition.Y + num2) : Rectanglei.FromLTRB(this._initialPosition.X - num1, this._initialPosition.Y - num2, this._initialPosition.X + 2048 /*0x0800*/, this._initialPosition.Y + num2)).Contains(character.Position);
  }

  protected override void OnAnimate() => this._animation.Animate();

  protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
  {
    renderer.GetObjectRenderer().Render(this._animation);
  }
}
