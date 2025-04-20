// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.POINTS.PointsInstance
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Objects.Metadata;
using SonicOrca.Geometry;
using SonicOrca.Graphics;
using SonicOrca.Resources;

namespace SONICORCA.OBJECTS.POINTS;

public class PointsInstance : ActiveObject
{
  private Font _font;
  private int _value;
  private double _velocityY;

  [StateVariable]
  public int Value
  {
    get => this._value;
    set => this._value = value;
  }

  protected override void OnStart()
  {
    this._font = this.Level.GameContext.ResourceTree.GetLoadedResource<Font>((ILoadedResource) this.Type, "SONICORCA/FONTS/POINTS");
    this._velocityY = -12.0;
    this.Priority = 2304 /*0x0900*/;
  }

  protected override void OnUpdate()
  {
    if (this._velocityY >= 0.0)
    {
      this.FinishForever();
    }
    else
    {
      this.PositionPrecise = this.PositionPrecise + new Vector2(0.0, this._velocityY);
      this._velocityY += 0.375;
    }
  }

  protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
  {
    renderer.GetFontRenderer().RenderStringWithShadow(this._value.ToString(), new Rectangle(), FontAlignment.Centre, this._font, Colours.White);
  }
}
