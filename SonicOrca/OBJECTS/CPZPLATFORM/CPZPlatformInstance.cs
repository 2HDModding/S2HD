// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.CPZPLATFORM.CPZPlatformInstance
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

#nullable disable
namespace SONICORCA.OBJECTS.CPZPLATFORM;

public class CPZPlatformInstance : Platform
{
  private const int AnimationBase = 0;
  private const int AnimationStripGlow = 1;
  private const int AnimationBarometerStill = 2;
  private const int AnimationBarometerTwitching = 3;
  private const int AnimationBarometerStillToTwitching = 4;
  private const int AnimationBarometerTwitchingToStill = 5;
  private const int AnimationLiquid = 6;
  private const int AnimationSmallBase = 7;
  private const int AnimationSmallStripGlow = 8;
  private const int AnimationSmallStripGlowBlue = 9;
  private const int AnimationSmallStripGlowGreen = 10;
  private const int AnimationSmallLiquid = 11;
  private AnimationInstance _animationBase;
  private AnimationInstance _animationStripGlow;
  private AnimationInstance _animationStripGlowBlue;
  private AnimationInstance _animationStripGlowGreen;
  private AnimationInstance _animationStripBarometerA;
  private AnimationInstance _animationStripBarometerB;
  private AnimationInstance _animationLiquid;
  private CPZPlatformInstance.PlatformSize _size;
  private int _lastY;
  private bool _rising;
  private double _glowOpacity;
  private double _blueGreenBalance;
  private int _switchBlueGreenBalanceTimePeriod;
  private EaseTimeline _switchBlueGreenBalanceEaseTimeline;

  [StateVariable]
  private CPZPlatformInstance.PlatformSize Size
  {
    get => this._size;
    set => this._size = value;
  }

  public CPZPlatformInstance() => this.DesignBounds = new Rectanglei(-130, -74, 260, 148);

  protected override void OnStart()
  {
    base.OnStart();
    AnimationGroup loadedResource = this.ResourceTree.GetLoadedResource<AnimationGroup>(this.Type.GetAbsolutePath("/ANIGROUP"));
    if (this.Size == CPZPlatformInstance.PlatformSize.Large)
    {
      this._animationBase = new AnimationInstance(loadedResource);
      this._animationStripGlow = new AnimationInstance(loadedResource, 1);
      this._animationStripBarometerA = new AnimationInstance(loadedResource, 3);
      this._animationStripBarometerB = new AnimationInstance(loadedResource, 2);
      this._animationLiquid = new AnimationInstance(loadedResource, 6);
    }
    else
    {
      this._animationBase = new AnimationInstance(loadedResource, 7);
      this._animationStripGlow = new AnimationInstance(loadedResource, 8);
      this._animationStripGlowBlue = new AnimationInstance(loadedResource, 9);
      this._animationStripGlowGreen = new AnimationInstance(loadedResource, 10);
      this._animationStripBarometerA = new AnimationInstance(loadedResource, 3);
      this._animationStripBarometerB = new AnimationInstance(loadedResource, 2);
      this._animationLiquid = new AnimationInstance(loadedResource, 11);
    }
    int x = this.Size == CPZPlatformInstance.PlatformSize.Small ? 96 /*0x60*/ : 128 /*0x80*/;
    this.CollisionVectors = new CollisionVector[1]
    {
      new CollisionVector((ActiveObject) this, new Vector2i(-x, -64), new Vector2i(x, -64), flags: CollisionFlags.Conveyor | CollisionFlags.Solid)
    };
  }

  protected override void OnAnimate()
  {
    if (this.TimePeriod != this._switchBlueGreenBalanceTimePeriod)
    {
      this._switchBlueGreenBalanceTimePeriod = this.TimePeriod;
      this._switchBlueGreenBalanceEaseTimeline = new EaseTimeline(new EaseTimeline.Entry[4]
      {
        new EaseTimeline.Entry(this.TimePeriod / 4 - 8, 1.0),
        new EaseTimeline.Entry(this.TimePeriod / 4 + 8, -1.0),
        new EaseTimeline.Entry(this.TimePeriod * 3 / 4 - 8, -1.0),
        new EaseTimeline.Entry(this.TimePeriod * 3 / 4 + 8, 1.0)
      });
    }
    this._blueGreenBalance = this.TimePeriod != 0 ? this._switchBlueGreenBalanceEaseTimeline.GetValueAt(this.CurrentTime) : 0.0;
    this._glowOpacity = MathX.GoTowards(this._glowOpacity, this.IsCharacterInteractingWithPlatform() ? 1.0 : 0.0, 1.0 / 30.0);
    bool rising = this._rising;
    Vector2i position = this.Position;
    if (position.Y != this._lastY)
    {
      position = this.Position;
      this._rising = position.Y <= this._lastY;
    }
    position = this.Position;
    this._lastY = position.Y;
    if (!rising && this._rising)
      this._animationStripBarometerB.Index = 4;
    else if (rising && !this._rising)
      this._animationStripBarometerB.Index = 5;
    this._animationBase.Animate();
    this._animationStripGlow.Animate();
    this._animationStripBarometerA.Animate();
    this._animationStripBarometerB.Animate();
    this._animationLiquid.Animate();
  }

  protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
  {
    IObjectRenderer objectRenderer = renderer.GetObjectRenderer();
    if (this.Size == CPZPlatformInstance.PlatformSize.Large)
    {
      objectRenderer.Render(this._animationBase);
      if (this._glowOpacity > 0.0)
      {
        objectRenderer.MultiplyColour = new Colour(this._glowOpacity, 1.0, 1.0, 1.0);
        objectRenderer.Render(this._animationStripGlow);
        objectRenderer.MultiplyColour = Colours.White;
      }
      objectRenderer.Render(this._animationStripBarometerA, new Vector2(-15.0, -14.0), true);
      objectRenderer.Render(this._animationStripBarometerB, new Vector2(15.0, -14.0));
      objectRenderer.Render(this._animationLiquid);
    }
    else
    {
      objectRenderer.Render(this._animationBase);
      double a1 = (this._blueGreenBalance + 1.0) / 2.0;
      double a2 = 1.0 - a1;
      if (a2 > 0.0)
      {
        objectRenderer.MultiplyColour = new Colour(a2, 1.0, 1.0, 1.0);
        objectRenderer.Render(this._animationStripGlowBlue);
      }
      if (a1 > 0.0)
      {
        objectRenderer.MultiplyColour = new Colour(a1, 1.0, 1.0, 1.0);
        objectRenderer.Render(this._animationStripGlowGreen);
      }
      if (this._glowOpacity > 0.0)
      {
        objectRenderer.MultiplyColour = new Colour(this._glowOpacity, 1.0, 1.0, 1.0);
        objectRenderer.Render(this._animationStripGlow);
      }
      objectRenderer.MultiplyColour = Colours.White;
      objectRenderer.Render(this._animationStripBarometerA, new Vector2(-15.0, -14.0), true);
      objectRenderer.Render(this._animationStripBarometerB, new Vector2(15.0, -14.0));
      objectRenderer.Render(this._animationLiquid);
    }
  }

  private enum PlatformSize
  {
    Small,
    Large,
  }
}
