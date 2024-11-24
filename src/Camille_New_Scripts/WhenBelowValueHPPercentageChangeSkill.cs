using System;
using System.Collections.Generic;
using HarmonyLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using BepInEx;

namespace BaseMod
{
    internal class WhenBelowValueHPPercentageChangeSkill : MonoBehaviour
    {
        public static void Setup(Harmony harmony)
        {
            ClassInjector.RegisterTypeInIl2Cpp<WhenBelowValueHPPercentageChangeSkill>();
            harmony.PatchAll(typeof(WhenBelowValueHPPercentageChangeSkill));
        }

        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleUnitModel.OnStartTurn_BeforeLog))]
        [HarmonyPostfix]
        private static void StartTurn_BeforeLog(BattleActionModel action, BattleUnitModel __instance)
        {
            foreach (var ability in action._skill.GetSkillAbilityScript())
            {
                var scriptName = ability.scriptName;
                if (scriptName.Contains("WhenBelowValueHPPercentageChangeSkill_"))
                {
                    // warning: horrible painful bullshit
                    // code to take an input of what skill becomes unclashable.
                    // horrible implementation but I need to do it, else the game can't find this specific skill.
                    var newskillID = Convert.ToInt32(scriptName.Replace("WhenBelowValueHPPercentageChangeSkill_", ""));
                    var whae = delegate (SkillModel x) { return x.GetID() == newskillID; };
                    var naenae = __instance.GetSkillList().Find(whae);

                    // at the input players need to specify the conditional they wan
                    var HP_percentage_needed = ability.buffData.value;

                    if (__instance.GetHpRatio() < HP_percentage_needed)
                    {
                        action.ChangeSkill(naenae);
                    }
                };
            }
        }
    }
}
