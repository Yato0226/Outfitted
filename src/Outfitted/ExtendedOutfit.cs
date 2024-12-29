// Decompiled with JetBrains decompiler
// Type: Outfitted.ExtendedOutfit
// Assembly: Outfitted, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FB12A581-65CC-4BDE-BF48-F09425B0255F
// Assembly location: C:\Users\louiz\source\repos\Outfitted\1.4\Assemblies\Outfitted.dll

using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

#nullable disable
namespace Outfitted
{
  public class ExtendedOutfit : Outfit, IExposable
  {
    public bool targetTemperaturesOverride;
    public FloatRange targetTemperatures = new FloatRange(-100f, 100f);
    public bool PenaltyWornByCorpse = true;
    public bool AutoWorkPriorities;
    private bool _autoTemp;
    public int autoTempOffset = 20;
    private static IEnumerable<StatCategoryDef> blacklistedCategories = (IEnumerable<StatCategoryDef>) new List<StatCategoryDef>()
    {
      StatCategoryDefOf.BasicsNonPawn,
      StatCategoryDefOf.Building,
      StatCategoryDefOf.StuffStatFactors
    };
    private static readonly IEnumerable<StatDef> blacklistedStats = (IEnumerable<StatDef>) new List<StatDef>()
    {
      StatDefOf.ComfyTemperatureMin,
      StatDefOf.ComfyTemperatureMax,
      StatDefOf.Insulation_Cold,
      StatDefOf.Insulation_Heat,
      StatDefOf.StuffEffectMultiplierInsulation_Cold,
      StatDefOf.StuffEffectMultiplierInsulation_Heat,
      StatDefOf.StuffEffectMultiplierArmor
    };
    private List<StatPriority> statPriorities = new List<StatPriority>();

    public bool AutoTemp
    {
      get => this._autoTemp;
      set
      {
        this._autoTemp = value;
        if (!this._autoTemp)
          return;
        this.targetTemperaturesOverride = true;
      }
    }

    internal static IEnumerable<StatDef> AllAvailableStats
    {
      get
      {
        return (IEnumerable<StatDef>) DefDatabase<StatDef>.AllDefs.Where<StatDef>((Func<StatDef, bool>) (i => !ExtendedOutfit.blacklistedCategories.Contains<StatCategoryDef>(i.category))).Except<StatDef>(ExtendedOutfit.blacklistedStats).ToList<StatDef>();
      }
    }

    public IEnumerable<StatDef> UnassignedStats
    {
      get
      {
        return ExtendedOutfit.AllAvailableStats.Except<StatDef>(this.StatPriorities.Select<StatPriority, StatDef>((Func<StatPriority, StatDef>) (i => i.Stat)));
      }
    }

    public IEnumerable<StatPriority> StatPriorities
    {
      get => (IEnumerable<StatPriority>) this.statPriorities;
    }

    public ExtendedOutfit(int uniqueId, string label)
      : base(uniqueId, label)
    {
    }

    public ExtendedOutfit(Outfit outfit)
      : base(outfit.uniqueId, outfit.label)
    {
      this.filter.CopyAllowancesFrom(outfit.filter);
    }

    public ExtendedOutfit()
    {
    }

    public void AddStatPriority(StatDef def, float priority, float defaultPriority = float.NaN)
    {
      this.statPriorities.Insert(0, new StatPriority(def, priority, defaultPriority));
    }

    public void AddRange(IEnumerable<StatPriority> priorities)
    {
      this.statPriorities.AddRange(priorities);
    }

    public void RemoveStatPriority(StatDef def)
    {
      this.statPriorities.RemoveAll((Predicate<StatPriority>) (i => i.Stat == def));
    }

    public void ExposeData()
    {
      Scribe_Values.Look<int>(ref this.uniqueId, "uniqueId", 0, false);
      Scribe_Values.Look<string>(ref this.label, "label", (string) null, false);
      Scribe_Deep.Look<ThingFilter>(ref this.filter, "filter", new object[0]);
      Scribe_Values.Look<bool>(ref this.targetTemperaturesOverride, "targetTemperaturesOverride", false, false);
      Scribe_Values.Look<FloatRange>(ref this.targetTemperatures, "targetTemperatures", new FloatRange(), false);
      Scribe_Values.Look<bool>(ref this.PenaltyWornByCorpse, "PenaltyWornByCorpse", true, false);
      Scribe_Collections.Look<StatPriority>(ref this.statPriorities, "statPriorities", (LookMode) 2, Array.Empty<object>());
      Scribe_Values.Look<bool>(ref this.AutoWorkPriorities, "AutoWorkPriorities", false, false);
      Scribe_Values.Look<bool>(ref this._autoTemp, "AutoTemp", false, false);
      Scribe_Values.Look<int>(ref this.autoTempOffset, "autoTempOffset", 0, false);
    }

    public void CopyFrom(ExtendedOutfit outfit)
    {
      this.filter.CopyAllowancesFrom(outfit.filter);
      this.targetTemperaturesOverride = outfit.targetTemperaturesOverride;
      this.targetTemperatures = outfit.targetTemperatures;
      this.PenaltyWornByCorpse = outfit.PenaltyWornByCorpse;
      this.statPriorities.Clear();
      foreach (StatPriority statPriority in outfit.statPriorities)
        this.statPriorities.Add(new StatPriority(statPriority.Stat, statPriority.Weight, statPriority.Default));
      this.AutoWorkPriorities = outfit.AutoWorkPriorities;
      this._autoTemp = outfit._autoTemp;
      this.autoTempOffset = outfit.autoTempOffset;
    }
  }
}
