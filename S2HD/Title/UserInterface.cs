// Decompiled with JetBrains decompiler
// Type: S2HD.Title.UserInterface
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca;
using SonicOrca.Audio;
using SonicOrca.Core;
using SonicOrca.Extensions;
using SonicOrca.Geometry;
using SonicOrca.Graphics;
using SonicOrca.Input;
using SonicOrca.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace S2HD.Title;

internal class UserInterface
{
  private readonly EffectEventManager _effectEventManager = new EffectEventManager();
  private readonly S2HDSonicOrcaGameContext _gameContext;
  private readonly TitleGameState _titleGameState;
  private readonly IMaskRenderer _maskRenderer;
  [ResourcePath("SONICORCA/TITLE/SELECTIONMARKER")]
  private ITexture _textureSelectionMarker;
  [ResourcePath("SONICORCA/TITLE/ZIGZAG")]
  private ITexture _textureZigZag;
  [ResourcePath("SONICORCA/MENU/LEFT")]
  private ITexture _textureLeftArrow;
  [ResourcePath("SONICORCA/MENU/RIGHT")]
  private ITexture _textureRightArrow;
  [ResourcePath("SONICORCA/FONTS/IMPACT/REGULAR")]
  private Font _fontImpactRegular;
  [ResourcePath("SONICORCA/FONTS/IMPACT/ITALIC")]
  private Font _fontImpactItalic;
  [ResourcePath("SONICORCA/SOUND/NAVIGATE/CURSOR")]
  private Sample _sampleNavigateCursor;
  [ResourcePath("SONICORCA/SOUND/NAVIGATE/BACK")]
  private Sample _sampleNavigateBack;
  [ResourcePath("SONICORCA/SOUND/NAVIGATE/YES")]
  private Sample _sampleNavigateYes;
  private int _ticks;
  private bool _pressStartActive;
  private int _selectionIndex;
  private bool _busy;
  private Vector2 _pressStartScale;
  private double _pressStartOpacity;
  private double _pressStartWhiteAdditive;
  private double _textOpacity;
  private double _textWhiteAdditive;
  private readonly Vector2[] _selectedMenuItemMarkerPositions = new Vector2[2];
  private int _levelSelectInputState;
  private bool _levelSelectEnabled;
  private int _levelSelectSelectionIndex;
  private UserInterface.LevelSelectItem[] _levelSelectItems;
  private UserInterface.MenuItem[] _menuItems;
  private UserInterface.MenuItemWidget[] _menuItemWidgets;
  private AnimationInstance _miniSonicAniInstance;
  private AnimationInstance _miniTailsAniInstance;
  private int _characterSelectionIndex;
  private bool _characterSelectActive;
  private double _characterSelectOpacity;
  private const int DemoInitialTimeout = 720;
  private int? _demoTimeout;
  private const int CharacterSelectTime = 60;
  private int? _characterSelectTimer;
  private bool _characterSelected;
  private static readonly EaseTimeline PressStartScaleXTimeline = new EaseTimeline(new EaseTimeline.Entry[3]
  {
    new EaseTimeline.Entry(5, 1.0),
    new EaseTimeline.Entry(10, 1.2),
    new EaseTimeline.Entry(20, 0.0)
  });
  private static readonly EaseTimeline PressStartScaleYTimeline = new EaseTimeline(new EaseTimeline.Entry[4]
  {
    new EaseTimeline.Entry(0, 1.0),
    new EaseTimeline.Entry(5, 1.45),
    new EaseTimeline.Entry(10, 1.0),
    new EaseTimeline.Entry(20, 0.0)
  });
  private static readonly EaseTimeline PressStartWhiteAdditiveTimeline = new EaseTimeline(new EaseTimeline.Entry[2]
  {
    new EaseTimeline.Entry(0, 0.0),
    new EaseTimeline.Entry(10, 1.0)
  });
  private static readonly EaseTimeline PressStartOpacityTimeline = new EaseTimeline(new EaseTimeline.Entry[2]
  {
    new EaseTimeline.Entry(10, 1.0),
    new EaseTimeline.Entry(20, 0.0)
  });
  private static readonly EaseTimeline TextOpacityTimeline = new EaseTimeline(new EaseTimeline.Entry[2]
  {
    new EaseTimeline.Entry(20, 0.0),
    new EaseTimeline.Entry(30, 1.0)
  });
  private static readonly EaseTimeline TextWhiteAdditiveTimeline = new EaseTimeline(new EaseTimeline.Entry[2]
  {
    new EaseTimeline.Entry(20, 1.0),
    new EaseTimeline.Entry(30, 0.0)
  });
  private static readonly EaseTimeline MenuItemOpacityEaseTimeline = new EaseTimeline(new EaseTimeline.Entry[5]
  {
    new EaseTimeline.Entry(260, 0.0),
    new EaseTimeline.Entry(560, 0.5),
    new EaseTimeline.Entry(960, 1.0),
    new EaseTimeline.Entry(1360, 0.5),
    new EaseTimeline.Entry(1660, 0.0)
  });
  private static readonly EaseTimeline ActivatedTextScaleTimeline = new EaseTimeline(new EaseTimeline.Entry[3]
  {
    new EaseTimeline.Entry(0, 1.0),
    new EaseTimeline.Entry(15, 0.8),
    new EaseTimeline.Entry(30, 1.4)
  });
  private static readonly EaseTimeline ActivatedTextOpacityTimeline = new EaseTimeline(new EaseTimeline.Entry[2]
  {
    new EaseTimeline.Entry(15, 1.0),
    new EaseTimeline.Entry(30, 0.0)
  });

  public bool Visible { get; set; }

  private bool IsSonicActive
  {
    get => this._characterSelectionIndex == 0 || this._characterSelectionIndex == 1;
  }

  private bool IsTailsActive
  {
    get => this._characterSelectionIndex == 0 || this._characterSelectionIndex == 2;
  }

  public UserInterface(
    S2HDSonicOrcaGameContext gameContext,
    TitleGameState titleGameState,
    IMaskRenderer maskRenderer)
  {
    this._gameContext = gameContext;
    this._titleGameState = titleGameState;
    this._maskRenderer = maskRenderer;
    ResourceTree resourceTree = this._gameContext.ResourceTree;
    resourceTree.FullfillLoadedResourcesByAttribute((object) this);
    this._miniSonicAniInstance = new AnimationInstance(resourceTree, "SONICORCA/TITLE/ANIGROUP", 11);
    this._miniTailsAniInstance = new AnimationInstance(resourceTree, "SONICORCA/TITLE/ANIGROUP", 13);
    this.InitialiseLevelSelect();
  }

  public void Reset()
  {
    this._ticks = 0;
    this._pressStartActive = true;
    this._pressStartScale = new Vector2(1.0);
    this._pressStartOpacity = 1.0;
    this._pressStartWhiteAdditive = 0.0;
    this._textWhiteAdditive = 0.0;
    this._selectionIndex = 0;
    this._levelSelectInputState = 0;
    this._levelSelectEnabled = false;
    this._levelSelectSelectionIndex = 0;
    this._effectEventManager.Clear();
    this._demoTimeout = new int?(720);
    this._characterSelectTimer = new int?(60);
    this.InitialiseMenuItemWidgets();
  }

  public void Update()
  {
    if (!this.Visible)
      return;
    if (this._characterSelectActive)
    {
      this._miniSonicAniInstance.Animate();
      this._miniTailsAniInstance.Animate();
      this._characterSelectOpacity = MathX.GoTowards(this._characterSelectOpacity, 1.0, 1.0 / 15.0);
      this._textOpacity = 1.0 - this._characterSelectOpacity;
    }
    else
    {
      this._characterSelectOpacity = MathX.GoTowards(this._characterSelectOpacity, 0.0, 1.0 / 15.0);
      this._textOpacity = 1.0 - this._characterSelectOpacity;
    }
    this._effectEventManager.Update();
    this.HandleInput();
    ++this._ticks;
    int? nullable;
    if (this._demoTimeout.HasValue)
    {
      int? demoTimeout = this._demoTimeout;
      this._demoTimeout = demoTimeout.HasValue ? new int?(demoTimeout.GetValueOrDefault() - 1) : new int?();
      nullable = this._demoTimeout;
      int num = 0;
      if ((nullable.GetValueOrDefault() <= num ? (nullable.HasValue ? 1 : 0) : 0) != 0)
      {
        this._demoTimeout = new int?();
        this.StartDemo();
      }
    }
    if (!this._characterSelected)
      return;
    nullable = this._characterSelectTimer;
    this._characterSelectTimer = nullable.HasValue ? new int?(nullable.GetValueOrDefault() - 1) : new int?();
    nullable = this._characterSelectTimer;
    int num1 = 0;
    if ((nullable.GetValueOrDefault() == num1 ? (nullable.HasValue ? 1 : 0) : 0) == 0)
      return;
    this.OnSelectCharacter();
  }

  private void HandleInput()
  {
    Controller pressed1 = this._gameContext.Pressed[0];
    InputState pressed2 = this._gameContext.Input.Pressed;
    if (this._busy)
      return;
    if (!this._levelSelectEnabled)
    {
      int selectControllerInput = this.GetLevelSelectControllerInput(pressed1);
      if (selectControllerInput != -1)
      {
        if (this._levelSelectInputState <= 3)
        {
          if (this._levelSelectInputState != selectControllerInput)
            this._levelSelectInputState = 0;
          else
            ++this._levelSelectInputState;
        }
        else if (selectControllerInput == 4 || selectControllerInput == 7)
        {
          if (this._gameContext.Current[0].Action1 && selectControllerInput == 7)
          {
            this._levelSelectInputState = 0;
            this._levelSelectEnabled = true;
            this._gameContext.Audio.PlaySound(this._sampleNavigateYes);
            return;
          }
        }
        else
          this._levelSelectInputState = 0;
      }
    }
    if (this._levelSelectEnabled)
    {
      if (Math.Abs(pressed1.DirectionLeft.Y) >= 0.5)
      {
        if (Math.Sign(pressed1.DirectionLeft.Y) < 0)
        {
          this._levelSelectSelectionIndex = UserInterface.NegMod(this._levelSelectSelectionIndex - 1, this._levelSelectItems.Length);
          this._gameContext.Audio.PlaySound(this._sampleNavigateCursor);
        }
        else
        {
          this._levelSelectSelectionIndex = UserInterface.NegMod(this._levelSelectSelectionIndex + 1, this._levelSelectItems.Length);
          this._gameContext.Audio.PlaySound(this._sampleNavigateCursor);
        }
      }
      if (pressed1.Start)
      {
        this._gameContext.Audio.PlaySound(this._sampleNavigateYes);
        this.OnLevelSelectStart();
      }
      if (!UserInterface.BackPressed(pressed2))
        return;
      this._gameContext.Audio.PlaySound(this._sampleNavigateBack);
      this._levelSelectEnabled = false;
    }
    else if (this._pressStartActive)
    {
      if (!pressed1.Start)
        return;
      this._effectEventManager.BeginEvent(this.EffectPressStart());
      this._pressStartActive = false;
      this._demoTimeout = new int?();
      this._gameContext.Audio.PlaySound(this._sampleNavigateYes);
    }
    else if (this._characterSelectActive)
    {
      if (Math.Abs(pressed1.DirectionLeft.X) >= 0.5)
      {
        this._characterSelectionIndex = Math.Sign(pressed1.DirectionLeft.X) >= 0 ? (this._characterSelectionIndex + 1) % 3 : (this._characterSelectionIndex - 1 + 3) % 3;
        this._miniSonicAniInstance.Index = 11;
        this._miniTailsAniInstance.Index = 13;
        this._gameContext.Audio.PlaySound(this._sampleNavigateCursor);
      }
      if (UserInterface.BackPressed(pressed2))
      {
        this._characterSelectActive = false;
        this._gameContext.Audio.PlaySound(this._sampleNavigateBack);
      }
      else
      {
        if (!pressed1.Start)
          return;
        this._gameContext.Audio.PlaySound(this._sampleNavigateYes);
        this._miniSonicAniInstance.Index = this.IsSonicActive ? 12 : 11;
        this._miniTailsAniInstance.Index = this.IsTailsActive ? 14 : 13;
        this._characterSelected = true;
      }
    }
    else
    {
      if (UserInterface.BackPressed(pressed2))
      {
        this._demoTimeout = new int?(720);
        this._pressStartActive = true;
        this._pressStartOpacity = 1.0;
        this._pressStartScale = new Vector2(1.0);
        this._pressStartWhiteAdditive = 0.0;
        this._selectionIndex = 0;
        this.InitialiseMenuItemWidgets();
        this._gameContext.Audio.PlaySound(this._sampleNavigateBack);
      }
      if (Math.Abs(pressed1.DirectionLeft.X) >= 0.5)
      {
        int direction = Math.Sign(pressed1.DirectionLeft.X);
        this._selectionIndex = direction >= 0 ? (this._selectionIndex + 1) % this._menuItems.Length : (this._selectionIndex - 1 + this._menuItems.Length) % this._menuItems.Length;
        this._effectEventManager.BeginEvent(this.EffectNavigateMenu(direction));
        this._gameContext.Audio.PlaySound(this._sampleNavigateCursor);
      }
      if (!pressed1.Start)
        return;
      Action action = this._menuItems[this._selectionIndex].Action;
      if (action != null)
        action();
      this._gameContext.Audio.PlaySound(this._sampleNavigateYes);
    }
  }

  private static bool BackPressed(InputState inputState)
  {
    GamePadInputState gamePadInputState = inputState.GamePad[0];
    return inputState.Keyboard[41] || gamePadInputState.Select || gamePadInputState.East;
  }

  private int GetLevelSelectControllerInput(Controller pressed)
  {
    if (pressed.DirectionLeft.Y <= -0.5)
      return 0;
    if (pressed.DirectionLeft.Y >= 0.5)
      return 1;
    if (pressed.DirectionLeft.X <= -0.5)
      return 2;
    if (pressed.DirectionLeft.X >= 0.5)
      return 3;
    if (pressed.Action1)
      return 4;
    if (pressed.Action2)
      return 5;
    if (pressed.Action3)
      return 6;
    return pressed.Start ? 7 : -1;
  }

  private void InitialiseMenuItemWidgets()
  {
    this._menuItems = new UserInterface.MenuItem[3]
    {
      new UserInterface.MenuItem()
      {
        Text = "NEW GAME",
        Action = new Action(this.OnSelectNewGame)
      },
      new UserInterface.MenuItem()
      {
        Text = "OPTIONS",
        Action = new Action(this.OnSelectOptions)
      },
      new UserInterface.MenuItem()
      {
        Text = "QUIT",
        Action = new Action(this.OnSelectQuit)
      }
    };
    this._menuItemWidgets = new UserInterface.MenuItemWidget[5];
    for (int index1 = -2; index1 <= 2; ++index1)
    {
      int index2 = (this._selectionIndex + index1 + this._menuItems.Length) % this._menuItems.Length;
      UserInterface.MenuItem menuItem = this._menuItems[index2];
      UserInterface.MenuItemWidget menuItemWidget = new UserInterface.MenuItemWidget()
      {
        MenuItemIndex = index2,
        OriginOffset = index1,
        X = (float) (960 + 400 * index1),
        Scale = new Vector2(1.0)
      };
      menuItemWidget.Opacity = (float) UserInterface.MenuItemOpacityEaseTimeline.GetValueAt((int) menuItemWidget.X);
      this._menuItemWidgets[index1 + 2] = menuItemWidget;
    }
    this.SetSelectionMarkerPositions();
  }

  private void SetSelectionMarkerPositions()
  {
    UserInterface.MenuItemWidget menuItemWidget = ((IEnumerable<UserInterface.MenuItemWidget>) this._menuItemWidgets).First<UserInterface.MenuItemWidget>((Func<UserInterface.MenuItemWidget, bool>) (x => x.OriginOffset == 0));
    int markerOffset = this.GetMarkerOffset(this._selectionIndex);
    this._selectedMenuItemMarkerPositions[0] = new Vector2((double) menuItemWidget.X - (double) markerOffset, 900.0);
    this._selectedMenuItemMarkerPositions[1] = new Vector2((double) menuItemWidget.X + (double) markerOffset, 900.0);
  }

  private int GetMenuItemWidth(int index)
  {
    return (int) this._fontImpactRegular.MeasureString(this._menuItems[index].Text).Width;
  }

  private int GetMarkerOffset(int index) => this.GetMenuItemWidth(index) / 2 + 48 /*0x30*/;

  private void InitialiseLevelSelect()
  {
    List<UserInterface.LevelSelectItem> levelSelectItemList = new List<UserInterface.LevelSelectItem>();
    int num = 1;
    foreach (LevelInfo level in (IEnumerable<LevelInfo>) Levels.LevelList)
    {
      if (!level.Unreleased)
      {
        for (int index = 1; index <= level.Acts; ++index)
        {
          string upper = $"{level.Name} ACT {index}".ToUpper();
          levelSelectItemList.Add(new UserInterface.LevelSelectItem()
          {
            Text = upper,
            Mnemonic = level.Mnemonic,
            Act = index,
            Number = num
          });
        }
        ++num;
      }
    }
    this._levelSelectItems = levelSelectItemList.ToArray();
  }

  private void OnSelectNewGame() => this._characterSelectActive = true;

  private void OnSelectCharacter()
  {
    this._busy = true;
    this._effectEventManager.BeginEvent(this.EffectFadeOut());
    this._titleGameState.Result = TitleGameState.ResultType.NewGame;
    LevelPrepareSettings lps = new LevelPrepareSettings()
    {
      AreaResourceKey = Levels.GetAreaResourceKey("ehz"),
      Act = 1,
      LevelNumber = 1
    };
    this.ApplyCharacterSelection(lps);
    this._titleGameState.LevelPrepareSettings = lps;
  }

  private void OnSelectOptions()
  {
    this._busy = true;
    this._effectEventManager.BeginEvent(this.EffectFadeOut());
    this._titleGameState.Result = TitleGameState.ResultType.ShowOptions;
  }

  private void OnSelectQuit()
  {
    this._busy = true;
    this._effectEventManager.BeginEvent(this.EffectFadeOut());
    this._titleGameState.Result = TitleGameState.ResultType.Quit;
  }

  private void StartDemo()
  {
    this._busy = true;
    this._effectEventManager.BeginEvent(this.EffectFadeOut());
    this._titleGameState.Result = TitleGameState.ResultType.StartDemo;
  }

  private void OnLevelSelectStart()
  {
    this._busy = true;
    UserInterface.LevelSelectItem levelSelectItem = this._levelSelectItems[this._levelSelectSelectionIndex];
    LevelPrepareSettings lps = new LevelPrepareSettings()
    {
      AreaResourceKey = Levels.GetAreaResourceKey(levelSelectItem.Mnemonic),
      Act = levelSelectItem.Act,
      LevelNumber = levelSelectItem.Number
    };
    this.ApplyCharacterSelection(lps);
    this._titleGameState.LevelPrepareSettings = lps;
    this._titleGameState.FadeOut();
    this._titleGameState.Result = TitleGameState.ResultType.LevelSelect;
  }

  private void ApplyCharacterSelection(LevelPrepareSettings lps)
  {
    switch (this._characterSelectionIndex)
    {
      case 0:
        lps.ProtagonistCharacter = CharacterType.Sonic;
        lps.SidekickCharacter = CharacterType.Tails;
        break;
      case 1:
        lps.ProtagonistCharacter = CharacterType.Sonic;
        lps.SidekickCharacter = CharacterType.Null;
        break;
      case 2:
        lps.ProtagonistCharacter = CharacterType.Tails;
        lps.SidekickCharacter = CharacterType.Null;
        break;
    }
  }

  private IEnumerable<UpdateResult> EffectPressStart()
  {
    this._busy = true;
    for (int t = 0; t <= 50; ++t)
    {
      this._pressStartScale = new Vector2(UserInterface.PressStartScaleXTimeline.GetValueAt(t), UserInterface.PressStartScaleYTimeline.GetValueAt(t));
      this._pressStartWhiteAdditive = UserInterface.PressStartWhiteAdditiveTimeline.GetValueAt(t);
      this._pressStartOpacity = UserInterface.PressStartOpacityTimeline.GetValueAt(t);
      this._textOpacity = UserInterface.TextOpacityTimeline.GetValueAt(t);
      this._textWhiteAdditive = UserInterface.TextWhiteAdditiveTimeline.GetValueAt(t);
      yield return UpdateResult.Next();
    }
    this._busy = false;
  }

  private IEnumerable<UpdateResult> EffectNavigateMenu(int direction)
  {
    this._busy = true;
    int markerOffset = this.GetMarkerOffset(UserInterface.NegMod(this._selectionIndex - direction, this._menuItems.Length));
    double markerVelocityX = (double) ((this.GetMarkerOffset(this._selectionIndex) - markerOffset) / 7);
    double markerVelocityY = 7.0;
    float velocity = (float) (direction * -1) * 26.666666f;
    for (int t = 0; t < 15; ++t)
    {
      foreach (UserInterface.MenuItemWidget menuItemWidget in this._menuItemWidgets)
      {
        menuItemWidget.X += velocity;
        menuItemWidget.Opacity = (float) UserInterface.MenuItemOpacityEaseTimeline.GetValueAt((int) menuItemWidget.X);
      }
      if (t <= 7)
      {
        for (int index = 0; index < this._selectedMenuItemMarkerPositions.Length; ++index)
          this._selectedMenuItemMarkerPositions[index].Y -= markerVelocityY;
      }
      else
      {
        this._selectedMenuItemMarkerPositions[0].X -= markerVelocityX;
        this._selectedMenuItemMarkerPositions[1].X += markerVelocityX;
        this._selectedMenuItemMarkerPositions[0].Y += markerVelocityY;
        this._selectedMenuItemMarkerPositions[1].Y += markerVelocityY;
      }
      yield return UpdateResult.Next();
    }
    foreach (UserInterface.MenuItemWidget menuItemWidget in this._menuItemWidgets)
    {
      int num = menuItemWidget.OriginOffset - direction;
      switch (num)
      {
        case -3:
          num = 2;
          menuItemWidget.MenuItemIndex = UserInterface.NegMod(menuItemWidget.MenuItemIndex - 1, this._menuItems.Length);
          break;
        case 3:
          num = -2;
          menuItemWidget.MenuItemIndex = UserInterface.NegMod(menuItemWidget.MenuItemIndex + 1, this._menuItems.Length);
          break;
      }
      menuItemWidget.X = (float) (960 + 400 * num);
      menuItemWidget.Opacity = (float) UserInterface.MenuItemOpacityEaseTimeline.GetValueAt((int) menuItemWidget.X);
      menuItemWidget.OriginOffset = num;
    }
    this.SetSelectionMarkerPositions();
    this._busy = false;
  }

  private IEnumerable<UpdateResult> EffectFadeOut()
  {
    UserInterface userInterface = this;
    // ISSUE: reference to a compiler-generated method
    UserInterface.MenuItemWidget widget = ((IEnumerable<UserInterface.MenuItemWidget>) userInterface._menuItemWidgets).First<UserInterface.MenuItemWidget>(new Func<UserInterface.MenuItemWidget, bool>(userInterface.\u003CEffectFadeOut\u003Eb__79_0));
    userInterface._titleGameState.Background.WipeOut();
    for (int t = 0; t <= 30; ++t)
    {
      if (t == 15)
        userInterface._titleGameState.FadeOut();
      widget.Scale = new Vector2(UserInterface.ActivatedTextScaleTimeline.GetValueAt(t));
      userInterface._textOpacity = UserInterface.ActivatedTextOpacityTimeline.GetValueAt(t);
      yield return UpdateResult.Next();
    }
  }

  public void Draw(Renderer renderer)
  {
    if (!this.Visible)
      return;
    this.DrawPressStart(renderer);
    if (!this._pressStartActive)
    {
      if (this._characterSelectOpacity != 0.0)
        this.DrawCharacterSelect(renderer);
      if (this._characterSelectOpacity != 1.0)
        this.DrawMenuItems(renderer);
    }
    if (!this._levelSelectEnabled)
      return;
    this.DrawLevelSelect(renderer);
  }

  private void DrawPressStart(Renderer renderer)
  {
    I2dRenderer g = renderer.Get2dRenderer();
    IFontRenderer fontRenderer = renderer.GetFontRenderer();
    Vector2i vector2i1 = new Vector2i(960, 900);
    if (this._pressStartOpacity == 0.0)
      return;
    g.BlendMode = BlendMode.Alpha;
    using (g.BeginMatixState())
    {
      g.ModelMatrix = g.ModelMatrix.Scale(this._pressStartScale.X, this._pressStartScale.Y);
      g.ModelMatrix = g.ModelMatrix.Translate((double) vector2i1.X, (double) vector2i1.Y);
      g.AdditiveColour = new Colour(this._pressStartWhiteAdditive);
      fontRenderer.RenderStringWithShadow("PRESS START", new Rectangle(), FontAlignment.Centre, this._fontImpactItalic, new Colour(this._pressStartOpacity, 1.0, 1.0, 1.0), new int?(0));
      g.AdditiveColour = Colours.Transparent;
    }
    int width1 = (int) this._fontImpactItalic.MeasureString("PRESS START").Width;
    int width2 = 79;
    int num1 = 16 /*0x10*/;
    Vector2i vector2i2 = new Vector2i(width1 / 2 + num1 + width2 / 2, 0);
    int width3 = this._textureZigZag.Width;
    int num2 = this._ticks / 2;
    int wrapOffsetX1 = width3 - num2 % width3;
    int wrapOffsetX2 = num2 % width3;
    this.DrawZigZag(g, new Rectanglei(vector2i1.X - vector2i2.X - width2 / 2, vector2i1.Y, width2, 0), wrapOffsetX1);
    this.DrawZigZag(g, new Rectanglei(vector2i1.X + vector2i2.X - width2 / 2, vector2i1.Y, width2, 0), wrapOffsetX2);
  }

  private void DrawZigZag(I2dRenderer g, Rectanglei rect, int wrapOffsetX)
  {
    rect.Y -= this._textureZigZag.Height / 2;
    rect.Height = this._textureZigZag.Height;
    Rectangle clipRectangle = g.ClipRectangle;
    g.ClipRectangle = (Rectangle) rect;
    Rectanglei destination = rect with
    {
      X = rect.X - wrapOffsetX,
      Width = this._textureZigZag.Width
    };
    for (; destination.X < rect.Right; destination.X += destination.Width)
      g.RenderTexture(this._textureZigZag, (Rectangle) destination);
    g.ClipRectangle = clipRectangle;
  }

  private void DrawMenuItems(Renderer renderer)
  {
    I2dRenderer obj = renderer.Get2dRenderer();
    renderer.GetFontRenderer();
    int y = 900;
    foreach (UserInterface.MenuItemWidget menuItemWidget in this._menuItemWidgets)
    {
      UserInterface.MenuItem menuItem = this._menuItems[menuItemWidget.MenuItemIndex];
      bool selected = menuItemWidget.MenuItemIndex == this._selectionIndex;
      this.DrawMenuItem(renderer, menuItem.Text, new Vector2i((int) menuItemWidget.X, y), (double) menuItemWidget.Opacity, menuItemWidget.Scale, selected);
    }
    obj.Colour = new Colour(this._textOpacity, 1.0, 1.0, 1.0);
    obj.AdditiveColour = new Colour(this._textWhiteAdditive);
    foreach (Vector2 itemMarkerPosition in this._selectedMenuItemMarkerPositions)
      obj.RenderTexture(this._textureSelectionMarker, (Vector2) (Vector2i) itemMarkerPosition);
    obj.AdditiveColour = Colours.Transparent;
  }

  private void DrawMenuItem(
    Renderer renderer,
    string text,
    Vector2i position,
    double opacity,
    Vector2 scale,
    bool selected = false)
  {
    I2dRenderer obj = renderer.Get2dRenderer();
    IFontRenderer fontRenderer = renderer.GetFontRenderer();
    int num = 1;
    if (!selected)
      num = 0;
    if (this._textOpacity < 1.0)
      opacity *= this._textOpacity;
    double a = opacity * opacity * opacity;
    Colour colour = new Colour(opacity, 1.0, 1.0, 1.0);
    Colour shadowColour = new Colour(a, 0.0, 0.0, 0.0);
    obj.AdditiveColour = new Colour(this._textWhiteAdditive);
    using (obj.BeginMatixState())
    {
      obj.ModelMatrix = obj.ModelMatrix.Scale(scale);
      obj.ModelMatrix = obj.ModelMatrix.Translate((Vector2) position);
      FontAlignment fontAlignment = FontAlignment.Centre;
      Font fontImpactRegular = this._fontImpactRegular;
      fontRenderer.RenderStringWithShadow(text, new Rectangle(), fontAlignment, fontImpactRegular, colour, new int?(num), fontImpactRegular.DefaultShadow, shadowColour);
    }
    obj.AdditiveColour = Colours.Transparent;
  }

  private void DrawCharacterSelect(Renderer renderer)
  {
    double characterSelectOpacity = this._characterSelectOpacity;
    Colour colour1 = new Colour(characterSelectOpacity, 0.25, 0.25, 0.25);
    Colour colour2 = new Colour(characterSelectOpacity, 1.0, 1.0, 1.0);
    string text1 = new string[3]
    {
      "SONIC & TAILS",
      "SONIC",
      "TAILS"
    }[this._characterSelectionIndex];
    I2dRenderer renderer1 = renderer.Get2dRenderer();
    IFontRenderer fontRenderer = renderer.GetFontRenderer();
    Rectangle rectangle1 = new Rectangle(0.0, 950.0, 1920.0, 60.0);
    renderer1.BlendMode = BlendMode.Alpha;
    renderer1.RenderQuad(new Colour(0.3 * characterSelectOpacity, 0.0, 0.0, 0.0), rectangle1);
    string text2 = text1;
    Rectangle boundary = rectangle1;
    Font fontImpactRegular = this._fontImpactRegular;
    Colour colour3 = Colour.FromOpacity(characterSelectOpacity);
    int? overlay = new int?(1);
    fontRenderer.RenderStringWithShadow(text2, boundary, FontAlignment.Centre, fontImpactRegular, colour3, overlay);
    renderer1.Colour = Colour.FromOpacity(characterSelectOpacity);
    Rectangle rectangle2 = this._fontImpactRegular.MeasureString(text1, rectangle1, FontAlignment.Centre);
    renderer1.RenderTexture(this._textureLeftArrow, new Vector2(rectangle2.Left - 50.0, rectangle2.CentreY));
    renderer1.RenderTexture(this._textureRightArrow, new Vector2(rectangle2.Right + 50.0, rectangle2.CentreY));
    Colour colour4 = this.IsSonicActive ? colour2 : colour1;
    this._miniSonicAniInstance.Draw(renderer1, colour4, (Vector2) new Vector2i(910, 880));
    Colour colour5 = this.IsTailsActive ? colour2 : colour1;
    this._miniTailsAniInstance.Draw(renderer1, colour5, (Vector2) new Vector2i(1010, 880));
  }

  private void DrawLevelSelect(Renderer renderer)
  {
    I2dRenderer obj = renderer.Get2dRenderer();
    IFontRenderer fontRenderer = renderer.GetFontRenderer();
    obj.BlendMode = BlendMode.Alpha;
    obj.RenderQuad(new Colour(0.8, 0.0, 0.0, 0.0), new Rectangle(0.0, 0.0, 1920.0, 1080.0));
    int x = 660;
    int y = 400;
    int num1 = 0;
    fontRenderer.RenderStringWithShadow("LEVEL SELECT", new Rectangle((double) x, 330.0, 0.0, 0.0), FontAlignment.MiddleY, this._fontImpactRegular, 0);
    foreach (UserInterface.LevelSelectItem levelSelectItem in this._levelSelectItems)
    {
      int num2 = this._levelSelectSelectionIndex == num1 ? 1 : 0;
      int num3 = num2 != 0 ? 1 : 0;
      Colour colour = num2 != 0 ? Colours.White : new Colour(0.5, 1.0, 1.0, 1.0);
      fontRenderer.RenderStringWithShadow(levelSelectItem.Text, new Rectangle((double) x, (double) y, 0.0, 0.0), FontAlignment.MiddleY, this._fontImpactRegular, colour, new int?(num3));
      y += 50;
      ++num1;
    }
  }

  private static int NegMod(int x, int divisor)
  {
    while (x < 0)
      x += divisor;
    return x % divisor;
  }

  private class MenuItem
  {
    public string Text { get; set; }

    public Action Action { get; set; }
  }

  private class MenuItemWidget
  {
    public int OriginOffset { get; set; }

    public int MenuItemIndex { get; set; }

    public float Opacity { get; set; }

    public float X { get; set; }

    public Vector2 Scale { get; set; }
  }

  private class LevelSelectItem
  {
    public string Text { get; set; }

    public string Mnemonic { get; set; }

    public int Act { get; set; }

    public int Number { get; set; }
  }
}
