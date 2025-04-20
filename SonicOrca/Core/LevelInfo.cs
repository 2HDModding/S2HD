// Decompiled with JetBrains decompiler
// Type: SonicOrca.Core.LevelInfo
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

#nullable disable
namespace SonicOrca.Core;

internal class LevelInfo
{
  public int GameIndex { get; }

  public string Mnemonic { get; }

  public string Name { get; }

  public int Acts { get; }

  public bool Unreleased { get; }

  public LevelInfo(int gameIndex, string mnemonic, string name, int acts, bool unreleased = false)
  {
    this.GameIndex = gameIndex;
    this.Mnemonic = mnemonic;
    this.Name = name;
    this.Acts = acts;
    this.Unreleased = unreleased;
  }
}
