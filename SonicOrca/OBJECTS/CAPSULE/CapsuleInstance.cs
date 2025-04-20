// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.CAPSULE.CapsuleInstance
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Collision;
using SonicOrca.Core.Objects.Metadata;
using SonicOrca.Geometry;
using SonicOrca.Graphics;
using SonicOrca.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace SONICORCA.OBJECTS.CAPSULE;

public class CapsuleInstance : ActiveObject
{
  private const int AnimationMain = 0;
  private const int AnimationLights = 1;
  private const int AnimationDoor = 2;
  private const int AnimationDoorOpening = 3;
  private const int AnimationDoorLights = 4;
  private const int AnimationMiddle = 5;
  private const int AnimationMiddleOpening = 6;
  private const int AnimationButton = 7;
  private const int AnimationLock = 8;
  private const int AnimationLockLight = 9;
  private const int AnimationLockBroken = 10;
  private AnimationInstance _animationButton;
  private AnimationInstance _animationMain;
  private AnimationInstance _animationLights;
  private AnimationInstance _animationDoor;
  private AnimationInstance _animationDoorLights;
  private AnimationInstance _animationMiddle;
  private int _lastButtonY;
  private int _buttonY;
  private bool _buttonPressed;
  private CapsuleInstance.Lock _lock;
  private bool _doorOpened;
  private int _openDoorDelay;
  private int _completeLevelDelay;
  private bool _unlocked;

  [StateVariable]
  private bool Unlocked
  {
    get => this._unlocked;
    set => this._unlocked = value;
  }

  public CapsuleInstance()
  {
    this.DesignBounds = new Rectanglei((int) sbyte.MinValue, -160, 256 /*0x0100*/, 320);
  }

  protected override void OnStart()
  {
    AnimationGroup loadedResource = this.ResourceTree.GetLoadedResource<AnimationGroup>((ILoadedResource) this.Type, "/ANIGROUP");
    this._animationButton = new AnimationInstance(loadedResource, 7);
    this._animationMain = new AnimationInstance(loadedResource);
    if ((this.Entry.State as CapsuleInstance).Unlocked)
    {
      this._buttonPressed = true;
      this._doorOpened = true;
      this._animationLights = (AnimationInstance) null;
      this._animationDoorLights = (AnimationInstance) null;
    }
    else
    {
      this._animationLights = new AnimationInstance(loadedResource, 1);
      this._animationDoor = new AnimationInstance(loadedResource, 2);
      this._animationDoorLights = new AnimationInstance(loadedResource, 4);
      this._animationMiddle = new AnimationInstance(loadedResource, 5);
    }
    this._lock = this.Level.ObjectManager.AddSubObject<CapsuleInstance.Lock>((ActiveObject) this);
    this._lock.Position = this.Position + new Vector2i(0, -46);
    this._lock.LockLifetime = true;
    CollisionVector[] second = CollisionVector.FromRectangle((ActiveObject) this, new Rectanglei(-64, this._buttonY - 158, 128 /*0x80*/, 64 /*0x40*/));
    second[1].Flags = CollisionFlags.Conveyor;
    this.CollisionVectors = ((IEnumerable<CollisionVector>) new CollisionVector[10]
    {
      new CollisionVector((ActiveObject) this, new Vector2i((int) sbyte.MinValue, -42), new Vector2i(-100, -64)),
      new CollisionVector((ActiveObject) this, new Vector2i(-100, -64), new Vector2i(-64, -82)),
      new CollisionVector((ActiveObject) this, new Vector2i(-64, -82), new Vector2i(-28, -94)),
      new CollisionVector((ActiveObject) this, new Vector2i(-28, -94), new Vector2i(28, -94)),
      new CollisionVector((ActiveObject) this, new Vector2i(28, -94), new Vector2i(64 /*0x40*/, -82)),
      new CollisionVector((ActiveObject) this, new Vector2i(64 /*0x40*/, -82), new Vector2i(100, -64)),
      new CollisionVector((ActiveObject) this, new Vector2i(100, -64), new Vector2i(128 /*0x80*/, -42)),
      new CollisionVector((ActiveObject) this, new Vector2i(128 /*0x80*/, -42), new Vector2i(128 /*0x80*/, 160 /*0xA0*/)),
      new CollisionVector((ActiveObject) this, new Vector2i(128 /*0x80*/, 160 /*0xA0*/), new Vector2i((int) sbyte.MinValue, 160 /*0xA0*/)),
      new CollisionVector((ActiveObject) this, new Vector2i((int) sbyte.MinValue, 160 /*0xA0*/), new Vector2i((int) sbyte.MinValue, -42))
    }).Concat<CollisionVector>((IEnumerable<CollisionVector>) second).ToArray<CollisionVector>();
    for (int index = 0; index < 10; ++index)
      this.CollisionVectors[index].Flags |= CollisionFlags.NoAngle;
    this.CollisionVectors[7].Flags |= CollisionFlags.NoPathFollowing;
    this.CollisionVectors[9].Flags |= CollisionFlags.NoPathFollowing;
  }

  protected override void OnStop()
  {
    if (this._buttonPressed)
      return;
    this._lock.Finish();
  }

  protected override void OnUpdate()
  {
    this.UpdateButton();
    this.UpdateDoor();
    this.UpdateButtonCollision();
  }

  private void UpdateButtonCollision()
  {
    Vector2i vector2i = new Vector2i(0, this._buttonY - this._lastButtonY);
    for (int index = 10; index < 14; ++index)
    {
      CollisionVector collisionVector = this.CollisionVectors[index];
      collisionVector.RelativeA += vector2i;
      collisionVector.RelativeB += vector2i;
    }
  }

  private void UpdateButton()
  {
    this._lastButtonY = this._buttonY;
    if (this.Level.ObjectManager.IsCharacterStandingOn(this.CollisionVectors[11]))
    {
      int buttonY = this._buttonY;
      this._buttonY = Math.Min(this._buttonY + 8, 24);
      int num = this._buttonY - buttonY;
      if (this._buttonY != 24 || this._buttonPressed)
        return;
      this.OnButtonPress();
    }
    else
      this._buttonY = Math.Max(0, this._buttonY - 8);
  }

  private void OnButtonPress()
  {
    this.Level.JustAboutToCompleteLevel();
    (this.Entry.State as CapsuleInstance).Unlocked = true;
    this._animationLights = (AnimationInstance) null;
    this._animationDoorLights = (AnimationInstance) null;
    this._lock.Break();
    this._lock.LockLifetime = false;
    this._buttonPressed = true;
    this._openDoorDelay = 60;
    this.LockLifetime = true;
  }

  private void UpdateDoor()
  {
    if (!this._doorOpened)
    {
      if (!this._buttonPressed || this._openDoorDelay <= 0)
        return;
      --this._openDoorDelay;
      if (this._openDoorDelay != 0)
        return;
      this.OpenDoor();
    }
    else
    {
      if (this._completeLevelDelay <= 0)
        return;
      --this._completeLevelDelay;
      if (this._completeLevelDelay != 0)
        return;
      this.Level.CompleteLevel();
      this.LockLifetime = false;
    }
  }

  private void OpenDoor()
  {
    this._doorOpened = true;
    this._animationDoor.Index = 3;
    this._animationMiddle.Index = 6;
    this._animationDoor.Cycles = 0;
    this._animationMiddle.Cycles = 0;
    this.CreateAnimals();
  }

  private void CreateAnimals()
  {
    int[] array = Enumerable.Range(0, 32 /*0x20*/).Select<int, int>((Func<int, int>) (x => (int) ((double) x * 6.0) - 96 /*0x60*/)).OrderBy<int, int>((Func<int, int>) (x => this.Level.Random.Next())).ToArray<int>();
    int delay = 60;
    for (int index = 0; index < 32 /*0x20*/; ++index)
    {
      Vector2i vector2i = new Vector2i(array[index], 32 /*0x20*/);
      this.Level.CreateRandomAnimalObject(this.Level.Map.Layers.IndexOf(this.Layer), this.Position + vector2i, vector2i.X <= 0 ? -1 : 1, delay);
      delay += this.Level.Random.Next(5, 10);
    }
    this._completeLevelDelay = delay + 120;
  }

  protected override void OnAnimate()
  {
    this._animationButton.Animate();
    this._animationMain.Animate();
    if (this._animationLights != null)
      this._animationLights.Animate();
    if (this._animationDoor != null)
      this._animationDoor.Animate();
    if (this._animationDoorLights != null)
      this._animationDoorLights.Animate();
    if (this._animationMiddle == null)
      return;
    this._animationMiddle.Animate();
  }

  protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
  {
    IObjectRenderer objectRenderer = renderer.GetObjectRenderer();
    objectRenderer.Render(this._animationButton, new Vector2(0.0, (double) (this._buttonY - 128 /*0x80*/)));
    objectRenderer.Render(this._animationMain);
    if (!viewOptions.Shadows && this._animationLights != null)
      objectRenderer.Render(this._animationLights);
    if (this._animationDoor != null && this._animationDoor.Cycles == 0 || !this._doorOpened)
      objectRenderer.Render(this._animationDoor, new Vector2(0.0, 16.0));
    if (!viewOptions.Shadows && this._animationDoorLights != null)
    {
      objectRenderer.EmitsLight = true;
      objectRenderer.Render(this._animationDoorLights, new Vector2(0.0, 16.0));
      objectRenderer.EmitsLight = false;
    }
    if ((this._animationMiddle == null || this._animationMiddle.Cycles != 0) && this._doorOpened)
      return;
    objectRenderer.Render(this._animationMiddle, new Vector2(0.0, 16.0));
  }

  private class Lock : ActiveObject
  {
    private AnimationInstance _animation;
    private AnimationInstance _animationLight;
    private Vector2 _velocity = new Vector2(32.0, -16.0);
    private double _rotation;
    private bool _broken;

    protected override void OnStart()
    {
      AnimationGroup loadedResource = this.ResourceTree.GetLoadedResource<AnimationGroup>((ILoadedResource) this.Type, "/ANIGROUP");
      this._animation = new AnimationInstance(loadedResource, 8);
      this._animationLight = new AnimationInstance(loadedResource, 9);
    }

    protected override void OnUpdate()
    {
      base.OnUpdate();
      if (!this._broken)
        return;
      this.PositionPrecise = this.PositionPrecise + this._velocity;
      this._velocity += new Vector2(0.0, 0.875);
    }

    public void Break()
    {
      this._broken = true;
      this._animation.Index = 10;
      this.CreateExplosionObject();
    }

    private void CreateExplosionObject()
    {
      this.Level.ObjectManager.AddObject(new ObjectPlacement(this.Level.CommonResources.GetResourcePath("badnikexplosionobject"), this.Level.Map.Layers.IndexOf(this.Layer), this.Position));
    }

    protected override void OnAnimate()
    {
      this._animation.Animate();
      this._animationLight.Animate();
      if (!this._broken)
        return;
      this._rotation += Math.PI / 32.0;
    }

    protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
    {
      IObjectRenderer objectRenderer = renderer.GetObjectRenderer();
      using (objectRenderer.BeginMatixState())
      {
        if (this._rotation != 0.0)
          objectRenderer.ModelMatrix = objectRenderer.ModelMatrix.RotateZ(this._rotation);
        objectRenderer.Render(this._animation, (Vector2) new Vector2i(-2, 0));
        if (viewOptions.Shadows || this._broken)
          return;
        objectRenderer.EmitsLight = true;
        objectRenderer.Render(this._animationLight, (Vector2) new Vector2i(-2, 0));
      }
    }
  }
}
