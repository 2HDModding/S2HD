// Decompiled with JetBrains decompiler
// Type: SONICORCA.LEVELS.EHZ.AREA.EmeraldHillZoneArea
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca;
using SonicOrca.Audio;
using SonicOrca.Core;
using SonicOrca.Core.Objects;
using SonicOrca.Core.Objects.Base;
using SonicOrca.Extensions;
using SonicOrca.Geometry;
using SonicOrca.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SONICORCA.LEVELS.EHZ.AREA;

public class EmeraldHillZoneArea : Area
{
  private const string TileSetResourceKey = "//TILESET";
  private const string MapResourceKey = "//MAP";
  private const string BindingResourceKey = "//BINDING";
  private const string BossObjectResourceKey = "SONICORCA/OBJECTS/DRILLEGGMAN";
  private static IReadOnlyList<string> AnimalResourceKeys = (IReadOnlyList<string>) new string[2]
  {
    "SONICORCA/OBJECTS/FLICKY",
    "SONICORCA/OBJECTS/RICKY"
  };
  private const string Act1MusicResourceKey = "SONICORCA/MUSIC/LEVELS/EHZ/COOPERATIVE/ACT1";
  private const string Act2MusicResourceKey = "SONICORCA/MUSIC/LEVELS/EHZ/COOPERATIVE/ACT2";
  private const string BossMusicResourceKey = "SONICORCA/MUSIC/BOSS/S2";
  private const string Act1FastMusicResourceKey = "SONICORCA/MUSIC/LEVELS/EHZ/COOPERATIVE/ACT1/FAST";
  private const string Act2FastMusicResourceKey = "SONICORCA/MUSIC/LEVELS/EHZ/COOPERATIVE/ACT2/FAST";
  private const string WaterfallResourceKey = "SONICORCA/SOUND/WATERFALL";
  public static IReadOnlyList<string> AreaDependencies = (IReadOnlyList<string>) ((IEnumerable<string>) new string[10]
  {
    "//TILESET",
    "//MAP",
    "//BINDING",
    "SONICORCA/OBJECTS/DRILLEGGMAN",
    "SONICORCA/MUSIC/LEVELS/EHZ/COOPERATIVE/ACT1",
    "SONICORCA/MUSIC/LEVELS/EHZ/COOPERATIVE/ACT2",
    "SONICORCA/MUSIC/BOSS/S2",
    "SONICORCA/MUSIC/LEVELS/EHZ/COOPERATIVE/ACT1/FAST",
    "SONICORCA/MUSIC/LEVELS/EHZ/COOPERATIVE/ACT2/FAST",
    "SONICORCA/SOUND/WATERFALL"
  }).Concat<string>((IEnumerable<string>) EmeraldHillZoneArea.AnimalResourceKeys).ToArray<string>();
  private SonicOrcaGameContext _gameContext;
  private Level _level;
  private int _state;
  private int _stateTimer;
  private BossObject _bossObject;
  private bool _bossDefeated;
  private int _sunFallTimer;
  private Rectanglei[] _waterfallRects;
  private SampleInstance _waterfallSampleInstance;

  public override IEnumerable<KeyValuePair<string, object>> StateVariables
  {
    get
    {
      return (IEnumerable<KeyValuePair<string, object>>) new KeyValuePair<string, object>[2]
      {
        new KeyValuePair<string, object>("STATE", (object) this._state),
        new KeyValuePair<string, object>("BOSS DEFEATED", (object) this._bossDefeated)
      };
    }
  }

  public EmeraldHillZoneArea()
    : base((IEnumerable<string>) EmeraldHillZoneArea.AreaDependencies)
  {
  }

  public override void Dispose() => this._waterfallSampleInstance?.Dispose();

  public override void Prepare(Level level, LevelPrepareSettings settings)
  {
    this._gameContext = level.GameContext;
    this._level = level;
    this._level.Name = "Emerald Hill";
    this._level.ShowAsZone = true;
    this._level.ShowAsAct = true;
    this._level.Scheme = LevelScheme.S2;
    this._level.AnimalResourceKeys = (IReadOnlyCollection<string>) ((IEnumerable<string>) EmeraldHillZoneArea.AnimalResourceKeys).Select<string, string>((Func<string, string>) (x => this.GetAbsolutePath(x))).ToArray<string>();
    if (!settings.Seamless)
    {
      this._level.TileSet = this._gameContext.ResourceTree.GetLoadedResource<TileSet>((ILoadedResource) this, "//TILESET");
      this._level.LoadMap(this._gameContext.ResourceTree.GetLoadedResource<LevelMap>((ILoadedResource) this, "//MAP"));
      this._level.LoadBinding(this._gameContext.ResourceTree.GetLoadedResource<LevelBinding>((ILoadedResource) this, "//BINDING"));
      this.PrepareWaterfalls();
    }
    if (settings.Act == 1)
      this.PrepareAct1();
    else if (settings.Act == 2)
      this.PrepareAct2(settings.Seamless);
    settings.StartPath = 0;
  }

  private void PrepareAct1()
  {
    this._level.LevelMusic = "SONICORCA/MUSIC/LEVELS/EHZ/COOPERATIVE/ACT1";
    this._level.CurrentAct = 1;
    this._level.SetStartPosition("startpos_stk_1");
    this._level.Bounds = this.GetAct1Bounds();
    this._level.RingsPerfectTarget = this._level.ObjectManager.ObjectEntryTable.GetRingCountInRegion(this._level.Bounds);
    this._level.SoundManager.SetJingle(JingleType.SpeedShoes, "SONICORCA/MUSIC/LEVELS/EHZ/COOPERATIVE/ACT1/FAST");
  }

  private void PrepareAct2(bool seamless)
  {
    this._level.SoundManager.SetJingle(JingleType.SpeedShoes, "SONICORCA/MUSIC/LEVELS/EHZ/COOPERATIVE/ACT2/FAST");
    this._level.LevelMusic = "SONICORCA/MUSIC/LEVELS/EHZ/COOPERATIVE/ACT2";
    this._level.CurrentAct = 2;
    this._level.SetStartPosition("startpos_stk_2");
    Rectanglei act2Bounds = this.GetAct2Bounds() with
    {
      Right = this._level.GetMarker("boss_right").Position.X
    };
    this._level.RingsPerfectTarget = this._level.ObjectManager.ObjectEntryTable.GetRingCountInRegion(act2Bounds);
    if (seamless)
      this.ExtendSeamlessLevelBounds(this._level, act2Bounds);
    else
      this._level.Bounds = act2Bounds;
    this._state = 0;
    this._bossDefeated = false;
  }

  private void PrepareWaterfalls()
  {
    this._waterfallSampleInstance = new SampleInstance(this._gameContext, this._gameContext.ResourceTree.GetLoadedResource<SampleInfo>("SONICORCA/SOUND/WATERFALL"));
    this._waterfallRects = this.GetWaterfalls(this._level.Map.Layers[10]).AsArray<Rectanglei>();
    this._level.Map.Layers[8].WaterfallEffects = (IEnumerable<Rectanglei>) this._waterfallRects;
  }

  private void UpdateWaterfallSounds()
  {
    Vector2i centre = (Vector2i) this._level.Camera.Bounds.Centre;
    Rectanglei rectanglei = new Rectanglei();
    int num1 = int.MaxValue;
    foreach (Rectanglei waterfallRect in this._waterfallRects)
    {
      int num2 = centre.X - waterfallRect.Centre.X * 64 /*0x40*/;
      if (Math.Abs(num2) < Math.Abs(num1))
      {
        num1 = num2;
        rectanglei = waterfallRect;
      }
    }
    int num3 = Math.Min(Math.Abs(centre.Y - rectanglei.Top * 64 /*0x40*/), Math.Abs(centre.Y - rectanglei.Bottom * 64 /*0x40*/));
    if (!this._level.StateFlags.HasFlag((Enum) LevelStateFlags.Paused) && !this._level.Player.Protagonist.IsDead && Math.Abs(num1) < 1000 && num3 < 1400)
    {
      this._waterfallSampleInstance.Volume = 0.75 * Math.Min(1.0 - (double) Math.Abs(num1) / 1000.0, 1.0 - (double) num3 / 1400.0);
      this._waterfallSampleInstance.Pan = (double) num1 / 1000.0;
      this._waterfallSampleInstance.Play();
    }
    else
      this._waterfallSampleInstance.Stop();
  }

  public override void OnPause() => this._waterfallSampleInstance?.Stop();

  public override void OnUpdate()
  {
    this.UpdateInsideLighting();
    this.UpdateWaterfallSounds();
    if (this._level.CurrentAct == 1)
      return;
    this._level.FinishOnCompleteLevel = false;
    switch (this._state)
    {
      case 0:
        double left = this._level.Camera.Bounds.Left;
        Vector2i position = this._level.GetMarker("boss_oneway").Position;
        double x1 = (double) position.X;
        if (left < x1)
          break;
        Rectanglei bounds1 = this._level.Bounds;
        ref Rectanglei local1 = ref bounds1;
        position = this._level.GetMarker("boss_oneway").Position;
        int x2 = position.X;
        local1.Left = x2;
        ref Rectanglei local2 = ref bounds1;
        position = this._level.GetMarker("boss_bottom").Position;
        int y = position.Y;
        local2.Bottom = y;
        this._level.ScrollBoundsTo(bounds1, 2);
        ++this._state;
        break;
      case 1:
        if (this._level.IsScrollingBounds || this._level.Camera.Bounds.Left < (double) this._level.GetMarker("boss_left").Position.X)
          break;
        this._level.Bounds = this.GetBossBounds();
        this._level.SoundManager.CrossFadeMusic(this.GetAbsolutePath("SONICORCA/MUSIC/BOSS/S2"));
        this._stateTimer = 90;
        ++this._state;
        break;
      case 2:
        --this._stateTimer;
        if (this._stateTimer > 0)
          break;
        this.SpawnBoss();
        ++this._state;
        break;
      case 3:
        if (!this._bossDefeated && this._bossObject.Defeated)
        {
          this._bossDefeated = true;
          this._level.SoundManager.PlayMusic(this.GetAbsolutePath("SONICORCA/MUSIC/LEVELS/EHZ/COOPERATIVE/ACT2"));
        }
        if (!this._bossObject.Fleeing)
          break;
        ++this._state;
        break;
      case 4:
        Rectangle bounds2 = (Rectangle) this._level.Bounds;
        ref Rectangle local3 = ref bounds2;
        double val1 = bounds2.Right + 8.0;
        Rectanglei rectanglei = this.GetAct2Bounds();
        double right1 = (double) rectanglei.Right;
        double num = Math.Min(val1, right1);
        local3.Right = num;
        bounds2.Left = Math.Max(bounds2.Left, this._level.Camera.Bounds.Left);
        this._level.Bounds = (Rectanglei) bounds2;
        if (!this._level.StateFlags.HasFlag((Enum) LevelStateFlags.StageCompleted))
          break;
        ++this._state;
        bounds2 = (Rectangle) this._level.Bounds;
        this._level.Camera.Limits = (Rectanglei) bounds2;
        ref Rectangle local4 = ref bounds2;
        rectanglei = this._level.Map.Bounds;
        double right2 = (double) rectanglei.Right;
        local4.Right = right2;
        this._level.Bounds = (Rectanglei) bounds2;
        break;
      case 5:
        this._level.StateFlags &= ~LevelStateFlags.AllowCharacterControl;
        CharacterIntelligence.SimpleMoveRightJumpObsticals(this._level.Player.Protagonist);
        if (this._level.Player.Sidekick != null)
          CharacterIntelligence.SimpleMoveRightJumpObsticals(this._level.Player.Sidekick);
        if (this._level.Player.Protagonist.Position.X <= this._level.Bounds.Right - 128 /*0x80*/)
          break;
        ++this._state;
        this._sunFallTimer = 0;
        break;
      case 6:
        CharacterIntelligence.SimpleMoveRightJumpObsticals(this._level.Player.Protagonist);
        if (this._level.Player.Sidekick != null)
          CharacterIntelligence.SimpleMoveRightJumpObsticals(this._level.Player.Sidekick);
        ++this._sunFallTimer;
        if (this._sunFallTimer >= 120)
          this._level.NightMode = Math.Min(this._level.NightMode + 1.0 / 160.0, 1.0);
        if (this._sunFallTimer < 380)
          break;
        this._level.FadeOut(LevelState.StageCompleted);
        ++this._state;
        break;
      case 7:
        CharacterIntelligence.SimpleMoveRightJumpObsticals(this._level.Player.Protagonist);
        if (this._level.Player.Sidekick == null)
          break;
        CharacterIntelligence.SimpleMoveRightJumpObsticals(this._level.Player.Sidekick);
        break;
    }
  }

  private void UpdateInsideLighting()
  {
    foreach (ICharacter character in this._level.ObjectManager.Characters)
    {
      float num1 = 0.0f;
      foreach (LevelMarker levelMarker in this._level.GetMarkersWithTag("inside"))
      {
        Rectanglei rectanglei = levelMarker.Bounds;
        rectanglei = rectanglei.Inflate(new Vector2i(-32, -32));
        Vector2i position = character.Position;
        int val1;
        if (position.X < rectanglei.X + rectanglei.Width / 2)
        {
          position = character.Position;
          val1 = position.X - rectanglei.Left;
        }
        else
        {
          int right = rectanglei.Right;
          position = character.Position;
          int x = position.X;
          val1 = right - x;
        }
        position = character.Position;
        int num2;
        if (position.Y < rectanglei.Y + rectanglei.Height / 2)
        {
          position = character.Position;
          num2 = position.Y - rectanglei.Top;
        }
        else
        {
          int bottom = rectanglei.Bottom;
          position = character.Position;
          int y = position.Y;
          num2 = bottom - y;
        }
        int val2 = num2;
        int num3 = Math.Min(val1, val2);
        if (num3 >= -64)
        {
          num1 = (float) (MathX.Clamp(0.0, (double) (num3 + 64 /*0x40*/) / 64.0, 1.0) * -0.41);
          break;
        }
      }
      character.Brightness = num1;
    }
  }

  private void SpawnBoss()
  {
    Rectanglei bossBounds = this.GetBossBounds();
    LevelMarker marker = this._level.GetMarker("boss_position");
    this._bossObject = this._level.ObjectManager.AddObject(new ObjectPlacement(this.GetAbsolutePath("SONICORCA/OBJECTS/DRILLEGGMAN"), this._level.Map.Layers.IndexOf(marker.Layer), marker.Position, (object) new
    {
      LeftEdge = bossBounds.Left,
      RightEdge = bossBounds.Right
    })) as BossObject;
  }

  private Rectanglei GetAct1Bounds()
  {
    Vector2i position1 = this._level.GetMarker("bounds_1_lt").Position;
    Vector2i position2 = this._level.GetMarker("bounds_1_rb").Position;
    return Rectanglei.FromLTRB(position1.X, position1.Y, position2.X, position2.Y);
  }

  private Rectanglei GetAct2Bounds()
  {
    Vector2i position1 = this._level.GetMarker("bounds_2_lt").Position;
    Vector2i position2 = this._level.GetMarker("bounds_2_rb").Position;
    return Rectanglei.FromLTRB(position1.X, position1.Y, position2.X, position2.Y);
  }

  private Rectanglei GetBossBounds()
  {
    return Rectanglei.FromLTRB(this._level.GetMarker("boss_left").Position.X, this._level.Bounds.Top, this._level.GetMarker("boss_right").Position.X, this._level.GetMarker("boss_bottom").Position.Y);
  }

  private IEnumerable<Rectanglei> GetWaterfalls(LevelLayer layer)
  {
    List<Rectanglei> rects = new List<Rectanglei>();
    for (int x = 0; x < layer.Columns; ++x)
    {
      Rectanglei currentRect = new Rectanglei(x, 0, 1, 0);
      for (int index = 0; index < layer.Rows; ++index)
      {
        if (this.IsWaterfallTopTile(layer.Tiles[x, index]) && currentRect.Height != 0)
        {
          this.AggregateWaterfalls((IList<Rectanglei>) rects, currentRect);
          currentRect.Y = index;
          currentRect.Height = 1;
        }
        else if (this.IsWaterfallTile(layer.Tiles[x, index]))
        {
          if (currentRect.Height == 0)
          {
            currentRect.Y = index;
            currentRect.Height = 1;
          }
          else
            ++currentRect.Height;
        }
        else if (currentRect.Height != 0)
        {
          this.AggregateWaterfalls((IList<Rectanglei>) rects, currentRect);
          currentRect.Y = 0;
          currentRect.Height = 0;
        }
      }
      if (currentRect.Height != 0)
        this.AggregateWaterfalls((IList<Rectanglei>) rects, currentRect);
    }
    return (IEnumerable<Rectanglei>) rects;
  }

  private void AggregateWaterfalls(IList<Rectanglei> rects, Rectanglei currentRect)
  {
    bool flag = false;
    for (int index = 0; index < rects.Count; ++index)
    {
      Rectanglei rect = rects[index];
      if (rect.Right == currentRect.X && rect.Y == currentRect.Y && rect.Height == currentRect.Height)
      {
        ++rect.Width;
        rects[index] = rect;
        flag = true;
      }
    }
    if (flag)
      return;
    rects.Add(currentRect);
  }

  private bool IsWaterfallTile(int tile)
  {
    int num = tile & 4095 /*0x0FFF*/;
    return num >= 780 && num <= 803;
  }

  private bool IsWaterfallTopTile(int tile)
  {
    int num = tile & 4095 /*0x0FFF*/;
    return num >= 792 && num <= 795;
  }
}
