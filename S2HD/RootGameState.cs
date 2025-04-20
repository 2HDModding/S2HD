// Decompiled with JetBrains decompiler
// Type: S2HD.RootGameState
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using S2HD.Menu;
using SonicOrca;
using SonicOrca.Core;
using SonicOrca.Geometry;
using SonicOrca.Input;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace S2HD;

internal class RootGameState : IGameState, IDisposable
{
  private static readonly string[] CharacterNames = new string[3]
  {
    "sonic",
    "tails",
    "knuckles"
  };
  private readonly S2HDSonicOrcaGameContext _gameContext;
  private readonly CommandLineArguments _cmdLineArgs;
  private IGameState _currentGameState;

  public RootGameState(S2HDSonicOrcaGameContext gameContext)
  {
    this._gameContext = gameContext;
    this._cmdLineArgs = gameContext.CommandLineArguments;
  }

  public IEnumerable<UpdateResult> Update()
  {
    this.Initialise();
    InputState controller = this._gameContext.Input.Pressed;
    InputContext input = this._gameContext.Input;
    if (this._cmdLineArgs.HasOption("level"))
    {
      LevelPrepareSettings settingsFromCommandLine = this.GetLevelPrepareSettingsFromCommandLine();
      if (settingsFromCommandLine == null)
      {
        Program.ShowErrorMessageBox("Level does not exist.");
      }
      else
      {
        StoryPlaythroughGameState newGameState = new StoryPlaythroughGameState(this._gameContext);
        newGameState.StartFrom(settingsFromCommandLine);
        this.ChangeGameState((IGameState) newGameState);
        foreach (UpdateResult updateResult in this._currentGameState.Update())
          yield return updateResult;
      }
    }
    else
    {
      if (this._cmdLineArgs.HasOption("credits"))
      {
        this.ShowCredits();
        foreach (UpdateResult updateResult in this._currentGameState.Update())
          yield return updateResult;
      }
      else if (this._cmdLineArgs.HasOption("options"))
      {
        this.ShowOptions();
        foreach (UpdateResult updateResult in this._currentGameState.Update())
          yield return updateResult;
      }
      if (this._cmdLineArgs.HasOption("demo"))
      {
        this.PlayDemo();
        foreach (UpdateResult updateResult in this._currentGameState.Update())
          yield return updateResult;
      }
      if (!this._cmdLineArgs.HasOption("nologos"))
      {
        this.ShowDisclaimer();
        foreach (UpdateResult updateResult in this._currentGameState.Update())
          yield return updateResult;
      }
      bool shouldQuit = false;
      while (!shouldQuit)
      {
        GamePadInputState gamePadInputState;
        if (!this._cmdLineArgs.HasOption("nologos"))
        {
          this.ShowLogos();
          foreach (UpdateResult updateResult in this._currentGameState.Update())
          {
            gamePadInputState = controller.GamePad[0];
            if (!gamePadInputState.Start && !input.Pressed.Keyboard[40])
              yield return updateResult;
            else
              break;
          }
        }
        if (!this._cmdLineArgs.HasOption("nologos"))
        {
          this.ShowTeamLogo();
          foreach (UpdateResult updateResult in this._currentGameState.Update())
          {
            gamePadInputState = controller.GamePad[0];
            if (!gamePadInputState.Start && !input.Pressed.Keyboard[40])
              yield return updateResult;
            else
              break;
          }
        }
        this.ShowTitle();
        foreach (UpdateResult updateResult in this._currentGameState.Update())
          yield return updateResult;
        TitleGameState currentGameState = this._currentGameState as TitleGameState;
        StoryPlaythroughGameState storyState;
        switch (currentGameState.Result)
        {
          case TitleGameState.ResultType.NewGame:
          case TitleGameState.ResultType.LevelSelect:
            this.PlayClassic();
            storyState = this._currentGameState as StoryPlaythroughGameState;
            if (currentGameState.LevelPrepareSettings != null)
              storyState.StartFrom(currentGameState.LevelPrepareSettings);
            foreach (UpdateResult updateResult in this._currentGameState.Update())
              yield return updateResult;
            if (!storyState.QuitViaOptions)
            {
              this.ShowCredits();
              foreach (UpdateResult updateResult in this._currentGameState.Update())
                yield return updateResult;
              break;
            }
            break;
          case TitleGameState.ResultType.ShowOptions:
            this.ShowOptions();
            foreach (UpdateResult updateResult in this._currentGameState.Update())
              yield return updateResult;
            break;
          case TitleGameState.ResultType.StartDemo:
            this.PlayDemo();
            foreach (UpdateResult updateResult in this._currentGameState.Update())
              yield return updateResult;
            break;
          case TitleGameState.ResultType.Quit:
            shouldQuit = true;
            break;
        }
        storyState = (StoryPlaythroughGameState) null;
      }
    }
  }

  public void Draw()
  {
    if (this._currentGameState == null)
      return;
    this._currentGameState.Draw();
  }

  private void Initialise() => SharedRenderers.Initialise(this._gameContext.RenderFactory);

  public void Dispose()
  {
    if (this._currentGameState != null)
      this._currentGameState.Dispose();
    SharedRenderers.Dispose();
  }

  private void ShowLogos()
  {
    this.ChangeGameState((IGameState) new LogosGameState((SonicOrcaGameContext) this._gameContext));
  }

  private void ShowTeamLogo()
  {
    this.ChangeGameState((IGameState) new TeamLogoGameState(this._gameContext));
  }

  private void ShowDisclaimer()
  {
    this.ChangeGameState((IGameState) new DisclaimerGameState(this._gameContext));
  }

  private void ShowTitle()
  {
    this.ChangeGameState((IGameState) new TitleGameState(this._gameContext));
  }

  private void PlayClassic()
  {
    this.ChangeGameState((IGameState) new StoryPlaythroughGameState(this._gameContext));
  }

  private void ShowOptions()
  {
    this.ChangeGameState((IGameState) new OptionsGameState(this._gameContext));
  }

  private void ShowCredits()
  {
    this.ChangeGameState((IGameState) new CreditsGameState((SonicOrcaGameContext) this._gameContext));
  }

  private void PlayDemo()
  {
    this.ChangeGameState((IGameState) new DemoGameState(this._gameContext, "SONICORCA/RECORDINGS/EHZ"));
  }

  private void ChangeGameState(IGameState newGameState)
  {
    if (this._currentGameState != null)
      this._currentGameState.Dispose();
    this._currentGameState = newGameState;
  }

  private LevelPrepareSettings GetLevelPrepareSettingsFromCommandLine()
  {
    string[] optionValues1 = this._cmdLineArgs.GetOptionValues("level");
    if (optionValues1.Length == 0)
      return (LevelPrepareSettings) null;
    string mnemonic = optionValues1[0];
    string areaResourceKey = Levels.GetAreaResourceKey(mnemonic);
    if (areaResourceKey == null)
      return (LevelPrepareSettings) null;
    int num = Levels.GetLevelIndex(mnemonic);
    if (num == -1)
      num = 1;
    LevelPrepareSettings settingsFromCommandLine = new LevelPrepareSettings()
    {
      Act = 1,
      AreaResourceKey = areaResourceKey,
      Debugging = this._cmdLineArgs.HasOption("debug"),
      LevelNumber = num,
      Lives = 3,
      TimeTrial = this._cmdLineArgs.HasOption("timetrial")
    };
    string[] optionValues2 = this._cmdLineArgs.GetOptionValues("act");
    int result1;
    if (optionValues2.Length != 0 && int.TryParse(optionValues2[0], out result1))
      settingsFromCommandLine.Act = result1;
    string[] optionValues3 = this._cmdLineArgs.GetOptionValues("lives");
    if (optionValues3.Length != 0 && int.TryParse(optionValues3[0], out result1))
      settingsFromCommandLine.Lives = result1;
    string[] optionValues4 = this._cmdLineArgs.GetOptionValues("protagonist");
    if (optionValues4.Length != 0)
      settingsFromCommandLine.ProtagonistCharacter = RootGameState.GetCharacterType(optionValues4[0]);
    string[] optionValues5 = this._cmdLineArgs.GetOptionValues("sidekick");
    if (optionValues5.Length != 0)
      settingsFromCommandLine.SidekickCharacter = RootGameState.GetCharacterType(optionValues5[0]);
    string[] optionValues6 = this._cmdLineArgs.GetOptionValues("startpos");
    int result2;
    int result3;
    if (optionValues6.Length >= 2 && int.TryParse(optionValues6[0], out result2) && int.TryParse(optionValues6[1], out result3))
      settingsFromCommandLine.StartPosition = new Vector2i?(new Vector2i(result2, result3));
    string[] optionValues7 = this._cmdLineArgs.GetOptionValues("night");
    float result4;
    if (optionValues7.Length != 0 && float.TryParse(optionValues7[0], NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result4))
      settingsFromCommandLine.NightMode = (double) result4;
    string[] optionValues8 = this._cmdLineArgs.GetOptionValues("record");
    if (optionValues8.Length != 0)
      settingsFromCommandLine.RecordedPlayWritePath = optionValues8[0];
    string[] optionValues9 = this._cmdLineArgs.GetOptionValues("playback");
    if (optionValues9.Length != 0)
      settingsFromCommandLine.RecordedPlayReadPath = optionValues9[0];
    string[] optionValues10 = this._cmdLineArgs.GetOptionValues("ghost");
    if (optionValues10.Length != 0)
      settingsFromCommandLine.RecordedPlayGhostReadPath = optionValues10[0];
    return settingsFromCommandLine;
  }

  private static CharacterType GetCharacterType(string characterType)
  {
    for (int index = 0; index < RootGameState.CharacterNames.Length; ++index)
    {
      if (RootGameState.CharacterNames[index].Equals(characterType, StringComparison.OrdinalIgnoreCase))
        return (CharacterType) (index + 1);
    }
    return CharacterType.Null;
  }
}
