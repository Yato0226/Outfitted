// Decompiled with JetBrains decompiler
// Type: Outfitted.Database.OutfitDatabase_GenerateStartingOutfits_Patch
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
  [HarmonyPatch(typeof (OutfitDatabase), "GenerateStartingOutfits")]
  public static class OutfitDatabase_GenerateStartingOutfits_Patch
  {
    private static bool Prefix(OutfitDatabase __instance)
    {
      try
      {
        OutfitDatabase_GenerateStartingOutfits_Patch.GenerateStartingOutfits(__instance);
      }
      catch (Exception ex)
      {
        Log.Error("Can't generate outfits: " + ex?.ToString());
      }
      return false;
    }

    internal static void GenerateStartingOutfits(OutfitDatabase db, bool vanilla = true)
    {
      if (vanilla)
      {
        OutfitDatabase_GenerateStartingOutfits_Patch.ConfigureOutfit(OutfitDatabase_GenerateStartingOutfits_Patch.MakeOutfit(db, "Anything", true), new Dictionary<StatDef, float>()
        {
          {
            StatDefOf.MoveSpeed,
            1f
          },
          {
            StatDefOf.WorkSpeedGlobal,
            2f
          },
          {
            StatDefOf.ArmorRating_Blunt,
            1f
          },
          {
            StatDefOf.ArmorRating_Sharp,
            1f
          }
        });
        OutfitDatabase_GenerateStartingOutfits_Patch.ConfigureOutfitWorker(OutfitDatabase_GenerateStartingOutfits_Patch.MakeOutfit(db, "Worker", true), new Dictionary<StatDef, float>()
        {
          {
            StatDefOf.MoveSpeed,
            0.0f
          },
          {
            StatDefOf.WorkSpeedGlobal,
            1f
          }
        });
      }
      OutfitDatabase_GenerateStartingOutfits_Patch.ConfigureOutfitWorker(OutfitDatabase_GenerateStartingOutfits_Patch.MakeOutfit(db, "Doctor"), new Dictionary<StatDef, float>()
      {
        {
          StatDefOf.MedicalSurgerySuccessChance,
          2f
        },
        {
          StatDefOf.MedicalOperationSpeed,
          2f
        },
        {
          StatDefOf.MedicalTendQuality,
          2f
        },
        {
          StatDefOf.MedicalTendSpeed,
          1f
        },
        {
          StatDefOf.WorkSpeedGlobal,
          1f
        }
      });
      OutfitDatabase_GenerateStartingOutfits_Patch.ConfigureOutfitWorker(OutfitDatabase_GenerateStartingOutfits_Patch.MakeOutfit(db, "Warden"), new Dictionary<StatDef, float>()
      {
        {
          StatDefOf.NegotiationAbility,
          2f
        },
        {
          StatDefOf.SocialImpact,
          1f
        },
        {
          StatDefOf.TradePriceImprovement,
          2f
        }
      });
      OutfitDatabase_GenerateStartingOutfits_Patch.ConfigureOutfitWorker(OutfitDatabase_GenerateStartingOutfits_Patch.MakeOutfit(db, "Handler"), new Dictionary<StatDef, float>()
      {
        {
          StatDefOf.TrainAnimalChance,
          2f
        },
        {
          StatDefOf.TameAnimalChance,
          2f
        },
        {
          StatDefOf.ArmorRating_Sharp,
          0.0f
        },
        {
          StatDefOf.MeleeDodgeChance,
          1f
        },
        {
          StatDefOf.MeleeHitChance,
          0.0f
        },
        {
          StatDefOf.MoveSpeed,
          0.0f
        },
        {
          StatDefOf.MeleeDPS,
          0.0f
        },
        {
          StatDefOf.AccuracyTouch,
          0.0f
        },
        {
          StatDefOf.MeleeWeapon_CooldownMultiplier,
          -2f
        },
        {
          StatDefOf.MeleeWeapon_DamageMultiplier,
          0.0f
        },
        {
          StatDefOf.PainShockThreshold,
          2f
        },
        {
          StatDefOf.AnimalGatherYield,
          2f
        },
        {
          StatDefOf.AnimalGatherSpeed,
          2f
        }
      });
      OutfitDatabase_GenerateStartingOutfits_Patch.ConfigureOutfitWorker(OutfitDatabase_GenerateStartingOutfits_Patch.MakeOutfit(db, "Cook"), new Dictionary<StatDef, float>()
      {
        {
          StatDefOf.DrugCookingSpeed,
          2f
        },
        {
          StatDefOf.ButcheryFleshSpeed,
          2f
        },
        {
          StatDefOf.ButcheryFleshEfficiency,
          2f
        },
        {
          StatDefOf.CookSpeed,
          2f
        },
        {
          StatDefOf.FoodPoisonChance,
          -2f
        },
        {
          StatDefOf.MoveSpeed,
          1f
        },
        {
          StatDefOf.WorkSpeedGlobal,
          1f
        }
      });
      OutfitDatabase_GenerateStartingOutfits_Patch.ConfigureOutfitSoldier(OutfitDatabase_GenerateStartingOutfits_Patch.MakeOutfit(db, "Hunter"), new Dictionary<StatDef, float>()
      {
        {
          StatDefOf.ShootingAccuracyPawn,
          2f
        },
        {
          StatDefOf.MoveSpeed,
          1f
        },
        {
          StatDefOf.AccuracyShort,
          1f
        },
        {
          StatDefOf.AccuracyMedium,
          1f
        },
        {
          StatDefOf.AccuracyLong,
          1f
        },
        {
          StatDefOf.MeleeDPS,
          0.0f
        },
        {
          StatDefOf.MeleeHitChance,
          0.0f
        },
        {
          StatDefOf.ArmorRating_Blunt,
          0.0f
        },
        {
          StatDefOf.ArmorRating_Sharp,
          0.0f
        },
        {
          StatDefOf.RangedWeapon_Cooldown,
          -2f
        },
        {
          StatDefOf.AimingDelayFactor,
          -2f
        },
        {
          StatDefOf.PainShockThreshold,
          2f
        }
      });
      OutfitDatabase_GenerateStartingOutfits_Patch.ConfigureOutfitWorker(OutfitDatabase_GenerateStartingOutfits_Patch.MakeOutfit(db, "Builder"), new Dictionary<StatDef, float>()
      {
        {
          StatDefOf.FixBrokenDownBuildingSuccessChance,
          2f
        },
        {
          StatDefOf.ConstructionSpeed,
          2f
        },
        {
          StatDefOf.ConstructSuccessChance,
          2f
        },
        {
          StatDefOf.SmoothingSpeed,
          2f
        },
        {
          StatDefOf.MoveSpeed,
          0.0f
        },
        {
          StatDefOf.WorkSpeedGlobal,
          0.0f
        }
      });
      OutfitDatabase_GenerateStartingOutfits_Patch.ConfigureOutfitWorker(OutfitDatabase_GenerateStartingOutfits_Patch.MakeOutfit(db, "Grower"), new Dictionary<StatDef, float>()
      {
        {
          StatDefOf.PlantHarvestYield,
          2f
        },
        {
          StatDefOf.PlantWorkSpeed,
          2f
        },
        {
          StatDefOf.MoveSpeed,
          0.0f
        },
        {
          StatDefOf.WorkSpeedGlobal,
          1f
        }
      });
      OutfitDatabase_GenerateStartingOutfits_Patch.ConfigureOutfitWorker(OutfitDatabase_GenerateStartingOutfits_Patch.MakeOutfit(db, "Miner"), new Dictionary<StatDef, float>()
      {
        {
          StatDefOf.MiningYield,
          2f
        },
        {
          StatDefOf.MiningSpeed,
          2f
        },
        {
          StatDefOf.MoveSpeed,
          0.0f
        },
        {
          StatDefOf.WorkSpeedGlobal,
          1f
        }
      });
      OutfitDatabase_GenerateStartingOutfits_Patch.ConfigureOutfitWorker(OutfitDatabase_GenerateStartingOutfits_Patch.MakeOutfit(db, "Smith"), new Dictionary<StatDef, float>()
      {
        {
          StatDefOf.GeneralLaborSpeed,
          2f
        },
        {
          StatDefOf.WorkSpeedGlobal,
          1f
        }
      });
      OutfitDatabase_GenerateStartingOutfits_Patch.ConfigureOutfitWorker(OutfitDatabase_GenerateStartingOutfits_Patch.MakeOutfit(db, "Tailor"), new Dictionary<StatDef, float>()
      {
        {
          StatDefOf.GeneralLaborSpeed,
          2f
        },
        {
          StatDefOf.WorkSpeedGlobal,
          1f
        }
      });
      OutfitDatabase_GenerateStartingOutfits_Patch.ConfigureOutfitWorker(OutfitDatabase_GenerateStartingOutfits_Patch.MakeOutfit(db, "Artist"), new Dictionary<StatDef, float>()
      {
        {
          StatDefOf.GeneralLaborSpeed,
          2f
        },
        {
          StatDefOf.WorkSpeedGlobal,
          1f
        }
      });
      OutfitDatabase_GenerateStartingOutfits_Patch.ConfigureOutfitWorker(OutfitDatabase_GenerateStartingOutfits_Patch.MakeOutfit(db, "Crafter"), new Dictionary<StatDef, float>()
      {
        {
          StatDefOf.GeneralLaborSpeed,
          2f
        },
        {
          StatDefOf.ButcheryMechanoidSpeed,
          2f
        },
        {
          StatDefOf.ButcheryMechanoidEfficiency,
          2f
        },
        {
          StatDefOf.WorkSpeedGlobal,
          2f
        }
      });
      OutfitDatabase_GenerateStartingOutfits_Patch.ConfigureOutfitWorker(OutfitDatabase_GenerateStartingOutfits_Patch.MakeOutfit(db, "Hauler"), new Dictionary<StatDef, float>()
      {
        {
          StatDefOf.MoveSpeed,
          2f
        },
        {
          StatDefOf.CarryingCapacity,
          2f
        }
      });
      OutfitDatabase_GenerateStartingOutfits_Patch.ConfigureOutfitWorker(OutfitDatabase_GenerateStartingOutfits_Patch.MakeOutfit(db, "Cleaner"), new Dictionary<StatDef, float>()
      {
        {
          StatDefOf.MoveSpeed,
          2f
        },
        {
          StatDefOf.WorkSpeedGlobal,
          2f
        }
      });
      OutfitDatabase_GenerateStartingOutfits_Patch.ConfigureOutfitWorker(OutfitDatabase_GenerateStartingOutfits_Patch.MakeOutfit(db, "Researcher"), new Dictionary<StatDef, float>()
      {
        {
          StatDefOf.ResearchSpeed,
          2f
        },
        {
          StatDefOf.WorkSpeedGlobal,
          1f
        }
      });
      OutfitDatabase_GenerateStartingOutfits_Patch.ConfigureOutfitSoldier(OutfitDatabase_GenerateStartingOutfits_Patch.MakeOutfit(db, "Brawler"), new Dictionary<StatDef, float>()
      {
        {
          StatDefOf.MoveSpeed,
          2f
        },
        {
          StatDefOf.AimingDelayFactor,
          -2f
        },
        {
          StatDefOf.MeleeDPS,
          2f
        },
        {
          StatDefOf.MeleeHitChance,
          2f
        },
        {
          StatDefOf.MeleeDodgeChance,
          2f
        },
        {
          StatDefOf.ArmorRating_Blunt,
          0.0f
        },
        {
          StatDefOf.ArmorRating_Sharp,
          1f
        },
        {
          StatDefOf.AccuracyTouch,
          2f
        },
        {
          StatDefOf.MeleeWeapon_DamageMultiplier,
          2f
        },
        {
          StatDefOf.MeleeWeapon_CooldownMultiplier,
          -2f
        },
        {
          StatDefOf.PainShockThreshold,
          2f
        }
      });
      if (!vanilla)
        return;
      OutfitDatabase_GenerateStartingOutfits_Patch.ConfigureOutfitSoldier(OutfitDatabase_GenerateStartingOutfits_Patch.MakeOutfit(db, "Soldier"), new Dictionary<StatDef, float>()
      {
        {
          StatDefOf.ShootingAccuracyPawn,
          2f
        },
        {
          StatDefOf.AccuracyShort,
          1f
        },
        {
          StatDefOf.AccuracyMedium,
          1f
        },
        {
          StatDefOf.AccuracyLong,
          1f
        },
        {
          StatDefOf.MoveSpeed,
          1f
        },
        {
          StatDefOf.ArmorRating_Blunt,
          0.0f
        },
        {
          StatDefOf.ArmorRating_Sharp,
          1f
        },
        {
          StatDefOf.MeleeDodgeChance,
          0.0f
        },
        {
          StatDefOf.AimingDelayFactor,
          -2f
        },
        {
          StatDefOf.RangedWeapon_Cooldown,
          -2f
        },
        {
          StatDefOf.PainShockThreshold,
          2f
        }
      });
      OutfitDatabase_GenerateStartingOutfits_Patch.ConfigureOutfitNudist(OutfitDatabase_GenerateStartingOutfits_Patch.MakeOutfit(db, "Nudist", true), new Dictionary<StatDef, float>()
      {
        {
          StatDefOf.MoveSpeed,
          1f
        },
        {
          StatDefOf.WorkSpeedGlobal,
          2f
        }
      });
    }

    private static ExtendedOutfit MakeOutfit(
      OutfitDatabase database,
      string name,
      bool autoWorkPriorities = false,
      bool autoTemp = true)
    {
      ExtendedOutfit extendedOutfit = database.MakeNewOutfit() as ExtendedOutfit;
      extendedOutfit.label = TaggedString.op_Implicit(Translator.Translate("Outfit" + name));
      extendedOutfit.AutoWorkPriorities = autoWorkPriorities;
      extendedOutfit.AutoTemp = autoTemp;
      return extendedOutfit;
    }

    private static void ConfigureOutfit(
      ExtendedOutfit outfit,
      Dictionary<StatDef, float> priorities)
    {
      outfit.AddRange(priorities.Select<KeyValuePair<StatDef, float>, StatPriority>((Func<KeyValuePair<StatDef, float>, StatPriority>) (i => new StatPriority(i.Key, i.Value, i.Value))));
    }

    private static void ConfigureOutfitFiltered(
      ExtendedOutfit outfit,
      Dictionary<StatDef, float> priorities,
      Func<ThingDef, bool> filter)
    {
      outfit.filter.SetDisallowAll((IEnumerable<ThingDef>) null, (IEnumerable<SpecialThingFilterDef>) null);
      outfit.filter.SetAllow(SpecialThingFilterDefOf.AllowDeadmansApparel, false);
      foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs.Where<ThingDef>(filter))
        outfit.filter.SetAllow(thingDef, true);
      OutfitDatabase_GenerateStartingOutfits_Patch.ConfigureOutfit(outfit, priorities);
    }

    private static void ConfigureOutfitTagged(
      ExtendedOutfit outfit,
      Dictionary<StatDef, float> priorities,
      string tag)
    {
      OutfitDatabase_GenerateStartingOutfits_Patch.ConfigureOutfitFiltered(outfit, priorities, (Func<ThingDef, bool>) (d => d.apparel?.defaultOutfitTags?.Contains(tag).GetValueOrDefault()));
    }

    private static void ConfigureOutfitWorker(
      ExtendedOutfit outfit,
      Dictionary<StatDef, float> priorities)
    {
      OutfitDatabase_GenerateStartingOutfits_Patch.ConfigureOutfitTagged(outfit, priorities, "Worker");
    }

    private static void ConfigureOutfitSoldier(
      ExtendedOutfit outfit,
      Dictionary<StatDef, float> priorities)
    {
      OutfitDatabase_GenerateStartingOutfits_Patch.ConfigureOutfitTagged(outfit, priorities, "Soldier");
    }

    private static void ConfigureOutfitNudist(
      ExtendedOutfit outfit,
      Dictionary<StatDef, float> priorities)
    {
      BodyPartGroupDef[] forbid = new BodyPartGroupDef[2]
      {
        BodyPartGroupDefOf.Legs,
        BodyPartGroupDefOf.Torso
      };
      OutfitDatabase_GenerateStartingOutfits_Patch.ConfigureOutfitFiltered(outfit, priorities, (Func<ThingDef, bool>) (d =>
      {
        ApparelProperties apparel = d.apparel;
        return apparel != null && apparel.bodyPartGroups.All<BodyPartGroupDef>((Func<BodyPartGroupDef, bool>) (g => !((IEnumerable<BodyPartGroupDef>) forbid).Contains<BodyPartGroupDef>(g)));
      }));
    }
  }
}
