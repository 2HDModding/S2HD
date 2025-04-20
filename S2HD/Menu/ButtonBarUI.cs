// Decompiled with JetBrains decompiler
// Type: S2HD.Menu.ButtonBarUI
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Geometry;
using SonicOrca.Graphics;
using System.Collections.Immutable;

namespace S2HD.Menu;

internal class ButtonBarUI
{
  private readonly IControlResources _controlResources;
  private Rectanglei _bounds;
  private ImmutableArray<ButtonBarItem> _items;
  private bool _layoutDirty = true;
  private int[] _itemX;

  public Rectanglei Bounds
  {
    get => this._bounds;
    set
    {
      this._bounds = value;
      this._layoutDirty = true;
    }
  }

  public ImmutableArray<ButtonBarItem> Items
  {
    get => this._items;
    set
    {
      this._items = value;
      this._layoutDirty = true;
    }
  }

  public ButtonBarUI(IControlResources resources) => this._controlResources = resources;

  private void CalculateItemPositions(I2dRenderer g)
  {
    int length = this._items.Length;
    int num1 = 0;
    this._itemX = new int[length];
    for (int index = 0; index < length; ++index)
    {
      this._itemX[index] = num1;
      ButtonBarItem buttonBarItem = this._items[index];
      num1 = num1 + this.GetItemWidth(g, buttonBarItem) + 16 /*0x10*/;
    }
    int num2 = this.Bounds.Width - (num1 - 16 /*0x10*/);
    for (int index = 0; index < length; ++index)
      this._itemX[index] += num2;
  }

  private int GetItemWidth(I2dRenderer g, ButtonBarItem item)
  {
    int num = 0;
    if (this.GetButtonTexture(item) != null)
      num += 64 /*0x40*/;
    TextRenderInfo textRenderInfo = this.GetTextRenderInfo(new Rectanglei(), item.Text);
    return num + (int) g.MeasureText(textRenderInfo).Width;
  }

  public void Draw(Renderer renderer)
  {
    I2dRenderer g = renderer.Get2dRenderer();
    if (this._layoutDirty)
    {
      this._layoutDirty = false;
      this.CalculateItemPositions(g);
    }
    this.DrawStrip(g);
    int y = this.Bounds.Centre.Y;
    for (int index = 0; index < this._items.Length; ++index)
    {
      ButtonBarItem buttonBarItem = this._items[index];
      int x = this._itemX[index];
      this.DrawButton(g, buttonBarItem, x, y);
    }
  }

  private void DrawStrip(I2dRenderer g)
  {
    g.BlendMode = BlendMode.Alpha;
    g.RenderQuad(new Colour(0.2, 0.0, 0.0, 0.0), (Rectangle) this.Bounds);
  }

  private void DrawButton(I2dRenderer g, ButtonBarItem item, int x, int y)
  {
    x += 32 /*0x20*/;
    ITexture buttonTexture = this.GetButtonTexture(item);
    if (buttonTexture != null)
    {
      g.BlendMode = BlendMode.Alpha;
      g.Colour = Colours.White;
      g.RenderTexture(buttonTexture, (Vector2) new Vector2i(x, y));
      x += 32 /*0x20*/;
    }
    if (string.IsNullOrEmpty(item.Text))
      return;
    TextRenderInfo textRenderInfo = this.GetTextRenderInfo(new Rectanglei(x, y - this.Bounds.Height / 2, 0, this.Bounds.Height), item.Text);
    g.RenderText(textRenderInfo);
  }

  private ITexture GetButtonTexture(ButtonBarItem item)
  {
    switch (item.Button)
    {
      case GamePadButton.A:
        return this._controlResources.ButtonA;
      case GamePadButton.B:
        return this._controlResources.ButtonB;
      default:
        return (ITexture) null;
    }
  }

  private TextRenderInfo GetTextRenderInfo(Rectanglei bounds, string text)
  {
    return new TextRenderInfo()
    {
      Bounds = (Rectangle) bounds,
      Colour = Colours.White,
      Overlay = new int?(0),
      Alignment = FontAlignment.MiddleY,
      Font = this._controlResources.Font,
      SizeMultiplier = 0.75f,
      Text = text
    };
  }
}
