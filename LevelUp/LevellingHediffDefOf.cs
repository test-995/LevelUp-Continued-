using System;
using RimWorld;
using Verse;

namespace LevelUp
{
	// Token: 0x02000006 RID: 6
	[DefOf]
	public static class LevellingHediffDefOf
	{
		// Token: 0x06000008 RID: 8 RVA: 0x000027DA File Offset: 0x000009DA
		static LevellingHediffDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(LevellingHediffDefOf));
		}

		// Token: 0x04000003 RID: 3
		public static HediffDef HealthLevel;
	}
}
