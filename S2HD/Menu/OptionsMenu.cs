﻿// Decompiled with JetBrains decompiler
// Type: S2HD.Menu.OptionsMenu
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca;
using SonicOrca.Drawing.Renderers;
using SonicOrca.Extensions;
using SonicOrca.Geometry;
using SonicOrca.Graphics;
using SonicOrca.Graphics.V2.Animation;
using SonicOrca.Resources;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace S2HD.Menu;

internal class OptionsMenu : IDisposable
{
  [ResourcePath("SONICORCA/MENU/OPTIONS/CIRCLE")]
  private ITexture _circleTexture;
  [ResourcePath("SONICORCA/MENU/OPTIONS/V2/PAUSE")]
  private CompositionGroup _menuPopInCompositionGroup;
  [ResourcePath("SONICORCA/MENU/OPTIONS/V2/EXITPAUSE")]
  private CompositionGroup _menuPopOutCompositionGroup;
  [ResourcePath("SONICORCA/MENU/OPTIONS/V2/AUDIO")]
  private CompositionGroup _subMenuAudioPopInCompositionGroup;
  [ResourcePath("SONICORCA/MENU/OPTIONS/V2/EXITAUDIO")]
  private CompositionGroup _subMenuAudioPopOutCompositionGroup;
  [ResourcePath("SONICORCA/MENU/OPTIONS/V2/VIDEO")]
  private CompositionGroup _subMenuVideoPopInCompositionGroup;
  [ResourcePath("SONICORCA/MENU/OPTIONS/V2/EXITVIDEO")]
  private CompositionGroup _subMenuVideoPopOutCompositionGroup;
  [ResourcePath("SONICORCA/MENU/OPTIONS/V2/OPTIONS")]
  private CompositionGroup _subMenuOptionsPopInCompositionGroup;
  private CompositionInstance _menuPopInComposition;
  private CompositionInstance _menuPopOutComposition;
  private CompositionInstance _subMenuAudioPopInComposition;
  private CompositionInstance _subMenuAudioPopOutComposition;
  private CompositionInstance _subMenuVideoPopInComposition;
  private CompositionInstance _subMenuVideoPopOutComposition;
  private CompositionInstance _subMenuOptionsPopInComposition;
  private CompositionInstance _currentComposition;
  private readonly S2HDSonicOrcaGameContext _gameContext;
  private readonly SettingUIResources _settingUIResources;
  private ResourceSession _resourceSession;
  private bool _loaded;
  private bool _initialised;
  private readonly IGraphicsContext _graphicsContext;
  private float _circleSize;
  private Rectanglei _circleBounds;
  private bool _showing;
  private readonly MenuViewPresenterFactory _viewPresenterFactory;
  private MenuViewPresenterHost _viewPresenterHost;
  private List<CompositionInstance> _compositions = new List<CompositionInstance>();

  public bool CanRestart { get; set; }

  public event EventHandler OnResume;

  public event EventHandler OnRestart;

  public event EventHandler OnQuit;

  public OptionsMenu(S2HDSonicOrcaGameContext gameContext)
  {
    this._gameContext = gameContext;
    this._graphicsContext = gameContext.Window.GraphicsContext;
    this._settingUIResources = new SettingUIResources();
    this._viewPresenterFactory = new MenuViewPresenterFactory(this._gameContext, (ISettingUIResources) this._settingUIResources);
  }

  public void Dispose() => this._resourceSession?.Dispose();

  public async Task LoadAsync(CancellationToken ct = default (CancellationToken))
  {
    OptionsMenu instance = this;
    ResourceTree resourceTree = instance._gameContext.ResourceTree;
    instance._resourceSession = new ResourceSession(resourceTree);
    instance._resourceSession.PushDependenciesByAttribute((object) instance);
    instance._settingUIResources.PushDependencies(instance._resourceSession);
    await instance._resourceSession.LoadAsync(ct);
    resourceTree.FullfillLoadedResourcesByAttribute((object) instance);
    instance._settingUIResources.FetchResources(resourceTree);
    instance._loaded = true;
  }

  private void Initialise()
  {
    this._menuPopInComposition = new CompositionInstance(this._menuPopInCompositionGroup);
    this._menuPopOutComposition = new CompositionInstance(this._menuPopOutCompositionGroup);
    this._subMenuAudioPopInComposition = new CompositionInstance(this._subMenuAudioPopInCompositionGroup);
    this._subMenuAudioPopOutComposition = new CompositionInstance(this._subMenuAudioPopOutCompositionGroup);
    this._subMenuVideoPopInComposition = new CompositionInstance(this._subMenuVideoPopInCompositionGroup);
    this._subMenuVideoPopOutComposition = new CompositionInstance(this._subMenuVideoPopOutCompositionGroup);
    this._subMenuOptionsPopInComposition = new CompositionInstance(this._subMenuOptionsPopInCompositionGroup);
    this._currentComposition = this._menuPopInComposition;
    this._compositions.Add(this._menuPopInComposition);
    this._compositions.Add(this._menuPopOutComposition);
    this._compositions.Add(this._subMenuAudioPopInComposition);
    this._compositions.Add(this._subMenuAudioPopOutComposition);
    this._compositions.Add(this._subMenuVideoPopInComposition);
    this._compositions.Add(this._subMenuVideoPopOutComposition);
    this._compositions.Add(this._subMenuOptionsPopInComposition);
  }

  public void Update()
  {
    if (!this._loaded)
      return;
    if (!this._initialised)
    {
      this._initialised = true;
      this.Initialise();
    }
    if (!this._showing || this._currentComposition == null)
      return;
    this._currentComposition.Animate();
    if (!this._currentComposition.Finished || this._currentComposition == this._menuPopOutComposition)
      return;
    this._circleSize = (float) this._circleTexture.Width;
    int circleSize = (int) this._circleSize;
    this._circleBounds = new Rectanglei(0, 0, circleSize, circleSize);
    if (this._currentComposition == this._subMenuVideoPopInComposition)
    {
      this._circleBounds.X = (1920 - circleSize) / 2;
      this._circleBounds.Y = 337;
    }
    else if (this._currentComposition == this._subMenuAudioPopInComposition)
    {
      this._circleBounds.X = (1920 - circleSize) / 2;
      this._circleBounds.Y = 400;
    }
    else
    {
      this._circleBounds.X = (1920 - circleSize) / 2;
      this._circleBounds.Y = (1080 - circleSize) / 2;
    }
    this._viewPresenterHost.Bounds = Rectanglei.FromLTRB(this._circleBounds.Left, this._circleBounds.Top + 60, this._circleBounds.Right, this._circleBounds.Bottom);
    this._viewPresenterHost.Update();
    this._viewPresenterHost.HandleInput();
  }

  private void NavigateNextHandler(object sender, NavigateNextEventArgs e)
  {
    if (!(e.Tag is int))
      return;
    switch ((int) e.Tag)
    {
      case 1:
        EventHandler onQuit = this.OnQuit;
        if (onQuit == null)
          break;
        onQuit((object) this, EventArgs.Empty);
        break;
      case 2:
        EventHandler onRestart = this.OnRestart;
        if (onRestart == null)
          break;
        onRestart((object) this, EventArgs.Empty);
        break;
      case 3:
        EventHandler onResume = this.OnResume;
        if (onResume == null)
          break;
        onResume((object) this, EventArgs.Empty);
        break;
      case 4:
        this._gameContext.Settings.Apply();
        break;
      case 5:
        this.ResetCompositions();
        this._currentComposition = this._subMenuOptionsPopInComposition;
        break;
      case 6:
        this.ResetCompositions();
        this._currentComposition = this._subMenuAudioPopInComposition;
        break;
      case 7:
        this.ResetCompositions();
        this._currentComposition = this._subMenuVideoPopInComposition;
        break;
    }
  }

  private void NavigateBackHandler(object sender, EventArgs e)
  {
    if (this._currentComposition == this._menuPopInComposition || this._currentComposition == this._subMenuOptionsPopInComposition)
      return;
    this.ResetCompositions();
    this._currentComposition = this._subMenuOptionsPopInComposition;
  }

  private void ResetCompositions()
  {
    foreach (CompositionInstance composition in this._compositions)
      composition.ResetFrame();
  }

  public void Draw(Renderer renderer)
  {
    if (!this._loaded || !this._showing)
      return;
    I2dRenderer g = renderer.Get2dRenderer();
    this.BlurGame(renderer, g);
    if (this._currentComposition == null)
      return;
    this._currentComposition.Draw(renderer);
    if (!this._currentComposition.Finished)
      return;
    this.DrawViewPresenterHost(renderer);
  }

  private void BlurGame(Renderer renderer, I2dRenderer g)
  {
    Rectanglei destination = new Rectanglei(0, 0, 1920, 1080);
    renderer.DeativateRenderer();
    IFramebuffer currentFramebuffer = this._graphicsContext.CurrentFramebuffer;
    GaussianBlurRenderer gaussianBlurRenderer = GaussianBlurRenderer.FromRenderer(renderer);
    gaussianBlurRenderer.Softness = 4.0;
    gaussianBlurRenderer.Render(currentFramebuffer.Textures[0], destination);
    g.BlendMode = BlendMode.Alpha;
    g.RenderQuad(new Colour(0.5, 0.0, 0.0, 0.0), (Rectangle) destination);
    renderer.DeativateRenderer();
  }

  private void DrawViewPresenterHost(Renderer renderer) => this._viewPresenterHost.Draw(renderer);

  public void Show()
  {
    this.ResetCompositions();
    this._currentComposition = this._menuPopInComposition;
    this._showing = true;
    this._viewPresenterHost = new MenuViewPresenterHost(this._viewPresenterFactory, (IMenuViewModel) new MenuViewFactory(this._gameContext).GetInGameOptionsView(this.CanRestart), (ISettingUIResources) this._settingUIResources);
    this._viewPresenterHost.NavigateNext += new EventHandler<NavigateNextEventArgs>(this.NavigateNextHandler);
    this._viewPresenterHost.NavigateBack += new EventHandler(this.NavigateBackHandler);
  }

  public void Hide()
  {
    this._showing = false;
    this._viewPresenterHost = (MenuViewPresenterHost) null;
  }
}
