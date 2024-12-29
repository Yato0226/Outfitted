// Decompiled with JetBrains decompiler
// Type: Outfitted.WorktypePriorities
// Assembly: Outfitted, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FB12A581-65CC-4BDE-BF48-F09425B0255F
// Assembly location: C:\Users\louiz\source\repos\Outfitted\1.4\Assemblies\Outfitted.dll

using System;
using System.Collections.Generic;
using Verse;

#nullable disable
namespace Outfitted
{
  public class WorktypePriorities : IExposable
  {
    public List<StatPriority> priorities = new List<StatPriority>();
    public WorkTypeDef worktype;

    public WorktypePriorities()
    {
    }

    public WorktypePriorities(WorkTypeDef worktype, List<StatPriority> priorities)
    {
      this.worktype = worktype;
      this.priorities = priorities;
    }

    public void ExposeData()
    {
      Scribe_Defs.Look<WorkTypeDef>(ref this.worktype, "worktype");
      Scribe_Collections.Look<StatPriority>(ref this.priorities, "statPriorities", (LookMode) 2, Array.Empty<object>());
    }
  }
}
