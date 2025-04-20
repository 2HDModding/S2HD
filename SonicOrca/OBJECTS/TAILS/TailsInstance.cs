// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.TAILS.TailsInstance
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Objects.Base;
using SonicOrca.Drawing.Renderers;
using SonicOrca.Extensions;
using SonicOrca.Geometry;
using SonicOrca.Graphics;
using SonicOrca.Resources;
using System;

namespace SONICORCA.OBJECTS.TAILS
{

    public class TailsInstance : Character
    {
      private AnimationInstance _tailAnimationInstance;
      private AnimationInstance _headAnimationInstance;

      protected override void OnStart()
      {
        this.AnimationGroupResourceKey = this.Type.GetAbsolutePath("/ANIGROUP");
        AnimationGroup loadedResource1 = this.ResourceTree.GetLoadedResource<AnimationGroup>((ILoadedResource) this.Type, "/TAIL/ANIGROUP");
        AnimationGroup loadedResource2 = this.ResourceTree.GetLoadedResource<AnimationGroup>((ILoadedResource) this.Type, "/HEAD/ANIGROUP");
        this._tailAnimationInstance = new AnimationInstance(loadedResource1);
        this._headAnimationInstance = new AnimationInstance(loadedResource2);
        this.NormalCollisionRadius = new Vector2i(40, 60);
        this.CanFly = true;
        base.OnStart();
      }

      protected override void OnAnimate()
      {
        base.OnAnimate();
        int index = this._tailAnimationInstance.Index;
        int num;
        switch (this.Animation.Index)
        {
          case 0:
          case 1:
          case 2:
            num = 0;
            break;
          case 3:
            num = 1;
            break;
          case 4:
            num = 1;
            break;
          case 5:
            num = 1;
            break;
          case 6:
            num = 0;
            break;
          case 9:
            num = 1;
            break;
          case 10:
            num = 1;
            break;
          case 11:
            num = 2;
            break;
          default:
            num = -1;
            break;
        }
        this._tailAnimationInstance.Index = num;
        if (num != -1)
          this._tailAnimationInstance.Animate();
        if (this.Animation.Index == 24)
        {
          this._headAnimationInstance.Index = 0;
          this._headAnimationInstance.Animate();
        }
        else
          this._headAnimationInstance.Index = -1;
      }

      protected override void DrawBody(Renderer renderer, LayerViewOptions viewOptions)
      {
        I2dRenderer obj = renderer.Get2dRenderer();
        CharacterRenderer characterRenderer = CharacterRenderer.FromRenderer(renderer);
        characterRenderer.ModelMatrix = obj.ModelMatrix;
        double propertyDouble1 = this.Level.GameContext.Configuration.GetPropertyDouble("debug", "tails_hue_shift");
        double propertyDouble2 = this.Level.GameContext.Configuration.GetPropertyDouble("debug", "tails_sat_shift");
        double propertyDouble3 = this.Level.GameContext.Configuration.GetPropertyDouble("debug", "tails_lum_shift");
        characterRenderer.Filter = viewOptions.Filter;
        characterRenderer.FilterAmount = viewOptions.FilterAmount;
        characterRenderer.Brightness = this.Brightness;
        if (this.DrawBodyRotated)
          characterRenderer.ModelMatrix *= Matrix4.CreateRotationZ(this.ShowAngle);
        Vector2 offset;
        Rectangle destination = new Rectangle();
        Rectanglei source;
        if (this._tailAnimationInstance.Index != -1)
        {
          SonicOrca.Graphics.Animation.Frame currentFrame = this._tailAnimationInstance.CurrentFrame;
          offset = (Vector2) currentFrame.Offset;
          ref Rectangle local = ref destination;
          double x = offset.X - (double) (currentFrame.Source.Width / 2);
          double y = offset.Y - (double) (currentFrame.Source.Height / 2);
          double width = (double) currentFrame.Source.Width;
          source = currentFrame.Source;
          double height = (double) source.Height;
          local = new Rectangle(x, y, width, height);
          if (this.Animation.Index == 10)
          {
            double angle = Math.Atan2(this.Velocity.Y, this.Velocity.X);
            characterRenderer.ModelMatrix *= Matrix4.CreateRotationZ(angle);
            characterRenderer.RenderTexture(this._tailAnimationInstance.AnimationGroup.Textures[1], this._tailAnimationInstance.AnimationGroup.Textures[0], propertyDouble1, propertyDouble2, propertyDouble3, (Rectangle) currentFrame.Source, destination, true, this.Velocity.X < 0.0);
            characterRenderer.ModelMatrix = obj.ModelMatrix;
          }
          else
            characterRenderer.RenderTexture(this._tailAnimationInstance.AnimationGroup.Textures[1], this._tailAnimationInstance.AnimationGroup.Textures[0], propertyDouble1, propertyDouble2, propertyDouble3, (Rectangle) currentFrame.Source, destination, this.IsFacingRight, false);
        }
        SonicOrca.Graphics.Animation.Frame currentFrame1 = this.Animation.CurrentFrame;
        offset = (Vector2) currentFrame1.Offset;
        ref Rectangle local1 = ref destination;
        double x1 = offset.X;
        source = currentFrame1.Source;
        double num1 = (double) (source.Width / 2);
        double x2 = x1 - num1;
        double y1 = offset.Y;
        source = currentFrame1.Source;
        double num2 = (double) (source.Height / 2);
        double y2 = y1 - num2;
        source = currentFrame1.Source;
        double width1 = (double) source.Width;
        source = currentFrame1.Source;
        double height1 = (double) source.Height;
        local1 = new Rectangle(x2, y2, width1, height1);
        characterRenderer.RenderTexture(this.Animation.AnimationGroup.Textures[1], this.Animation.AnimationGroup.Textures[0], propertyDouble1, propertyDouble2, propertyDouble3, (Rectangle) currentFrame1.Source, destination, this.IsFacingRight, this.IsFacingLeft && this.Animation.Index == 16 /*0x10*/);
        if (this._headAnimationInstance.Index == -1)
          return;
        SonicOrca.Graphics.Animation.Frame currentFrame2 = this._headAnimationInstance.CurrentFrame;
        offset = (Vector2) currentFrame2.Offset;
        ref Rectangle local2 = ref destination;
        double x3 = offset.X;
        source = currentFrame2.Source;
        double num3 = (double) (source.Width / 2);
        double x4 = x3 - num3;
        double y3 = offset.Y;
        source = currentFrame2.Source;
        double num4 = (double) (source.Height / 2);
        double y4 = y3 - num4;
        source = currentFrame2.Source;
        double width2 = (double) source.Width;
        source = currentFrame2.Source;
        double height2 = (double) source.Height;
        local2 = new Rectangle(x4, y4, width2, height2);
        characterRenderer.RenderTexture(this._headAnimationInstance.AnimationGroup.Textures[1], this._headAnimationInstance.AnimationGroup.Textures[0], propertyDouble1, propertyDouble2, propertyDouble3, (Rectangle) currentFrame2.Source, destination, this.IsFacingRight, false);
      }
    }
}
