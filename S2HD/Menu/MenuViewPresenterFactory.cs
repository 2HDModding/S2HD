// Decompiled with JetBrains decompiler
// Type: S2HD.Menu.MenuViewPresenterFactory
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca;
using System;

#nullable disable
namespace S2HD.Menu;

internal class MenuViewPresenterFactory
{
  private readonly S2HDSonicOrcaGameContext _gameContext;
  private readonly ISettingUIResources _resources;

  public MenuViewPresenterFactory(
    S2HDSonicOrcaGameContext gameContext,
    ISettingUIResources resources)
  {
    this._gameContext = gameContext;
    this._resources = resources;
  }

  public IMenuViewPresenter Create(IMenuViewModel viewModel)
  {
    switch (viewModel)
    {
      case IListMenuViewModel _:
        return (IMenuViewPresenter) new ListMenuViewPresenter(this._gameContext, this._resources, viewModel as IListMenuViewModel);
      case ISettingListMenuViewModel _:
        return (IMenuViewPresenter) new SettingListMenuViewPresenter(this._gameContext, this._resources, viewModel as ISettingListMenuViewModel);
      default:
        throw new NotSupportedException();
    }
  }
}
