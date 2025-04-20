// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.HTZVINELIFT.PILLAR.HTZVineLiftPillarInstance
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Objects.Metadata;
using SonicOrca.Extensions;
using SonicOrca.Geometry;
using SonicOrca.Graphics;

#nullable disable
namespace SONICORCA.OBJECTS.HTZVINELIFT.PILLAR;

public class HTZVineLiftPillarInstance : ActiveObject
{
  private const int TopAnimationIndex = 2;
  private const int BottomAnimationIndex = 3;
  private AnimationInstance _animation;
  private bool _flipX;
  private bool _bottom;

  [StateVariable]
  private bool FlipX
  {
    get => this._flipX;
    set => this._flipX = value;
  }

  [StateVariable]
  private bool Bottom
  {
    get => this._bottom;
    set => this._bottom = value;
  }

  public HTZVineLiftPillarInstance()
  {
    this.DesignBounds = new Rectanglei(-64, -170, 128 /*0x80*/, 340);
  }

  protected override void OnStart()
  {
    base.OnStart();
    this._animation = new AnimationInstance(this.ResourceTree, this.Type.GetAbsolutePath("//ANIGROUP"), this._bottom ? 3 : 2);
    this.Priority = -256;
  }

  protected override void OnAnimate() => this._animation.Animate();

  protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
  {
    renderer.GetObjectRenderer().Render(this._animation, this._flipX);
  }
}
