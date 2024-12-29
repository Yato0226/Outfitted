// Decompiled with JetBrains decompiler
// Type: Outfitted.WorkPriorities
// Assembly: Outfitted, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FB12A581-65CC-4BDE-BF48-F09425B0255F
// Assembly location: C:\Users\louiz\source\repos\Outfitted\1.4\Assemblies\Outfitted.dll

using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

#nullable disable
namespace Outfitted
{
  public class WorkPriorities : WorldComponent
  {
    private static List<WorktypePriorities> _worktypePriorities;
    private static WorkPriorities _instance;

    public WorkPriorities(World world)
      : base(world)
    {
      WorkPriorities._instance = this;
      Log.Message("WorldComponent created!");
    }

    public static List<StatPriority> WorktypeStatPriorities(Pawn pawn)
    {
      IEnumerable<\u003C\u003Ef__AnonymousType2<int, WorkTypeDef, List<StatPriority>>> source = DefDatabase<WorkTypeDef>.AllDefsListForReading.Select(wtd => new
      {
        priority = pawn?.workSettings?.GetPriority(wtd).GetValueOrDefault(),
        worktype = wtd
      }).Where(x => x.priority > 0).Select(x => new
      {
        priority = x.priority,
        worktype = x.worktype,
        weights = WorkPriorities.WorktypeStatPriorities(x.worktype)
      });
      if (!source.Any())
        return new List<StatPriority>();
      IntRange intRange;
      // ISSUE: explicit constructor call
      ((IntRange) ref intRange).\u002Ector(source.Min(s => s.priority), source.Max(s => s.priority));
      List<StatPriority> statPriorityList = new List<StatPriority>();
      float num1 = 0.0f;
      foreach (var data in source)
      {
        int num2 = intRange.min == intRange.max ? 1 : 1 - (data.priority - intRange.min) / (intRange.max - intRange.min);
        foreach (StatPriority weight1 in data.weights)
        {
          StatPriority weight = weight1;
          StatPriority statPriority = GenCollection.FirstOrDefault<StatPriority>(statPriorityList, (Predicate<StatPriority>) (sp => sp.Stat == weight.Stat));
          if (statPriority != null)
          {
            statPriority.Weight += (float) num2 * weight.Weight;
          }
          else
          {
            statPriority = new StatPriority(weight.Stat, (float) num2 * weight.Weight);
            statPriorityList.Add(statPriority);
          }
          num1 += statPriority.Weight;
        }
      }
      if (GenCollection.Any<StatPriority>(statPriorityList) && (double) num1 != 0.0)
      {
        foreach (StatPriority statPriority in statPriorityList)
          statPriority.Weight *= 10f / num1;
      }
      return statPriorityList;
    }

    public static List<StatPriority> WorktypeStatPriorities(WorkTypeDef worktype)
    {
      WorktypePriorities worktypePriorities = WorkPriorities._worktypePriorities.Find((Predicate<WorktypePriorities>) (wp => wp.worktype == worktype));
      if (worktypePriorities == null)
      {
        Log.Warning("Outfitted :: Created worktype stat priorities for '" + ((Def) worktype).defName + "' after initial init. This should never happen!");
        worktypePriorities = new WorktypePriorities(worktype, WorkPriorities.DefaultPriorities(worktype));
        WorkPriorities._worktypePriorities.Add(worktypePriorities);
      }
      return worktypePriorities.priorities;
    }

    public virtual void ExposeData()
    {
      base.ExposeData();
      Scribe_Collections.Look<WorktypePriorities>(ref WorkPriorities._worktypePriorities, "worktypePriorities", (LookMode) 2, Array.Empty<object>());
    }

    public virtual void FinalizeInit()
    {
      base.FinalizeInit();
      if (!GenList.NullOrEmpty<WorktypePriorities>((IList<WorktypePriorities>) WorkPriorities._worktypePriorities))
        return;
      WorkPriorities._worktypePriorities = new List<WorktypePriorities>();
      foreach (WorkTypeDef worktype in DefDatabase<WorkTypeDef>.AllDefsListForReading)
        WorkPriorities._worktypePriorities.Add(new WorktypePriorities(worktype, WorkPriorities.DefaultPriorities(worktype)));
    }

    private static List<StatPriority> DefaultPriorities(WorkTypeDef worktype)
    {
      List<StatPriority> statPriorityList = new List<StatPriority>();
      if (worktype == WorkTypeDefOf.Art)
      {
        statPriorityList.Add(new StatPriority(StatDefOf.WorkSpeedGlobal, 1f));
        statPriorityList.Add(new StatPriority(StatDefOf.GeneralLaborSpeed, 2f));
      }
      if (worktype == WorkTypeDefOf.BasicWorker)
      {
        statPriorityList.Add(new StatPriority(StatDefOf.WorkSpeedGlobal, 1f));
        statPriorityList.Add(new StatPriority(StatDefOf.GeneralLaborSpeed, 2f));
      }
      if (worktype == WorkTypeDefOf.Cleaning)
      {
        statPriorityList.Add(new StatPriority(StatDefOf.MoveSpeed, 2f));
        statPriorityList.Add(new StatPriority(StatDefOf.WorkSpeedGlobal, 1f));
      }
      if (worktype == WorkTypeDefOf.Cooking)
      {
        statPriorityList.Add(new StatPriority(StatDefOf.CookSpeed, 2f));
        statPriorityList.Add(new StatPriority(StatDefOf.FoodPoisonChance, -2f));
        statPriorityList.Add(new StatPriority(StatDefOf.ButcheryFleshSpeed, 1f));
        statPriorityList.Add(new StatPriority(StatDefOf.ButcheryFleshEfficiency, 1f));
      }
      if (worktype == WorkTypeDefOf.Construction)
      {
        statPriorityList.Add(new StatPriority(StatDefOf.ConstructionSpeed, 2f));
        statPriorityList.Add(new StatPriority(StatDefOf.ConstructSuccessChance, 2f));
        statPriorityList.Add(new StatPriority(StatDefOf.FixBrokenDownBuildingSuccessChance, 1f));
        statPriorityList.Add(new StatPriority(StatDefOf.SmoothingSpeed, 1f));
      }
      if (worktype == WorkTypeDefOf.Crafting)
      {
        statPriorityList.Add(new StatPriority(StatDefOf.WorkSpeedGlobal, 2f));
        statPriorityList.Add(new StatPriority(StatDefOf.DrugSynthesisSpeed, 1f));
        statPriorityList.Add(new StatPriority(StatDefOf.DrugCookingSpeed, 1f));
        statPriorityList.Add(new StatPriority(StatDefOf.ButcheryMechanoidSpeed, 1f));
        statPriorityList.Add(new StatPriority(StatDefOf.ButcheryMechanoidEfficiency, 1f));
      }
      if (worktype == WorkTypeDefOf.Doctor)
      {
        statPriorityList.Add(new StatPriority(StatDefOf.MedicalTendSpeed, 1f));
        statPriorityList.Add(new StatPriority(StatDefOf.MedicalTendQuality, 2f));
        statPriorityList.Add(new StatPriority(StatDefOf.MedicalOperationSpeed, 1f));
        statPriorityList.Add(new StatPriority(StatDefOf.MedicalSurgerySuccessChance, 2f));
      }
      if (worktype == WorkTypeDefOf.Firefighter)
        statPriorityList.Add(new StatPriority(StatDefOf.MoveSpeed, 2f));
      if (worktype == WorkTypeDefOf.Growing)
      {
        statPriorityList.Add(new StatPriority(StatDefOf.PlantWorkSpeed, 2f));
        statPriorityList.Add(new StatPriority(StatDefOf.PlantHarvestYield, 2f));
      }
      if (worktype == WorkTypeDefOf.Handling)
      {
        statPriorityList.Add(new StatPriority(StatDefOf.MoveSpeed, 1f));
        statPriorityList.Add(new StatPriority(StatDefOf.TameAnimalChance, 2f));
        statPriorityList.Add(new StatPriority(StatDefOf.TrainAnimalChance, 2f));
        statPriorityList.Add(new StatPriority(StatDefOf.AnimalGatherSpeed, 1f));
        statPriorityList.Add(new StatPriority(StatDefOf.AnimalGatherYield, 1f));
      }
      if (worktype == WorkTypeDefOf.Hauling)
      {
        statPriorityList.Add(new StatPriority(StatDefOf.CarryingCapacity, 2f));
        statPriorityList.Add(new StatPriority(StatDefOf.MoveSpeed, 2f));
      }
      if (worktype == WorkTypeDefOf.Hunting)
      {
        statPriorityList.Add(new StatPriority(StatDefOf.MoveSpeed, 1f));
        statPriorityList.Add(new StatPriority(StatDefOf.ShootingAccuracyPawn, 2f));
        statPriorityList.Add(new StatPriority(StatDefOf.AimingDelayFactor, 1f));
        statPriorityList.Add(new StatPriority(StatDefOf.HuntingStealth, 2f));
        statPriorityList.Add(new StatPriority(StatDefOf.AccuracyTouch, 1f));
        statPriorityList.Add(new StatPriority(StatDefOf.AccuracyShort, 1f));
        statPriorityList.Add(new StatPriority(StatDefOf.AccuracyMedium, 1f));
        statPriorityList.Add(new StatPriority(StatDefOf.AccuracyLong, 2f));
        statPriorityList.Add(new StatPriority(StatDefOf.RangedWeapon_Cooldown, 1f));
        statPriorityList.Add(new StatPriority(StatDefOf.RangedWeapon_DamageMultiplier, 1f));
      }
      if (worktype == WorkTypeDefOf.Mining)
      {
        statPriorityList.Add(new StatPriority(StatDefOf.MiningSpeed, 2f));
        statPriorityList.Add(new StatPriority(StatDefOf.MiningYield, 2f));
      }
      if (worktype != WorkTypeDefOf.Patient)
      {
        WorkTypeDef patientBedRest = WorkTypeDefOf.PatientBedRest;
      }
      if (worktype == WorkTypeDefOf.PlantCutting)
      {
        statPriorityList.Add(new StatPriority(StatDefOf.PlantWorkSpeed, 2f));
        statPriorityList.Add(new StatPriority(StatDefOf.PlantHarvestYield, 2f));
      }
      if (worktype == WorkTypeDefOf.Research)
        statPriorityList.Add(new StatPriority(StatDefOf.ResearchSpeed, 2f));
      if (worktype == WorkTypeDefOf.Smithing)
      {
        statPriorityList.Add(new StatPriority(StatDefOf.WorkSpeedGlobal, 1f));
        statPriorityList.Add(new StatPriority(StatDefOf.GeneralLaborSpeed, 2f));
      }
      if (worktype == WorkTypeDefOf.Tailoring)
      {
        statPriorityList.Add(new StatPriority(StatDefOf.WorkSpeedGlobal, 1f));
        statPriorityList.Add(new StatPriority(StatDefOf.GeneralLaborSpeed, 2f));
      }
      if (worktype == WorkTypeDefOf.Warden)
      {
        statPriorityList.Add(new StatPriority(StatDefOf.NegotiationAbility, 2f));
        statPriorityList.Add(new StatPriority(StatDefOf.TradePriceImprovement, 1f));
        statPriorityList.Add(new StatPriority(StatDefOf.SocialImpact, 2f));
      }
      GenList.RemoveDuplicates<StatPriority>(statPriorityList, (Func<StatPriority, StatPriority, bool>) null);
      return statPriorityList;
    }
  }
}
