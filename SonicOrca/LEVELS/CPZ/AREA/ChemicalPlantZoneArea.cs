// Decompiled with JetBrains decompiler
// Type: SONICORCA.LEVELS.CPZ.AREA.ChemicalPlantZoneArea
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca;
using SonicOrca.Core;
using SonicOrca.Core.Objects.Base;
using SonicOrca.Extensions;
using SonicOrca.Geometry;
using SonicOrca.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SONICORCA.LEVELS.CPZ.AREA
{

    public class ChemicalPlantZoneArea : Area
    {
        private const string TileSetResourceKey = "//TILESET";
        private const string MapResourceKey = "//MAP";
        private const string BindingResourceKey = "//BINDING";
        private const string BossObjectResourceKey = "SONICORCA/OBJECTS/WATEREGGMAN";
        private static IReadOnlyList<string> AnimalResourceKeys = (IReadOnlyList<string>)new string[2]
        {
            "SONICORCA/OBJECTS/LOCKY",
            "SONICORCA/OBJECTS/POCKY"
        };
        private const string Act1MusicResourceKey = "SONICORCA/MUSIC/LEVELS/CPZ/ACT1";
        private const string Act2MusicResourceKey = "SONICORCA/MUSIC/LEVELS/CPZ/ACT2";
        private const string BossMusicResourceKey = "SONICORCA/MUSIC/BOSS/S2";
        private const string Act1FastMusicResourceKey = "SONICORCA/MUSIC/LEVELS/CPZ/ACT1/FAST";
        public static IReadOnlyList<string> AreaDependencies = (IReadOnlyList<string>)((IEnumerable<string>)new string[8]
        {
            "//TILESET",
            "//MAP",
            "//BINDING",
            "SONICORCA/MUSIC/LEVELS/CPZ/ACT1",
            "SONICORCA/MUSIC/LEVELS/CPZ/ACT2",
            "SONICORCA/MUSIC/BOSS/S2",
            "SONICORCA/MUSIC/LEVELS/CPZ/ACT1/FAST",
            "SONICORCA/OBJECTS/WATEREGGMAN"
        }).Concat<string>((IEnumerable<string>)ChemicalPlantZoneArea.AnimalResourceKeys).ToArray<string>();
        private SonicOrcaGameContext _gameContext;
        private ResourceTree _resourceTree;
        private Level _level;
        private int _state;
        private int _stateTimer;
        private bool _bossDefeated;
        private BossObject _bossObject;
        private bool _waterRaised;
        private int _waterHeight;
        private int _waterHeightOscillation;

        public ChemicalPlantZoneArea()
          : base((IEnumerable<string>)ChemicalPlantZoneArea.AreaDependencies)
        {
        }

        public override void Prepare(Level level, LevelPrepareSettings settings)
        {
            this._gameContext = level.GameContext;
            this._resourceTree = this._gameContext.ResourceTree;
            this._level = level;
            this._level.Name = "Chemical Plant";
            this._level.ShowAsZone = true;
            this._level.ShowAsAct = true;
            this._level.Scheme = LevelScheme.S2;
            this._level.AnimalResourceKeys = (IReadOnlyCollection<string>)((IEnumerable<string>)ChemicalPlantZoneArea.AnimalResourceKeys).Select<string, string>((Func<string, string>)(x => this.GetAbsolutePath(x))).ToArray<string>();
            if (!settings.Seamless)
            {
                this._level.TileSet = this._resourceTree.GetLoadedResource<TileSet>((ILoadedResource)this, "//TILESET");
                this._level.LoadMap(this._resourceTree.GetLoadedResource<LevelMap>((ILoadedResource)this, "//MAP"));
                this._level.LoadBinding(this._resourceTree.GetLoadedResource<LevelBinding>((ILoadedResource)this, "//BINDING"));
            }
            if (settings.Act == 1)
                this.PrepareAct1();
            else if (settings.Act == 2)
            {
                if (settings.Seamless)
                    this.SeamlessPrepareAct2();
                else
                    this.PrepareAct2();
            }
            settings.StartPath = 1;
        }

        private void PrepareAct1()
        {
            this._level.LevelMusic = "SONICORCA/MUSIC/LEVELS/CPZ/ACT1";
            this._level.CurrentAct = 1;
            this._level.SetStartPosition("startpos_stk_1");
            this._level.Bounds = this.GetAct1Bounds();
            this._level.RingsPerfectTarget = this._level.ObjectManager.ObjectEntryTable.GetRingCountInRegion(this._level.Bounds);
            this._level.SoundManager.SetJingle(JingleType.SpeedShoes, "SONICORCA/MUSIC/LEVELS/CPZ/ACT1/FAST");
        }

        private void PrepareAct2()
        {
            this._level.LevelMusic = "SONICORCA/MUSIC/LEVELS/CPZ/ACT2";
            this._level.CurrentAct = 2;
            this._level.SetStartPosition("startpos_stk_2");
            Rectanglei act2Bounds = this.GetAct2Bounds() with
            {
                Right = this._level.GetMarker("boss_right").Position.X
            };
            this._level.Bounds = act2Bounds;
            this._level.RingsPerfectTarget = this._level.ObjectManager.ObjectEntryTable.GetRingCountInRegion(act2Bounds);
            this._state = 0;
            this._bossDefeated = false;
            this.InitialiseWater();
        }

        private void SeamlessPrepareAct2()
        {
            this._level.LevelMusic = "SONICORCA/MUSIC/LEVELS/CPZ/ACT2";
            this._level.CurrentAct = 2;
            Rectanglei act2Bounds = this.GetAct2Bounds() with
            {
                Right = this._level.GetMarker("boss_right").Position.X
            };
            this._level.RingsPerfectTarget = this._level.ObjectManager.ObjectEntryTable.GetRingCountInRegion(act2Bounds);
            this.ExtendSeamlessLevelBounds(this._level, act2Bounds);
            this._state = 0;
            this._bossDefeated = false;
            this.InitialiseWater();
        }

        private void InitialiseWater()
        {
            this._waterHeightOscillation = 0;
            Rectanglei bounds = this._level.Map.Bounds;
            Rectanglei rectanglei = Rectanglei.FromLTRB(bounds.Left, 10304, bounds.Right, bounds.Bottom);
            if (this._level.GetPlayerStartPosition().X - 960 >= 71936)
            {
                this._waterHeight = 8256;
                this._waterRaised = true;
            }
            else
            {
                this._waterHeight = 10304;
                this._waterRaised = false;
            }
            rectanglei.Top = 8256;
            this._level.WaterManager.Enabled = true;
            this._level.WaterManager.WaterAreas.Clear();
            this._level.WaterManager.WaterAreas.Add(rectanglei);
        }

        private void UpdateWater()
        {
            Rectanglei waterArea = this._level.WaterManager.WaterAreas[0];
            int top = waterArea.Top;
            if (!this._waterRaised && this._level.Camera.Bounds.Left >= 71936.0)
            {
                this._waterRaised = true;
                this._waterHeight = top;
                this._waterHeightOscillation = 0;
            }
            int dest = this._waterRaised ? 8256 : 10304;
            int num1;
            if (this._waterHeight == dest)
            {
                ++this._waterHeightOscillation;
                double num2 = (double)this._waterHeightOscillation / 252.0;
                int waterHeight = this._waterHeight;
                double num3 = (1.0 - Math.Cos(num2 * (2.0 * Math.PI))) / 2.0;
                num1 = waterHeight + 0;
            }
            else
            {
                this._waterHeight = MathX.GoTowards(this._waterHeight, dest, 4);
                num1 = this._waterHeight;
            }
            waterArea.Top = num1;
            this._level.WaterManager.WaterAreas[0] = waterArea;
            this._level.WaterManager.HueTarget = 0.3;
            this._level.WaterManager.HueAmount = 0.8;
        }

        public override void OnUpdate()
        {
            if (!this._level.StateFlags.HasFlag((Enum)LevelStateFlags.Updating) || this._level.CurrentAct != 2)
                return;
            this.UpdateWater();
            Rectangle bounds1 = (Rectangle)this._level.Bounds;
            Rectanglei bounds2 = (Rectanglei)this._level.Camera.Bounds;
            switch (this._state)
            {
                case 0:
                    int x1 = this._level.GetMarker("boss_oneway").Position.X;
                    int y = this._level.GetMarker("boss_bottom").Position.Y;
                    if (bounds2.Left < x1)
                        break;
                    this._level.ScrollBoundsTo((Rectanglei)bounds1 with
                    {
                        Left = x1,
                        Bottom = y
                    }, 2);
                    ++this._state;
                    break;
                case 1:
                    int x2 = this._level.GetMarker("boss_left").Position.X;
                    if (this._level.IsScrollingBounds || bounds2.Left < x2)
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
                        this._level.SoundManager.PlayMusic(this.GetAbsolutePath("SONICORCA/MUSIC/LEVELS/CPZ/ACT2"));
                    }
                    if (!this._bossObject.Fleeing)
                        break;
                    ++this._state;
                    break;
                case 4:
                    Rectangle bounds3 = (Rectangle)this._level.Bounds;
                    bounds3.Right = Math.Min(bounds3.Right + 8.0, (double)this.GetAct2Bounds().Right);
                    bounds3.Left = Math.Max(bounds3.Left, this._level.Camera.Bounds.Left);
                    this._level.Bounds = (Rectanglei)bounds3;
                    if (!this._level.StateFlags.HasFlag((Enum)LevelStateFlags.StageCompleted))
                        break;
                    this._level.FadeOut(LevelState.StageCompleted);
                    ++this._state;
                    break;
            }
        }

        private void SpawnBoss()
        {
            LevelMarker marker1 = this._level.GetMarker("boss_position");
            LevelMarker marker2 = this._level.GetMarker("boss_fill_left");
            LevelMarker marker3 = this._level.GetMarker("boss_fill_right");
            Vector2i position = marker2.Position;
            int x1 = position.X;
            position = marker3.Position;
            int x2 = position.X;
            var state = new { LeftFillX = x1, RightFillX = x2 };
            this._bossObject = this._level.ObjectManager.AddObject(new ObjectPlacement(this.GetAbsolutePath("SONICORCA/OBJECTS/WATEREGGMAN"), this._level.Map.Layers.IndexOf(marker1.Layer), marker1.Position, (object)state)) as BossObject;
        }

        private Rectanglei GetAct1Bounds()
        {
            return Rectanglei.FromLTRB(0, 0, this._level.GetMarker("bounds_1_r").Position.X, 8192 /*0x2000*/);
        }

        private Rectanglei GetAct2Bounds()
        {
            int x = this._level.GetMarker("bounds_2_l").Position.X;
            Rectanglei bounds = this._level.Map.Bounds;
            int width = bounds.Width;
            bounds = this._level.Map.Bounds;
            int height = bounds.Height;
            return Rectanglei.FromLTRB(x, 3072 /*0x0C00*/, width, height);
        }

        private Rectanglei GetBossBounds()
        {
            return Rectanglei.FromLTRB(this._level.GetMarker("boss_left").Position.X, this._level.Bounds.Top, this._level.GetMarker("boss_right").Position.X, this._level.GetMarker("boss_bottom").Position.Y);
        }
    }
}