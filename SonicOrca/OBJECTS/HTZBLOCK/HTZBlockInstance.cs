// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.HTZBLOCK.HTZBlockInstance
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Collision;
using SonicOrca.Core.Objects.Base;
using SonicOrca.Extensions;
using SonicOrca.Geometry;
using SonicOrca.Graphics;

namespace SONICORCA.OBJECTS.HTZBLOCK
{

    public class HTZBlockInstance : Platform
    {
      private AnimationInstance _animation;

      public HTZBlockInstance()
      {
        this.DesignBounds = new Rectanglei((int) sbyte.MinValue, -192, 256 /*0x0100*/, 384);
      }

      protected override void OnStart()
      {
        base.OnStart();
        this._animation = new AnimationInstance(this.ResourceTree, this.Type.GetAbsolutePath("/ANIGROUP"));
        this.CollisionVectors = CollisionVector.FromRectangle((ActiveObject) this, new Rectanglei((int) sbyte.MinValue, -164, 256 /*0x0100*/, 356));
        this.CollisionVectors[0].Id = 1;
        this.CollisionVectors[2].Id = 1;
        this.CollisionVectors[3].Id = 1;
        this.CollisionVectors[1].Flags = CollisionFlags.Conveyor;
      }

      protected override void OnAnimate() => this._animation.Animate();

      protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
      {
        renderer.GetObjectRenderer().Render(this._animation);
      }
    }
}
