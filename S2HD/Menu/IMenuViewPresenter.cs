﻿// Decompiled with JetBrains decompiler
// Type: S2HD.Menu.IMenuViewPresenter
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Geometry;
using SonicOrca.Graphics;
using System;

namespace S2HD.Menu;

internal interface IMenuViewPresenter
{
  Rectanglei Bounds { get; set; }

  event EventHandler NavigateBack;

  event EventHandler<NavigateNextEventArgs> NavigateNext;

  void Update();

  void HandleInput();

  void Draw(Renderer renderer);
}
