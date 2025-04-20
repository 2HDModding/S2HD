// Decompiled with JetBrains decompiler
// Type: S2HD.DisclaimerGameState
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca;
using SonicOrca.Extensions;
using SonicOrca.Geometry;
using SonicOrca.Graphics;
using SonicOrca.Resources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace S2HD;

internal class DisclaimerGameState : IGameState, IDisposable
{
  private const int FadeTime = 60;
  private const int ShowTime = 240 /*0xF0*/;
  [ResourcePath("SONICORCA/DISCLAIMER")]
  private ITexture _disclaimerTexture;
  private S2HDSonicOrcaGameContext _gameContext;
  private ResourceSession _resourceSession;
  private bool _loaded;
  private float _opacity;

  public DisclaimerGameState(S2HDSonicOrcaGameContext gameContext)
  {
    this._gameContext = gameContext;
  }

  public async Task LoadAsync()
  {
    DisclaimerGameState instance = this;
    ResourceTree resourceTree = instance._gameContext.ResourceTree;
    instance._resourceSession = new ResourceSession(resourceTree);
    instance._resourceSession.PushDependenciesByAttribute((object) instance);
    await instance._resourceSession.LoadAsync();
    resourceTree.FullfillLoadedResourcesByAttribute((object) instance);
    instance._loaded = true;
  }

  public void Dispose() => this._resourceSession?.Dispose();

  public IEnumerable<UpdateResult> Update()
  {
    Task loadTask = this.LoadAsync();
    while (!loadTask.IsCompleted)
      yield return UpdateResult.Next();
    yield return UpdateResult.Wait(60);
    this._opacity = 0.0f;
    while ((double) this._opacity < 1.0)
    {
      this._opacity += 0.0166666675f;
      this._opacity = Math.Min(this._opacity, 1f);
      yield return UpdateResult.Next();
    }
    int time = 240 /*0xF0*/;
    while (time > 0)
    {
      --time;
      yield return UpdateResult.Next();
    }
    while ((double) this._opacity > 0.0)
    {
      this._opacity -= 0.0166666675f;
      this._opacity = Math.Max(this._opacity, 0.0f);
      yield return UpdateResult.Next();
    }
  }

  public void Draw()
  {
    if (!this._loaded)
      return;
    I2dRenderer obj = this._gameContext.Renderer.Get2dRenderer();
    Rectanglei rectanglei = (Rectanglei) new Rectangle(0.0, 0.0, 1920.0, 1080.0);
    obj.BlendMode = BlendMode.Opaque;
    obj.Colour = Colours.White;
    obj.RenderTexture(this._disclaimerTexture, (Vector2) rectanglei.Centre);
    Colour colour = new Colour(1.0 - (double) this._opacity, 0.0, 0.0, 0.0);
    obj.BlendMode = BlendMode.Alpha;
    obj.RenderQuad(colour, new Rectangle(0.0, 0.0, 1920.0, 1080.0));
  }
}
