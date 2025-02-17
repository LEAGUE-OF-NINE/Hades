using System;
using System.Collections.Generic;
using HarmonyLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using BepInEx;

namespace BaseMod
{
    internal class AtLessThanConditionalChangeSkill : MonoBehaviour
    {
        public static void Setup(Harmony harmony)
        {
            ClassInjector.RegisterTypeInIl2Cpp<AtLessThanConditionalChangeSkill>();
            harmony.PatchAll(typeof(AtLessThanConditionalChangeSkill));
        }

        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleUnitModel.OnStartTurn_BeforeLog))]
        [HarmonyPostfix]
        private static void StartTurn_BeforeLog(BattleActionModel action, BattleUnitModel __instance)
        {
            foreach (var ability in action._skill.GetSkillAbilityScript())
            {
                var scriptName = ability.scriptName;
                if (scriptName.Contains("AtLessThanValueChangeSkill_"))
                {
                    FrogMainClass.Logg.LogInfo("Successfully Detected: " + scriptName);
                    // warning: horrible painful bullshit
                    // code to take an input of what skill we change into.
                    // horrible implementation but I need to do it, else the game can't find this specific skill.
                    var newskillID = Convert.ToInt32(scriptName.Replace("AtLessThanValueChangeSkill_", ""));
                    var whae = delegate (SkillModel x) { return x.GetID() == newskillID; };
                    var naenae = __instance.GetSkillList().Find(whae);

                    var keyword_one = ability.buffData.buffKeyword;
                    var keyword_status = Enum.Parse<BUFF_UNIQUE_KEYWORD>(keyword_one);

                    var potency_check = ability.buffData.stack;
                    var count_check = ability.buffData.turn;
                    if (__instance.GetActivatedBuffStack(keyword_status) <= potency_check && __instance.GetActivatedBuffTurn(keyword_status) <= count_check)
                    {
                        action.ChangeSkill(naenae);
                    }
                };
            }
        }
    }
}
