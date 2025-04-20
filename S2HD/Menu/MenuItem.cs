// Decompiled with JetBrains decompiler
// Type: S2HD.Menu.MenuItem
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

#nullable disable
namespace S2HD.Menu;

internal class MenuItem : IMenuItem
{
  public string Text { get; }

  public IMenuViewModel Next { get; }

  public object Tag { get; }

  public MenuItem(string text, IMenuViewModel next = null, object tag = null)
  {
    this.Text = text;
    this.Next = next;
    this.Tag = tag;
  }

  public override string ToString() => this.Text;
}
