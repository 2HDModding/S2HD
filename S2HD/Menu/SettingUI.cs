// Decompiled with JetBrains decompiler
// Type: S2HD.Menu.SettingUI
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca;
using SonicOrca.Geometry;
using SonicOrca.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace S2HD.Menu;

internal class SettingUI
{
  private readonly int[] SliderWidths = new int[11]
  {
    0,
    45,
    59,
    73,
    89,
    105,
    121,
    139,
    159,
    181,
    223
  };
  private const char ArrowLeftChar = '◀';
  private const char ArrowRightChar = '▶';
  private readonly ISetting _setting;
  private readonly ISettingUIResources _resources;
  private string _name;
  private string[] _values;

  public Rectanglei Bounds { get; set; }

  public bool Highlighted { get; set; }

  public SettingUI(ISetting setting, ISettingUIResources resources)
  {
    this._setting = setting;
    this._resources = resources;
    this._name = setting.Name;
    if (!(setting is ISpinnerSetting))
      return;
    this._values = ((IEnumerable<string>) (setting as ISpinnerSetting).Values).ToArray<string>();
  }

  public void Draw(Renderer renderer)
  {
    this.DrawLeft(renderer);
    this.DrawRight(renderer);
  }

  private void DrawLeft(Renderer renderer)
  {
    renderer.GetFontRenderer().RenderStringWithShadow(this._name, (Rectangle) this.Bounds, FontAlignment.MiddleY, this._resources.Font, this.FontColour, new int?(this.FontOverlay));
  }

  private void DrawRight(Renderer renderer)
  {
    if (this._setting is IAudioSliderSetting)
    {
      this.DrawRightAudioSlider(renderer, this._setting as IAudioSliderSetting);
    }
    else
    {
      if (!(this._setting is ISpinnerSetting))
        return;
      this.DrawRightSpinner(renderer, this._setting as ISpinnerSetting);
    }
  }

  private void DrawRightAudioSlider(Renderer renderer, IAudioSliderSetting setting)
  {
    ITexture sliderEmptyTexture = this._resources.AudioSliderEmptyTexture;
    ITexture texture = this.Highlighted ? this._resources.AudioSliderGoldTexture : this._resources.AudioSliderSilverTexture;
    Rectanglei destination1 = new Rectanglei(0, 0, sliderEmptyTexture.Width, sliderEmptyTexture.Height)
    {
      X = this.Bounds.Right - sliderEmptyTexture.Width - 24
    };
    destination1.Y = this.Bounds.Centre.Y - destination1.Height / 2;
    I2dRenderer obj = renderer.Get2dRenderer();
    obj.RenderTexture(sliderEmptyTexture, (Rectangle) destination1);
    int sliderWidth = this.SliderWidths[MathX.Clamp(0, (int) Math.Floor(setting.Value * (double) this.SliderWidths.Length), this.SliderWidths.Length - 1)];
    Rectanglei destination2 = destination1 with
    {
      Width = sliderWidth
    };
    obj.RenderTexture(texture, new Rectangle(0.0, 0.0, (double) destination2.Width, (double) destination2.Height), (Rectangle) destination2);
    IFontRenderer fontRenderer = renderer.GetFontRenderer();
    Rectanglei bounds = this.Bounds;
    fontRenderer.RenderStringWithShadow('▶'.ToString(), (Rectangle) bounds, FontAlignment.Right | FontAlignment.MiddleY, this._resources.Font, this.FontColour, new int?(this.FontOverlay));
    bounds.Right = destination1.X + 12;
    fontRenderer.RenderStringWithShadow('◀'.ToString(), (Rectangle) bounds, FontAlignment.Right | FontAlignment.MiddleY, this._resources.Font, this.FontColour, new int?(this.FontOverlay));
  }

  private void DrawRightSpinner(Renderer renderer, ISpinnerSetting setting)
  {
    string text = $"◀ {this._values[setting.SelectedIndex]} ▶";
    renderer.GetFontRenderer().RenderStringWithShadow(text, (Rectangle) this.Bounds, FontAlignment.Right | FontAlignment.MiddleY, this._resources.Font, this.FontColour, new int?(this.FontOverlay));
  }

  private Colour FontColour => !this.Highlighted ? Colour.FromOpacity(0.6) : Colours.White;

  private int FontOverlay => !this.Highlighted ? 0 : 1;
}
