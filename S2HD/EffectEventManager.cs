// Decompiled with JetBrains decompiler
// Type: S2HD.EffectEventManager
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca;
using System.Collections.Generic;

namespace S2HD
{

    internal class EffectEventManager
    {
      private List<Updater> _runningEvents = new List<Updater>();

      public void Clear() => this._runningEvents.Clear();

      public void BeginEvent(IEnumerable<UpdateResult> eventMethod)
      {
        Updater updater = new Updater(eventMethod);
        if (!updater.Update())
          return;
        this._runningEvents.Add(updater);
      }

      public void Update()
      {
        int index = 0;
        while (index < this._runningEvents.Count)
        {
          if (this._runningEvents[index].Update())
            ++index;
          else
            this._runningEvents.RemoveAt(index);
        }
      }
    }
}
