// Decompiled with JetBrains decompiler
// Type: Outfitted.OutfittedMod
// Assembly: Outfitted, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FB12A581-65CC-4BDE-BF48-F09425B0255F
// Assembly location: C:\Users\louiz\source\repos\Outfitted\1.4\Assemblies\Outfitted.dll

using HarmonyLib;
using RimWorld;
using System;
using System.Linq;
using UnityEngine;
using Verse;

#nullable disable
namespace Outfitted
{
  [StaticConstructorOnStartup]
  public static class OutfittedMod
  {
    internal static bool showApparelScores;
    internal static bool isSaveStorageSettingsEnabled;
    private static readonly SimpleCurve HitPointsPercentScoreFactorCurve;
    private static readonly SimpleCurve InsulationTemperatureScoreFactorCurve_Need;
    private static readonly SimpleCurve InsulationFactorCurve;

    static OutfittedMod()
    {
      SimpleCurve simpleCurve1 = new SimpleCurve();
      simpleCurve1.Add(new CurvePoint(0.0f, 0.0f), true);
      simpleCurve1.Add(new CurvePoint(0.2f, 0.2f), true);
      simpleCurve1.Add(new CurvePoint(0.22f, 0.6f), true);
      simpleCurve1.Add(new CurvePoint(0.5f, 0.6f), true);
      simpleCurve1.Add(new CurvePoint(0.52f, 1f), true);
      OutfittedMod.HitPointsPercentScoreFactorCurve = simpleCurve1;
      SimpleCurve simpleCurve2 = new SimpleCurve();
      simpleCurve2.Add(new CurvePoint(0.0f, 1f), true);
      simpleCurve2.Add(new CurvePoint(30f, 4f), true);
      OutfittedMod.InsulationTemperatureScoreFactorCurve_Need = simpleCurve2;
      SimpleCurve simpleCurve3 = new SimpleCurve();
      simpleCurve3.Add(new CurvePoint(-20f, -3f), true);
      simpleCurve3.Add(new CurvePoint(-10f, -2f), true);
      simpleCurve3.Add(new CurvePoint(10f, 2f), true);
      simpleCurve3.Add(new CurvePoint(20f, 3f), true);
      OutfittedMod.InsulationFactorCurve = simpleCurve3;
      OutfittedMod.isSaveStorageSettingsEnabled = ModLister.GetActiveModWithIdentifier("savestoragesettings.kv.rw", false) != null;
      new Harmony("rimworld.outfitted").PatchAll();
    }

    public static float ApparelScoreExtra(Pawn pawn, Apparel apparel, NeededWarmth neededWarmth)
    {
      if (!(pawn.outfits.CurrentOutfit is ExtendedOutfit currentOutfit))
      {
        Log.ErrorOnce("Outfitted :: Not an ExtendedOutfit, something went wrong.", 399441);
        return 0.0f;
      }
      float num1 = 0.1f + ((Thing) apparel).def.apparel.scoreOffset + OutfittedMod.ApparelScoreRawPriorities(apparel, currentOutfit);
      if (currentOutfit.AutoWorkPriorities)
        num1 += OutfittedMod.ApparelScoreAutoWorkPriorities(pawn, apparel);
      if (((Thing) apparel).def.useHitPoints)
        num1 *= OutfittedMod.HitPointsPercentScoreFactorCurve.Evaluate((float) ((Thing) apparel).HitPoints / (float) ((Thing) apparel).MaxHitPoints);
      float num2 = num1 + apparel.GetSpecialApparelScoreOffset();
      if (pawn != null && currentOutfit != null)
        num2 += OutfittedMod.ApparelScoreRawInsulation(pawn, apparel, currentOutfit, neededWarmth);
      if (currentOutfit.PenaltyWornByCorpse && apparel.WornByCorpse && ThoughtUtility.CanGetThought(pawn, ThoughtDefOf.DeadMansApparel, true))
      {
        num2 -= 0.5f;
        if ((double) num2 > 0.0)
          num2 *= 0.1f;
      }
      return num2;
    }

    private static float ApparelScoreRawPriorities(Apparel apparel, ExtendedOutfit outfit)
    {
      return !outfit.StatPriorities.Any<StatPriority>() ? 0.0f : outfit.StatPriorities.Select(sp => new
      {
        weight = sp.Weight,
        value = StatUtility.GetStatOffsetFromList(((Thing) apparel).def.equippedStatOffsets, sp.Stat) + StatExtension.GetStatValue((Thing) apparel, sp.Stat, true, -1),
        def = sp.Stat.defaultBaseValue
      }).Average(sp => ((double) Math.Abs(sp.def) < 1.0 / 1000.0 ? sp.value : (sp.value - sp.def) / sp.def) * Mathf.Pow(sp.weight, 3f));
    }

    private static float ApparelScoreAutoWorkPriorities(Pawn pawn, Apparel apparel)
    {
      return WorkPriorities.WorktypeStatPriorities(pawn).Select<StatPriority, float>((Func<StatPriority, float>) (sp => (StatUtility.GetStatOffsetFromList(((Thing) apparel).def.equippedStatOffsets, sp.Stat) + StatExtension.GetStatValue((Thing) apparel, sp.Stat, true, -1) - sp.Stat.defaultBaseValue) * sp.Weight)).Sum();
    }

    private static float ApparelScoreRawInsulation(
      Pawn pawn,
      Apparel apparel,
      ExtendedOutfit outfit,
      NeededWarmth neededWarmth)
    {
      float num1;
      if (outfit.targetTemperaturesOverride)
      {
        int num2 = pawn.apparel.WornApparel.Contains(apparel) ? 1 : 0;
        FloatRange floatRange1 = GenTemperature.ComfortableTemperatureRange(pawn);
        FloatRange floatRange2 = floatRange1;
        if (outfit.AutoTemp)
        {
          float seasonalTemp = ((Thing) pawn).Map.mapTemperature.SeasonalTemp;
          outfit.targetTemperatures = new FloatRange(seasonalTemp - (float) outfit.autoTempOffset, seasonalTemp + (float) outfit.autoTempOffset);
        }
        FloatRange targetTemperatures = outfit.targetTemperatures;
        FloatRange insulationStats1 = OutfittedMod.GetInsulationStats(apparel);
        floatRange2.min += insulationStats1.min;
        floatRange2.max += insulationStats1.max;
        if (num2 == 0)
        {
          foreach (Apparel apparel1 in pawn.apparel.WornApparel)
          {
            if (!ApparelUtility.CanWearTogether(((Thing) apparel).def, ((Thing) apparel1).def, pawn.RaceProps.body))
            {
              FloatRange insulationStats2 = OutfittedMod.GetInsulationStats(apparel1);
              floatRange2.min -= insulationStats2.min;
              floatRange2.max -= insulationStats2.max;
            }
          }
        }
        FloatRange floatRange3;
        // ISSUE: explicit constructor call
        ((FloatRange) ref floatRange3).\u002Ector(Mathf.Max(floatRange1.min - targetTemperatures.min, 0.0f), Mathf.Max(targetTemperatures.max - floatRange1.max, 0.0f));
        FloatRange floatRange4;
        // ISSUE: explicit constructor call
        ((FloatRange) ref floatRange4).\u002Ector(Mathf.Max(floatRange2.min - targetTemperatures.min, 0.0f), Mathf.Max(targetTemperatures.max - floatRange2.max, 0.0f));
        num1 = OutfittedMod.InsulationFactorCurve.Evaluate(floatRange3.min - floatRange4.min) + OutfittedMod.InsulationFactorCurve.Evaluate(floatRange3.max - floatRange4.max);
      }
      else if (neededWarmth == 1)
      {
        float statValue = StatExtension.GetStatValue((Thing) apparel, StatDefOf.Insulation_Cold, true, -1);
        num1 = OutfittedMod.InsulationTemperatureScoreFactorCurve_Need.Evaluate(statValue);
      }
      else if (neededWarmth == 2)
      {
        float statValue = StatExtension.GetStatValue((Thing) apparel, StatDefOf.Insulation_Heat, true, -1);
        num1 = OutfittedMod.InsulationTemperatureScoreFactorCurve_Need.Evaluate(statValue);
      }
      else
        num1 = 1f;
      return num1;
    }

    private static FloatRange GetInsulationStats(Apparel apparel)
    {
      return new FloatRange(-StatExtension.GetStatValue((Thing) apparel, StatDefOf.Insulation_Cold, true, -1), StatExtension.GetStatValue((Thing) apparel, StatDefOf.Insulation_Heat, true, -1));
    }

    internal static void Notify_OutfitChanged(int id)
    {
      foreach (Pawn pawn in PawnsFinder.AllMaps_SpawnedPawnsInFaction(Faction.OfPlayer).Where<Pawn>((Func<Pawn, bool>) (i => i.outfits.CurrentOutfit.uniqueId == id)))
        pawn.mindState?.Notify_OutfitChanged();
    }
  }
}
