// Decompiled with JetBrains decompiler
// Type: S2HD.Menu.NavigateNextEventArgs
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using System;

#nullable disable
namespace S2HD.Menu;

internal class NavigateNextEventArgs : EventArgs
{
  public IMenuViewModel ViewModel { get; }

  public object Tag { get; }

  public NavigateNextEventArgs(IMenuViewModel viewModel, object tag)
  {
    this.ViewModel = viewModel;
    this.Tag = tag;
  }
}
