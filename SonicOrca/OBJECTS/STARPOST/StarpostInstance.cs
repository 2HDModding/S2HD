// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.STARPOST.StarpostInstance
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Collision;
using SonicOrca.Core.Objects;
using SonicOrca.Core.Objects.Metadata;
using SonicOrca.Extensions;
using SonicOrca.Geometry;
using SonicOrca.Graphics;
using SonicOrca.Resources;
using System;

namespace SONICORCA.OBJECTS.STARPOST
{

    public class StarpostInstance : ActiveObject
    {
      public const int AnimationStand = 0;
      public const int AnimationStarInactive = 1;
      public const int AnimationStarActive = 2;
      public const int AnimationStarActiveGlow = 3;
      private static double StarAxelRadius = 44.0;
      private static Vector2 StarAxelMidpoint = new Vector2(0.0, -52.0);
      private AnimationInstance _standAnimation;
      private AnimationInstance _starAnimation;
      private AnimationInstance _starGlowAnimation;
      private Vector2 _starPosition;
      private int _index;
      private bool _activated;
      private bool _glowing;
      private bool _starSpinning;
      private double _starAngle;

      [StateVariable]
      private int Index
      {
        get => this._index;
        set => this._index = value;
      }

      public StarpostInstance() => this.DesignBounds = new Rectanglei(-50, -150, 100, 300);

      protected override void OnStart()
      {
        AnimationGroup loadedResource = this.Level.GameContext.ResourceTree.GetLoadedResource<AnimationGroup>((ILoadedResource) this.Type, "/ANIGROUP");
        this._standAnimation = new AnimationInstance(loadedResource);
        this._starAnimation = new AnimationInstance(loadedResource);
        this._starGlowAnimation = new AnimationInstance(loadedResource);
        this._standAnimation.Index = 0;
        this._starAnimation.Index = 1;
        this._starGlowAnimation.Index = 3;
        this.CollisionRectangles = new CollisionRectangle[1]
        {
          new CollisionRectangle((ActiveObject) this, 0, -32, (int) sbyte.MinValue, 64 /*0x40*/, 256 /*0x0100*/)
        };
        this._activated = this.Level.Player.IsStarpostActivated(this._index);
        this._glowing = this._activated;
        this.UpdateStarPosition();
      }

      protected override void OnUpdate()
      {
        if (!this._activated && this.Level.Player.IsStarpostActivated(this._index))
        {
          this._activated = true;
          this._glowing = true;
        }
        if (this._starSpinning)
        {
          this._starAngle += 2.0 * Math.PI / 15.0;
          if (this._starAngle >= 4.0 * Math.PI)
          {
            this._starAngle = 0.0;
            this._starSpinning = false;
            this._glowing = true;
          }
        }
        this.UpdateStarPosition();
      }

      private void UpdateStarPosition()
      {
        this._starPosition = new Vector2(StarpostInstance.StarAxelMidpoint.X + Math.Sin(this._starAngle) * StarpostInstance.StarAxelRadius, StarpostInstance.StarAxelMidpoint.Y - Math.Cos(this._starAngle) * StarpostInstance.StarAxelRadius);
      }

      protected override void OnCollision(CollisionEvent e)
      {
        if (this._activated || e.ActiveObject.Type.Classification != ObjectClassification.Character)
          return;
        ICharacter activeObject = (ICharacter) e.ActiveObject;
        if (activeObject.IsSidekick)
          return;
        activeObject.Player.ActivateStarpost(this._index, this.Position + new Vector2i(0, 64 /*0x40*/));
        this._activated = true;
        this._starSpinning = true;
        this.Level.SoundManager.PlaySound((IActiveObject) this, this.Type.GetAbsolutePath("SONICORCA/SOUND/STARPOST"));
      }

      protected override void OnAnimate()
      {
        if (this._glowing)
        {
          this._starAnimation.Index = 2;
          this._starGlowAnimation.CurrentFrameIndex = this._starAnimation.CurrentFrameIndex;
        }
        this._standAnimation.Animate();
        this._starAnimation.Animate();
        this._starGlowAnimation.Animate();
      }

      protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
      {
        IObjectRenderer objectRenderer = renderer.GetObjectRenderer();
        objectRenderer.Render(this._standAnimation);
        objectRenderer.Render(this._starAnimation, this._starPosition);
        if (!this._glowing)
          return;
        objectRenderer.BlendMode = BlendMode.Additive;
        objectRenderer.EmitsLight = true;
        objectRenderer.Render(this._starGlowAnimation, this._starPosition);
      }
    }
}
