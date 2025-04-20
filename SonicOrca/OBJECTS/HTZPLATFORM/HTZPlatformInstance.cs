// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.HTZPLATFORM.HTZPlatformInstance
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Collision;
using SonicOrca.Core.Objects.Base;
using SonicOrca.Extensions;
using SonicOrca.Geometry;
using SonicOrca.Graphics;

#nullable disable
namespace SONICORCA.OBJECTS.HTZPLATFORM;

public class HTZPlatformInstance : Platform
{
  private AnimationInstance _animation;

  public HTZPlatformInstance()
  {
    this.DesignBounds = new Rectanglei((int) sbyte.MinValue, -64, 256 /*0x0100*/, 128 /*0x80*/);
    this.SagWhenStoodOn = true;
  }

  protected override void OnStart()
  {
    base.OnStart();
    this._animation = new AnimationInstance(this.ResourceTree, this.Type.GetAbsolutePath("/ANIGROUP"));
    this.CollisionVectors = new CollisionVector[1]
    {
      new CollisionVector((ActiveObject) this, new Vector2i((int) sbyte.MinValue, -32), new Vector2i(128 /*0x80*/, -32), flags: CollisionFlags.Conveyor)
    };
  }

  protected override void OnAnimate() => this._animation.Animate();

  protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
  {
    renderer.GetObjectRenderer().Render(this._animation);
  }
}
