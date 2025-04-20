// Decompiled with JetBrains decompiler
// Type: S2HD.Title.Background
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca;
using SonicOrca.Extensions;
using SonicOrca.Geometry;
using SonicOrca.Graphics;
using SonicOrca.Resources;
using System;

namespace S2HD.Title;

internal class Background
{
  private const int BackgroundSkyStartX = 1260;
  private const int BackgroundIslandStartX = 1088;
  private const double BackgroundSkyVelocity = -0.05;
  private const double BackgroundIslandVelocity = -0.1;
  private readonly S2HDSonicOrcaGameContext _gameContext;
  [ResourcePath("SONICORCA/TITLE/BACKGROUND/SKY")]
  private ITexture _textureBackgroundSky;
  [ResourcePath("SONICORCA/TITLE/BACKGROUND/ISLAND")]
  private ITexture _textureBackgroundIsland;
  [ResourcePath("SONICORCA/TITLE/BACKGROUND/DEATHEGG")]
  private ITexture _textureBackgroundDeathEgg;
  [ResourcePath("SONICORCA/TITLE/WIPE")]
  private ITexture _textureWipe;
  [ResourcePath("SONICORCA/TITLE/ANIGROUP")]
  private AnimationGroup _animationGroup;
  private AnimationInstance _waterSparkleAnimationInstance;
  private double _backgroundFlash;
  private double _backgroundSkyCentreX;
  private double _backgroundIslandCentreX;
  private int _ticks;
  private int _wipeHeight;
  private bool _wipeTransitionActive;

  public bool Visible { get; set; }

  public Background(S2HDSonicOrcaGameContext gameContext)
  {
    this._gameContext = gameContext;
    this._gameContext.ResourceTree.FullfillLoadedResourcesByAttribute((object) this);
  }

  public void Reset()
  {
    this._ticks = 0;
    this._backgroundFlash = 0.0;
    this._backgroundSkyCentreX = 1260.0;
    this._backgroundIslandCentreX = 1088.0;
    this._waterSparkleAnimationInstance = (AnimationInstance) null;
    this.Visible = false;
  }

  public void WipeOut() => this._wipeTransitionActive = true;

  public void Update()
  {
    if (this._ticks == 0)
    {
      this.Visible = true;
      this._backgroundFlash = 1.0;
      this._waterSparkleAnimationInstance = new AnimationInstance(this._animationGroup, 10);
      this._waterSparkleAnimationInstance.AdditiveBlending = true;
    }
    else
      this._backgroundFlash = Math.Max(0.0, this._backgroundFlash - 1.0 / 32.0);
    this._backgroundSkyCentreX += -0.05;
    this._backgroundIslandCentreX += -0.1;
    if (this._backgroundSkyCentreX + (double) (this._textureBackgroundSky.Width / 2) < 0.0)
      this._backgroundSkyCentreX = (double) (this._textureBackgroundSky.Width / 2);
    if (this._backgroundIslandCentreX < (double) -this._textureBackgroundIsland.Width)
      this._backgroundIslandCentreX = (double) (1920 + this._textureBackgroundIsland.Width);
    this._waterSparkleAnimationInstance.Animate();
    if (this._wipeTransitionActive)
    {
      this._wipeHeight += 20;
      if (this._wipeHeight >= 600)
        this._wipeTransitionActive = false;
    }
    ++this._ticks;
  }

  public void Draw(Renderer renderer)
  {
    I2dRenderer renderer1 = renderer.Get2dRenderer();
    renderer1.BlendMode = BlendMode.Alpha;
    if (this.Visible)
    {
      renderer1.Colour = Colours.White;
      int backgroundSkyCentreX = (int) this._backgroundSkyCentreX;
      do
      {
        renderer1.RenderTexture(this._textureBackgroundSky, (Vector2) new Vector2i(backgroundSkyCentreX, 540));
        backgroundSkyCentreX += this._textureBackgroundSky.Width;
      }
      while (backgroundSkyCentreX - this._textureBackgroundSky.Width / 2 < 1920);
      renderer1.RenderTexture(this._textureBackgroundDeathEgg, new Vector2(1750.0, 192.0));
      renderer1.RenderTexture(this._textureBackgroundIsland, new Vector2(this._backgroundIslandCentreX, 540.0));
      this._waterSparkleAnimationInstance.Draw(renderer1, new Vector2(this._backgroundIslandCentreX + 38.0, 892.0));
    }
    if (this._backgroundFlash > 0.0)
      renderer1.RenderQuad(new Colour(this._backgroundFlash, 1.0, 1.0, 1.0), new Rectangle(0.0, 0.0, 1920.0, 1080.0));
    if (this._wipeHeight <= 0)
      return;
    Rectanglei destination = new Rectanglei(0, this._wipeHeight - this._textureWipe.Height, 1920, this._textureWipe.Height);
    renderer1.RenderTexture(this._textureWipe, (Rectangle) destination);
    destination = new Rectanglei(0, 1080 - this._wipeHeight, 1920, this._textureWipe.Height);
    renderer1.RenderTexture(this._textureWipe, (Rectangle) destination, flipy: true);
  }
}
