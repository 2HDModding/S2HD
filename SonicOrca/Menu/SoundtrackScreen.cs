// Decompiled with JetBrains decompiler
// Type: SonicOrca.Menu.SoundtrackScreen
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Audio;
using SonicOrca.Extensions;
using SonicOrca.Geometry;
using SonicOrca.Graphics;
using SonicOrca.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace SonicOrca.Menu;

internal class SoundtrackScreen : Screen
{
  private const string BackgroundResourceKey = "SONICORCA/MENU/LEVELSELECT/BACKGROUND";
  private const string FontResourceKey = "SONICORCA/S2/HUD/FONT";
  private const string TrackBarResourceKey = "SONICORCA/S2/HUD/LIFE/SONIC";
  private const string SelectionChangeResourceKey = "SONICORCA/SOUND/TALLY/SWITCH";
  private const string ListingResourceKey = "SONICORCA/MENU/SOUNDTRACK/LISTING";
  private const int CursorMinDelay = 10;
  private const int ListSize = 10;
  private const int ListHeight = 74;
  private const int ListGap = 8;
  private readonly SonicOrcaGameContext _gameContext;
  private ResourceSession _resourceSession;
  private ITexture _backgroundTexture;
  private ITexture _trackBarTexture;
  private Font _font;
  private Sample _selectionChangeSample;
  private readonly FadeTransition _fadeTransition = new FadeTransition(30);
  private SoundtrackScreen.InternalState _state;
  private SoundtrackScreen.TrackNode _listingHead;
  private SoundtrackScreen.TrackNode _currentTrackNode;
  private CancellationTokenSource _loadTrackCancellationTokenSource;
  private Task _loadTrackTask;
  private ResourceSession _loadedTrackResourceSession;
  private string _loadedTrackResourceKey;
  private SampleInstance _loadedTrackSampleInstance;
  private readonly object _loadTrackSync = new object();
  private int _cursorDelay;

  public SoundtrackScreen(SonicOrcaGameContext gameContext) => this._gameContext = gameContext;

  public override async Task LoadAsync(ScreenLoadingProgress progress, CancellationToken ct = default (CancellationToken))
  {
    ResourceTree resourceTree = this._gameContext.ResourceTree;
    this._loadedTrackResourceSession = new ResourceSession(resourceTree);
    this._resourceSession = new ResourceSession(resourceTree);
    this._resourceSession.PushDependencies("SONICORCA/MENU/LEVELSELECT/BACKGROUND", "SONICORCA/S2/HUD/FONT", "SONICORCA/S2/HUD/LIFE/SONIC", "SONICORCA/SOUND/TALLY/SWITCH", "SONICORCA/MENU/SOUNDTRACK/LISTING");
    await this._resourceSession.LoadAsync(ct);
    this._backgroundTexture = resourceTree.GetLoadedResource<ITexture>("SONICORCA/MENU/LEVELSELECT/BACKGROUND");
    this._trackBarTexture = resourceTree.GetLoadedResource<ITexture>("SONICORCA/S2/HUD/LIFE/SONIC");
    this._font = resourceTree.GetLoadedResource<Font>("SONICORCA/S2/HUD/FONT");
    this._selectionChangeSample = resourceTree.GetLoadedResource<Sample>("SONICORCA/SOUND/TALLY/SWITCH");
    this.ParseListingXml(resourceTree.GetLoadedResource<XmlLoadedResource>("SONICORCA/MENU/SOUNDTRACK/LISTING"));
  }

  public override void Unload()
  {
    if (this._resourceSession == null)
      return;
    this._resourceSession.Unload();
  }

  private void ParseListingXml(XmlLoadedResource xmlResource)
  {
    this._currentTrackNode = this._listingHead = new SoundtrackScreen.TrackNode((string) null, this.ParseListingXml(xmlResource.XmlDocument.SelectSingleNode("soundtrack")));
  }

  private IEnumerable<SoundtrackScreen.TrackNode> ParseListingXml(XmlNode xmlNode)
  {
    foreach (XmlNode child in xmlNode.ChildNodes)
    {
      if (child.Name == "track")
        yield return new SoundtrackScreen.TrackNode(child.Attributes["name"].Value, child.InnerText);
      else if (child.Name == "group")
      {
        string str;
        child.TryGetAttributeValue("type", out str);
        yield return new SoundtrackScreen.TrackNode(child.Attributes["name"].Value, this.ParseListingXml(child), str == "mini");
      }
    }
  }

  public override void Update()
  {
    switch (this._state)
    {
      case SoundtrackScreen.InternalState.BeginFadeIn:
        this._fadeTransition.BeginFadeIn();
        this._state = SoundtrackScreen.InternalState.FadingIn;
        break;
      case SoundtrackScreen.InternalState.FadingIn:
        this._fadeTransition.Update();
        if (!this._fadeTransition.Finished)
          break;
        this._state = SoundtrackScreen.InternalState.Normal;
        break;
      case SoundtrackScreen.InternalState.Normal:
        this.HandleInput();
        break;
      case SoundtrackScreen.InternalState.BeginFadeOut:
        this._fadeTransition.BeginFadeOut();
        this._state = SoundtrackScreen.InternalState.FadingOut;
        break;
      case SoundtrackScreen.InternalState.FadingOut:
        this._fadeTransition.Update();
        if (!this._fadeTransition.Finished)
          break;
        this.Finish();
        break;
    }
  }

  private void HandleInput()
  {
    Controller controller = this._gameContext.Pressed[0];
    if (controller.DirectionLeft.X == 0.0 && controller.DirectionLeft.Y == 0.0)
      this._cursorDelay = 0;
    if (this._cursorDelay > 0)
    {
      --this._cursorDelay;
    }
    else
    {
      if (controller.DirectionLeft.X < 0.0)
        this.ListingLeft();
      if (controller.DirectionLeft.X > 0.0)
        this.ListingRight();
      if (controller.DirectionLeft.Y < 0.0)
        this.ListingUp();
      if (controller.DirectionLeft.Y > 0.0)
        this.ListingDown();
      if (controller.DirectionLeft.X != 0.0 || controller.DirectionLeft.Y != 0.0)
        this._cursorDelay = (int) (10.0 * (2.0 * controller.DirectionLeft.Y));
    }
    if (controller.Action2)
      this.ListingEnter();
    if (controller.Action3)
      this.ListingExit();
    lock (this._loadTrackSync)
    {
      if (this._loadedTrackSampleInstance == null)
        return;
      if (controller.Action1)
        this.TogglePlayTrack();
      double factor = 0.0;
      if (controller.LeftTrigger != 0.0 && controller.RightTrigger == 0.0)
        factor = -controller.LeftTrigger;
      if (controller.LeftTrigger == 0.0 && controller.RightTrigger != 0.0)
        factor = controller.RightTrigger;
      if (this._gameContext.Input.CurrentState.Keyboard[18])
        factor = -1.0;
      if (this._gameContext.Input.CurrentState.Keyboard[19])
        factor = 1.0;
      this.SeekTrack(factor);
      this.SeekTrack(controller.DirectionRight.X, controller.DirectionRight.Y);
    }
  }

  private void ListingUp()
  {
    if (this._currentTrackNode.SelectedIndex <= 0)
      return;
    --this._currentTrackNode.SelectedIndex;
    this.PlayChangeListingSound();
  }

  private void ListingDown()
  {
    if (this._currentTrackNode.SelectedIndex >= ((IReadOnlyCollection<SoundtrackScreen.TrackNode>) this._currentTrackNode.Children).Count - 1)
      return;
    ++this._currentTrackNode.SelectedIndex;
    this.PlayChangeListingSound();
  }

  private void ListingLeft()
  {
    if (((IReadOnlyCollection<SoundtrackScreen.TrackNode>) this._currentTrackNode.Children).Count == 0)
      return;
    SoundtrackScreen.TrackNode child = this._currentTrackNode.Children[this._currentTrackNode.SelectedIndex];
    if (child.Type != SoundtrackScreen.ListingType.MiniGroup || child.SelectedIndex <= 0)
      return;
    --child.SelectedIndex;
    this.PlayChangeListingSound();
  }

  private void ListingRight()
  {
    if (((IReadOnlyCollection<SoundtrackScreen.TrackNode>) this._currentTrackNode.Children).Count == 0)
      return;
    SoundtrackScreen.TrackNode child = this._currentTrackNode.Children[this._currentTrackNode.SelectedIndex];
    if (child.Type != SoundtrackScreen.ListingType.MiniGroup || child.SelectedIndex >= ((IReadOnlyCollection<SoundtrackScreen.TrackNode>) child.Children).Count - 1)
      return;
    ++child.SelectedIndex;
    this.PlayChangeListingSound();
  }

  private void ListingExit()
  {
    if (this._currentTrackNode.Parent == null)
      return;
    this._currentTrackNode = this._currentTrackNode.Parent;
    this.PlayChangeListingSound();
  }

  private void ListingEnter()
  {
    if (((IReadOnlyCollection<SoundtrackScreen.TrackNode>) this._currentTrackNode.Children).Count == 0)
      return;
    SoundtrackScreen.TrackNode child1 = this._currentTrackNode.Children[this._currentTrackNode.SelectedIndex];
    if (child1.Type == SoundtrackScreen.ListingType.Track)
      this.SetAndPlayTrack(child1);
    else if (child1.Type == SoundtrackScreen.ListingType.MiniGroup)
    {
      if (((IReadOnlyCollection<SoundtrackScreen.TrackNode>) child1.Children).Count == 0)
        return;
      SoundtrackScreen.TrackNode child2 = child1.Children[child1.SelectedIndex];
      if (child2.Type != SoundtrackScreen.ListingType.Track)
        return;
      this.SetAndPlayTrack(child2);
    }
    else
    {
      this._currentTrackNode = child1;
      this.PlayChangeListingSound();
    }
  }

  private void PlayChangeListingSound()
  {
    this._gameContext.Audio.PlaySound(this._selectionChangeSample);
  }

  private void SetAndPlayTrack(SoundtrackScreen.TrackNode node)
  {
    if (node.ResourceKey == this._loadedTrackResourceKey)
    {
      if (this._loadTrackTask != null)
        return;
      this.RestartTrack();
    }
    else
    {
      if (this._loadTrackTask != null)
      {
        this._loadTrackCancellationTokenSource.Cancel();
        while (this._loadTrackTask != null)
          ;
      }
      this._loadTrackCancellationTokenSource = new CancellationTokenSource();
      CancellationToken ct = this._loadTrackCancellationTokenSource.Token;
      this._loadTrackTask = Task.Run((Func<Task>) (async () =>
      {
        try
        {
          await this.SetTrack(node.ResourceKey, ct);
          if (ct.IsCancellationRequested)
            return;
          this.RestartTrack();
        }
        finally
        {
          this._loadTrackTask = (Task) null;
        }
      }));
    }
  }

  private async Task SetTrack(string resourceKey, CancellationToken ct = default (CancellationToken))
  {
    if (this._loadedTrackResourceKey == resourceKey)
      return;
    lock (this._loadTrackSync)
    {
      if (this._loadedTrackSampleInstance != null)
      {
        this._loadedTrackSampleInstance.Dispose();
        this._loadedTrackSampleInstance = (SampleInstance) null;
      }
    }
    this._loadedTrackResourceKey = resourceKey;
    this._loadedTrackResourceSession.Unload();
    try
    {
      this._loadedTrackResourceSession.PushDependency(resourceKey);
      await this._loadedTrackResourceSession.LoadAsync(ct);
      lock (this._loadTrackSync)
        this._loadedTrackSampleInstance = this._loadedTrackResourceSession.ResourceTree[resourceKey].Resource.Identifier != ResourceTypeIdentifier.SampleInfo ? new SampleInstance(this._gameContext.Audio, this._loadedTrackResourceSession.ResourceTree.GetLoadedResource<Sample>(resourceKey)) : new SampleInstance(this._gameContext.Audio, this._loadedTrackResourceSession.ResourceTree.GetLoadedResource<SampleInfo>(resourceKey));
    }
    catch (Exception ex)
    {
      this._loadedTrackResourceKey = (string) null;
    }
  }

  private void RestartTrack()
  {
    lock (this._loadTrackSync)
    {
      if (this._loadedTrackSampleInstance == null)
        return;
      this._loadedTrackSampleInstance.Stop();
      this._loadedTrackSampleInstance.SeekToStart();
      this._loadedTrackSampleInstance.Play();
    }
  }

  private void TogglePlayTrack()
  {
    lock (this._loadTrackSync)
    {
      if (this._loadedTrackSampleInstance == null)
        return;
      if (this._loadedTrackSampleInstance.Playing)
        this._loadedTrackSampleInstance.Stop();
      else
        this._loadedTrackSampleInstance.Play();
    }
  }

  private void SeekTrack(double factor)
  {
    lock (this._loadTrackSync)
    {
      if (this._loadedTrackSampleInstance == null || factor == 0.0)
        return;
      int sampleIndex = this._loadedTrackSampleInstance.SampleIndex;
      int num1 = this._loadedTrackSampleInstance.Sample.SampleRate / 2;
      int sampleCount = this._loadedTrackSampleInstance.Sample.SampleCount;
      factor = (double) Math.Sign(factor) * factor * factor;
      int num2 = (int) ((double) num1 * factor);
      this._loadedTrackSampleInstance.SeekTo(MathX.Clamp(0, sampleIndex + num2, sampleCount));
    }
  }

  private void SeekTrack(double x, double y)
  {
    lock (this._loadTrackSync)
    {
      if (x == 0.0 && y == 0.0)
        return;
      this._loadedTrackSampleInstance.SeekTo(MathX.Clamp(0, (int) ((Math.Atan2(y, x) / (2.0 * Math.PI) + 1.25) % 1.0 * (double) this._loadedTrackSampleInstance.Sample.SampleCount), this._loadedTrackSampleInstance.Sample.SampleCount));
    }
  }

  private TimeSpan GetTime(int sampleOffset)
  {
    return TimeSpan.FromSeconds((double) sampleOffset / (double) this._loadedTrackSampleInstance.Sample.SampleRate);
  }

  public override void Draw(Renderer renderer)
  {
    I2dRenderer obj = renderer.Get2dRenderer();
    IFontRenderer fontRenderer = renderer.GetFontRenderer();
    obj.Colour = Colours.White;
    obj.RenderTexture(this._backgroundTexture, new Rectangle(0.0, 0.0, 1920.0, 1080.0));
    using (obj.BeginMatixState())
    {
      obj.ModelMatrix = obj.ModelMatrix.Scale(2.5, 2.5);
      fontRenderer.RenderString("SOUNDTRACK", new Rectangle(32.0, 32.0, 0.0, 0.0), FontAlignment.Left, this._font, Colours.Black);
    }
    this.DrawList(renderer, new Rectangle(32.0, 228.0, 896.0, 820.0));
    lock (this._loadTrackSync)
      this.DrawTrackbar(renderer, new Rectangle(1080.0, 244.0, 804.0, 804.0));
    this._fadeTransition.Draw(renderer);
  }

  private void DrawList(Renderer renderer, Rectangle bounds)
  {
    I2dRenderer obj = renderer.Get2dRenderer();
    using (obj.BeginMatixState())
    {
      obj.ModelMatrix = obj.ModelMatrix.Translate(bounds.X, bounds.Y);
      int num1 = 0;
      int num2 = 0;
      int num3 = MathX.Clamp(0, this._currentTrackNode.SelectedIndex - 5, ((IReadOnlyCollection<SoundtrackScreen.TrackNode>) this._currentTrackNode.Children).Count - 1 - 9);
      double y = 0.0;
      foreach (SoundtrackScreen.TrackNode child in (IEnumerable<SoundtrackScreen.TrackNode>) this._currentTrackNode.Children)
      {
        bool selected = num1 == this._currentTrackNode.SelectedIndex;
        if (num1++ >= num3)
        {
          y += this.DrawListItem(renderer, child, new Rectangle(0.0, y, bounds.Width, 74.0), selected) + 8.0;
          if (child.Type == SoundtrackScreen.ListingType.MiniGroup & selected)
            ++num2;
          if (++num2 >= 10)
            break;
        }
      }
    }
  }

  private double DrawListItem(
    Renderer renderer,
    SoundtrackScreen.TrackNode node,
    Rectangle bounds,
    bool selected)
  {
    bool flag = false;
    if (node.Type == SoundtrackScreen.ListingType.MiniGroup & selected)
      flag = true;
    if (flag)
      bounds.Height = bounds.Height * 2.0 + 8.0;
    I2dRenderer obj = renderer.Get2dRenderer();
    IFontRenderer fontRenderer = renderer.GetFontRenderer();
    Colour colour1 = new Colour((byte) 128 /*0x80*/, (byte) 0, (byte) 0, (byte) 0);
    Rectangle destination = bounds;
    obj.RenderQuad(colour1, destination);
    if (flag)
      bounds.Height = (bounds.Height - 8.0) / 2.0;
    bounds.X += 12.0;
    Colour colour2 = new Colour((byte) 128 /*0x80*/, (byte) 128 /*0x80*/, (byte) 128 /*0x80*/);
    Colour colour3 = Colours.White;
    if (this._loadedTrackResourceKey == node.ResourceKey && this._loadTrackTask != null)
      colour3 = colour2;
    fontRenderer.RenderStringWithShadow(node.Name.ToUpper(), bounds, FontAlignment.MiddleY, this._font, colour3, new int?(selected ? 1 : 0));
    if (flag)
    {
      bounds.Y += bounds.Height + 8.0;
      for (int index = 0; index < ((IReadOnlyCollection<SoundtrackScreen.TrackNode>) node.Children).Count; ++index)
      {
        SoundtrackScreen.TrackNode child = node.Children[index];
        Colour colour4 = Colours.White;
        if (this._loadedTrackResourceKey == child.ResourceKey && this._loadTrackTask != null)
          colour4 = colour2;
        fontRenderer.RenderStringWithShadow(child.Name.ToUpper(), bounds, FontAlignment.MiddleY, this._font, colour4, new int?(node.SelectedIndex == index ? 1 : 0));
        bounds.X += this._font.MeasureString(child.Name.ToUpper(), bounds, FontAlignment.MiddleY).Width + bounds.Height;
      }
      bounds.Height = bounds.Height * 2.0 + 8.0;
    }
    return bounds.Height;
  }

  private void DrawTrackbar(Renderer renderer, Rectangle bounds)
  {
    I2dRenderer obj = renderer.Get2dRenderer();
    IFontRenderer fontRenderer = renderer.GetFontRenderer();
    using (obj.BeginMatixState())
    {
      obj.ModelMatrix = obj.ModelMatrix.Translate(bounds.CentreX, bounds.CentreY);
      double outerRadius = bounds.Width / 2.0;
      double innerRadius = outerRadius - 32.0;
      double num1 = innerRadius + (outerRadius - innerRadius) / 2.0;
      obj.RenderEllipse(new Colour((byte) 128 /*0x80*/, (byte) 0, (byte) 0, (byte) 0), new Vector2(), innerRadius, outerRadius, 64 /*0x40*/);
      if (this._loadedTrackSampleInstance == null)
        return;
      double num2 = (double) this._loadedTrackSampleInstance.SampleIndex / (double) this._loadedTrackSampleInstance.Sample.SampleCount * (2.0 * Math.PI);
      obj.Colour = Colours.White;
      obj.RenderTexture(this._trackBarTexture, new Vector2(Math.Sin(num2) * num1, -Math.Cos(num2) * num1));
      fontRenderer.RenderStringWithShadow(this._loadedTrackSampleInstance.SampleIndex.ToString(), new Rectangle(), FontAlignment.Centre, this._font, 0);
      TimeSpan time1 = this.GetTime(this._loadedTrackSampleInstance.SampleIndex);
      TimeSpan time2 = this.GetTime(this._loadedTrackSampleInstance.Sample.SampleCount);
      fontRenderer.RenderStringWithShadow(string.Format("{0:m\\:ss} / {1:m\\:ss}", (object) time1, (object) time2), new Rectangle(0.0, innerRadius * 0.75, 0.0, 0.0), FontAlignment.Centre, this._font, 0);
    }
  }

  private enum ListingType
  {
    Group,
    MiniGroup,
    Track,
  }

  private class TrackNode
  {
    private readonly SoundtrackScreen.TrackNode[] _children;
    private readonly SoundtrackScreen.ListingType _type;
    private readonly string _name;
    private readonly string _resourceKey;
    private SoundtrackScreen.TrackNode _parent;

    public int SelectedIndex { get; set; }

    public SoundtrackScreen.TrackNode Parent => this._parent;

    public SoundtrackScreen.ListingType Type => this._type;

    public IReadOnlyList<SoundtrackScreen.TrackNode> Children
    {
      get => (IReadOnlyList<SoundtrackScreen.TrackNode>) this._children;
    }

    public string Name => this._name;

    public string ResourceKey => this._resourceKey;

    public TrackNode(string name, IEnumerable<SoundtrackScreen.TrackNode> children, bool mini = false)
    {
      this._children = children.ToArray<SoundtrackScreen.TrackNode>();
      this._type = mini ? SoundtrackScreen.ListingType.MiniGroup : SoundtrackScreen.ListingType.Group;
      this._name = name;
      foreach (SoundtrackScreen.TrackNode child in this._children)
        child._parent = this;
    }

    public TrackNode(string name, string resourceKey)
    {
      this._type = SoundtrackScreen.ListingType.Track;
      this._name = name;
      this._resourceKey = resourceKey;
    }

    public override string ToString() => this.Name;
  }

  private enum InternalState
  {
    BeginFadeIn,
    FadingIn,
    Normal,
    BeginFadeOut,
    FadingOut,
  }
}
