// Decompiled with JetBrains decompiler
// Type: SonicOrca.Menu.SegaScreen
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Audio;
using SonicOrca.Geometry;
using SonicOrca.Graphics;
using SonicOrca.Resources;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SonicOrca.Menu;

internal class SegaScreen : Screen
{
  private const string LogoResourceKey = "SONICORCA/MENU/SEGA/SCREENLOGO";
  private const string SoundResourceKey = "SONICORCA/MENU/SEGA/SOUND";
  private readonly SonicOrcaGameContext _context;
  private readonly FadeTransition _fadeTransition = new FadeTransition(30);
  private ResourceSession _resourceSession;
  private ITexture _logoTexture;
  private Sample _segaSample;
  private SampleInstance _segaSampleInstance;
  private readonly FiniteStateMachine _stateMachine;
  private bool _cancelled;

  public SegaScreen(SonicOrcaGameContext context)
  {
    this._context = context;
    this._fadeTransition.Set();
    this._stateMachine = this.CreateFSM();
  }

  public override async Task LoadAsync(ScreenLoadingProgress progress, CancellationToken ct = default (CancellationToken))
  {
    ResourceTree resourceTree = this._context.ResourceTree;
    this._resourceSession = new ResourceSession(resourceTree);
    this._resourceSession.PushDependencies("SONICORCA/MENU/SEGA/SCREENLOGO", "SONICORCA/MENU/SEGA/SOUND");
    await this._resourceSession.LoadAsync(ct);
    this._logoTexture = resourceTree.GetLoadedResource<ITexture>("SONICORCA/MENU/SEGA/SCREENLOGO");
    this._segaSample = resourceTree.GetLoadedResource<Sample>("SONICORCA/MENU/SEGA/SOUND");
  }

  public override void Unload()
  {
    if (this._segaSampleInstance != null)
      this._segaSampleInstance.Dispose();
    if (this._resourceSession == null)
      return;
    this._resourceSession.Unload();
  }

  private FiniteStateMachine CreateFSM()
  {
    int cancelWait = 0;
    FiniteStateMachine finiteStateMachine = new FiniteStateMachine();
    finiteStateMachine.Begin().Do((Action) (() => cancelWait = 120)).While((Func<bool>) (() => cancelWait-- >= 0 && !this._cancelled)).Do((Action) (() =>
    {
      if (this._cancelled)
        finiteStateMachine.Finish();
      else
        this._fadeTransition.BeginFadeIn();
    })).While((Func<bool>) (() => !this._fadeTransition.Finished), (Action) (() =>
    {
      if (this._cancelled)
      {
        this._fadeTransition.BeginFadeOut();
        finiteStateMachine.Begin().Do((Action) (() => this._fadeTransition.BeginFadeOut())).While((Func<bool>) (() => !this._fadeTransition.Finished), (Action) (() => this._fadeTransition.Update())).Do((Action) (() => finiteStateMachine.Finish()));
      }
      else
        this._fadeTransition.Update();
    })).Do((Action) (() =>
    {
      this._segaSampleInstance = new SampleInstance(this._context, this._segaSample);
      this._segaSampleInstance.Play();
    })).While((Func<bool>) (() => this._segaSampleInstance.Playing)).Do((Action) (() => cancelWait = 60)).While((Func<bool>) (() => cancelWait-- >= 0 && !this._cancelled)).Do((Action) (() => this._fadeTransition.BeginFadeOut())).While((Func<bool>) (() => !this._fadeTransition.Finished), (Action) (() => this._fadeTransition.Update()));
    return finiteStateMachine;
  }

  public override void Update()
  {
    if (this._context.Pressed[0].Start)
      this._cancelled = true;
    if (this._stateMachine.Update())
      return;
    this.Finish();
  }

  public override void Draw(Renderer renderer)
  {
    I2dRenderer obj = renderer.Get2dRenderer();
    obj.RenderQuad(Colours.White, new Rectangle(0.0, 0.0, 1920.0, 1080.0));
    obj.RenderTexture(this._logoTexture, new Vector2(960.0, 540.0));
    this._fadeTransition.Draw(renderer);
  }
}
