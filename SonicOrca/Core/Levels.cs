// Decompiled with JetBrains decompiler
// Type: SonicOrca.Core.Levels
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace SonicOrca.Core
{

    internal static class Levels
    {
        public const int Sonic1 = 1;
        public const int Sonic2 = 2;
        public const int Sonic3 = 3;
        public const int SonicSK = 4;
        public static readonly IReadOnlyList<LevelInfo> LevelList = (IReadOnlyList<LevelInfo>)new LevelInfo[11]
        {
            new LevelInfo(2, "ehz", "Emerald Hill Zone", 2),
            new LevelInfo(2, "cpz", "Chemical Plant Zone", 2),
            new LevelInfo(2, "arz", "Aquatic Ruin Zone", 2, true),
            new LevelInfo(2, "cnz", "Casino Night Zone", 2, true),
            new LevelInfo(2, "htz", "Hill Top Zone", 2),
            new LevelInfo(2, "mcz", "Mystic Cave Zone", 2, true),
            new LevelInfo(2, "ooz", "Oil Ocean Zone", 2, true),
            new LevelInfo(2, "mtz", "Metropolis Zone", 3, true),
            new LevelInfo(2, "scz", "Sky Chase Zone", 1, true),
            new LevelInfo(2, "wfz", "Wing Fortress Zone", 1, true),
            new LevelInfo(2, "dez", "Death Egg Zone", 1, true)
        };

        public static string GetAreaResourceKey(string mnemonic)
        {
            return ((IEnumerable<LevelInfo>)Levels.LevelList).Where<LevelInfo>((Func<LevelInfo, bool>)(x => x.GameIndex == 2)).FirstOrDefault<LevelInfo>((Func<LevelInfo, bool>)(x => x.Mnemonic.Equals(mnemonic, StringComparison.OrdinalIgnoreCase))) != null ? $"SONICORCA/LEVELS/{mnemonic.ToUpper()}/AREA" : (string)null;
        }

        public static int GetLevelIndex(string mnemonic)
        {
            int levelIndex = 1;
            foreach (LevelInfo level in (IEnumerable<LevelInfo>)Levels.LevelList)
            {
                if (level.GameIndex == 2)
                {
                    if (level.Mnemonic == mnemonic)
                        return levelIndex;
                    ++levelIndex;
                }
            }
            return -1;
        }
    }
}