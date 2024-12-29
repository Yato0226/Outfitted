// Decompiled with JetBrains decompiler
// Type: Outfitted.Widgets_FloatRange
// Assembly: Outfitted, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FB12A581-65CC-4BDE-BF48-F09425B0255F
// Assembly location: C:\Users\louiz\source\repos\Outfitted\1.4\Assemblies\Outfitted.dll

using UnityEngine;
using Verse;

#nullable disable
namespace Outfitted
{
  public static class Widgets_FloatRange
  {
    private static Widgets_FloatRange.Handle _draggingHandle = Widgets_FloatRange.Handle.None;
    private static int _draggingId = 0;

    public static void FloatRange(
      Rect canvas,
      int id,
      ref Verse.FloatRange range,
      Verse.FloatRange sliderRange,
      ToStringStyle valueStyle = 2,
      string labelKey = null)
    {
      ref Rect local1 = ref canvas;
      ((Rect) ref local1).xMin = ((Rect) ref local1).xMin + 8f;
      ref Rect local2 = ref canvas;
      ((Rect) ref local2).xMax = ((Rect) ref local2).xMax - 8f;
      Color color = GUI.color;
      GUI.color = new Color(0.4f, 0.4f, 0.4f);
      string str = GenText.ToStringByStyle(range.min, valueStyle, (ToStringNumberSense) 1) + " - " + GenText.ToStringByStyle(range.max, valueStyle, (ToStringNumberSense) 1);
      if (labelKey != null)
        str = TaggedString.op_Implicit(TranslatorFormattedStringExtensions.Translate(labelKey, NamedArgument.op_Implicit(str)));
      Text.Font = (GameFont) 0;
      Rect rect1;
      // ISSUE: explicit constructor call
      ((Rect) ref rect1).\u002Ector(((Rect) ref canvas).x, ((Rect) ref canvas).y, ((Rect) ref canvas).width, 19f);
      Text.Anchor = (TextAnchor) 1;
      Widgets.Label(rect1, str);
      Text.Anchor = (TextAnchor) 0;
      Rect rect2;
      // ISSUE: explicit constructor call
      ((Rect) ref rect2).\u002Ector(((Rect) ref canvas).x, ((Rect) ref rect1).yMax, ((Rect) ref canvas).width, 2f);
      GUI.DrawTexture(rect2, (Texture) BaseContent.WhiteTex);
      GUI.color = color;
      float num1 = ((Rect) ref rect2).width / ((Verse.FloatRange) ref sliderRange).Span;
      float num2 = ((Rect) ref rect2).xMin + (range.min - sliderRange.min) * num1;
      float num3 = ((Rect) ref rect2).xMin + (range.max - sliderRange.min) * num1;
      Rect rect3;
      // ISSUE: explicit constructor call
      ((Rect) ref rect3).\u002Ector(num2 - 16f, ((Rect) ref rect2).center.y - 8f, 16f, 16f);
      GUI.DrawTexture(rect3, (Texture) ResourceBank.Textures.FloatRangeSliderTex);
      Rect rect4;
      // ISSUE: explicit constructor call
      ((Rect) ref rect4).\u002Ector(num3 + 16f, ((Rect) ref rect2).center.y - 8f, -16f, 16f);
      GUI.DrawTexture(rect4, (Texture) ResourceBank.Textures.FloatRangeSliderTex);
      Rect rect5 = canvas;
      ref Rect local3 = ref rect5;
      ((Rect) ref local3).xMin = ((Rect) ref local3).xMin - 8f;
      ref Rect local4 = ref rect5;
      ((Rect) ref local4).xMax = ((Rect) ref local4).xMax + 8f;
      bool flag = false;
      if (Mouse.IsOver(rect5) || Widgets_FloatRange._draggingId == id)
      {
        if (Event.current.type == null && Event.current.button == 0)
        {
          Widgets_FloatRange._draggingId = id;
          float x = Event.current.mousePosition.x;
          Widgets_FloatRange._draggingHandle = (double) x >= (double) ((Rect) ref rect3).xMax ? ((double) x <= (double) ((Rect) ref rect4).xMin ? ((double) Mathf.Abs(x - ((Rect) ref rect3).xMax) >= (double) Mathf.Abs(x - (((Rect) ref rect4).x - 16f)) ? Widgets_FloatRange.Handle.Max : Widgets_FloatRange.Handle.Min) : Widgets_FloatRange.Handle.Max) : Widgets_FloatRange.Handle.Min;
          flag = true;
          Event.current.Use();
        }
        if (flag || Widgets_FloatRange._draggingHandle != Widgets_FloatRange.Handle.None && Event.current.type == 3)
        {
          float num4 = Mathf.Clamp((Event.current.mousePosition.x - ((Rect) ref canvas).x) / ((Rect) ref canvas).width * ((Verse.FloatRange) ref sliderRange).Span + sliderRange.min, sliderRange.min, sliderRange.max);
          switch (Widgets_FloatRange._draggingHandle)
          {
            case Widgets_FloatRange.Handle.Min:
              range.min = num4;
              if ((double) range.max < (double) range.min)
              {
                range.max = range.min;
                break;
              }
              break;
            case Widgets_FloatRange.Handle.Max:
              range.max = num4;
              if ((double) range.min > (double) range.max)
              {
                range.min = range.max;
                break;
              }
              break;
          }
          Event.current.Use();
        }
      }
      if (Widgets_FloatRange._draggingHandle == Widgets_FloatRange.Handle.None || Event.current.type != 1)
        return;
      Widgets_FloatRange._draggingId = 0;
      Widgets_FloatRange._draggingHandle = Widgets_FloatRange.Handle.None;
      Event.current.Use();
    }

    public enum Handle
    {
      None,
      Min,
      Max,
    }
  }
}
