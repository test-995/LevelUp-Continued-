using System;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace LevelUp
{


	[StaticConstructorOnStartup]
	internal static class HarmonyPatches
	{

		static HarmonyPatches()
		{
			Harmony harmony = new Harmony("Jdalt.RimWorld.LevelUp.Main");
			harmony.Patch(AccessTools.Property(typeof(Pawn), "HealthScale").GetGetMethod(), null, new HarmonyMethod(typeof(HarmonyPatches), "LevelUpHealthScale", null), null, null);
            harmony.Patch(
            AccessTools.Method(typeof(BodyPartDef), "GetMaxHealth"),
            new HarmonyMethod(typeof(HarmonyPatches), "GetMaxHealth_Postfix"), // 前置方法（Prefix），这里不修改
            null // 后置方法（Postfix）
			);
            harmony.Patch(
            AccessTools.Method(typeof(HediffSet), "GetPartHealth"),
            new HarmonyMethod(typeof(HarmonyPatches), "GetPartHealth_Postfix"), // 前置方法（Prefix），这里不修改
            null // 后置方法（Postfix）
			);
            harmony.Patch(
            AccessTools.Method(typeof(StatExtension), "GetStatValue", new Type[]
            {
                typeof(Thing),
                typeof(StatDef),
                typeof(bool)
            }),
            null,
            new HarmonyMethod(typeof(HarmonyPatches), "GetStatValue_Postfix"),
            null,
            null
            );
        }
        public static bool GetPartHealth_Postfix(HediffSet __instance, BodyPartRecord part, ref float __result)
		{
            if (part == null)
            {
                __result = 0f;
                return false; // 跳过原函数
            }

            float num = part.def.GetMaxHealth(__instance.pawn);
            foreach (Hediff hediff in __instance.hediffs)
            {
                if (hediff is Hediff_MissingPart && hediff.Part == part)
                {
                    __result = 0f;
                    return false;
                }

                if (hediff.Part == part && hediff is Hediff_Injury hediff_Injury)
                {
                    num = ((!hediff_Injury.destroysBodyParts) ? Mathf.Max(num - hediff_Injury.Severity, 1f) : (num - hediff_Injury.Severity));
                }
            }

            num = Mathf.Max(num, 0f);
            if (!part.def.destroyableByDamage)
            {
                num = Mathf.Max(num, 1f);
            }

            __result = num; // 不转换为整数
            return false; // 拦截原函数
		}
        public static bool GetMaxHealth_Postfix(BodyPartDef __instance, Pawn pawn, ref float __result)
        {
            // 让血量不受 Mathf.CeilToInt 影响，保持 float
            __result = (float)__instance.hitPoints * pawn.HealthScale;
			return false;
        }


        private static int GetPawnLevel(Pawn pawn)
        {
            if (pawn == null || pawn.health == null || pawn.health.hediffSet == null)
            {
                return 0;
            }
            Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(LevellingHediffDefOf.HealthLevel, false);
            if (hediff == null)
            {
                return 0;
            }
            return Mathf.Max(0, Mathf.FloorToInt(hediff.Severity));
        }

        private static int GetEffectiveLevel(LevelUpModSettings settings, int level)
        {
            if (!settings.enableLevelCap)
            {
                return level;
            }
            int cap = Mathf.Clamp(Mathf.FloorToInt(settings.MaxLevelCap), 100, 500);
            return Mathf.Min(level, cap);
        }

        private static int GetTalentTier(LevelUpModSettings settings, int level)
        {
            int cap = Mathf.Clamp(Mathf.FloorToInt(settings.MaxLevelCap), 100, 500);
            int step = Mathf.Max(1, Mathf.FloorToInt((float)cap / 4f));
            int tier = 0;
            for (int i = 1; i <= 4; i++)
            {
                if (level >= step * i)
                {
                    tier = i;
                }
            }
            return tier;
        }

        private static float GetTalentMultiplier(LevelUpModSettings settings, int level, string statDefName)
        {
            if (!settings.enableTalentNodes)
            {
                return 1f;
            }

            int tier = GetTalentTier(settings, level);
            float mult = 1f;

            if (tier >= 1)
            {
                if (statDefName == "MoveSpeed" || statDefName == "BleedRate")
                {
                    mult *= (statDefName == "MoveSpeed") ? 1.06f : 0.93f;
                }
            }
            if (tier >= 2)
            {
                if (statDefName == "MeleeHitChance" || statDefName == "ShootingAccuracyPawn")
                {
                    mult *= 1.08f;
                }
            }
            if (tier >= 3)
            {
                if (statDefName == "IncomingDamageFactor")
                {
                    mult *= 0.90f;
                }
            }
            if (tier >= 4)
            {
                if (statDefName == "CarryingCapacity" || statDefName == "WorkSpeedGlobal")
                {
                    mult *= 1.10f;
                }
            }

            return mult;
        }

        public static void GetStatValue_Postfix(Thing thing, StatDef stat, bool applyPostProcess, ref float __result)
        {
            Pawn pawn = thing as Pawn;
            if (pawn == null || stat == null)
            {
                return;
            }

            LevelUpModSettings settings = LoadedModManager.GetMod<LevelUpMod>().GetSettings<LevelUpModSettings>();
            if (!settings.enableParallelGrowth)
            {
                return;
            }

            int level = GetEffectiveLevel(settings, GetPawnLevel(pawn));
            if (level <= 0)
            {
                return;
            }

            string defName = stat.defName;
            float growth = settings.NonHealthGrowthRate * (float)level;

            if (defName == "MoveSpeed")
            {
                __result *= 1f + growth * 0.5f;
            }
            else if (defName == "WorkSpeedGlobal")
            {
                __result *= 1f + growth;
            }
            else if (defName == "ShootingAccuracyPawn")
            {
                __result *= 1f + growth * 0.75f;
            }
            else if (defName == "MeleeHitChance")
            {
                __result *= 1f + growth * 0.85f;
            }
            else if (defName == "IncomingDamageFactor")
            {
                __result *= Mathf.Max(0.2f, 1f - settings.DefenseGrowthRate * (float)level);
            }

            __result *= GetTalentMultiplier(settings, level, defName);
        }



        public static void LevelUpHealthScale(Pawn __instance, ref float __result)
		{

            

            LevelUpModSettings settings = LoadedModManager.GetMod<LevelUpMod>().GetSettings<LevelUpModSettings>();
			bool flag = __instance.health.hediffSet.HasHediff(LevellingHediffDefOf.HealthLevel, false);
			bool flag2 = flag;
			if (flag2)
			{
				Hediff firstHediffOfDef = __instance.health.hediffSet.GetFirstHediffOfDef(LevellingHediffDefOf.HealthLevel, false);
				bool flag3 = firstHediffOfDef.Severity < 0f;
				bool flag4 = flag3;
				if (flag4)
				{
					float num = Rand.Range(0f, settings.MaxRandomLevel);
					float baseHealthScale = __instance.RaceProps.baseHealthScale;
					firstHediffOfDef.Severity = num / baseHealthScale;
					Log.Message("LT_LevelNegative".Translate());
				}
				int num2 = Mathf.FloorToInt(firstHediffOfDef.Severity);
				if (settings.enableLevelCap)
				{
					num2 = Mathf.Min(num2, Mathf.Clamp(Mathf.FloorToInt(settings.MaxLevelCap), 100, 500));
				}
				float num3 = (float)Math.Round((double)settings.LevelUpHealthRate, 2);
				float f = num3 + 1f;
				bool flag5 = settings.HealthScalingType == "Compounding Health";
				bool flag6 = flag5;
				if (flag6)
				{
					__result *= 1f * Mathf.Pow(f, (float)num2);
				}
				else
				{
					bool flag7 = settings.HealthScalingType == "Simple Health";
					bool flag8 = flag7;
					if (flag8)
					{
						__result *= 1f + num3 * (float)num2;
					}
				}
			}
		}
	}
}
