// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.CPZTRAPDOOR.CPZTrapDoorInstance
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Collision;
using SonicOrca.Core.Objects.Metadata;
using SonicOrca.Extensions;
using SonicOrca.Geometry;
using SonicOrca.Graphics;
using System;

#nullable disable
namespace SONICORCA.OBJECTS.CPZTRAPDOOR;

public class CPZTrapDoorInstance : ActiveObject
{
  private const int AnimationClosed = 0;
  private const int AnimationOpen = 1;
  private const int AnimationClosedToOpen = 2;
  private const int AnimationOpenToClosed = 3;
  private AnimationInstance _animation;
  private int _closeAnimationLength;
  private int _timeOffset;
  private bool _open;

  [StateVariable]
  private int TimeOffset
  {
    get => this._timeOffset;
    set => this._timeOffset = value;
  }

  public CPZTrapDoorInstance()
  {
    this.DesignBounds = new Rectanglei(-64, -64, 128 /*0x80*/, 128 /*0x80*/);
  }

  protected override void OnStart()
  {
    this._animation = new AnimationInstance(this.ResourceTree, this.Type.GetAbsolutePath("/ANIGROUP"));
    this._closeAnimationLength = this._animation.AnimationGroup[3].Duration;
    int num = (this.Level.Ticks + this._timeOffset) % 256 /*0x0100*/;
    this._open = num >= 128 /*0x80*/;
    if (num >= 256 /*0x0100*/ - this._closeAnimationLength)
    {
      this._animation.Index = 3;
      this._animation.Seek(num - (256 /*0x0100*/ - this._closeAnimationLength));
    }
    else if (num >= 128 /*0x80*/)
    {
      this._animation.Index = 2;
      this._animation.Seek(num - 128 /*0x80*/);
    }
    this.CollisionVectors = new CollisionVector[1]
    {
      new CollisionVector((ActiveObject) this, new Vector2i(-64, -64), new Vector2i(64 /*0x40*/, -64))
    };
  }

  protected override void OnUpdate()
  {
    int num = (this.Level.Ticks + this._timeOffset) % 256 /*0x0100*/;
    this._open = num >= 128 /*0x80*/;
    if (num == 128 /*0x80*/)
      this._animation.Index = 2;
    else if (num == 256 /*0x0100*/ - this._closeAnimationLength)
      this._animation.Index = 3;
    if (this._open)
    {
      foreach (CollisionVector collisionVector in this.CollisionVectors)
      {
        if (!collisionVector.Flags.HasFlag((Enum) CollisionFlags.Ignore))
          collisionVector.Flags |= CollisionFlags.Ignore;
      }
    }
    else
    {
      foreach (CollisionVector collisionVector in this.CollisionVectors)
      {
        if (collisionVector.Flags.HasFlag((Enum) CollisionFlags.Ignore))
          collisionVector.Flags &= ~CollisionFlags.Ignore;
      }
    }
  }

  protected override void OnCollision(CollisionEvent e) => base.OnCollision(e);

  protected override void OnAnimate() => this._animation.Animate();

  protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
  {
    renderer.GetObjectRenderer().Render(this._animation);
  }
}
