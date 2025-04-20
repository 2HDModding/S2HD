// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.FORCESPINBALL.ForceSpinballInstance
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Objects;
using SonicOrca.Core.Objects.Metadata;
using SonicOrca.Geometry;
using SonicOrca.Graphics;
using System;
using System.Collections.Generic;

#nullable disable
namespace SONICORCA.OBJECTS.FORCESPINBALL;

public class ForceSpinballInstance : ActiveObject
{
  private readonly Dictionary<ICharacter, int> _characterTrace = new Dictionary<ICharacter, int>();
  [StateVariable]
  private int _direction = 1;

  protected override void OnStart()
  {
  }

  protected override void OnUpdate()
  {
    foreach (ICharacter character in this.Level.ObjectManager.Characters)
      this.UpdateCharacter(character);
  }

  private void UpdateCharacter(ICharacter character)
  {
    if (character.IsDebug || this._direction == 0 || this._direction == 2)
      return;
    Vector2i position = character.Position;
    int x1 = position.X;
    position = this.Position;
    int x2 = position.X;
    int num1 = x1 < x2 ? -1 : 1;
    if (this._characterTrace.ContainsKey(character) && this._characterTrace[character] != num1)
    {
      position = character.Position;
      int y1 = position.Y;
      position = this.Position;
      int num2 = position.Y - 64 /*0x40*/;
      if (y1 >= num2)
      {
        position = character.Position;
        int y2 = position.Y;
        position = this.Position;
        int num3 = position.Y + 64 /*0x40*/;
        if (y2 < num3)
          character.ForceSpinball = num1 != -1 ? this._direction == 1 : this._direction == 3;
      }
    }
    this._characterTrace[character] = num1;
  }

  protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
  {
    if (!viewOptions.ShowObjectCollision && !this.Level.StateFlags.HasFlag((Enum) LevelStateFlags.Editing))
      return;
    renderer.Get2dRenderer().RenderRectangle(Colours.White, new Rectangle(-64.0, -64.0, 128.0, 128.0), 1.0);
  }
}
