// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.CPZCHEMICALFALL.CPZChemicalFallInstance
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Objects.Base;
using SonicOrca.Extensions;
using SonicOrca.Geometry;
using SonicOrca.Graphics;
using System;
using System.Collections.Generic;

#nullable disable
namespace SONICORCA.OBJECTS.CPZCHEMICALFALL;

public class CPZChemicalFallInstance : ActiveObject
{
  private const int AnimationCorner = 0;
  private const int AnimationVertical = 1;
  private const int AnimationDrop = 2;
  private AnimationInstance _animationCorner;
  private AnimationInstance _animationVertical;
  private int _bottom;

  protected override void OnStart()
  {
    AnimationGroup loadedResource = this.ResourceTree.GetLoadedResource<AnimationGroup>(this.Type.GetAbsolutePath("/ANIGROUP"));
    this._animationCorner = new AnimationInstance(loadedResource);
    this._animationVertical = new AnimationInstance(loadedResource, 1);
    this.DesignBounds = new Rectanglei(-32, -32, 64 /*0x40*/, 128 /*0x80*/);
    this.Priority = -64;
  }

  protected override void OnUpdateEditor() => this.UpdateBottom();

  protected override void OnUpdate()
  {
    this.UpdateBottom();
    this.CreateDrops();
  }

  protected override void OnAnimate()
  {
    this._animationCorner.Animate();
    this._animationVertical.Animate();
  }

  protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
  {
    IObjectRenderer objectRenderer = renderer.GetObjectRenderer();
    objectRenderer.Render(this._animationCorner, new Vector2(0.0, 0.0));
    objectRenderer.Texture = this._animationVertical.CurrentTexture;
    int y = this._animationCorner.CurrentFrame.Source.Height / 2;
    Rectanglei source = this._animationVertical.CurrentFrame.Source;
    Rectanglei destination = new Rectanglei(-source.Width / 2, y, source.Width, source.Height);
    for (int index = this._bottom - this.Position.Y; destination.Y < index; destination.Y += 64 /*0x40*/)
    {
      int num = destination.Bottom - index;
      source.Height -= num;
      destination.Height -= num;
      objectRenderer.Render((Rectangle) source, (Rectangle) destination);
    }
  }

  private void UpdateBottom()
  {
    int y = this.Position.Y;
    int num1 = this.Position.X - 32 /*0x20*/;
    int num2 = this.Position.Y + 32 /*0x20*/;
    int val1 = this.Level.Map.Bounds.Height;
    foreach (Rectanglei waterArea in (IEnumerable<Rectanglei>) this.Level.WaterManager.WaterAreas)
    {
      if (waterArea.Left <= num2 && waterArea.Right >= num1 && waterArea.Bottom >= y)
        val1 = Math.Min(val1, waterArea.Top);
    }
    this._bottom = val1;
  }

  private void CreateDrops()
  {
    if (this.Level.Ticks % 30 != 0)
      return;
    Random random = this.Level.Random;
    int y = this.Position.Y + 64 /*0x40*/;
    int bottom = this._bottom;
    int num = 30;
    while (y < bottom)
    {
      int x = this.Position.X + num;
      if (random.Next(0, 4) == 0)
        this.CreateDrop(x, y);
      y += 32 /*0x20*/;
      num *= -1;
    }
  }

  private void CreateDrop(int x, int y)
  {
    ObjectManager objectManager = this.Level.ObjectManager;
    Rectanglei rect = new Rectanglei(x - 16 /*0x10*/, y - 16 /*0x10*/, 32 /*0x20*/, 32 /*0x20*/);
    if (!objectManager.IsInLifetimeArea(rect))
      return;
    objectManager.AddSubObject<CPZChemicalFallInstance.Drop>((ActiveObject) this).Position = new Vector2i(x, y);
  }

  private class Drop : ParticleObject
  {
    public Drop()
      : base("/ANIGROUP", 2)
    {
    }
  }
}
