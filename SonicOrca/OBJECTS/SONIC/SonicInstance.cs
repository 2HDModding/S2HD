// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.SONIC.SonicInstance
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Objects.Base;
using SonicOrca.Extensions;
using SonicOrca.Geometry;
using SonicOrca.Graphics;

namespace SONICORCA.OBJECTS.SONIC
{

    public class SonicInstance : Character
    {
      private int _winningTicks;

      protected override void OnStart()
      {
        this.AnimationGroupResourceKey = this.Type.GetAbsolutePath("/ANIGROUP");
        base.OnStart();
      }

      protected override void OnUpdate()
      {
        base.OnUpdate();
        if (this.IsWinning)
        {
          if (this._winningTicks == 0)
            this.Level.SoundManager.PlaySound((IActiveObject) this, "SONICORCA/SOUND/PEELOUT/CHARGE");
          else if (this._winningTicks == 70)
            this.Level.SoundManager.PlaySound((IActiveObject) this, "SONICORCA/SOUND/PEELOUT/RELEASE");
          ++this._winningTicks;
        }
        else
          this._winningTicks = 0;
      }

      protected override void DrawBody(Renderer renderer, LayerViewOptions viewOptions)
      {
        I2dRenderer obj = renderer.Get2dRenderer();
        ICharacterRenderer characterRenderer = renderer.GetCharacterRenderer();
        characterRenderer.ModelMatrix = obj.ModelMatrix;
        double propertyDouble1 = this.Level.GameContext.Configuration.GetPropertyDouble("debug", "sonic_hue_shift");
        double propertyDouble2 = this.Level.GameContext.Configuration.GetPropertyDouble("debug", "sonic_sat_shift");
        double propertyDouble3 = this.Level.GameContext.Configuration.GetPropertyDouble("debug", "sonic_lum_shift");
        if (this.DrawBodyRotated)
          characterRenderer.ModelMatrix *= Matrix4.CreateRotationZ(this.ShowAngle);
        SonicOrca.Graphics.Animation.Frame currentFrame = this.Animation.CurrentFrame;
        Vector2 offset = (Vector2) currentFrame.Offset;
        Vector2 vector2_1 = new Vector2();
        vector2_1 = !this.IsSpinball ? new Vector2(offset.X, offset.Y - 16.0) : new Vector2(offset.X, offset.Y - 4.0);
        Vector2 vector2_2 = vector2_1;
        Rectangle destination = new Rectangle();
        ref Rectangle local = ref destination;
        double x = vector2_2.X - (double) (currentFrame.Source.Width / 2);
        double y1 = vector2_2.Y;
        Rectanglei source = currentFrame.Source;
        double num = (double) (source.Height / 2);
        double y2 = y1 - num;
        source = currentFrame.Source;
        double width = (double) source.Width;
        source = currentFrame.Source;
        double height = (double) source.Height;
        local = new Rectangle(x, y2, width, height);
        characterRenderer.Filter = viewOptions.Filter;
        characterRenderer.FilterAmount = viewOptions.FilterAmount;
        characterRenderer.Brightness = viewOptions.Shadows ? 0.0f : this.Brightness;
        characterRenderer.RenderTexture(this.Animation.AnimationGroup.Textures[1], this.Animation.AnimationGroup.Textures[0], propertyDouble1, propertyDouble2, propertyDouble3, (Rectangle) currentFrame.Source, destination, this.IsFacingRight, this.IsFacingLeft && this.Animation.Index == 16 /*0x10*/);
      }

      protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
      {
        base.OnDraw(renderer, viewOptions);
      }
    }
}
