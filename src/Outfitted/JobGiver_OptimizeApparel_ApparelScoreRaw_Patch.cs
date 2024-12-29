// Decompiled with JetBrains decompiler
// Type: Outfitted.JobGiver_OptimizeApparel_ApparelScoreRaw_Patch
// Assembly: Outfitted, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FB12A581-65CC-4BDE-BF48-F09425B0255F
// Assembly location: C:\Users\louiz\source\repos\Outfitted\1.4\Assemblies\Outfitted.dll

using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using Verse;

#nullable disable
namespace Outfitted
{
  [HarmonyPatch(typeof (JobGiver_OptimizeApparel), "ApparelScoreRaw")]
  internal static class JobGiver_OptimizeApparel_ApparelScoreRaw_Patch
  {
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
      MethodInfo add = AccessTools.Method(typeof (JobGiver_OptimizeApparel_ApparelScoreRaw_Patch), "ApparelScoreExtra", (Type[]) null, (Type[]) null);
      MethodInfo find = AccessTools.PropertyGetter(typeof (Thing), "Stuff");
      FieldInfo fld = AccessTools.Field(typeof (JobGiver_OptimizeApparel), "neededWarmth");
      foreach (CodeInstruction ins in instructions)
      {
        if (ins.opcode == OpCodes.Callvirt && find.Equals(ins.operand))
        {
          yield return new CodeInstruction(OpCodes.Ldarg_0, (object) null);
          yield return new CodeInstruction(OpCodes.Ldsfld, (object) fld);
          yield return new CodeInstruction(OpCodes.Call, (object) add);
          yield return new CodeInstruction(OpCodes.Stloc_0, (object) null);
          yield return new CodeInstruction(OpCodes.Ldarg_1, (object) null);
        }
        yield return ins;
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float ApparelScoreExtra(Apparel ap, Pawn pawn, NeededWarmth neededWarmth)
    {
      return OutfittedMod.ApparelScoreExtra(pawn, ap, neededWarmth);
    }
  }
}
