// Decompiled with JetBrains decompiler
// Type: S2HD.StoryPlaythroughGameState
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using S2HD.Menu;
using SonicOrca;
using SonicOrca.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable disable
namespace S2HD;

internal class StoryPlaythroughGameState : IGameState, IDisposable
{
  private S2HDSonicOrcaGameContext _gameContext;
  private LevelPrepareSettings _prepareSettings;
  private LevelGameState _levelGameState;
  private int _levelNumber;
  private OptionsMenu _optionsMenu;
  private bool _optionsMenuShowing;

  public bool QuitViaOptions { get; private set; }

  public StoryPlaythroughGameState(S2HDSonicOrcaGameContext gameContext)
  {
    this._gameContext = gameContext;
    this._prepareSettings = new LevelPrepareSettings();
    this._levelNumber = 1;
  }

  public void StartFrom(LevelPrepareSettings prepareSettings)
  {
    this._prepareSettings = prepareSettings;
    this._levelGameState = new LevelGameState((SonicOrcaGameContext) this._gameContext);
    this._levelGameState.PrepareSettings = prepareSettings;
    this._levelNumber = prepareSettings.LevelNumber;
    this._gameContext.Console.LevelGameState = this._levelGameState;
  }

  public IEnumerable<UpdateResult> Update()
  {
    StoryPlaythroughGameState playthroughGameState = this;
    playthroughGameState.QuitViaOptions = false;
    playthroughGameState._optionsMenu = new OptionsMenu(playthroughGameState._gameContext);
    // ISSUE: reference to a compiler-generated method
    playthroughGameState._optionsMenu.OnResume += new EventHandler(playthroughGameState.\u003CUpdate\u003Eb__12_0);
    // ISSUE: reference to a compiler-generated method
    playthroughGameState._optionsMenu.OnRestart += new EventHandler(playthroughGameState.\u003CUpdate\u003Eb__12_1);
    // ISSUE: reference to a compiler-generated method
    playthroughGameState._optionsMenu.OnQuit += new EventHandler(playthroughGameState.\u003CUpdate\u003Eb__12_2);
    Task optionsLoadTask = playthroughGameState._optionsMenu.LoadAsync();
    while (!optionsLoadTask.IsCompleted)
      yield return UpdateResult.Next();
    if (playthroughGameState._levelGameState == null)
      playthroughGameState.SetLevel(playthroughGameState._levelNumber);
    bool flag;
    do
    {
      foreach (UpdateResult updateResult in playthroughGameState._levelGameState.Update())
      {
        Level level = playthroughGameState._levelGameState.Level;
        if (level != null && level.StateFlags.HasFlag((Enum) LevelStateFlags.Paused))
        {
          if (!playthroughGameState._optionsMenuShowing)
          {
            playthroughGameState._optionsMenuShowing = true;
            playthroughGameState._optionsMenu.CanRestart = playthroughGameState._levelGameState.Player.Lives > 1;
            playthroughGameState._optionsMenu.Show();
          }
        }
        else if (playthroughGameState._optionsMenuShowing)
        {
          playthroughGameState._optionsMenuShowing = false;
          playthroughGameState._optionsMenu.Hide();
        }
        else
          playthroughGameState._levelGameState.HandleInput();
        playthroughGameState._optionsMenu.Update();
        yield return updateResult;
      }
      flag = playthroughGameState._levelGameState.Completed;
      if (flag)
      {
        playthroughGameState._prepareSettings.Lives = playthroughGameState._levelGameState.Player.Lives;
        playthroughGameState._prepareSettings.Score = playthroughGameState._levelGameState.Player.Score;
        if (!playthroughGameState.LoadNextLevel())
          flag = false;
      }
    }
    while (flag);
  }

  public void Draw()
  {
    if (this._levelGameState == null)
      return;
    this._levelGameState.Draw();
    this._optionsMenu.Draw(this._gameContext.Renderer);
  }

  private bool LoadNextLevel()
  {
    ++this._levelNumber;
    return this.SetLevel(this._levelNumber);
  }

  private bool SetLevel(int index)
  {
    if (this._levelGameState != null)
      this._levelGameState.Dispose();
    LevelInfo levelInfo = ((IEnumerable<LevelInfo>) Levels.LevelList).Where<LevelInfo>((Func<LevelInfo, bool>) (x => x.GameIndex == 2)).Where<LevelInfo>((Func<LevelInfo, bool>) (x => !x.Unreleased)).Skip<LevelInfo>(this._levelNumber - 1).FirstOrDefault<LevelInfo>();
    if (levelInfo == null)
      return false;
    this._levelGameState = new LevelGameState((SonicOrcaGameContext) this._gameContext);
    this._levelGameState.PrepareSettings = new LevelPrepareSettings()
    {
      AreaResourceKey = Levels.GetAreaResourceKey(levelInfo.Mnemonic),
      Act = 1,
      Lives = this._prepareSettings.Lives,
      Score = this._prepareSettings.Score,
      Debugging = this._prepareSettings.Debugging,
      ProtagonistCharacter = this._prepareSettings.ProtagonistCharacter,
      SidekickCharacter = this._prepareSettings.SidekickCharacter
    };
    this._gameContext.Console.LevelGameState = this._levelGameState;
    return true;
  }

  public void Dispose()
  {
    if (this._levelGameState == null)
      return;
    this._levelGameState.Dispose();
  }
}
