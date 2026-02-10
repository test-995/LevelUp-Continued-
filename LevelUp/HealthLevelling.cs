using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace LevelUp
{
	// Token: 0x02000005 RID: 5
	public class HealthLevelling : ThingComp
	{
		// Token: 0x06000005 RID: 5 RVA: 0x000021F4 File Offset: 0x000003F4
		public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			Pawn pawn = this.parent as Pawn;
            bool flag = pawn != null;
            bool flag2 = flag;
			if (flag2)
			{
                float baseHealthScale = pawn.RaceProps.baseHealthScale;
				bool flag3 = pawn.health.hediffSet.HasHediff(LevellingHediffDefOf.HealthLevel, false);
				if (flag3)
				{
                    Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(LevellingHediffDefOf.HealthLevel, false);
                    bool is_open_player = this.settings.playerOnly;
					if (is_open_player) 
					{
						if(pawn.Faction == null || pawn.Faction != Faction.OfPlayer)
						{
                            pawn.health.RemoveHediff(firstHediffOfDef);
                            return;
                        }
					}
				}
				else
				{
                    bool is_open_player = this.settings.playerOnly;
					//给没有状态的pawn加入殖民地后添加状态
					if (pawn.Faction != null && pawn.Faction == Faction.OfPlayer)
					{
                        HediffMaker.MakeHediff(LevellingHediffDefOf.HealthLevel, pawn, null);
                        HealthUtility.AdjustSeverity(pawn, LevellingHediffDefOf.HealthLevel, 0.0001f);
                        return;
                    }
                    if (!is_open_player)
                    {
                        float num = Rand.Range(0f, this.settings.MaxRandomLevel);
                        if (pawn.Faction == null || pawn.Faction != Faction.OfPlayer)
                        {
                            HediffMaker.MakeHediff(LevellingHediffDefOf.HealthLevel, pawn, null);
                            HealthUtility.AdjustSeverity(pawn, LevellingHediffDefOf.HealthLevel, num / baseHealthScale);
                            return;
                        }
                    }
                }
				if (flag3)
				{
					float severity = pawn.health.hediffSet.GetFirstHediffOfDef(LevellingHediffDefOf.HealthLevel, false).Severity;
					int num2 = Mathf.FloorToInt(severity);
					float num3 = this.settings.LevelUpRate + 1f;
					float num4 = num3 * (float)num2;
					float num5 = Mathf.Pow(num3, (float)num2);
					totalDamageDealt = Mathf.Max(1f, totalDamageDealt);
					float num6 = this.settings.BaseXP * num5 * baseHealthScale;
					float num7 = this.settings.BaseXP * num4 * baseHealthScale;
					bool flag12 = num7 == 0f;
					bool flag13 = flag12;
					if (flag13)
					{
						num7 = this.settings.BaseXP * baseHealthScale;
					}
					bool flag14 = pawn.Faction != null;
					bool flag15 = flag14;
					if (flag15)
					{
						bool flag16 = this.settings.LevellingType == "Compound Levelling";
						bool flag17 = flag16;
						if (flag17)
						{
							bool flag18 = severity - (float)num2 + totalDamageDealt / num6 >= 1f && pawn.Faction.IsPlayer;
							bool flag19 = flag18;
							if (flag19)
							{
								Messages.Message(pawn.Name.ToStringShort + "LT_LevelUp".Translate(), pawn, MessageTypeDefOf.SilentInput, true);
								LevellingSoundDefOf.Level_Up.PlayOneShotOnCamera(null);
							}
						}
						else
						{
							bool flag20 = this.settings.LevellingType == "Simple Levelling";
							bool flag21 = flag20;
							if (flag21)
							{
								bool flag22 = severity - (float)num2 + totalDamageDealt / num7 >= 1f && pawn.Faction.IsPlayer;
								bool flag23 = flag22;
								if (flag23)
								{
									Messages.Message(pawn.Name.ToStringShort + "LT_LevelUp".Translate(), pawn, MessageTypeDefOf.SilentInput, true);
									LevellingSoundDefOf.Level_Up.PlayOneShotOnCamera(null);
								}
							}
						}
					}
					bool flag24 = this.settings.LevellingType == "Compound Levelling";
					bool flag25 = flag24;
					if (flag25)
					{
						this.ApplySeverityWithUnlockNotice(pawn, severity, totalDamageDealt / num6);
					}
					else
					{
						bool flag26 = this.settings.LevellingType == "Simple Levelling";
						bool flag27 = flag26;
						if (flag27)
						{
							this.ApplySeverityWithUnlockNotice(pawn, severity, totalDamageDealt / num7);
						}
					}
				}
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000025EC File Offset: 0x000007EC
		public override void PostSpawnSetup(bool respawnAfterLoad)
		{
			base.PostSpawnSetup(respawnAfterLoad);
			bool flag = this.parent != null;
			bool flag2 = flag;
			if (flag2)
			{
				Pawn pawn = this.parent as Pawn;
				bool flag3 = pawn != null;
				if (flag3)
				{
					float baseHealthScale = pawn.RaceProps.baseHealthScale;
					float num = Rand.Range(0f, this.settings.MaxRandomLevel);
					bool flag5 = pawn.health.hediffSet.HasHediff(LevellingHediffDefOf.HealthLevel, false);
					bool is_open_player = this.settings.playerOnly;
					if (flag5)
					{
						return;
					}
                    if (pawn.Faction != null)
                    {
                        if (pawn.Faction == Faction.OfPlayer || pawn.kindDef.defaultFactionType.isPlayer)
                        {
                            HediffMaker.MakeHediff(LevellingHediffDefOf.HealthLevel, pawn, null);
                            HealthUtility.AdjustSeverity(pawn, LevellingHediffDefOf.HealthLevel, 0.0001f);
                            return;
                        }
                    }
                    if (!is_open_player)
					{
                        HediffMaker.MakeHediff(LevellingHediffDefOf.HealthLevel, pawn, null);
                        HealthUtility.AdjustSeverity(pawn, LevellingHediffDefOf.HealthLevel, num / baseHealthScale);
                        return;
                    }
					else
					{
						return;
                    }
				}
			}
		}


		private void NotifyTalentUnlocks(Pawn pawn, int oldLevel, int newLevel)
		{
			if (!this.settings.enableTalentNodes || pawn == null || pawn.Faction == null || !pawn.Faction.IsPlayer)
			{
				return;
			}

			int maxLevel = Mathf.Max(1, Mathf.FloorToInt(this.settings.MaxLevelCap));
			int step = Mathf.Max(1, Mathf.FloorToInt((float)maxLevel / 4f));
			for (int i = 1; i <= 4; i++)
			{
				int threshold = step * i;
				if (oldLevel < threshold && newLevel >= threshold)
				{
					Messages.Message(pawn.Name.ToStringShort + " unlocked Talent " + i.ToString() + " at Lv." + threshold.ToString(), pawn, MessageTypeDefOf.PositiveEvent, true);
				}
			}
		}

		private void ApplySeverityWithUnlockNotice(Pawn pawn, float currentSeverity, float severityDelta)
		{
			int oldLevel = Mathf.FloorToInt(currentSeverity);
			float targetSeverity = currentSeverity + severityDelta;
			if (this.settings.enableLevelCap)
			{
				float levelCap = Mathf.Clamp(this.settings.MaxLevelCap, 100f, 500f);
				targetSeverity = Mathf.Min(targetSeverity, levelCap);
			}

			float appliedDelta = targetSeverity - currentSeverity;
			if (appliedDelta <= 0f)
			{
				return;
			}

			int newLevel = Mathf.FloorToInt(targetSeverity);
			HealthUtility.AdjustSeverity(pawn, LevellingHediffDefOf.HealthLevel, appliedDelta);
			this.NotifyTalentUnlocks(pawn, oldLevel, newLevel);
		}


		// Token: 0x04000002 RID: 2
		private LevelUpModSettings settings = LoadedModManager.GetMod<LevelUpMod>().GetSettings<LevelUpModSettings>();
	}
}
