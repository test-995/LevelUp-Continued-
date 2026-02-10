using System;
using Verse;

namespace LevelUp
{
	// Token: 0x0200000A RID: 10
	public class LevelUpModSettings : ModSettings
	{
		// Token: 0x06000010 RID: 16 RVA: 0x00002B6C File Offset: 0x00000D6C
		public override void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.LevelUpRate, "LevellingRate", 0.075f, false);
			Scribe_Values.Look<float>(ref this.LevelUpHealthRate, "HealthPerLevelUp", 0.1f, false);
			Scribe_Values.Look<float>(ref this.MaxRandomLevel, "MaxRandomLevel", 10f, false);
			Scribe_Values.Look<float>(ref this.BaseXP, "BaseXPForLevelling", 75f, false);
			Scribe_Values.Look<string>(ref this.LevellingType, "LevellingMode", "Compound Levelling", false);
			Scribe_Values.Look<string>(ref this.HealthScalingType, "HealthScalingMode", "Compounding Health", false);
			Scribe_Values.Look<bool>(ref this.playerOnly, "PlayerOnly", true, false);
			Scribe_Values.Look<bool>(ref this.enableParallelGrowth, "EnableParallelGrowth", true, false);
			Scribe_Values.Look<bool>(ref this.enableTalentNodes, "EnableTalentNodes", true, false);
			Scribe_Values.Look<float>(ref this.NonHealthGrowthRate, "NonHealthGrowthRate", 0.0125f, false);
			Scribe_Values.Look<float>(ref this.DefenseGrowthRate, "DefenseGrowthRate", 0.005f, false);
			Scribe_Values.Look<bool>(ref this.enableLevelCap, "EnableLevelCap", false, false);
			Scribe_Values.Look<float>(ref this.MaxLevelCap, "MaxLevelCap", 200f, false);
			base.ExposeData();
		}

		// Token: 0x04000008 RID: 8
		public float LevelUpRate = 0.075f;

		// Token: 0x04000009 RID: 9
		public float LevelUpHealthRate = 0.1f;

		// Token: 0x0400000A RID: 10
		public float MaxRandomLevel = 10f;

		// Token: 0x0400000B RID: 11
		public float BaseXP = 75f;

		// Token: 0x0400000C RID: 12
		public const string defaultLevellingMode = "Compound Levelling";

		// Token: 0x0400000D RID: 13
		public const string defaultHealthScalingMode = "Compounding Health";

		// Token: 0x0400000E RID: 14
		public string LevellingType = "Compound Levelling";

		// Token: 0x0400000F RID: 15
		public string HealthScalingType = "Compounding Health";

		// Token: 0x04000010 RID: 16
		public bool playerOnly = true;

		public bool enableParallelGrowth = true;

		public bool enableTalentNodes = true;

		public float NonHealthGrowthRate = 0.0125f;

		public float DefenseGrowthRate = 0.005f;

		public bool enableLevelCap = false;

		public float MaxLevelCap = 200f;
	}
}
