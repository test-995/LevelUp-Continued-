using System;
using UnityEngine;
using Verse;

namespace LevelUp
{
	// Token: 0x02000009 RID: 9
	public class LevelUpMod : Mod
	{
		// Token: 0x0600000C RID: 12 RVA: 0x000028FC File Offset: 0x00000AFC
		public LevelUpMod(ModContentPack content) : base(content)
		{
			this.settings = base.GetSettings<LevelUpModSettings>();
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002914 File Offset: 0x00000B14
		public override string SettingsCategory()
		{
			return "LT_ModName".Translate();
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002938 File Offset: 0x00000B38
		public override void DoSettingsWindowContents(Rect inRect)
		{
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.Begin(inRect);
			listing_Standard.CheckboxLabeled("LT_PlayerOnly".Translate(), ref this.settings.playerOnly, null, 0f, 1f);
			listing_Standard.CheckboxLabeled("Enable Parallel Growth", ref this.settings.enableParallelGrowth, "Scale non-health combat/survival stats with level.", 0f, 1f);
			listing_Standard.CheckboxLabeled("Enable Talent Nodes", ref this.settings.enableTalentNodes, "Unlock roguelike talent breakpoints by level cap quartiles.", 0f, 1f);
			listing_Standard.CheckboxLabeled("Enable Level Cap", ref this.settings.enableLevelCap, "Enable hard level cap for this mod.", 0f, 1f);
			listing_Standard.GapLine(3f);
			listing_Standard.AddLabeledRadioList("LT_LevellingMode".Translate(), LevelUpMod.LevellingMode, ref this.settings.LevellingType, null);
			listing_Standard.GapLine(3f);
			listing_Standard.AddLabeledRadioList("LT_HealthScalingMode".Translate(), LevelUpMod.HealthScalingMode, ref this.settings.HealthScalingType, null);
			listing_Standard.GapLine(3f);
			listing_Standard.Label("LT_LevellingRate".Translate() + ("(" + this.settings.LevelUpRate.ToStringPercent() + ")"), -1f, null);
			this.settings.LevelUpRate = listing_Standard.Slider(this.settings.LevelUpRate, 0.001f, 1f);
			listing_Standard.Label("LT_HealthPerLevelRate".Translate() + ("(" + this.settings.LevelUpHealthRate.ToStringPercent() + ")"), -1f, null);
			this.settings.LevelUpHealthRate = listing_Standard.Slider(this.settings.LevelUpHealthRate, 0.001f, 1f);
			listing_Standard.Label("Non-Health Growth / Level" + ("(" + this.settings.NonHealthGrowthRate.ToStringPercent() + ")"), -1f, null);
			this.settings.NonHealthGrowthRate = listing_Standard.Slider(this.settings.NonHealthGrowthRate, 0.001f, 0.2f);
			listing_Standard.Label("Defense Growth / Level" + ("(" + this.settings.DefenseGrowthRate.ToStringPercent() + ")"), -1f, null);
			this.settings.DefenseGrowthRate = listing_Standard.Slider(this.settings.DefenseGrowthRate, 0.001f, 0.1f);
			listing_Standard.Label("Max Level Cap" + ("(" + Mathf.FloorToInt(this.settings.MaxLevelCap).ToString() + ")"), -1f, null);
			this.settings.MaxLevelCap = listing_Standard.Slider(this.settings.MaxLevelCap, 100f, 500f);
			listing_Standard.AddLabeledNumericalTextField("LT_MaximumRandomLevel".Translate(), ref this.settings.MaxRandomLevel, 0.5f, 1f, 1000f);
			listing_Standard.AddLabeledNumericalTextField("LT_BaseXPRequired".Translate(), ref this.settings.BaseXP, 0.5f, 1f, 100000f);
			listing_Standard.End();
			base.DoSettingsWindowContents(inRect);
		}

		// Token: 0x04000005 RID: 5
		public static string[] LevellingMode = new string[]
		{
			"Compound Levelling",
			"Simple Levelling"
		};

		// Token: 0x04000006 RID: 6
		public static string[] HealthScalingMode = new string[]
		{
			"Compounding Health",
			"Simple Health"
		};

		// Token: 0x04000007 RID: 7
		private LevelUpModSettings settings;
	}
}
