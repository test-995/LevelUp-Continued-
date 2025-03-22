using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace LevelUp
{
	// Token: 0x0200000B RID: 11
	internal static class SettingsHelper
	{
		// Token: 0x06000012 RID: 18 RVA: 0x00002C80 File Offset: 0x00000E80
		public static Rect GetRect(this Listing_Standard listing_Standard, float? height = null)
		{
			return listing_Standard.GetRect(height ?? Text.LineHeight, 1f);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002CB8 File Offset: 0x00000EB8
		public static void AddLabeledRadioList(this Listing_Standard listing_Standard, string header, string[] labels, ref string val, float? headerHeight = null)
		{
			bool flag = header != string.Empty;
			bool flag2 = flag;
			if (flag2)
			{
				Widgets.Label(listing_Standard.GetRect(headerHeight), header);
			}
			listing_Standard.AddRadioList(SettingsHelper.GenerateLabeledRadioValues(labels), ref val, null);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002D00 File Offset: 0x00000F00
		private static void AddRadioList<T>(this Listing_Standard listing_Standard, List<SettingsHelper.LabeledRadioValue<T>> items, ref T val, float? height = null)
		{
			foreach (SettingsHelper.LabeledRadioValue<T> labeledRadioValue in items)
			{
				Rect rect = listing_Standard.GetRect(height);
				bool flag = Widgets.RadioButtonLabeled(rect, labeledRadioValue.Label, EqualityComparer<T>.Default.Equals(labeledRadioValue.Value, val), false);
				bool flag2 = flag;
				if (flag2)
				{
					val = labeledRadioValue.Value;
				}
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002D90 File Offset: 0x00000F90
		private static List<SettingsHelper.LabeledRadioValue<string>> GenerateLabeledRadioValues(string[] labels)
		{
			List<SettingsHelper.LabeledRadioValue<string>> list = new List<SettingsHelper.LabeledRadioValue<string>>();
			foreach (string text in labels)
			{
				list.Add(new SettingsHelper.LabeledRadioValue<string>(text, text));
			}
			return list;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002DD0 File Offset: 0x00000FD0
		public static void AddLabeledNumericalTextField<T>(this Listing_Standard listing_Standard, string label, ref T settingsValue, float leftPartPct = 0.5f, float minValue = 1f, float maxValue = 100000f) where T : struct
		{
			listing_Standard.Gap(12f);
			Rect rect;
			Rect rect2;
			listing_Standard.LineRectSpilter(out rect, out rect2, leftPartPct, null);
			Widgets.Label(rect, label);
			string text = settingsValue.ToString();
			Widgets.TextFieldNumeric<T>(rect2, ref settingsValue, ref text, minValue, maxValue);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002E24 File Offset: 0x00001024
		public static Rect LineRectSpilter(this Listing_Standard listing_Standard, out Rect leftHalf, out Rect rightHalf, float leftPartPct = 0.5f, float? height = null)
		{
			Rect rect = listing_Standard.LineRectSpilter(out leftHalf, leftPartPct, height);
			rightHalf = rect.RightPart(1f - leftPartPct).Rounded();
			return rect;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002E5C File Offset: 0x0000105C
		public static Rect LineRectSpilter(this Listing_Standard listing_Standard, out Rect leftHalf, float leftPartPct = 0.5f, float? height = null)
		{
			Rect rect = listing_Standard.GetRect(height);
			leftHalf = rect.LeftPart(leftPartPct).Rounded();
			return rect;
		}

		// Token: 0x0200000C RID: 12
		public class LabeledRadioValue<T>
		{
			// Token: 0x06000019 RID: 25 RVA: 0x00002E89 File Offset: 0x00001089
			public LabeledRadioValue(string label, T val)
			{
				this.Label = label;
				this.Value = val;
			}

			// Token: 0x17000002 RID: 2
			// (get) Token: 0x0600001A RID: 26 RVA: 0x00002EA4 File Offset: 0x000010A4
			// (set) Token: 0x0600001B RID: 27 RVA: 0x00002EBC File Offset: 0x000010BC
			public string Label
			{
				get
				{
					return this.label;
				}
				set
				{
					this.label = value;
				}
			}

			// Token: 0x17000003 RID: 3
			// (get) Token: 0x0600001C RID: 28 RVA: 0x00002EC8 File Offset: 0x000010C8
			// (set) Token: 0x0600001D RID: 29 RVA: 0x00002EE0 File Offset: 0x000010E0
			public T Value
			{
				get
				{
					return this.val;
				}
				set
				{
					this.val = value;
				}
			}

			// Token: 0x04000011 RID: 17
			private string label;

			// Token: 0x04000012 RID: 18
			private T val;
		}
	}
}
