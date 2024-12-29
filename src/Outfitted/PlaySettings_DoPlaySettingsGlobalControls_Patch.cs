// Decompiled with JetBrains decompiler
// Type: Outfitted.PlaySettings_DoPlaySettingsGlobalControls_Patch
// Assembly: Outfitted, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FB12A581-65CC-4BDE-BF48-F09425B0255F
// Assembly location: C:\Users\louiz\source\repos\Outfitted\1.4\Assemblies\Outfitted.dll

using HarmonyLib;
using RimWorld;
using Verse;

#nullable disable
namespace Outfitted
{
  [HarmonyPatch(typeof (PlaySettings), "DoPlaySettingsGlobalControls")]
  internal static class PlaySettings_DoPlaySettingsGlobalControls_Patch
  {
    private static void Postfix(WidgetRow row, bool worldView)
    {
      if (worldView)
        return;
      row.ToggleableIcon(ref OutfittedMod.showApparelScores, ResourceBank.Textures.ShirtBasic, ResourceBank.Strings.OutfitShow, SoundDefOf.Mouseover_ButtonToggle, (string) null);
    }
  }
}
