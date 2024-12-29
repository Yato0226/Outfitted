// Decompiled with JetBrains decompiler
// Type: Outfitted.ExtendedOutfitProxy
// Assembly: Outfitted, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FB12A581-65CC-4BDE-BF48-F09425B0255F
// Assembly location: C:\Users\louiz\source\repos\Outfitted\1.4\Assemblies\Outfitted.dll

using Multiplayer.API;
using RimWorld;
using System;
using System.Linq;
using Verse;

#nullable disable
namespace Outfitted
{
  [StaticConstructorOnStartup]
  public static class ExtendedOutfitProxy
  {
    private static readonly ISyncField[] ExtendedOutfitFields;
    private static readonly ISyncField[] ProxyFields;
    private static int targetOutfitId;
    private static StatDef targetStat;
    private static float targetWeight;

    static ExtendedOutfitProxy()
    {
      if (!MP.enabled)
        return;
      ExtendedOutfitProxy.ProxyFields = new ISyncField[1]
      {
        MP.RegisterSyncField(typeof (ExtendedOutfitProxy), nameof (targetWeight)).SetBufferChanges().PostApply(new Action<object, object>(ExtendedOutfitProxy.Update))
      };
      ExtendedOutfitProxy.ExtendedOutfitFields = new ISyncField[5]
      {
        MP.RegisterSyncField(typeof (ExtendedOutfit), "targetTemperaturesOverride"),
        MP.RegisterSyncField(typeof (ExtendedOutfit), "targetTemperatures"),
        MP.RegisterSyncField(typeof (ExtendedOutfit), "PenaltyWornByCorpse"),
        MP.RegisterSyncField(typeof (ExtendedOutfit), "AutoWorkPriorities"),
        MP.RegisterSyncField(typeof (ExtendedOutfit), "autoTempOffset")
      };
      MP.RegisterSyncMethod(typeof (ExtendedOutfit), "AutoTemp", (SyncType[]) null);
      MP.RegisterSyncMethod(typeof (ExtendedOutfit), "AddStatPriority", (SyncType[]) null);
      MP.RegisterSyncMethod(typeof (ExtendedOutfit), "RemoveStatPriority", (SyncType[]) null);
      MP.RegisterSyncMethod(typeof (ExtendedOutfit), "CopyFrom", (SyncType[]) null);
      MP.RegisterSyncMethod(typeof (ExtendedOutfitProxy), "SetStat", (SyncType[]) null);
      // ISSUE: method pointer
      MP.RegisterSyncWorker<ExtendedOutfit>(new SyncWorkerDelegate<ExtendedOutfit>((object) null, __methodptr(ExtendedOutfitSyncer)), (Type) null, false, false);
    }

    private static void Update(object arg1, object arg2)
    {
      float priority = (float) arg2;
      if (!(Current.Game.outfitDatabase.AllOutfits.Find((Predicate<Outfit>) (o => o.uniqueId == ExtendedOutfitProxy.targetOutfitId)) is ExtendedOutfit extendedOutfit))
        throw new Exception("Not an ExtendedOutfit");
      StatPriority statPriority = extendedOutfit.StatPriorities.FirstOrDefault<StatPriority>((Func<StatPriority, bool>) (sp => sp.Stat == ExtendedOutfitProxy.targetStat));
      if (statPriority == null)
        extendedOutfit.AddStatPriority(ExtendedOutfitProxy.targetStat, priority);
      else
        statPriority.Weight = priority;
    }

    private static void Watch(this ISyncField[] fields, object target = null)
    {
      foreach (ISyncField field in fields)
        field.Watch(target, (object) null);
    }

    public static void Watch(ref ExtendedOutfit outfit)
    {
      ExtendedOutfitProxy.ProxyFields.Watch();
      ExtendedOutfitProxy.ExtendedOutfitFields.Watch((object) outfit);
    }

    public static void SetStatPriority(int selectedOutfitId, StatDef stat, float weight)
    {
      if (ExtendedOutfitProxy.targetOutfitId != selectedOutfitId || !ExtendedOutfitProxy.targetStat.Equals((object) stat))
        ExtendedOutfitProxy.SetStat(selectedOutfitId, stat, weight);
      else
        ExtendedOutfitProxy.targetWeight = weight;
    }

    private static void SetStat(int uid, StatDef stat, float weight)
    {
      ExtendedOutfitProxy.targetOutfitId = uid;
      ExtendedOutfitProxy.targetStat = stat;
      ExtendedOutfitProxy.Update((object) null, (object) weight);
    }

    private static void ExtendedOutfitSyncer(SyncWorker sync, ref ExtendedOutfit outfit)
    {
      if (sync.isWriting)
      {
        sync.Bind(ref outfit.uniqueId);
      }
      else
      {
        int uid = 0;
        sync.Bind(ref uid);
        if (!(Current.Game.outfitDatabase.AllOutfits.Find((Predicate<Outfit>) (o => o.uniqueId == uid)) is ExtendedOutfit extendedOutfit))
          return;
        outfit = extendedOutfit;
      }
    }
  }
}
