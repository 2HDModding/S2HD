// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.LAVA.LavaInstance
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Collision;
using SonicOrca.Core.Lighting;
using SonicOrca.Core.Objects;
using SonicOrca.Extensions;
using SonicOrca.Geometry;
using SonicOrca.Graphics;
using System;

namespace SONICORCA.OBJECTS.LAVA;

public class LavaInstance : ActiveObject
{
  private const int AnimationTop = 0;
  private const int AnimationMiddle = 1;
  private const int AnimationBottom = 2;
  private AnimationInstance _animationTop;
  private AnimationInstance _animationMiddle;
  private AnimationInstance _animationBottom;
  private Vector2i _size = new Vector2i(1, 1);
  private VectorLightSource _lightSource;

  public int Width
  {
    get => this._size.X;
    set => this._size.X = value;
  }

  public int Height
  {
    get => this._size.Y;
    set => this._size.Y = value;
  }

  private Rectanglei Bounds
  {
    get
    {
      return new Rectanglei(-this._size.X / 2 * 64 /*0x40*/, -this._size.Y / 2 * 64 /*0x40*/, this._size.X * 64 /*0x40*/, this._size.Y * 64 /*0x40*/);
    }
  }

  protected override void OnStart()
  {
    AnimationGroup loadedResource = this.ResourceTree.GetLoadedResource<AnimationGroup>(this.Type.GetAbsolutePath("/ANIGROUP"));
    this._animationTop = new AnimationInstance(loadedResource);
    this._animationMiddle = new AnimationInstance(loadedResource, 1);
    this._animationBottom = new AnimationInstance(loadedResource, 2);
    this.CollisionVectors = CollisionVector.FromRectangle((ActiveObject) this, new Rectanglei(-this._size.X / 2 * 64 /*0x40*/, -this._size.Y / 2 * 64 /*0x40*/, this._size.X * 64 /*0x40*/, this._size.Y * 64 /*0x40*/));
    this.CollisionVectors[1].Flags = CollisionFlags.Conveyor;
    this.Priority = 4096 /*0x1000*/;
    Rectanglei rectanglei = this.Bounds;
    this.DesignBounds = rectanglei;
    rectanglei = rectanglei.OffsetBy(this.Position);
    this._lightSource = new VectorLightSource(32 /*0x20*/, rectanglei.TopLeft, rectanglei.TopRight);
    this.Level.LightingManager.RegisterLightSource((ILightSource) this._lightSource);
  }

  protected override void OnStop()
  {
    this.Level.LightingManager.UnregisterLightSource((ILightSource) this._lightSource);
  }

  protected override void OnCollision(CollisionEvent e)
  {
    if (e.ActiveObject.Type.Classification != ObjectClassification.Character)
      return;
    ((ICharacter) e.ActiveObject).Hurt(-1);
  }

  protected override void OnUpdate()
  {
    Rectanglei rectanglei = this.Bounds.OffsetBy(this.Position);
    this._lightSource.A = rectanglei.TopLeft;
    this._lightSource.B = rectanglei.TopRight;
    if (this.Level.Ticks % 24 != 0)
      return;
    ParticleManager particleManager = this.Level.ParticleManager;
    Random random = particleManager.Random;
    int num1 = -this._size.X / 2 * 64 /*0x40*/;
    int num2 = -this._size.Y / 2 * 64 /*0x40*/;
    for (int index1 = 0; index1 < this._size.X; ++index1)
    {
      for (int index2 = 0; index2 < 1; ++index2)
      {
        Vector2i vector2i = this.Position + new Vector2i(num1 + index1 * 64 /*0x40*/ + 32 /*0x20*/, num2 + index2 * 64 /*0x40*/ + 32 /*0x20*/);
        particleManager.Add(new Particle()
        {
          Type = ParticleType.Heat,
          Layer = this.Layer,
          Position = (Vector2) (vector2i + new Vector2i(random.Next(-4, 8), random.Next(-4, 8))),
          Velocity = new Vector2(random.NextSignedDouble() * 1.0, random.NextDouble() * -1.4 - 0.8),
          Time = random.Next(60, 120)
        });
      }
    }
  }

  protected override void OnAnimate()
  {
    this._animationTop.Animate();
    this._animationMiddle.Animate();
    this._animationBottom.Animate();
  }

  protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
  {
    if (viewOptions.Shadows)
      return;
    IObjectRenderer objectRenderer = renderer.GetObjectRenderer();
    objectRenderer.EmitsLight = true;
    int num1 = -this._size.X / 2 * 64 /*0x40*/;
    int y1 = -this._size.Y / 2 * 64 /*0x40*/;
    int val2_1 = (this._size.X + 1) / 2 * 64 /*0x40*/;
    int val2_2 = (this._size.Y + 1) / 2 * 64 /*0x40*/;
    Rectanglei source1 = this._animationTop.CurrentFrame.Source;
    Vector2 scale;
    for (int x1 = num1; x1 < val2_1; x1 += source1.Width)
    {
      Rectangle destination = new Rectangle((double) x1, (double) (y1 - source1.Height), (double) source1.Width, (double) source1.Height);
      destination.Right = Math.Min(destination.Right, (double) val2_1);
      destination.Bottom = Math.Min(destination.Bottom, (double) val2_2);
      Rectangle source2 = (Rectangle) this._animationTop.CurrentFrame.Source with
      {
        Width = destination.Width,
        Height = destination.Height
      };
      objectRenderer.Texture = this._animationTop.CurrentTexture;
      objectRenderer.BlendMode = BlendMode.Additive;
      ref Rectangle local1 = ref destination;
      double x2 = local1.X;
      scale = objectRenderer.Scale;
      double x3 = scale.X;
      local1.X = x2 * x3;
      ref Rectangle local2 = ref destination;
      double y2 = local2.Y;
      scale = objectRenderer.Scale;
      double y3 = scale.Y;
      local2.Y = y2 * y3;
      ref Rectangle local3 = ref destination;
      double width = local3.Width;
      scale = objectRenderer.Scale;
      double x4 = scale.X;
      local3.Width = width * x4;
      ref Rectangle local4 = ref destination;
      double height = local4.Height;
      scale = objectRenderer.Scale;
      double y4 = scale.Y;
      local4.Height = height * y4;
      objectRenderer.Render(source2, destination);
    }
    objectRenderer.BlendMode = BlendMode.Alpha;
    int num2 = y1;
    Animation.Frame currentFrame = this._animationMiddle.CurrentFrame;
    Rectanglei source3 = currentFrame.Source;
    for (int x5 = num1; x5 < val2_1; x5 += source3.Width)
    {
      Rectangle destination = new Rectangle((double) x5, (double) y1, (double) source3.Width, (double) source3.Height);
      destination.Right = Math.Min(destination.Right, (double) val2_1);
      destination.Bottom = Math.Min(destination.Bottom, (double) val2_2);
      currentFrame = this._animationMiddle.CurrentFrame;
      Rectangle source4 = (Rectangle) currentFrame.Source with
      {
        Width = destination.Width,
        Height = destination.Height
      };
      num2 = (int) destination.Bottom;
      ref Rectangle local5 = ref destination;
      double x6 = local5.X;
      scale = objectRenderer.Scale;
      double x7 = scale.X;
      local5.X = x6 * x7;
      ref Rectangle local6 = ref destination;
      double y5 = local6.Y;
      scale = objectRenderer.Scale;
      double y6 = scale.Y;
      local6.Y = y5 * y6;
      ref Rectangle local7 = ref destination;
      double width = local7.Width;
      scale = objectRenderer.Scale;
      double x8 = scale.X;
      local7.Width = width * x8;
      ref Rectangle local8 = ref destination;
      double height = local8.Height;
      scale = objectRenderer.Scale;
      double y7 = scale.Y;
      local8.Height = height * y7;
      objectRenderer.Texture = this._animationMiddle.CurrentTexture;
      objectRenderer.Render(source4, destination);
    }
    if (num2 >= val2_2)
      return;
    currentFrame = this._animationBottom.CurrentFrame;
    source3 = currentFrame.Source;
    for (int y8 = num2; y8 < val2_2; y8 += source3.Height)
    {
      for (int x9 = num1; x9 < val2_1; x9 += source3.Width)
      {
        Rectangle destination = new Rectangle((double) x9, (double) y8, (double) source3.Width, (double) source3.Height);
        destination.Right = Math.Min(destination.Right, (double) val2_1);
        destination.Bottom = Math.Min(destination.Bottom, (double) val2_2);
        currentFrame = this._animationBottom.CurrentFrame;
        Rectangle source5 = (Rectangle) currentFrame.Source with
        {
          Width = destination.Width,
          Height = destination.Height
        };
        ref Rectangle local9 = ref destination;
        double x10 = local9.X;
        scale = objectRenderer.Scale;
        double x11 = scale.X;
        local9.X = x10 * x11;
        ref Rectangle local10 = ref destination;
        double y9 = local10.Y;
        scale = objectRenderer.Scale;
        double y10 = scale.Y;
        local10.Y = y9 * y10;
        ref Rectangle local11 = ref destination;
        double width = local11.Width;
        scale = objectRenderer.Scale;
        double x12 = scale.X;
        local11.Width = width * x12;
        ref Rectangle local12 = ref destination;
        double height = local12.Height;
        scale = objectRenderer.Scale;
        double y11 = scale.Y;
        local12.Height = height * y11;
        objectRenderer.Texture = this._animationBottom.CurrentTexture;
        objectRenderer.Render(source5, destination);
      }
    }
  }
}
