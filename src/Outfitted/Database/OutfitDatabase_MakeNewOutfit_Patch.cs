// Decompiled with JetBrains decompiler
// Type: Outfitted.Database.OutfitDatabase_MakeNewOutfit_Patch
// Assembly: Outfitted, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FB12A581-65CC-4BDE-BF48-F09425B0255F
// Assembly location: C:\Users\louiz\source\repos\Outfitted\1.4\Assemblies\Outfitted.dll

using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Outfitted.Database
{
  [HarmonyPatch(typeof (OutfitDatabase), "MakeNewOutfit")]
  internal static class OutfitDatabase_MakeNewOutfit_Patch
  {
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
      ConstructorInfo oldConstructor = AccessTools.Constructor(typeof (Outfit), new Type[2]
      {
        typeof (int),
        typeof (string)
      }, false);
      ConstructorInfo newConstructor = AccessTools.Constructor(typeof (ExtendedOutfit), new Type[2]
      {
        typeof (int),
        typeof (string)
      }, false);
      foreach (CodeInstruction instruction in instructions)
      {
        if (instruction.opcode == OpCodes.Newobj && oldConstructor.Equals(instruction.operand))
          instruction.operand = (object) newConstructor;
        yield return instruction;
      }
    }
  }
}
