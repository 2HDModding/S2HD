// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.EHZBRIDGE.EHZBridgeInstance
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Collision;
using SonicOrca.Core.Objects;
using SonicOrca.Extensions;
using SonicOrca.Geometry;
using SonicOrca.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace SONICORCA.OBJECTS.EHZBRIDGE;

public class EHZBridgeInstance : ActiveObject
{
  private AnimationInstance _logAnimation;
  private int _logs = 2;
  private Vector2i[] _logPositions;
  private bool[] _depressedLogs;

  private int TotalWidth => this._logs * 64 /*0x40*/;

  public int Logs
  {
    get => this._logs;
    set => this._logs = value;
  }

  protected override void OnStart()
  {
    this._logAnimation = new AnimationInstance(this.ResourceTree, this.Type.GetAbsolutePath("/ANIGROUP"));
    this._logAnimation.Index = 1;
    this._logPositions = new Vector2i[this._logs];
    int x = -(this.TotalWidth / 2);
    for (int index = 0; index < this._logs; ++index)
    {
      this._logPositions[index] = new Vector2i(x, 0);
      x += 64 /*0x40*/;
    }
    this._depressedLogs = new bool[this._logs];
    this.DesignBounds = new Rectanglei(-this.TotalWidth / 2 - 32 /*0x20*/, -32, this.TotalWidth, 64 /*0x40*/);
    this.CollisionVectors = ((IEnumerable<Vector2i>) this._logPositions).Select<Vector2i, CollisionVector>((Func<Vector2i, CollisionVector>) (log => new CollisionVector((ActiveObject) this, new Vector2i(log.X - 32 /*0x20*/, log.Y - 32 /*0x20*/), new Vector2i(log.X + 32 /*0x20*/, log.Y - 32 /*0x20*/), flags: CollisionFlags.Conveyor | CollisionFlags.NoBalance))).ToArray<CollisionVector>();
  }

  protected override void OnUpdate()
  {
    for (int index = 0; index < this._logs; ++index)
      this._depressedLogs[index] = false;
    double num1 = (double) (this.Position.X + this._logPositions[1].X - 32 /*0x20*/);
    Vector2i position = this.Position;
    double num2 = (double) (position.X + this._logPositions[this._logs - 2].X + 32 /*0x20*/);
    foreach (ICharacter character in this.Level.ObjectManager.Characters)
    {
      if (character.ObjectLink == this)
      {
        position = character.Position;
        if ((double) position.X >= num1)
        {
          position = character.Position;
          if ((double) position.X < num2)
          {
            position = character.Position;
            int x = position.X;
            position = this.Position;
            int num3 = position.X + (this._logPositions[0].X - 32 /*0x20*/);
            int index = (int) ((double) (x - num3) / 64.0);
            if (index >= 0 && index < this._logs)
              this._depressedLogs[index] = true;
          }
        }
      }
    }
    int logIndex = -1;
    for (int index = 0; index < this._logs; ++index)
    {
      if (this._depressedLogs[index] && Math.Abs(this._logs / 2 - index) < Math.Abs(this._logs / 2 - logIndex))
        logIndex = index;
    }
    if (logIndex != -1)
    {
      this.DepressLogs(logIndex);
    }
    else
    {
      for (int index = 0; index < this._logs; ++index)
        this._logPositions[index].Y = Math.Max(0, this._logPositions[index].Y - 4);
    }
    for (int index = 0; index < this._logs; ++index)
    {
      Vector2i logPosition = this._logPositions[index];
      CollisionVector collisionVector = this.CollisionVectors[index];
      collisionVector.RelativeA = new Vector2i(logPosition.X - 32 /*0x20*/, logPosition.Y - 32 /*0x20*/);
      collisionVector.RelativeB = new Vector2i(logPosition.X + 32 /*0x20*/, logPosition.Y - 32 /*0x20*/);
    }
    this.RegisterCollisionUpdate();
    int count = this.Level.Map.CollisionPathLayers.Count;
    for (int index = 0; index < this._logs; ++index)
    {
      CollisionVector collisionVector = this.CollisionVectors[index];
      for (int path = 0; path < count; ++path)
      {
        if (index > 0)
          collisionVector.SetConnectionA(path, this.CollisionVectors[index - 1]);
        if (index < this._logs - 1)
          collisionVector.SetConnectionB(path, this.CollisionVectors[index + 1]);
      }
    }
  }

  private void DepressLogs(int logIndex)
  {
    int num = (int) (Math.Sin((double) logIndex / (double) this._logs * Math.PI) * 40.0);
    for (int i = 0; i < logIndex; ++i)
    {
      int val1 = (int) (Math.Sin((double) i / (double) logIndex * (Math.PI / 2.0)) * 40.0);
      this.UpdateLogY(i, Math.Min(val1, num));
    }
    this.UpdateLogY(logIndex, num);
    for (int i = logIndex + 1; i < this._logs; ++i)
    {
      int val1 = (int) (Math.Sin(Math.PI / 2.0 + (double) (i - logIndex) / (double) (this._logs - logIndex - 1) * (Math.PI / 2.0)) * 40.0);
      this.UpdateLogY(i, Math.Min(val1, num));
    }
  }

  private void UpdateLogY(int i, int depress)
  {
    int y = this._logPositions[i].Y;
    int num;
    if (y > depress)
    {
      num = y - 4;
      if (num < depress)
        num = depress;
    }
    else
    {
      num = y + 4;
      if (num > depress)
        num = depress;
    }
    this._logPositions[i].Y = num;
  }

  protected override void OnAnimate() => this._logAnimation.Animate();

  protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
  {
    IObjectRenderer objectRenderer = renderer.GetObjectRenderer();
    for (int index = 0; index < this._logs; ++index)
      objectRenderer.Render(this._logAnimation, (Vector2) this._logPositions[index]);
  }
}
