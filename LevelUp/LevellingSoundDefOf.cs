using System;
using RimWorld;
using Verse;

namespace LevelUp
{
	// Token: 0x02000007 RID: 7
	[DefOf]
	public static class LevellingSoundDefOf
	{
		// Token: 0x06000009 RID: 9 RVA: 0x000027ED File Offset: 0x000009ED
		static LevellingSoundDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(LevellingSoundDefOf));
		}

		// Token: 0x04000004 RID: 4
		public static SoundDef Level_Up;
	}
}
