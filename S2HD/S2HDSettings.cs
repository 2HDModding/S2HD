// Decompiled with JetBrains decompiler
// Type: S2HD.S2HDSettings
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca;
using SonicOrca.Audio;
using SonicOrca.Graphics;

#nullable disable
namespace S2HD;

internal class S2HDSettings : IAudioSettings, IVideoSettings
{
  private readonly AudioContext _audioContext;
  private readonly WindowContext _windowContext;
  private readonly IniConfiguration _config;
  private VideoMode _mode;
  private bool _enableShadows;
  private bool _enableWaterEffects;
  private bool _enableHeatEffects;

  public double MusicVolume
  {
    get => this._audioContext.MusicVolume;
    set
    {
      this._audioContext.MusicVolume = value;
      this._config.SetProperty("audio", "music_volume", value.ToString().ToLower());
      this._config.Save();
    }
  }

  public double SoundVolume
  {
    get => this._audioContext.SoundVolume;
    set
    {
      this._audioContext.SoundVolume = value;
      this._config.SetProperty("audio", "sound_volume", value.ToString().ToLower());
      this._config.Save();
    }
  }

  public VideoMode Mode
  {
    get => this._mode;
    set
    {
      this._mode = value;
      this._config.SetProperty("video", "fullscreen", ((int) this.Mode).ToString().ToLower());
      this._config.Save();
    }
  }

  public Resolution Resolution { get; set; }

  public bool EnableShadows
  {
    get => this._enableShadows;
    set
    {
      this._enableShadows = value;
      this._config.SetProperty("graphics", "shadows", value.ToString().ToLower());
      this._config.Save();
    }
  }

  public bool EnableWaterEffects
  {
    get => this._enableWaterEffects;
    set
    {
      this._enableWaterEffects = value;
      this._config.SetProperty("graphics", "water_effects", value.ToString().ToLower());
      this._config.Save();
    }
  }

  public bool EnableHeatEffects
  {
    get => this._enableHeatEffects;
    set
    {
      this._enableHeatEffects = value;
      this._config.SetProperty("graphics", "heat_effects", value.ToString().ToLower());
      this._config.Save();
    }
  }

  public S2HDSettings(
    IniConfiguration config,
    AudioContext audioContext,
    WindowContext windowContext)
  {
    this._config = config;
    this._audioContext = audioContext;
    this._windowContext = windowContext;
    this._audioContext.MusicVolume = config.GetPropertyDouble("audio", "music_volume", 1.0);
    this._audioContext.SoundVolume = config.GetPropertyDouble("audio", "sound_volume", 1.0);
    this._mode = VideoMode.Windowed;
    int result;
    if (int.TryParse(config.GetProperty("video", "fullscreen", "0"), out result))
      this._mode = (VideoMode) result;
    this.Resolution = new Resolution(1920, 1080);
    this._enableShadows = config.GetPropertyBoolean("graphics", "shadows", true);
    this._enableWaterEffects = config.GetPropertyBoolean("graphics", "water_effects", true);
    this._enableHeatEffects = config.GetPropertyBoolean("graphics", "heat_effects");
  }

  public void Apply() => this._windowContext.Mode = this.Mode;
}
