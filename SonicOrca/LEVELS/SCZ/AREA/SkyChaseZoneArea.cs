﻿// Decompiled with JetBrains decompiler
// Type: SONICORCA.LEVELS.SCZ.AREA.SkyChaseZoneArea
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca;
using SonicOrca.Core;
using SonicOrca.Resources;
using System.Collections.Generic;
using System.Linq;

namespace SONICORCA.LEVELS.SCZ.AREA
{

    public class SkyChaseZoneArea : Area
    {
        private const string TileSetResourceKey = "//TILESET";
        private const string MapResourceKey = "//MAP";
        private const string BindingResourceKey = "//BINDING";
        private const string LevelMusicResourceKey = "SONICORCA/MUSIC/LEVELS/SCZ";
        public static IReadOnlyList<string> AreaDependencies = (IReadOnlyList<string>)((IEnumerable<string>)new string[4]
        {
    "//TILESET",
    "//MAP",
    "//BINDING",
    "SONICORCA/MUSIC/LEVELS/SCZ"
        }).ToArray<string>();
        private SonicOrcaGameContext _gameContext;
        private ResourceTree _resourceTree;
        private Level _level;

        public SkyChaseZoneArea()
          : base((IEnumerable<string>)SkyChaseZoneArea.AreaDependencies)
        {
        }

        public override void Prepare(Level level, LevelPrepareSettings settings)
        {
            this._gameContext = level.GameContext;
            this._resourceTree = this._gameContext.ResourceTree;
            this._level = level;
            this._level.Name = "Sky Chase";
            this._level.ShowAsZone = true;
            this._level.ShowAsAct = true;
            this._level.Scheme = LevelScheme.S2;
            this._level.LevelMusic = "SONICORCA/MUSIC/LEVELS/SCZ";
            if (!settings.Seamless)
            {
                this._level.TileSet = this._resourceTree.GetLoadedResource<TileSet>((ILoadedResource)this, "//TILESET");
                this._level.LoadMap(this._resourceTree.GetLoadedResource<LevelMap>((ILoadedResource)this, "//MAP"));
                this._level.LoadBinding(this._resourceTree.GetLoadedResource<LevelBinding>((ILoadedResource)this, "//BINDING"));
            }
            this._level.Bounds = this._level.Map.Bounds;
        }
    }
}