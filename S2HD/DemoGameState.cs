// Decompiled with JetBrains decompiler
// Type: S2HD.DemoGameState
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca;
using SonicOrca.Core;
using SonicOrca.Input;
using SonicOrca.Resources;
using System;
using System.Collections.Generic;

#nullable disable
namespace S2HD;

internal class DemoGameState : IGameState, IDisposable
{
  private readonly S2HDSonicOrcaGameContext _gameContext;
  private readonly string _demoResourceKey;
  private LevelPrepareSettings _prepareSettings;
  private LevelGameState _levelGameState;
  private int _demoLength = 1800;

  public DemoGameState(S2HDSonicOrcaGameContext gameContext, string demoResourceKey)
  {
    this._gameContext = gameContext;
    this._demoResourceKey = demoResourceKey;
  }

  private byte[] GetDemoData(string resourceKey)
  {
    ResourceTree resourceTree = this._gameContext.ResourceTree;
    using (ResourceSession resourceSession = new ResourceSession(resourceTree))
    {
      resourceSession.PushDependency(resourceKey);
      resourceSession.LoadAsync(serial: true).Wait();
      return resourceTree.GetLoadedResource<InputRecordingResource>(resourceKey).Data;
    }
  }

  private void StartLevel(LevelPrepareSettings prepareSettings)
  {
    this._prepareSettings = prepareSettings;
    this._levelGameState = new LevelGameState((SonicOrcaGameContext) this._gameContext);
    this._levelGameState.PrepareSettings = prepareSettings;
    this._gameContext.Console.LevelGameState = this._levelGameState;
  }

  public IEnumerable<UpdateResult> Update()
  {
    byte[] demoData = this.GetDemoData(this._demoResourceKey);
    this.StartLevel(new LevelPrepareSettings()
    {
      Act = 1,
      AreaResourceKey = Levels.GetAreaResourceKey("ehz"),
      LevelNumber = 1,
      Lives = 3,
      ProtagonistCharacter = CharacterType.Sonic,
      SidekickCharacter = CharacterType.Null,
      RecordedPlayReadData = demoData
    });
    bool finishingDemo = false;
label_1:
    foreach (UpdateResult updateResult in this._levelGameState.Update())
    {
      if (!finishingDemo && this.ShouldFinishDemo())
      {
        finishingDemo = true;
        this._levelGameState.Level?.FadeOut(LevelState.Quit);
      }
      yield return updateResult;
    }
    if (this._levelGameState.Completed)
      goto label_1;
  }

  private bool ShouldFinishDemo()
  {
    Level level = this._levelGameState.Level;
    if (level != null && level.Ticks >= this._demoLength)
      return true;
    InputState pressed = this._gameContext.Input.Pressed;
    return pressed.Keyboard[40] || pressed.GamePad[0].Start;
  }

  public void Draw()
  {
    if (this._levelGameState == null)
      return;
    this._levelGameState.Draw();
  }

  public void Dispose()
  {
    if (this._levelGameState == null)
      return;
    this._levelGameState.Dispose();
  }
}
