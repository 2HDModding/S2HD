using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using S2HD.Menu;
using SonicOrca;
using SonicOrca.Core;

namespace S2HD
{
    // Token: 0x020000A4 RID: 164
    internal class StoryPlaythroughGameState : IGameState, IDisposable
    {
        // Token: 0x17000075 RID: 117
        // (get) Token: 0x060003C8 RID: 968 RVA: 0x0001A65C File Offset: 0x0001885C
        // (set) Token: 0x060003C9 RID: 969 RVA: 0x0001A664 File Offset: 0x00018864
        public bool QuitViaOptions { get; private set; }

        // Token: 0x060003CA RID: 970 RVA: 0x0001A66D File Offset: 0x0001886D
        public StoryPlaythroughGameState(S2HDSonicOrcaGameContext gameContext)
        {
            this._gameContext = gameContext;
            this._prepareSettings = new LevelPrepareSettings();
            this._levelNumber = 1;
        }

        // Token: 0x060003CB RID: 971 RVA: 0x0001A690 File Offset: 0x00018890
        public void StartFrom(LevelPrepareSettings prepareSettings)
        {
            this._prepareSettings = prepareSettings;
            this._levelGameState = new LevelGameState(this._gameContext);
            this._levelGameState.PrepareSettings = prepareSettings;
            this._levelNumber = prepareSettings.LevelNumber;
            this._gameContext.Console.LevelGameState = this._levelGameState;
        }

        // Token: 0x060003CC RID: 972 RVA: 0x0001A6E3 File Offset: 0x000188E3
        public IEnumerable<UpdateResult> Update()
        {
            this.QuitViaOptions = false;
            this._optionsMenu = new OptionsMenu(this._gameContext);
            this._optionsMenu.OnResume += delegate (object s, EventArgs e)
            {
                this._levelGameState.Level.Unpause();
            };
            this._optionsMenu.OnRestart += delegate (object s, EventArgs e)
            {
                Level level2 = this._levelGameState.Level;
                if (level2.Player.Lives > 0)
                {
                    level2.Unpause();
                    level2.FadeOut(LevelState.RestartCheckpoint);
                    Player player = level2.Player;
                    int lives = player.Lives;
                    player.Lives = lives - 1;
                }
            };
            this._optionsMenu.OnQuit += delegate (object s, EventArgs e)
            {
                Level level2 = this._levelGameState.Level;
                level2.Unpause();
                level2.FadeOut(LevelState.Quit);
                this.QuitViaOptions = true;
            };
            Task optionsLoadTask = this._optionsMenu.LoadAsync(default(CancellationToken));
            while (!optionsLoadTask.IsCompleted)
            {
                yield return UpdateResult.Next();
            }
            if (this._levelGameState == null)
            {
                this.SetLevel(this._levelNumber);
            }
            bool flag;
            do
            {
                foreach (UpdateResult updateResult in this._levelGameState.Update())
                {
                    Level level = this._levelGameState.Level;
                    if (level != null && level.StateFlags.HasFlag(LevelStateFlags.Paused))
                    {
                        if (!this._optionsMenuShowing)
                        {
                            this._optionsMenuShowing = true;
                            this._optionsMenu.CanRestart = (this._levelGameState.Player.Lives > 1);
                            this._optionsMenu.Show();
                        }
                    }
                    else if (this._optionsMenuShowing)
                    {
                        this._optionsMenuShowing = false;
                        this._optionsMenu.Hide();
                    }
                    else
                    {
                        this._levelGameState.HandleInput();
                    }
                    this._optionsMenu.Update();
                    yield return updateResult;
                }
                IEnumerator<UpdateResult> enumerator = null;
                flag = this._levelGameState.Completed;
                if (flag)
                {
                    this._prepareSettings.Lives = this._levelGameState.Player.Lives;
                    this._prepareSettings.Score = this._levelGameState.Player.Score;
                    if (!this.LoadNextLevel())
                    {
                        flag = false;
                    }
                }
            }
            while (flag);
            yield break;
            yield break;
        }

        // Token: 0x060003CD RID: 973 RVA: 0x0001A6F3 File Offset: 0x000188F3
        public void Draw()
        {
            if (this._levelGameState != null)
            {
                this._levelGameState.Draw();
                this._optionsMenu.Draw(this._gameContext.Renderer);
            }
        }

        // Token: 0x060003CE RID: 974 RVA: 0x0001A71E File Offset: 0x0001891E
        private bool LoadNextLevel()
        {
            this._levelNumber++;
            return this.SetLevel(this._levelNumber);
        }

        // Token: 0x060003CF RID: 975 RVA: 0x0001A73C File Offset: 0x0001893C
        private bool SetLevel(int index)
        {
            if (this._levelGameState != null)
            {
                this._levelGameState.Dispose();
            }
            LevelInfo levelInfo = (from x in Levels.LevelList
                                   where x.GameIndex == 2
                                   where !x.Unreleased
                                   select x).Skip(this._levelNumber - 1).FirstOrDefault<LevelInfo>();
            if (levelInfo == null)
            {
                return false;
            }
            this._levelGameState = new LevelGameState(this._gameContext);
            this._levelGameState.PrepareSettings = new LevelPrepareSettings
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

        // Token: 0x060003D0 RID: 976 RVA: 0x0001A866 File Offset: 0x00018A66
        public void Dispose()
        {
            if (this._levelGameState != null)
            {
                this._levelGameState.Dispose();
            }
        }

        // Token: 0x04000460 RID: 1120
        private S2HDSonicOrcaGameContext _gameContext;

        // Token: 0x04000461 RID: 1121
        private LevelPrepareSettings _prepareSettings;

        // Token: 0x04000462 RID: 1122
        private LevelGameState _levelGameState;

        // Token: 0x04000463 RID: 1123
        private int _levelNumber;

        // Token: 0x04000464 RID: 1124
        private OptionsMenu _optionsMenu;

        // Token: 0x04000465 RID: 1125
        private bool _optionsMenuShowing;
    }
}
