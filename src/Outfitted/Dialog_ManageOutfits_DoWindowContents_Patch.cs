// Decompiled with JetBrains decompiler
// Type: Outfitted.Dialog_ManageOutfits_DoWindowContents_Patch
// Assembly: Outfitted, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FB12A581-65CC-4BDE-BF48-F09425B0255F
// Assembly location: C:\Users\louiz\source\repos\Outfitted\1.4\Assemblies\Outfitted.dll

using HarmonyLib;
using Multiplayer.API;
using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

#nullable disable
namespace Outfitted
{
  [HarmonyPatch(typeof (Dialog_ManageOutfits), "DoWindowContents")]
  internal static class Dialog_ManageOutfits_DoWindowContents_Patch
  {
    private const float marginVertical = 10f;
    private const float marginLeft = 320f;
    private const float marginRight = 10f;
    private const float marginTop = 10f;
    private const float marginBottom = 55f;
    private const float MaxValue = 2.5f;
    private static readonly FloatRange MinMaxTemperatureRange = new FloatRange(-100f, 100f);
    private static Vector2 scrollPosition = Vector2.zero;

    private static void Postfix(Rect inRect, Outfit ___selOutfitInt)
    {
      ExtendedOutfit outfit = ___selOutfitInt as ExtendedOutfit;
      Dialog_ManageOutfits_DoWindowContents_Patch.DrawCloneButton(outfit);
      if (outfit == null)
        return;
      float num = 50f;
      if (OutfittedMod.isSaveStorageSettingsEnabled)
        num += 40f;
      Rect canvas;
      // ISSUE: explicit constructor call
      ((Rect) ref canvas).\u002Ector(320f, num, (float) ((double) ((Rect) ref inRect).xMax - 320.0 - 10.0), (float) ((double) ((Rect) ref inRect).yMax - (double) num - 10.0 - 55.0));
      GUI.BeginGroup(canvas);
      Vector2 zero = Vector2.zero;
      if (MP.IsInMultiplayer)
      {
        MP.WatchBegin();
        ExtendedOutfitProxy.Watch(ref outfit);
      }
      Dialog_ManageOutfits_DoWindowContents_Patch.DrawDeadmanToogle(outfit, ref zero, canvas);
      Dialog_ManageOutfits_DoWindowContents_Patch.DrawAutoWorkPrioritiesToggle(outfit, ref zero, canvas);
      Dialog_ManageOutfits_DoWindowContents_Patch.DrawAutoTempToggle(outfit, ref zero, canvas);
      if (!outfit.AutoTemp)
        Dialog_ManageOutfits_DoWindowContents_Patch.DrawTemperatureStats(outfit, ref zero, canvas);
      else
        Dialog_ManageOutfits_DoWindowContents_Patch.DrawAutoTempOffsetInput(outfit, ref zero, canvas);
      zero.y += 10f;
      Dialog_ManageOutfits_DoWindowContents_Patch.DrawApparelStats(outfit, zero, canvas);
      if (MP.IsInMultiplayer)
        MP.WatchEnd();
      else if (GUI.changed)
        OutfittedMod.Notify_OutfitChanged(outfit.uniqueId);
      GUI.EndGroup();
      GUI.color = Color.white;
      Text.Anchor = (TextAnchor) 0;
    }

    private static void DrawCloneButton(ExtendedOutfit selectedOutfit)
    {
      if (!Widgets.ButtonText(new Rect(480f, 0.0f, 150f, 35f), TaggedString.op_Implicit(Translator.Translate("CommandCopyZoneSettingsLabel")), true, true, true, new TextAnchor?()))
        return;
      if (selectedOutfit == null)
      {
        Messages.Message(TaggedString.op_Implicit(Translator.Translate("NoOutfitSelected")), MessageTypeDefOf.RejectInput, false);
      }
      else
      {
        List<FloatMenuOption> floatMenuOptionList = new List<FloatMenuOption>();
        foreach (Outfit allOutfit in Current.Game.outfitDatabase.AllOutfits)
        {
          Outfit outfit = allOutfit;
          if (outfit != selectedOutfit)
            floatMenuOptionList.Add(new FloatMenuOption(outfit.label, (Action) (() => selectedOutfit.CopyFrom((ExtendedOutfit) outfit)), (MenuOptionPriority) 4, (Action<Rect>) null, (Thing) null, 0.0f, (Func<Rect, bool>) null, (WorldObject) null, true, 0));
        }
        Find.WindowStack.Add((Window) new FloatMenu(floatMenuOptionList));
      }
    }

    private static void DrawDeadmanToogle(
      ExtendedOutfit selectedOutfit,
      ref Vector2 cur,
      Rect canvas)
    {
      Rect rect;
      // ISSUE: explicit constructor call
      ((Rect) ref rect).\u002Ector(cur.x, cur.y, ((Rect) ref canvas).width, 30f);
      Widgets.CheckboxLabeled(rect, ResourceBank.Strings.PenaltyWornByCorpse, ref selectedOutfit.PenaltyWornByCorpse, false, (Texture2D) null, (Texture2D) null, false);
      TooltipHandler.TipRegion(rect, TipSignal.op_Implicit(ResourceBank.Strings.PenaltyWornByCorpseTooltip));
      cur.y += ((Rect) ref rect).height;
    }

    private static void DrawAutoWorkPrioritiesToggle(
      ExtendedOutfit outfit,
      ref Vector2 pos,
      Rect canvas)
    {
      Rect rect;
      // ISSUE: explicit constructor call
      ((Rect) ref rect).\u002Ector(pos.x, pos.y, ((Rect) ref canvas).width, 30f);
      Widgets.CheckboxLabeled(rect, ResourceBank.Strings.AutoWorkPriorities, ref outfit.AutoWorkPriorities, false, (Texture2D) null, (Texture2D) null, false);
      TooltipHandler.TipRegion(rect, TipSignal.op_Implicit(ResourceBank.Strings.AutoWorkPrioritiesTooltip));
      pos.y += ((Rect) ref rect).height;
    }

    private static void DrawAutoTempToggle(ExtendedOutfit outfit, ref Vector2 pos, Rect canvas)
    {
      Rect rect;
      // ISSUE: explicit constructor call
      ((Rect) ref rect).\u002Ector(pos.x, pos.y, ((Rect) ref canvas).width, 30f);
      bool autoTemp = outfit.AutoTemp;
      Widgets.CheckboxLabeled(rect, ResourceBank.Strings.AutoTemp, ref autoTemp, false, (Texture2D) null, (Texture2D) null, false);
      if (autoTemp != outfit.AutoTemp)
        outfit.AutoTemp = autoTemp;
      TooltipHandler.TipRegion(rect, TipSignal.op_Implicit(ResourceBank.Strings.AutoTempTooltip));
      pos.y += ((Rect) ref rect).height;
    }

    private static void DrawAutoTempOffsetInput(
      ExtendedOutfit outfit,
      ref Vector2 pos,
      Rect canvas)
    {
      Rect rect;
      // ISSUE: explicit constructor call
      ((Rect) ref rect).\u002Ector(pos.x, pos.y, ((Rect) ref canvas).width, 30f);
      string str = outfit.autoTempOffset.ToString();
      Widgets.IntEntry(rect, ref outfit.autoTempOffset, ref str, 1);
      TooltipHandler.TipRegion(rect, TipSignal.op_Implicit(ResourceBank.Strings.AutoTempOffsetTooltip));
      pos.y += ((Rect) ref rect).height;
    }

    private static void DrawTemperatureStats(
      ExtendedOutfit selectedOutfit,
      ref Vector2 cur,
      Rect canvas)
    {
      Rect rect1 = new Rect(cur.x, cur.y, ((Rect) ref canvas).width, 30f);
      cur.y += 30f;
      Text.Anchor = (TextAnchor) 6;
      string preferedTemperature = ResourceBank.Strings.PreferedTemperature;
      Widgets.Label(rect1, preferedTemperature);
      Text.Anchor = (TextAnchor) 0;
      GUI.color = Color.grey;
      Widgets.DrawLineHorizontal(cur.x, cur.y, ((Rect) ref canvas).width);
      GUI.color = Color.white;
      cur.y += 10f;
      Rect canvas1;
      // ISSUE: explicit constructor call
      ((Rect) ref canvas1).\u002Ector(cur.x, cur.y, ((Rect) ref canvas).width - 20f, 40f);
      Rect rect2;
      // ISSUE: explicit constructor call
      ((Rect) ref rect2).\u002Ector(((Rect) ref canvas1).xMax + 4f, cur.y + 10f, 16f, 16f);
      cur.y += 40f;
      FloatRange range;
      if (selectedOutfit.targetTemperaturesOverride)
      {
        range = selectedOutfit.targetTemperatures;
        GUI.color = Color.white;
      }
      else
      {
        range = Dialog_ManageOutfits_DoWindowContents_Patch.MinMaxTemperatureRange;
        GUI.color = Color.grey;
      }
      FloatRange temperatureRange = Dialog_ManageOutfits_DoWindowContents_Patch.MinMaxTemperatureRange;
      Widgets_FloatRange.FloatRange(canvas1, 123123123, ref range, temperatureRange, (ToStringStyle) 11);
      GUI.color = Color.white;
      if ((double) Math.Abs(range.min - selectedOutfit.targetTemperatures.min) > 0.0001 || (double) Math.Abs(range.max - selectedOutfit.targetTemperatures.max) > 0.0001)
      {
        selectedOutfit.targetTemperatures = range;
        selectedOutfit.targetTemperaturesOverride = true;
      }
      if (!selectedOutfit.targetTemperaturesOverride)
        return;
      if (Widgets.ButtonImage(rect2, ResourceBank.Textures.ResetButton, true))
      {
        selectedOutfit.targetTemperaturesOverride = false;
        selectedOutfit.targetTemperatures = Dialog_ManageOutfits_DoWindowContents_Patch.MinMaxTemperatureRange;
      }
      TooltipHandler.TipRegion(rect2, TipSignal.op_Implicit(ResourceBank.Strings.TemperatureRangeReset));
    }

    private static void DrawApparelStats(ExtendedOutfit selectedOutfit, Vector2 cur, Rect canvas)
    {
      Rect rect1;
      // ISSUE: explicit constructor call
      ((Rect) ref rect1).\u002Ector(cur.x, cur.y, ((Rect) ref canvas).width, 30f);
      cur.y += 30f;
      Text.Anchor = (TextAnchor) 6;
      Text.Font = (GameFont) 1;
      Widgets.Label(rect1, ResourceBank.Strings.PreferedStats);
      Text.Anchor = (TextAnchor) 0;
      Rect rect2;
      // ISSUE: explicit constructor call
      ((Rect) ref rect2).\u002Ector(((Rect) ref rect1).xMax - 16f, ((Rect) ref rect1).yMin + 10f, 16f, 16f);
      if (Widgets.ButtonImage(rect2, ResourceBank.Textures.AddButton, true))
      {
        List<FloatMenuOption> floatMenuOptionList = new List<FloatMenuOption>();
        foreach (StatDef statDef in (IEnumerable<StatDef>) selectedOutfit.UnassignedStats.Where<StatDef>((Func<StatDef, bool>) (i => !i.alwaysHide)).OrderBy<StatDef, string>((Func<StatDef, string>) (i => ((Def) i).label)).OrderBy<StatDef, int>((Func<StatDef, int>) (i => i.category.displayOrder)))
        {
          StatDef def = statDef;
          FloatMenuOption floatMenuOption = new FloatMenuOption(TaggedString.op_Implicit(((Def) def).LabelCap), (Action) (() => selectedOutfit.AddStatPriority(def, 0.0f)), (MenuOptionPriority) 4, (Action<Rect>) null, (Thing) null, 0.0f, (Func<Rect, bool>) null, (WorldObject) null, true, 0);
          floatMenuOptionList.Add(floatMenuOption);
        }
        Find.WindowStack.Add((Window) new FloatMenu(floatMenuOptionList));
      }
      TooltipHandler.TipRegion(rect2, TipSignal.op_Implicit(ResourceBank.Strings.StatPriorityAdd));
      GUI.color = Color.grey;
      Widgets.DrawLineHorizontal(cur.x, cur.y, ((Rect) ref canvas).width);
      GUI.color = Color.white;
      cur.y += 10f;
      List<StatPriority> list = selectedOutfit.StatPriorities.ToList<StatPriority>();
      Rect rect3;
      // ISSUE: explicit constructor call
      ((Rect) ref rect3).\u002Ector(cur.x, cur.y, ((Rect) ref canvas).width, ((Rect) ref canvas).height - cur.y);
      Rect rect4;
      // ISSUE: explicit constructor call
      ((Rect) ref rect4).\u002Ector(rect3);
      ((Rect) ref rect4).height = 30f * (float) list.Count;
      Rect rect5 = rect4;
      if ((double) ((Rect) ref rect5).height > (double) ((Rect) ref rect3).height)
      {
        ref Rect local = ref rect5;
        ((Rect) ref local).width = ((Rect) ref local).width - 20f;
      }
      Widgets.BeginScrollView(rect3, ref Dialog_ManageOutfits_DoWindowContents_Patch.scrollPosition, rect5, true);
      GUI.BeginGroup(rect5);
      cur = Vector2.zero;
      if (list.Count > 0)
      {
        Rect rect6 = new Rect(cur.x + (float) (((double) ((Rect) ref rect5).width - 24.0) / 2.0), cur.y, (float) (((double) ((Rect) ref rect5).width - 24.0) / 2.0), 20f);
        Text.Font = (GameFont) 0;
        GUI.color = Color.grey;
        Text.Anchor = (TextAnchor) 6;
        float num = 2.5f;
        Widgets.Label(rect6, "-" + num.ToString("N1"));
        Text.Anchor = (TextAnchor) 8;
        num = 2.5f;
        Widgets.Label(rect6, num.ToString("N1"));
        Text.Anchor = (TextAnchor) 0;
        Text.Font = (GameFont) 1;
        GUI.color = Color.white;
        cur.y += 15f;
        foreach (StatPriority statPriority in list)
          Dialog_ManageOutfits_DoWindowContents_Patch.DrawStatRow(selectedOutfit, statPriority, ref cur, ((Rect) ref rect5).width);
      }
      else
      {
        Rect rect7 = new Rect(cur.x, cur.y, ((Rect) ref rect5).width, 30f);
        GUI.color = Color.grey;
        Text.Anchor = (TextAnchor) 4;
        string none = ResourceBank.Strings.None;
        Widgets.Label(rect7, none);
        Text.Anchor = (TextAnchor) 0;
        GUI.color = Color.white;
        cur.y += 30f;
      }
      GUI.EndGroup();
      Widgets.EndScrollView();
    }

    private static void DrawStatRow(
      ExtendedOutfit selectedOutfit,
      StatPriority statPriority,
      ref Vector2 cur,
      float width)
    {
      Rect rect1;
      // ISSUE: explicit constructor call
      ((Rect) ref rect1).\u002Ector(cur.x, cur.y, (float) (((double) width - 24.0) / 2.0), 30f);
      Rect rect2;
      // ISSUE: explicit constructor call
      ((Rect) ref rect2).\u002Ector(((Rect) ref rect1).xMax + 4f, cur.y + 5f, ((Rect) ref rect1).width, 25f);
      Rect rect3;
      // ISSUE: explicit constructor call
      ((Rect) ref rect3).\u002Ector(((Rect) ref rect2).xMax + 4f, cur.y + 3f, 16f, 16f);
      Text.Font = (double) Text.CalcHeight(TaggedString.op_Implicit(((Def) statPriority.Stat).LabelCap), ((Rect) ref rect1).width) > (double) ((Rect) ref rect1).height ? (GameFont) 0 : (GameFont) 1;
      GUI.color = Dialog_ManageOutfits_DoWindowContents_Patch.AssigmentColor(statPriority);
      Widgets.Label(rect1, ((Def) statPriority.Stat).LabelCap);
      Text.Font = (GameFont) 1;
      string str = string.Empty;
      if (statPriority.IsManual)
      {
        str = ResourceBank.Strings.StatPriorityDelete(TaggedString.op_Implicit(((Def) statPriority.Stat).LabelCap));
        if (Widgets.ButtonImage(rect3, ResourceBank.Textures.DeleteButton, true))
          selectedOutfit.RemoveStatPriority(statPriority.Stat);
      }
      else if (statPriority.IsOverride)
      {
        str = ResourceBank.Strings.StatPriorityReset(TaggedString.op_Implicit(((Def) statPriority.Stat).LabelCap));
        if (Widgets.ButtonImage(rect3, ResourceBank.Textures.ResetButton, true))
        {
          statPriority.Weight = statPriority.Default;
          if (MP.IsInMultiplayer)
            ExtendedOutfitProxy.SetStatPriority(selectedOutfit.uniqueId, statPriority.Stat, statPriority.Default);
        }
      }
      GUI.color = new Color(0.3f, 0.3f, 0.3f);
      for (int y = (int) cur.y; (double) y < (double) cur.y + 30.0; y += 5)
        Widgets.DrawLineVertical((float) (((double) ((Rect) ref rect2).xMin + (double) ((Rect) ref rect2).xMax) / 2.0), (float) y, 3f);
      GUI.color = Dialog_ManageOutfits_DoWindowContents_Patch.AssigmentColor(statPriority);
      float weight = GUI.HorizontalSlider(rect2, statPriority.Weight, -2.5f, 2.5f);
      if ((double) Mathf.Abs(weight - statPriority.Weight) > 0.0001)
      {
        statPriority.Weight = weight;
        if (MP.IsInMultiplayer)
          ExtendedOutfitProxy.SetStatPriority(selectedOutfit.uniqueId, statPriority.Stat, weight);
      }
      GUI.color = Color.white;
      TooltipHandler.TipRegion(rect1, TipSignal.op_Implicit(TaggedString.op_Addition(TaggedString.op_Addition(((Def) statPriority.Stat).LabelCap, "\n\n"), ((Def) statPriority.Stat).description)));
      if (str != string.Empty)
        TooltipHandler.TipRegion(rect3, TipSignal.op_Implicit(str));
      TooltipHandler.TipRegion(rect2, TipSignal.op_Implicit(GenText.ToStringByStyle(statPriority.Weight, (ToStringStyle) 2, (ToStringNumberSense) 1)));
      cur.y += 30f;
    }

    private static Color AssigmentColor(StatPriority statPriority)
    {
      if (statPriority.IsManual)
        return Color.white;
      if (statPriority.IsDefault)
        return Color.grey;
      return statPriority.IsOverride ? new Color(0.75f, 0.69f, 0.33f) : Color.cyan;
    }
  }
}
