// Decompiled with JetBrains decompiler
// Type: S2HD.SharedRenderers
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Graphics;
using System;

#nullable disable
namespace S2HD;

internal static class SharedRenderers
{
  private static readonly object InitialiseLock = new object();

  public static bool Initialised { get; private set; }

  public static I2dRenderer Standard2d { get; private set; }

  public static IFadeTransitionRenderer FadeTransition { get; private set; }

  public static void Initialise(IRendererFactory rendererFactory)
  {
    lock (SharedRenderers.InitialiseLock)
    {
      if (SharedRenderers.Initialised)
        throw new InvalidOperationException("Shared renderers already initialised.");
      SharedRenderers.Standard2d = rendererFactory.Create2dRenderer();
      SharedRenderers.FadeTransition = rendererFactory.CreateFadeTransitionRenderer();
      SharedRenderers.Initialised = true;
    }
  }

  public static void Dispose()
  {
    lock (SharedRenderers.InitialiseLock)
    {
      if (SharedRenderers.Standard2d != null)
      {
        SharedRenderers.Standard2d.Dispose();
        SharedRenderers.Standard2d = (I2dRenderer) null;
      }
      if (SharedRenderers.FadeTransition != null)
      {
        SharedRenderers.FadeTransition.Dispose();
        SharedRenderers.FadeTransition = (IFadeTransitionRenderer) null;
      }
      SharedRenderers.Initialised = false;
    }
  }
}
