// Decompiled with JetBrains decompiler
// Type: S2HD.Title.Banner
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca;
using SonicOrca.Geometry;
using SonicOrca.Graphics;
using SonicOrca.Resources;
using System;
using System.Collections.Generic;

#nullable disable
namespace S2HD.Title;

internal class Banner
{
  private const int BannerStartTime = 332;
  private static readonly Vector2i[] SpinningStarOffsets = new Vector2i[8]
  {
    new Vector2i(-214, -41),
    new Vector2i(-190, -129),
    new Vector2i(-130, -193),
    new Vector2i(-50, -233),
    new Vector2i(50, -233),
    new Vector2i(130, -193),
    new Vector2i(190, -129),
    new Vector2i(214, -41)
  };
  private readonly S2HDSonicOrcaGameContext _gameContext;
  private readonly IMaskRenderer _maskRenderer;
  private AnimationGroup _animationGroup;
  [ResourcePath("SONICORCA/TITLE/FRAMES/0")]
  private ITexture _bannerTexture;
  [ResourcePath("SONICORCA/TITLE/FRAMES/1")]
  private ITexture _bannerInsideTexture;
  [ResourcePath("SONICORCA/TITLE/ADDFRAMES/0")]
  private ITexture _hdStarTexture;
  [ResourcePath("SONICORCA/TITLE/ADDFRAMES/1")]
  private ITexture _maskTexture;
  [ResourcePath("SONICORCA/TITLE/ADDFRAMES/2")]
  private ITexture _titleOutlineTexture;
  private AnimationInstance[] _preBannerStarSpin;
  private AnimationInstance _sonicAnimationInstance;
  private AnimationInstance _sonicHandAnimationInstance;
  private AnimationInstance _tailsAnimationInstance;
  private AnimationInstance _tailsTailsAnimationInstance;
  private AnimationInstance _sonic2AnimationInstance;
  private AnimationInstance _theHedgehogAnimationInstance;
  private AnimationInstance _hdAnimationInstance;
  private readonly EffectEventManager _effectEventManager = new EffectEventManager();
  private int _ticks;
  private double _bannerInsideOpacity;
  private double _bannerOpacity;
  private double _sonic2Opacity;
  private double _maskOffset;

  public Vector2i Position { get; set; }

  public bool ShowStarLensFare { get; set; }

  public Banner(S2HDSonicOrcaGameContext gameContext, IMaskRenderer maskRenderer)
  {
    this._gameContext = gameContext;
    this._maskRenderer = maskRenderer;
    this.GetResourceReferences();
    this.Position = new Vector2i(960, 476);
  }

  private void GetResourceReferences()
  {
    ResourceTree resourceTree = this._gameContext.ResourceTree;
    this._animationGroup = resourceTree.GetLoadedResource<AnimationGroup>("SONICORCA/TITLE/ANIGROUP");
    this._bannerTexture = resourceTree.GetLoadedResource<ITexture>("SONICORCA/TITLE/FRAMES/0");
    this._bannerInsideTexture = resourceTree.GetLoadedResource<ITexture>("SONICORCA/TITLE/FRAMES/1");
    this._titleOutlineTexture = resourceTree.GetLoadedResource<ITexture>("SONICORCA/TITLE/ADDFRAMES/0");
    this._maskTexture = resourceTree.GetLoadedResource<ITexture>("SONICORCA/TITLE/ADDFRAMES/1");
    this._hdStarTexture = resourceTree.GetLoadedResource<ITexture>("SONICORCA/TITLE/ADDFRAMES/2");
  }

  public void Reset()
  {
    this._ticks = 0;
    this._preBannerStarSpin = new AnimationInstance[8];
    this._sonicAnimationInstance = (AnimationInstance) null;
    this._sonicHandAnimationInstance = (AnimationInstance) null;
    this._tailsAnimationInstance = (AnimationInstance) null;
    this._tailsTailsAnimationInstance = (AnimationInstance) null;
    this._sonic2AnimationInstance = (AnimationInstance) null;
    this._theHedgehogAnimationInstance = (AnimationInstance) null;
    this._hdAnimationInstance = (AnimationInstance) null;
    this._bannerInsideOpacity = 0.0;
    this._bannerOpacity = 0.0;
    this._sonic2Opacity = 0.0;
    this._maskOffset = -1.0;
    this.ShowStarLensFare = false;
  }

  public void DoShine(int duration)
  {
    this._effectEventManager.BeginEvent(this.EffectShine(duration));
  }

  private IEnumerable<UpdateResult> EffectShine(int duration)
  {
    this._maskOffset = -1.0;
    do
    {
      yield return UpdateResult.Next();
      this._maskOffset += 2.0 / (double) duration;
    }
    while (this._maskOffset < 1.0);
    this._maskOffset = 1.0;
  }

  public void Update()
  {
    if (this._ticks == 268)
      this.DoShine(368);
    if (this._ticks >= 296)
      this._bannerInsideOpacity = Math.Min(1.0, this._bannerInsideOpacity + 1.0 / 72.0);
    if (this._ticks >= 332)
      this._bannerOpacity = Math.Min(1.0, this._bannerOpacity + 1.0 / 32.0);
    for (int index = 0; index < this._preBannerStarSpin.Length; ++index)
    {
      if (this._preBannerStarSpin[index] == null)
      {
        if (this._ticks >= 298 + index * 4)
          this._preBannerStarSpin[index] = new AnimationInstance(this._animationGroup, 3);
      }
      else if (this._preBannerStarSpin[index].Cycles == 0)
        this._preBannerStarSpin[index].Animate();
    }
    if (this._ticks >= 346)
    {
      this._sonic2Opacity = Math.Min(1.0, this._sonic2Opacity + 1.0 / 16.0);
      if (this._sonic2AnimationInstance == null)
        this._sonic2AnimationInstance = new AnimationInstance(this._animationGroup, 2);
    }
    if (this._ticks >= 360)
    {
      if (this._theHedgehogAnimationInstance == null)
        this._theHedgehogAnimationInstance = new AnimationInstance(this._animationGroup, 1);
      this._theHedgehogAnimationInstance.Animate();
    }
    if (this._ticks >= 364)
    {
      if (this._sonicAnimationInstance == null)
        this._sonicAnimationInstance = new AnimationInstance(this._animationGroup, 4);
      this._sonicAnimationInstance.Animate();
    }
    if (this._ticks >= 413)
    {
      if (this._sonicHandAnimationInstance == null)
        this._sonicHandAnimationInstance = new AnimationInstance(this._animationGroup, 5);
      this._sonicHandAnimationInstance.Animate();
    }
    if (this._ticks >= 400)
    {
      if (this._tailsAnimationInstance == null)
      {
        this._tailsAnimationInstance = new AnimationInstance(this._animationGroup, 6);
        this._tailsTailsAnimationInstance = new AnimationInstance(this._animationGroup, 7);
      }
      this._tailsAnimationInstance.Animate();
      this._tailsTailsAnimationInstance.Animate();
    }
    if (this._ticks >= 436)
      this._sonic2AnimationInstance.Animate();
    if (this._ticks >= 454)
    {
      if (this._hdAnimationInstance == null)
        this._hdAnimationInstance = new AnimationInstance(this._animationGroup);
      this._hdAnimationInstance.Animate();
    }
    this._effectEventManager.Update();
    ++this._ticks;
  }

  public void Draw(Renderer renderer)
  {
    I2dRenderer renderer1 = renderer.Get2dRenderer();
    renderer1.BlendMode = BlendMode.Alpha;
    using (renderer1.BeginMatixState())
    {
      I2dRenderer obj = renderer1;
      Matrix4 modelMatrix = renderer1.ModelMatrix;
      ref Matrix4 local = ref modelMatrix;
      Vector2i position = this.Position;
      double x = (double) position.X;
      position = this.Position;
      double y = (double) position.Y;
      Matrix4 matrix4 = local.Translate(x, y);
      obj.ModelMatrix = matrix4;
      if (this._bannerInsideOpacity > 0.0 && this._bannerOpacity < 1.0)
      {
        renderer1.Colour = new Colour(this._bannerInsideOpacity, 1.0, 1.0, 1.0);
        renderer1.RenderTexture(this._bannerInsideTexture, new Vector2(0.0, -114.0));
      }
      if (this._bannerOpacity > 0.0)
      {
        renderer1.Colour = new Colour(this._bannerOpacity, 1.0, 1.0, 1.0);
        renderer1.RenderTexture(this._bannerTexture, new Vector2(0.0, 0.0));
      }
      for (int index = 0; index < 8; ++index)
      {
        if (this._preBannerStarSpin[index] != null && this._preBannerStarSpin[index].Cycles == 0)
          this._preBannerStarSpin[index].Draw(renderer1, (Vector2) Banner.SpinningStarOffsets[index]);
      }
      this.DrawSonicAndTails(renderer);
      if (this._sonic2AnimationInstance != null)
        this._sonic2AnimationInstance.Draw(renderer1, new Colour(this._sonic2Opacity, 1.0, 1.0, 1.0), new Vector2(0.0, 77.0));
      if (this._theHedgehogAnimationInstance != null)
        this._theHedgehogAnimationInstance.Draw(renderer1, new Vector2(0.0, 153.0));
      if (this._hdAnimationInstance != null)
        this._hdAnimationInstance.Draw(renderer1, new Vector2(0.0, 266.0));
      if (!this.ShowStarLensFare)
        return;
      renderer1.BlendMode = BlendMode.Additive;
      renderer1.RenderTexture(this._hdStarTexture, new Vector2(0.0, 188.0));
    }
  }

  public void DrawUnfaded(Renderer renderer) => this.DrawTitleShine(renderer);

  private void DrawSonicAndTails(Renderer renderer)
  {
    I2dRenderer renderer1 = renderer.Get2dRenderer();
    if (this._sonicAnimationInstance != null)
      this._sonicAnimationInstance.Draw(renderer1, new Vector2(89.0, -158.0));
    if (this._sonicHandAnimationInstance != null)
      this._sonicHandAnimationInstance.Draw(renderer1, new Vector2(230.0, -89.0));
    if (this._tailsTailsAnimationInstance != null)
      this._tailsTailsAnimationInstance.Draw(renderer1, new Vector2(-264.0, -65.0));
    if (this._tailsAnimationInstance == null)
      return;
    this._tailsAnimationInstance.Draw(renderer1, new Vector2(-108.0, -121.0));
  }

  private void DrawTitleShine(Renderer renderer)
  {
    if (this._maskOffset <= -1.0 || this._maskOffset >= 1.0)
      return;
    renderer.DeativateRenderer();
    this._maskRenderer.Texture = this._titleOutlineTexture;
    this._maskRenderer.Source = new Rectanglei(0, 0, this._titleOutlineTexture.Width, this._titleOutlineTexture.Height);
    this._maskRenderer.Destination = new Rectanglei(this.Position.X - this._titleOutlineTexture.Width / 2, this.Position.Y - this._titleOutlineTexture.Height / 2, this._titleOutlineTexture.Width, this._titleOutlineTexture.Height);
    this._maskTexture.Wrapping = TextureWrapping.Clamp;
    this._maskRenderer.MaskTexture = this._maskTexture;
    this._maskRenderer.MaskSource = new Rectanglei(0, 0, this._maskTexture.Width, this._maskTexture.Height);
    this._maskRenderer.MaskDestination = new Rectanglei(this.Position.X + (int) (this._maskOffset * 1920.0), this.Position.Y + (int) (this._maskOffset * 1080.0), 1920, 1080);
    this._maskRenderer.BlendMode = BlendMode.Additive;
    this._maskRenderer.Render(true);
  }
}
