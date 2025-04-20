// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.EHZTWISTEDPATHWAY.EHZTwistedPathwayInstance
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Collision;
using SonicOrca.Core.Objects;
using SonicOrca.Geometry;
using SonicOrca.Graphics;
using System;

namespace SONICORCA.OBJECTS.EHZTWISTEDPATHWAY;

public class EHZTwistedPathwayInstance : ActiveObject
{
  private Vector2i _radius = new Vector2i(832, 200);
  private double _previousTumbleAngle;

  public EHZTwistedPathwayInstance()
  {
    this.DesignBounds = new Rectanglei(-this._radius.X, -this._radius.Y, this._radius.X * 2, this._radius.Y * 2);
  }

  protected override void OnStop()
  {
    foreach (ICharacter character in this.Level.ObjectManager.Characters)
    {
      if (character.ObjectLink == this)
        this.DisengadgeCharacter(character);
    }
  }

  protected override void OnUpdate()
  {
    this._radius.X = 768 /*0x0300*/;
    this._radius.Y = 192 /*0xC0*/;
    foreach (ICharacter character in this.Level.ObjectManager.Characters)
      this.UpdateCharacter(character);
  }

  private void UpdateCharacter(ICharacter character)
  {
    if (character.ObjectLink == this)
    {
      character.CheckLandscapeCollision = false;
      character.IsAirborne = false;
      if (Math.Abs(character.GroundVelocity) < 20.0)
        this.DisengadgeCharacter(character);
      else if (character.IsAirborne || character.IsDebug)
      {
        this.DisengadgeCharacter(character);
      }
      else
      {
        Vector2i vector2i = character.Position;
        int x1 = vector2i.X;
        vector2i = character.CollisionRadius;
        int x2 = vector2i.X;
        int num1 = x1 + x2;
        vector2i = this.Position;
        int num2 = vector2i.X - this._radius.X - 44;
        if (num1 >= num2)
        {
          vector2i = character.Position;
          int x3 = vector2i.X;
          vector2i = character.CollisionRadius;
          int x4 = vector2i.X;
          int num3 = x3 - x4;
          vector2i = this.Position;
          int num4 = vector2i.X + this._radius.X + 44;
          if (num3 < num4)
          {
            this.UpdateTumble(character);
            return;
          }
        }
        this.DisengadgeCharacter(character);
      }
    }
    else
    {
      if (Math.Abs(character.GroundVelocity) < 20.0 || character.IsAirborne || character.IsDebug)
        return;
      Vector2i vector2i = character.Position;
      int x5 = vector2i.X;
      vector2i = character.CollisionRadius;
      int x6 = vector2i.X;
      int num5 = x5 + x6;
      vector2i = this.Position;
      int num6 = vector2i.X - this._radius.X;
      if (num5 < num6)
        return;
      vector2i = character.Position;
      int x7 = vector2i.X;
      vector2i = character.CollisionRadius;
      int x8 = vector2i.X;
      int num7 = x7 - x8;
      vector2i = this.Position;
      int num8 = vector2i.X + this._radius.X;
      if (num7 >= num8)
        return;
      vector2i = character.Position;
      int y1 = vector2i.Y;
      vector2i = character.CollisionRadius;
      int y2 = vector2i.Y;
      int num9 = y1 + y2;
      vector2i = this.Position;
      int num10 = vector2i.Y + this._radius.Y - 32 /*0x20*/;
      if (num9 < num10)
        return;
      vector2i = character.Position;
      int y3 = vector2i.Y;
      vector2i = character.CollisionRadius;
      int y4 = vector2i.Y;
      int num11 = y3 + y4;
      vector2i = this.Position;
      int num12 = vector2i.Y + this._radius.Y + 32 /*0x20*/;
      if (num11 >= num12)
        return;
      this.EngadgeCharacter(character);
      this.UpdateTumble(character);
    }
  }

  private void UpdateTumble(ICharacter character)
  {
    Vector2i position1 = character.Position;
    int x1 = position1.X;
    position1 = this.Position;
    int num1 = position1.X - this._radius.X;
    double num2 = (double) (x1 - num1) / (double) (this._radius.X * 2);
    double num3 = Math.Cos(num2 * (2.0 * Math.PI)) * (double) (this._radius.Y - character.CollisionRadius.Y);
    ICharacter character1 = character;
    Vector2i position2 = character.Position;
    double x2 = (double) position2.X;
    position2 = this.Position;
    double y = (double) position2.Y + num3;
    Vector2 vector2 = new Vector2(x2, y);
    character1.PositionPrecise = vector2;
    character.TumbleAngle = num2 < 0.5 ? num2 * 2.0 : (1.0 - num2) * -2.0;
    if (character.TumbleAngle <= 0.0)
    {
      if (character.TumbleAngle + 3.0 / 128.0 > 0.0)
        character.TumbleAngle = this._previousTumbleAngle;
      else
        this._previousTumbleAngle = character.TumbleAngle;
    }
    else if (character.TumbleAngle - 3.0 / 128.0 < 0.0)
      character.TumbleAngle = this._previousTumbleAngle;
    else
      this._previousTumbleAngle = character.TumbleAngle;
  }

  private void EngadgeCharacter(ICharacter character)
  {
    if (character.ObjectLink != null)
    {
      if (character.ObjectLink.Type.Name == "Twisted pathway")
        this._previousTumbleAngle = (character.ObjectLink as EHZTwistedPathwayInstance)._previousTumbleAngle;
    }
    else
      this._previousTumbleAngle = character.TumbleAngle;
    character.LeaveGround();
    character.ObjectLink = (ActiveObject) this;
    character.CheckLandscapeCollision = false;
    character.ShowAngle = 0.0;
    character.Velocity = new Vector2(character.Velocity.X, 0.0);
    character.Mode = CollisionMode.Top;
  }

  private void DisengadgeCharacter(ICharacter character)
  {
    this._previousTumbleAngle = 0.0;
    character.ObjectLink = (ActiveObject) null;
    character.CheckLandscapeCollision = true;
    character.IsAirborne = true;
  }

  protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
  {
    if (!this.Level.LayerViewOptions.ShowObjectCollision && !this.Level.StateFlags.HasFlag((Enum) LevelStateFlags.Editing) || viewOptions.Shadows)
      return;
    I2dRenderer obj = renderer.Get2dRenderer();
    Colour white1 = Colours.White;
    double num1 = (double) -this._radius.X;
    Vector2 scale = renderer.GetObjectRenderer().Scale;
    double x1 = scale.X;
    Vector2 a1 = new Vector2(num1 * x1, 0.0);
    double x2 = (double) this._radius.X;
    scale = renderer.GetObjectRenderer().Scale;
    double x3 = scale.X;
    Vector2 b1 = new Vector2(x2 * x3, 0.0);
    obj.RenderLine(white1, a1, b1, 2.0);
    Colour white2 = Colours.White;
    double num2 = (double) -this._radius.Y;
    scale = renderer.GetObjectRenderer().Scale;
    double y1 = scale.Y;
    Vector2 a2 = new Vector2(0.0, num2 * y1);
    double y2 = (double) this._radius.Y;
    scale = renderer.GetObjectRenderer().Scale;
    double y3 = scale.Y;
    Vector2 b2 = new Vector2(0.0, y2 * y3);
    obj.RenderLine(white2, a2, b2, 2.0);
    Colour white3 = Colours.White;
    double num3 = (double) -this._radius.X;
    scale = renderer.GetObjectRenderer().Scale;
    double x4 = scale.X;
    double x5 = num3 * x4;
    double num4 = (double) -this._radius.Y;
    scale = renderer.GetObjectRenderer().Scale;
    double y4 = scale.Y;
    double y5 = num4 * y4;
    double num5 = (double) (this._radius.X * 2);
    scale = renderer.GetObjectRenderer().Scale;
    double x6 = scale.X;
    double width = num5 * x6;
    double num6 = (double) (this._radius.Y * 2);
    scale = renderer.GetObjectRenderer().Scale;
    double y6 = scale.Y;
    double height = num6 * y6;
    Rectangle destination = new Rectangle(x5, y5, width, height);
    obj.RenderRectangle(white3, destination, 2.0);
  }
}
