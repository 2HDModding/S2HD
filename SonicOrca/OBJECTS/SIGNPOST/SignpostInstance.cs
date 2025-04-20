// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.SIGNPOST.SignpostInstance
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca;
using SonicOrca.Core;
using SonicOrca.Core.Objects;
using SonicOrca.Core.Objects.Metadata;
using SonicOrca.Extensions;
using SonicOrca.Geometry;
using SonicOrca.Graphics;
using System;
using System.Collections.Generic;

#nullable disable
namespace SONICORCA.OBJECTS.SIGNPOST;

public class SignpostInstance : ActiveObject
{
  private static readonly IReadOnlyList<Vector2i> SparkleParticleOffsets = (IReadOnlyList<Vector2i>) new Vector2i[4]
  {
    new Vector2i(16 /*0x10*/, 4),
    new Vector2i(-16, 12),
    new Vector2i(-12, -16),
    new Vector2i(20, -24)
  };
  private static Vector2i[] SparklePositionOffsets = new Vector2i[8]
  {
    new Vector2i(-96, -64),
    new Vector2i(32 /*0x20*/, 32 /*0x20*/),
    new Vector2i(-64, 32 /*0x20*/),
    new Vector2i(96 /*0x60*/, -32),
    new Vector2i(0, -32),
    new Vector2i(64 /*0x40*/, 0),
    new Vector2i(-96, 32 /*0x20*/),
    new Vector2i(96 /*0x60*/, 16 /*0x10*/)
  };
  private const int AnimationSide = 0;
  private const int AnimationRobotnik = 1;
  private const int AnimationSonic = 4;
  private const int AnimationTails = 7;
  private const int OffsetToFront = 0;
  private const int OffsetFront = 1;
  private const int OffsetToSide = 2;
  private const int CharacterIndexRobotnik = 0;
  private const int CharacterIndexSonic = 1;
  private const int CharacterIndexTails = 2;
  private Updater _updater;
  private AnimationInstance _animation;
  private bool _activated;
  private bool _triggedEndGame;
  private int _sparkleIndex;
  private int _sparkleDelay;
  private int _targetCameraBottom;

  [StateVariable]
  private bool Activated
  {
    get => this._activated;
    set => this._activated = value;
  }

  public SignpostInstance()
  {
    this.DesignBounds = new Rectanglei(-96, -96, 192 /*0xC0*/, 192 /*0xC0*/);
  }

  protected override void OnStart()
  {
    this._updater = new Updater(this.GetUpdates());
    this._animation = new AnimationInstance(this.ResourceTree, this.Type.GetAbsolutePath("/ANIGROUP"));
    this._animation.Index = 2;
    if (!this._activated)
      return;
    this._animation.Index = this.GetAnimationIndexFromCharacter(this.GetProtagonistCharacterIndex()) + 1;
    this._activated = true;
    this._triggedEndGame = true;
  }

  protected override void OnUpdate() => this._updater.Update();

  private IEnumerable<UpdateResult> GetUpdates()
  {
    SignpostInstance signpostInstance = this;
    if (!signpostInstance._triggedEndGame)
    {
      while (!signpostInstance._activated)
      {
        // ISSUE: explicit non-virtual call
        foreach (ICharacter character in __nonvirtual (signpostInstance.Level).ObjectManager.Characters)
        {
          // ISSUE: explicit non-virtual call
          // ISSUE: explicit non-virtual call
          if (!character.IsSidekick && !character.IsDebug && character.ShouldReactToLevel && character.Position.X >= __nonvirtual (signpostInstance.Position).X && character.LastPosition.X < __nonvirtual (signpostInstance.Position).X)
            signpostInstance.Activate(character);
        }
        yield return UpdateResult.Next();
      }
      int minSpins = 10;
      int spins = 0;
      int characterIndex = 0;
      int targetCharacterIndex = signpostInstance.GetProtagonistCharacterIndex();
      for (spins = 0; spins < minSpins || characterIndex != targetCharacterIndex; ++spins)
      {
        ++signpostInstance._animation.Index;
        while (signpostInstance._animation.Index != 0)
        {
          signpostInstance.UpdateCamera();
          signpostInstance.UpdateSparkles();
          yield return UpdateResult.Next();
        }
        characterIndex = (characterIndex + 1) % 3;
        int animationIndex = signpostInstance.GetAnimationIndexFromCharacter(characterIndex);
        signpostInstance._animation.Index = animationIndex;
        while (signpostInstance._animation.Index != animationIndex + 1)
        {
          signpostInstance.UpdateCamera();
          signpostInstance.UpdateSparkles();
          yield return UpdateResult.Next();
        }
      }
      // ISSUE: explicit non-virtual call
      Rectanglei bounds = __nonvirtual (signpostInstance.Level).Bounds;
      while (bounds.Bottom != signpostInstance._targetCameraBottom)
      {
        bounds.Bottom = MathX.GoTowards(bounds.Bottom, signpostInstance._targetCameraBottom, 6);
        // ISSUE: explicit non-virtual call
        __nonvirtual (signpostInstance.Level).Bounds = bounds;
        yield return UpdateResult.Next();
      }
      yield return UpdateResult.Wait(80 /*0x50*/);
      // ISSUE: explicit non-virtual call
      __nonvirtual (signpostInstance.Level).CompleteLevel();
      signpostInstance._triggedEndGame = true;
    }
  }

  private int GetAnimationIndexFromCharacter(int character)
  {
    switch (character)
    {
      case 1:
        return 4;
      case 2:
        return 7;
      default:
        return 1;
    }
  }

  private int GetProtagonistCharacterIndex()
  {
    switch (this.Level.Player.ProtagonistCharacterType)
    {
      case CharacterType.Tails:
        return 2;
      default:
        return 1;
    }
  }

  private void Activate(ICharacter character)
  {
    this._activated = true;
    this._targetCameraBottom = this.Position.Y + 340;
    this.Level.Bounds = this.Level.Bounds with
    {
      Bottom = Math.Max(this._targetCameraBottom, (int) this.Level.Camera.Bounds.Bottom)
    };
    this.Level.JustAboutToCompleteLevel();
    this.Level.SoundManager.PlaySound((IActiveObject) this, this.Type.GetAbsolutePath("SONICORCA/SOUND/SIGNPOST"));
    (this.Entry.State as SignpostInstance).Activated = true;
  }

  private void UpdateCamera()
  {
    Rectanglei bounds = this.Level.Bounds;
    bounds.Bottom = MathX.GoTowards(bounds.Bottom, this._targetCameraBottom, 6);
    this.Level.Bounds = bounds;
  }

  private void UpdateSparkles()
  {
    --this._sparkleDelay;
    if (this._sparkleDelay > 0)
      return;
    this.CreateSparkles(SignpostInstance.SparklePositionOffsets[this._sparkleIndex]);
    this._sparkleDelay = 12;
    this._sparkleIndex = (this._sparkleIndex + 1) % SignpostInstance.SparklePositionOffsets.Length;
  }

  private void CreateSparkles(Vector2i relativePosition)
  {
    Vector2i vector2i = this.Position + relativePosition;
    int ticks = 0;
    foreach (Vector2i sparkleParticleOffset in (IEnumerable<Vector2i>) SignpostInstance.SparkleParticleOffsets)
    {
      ObjectPlacement objectPlacement = new ObjectPlacement(this.Type.GetAbsolutePath("SONICORCA/OBJECTS/RING/SPARKLE"), this.Level.Map.Layers.IndexOf(this.Layer), vector2i + sparkleParticleOffset);
      this.Level.SetInterval(ticks, (Action) (() => this.Level.ObjectManager.AddObject(objectPlacement)));
      ticks += 2;
    }
  }

  protected override void OnAnimate() => this._animation.Animate();

  protected override void OnDraw(Renderer renderer, LayerViewOptions viewOptions)
  {
    renderer.GetObjectRenderer().Render(this._animation);
  }
}
