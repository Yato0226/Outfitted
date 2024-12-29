// Decompiled with JetBrains decompiler
// Type: Outfitted.Thing_DrawGUIOverlay_Patch
// Assembly: Outfitted, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FB12A581-65CC-4BDE-BF48-F09425B0255F
// Assembly location: C:\Users\louiz\source\repos\Outfitted\1.4\Assemblies\Outfitted.dll

using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

#nullable disable
namespace Outfitted
{
  [HarmonyPatch(typeof (Thing), "DrawGUIOverlay")]
  internal static class Thing_DrawGUIOverlay_Patch
  {
    private static int cachedId = -1;
    private static int cachedTick = -1;
    private static List<float> cachedScores = new List<float>();

    private static void Postfix(Thing __instance)
    {
      if (!OutfittedMod.showApparelScores || Find.CameraDriver.CurrentZoom != null || !(Find.Selector.SingleSelectedThing is Pawn singleSelectedThing) || !singleSelectedThing.IsColonistPlayerControlled || !(__instance is Apparel apparel) || !(singleSelectedThing.outfits.CurrentOutfit is ExtendedOutfit currentOutfit) || !currentOutfit.filter.Allows((Thing) apparel))
        return;
      List<float> floatList = Thing_DrawGUIOverlay_Patch.CachedScoresForPawn(singleSelectedThing);
      float num = JobGiver_OptimizeApparel.ApparelScoreGain(singleSelectedThing, apparel, floatList);
      if ((double) Math.Abs(num) <= 0.0099999997764825821)
        return;
      GenMapUI.DrawThingLabel(GenMapUI.LabelDrawPosFor((Thing) apparel, 0.0f), num.ToString("F1"), BeautyDrawer.BeautyColor(num, 3f));
    }

    private static List<float> CachedScoresForPawn(Pawn pawn)
    {
      if (Thing_DrawGUIOverlay_Patch.cachedId != ((Thing) pawn).thingIDNumber || Thing_DrawGUIOverlay_Patch.cachedTick < GenTicks.TicksGame)
      {
        Thing_DrawGUIOverlay_Patch.cachedScores = Thing_DrawGUIOverlay_Patch.ScoresForPawn(pawn);
        Thing_DrawGUIOverlay_Patch.cachedId = ((Thing) pawn).thingIDNumber;
        Thing_DrawGUIOverlay_Patch.cachedTick = GenTicks.TicksGame;
      }
      return Thing_DrawGUIOverlay_Patch.cachedScores;
    }

    private static List<float> ScoresForPawn(Pawn pawn)
    {
      List<float> floatList = new List<float>();
      for (int index = 0; index < pawn.apparel.WornApparel.Count; ++index)
        floatList.Add(JobGiver_OptimizeApparel.ApparelScoreRaw(pawn, pawn.apparel.WornApparel[index]));
      return floatList;
    }
  }
}
