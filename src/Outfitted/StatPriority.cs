// Decompiled with JetBrains decompiler
// Type: Outfitted.StatPriority
// Assembly: Outfitted, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FB12A581-65CC-4BDE-BF48-F09425B0255F
// Assembly location: C:\Users\louiz\source\repos\Outfitted\1.4\Assemblies\Outfitted.dll

using RimWorld;
using Verse;

#nullable disable
namespace Outfitted
{
  public class StatPriority : IExposable
  {
    private StatDef stat;
    public float Weight;
    public float Default;

    public StatPriority(StatDef stat, float weight, float defaultWeight = float.NaN)
    {
      this.stat = stat;
      this.Weight = weight;
      this.Default = defaultWeight;
    }

    public StatPriority()
    {
    }

    public StatDef Stat => this.stat;

    public bool IsDefault => (double) this.Default == (double) this.Weight;

    public bool IsManual => float.IsNaN(this.Default);

    public bool IsOverride => !this.IsManual && !this.IsDefault;

    public void ExposeData()
    {
      Scribe_Defs.Look<StatDef>(ref this.stat, "Stat");
      Scribe_Values.Look<float>(ref this.Weight, "Weight", 0.0f, false);
      Scribe_Values.Look<float>(ref this.Default, "Default", float.NaN, false);
    }
  }
}
