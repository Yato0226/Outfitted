// Decompiled with JetBrains decompiler
// Type: Outfitted.Database.OutfitDatabase_ExposeData_Patch
// Assembly: Outfitted, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FB12A581-65CC-4BDE-BF48-F09425B0255F
// Assembly location: C:\Users\louiz\source\repos\Outfitted\1.4\Assemblies\Outfitted.dll

using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

#nullable disable
namespace Outfitted.Database
{
  [HarmonyPatch(typeof (OutfitDatabase), "ExposeData")]
  internal static class OutfitDatabase_ExposeData_Patch
  {
    private static void Postfix(OutfitDatabase __instance, List<Outfit> ___outfits)
    {
      if (Scribe.mode != 2 || GenCollection.Any<Outfit>(___outfits, (Predicate<Outfit>) (i => i is ExtendedOutfit)))
        return;
      foreach (Outfit outfit in ___outfits.ToList<Outfit>())
      {
        ___outfits.Remove(outfit);
        ___outfits.Add(OutfitDatabase_ExposeData_Patch.ReplaceKnownVanillaOutfits(outfit));
      }
      OutfitDatabase_GenerateStartingOutfits_Patch.GenerateStartingOutfits(__instance, false);
    }

    private static Outfit ReplaceKnownVanillaOutfits(Outfit outfit)
    {
      ExtendedOutfit extendedOutfit = new ExtendedOutfit(outfit);
      switch (extendedOutfit.label)
      {
        case "Worker":
          extendedOutfit.AddRange((IEnumerable<StatPriority>) new List<StatPriority>()
          {
            new StatPriority(StatDefOf.MoveSpeed, 0.0f),
            new StatPriority(StatDefOf.WorkSpeedGlobal, 1f)
          });
          break;
        case "Soldier":
          extendedOutfit.AddRange((IEnumerable<StatPriority>) new List<StatPriority>()
          {
            new StatPriority(StatDefOf.ShootingAccuracyPawn, 2f),
            new StatPriority(StatDefOf.AccuracyShort, 1f),
            new StatPriority(StatDefOf.AccuracyMedium, 1f),
            new StatPriority(StatDefOf.AccuracyLong, 1f),
            new StatPriority(StatDefOf.MoveSpeed, 1f),
            new StatPriority(StatDefOf.ArmorRating_Blunt, 0.0f),
            new StatPriority(StatDefOf.ArmorRating_Sharp, 1f),
            new StatPriority(StatDefOf.MeleeDodgeChance, 0.0f),
            new StatPriority(StatDefOf.AimingDelayFactor, -2f),
            new StatPriority(StatDefOf.RangedWeapon_Cooldown, -2f),
            new StatPriority(StatDefOf.PainShockThreshold, 2f)
          });
          break;
        case "Nudist":
          extendedOutfit.AddRange((IEnumerable<StatPriority>) new List<StatPriority>()
          {
            new StatPriority(StatDefOf.MoveSpeed, 1f),
            new StatPriority(StatDefOf.WorkSpeedGlobal, 2f)
          });
          break;
        default:
          extendedOutfit.AddRange((IEnumerable<StatPriority>) new List<StatPriority>()
          {
            new StatPriority(StatDefOf.MoveSpeed, 1f),
            new StatPriority(StatDefOf.WorkSpeedGlobal, 2f),
            new StatPriority(StatDefOf.ArmorRating_Blunt, 1f),
            new StatPriority(StatDefOf.ArmorRating_Sharp, 1f)
          });
          break;
      }
      return (Outfit) extendedOutfit;
    }
  }
}
