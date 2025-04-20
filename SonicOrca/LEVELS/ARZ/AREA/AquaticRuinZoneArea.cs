// Decompiled with JetBrains decompiler
// Type: SONICORCA.LEVELS.ARZ.AREA.AquaticRuinZoneArea
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca;
using SonicOrca.Core;
using SonicOrca.Geometry;
using SonicOrca.Resources;
using System.Collections.Generic;
using System.Linq;

namespace SONICORCA.LEVELS.ARZ.AREA;

public class AquaticRuinZoneArea : Area
{
  private const string TileSetResourceKey = "//TILESET";
  private const string MapResourceKey = "//MAP";
  private const string BindingResourceKey = "//BINDING";
  private const string LevelMusicResourceKey = "SONICORCA/MUSIC/LEVELS/ARZ";
  public static IReadOnlyList<string> AreaDependencies = (IReadOnlyList<string>) ((IEnumerable<string>) new string[4]
  {
    "//TILESET",
    "//MAP",
    "//BINDING",
    "SONICORCA/MUSIC/LEVELS/ARZ"
  }).ToArray<string>();
  private SonicOrcaGameContext _gameContext;
  private ResourceTree _resourceTree;
  private Level _level;

  public AquaticRuinZoneArea()
    : base((IEnumerable<string>) AquaticRuinZoneArea.AreaDependencies)
  {
  }

  public override void Prepare(Level level, LevelPrepareSettings settings)
  {
    this._gameContext = level.GameContext;
    this._resourceTree = this._gameContext.ResourceTree;
    this._level = level;
    this._level.Name = "Aquatic Ruin";
    this._level.ShowAsZone = true;
    this._level.ShowAsAct = true;
    this._level.Scheme = LevelScheme.S2;
    this._level.LevelMusic = "SONICORCA/MUSIC/LEVELS/ARZ";
    if (!settings.Seamless)
    {
      this._level.TileSet = this._resourceTree.GetLoadedResource<TileSet>((ILoadedResource) this, "//TILESET");
      this._level.LoadMap(this._resourceTree.GetLoadedResource<LevelMap>((ILoadedResource) this, "//MAP"));
      this._level.LoadBinding(this._resourceTree.GetLoadedResource<LevelBinding>((ILoadedResource) this, "//BINDING"));
    }
    if (settings.Act == 1)
    {
      this.PrepareAct1();
    }
    else
    {
      if (settings.Act != 2)
        return;
      if (settings.Seamless)
        this.SeamlessPrepareAct2();
      else
        this.PrepareAct2();
    }
  }

  private void PrepareAct1()
  {
    this._level.CurrentAct = 1;
    this._level.SetStartPosition("startpos_stk_1");
    this._level.Bounds = this._level.Map.Bounds;
    this._level.WaterManager.WaterAreas.Add(new Rectanglei(0, 4096 /*0x1000*/, 65536 /*0x010000*/, 4096 /*0x1000*/));
  }

  private void PrepareAct2()
  {
    this._level.CurrentAct = 2;
    this._level.SetStartPosition("startpos_stk_2");
    this._level.Bounds = this._level.Map.Bounds;
  }

  private void SeamlessPrepareAct2()
  {
    this._level.CurrentAct = 2;
    this.ExtendSeamlessLevelBounds(this._level, this._level.Map.Bounds);
  }
}
