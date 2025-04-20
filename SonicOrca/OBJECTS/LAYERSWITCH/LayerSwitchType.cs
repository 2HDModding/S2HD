// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.LAYERSWITCH.LayerSwitchType
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Core.Objects.Metadata;
using SonicOrca.Geometry;

namespace SONICORCA.OBJECTS.LAYERSWITCH
{

    [SonicOrca.Core.Objects.Metadata.Name("Layer switch")]
    [Description("Allow characters to switch layers.")]
    [SonicOrca.Core.Objects.Metadata.Classification(ObjectClassification.General)]
    [ObjectInstance(typeof (LayerSwitchInstance))]
    public class LayerSwitchType : ObjectType
    {
      public Vector2 GetLifeRadius(LayerSwitchInstance state)
      {
        return new Vector2((double) (state.Width / 2), (double) (state.Height / 2));
      }
    }
}
