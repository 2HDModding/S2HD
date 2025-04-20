// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.HTZVINELIFT.HTZVineLiftInstance
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

#nullable disable
namespace SONICORCA.OBJECTS.HTZVINELIFT;

public class HTZVineLiftInstance : ActiveObject
{
  private const int VerticalVinesOutdoorsAnimationIndex = 0;
  private const int VerticalVinesDetachOutdoorsAnimationIndex = 1;
  private const int VerticalVinesInsideAnimationIndex = 5;
  private const int VerticalVinesDetachInsideAnimationIndex = 6;
  private AnimationInstance _animation;
  private Vector2 _velocity;
  private int _duration = 60;
  private HTZVineLiftInstance.LiftDirection _direction;
  private bool _outdoors = true;
  private HTZVineLiftInstance.State _state;
  private HTZVineLiftInstance.LiftPlatform _platform;

  [StateVariable]
  private int Duration
  {
    get => this._duration;
    set => this._duration = value;
  }

  [StateVariable]
  private HTZVineLiftInstance.LiftDirection Direction
  {
    get => this._direction;
    set => this._direction = value;
  }

  [StateVariable]
  private bool Outdoors
  {
    get => this._outdoors;
    set => this._outdoors = value;
  }

  public HTZVineLiftInstance() => this.DesignBounds = new Rectanglei(-132, -41, 264, 82);

  protected override void OnStart()
  {
    base.OnStart();
    this._velocity = new Vector2(8.0, 4.0);
    if (this._direction == HTZVineLiftInstance.LiftDirection.Left)
      this._velocity.X *= -1.0;
    this.Priority = 1256;
    int index = 5;
    if (this._outdoors)
      index = 0;
    this._animation = new AnimationInstance(this.ResourceTree, this.Type.GetAbsolutePath("/ANIGROUP"), index);
    this._platform = this.Level.ObjectManager.AddSubObject<HTZVineLiftInstance.LiftPlatform>((ActiveObject) this);
    this._platform.Position = this.Position;
    this._platform.LockLifetime = true;
    if (this.Direction != HTZVineLiftInstance.LiftDirection.Left)
      return;
    this._platform.Position = new Vector2i(this.Position.X - 16 /*0x10*/, this.Position.Y);
  }

  protected override void OnUpdateEditor() => this._platform.Position = this.Position;

  protected override void OnUpdatePrepare()
  {
    switch (this._state)
    {
      case HTZVineLiftInstance.State.Waiting:
        if (!this._platform.Activated)
          break;
        this._state = HTZVineLiftInstance.State.Moving;
        break;
      case HTZVineLiftInstance.State.Moving:
        --this._duration;
        if (this._duration == 0)
        {
          this._state = HTZVineLiftInstance.State.Falling;
          this._velocity = new Vector2();
          this._platform.Velocity = new Vector2();
          if (this.Direction == HTZVineLiftInstance.LiftDirection.Left)
            this._platform.Position = new Vector2i(this._platform.Position.X - 16 /*0x10*/, this._platform.Position.Y);
          this._platform.BeginDetach();
          break;
        }
        this._platform.Velocity = this._velocity;
        break;
      case HTZVineLiftInstance.State.Falling:
        int num = 6;
        if (this._outdoors)
          num = 1;
        this._animation.Index = num;
        break;
    }
  }

  protected override void OnUpdate()
  {
    if (this._state != HTZVineLiftInstance.State.Moving)
      return;
    this.MovePrecise(this._velocity);
  }

  protected override void OnStop()
  {
    if (this._state == HTZVineLiftInstance.State.Falling)
      return;
    this._platform.FinishForever();
  }

  protected override void OnAnimate() => this._animation.Animate();

  protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
  {
    IObjectRenderer objectRenderer = renderer.GetObjectRenderer();
    bool flag = this._direction == HTZVineLiftInstance.LiftDirection.Left;
    AnimationInstance animation = this._animation;
    Vector2 destination = (Vector2) new Vector2i(-16, -8);
    int num = flag ? 1 : 0;
    objectRenderer.Render(animation, destination, num != 0);
  }

  private enum State
  {
    Waiting,
    Moving,
    Falling,
  }

  private enum LiftDirection
  {
    Left,
    Right,
  }

  private class LiftPlatform : ActiveObject, IPlatform, IActiveObject
  {
    private const int PlatformAnimationIndex = 4;
    private const double Gravity = 0.875;
    private static readonly int[] FallElasticYOffsets = new int[6]
    {
      0,
      20,
      40,
      40,
      60,
      60
    };
    private AnimationInstance _animation;
    private int _fallingTicks;
    private Vector2i _fallingOriginPosition;

    public bool Activated { get; set; }

    public bool Falling { get; set; }

    public Vector2 Velocity { get; set; }

    protected override void OnStart()
    {
      this.CollisionVectors = new CollisionVector[1]
      {
        new CollisionVector((ActiveObject) this, new Vector2i(-132, 17), new Vector2i(132, 17), flags: CollisionFlags.Conveyor)
      };
      this.Priority = 1256;
      this._animation = new AnimationInstance(this.ResourceTree, this.Type.GetAbsolutePath("/ANIGROUP"), 4);
    }

    protected override void OnUpdatePrepare()
    {
      if (!this.Falling)
        return;
      if (this._fallingTicks < 6)
      {
        int fallElasticYoffset = HTZVineLiftInstance.LiftPlatform.FallElasticYOffsets[this._fallingTicks];
        Vector2i fallingOriginPosition = this._fallingOriginPosition;
        fallingOriginPosition.Y += fallElasticYoffset;
        this.Velocity = (Vector2) fallingOriginPosition - this.PositionPrecise;
      }
      else
        this.Velocity += new Vector2(0.0, 0.875);
    }

    protected override void OnUpdate()
    {
      if (!this.Activated && this.Level.ObjectManager.IsCharacterStandingOn(this.CollisionVectors[0]))
        this.Activated = true;
      if (this.Falling)
        ++this._fallingTicks;
      else if (this.Activated && this.Level.Ticks % 16 /*0x10*/ == 0)
        this.Level.SoundManager.PlaySound((IActiveObject) this, this.Type.GetAbsolutePath("SONICORCA/SOUND/VINELIFT"));
      this.MovePrecise(this.Velocity);
    }

    protected override void OnStop()
    {
      if (!this.Falling)
        return;
      this.FinishForever();
    }

    protected override void OnAnimate() => this._animation.Animate();

    public void BeginDetach()
    {
      this.Falling = true;
      this.LockLifetime = false;
      this._fallingOriginPosition = this.Position;
    }

    protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
    {
      if (!this.Falling || this._fallingTicks < 7)
        return;
      renderer.GetObjectRenderer().Render(this._animation, (Vector2) new Vector2i(8, 13));
    }
  }
}
