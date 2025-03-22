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
