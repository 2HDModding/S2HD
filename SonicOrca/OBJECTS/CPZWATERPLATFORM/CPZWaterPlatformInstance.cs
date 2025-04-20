// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.CPZWATERPLATFORM.CPZWaterPlatformInstance
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca;
using SonicOrca.Core;
using SonicOrca.Core.Collision;
using SonicOrca.Core.Objects.Base;
using SonicOrca.Extensions;
using SonicOrca.Geometry;
using SonicOrca.Graphics;
using System;

#nullable disable
namespace SONICORCA.OBJECTS.CPZWATERPLATFORM;

public class CPZWaterPlatformInstance : Platform
{
  private const int AnimationPlatform = 0;
  private const int AnimationPlatformGlow = 1;
  private const int AnimationWheelsMoveRight = 2;
  private const int AnimationWheelsMoveLeft = 3;
  private AnimationInstance _animationPlatform;
  private AnimationInstance _animationPlatformGlow;
  private AnimationInstance _animationWheel;
  private double _glow;

  public CPZWaterPlatformInstance()
  {
    this.DesignBounds = new Rectanglei(-132, -62, 265, 125);
    this.Linear = true;
  }

  protected override void OnStart()
  {
    base.OnStart();
    AnimationGroup loadedResource = this.ResourceTree.GetLoadedResource<AnimationGroup>(this.Type.GetAbsolutePath("/ANIGROUP"));
    this._animationPlatform = new AnimationInstance(loadedResource);
    this._animationPlatformGlow = new AnimationInstance(loadedResource, 1);
    this._animationWheel = new AnimationInstance(loadedResource, 3);
    this.CollisionVectors = new CollisionVector[1]
    {
      new CollisionVector((ActiveObject) this, new Vector2i(-96, -32), new Vector2i(96 /*0x60*/, -32), flags: CollisionFlags.Conveyor)
    };
  }

  protected override void OnAnimate()
  {
    int num = 120;
    this._glow = MathX.Clamp(0.0, (1.0 - Math.Cos(2.0 * Math.PI * (double) (this.Level.Ticks % num) / ((double) num - 1.0))) / 2.0, 1.0);
    this._animationPlatform.Animate();
    this._animationPlatformGlow.Animate();
    Vector2i vector2i = this.Position;
    int x1 = vector2i.X;
    vector2i = this.LastPosition;
    int x2 = vector2i.X;
    switch (Math.Sign(x1 - x2))
    {
      case -1:
        this._animationWheel.Index = 3;
        this._animationWheel.Animate();
        break;
      case 1:
        this._animationWheel.Index = 2;
        this._animationWheel.Animate();
        break;
    }
  }

  protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
  {
    IObjectRenderer objectRenderer = renderer.GetObjectRenderer();
    objectRenderer.Render(this._animationPlatform);
    objectRenderer.MultiplyColour = new Colour(this._glow, 1.0, 1.0, 1.0);
    objectRenderer.Render(this._animationPlatformGlow);
    objectRenderer.MultiplyColour = Colours.White;
    objectRenderer.Render(this._animationWheel, (Vector2) new Vector2i(-78, 0));
    objectRenderer.Render(this._animationWheel, (Vector2) new Vector2i(78, 0));
  }
}
