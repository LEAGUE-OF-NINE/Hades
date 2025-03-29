using System;
using System.Collections.Generic;
using HarmonyLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using BepInEx;

namespace BaseMod
{
    internal class ChangeDefenseOnConditional : MonoBehaviour
    {
        public static void Setup(Harmony harmony)
        {
            ClassInjector.RegisterTypeInIl2Cpp<ChangeDefenseOnConditional>();
            harmony.PatchAll(typeof(ChangeDefenseOnConditional));
        }

        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleUnitModel.OnBeforeDefense))]
        [HarmonyPostfix]
        private static void OnBeforeDefense(BattleActionModel action, BattleUnitModel __instance)
        {
            foreach (var ability in action._skill.GetSkillAbilityScript())
            {
                var scriptName = ability.scriptName;
                if (scriptName.Contains("ChangeDefenseOnConditional_"))
                {
                    FrogMainClass.Logg.LogInfo("Successfully Detected: " + scriptName);
                    var newskillID = Convert.ToInt32(scriptName.Replace("ChangeDefenseOnConditional_", ""));
                    var whae = delegate (SkillModel x) { return x.GetID() == newskillID; };
                    var naenae = __instance.GetSkillList().Find(whae);

                    var keyword_one = ability.buffData.buffKeyword;
                    var keyword_status = Enum.Parse<BUFF_UNIQUE_KEYWORD>(keyword_one);

                    var potency_check = ability.buffData.stack;
                    var count_check = ability.buffData.turn;
                    if (__instance.GetActivatedBuffStack(keyword_status) >= potency_check && __instance.GetActivatedBuffTurn(keyword_status) >= count_check)
                    {
                        action.ChangeSkill(naenae);
                    }
                };
            }
        }
    }
}
