// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.CPZSPEEDBOOSTER.CPZSpeedBoosterInstance
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

namespace SONICORCA.OBJECTS.CPZSPEEDBOOSTER;

public class CPZSpeedBoosterInstance : ActiveObject
{
  private const int AnimationFrame = 0;
  private const int AnimationCore = 1;
  private const int AnimationArrows = 2;
  private const int AnimationSpinner = 3;
  private const int AnimationSmallWheel = 4;
  private AnimationInstance _animationFrame;
  private AnimationInstance _animationCore;
  private AnimationInstance _animationArrows;
  private AnimationInstance _animationSpinner;
  private AnimationInstance _animationSmallWheel;
  private int _strength = 128 /*0x80*/;

  [StateVariable]
  private int Strength
  {
    get => this._strength;
    set => this._strength = value;
  }

  protected override void OnStart()
  {
    AnimationGroup loadedResource = this.ResourceTree.GetLoadedResource<AnimationGroup>(this.Type.GetAbsolutePath("/ANIGROUP"));
    this._animationFrame = new AnimationInstance(loadedResource);
    this._animationCore = new AnimationInstance(loadedResource, 1);
    this._animationArrows = new AnimationInstance(loadedResource, 2);
    this._animationSpinner = new AnimationInstance(loadedResource, 3);
    this._animationSmallWheel = new AnimationInstance(loadedResource, 4);
    this.CollisionRectangles = new CollisionRectangle[1]
    {
      new CollisionRectangle((ActiveObject) this, 0, -64, -64, 128 /*0x80*/, 32 /*0x20*/)
    };
    this.DesignBounds = new Rectanglei(-158, -188, 316, 376);
    this.Priority = 1512;
    this.Level.ObjectManager.AddSubObject<CPZSpeedBoosterInstance.BackWheel>((ActiveObject) this);
  }

  protected override void OnCollision(CollisionEvent e)
  {
    ICharacter activeObject = (ICharacter) e.ActiveObject;
    if (activeObject.IsAirborne || this._strength == 0)
      return;
    if (activeObject.GroundVelocity < (double) Math.Abs(this._strength) || Math.Sign(activeObject.GroundVelocity) != Math.Sign(this._strength))
    {
      activeObject.GroundVelocity = (double) this._strength;
      activeObject.SlopeLockTicks = 15;
      activeObject.Facing = Math.Sign(this._strength);
      bool flag = false;
      double x = this.Level.Camera.Velocity.X;
      if (this._strength < 0)
        flag = x >= 0.0;
      else if (this._strength > 0)
        flag = x <= 0.0;
      if (flag)
        activeObject.CameraProperties.Delay = new Vector2i(16 /*0x10*/, this.CameraProperties.Delay.Y);
    }
    this.Level.SoundManager.PlaySound((IActiveObject) this, "SONICORCA/SOUND/SPRING");
  }

  protected override void OnAnimate()
  {
    if (this._strength == 0)
      return;
    this._animationFrame.Animate();
    this._animationCore.Animate();
    this._animationArrows.Animate();
    this._animationSpinner.Animate();
    this._animationSmallWheel.Animate();
  }

  protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
  {
    if (this._strength == 0)
      return;
    bool flipX = this._strength < 0;
    IObjectRenderer objectRenderer = renderer.GetObjectRenderer();
    objectRenderer.Render(this._animationFrame, new Vector2(0.0, 0.0));
    objectRenderer.Render(this._animationCore, new Vector2(0.0, 0.0));
    objectRenderer.Render(this._animationArrows, new Vector2(0.0, 0.0), flipX);
    objectRenderer.Render(this._animationSpinner, new Vector2(64.0, 0.0), !flipX);
    objectRenderer.Render(this._animationSmallWheel, new Vector2(-112.0, 0.0), flipX);
    objectRenderer.Render(this._animationSmallWheel, new Vector2(112.0, 0.0), flipX);
  }

  private class BackWheel : ActiveObject
  {
    protected override void OnUpdate()
    {
      if (this.ParentObject is CPZSpeedBoosterInstance parentObject && !parentObject.Finished)
        return;
      this.Finish();
    }

    protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
    {
      if (!(this.ParentObject is CPZSpeedBoosterInstance parentObject) || parentObject._strength == 0)
        return;
      bool flipX = parentObject._strength < 0;
      renderer.GetObjectRenderer().Render(parentObject._animationSpinner, new Vector2(-64.0, 0.0), flipX);
    }
  }
}
