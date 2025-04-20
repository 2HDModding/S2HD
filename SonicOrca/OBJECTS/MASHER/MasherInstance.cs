// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.MASHER.MasherInstance
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Collision;
using SonicOrca.Core.Objects.Base;
using SonicOrca.Extensions;
using SonicOrca.Geometry;
using SonicOrca.Graphics;

namespace SONICORCA.OBJECTS.MASHER
{

    public class MasherInstance : Badnik
    {
      private const double InitialVelocity = -1.5625;
      private const double LaunchVelocity = -20.0;
      private const double Acceleration = 0.375;
      private const int AnimationNormal = 0;
      private const int AnimationMunching = 1;
      private AnimationInstance _animation;
      private double _initialY;
      private double _velocityY;

      public MasherInstance() => this.DesignBounds = new Rectanglei(-51, -76, 102, 152);

      protected override void OnStart()
      {
        base.OnStart();
        this._animation = new AnimationInstance(this.ResourceTree, this.Type.GetAbsolutePath("/ANIGROUP"));
        this.CollisionRectangles = new CollisionRectangle[1]
        {
          new CollisionRectangle((ActiveObject) this, 0, -48, -64, 96 /*0x60*/, 128 /*0x80*/)
        };
        this._initialY = (double) this.Position.Y;
        this._velocityY = -25.0 / 16.0;
      }

      protected override void OnUpdate()
      {
        base.OnUpdate();
        this.PositionPrecise = this.PositionPrecise + new Vector2(0.0, this._velocityY);
        this._velocityY += 0.375;
        if ((double) this.Position.Y <= this._initialY)
          return;
        this.PositionPrecise = new Vector2((double) this.Position.X, this._initialY);
        this._velocityY = -20.0;
      }

      protected override void OnAnimate()
      {
        this._animation.Index = this._velocityY >= 0.0 ? 0 : 1;
        this._animation.Animate();
      }

      protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
      {
        renderer.GetObjectRenderer().Render(this._animation);
      }
    }
}
