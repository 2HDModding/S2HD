// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.BUBBLE.BubbleInstance
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca;
using SonicOrca.Core;
using SonicOrca.Core.Collision;
using SonicOrca.Core.Objects;
using SonicOrca.Core.Objects.Metadata;
using SonicOrca.Extensions;
using SonicOrca.Geometry;
using SonicOrca.Graphics;
using System;
using System.Collections.Generic;

namespace SONICORCA.OBJECTS.BUBBLE {
  public class BubbleInstance : ActiveObject
  {
    private static IReadOnlyList<int> SizeRadius = (IReadOnlyList<int>) new int[3]
    {
      16 /*0x10*/,
      38,
      112 /*0x70*/
    };
    private AnimationInstance _animation;
    private Vector2 _velocity;
    private BubbleInstance.SizeType _size;
    private double _angle;
    private int _maxSize;

    [StateVariable]
    private BubbleInstance.SizeType Size
    {
      get => this._size;
      set => this._size = value;
    }

    protected override void OnStart()
    {
      this._animation = new AnimationInstance(this.ResourceTree, this.Type.GetAbsolutePath("/ANIGROUP"));
      this.Priority = 1024 /*0x0400*/;
      this._velocity = new Vector2(0.0, -17.0 / 32.0);
      this._angle = this.Level.Random.NextDouble() * (2.0 * Math.PI);
      this._maxSize = BubbleInstance.SizeRadius[(int) this._size];
      if (this._size != BubbleInstance.SizeType.Large)
        this._maxSize += this.Level.Random.Next(-4, 4);
      else
        this.CollisionRectangles = new CollisionRectangle[1]
        {
          new CollisionRectangle((ActiveObject) this, 0, -28, -28, 56, 56)
        };
    }

    protected override void OnUpdate()
    {
      this._angle = MathX.WrapRadians(this._angle + Math.PI / 64.0);
      this._velocity.X = Math.Sin(this._angle) * 0.5;
      this.PositionPrecise = this.PositionPrecise + this._velocity;
      this._size = (BubbleInstance.SizeType) Math.Min((int) (this._size + 1), this._maxSize);
      if (this.Level.WaterManager.IsUnderwater(this.Position))
        return;
      this.FinishForever();
    }

    protected override void OnCollision(CollisionEvent e)
    {
      if (this._size != BubbleInstance.SizeType.Large || e.ActiveObject.Type.Classification != ObjectClassification.Character)
        return;
      ICharacter activeObject = (ICharacter) e.ActiveObject;
      activeObject.Velocity = new Vector2();
      activeObject.InhaleOxygen();
      this.FinishForever();
    }

    protected override void OnAnimate() => this._animation.Animate();

    protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
    {
      double num = (double) this._size / (double) this._animation.CurrentTexture.Width;
      IObjectRenderer objectRenderer = renderer.GetObjectRenderer();
      using (objectRenderer.BeginMatixState())
      {
        objectRenderer.ModelMatrix *= Matrix4.CreateScale(num, num);
        objectRenderer.Render(this._animation);
      }
    }

    private enum SizeType
    {
      Small,
      Medium,
      Large,
    }
  }
}