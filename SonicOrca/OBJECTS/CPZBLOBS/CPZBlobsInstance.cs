// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.CPZBLOBS.CPZBlobsInstance
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Collision;
using SonicOrca.Core.Objects.Base;
using SonicOrca.Core.Objects.Metadata;
using SonicOrca.Extensions;
using SonicOrca.Geometry;
using SonicOrca.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SONICORCA.OBJECTS.CPZBLOBS;

public class CPZBlobsInstance : ActiveObject
{
  private const int AnimationBlob = 0;
  private const int AnimationTopSplashStart = 1;
  private const int AnimationTopSplashEnd = 2;
  private const int AnimationBottomSplashStart = 3;
  private const int AnimationBottomSplashEnd = 4;
  private const int AnimationBottomOverflow = 5;
  private AnimationGroup _animationGroup;
  private CPZBlobsInstance.ArrangementType _arrangementType;
  private CPZBlobsInstance.Blob[] _children;
  private List<AnimationInstance> _bottomStartOverflowTriggeredAnimations = new List<AnimationInstance>();
  private Dictionary<AnimationInstance, Vector2> _triggeredAnimations = new Dictionary<AnimationInstance, Vector2>();
  private Dictionary<AnimationInstance, Vector2> _triggeredAnimationsPositions = new Dictionary<AnimationInstance, Vector2>();

  [StateVariable]
  private CPZBlobsInstance.ArrangementType Arrangement
  {
    get => this._arrangementType;
    set => this._arrangementType = value;
  }

  [StateVariable]
  private bool FlipX { get; set; }

  protected override void OnStart()
  {
    this._animationGroup = this.ResourceTree.GetLoadedResource<AnimationGroup>(this.Type.GetAbsolutePath("/ANIGROUP"));
    this.DesignBounds = new Rectanglei((int) sbyte.MinValue, (int) sbyte.MinValue, 256 /*0x0100*/, 256 /*0x0100*/);
    this._children = new CPZBlobsInstance.Blob[6];
    for (int index = 5; index >= 0; --index)
    {
      CPZBlobsInstance.Blob blob = this.Level.ObjectManager.AddSubObject<CPZBlobsInstance.Blob>((ActiveObject) this);
      blob.Initialise(this.Position, index, this.FlipX);
      this._children[index] = blob;
      this._children[index].Priority = index + 2;
    }
  }

  protected override void OnUpdate()
  {
  }

  [HideInEditor]
  public List<AnimationInstance> BottomStartOverflowTriggeredAnimations
  {
    get => this._bottomStartOverflowTriggeredAnimations;
  }

  [HideInEditor]
  public Dictionary<AnimationInstance, Vector2> TriggeredAnimationsPositions
  {
    get => this._triggeredAnimationsPositions;
  }

  [HideInEditor]
  public Dictionary<AnimationInstance, Vector2> TriggeredAnimations => this._triggeredAnimations;

  protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
  {
    base.OnDraw(renderer, viewOptions);
    IObjectRenderer objectRenderer = renderer.GetObjectRenderer();
    List<AnimationInstance> animationInstanceList = new List<AnimationInstance>();
    foreach (KeyValuePair<AnimationInstance, Vector2> triggeredAnimation in this.TriggeredAnimations)
    {
      triggeredAnimation.Key.Animate();
      if (triggeredAnimation.Key.Cycles > 0)
        animationInstanceList.Add(triggeredAnimation.Key);
    }
    foreach (AnimationInstance key in animationInstanceList)
    {
      key.ResetFrame();
      key.Cycles = 0;
      if (key.Index == 4 || this.BottomStartOverflowTriggeredAnimations.Contains(key))
      {
        CPZBlobsInstance.OverflowInstance overflowInstance = this.Level.ObjectManager.AddSubObject<CPZBlobsInstance.OverflowInstance>((ActiveObject) this);
        overflowInstance.InitOverflow();
        overflowInstance.Layer = this.Level.Map.Layers.Last<LevelLayer>();
        Vector2 animationsPosition = this.TriggeredAnimationsPositions[key];
        if (this.BottomStartOverflowTriggeredAnimations.Contains(key))
        {
          if (this._arrangementType == CPZBlobsInstance.ArrangementType.Arc)
            overflowInstance.PositionPrecise = new Vector2(animationsPosition.X, animationsPosition.Y - 40.0);
          else
            overflowInstance.PositionPrecise = new Vector2(animationsPosition.X, animationsPosition.Y - 40.0);
        }
        else if (this._arrangementType == CPZBlobsInstance.ArrangementType.Arc)
          overflowInstance.PositionPrecise = new Vector2(animationsPosition.X, animationsPosition.Y - 15.0);
        else
          overflowInstance.PositionPrecise = new Vector2(animationsPosition.X, animationsPosition.Y - 23.0);
      }
      if (this.BottomStartOverflowTriggeredAnimations.Contains(key))
        this.BottomStartOverflowTriggeredAnimations.Remove(key);
      this.TriggeredAnimations.Remove(key);
      this.TriggeredAnimationsPositions.Remove(key);
    }
    foreach (KeyValuePair<AnimationInstance, Vector2> triggeredAnimation in this.TriggeredAnimations)
    {
      Vector2 animationsPosition = this.TriggeredAnimationsPositions[triggeredAnimation.Key];
      objectRenderer.Render(triggeredAnimation.Key, animationsPosition - this.PositionPrecise + triggeredAnimation.Value);
    }
  }

  private enum ArrangementType
  {
    Arc,
    Straight,
  }

  private class Blob : Enemy
  {
    private AnimationInstance _animationBlob;
    private AnimationInstance _animationTopSplashStart;
    private AnimationInstance _animationTopSplashEnd;
    private AnimationInstance _animationBottomSplashStart;
    private AnimationInstance _animationBottomSplashEnd;
    private AnimationInstance _animationBottomOverflow;
    private int _index;
    private int _originalY;
    private Vector2 _velocity;
    private double _upVelocity;
    private int _upX;
    private int _downX;
    private bool _leftToRight;
    private AnimationInstance _lastTriggeredAnimation;
    private bool _directionChange;
    private int _directionChangeNormal;
    private bool _startAnimation;
    private double _currentAngle;

    public int Delay { get; set; }

    public CPZBlobsInstance Host => this.ParentObject as CPZBlobsInstance;

    public Vector2 Velocity => this._velocity;

    protected override void OnStart()
    {
      base.OnStart();
      this._animationBlob = new AnimationInstance(this.Host._animationGroup);
      this._animationTopSplashStart = new AnimationInstance(this.Host._animationGroup, 1);
      this._animationTopSplashEnd = new AnimationInstance(this.Host._animationGroup, 2);
      this._animationBottomSplashStart = new AnimationInstance(this.Host._animationGroup, 3);
      this._animationBottomSplashEnd = new AnimationInstance(this.Host._animationGroup, 4);
      this._animationBottomOverflow = new AnimationInstance(this.Host._animationGroup, 5);
      this.Priority = (int) sbyte.MinValue;
      this.CollisionRectangles = new CollisionRectangle[1]
      {
        new CollisionRectangle((ActiveObject) this, 0, -16, -16, 32 /*0x20*/, 32 /*0x20*/)
      };
    }

    public void Initialise(Vector2i position, int index, bool flipX)
    {
      this._index = index;
      this._upVelocity = this.Host._arrangementType != CPZBlobsInstance.ArrangementType.Arc ? -9.0 : -17.55;
      this._velocity = new Vector2(0.0, this._upVelocity);
      this.Position = position;
      this._originalY = this.Host._arrangementType != CPZBlobsInstance.ArrangementType.Arc ? position.Y : position.Y + 8;
      if (flipX)
      {
        this._leftToRight = false;
        this._upX = position.X;
        this._downX = position.X + 384;
        this._currentAngle = Math.PI;
      }
      else
      {
        this._leftToRight = true;
        this._upX = position.X + 384;
        this._downX = position.X;
        this._currentAngle = 0.0;
      }
      this.Delay = this.Host._arrangementType != CPZBlobsInstance.ArrangementType.Arc ? index * 4 : index * 5;
      this.LockLifetime = true;
    }

    protected override void OnUpdate()
    {
      base.OnUpdate();
      if (this.Host.Finished)
        this.Finish();
      else if (this.Delay > 0)
        --this.Delay;
      else if (this.Host._arrangementType == CPZBlobsInstance.ArrangementType.Arc)
        this.UpdateArc();
      else
        this.UpdateStraight();
    }

    private void UpdateArc()
    {
      if (this.Host._arrangementType == CPZBlobsInstance.ArrangementType.Arc)
        this.ArcPrepareTriggers();
      if (this._leftToRight && this._currentAngle <= 0.0)
        this.PlayGloopSound();
      if (!this._leftToRight && this._currentAngle >= Math.PI)
        this.PlayGloopSound();
      Vector2 relative = new Vector2((double) (this._downX + this._upX) * 0.5, (double) this._originalY);
      double num = Vector2.GetDistance(new Vector2((double) this._downX, 0.0), new Vector2((double) this._upX, 0.0)) * 0.5;
      double x = relative.X - num;
      if (this._leftToRight)
        this._currentAngle += 0.031939525311496228;
      else
        this._currentAngle -= 0.031939525311496228;
      this.PositionPrecise = this.PositionPrecise + this._velocity;
      if (this._leftToRight)
      {
        Vector2 point = new Vector2(x, (double) this._originalY);
        this.PositionPrecise = new Vector2(Vector2.GetPointRotatedFromRelative(relative, point, this._currentAngle).X, this.PositionPrecise.Y);
      }
      else
      {
        Vector2 point = new Vector2(x, (double) this._originalY);
        this.PositionPrecise = new Vector2(Vector2.GetPointRotatedFromRelative(relative, point, this._currentAngle).X, this.PositionPrecise.Y);
      }
      this._velocity.Y += 0.375;
      if (this.PositionPrecise.Y >= (double) this._originalY && !this._directionChange)
      {
        this._directionChange = true;
        this.Delay = 72;
      }
      if (!this._directionChange)
        return;
      this._velocity.Y = this._upVelocity;
      this._directionChange = false;
      this._leftToRight = !this._leftToRight;
      if (this._leftToRight)
        this._currentAngle = 0.0;
      if (!this._leftToRight)
        this._currentAngle = Math.PI;
      this.PositionPrecise = new Vector2(this._leftToRight ? (double) this._downX : (double) this._upX, (double) this._originalY);
    }

    private void UpdateStraight()
    {
      this.PositionPrecise = this.PositionPrecise + this._velocity;
      if (this._directionChange)
      {
        this._velocity.Y = this._upVelocity * (double) this._directionChangeNormal;
        this._directionChange = false;
      }
      if (this.Position.Y > this._originalY && this._velocity.Y > 0.0)
      {
        this.PlayGloopSound();
        this.Delay = 72;
        this._directionChange = true;
        this._velocity.Y = 0.0;
        this._directionChangeNormal = 1;
      }
      else if (this.Position.Y < this._originalY - 324 && this._velocity.Y < 0.0)
      {
        this.PlayGloopSound();
        this.Delay = 9;
        this._directionChange = true;
        this._velocity.Y = 0.0;
        this._directionChangeNormal = -1;
      }
      this.PositionPrecise = new Vector2(this._velocity.Y >= 0.0 ? (double) this._downX : (double) this._upX, this.PositionPrecise.Y);
    }

    private void PlayGloopSound()
    {
      this.Level.SoundManager.PlaySoundVisibleOnly("SONICORCA/SOUND/BLOB", this.Position);
    }

    protected override void OnAnimate()
    {
      if (!this._startAnimation)
        return;
      this._animationBlob.Animate();
    }

    private void TriggerAnimation(AnimationInstance animation, Vector2 position, bool isFinal = false)
    {
      if (!(this._lastTriggeredAnimation != animation | isFinal))
        return;
      this._lastTriggeredAnimation = animation;
      if (isFinal)
        this._lastTriggeredAnimation = (AnimationInstance) null;
      if (!this.Host.TriggeredAnimations.Any<KeyValuePair<AnimationInstance, Vector2>>((Func<KeyValuePair<AnimationInstance, Vector2>, bool>) (a => a.Key.Index == animation.Index)) && !this.Host.TriggeredAnimationsPositions.Any<KeyValuePair<AnimationInstance, Vector2>>((Func<KeyValuePair<AnimationInstance, Vector2>, bool>) (p => p.Value == this.PositionPrecise)))
      {
        animation.ResetFrame();
        animation.Cycles = 0;
        this.Host.TriggeredAnimations.Add(animation, position);
        this.Host.TriggeredAnimationsPositions.Add(animation, this.PositionPrecise);
      }
      if (this._index == 5 && animation.Index == 3 && this.Host._arrangementType == CPZBlobsInstance.ArrangementType.Straight)
      {
        KeyValuePair<AnimationInstance, Vector2> keyValuePair = this.Host.TriggeredAnimations.LastOrDefault<KeyValuePair<AnimationInstance, Vector2>>((Func<KeyValuePair<AnimationInstance, Vector2>, bool>) (a => a.Key.Index == 3));
        if (keyValuePair.Key == null)
          return;
        this.Host.BottomStartOverflowTriggeredAnimations.Add(keyValuePair.Key);
      }
      else
      {
        if (this._index != 5 || animation.Index != 3 || this.Host._arrangementType != CPZBlobsInstance.ArrangementType.Arc || this._velocity.Y >= 0.0 || this.Host.BottomStartOverflowTriggeredAnimations.Count != 0)
          return;
        KeyValuePair<AnimationInstance, Vector2> keyValuePair = this.Host.TriggeredAnimations.LastOrDefault<KeyValuePair<AnimationInstance, Vector2>>((Func<KeyValuePair<AnimationInstance, Vector2>, bool>) (a => a.Key.Index == 3));
        if (keyValuePair.Key == null)
          return;
        this.Host.BottomStartOverflowTriggeredAnimations.Add(keyValuePair.Key);
      }
    }

    protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
    {
      if (this.Host._arrangementType == CPZBlobsInstance.ArrangementType.Straight)
        this.StraightPrepareTriggers();
      IObjectRenderer objectRenderer = renderer.GetObjectRenderer();
      if (this.Host._arrangementType == CPZBlobsInstance.ArrangementType.Arc)
        this.DrawArc(objectRenderer, viewOptions);
      else
        this.DrawStraight(objectRenderer, viewOptions);
    }

    private void ArcPrepareTriggers()
    {
      Rectanglei absoluteBounds = this.CollisionRectangles[0].AbsoluteBounds;
      Rectangle rect1 = Rectangle.FromLTRB((double) this._downX - (double) absoluteBounds.Width * 0.5, (double) (this._originalY - absoluteBounds.Height), (double) this._downX + (double) absoluteBounds.Width * 0.5, (double) this._originalY + (double) absoluteBounds.Height * 0.5);
      Rectangle rect2 = Rectangle.FromLTRB((double) this._upX - (double) absoluteBounds.Width * 0.5, (double) (this._originalY - absoluteBounds.Height), (double) this._upX + (double) absoluteBounds.Width * 0.5, (double) this._originalY + (double) absoluteBounds.Height * 0.5);
      Vector2i position1;
      if (absoluteBounds.IntersectsWith((Rectanglei) rect2) && !this._leftToRight)
      {
        this._startAnimation = true;
        AnimationInstance bottomSplashStart = this._animationBottomSplashStart;
        double x = (double) (this._upX - this.Position.X);
        int num = this._originalY - 112 /*0x70*/;
        position1 = this.Position;
        int y1 = position1.Y;
        double y2 = (double) (num - y1);
        Vector2 position2 = new Vector2(x, y2);
        this.TriggerAnimation(bottomSplashStart, position2, true);
      }
      if (absoluteBounds.IntersectsWith((Rectanglei) rect1) && this._leftToRight)
      {
        this._startAnimation = true;
        AnimationInstance bottomSplashStart = this._animationBottomSplashStart;
        int downX = this._downX;
        position1 = this.Position;
        int x1 = position1.X;
        double x2 = (double) (downX - x1);
        int num = this._originalY - 112 /*0x70*/;
        position1 = this.Position;
        int y3 = position1.Y;
        double y4 = (double) (num - y3);
        Vector2 position3 = new Vector2(x2, y4);
        this.TriggerAnimation(bottomSplashStart, position3);
      }
      if (absoluteBounds.IntersectsWith((Rectanglei) rect2) && this._leftToRight)
      {
        if (this._index == 5)
        {
          AnimationInstance animationBottomSplashEnd = this._animationBottomSplashEnd;
          int upX = this._upX;
          position1 = this.Position;
          int x3 = position1.X;
          double x4 = (double) (upX - x3);
          int num = this._originalY - 120;
          position1 = this.Position;
          int y5 = position1.Y;
          double y6 = (double) (num - y5);
          Vector2 position4 = new Vector2(x4, y6);
          this.TriggerAnimation(animationBottomSplashEnd, position4, true);
        }
        else
        {
          AnimationInstance bottomSplashStart = this._animationBottomSplashStart;
          int upX = this._upX;
          position1 = this.Position;
          int x5 = position1.X;
          double x6 = (double) (upX - x5);
          int num = this._originalY - 112 /*0x70*/;
          position1 = this.Position;
          int y7 = position1.Y;
          double y8 = (double) (num - y7);
          Vector2 position5 = new Vector2(x6, y8);
          this.TriggerAnimation(bottomSplashStart, position5, true);
        }
      }
      if (!absoluteBounds.IntersectsWith((Rectanglei) rect1) || this._leftToRight)
        return;
      if (this._index == 5)
      {
        AnimationInstance animationBottomSplashEnd = this._animationBottomSplashEnd;
        int downX = this._downX;
        position1 = this.Position;
        int x7 = position1.X;
        double x8 = (double) (downX - x7);
        int num = this._originalY - 120;
        position1 = this.Position;
        int y9 = position1.Y;
        double y10 = (double) (num - y9);
        Vector2 position6 = new Vector2(x8, y10);
        this.TriggerAnimation(animationBottomSplashEnd, position6, true);
      }
      else
      {
        AnimationInstance bottomSplashStart = this._animationBottomSplashStart;
        int downX = this._downX;
        position1 = this.Position;
        int x9 = position1.X;
        double x10 = (double) (downX - x9);
        int num = this._originalY - 112 /*0x70*/;
        position1 = this.Position;
        int y11 = position1.Y;
        double y12 = (double) (num - y11);
        Vector2 position7 = new Vector2(x10, y12);
        this.TriggerAnimation(bottomSplashStart, position7, true);
      }
    }

    private void StraightPrepareTriggers()
    {
      if (this._velocity.Y > 0.0 && this.Position.Y > this._originalY - 224 /*0xE0*/ && this.Position.Y < this._originalY - 14)
      {
        this._startAnimation = true;
        this.TriggerAnimation(this._animationTopSplashStart, new Vector2((double) (this._downX - this.Position.X), (double) (this._originalY - 224 /*0xE0*/ - this.Position.Y)));
      }
      if (this._velocity.Y > 0.0 && this.Position.Y > this._originalY - 14)
      {
        if (this._index == 5)
          this.TriggerAnimation(this._animationBottomSplashEnd, new Vector2((double) (this._downX - this.Position.X), (double) (this._originalY - 120 - this.Position.Y)));
        else
          this.TriggerAnimation(this._animationBottomSplashStart, new Vector2((double) (this._downX - this.Position.X), (double) (this._originalY - 112 /*0x70*/ - this.Position.Y)), true);
      }
      if (this._velocity.Y < 0.0 && this.Position.Y < this._originalY - 224 /*0xE0*/ - 60)
      {
        if (this._index == 5)
          this.TriggerAnimation(this._animationTopSplashEnd, new Vector2((double) (this._upX - this.Position.X), (double) (this._originalY - 224 /*0xE0*/ - this.Position.Y + 24)), true);
        else
          this.TriggerAnimation(this._animationTopSplashStart, new Vector2((double) (this._upX - this.Position.X), (double) (this._originalY - 224 /*0xE0*/ - this.Position.Y + 24)), true);
      }
      if (this._velocity.Y >= 0.0 || this.Position.Y <= this._originalY - 14 || this.Position.Y <= this._originalY - 224 /*0xE0*/ - 60)
        return;
      this.TriggerAnimation(this._animationBottomSplashStart, new Vector2((double) (this._upX - this.Position.X), (double) (this._originalY - 112 /*0x70*/ - this.Position.Y)));
    }

    private void DrawArc(IObjectRenderer or, LayerViewOptions viewOptions)
    {
      Rectanglei clipRectangle = (Rectanglei) or.ClipRectangle;
      IObjectRenderer objectRenderer = or;
      Rectangle rectangle1 = or.ClipRectangle;
      double x = rectangle1.X;
      int num = this._originalY - 288 - 256 /*0x0100*/;
      rectangle1 = this.Level.Camera.Bounds;
      int y1 = (int) rectangle1.Y;
      double y2 = (double) (num - y1);
      rectangle1 = or.ClipRectangle;
      double width = rectangle1.Width;
      Rectangle rectangle2 = new Rectangle(x, y2, width, 512.0);
      objectRenderer.ClipRectangle = rectangle2;
      or.Render(this._animationBlob);
      or.ClipRectangle = (Rectangle) clipRectangle;
    }

    private void DrawStraight(IObjectRenderer or, LayerViewOptions viewOptions)
    {
      Rectangle clipRectangle = or.ClipRectangle;
      int num = Math.Min(this._downX, this._upX);
      or.ClipRectangle = new Rectangle((double) (num - 64 /*0x40*/ - (int) this.Level.Camera.Bounds.X), (double) (this._originalY - 288 - (int) this.Level.Camera.Bounds.Y), 512.0, 256.0);
      or.Render(this._animationBlob);
      or.ClipRectangle = clipRectangle;
    }
  }

  public class OverflowInstance : ParticleObject
  {
    public OverflowInstance()
      : base("/ANIGROUP")
    {
    }

    public void InitOverflow() => this._animationInstance.Index = 5;
  }
}
