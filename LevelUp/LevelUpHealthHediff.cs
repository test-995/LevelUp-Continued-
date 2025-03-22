using System;
using System.Text;
using UnityEngine;
using Verse;

namespace LevelUp
{
	// Token: 0x02000008 RID: 8
	public class LevelUpHealthHediff : Hediff
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000A RID: 10 RVA: 0x00002800 File Offset: 0x00000A00
		public override string LabelInBrackets
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.LabelInBrackets);
				bool flag = this.pawn != null;
				bool flag2 = flag;
				if (flag2)
				{
					bool flag3 = this.pawn.health.hediffSet.GetFirstHediffOfDef(LevellingHediffDefOf.HealthLevel, false) != null;
					bool flag4 = flag3;
					if (flag4)
					{
						float severity = this.pawn.health.hediffSet.GetFirstHediffOfDef(LevellingHediffDefOf.HealthLevel, false).Severity;
						float f = severity - (float)Mathf.FloorToInt(severity);
						string value = "LT_Level".Translate() + " " + Mathf.FloorToInt(severity).ToString() + ": " + f.ToStringPercent();
						stringBuilder.Append(value);
						return stringBuilder.ToString();
					}
				}
				return stringBuilder.ToString();
			}
		}
	}
}
